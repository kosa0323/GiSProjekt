using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorytmMrówkowy
{
    struct GranicePrzedziału
    {
        public double PoczątekPrzedziału { get; set; }
        public double KoniecPrzedziału { get; set; }
        public GranicePrzedziału(double początekPrzedziału, double koniecPrzedziału)
        {
            PoczątekPrzedziału = początekPrzedziału;
            KoniecPrzedziału = koniecPrzedziału;
        }
    }
    class Generator
    {
        public static double[,] GenerujGraf(int liczbaWierchołków, int parametrM, double[,] grafPoczątkowy, double najmniejszaDopuszczalnaWaga, double największaDopuszczalnaWaga)
        {
            double[,] graf = new double[liczbaWierchołków, liczbaWierchołków];
            if (parametrM > grafPoczątkowy.GetLength(0))//parametr m określa z jaką liczbą wiechołków będziemy łączyć nowy wierchołek
            {
                Exception zaDużaWartośćM;
                zaDużaWartośćM = new Exception("Wartość parametru m nie może być większa niż liczba wierzchołków w grafie początkowym");
                throw zaDużaWartośćM;
            }
            for (int i = 0; i < grafPoczątkowy.GetLength(0); i++)//przepisywanie grafu początkowego do grafu końcowego
            {
                for (int j = 0; j < grafPoczątkowy.GetLength(0); j++)
                {
                    graf[i, j] = grafPoczątkowy[i, j];
                }
            }
            int liczbaIteracji = liczbaWierchołków - grafPoczątkowy.GetLength(0);
            int aktualnyWierzchołek = 0;
            for (int i = 0; i < liczbaIteracji; i++)
            {
                aktualnyWierzchołek = grafPoczątkowy.GetLength(0) + i;
                double sumaStopniWierchołków = 0;
                double początekPrzedziału = 0;
                double koniecPrzedziału = 0;
                double[] rozkładStopniWierchołków = new double[liczbaWierchołków];
                GranicePrzedziału[] granicePrzedziałów = new GranicePrzedziału[liczbaWierchołków];
                for (int j = 0; j < liczbaWierchołków; j++)//obliczanie rozkładu stopni wierzchołków dla danej iteracji
                {
                    for (int k = 0; k < liczbaWierchołków; k++)
                    {
                        if (graf[j, k] != 0)
                        {
                            rozkładStopniWierchołków[j]++;
                        }
                    }
                    sumaStopniWierchołków += rozkładStopniWierchołków[j];
                }
                for (int l = 0; l < liczbaWierchołków; l++)//przypisanie każdemu wiechołkowi odpowiendiego odcinka z przedziału [0,1)
                {
                    koniecPrzedziału += (rozkładStopniWierchołków[l] / sumaStopniWierchołków);
                    granicePrzedziałów[l] = new GranicePrzedziału(początekPrzedziału, koniecPrzedziału);
                    początekPrzedziału = koniecPrzedziału;
                }
                double liczbaLosowa = 0;
                double wagaŁącza = 0;
                List<int> listaTabu = new List<int>();
                for (int m = 0; m < parametrM; m++)//dodanie krawędzi pomiędzy aktualnym wierzchołkiem a obecnym grafem
                {
                    bool zawieraWierzchołek = false;
                    do
                    {
                        Random r = new Random(Guid.NewGuid().GetHashCode());
                        liczbaLosowa = r.NextDouble();
                        for (int n = 0; n < granicePrzedziałów.Length; n++)
                        {
                            if (liczbaLosowa >= granicePrzedziałów[n].PoczątekPrzedziału && liczbaLosowa < granicePrzedziałów[n].KoniecPrzedziału)
                            {
                                wagaŁącza = (największaDopuszczalnaWaga - najmniejszaDopuszczalnaWaga) * r.NextDouble() + najmniejszaDopuszczalnaWaga;
                                if (!listaTabu.Contains(n))
                                {
                                    graf[n, aktualnyWierzchołek] = wagaŁącza;
                                    graf[aktualnyWierzchołek, n] = wagaŁącza;
                                    listaTabu.Add(n);
                                    zawieraWierzchołek = false;
                                }
                                else
                                {
                                    zawieraWierzchołek = true;
                                }
                                break;
                            }
                        }
                    } while (zawieraWierzchołek);
                }
            }
            return graf;
        }
    }
}
