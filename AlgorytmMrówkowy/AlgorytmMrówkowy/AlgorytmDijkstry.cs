using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorytmMrówkowy
{
    class AlgorytmDijkstry
    {
        public static double wykonajAlgorytm(double[,] graf, int wierzchołekPoczątkowy, int wierzchołekKońcowy)
        {
            Stóg<Koszt> kolejkaPriorytetowa = new Stóg<Koszt>();
            bool[] jestOznaczony = new bool[graf.GetLength(0)];
            double[] tablicaOdległośći = new double[graf.GetLength(0)];
            int[] tablicePoprzedników = new int[graf.GetLength(0)];
            for (int i = 0; i < graf.GetLength(0); i++)
            {
                tablicaOdległośći[i] = double.PositiveInfinity;
                jestOznaczony[i] = false;
            }
            tablicaOdległośći[wierzchołekPoczątkowy] = 0;
            for (int i = 0; i < graf.GetLength(0); i++)
            {
                kolejkaPriorytetowa.DodajElement(new Koszt(i, tablicaOdległośći[i]));
            }
            while (jestOznaczony.Contains(false))
            {
                Koszt aktualnyWierzchołek = new Koszt();
                aktualnyWierzchołek = kolejkaPriorytetowa.ZwrocElement(0);
                kolejkaPriorytetowa.UsunElement();
                jestOznaczony[aktualnyWierzchołek.idWierzchołka] = true;
                for (int i = 0; i < graf.GetLength(0); i++)
                {
                    if (graf[aktualnyWierzchołek.idWierzchołka, i] != 0)
                    {
                        // Dla każdego sąsiada v /*wierzchołka u dokonaj relaksacji poprzez u: jeśli d[u] +w(u, v) < d[v](poprzez u da się dojść do v szybciej niż dotychczasową ścieżką), to d[v] := d[u] + w(u, v).*/
                        if (tablicaOdległośći[aktualnyWierzchołek.idWierzchołka] + graf[aktualnyWierzchołek.idWierzchołka, i] < tablicaOdległośći[i])
                        {
                            tablicaOdległośći[i] = tablicaOdległośći[aktualnyWierzchołek.idWierzchołka] + graf[aktualnyWierzchołek.idWierzchołka, i];
                            tablicePoprzedników[i] = aktualnyWierzchołek.idWierzchołka;
                            kolejkaPriorytetowa.DodajElement(new Koszt(aktualnyWierzchołek.idWierzchołka, aktualnyWierzchołek.odległośćOdWierzchołkaŹródłowego));
                        }
                    }
                }
            }
            List<int> wierzchołkiNaTrasie = new List<int>();//odtwarzanie trasy
            if (tablicaOdległośći[wierzchołekKońcowy] != double.PositiveInfinity)
            {
                int rozważanyWierzchołek = wierzchołekKońcowy;
                while (tablicePoprzedników[rozważanyWierzchołek] != wierzchołekPoczątkowy)
                {
                    wierzchołkiNaTrasie.Add(tablicePoprzedników[rozważanyWierzchołek]);
                    rozważanyWierzchołek = tablicePoprzedników[rozważanyWierzchołek];
                }
             
            }
            return tablicaOdległośći[wierzchołekKońcowy];

        }
    }
}