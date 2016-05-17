using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorytmMrówkowy
{
    struct Info
    {
        public double stezenieFeromonu;
        public double wagaLacza;
        public Info(double stezenie)
        {
            this.stezenieFeromonu = stezenie;
            wagaLacza = 0;
        }

        public Info(double stezenie, double waga)
        {
            this.stezenieFeromonu = stezenie;
            this.wagaLacza = waga;
        }
    }
    class PrawdPlusWezel
    {
        public double prawdopodobienstwo;
        public int nrWezla;
    }
    static class AlgorytmMrówkowy
    {
        /// <summary>
        /// Funkcja wywołująca realizująca algorytm mrówkowy(Max-Min).
        /// </summary>
        /// <param name="grafWejsciowy">Graf Wejściowy</param>
        /// <param name="alfa">Współczynnik określający znaczenie stężenia feromonów.</param>
        /// <param name="beta">Współczynnik sterujący atrakcyjnością łącza.</param>
        /// <param name="n">Liczba mrówek</param>
        /// <param name="ro">Współczynnik parowania.</param>
        /// <param name="pDaszkiem">Prawdopodobieństwo uzyskania najlepszego wyniku.</param>
        /// <param name="tałMin">Minimalny poziom stężenia feromonu.</param>
        /// <param name="tałMax">Maksymalny poziom stężęnia feromonu.</param>
        /// <param name="fLambda">Częstotliwość aktualizacji stężenia feromonów globalną ścieżką.</param>
        /// <param name="wezelStartowy">Węzeł początkowy.</param>
        /// <param name="wezelKoncowy">Węzeł końcowy.</param>
        /// <param name="liczbaIteracji">Liczba iteracji - warunek stopu.</param>
        static public void WykonajAlgorytm( double[,] grafWejsciowy, 
                                            double alfa, 
                                            double beta,
                                            double n,
                                            double ro,
                                            double pDaszkiem,
                                            double tałMin,
                                            double tałMax,
                                            int fLambda,
                                            int wezelStartowy,
                                            int wezelKoncowy,
                                            int liczbaIteracji)
        {
            int wymiarMacierzyDol = grafWejsciowy.GetLength(1);
            int wymiarMacierzyBok = grafWejsciowy.GetLength(2);
            //Tablica przechowująca informacje o grafie
            Info[,] infoSiec = new Info[wymiarMacierzyDol, wymiarMacierzyBok];


            for (int i = 0; i < grafWejsciowy.GetLength(1); i++)
                for (int j = 0; j < grafWejsciowy.GetLength(2); j++)
                {
                    infoSiec[i, j].stezenieFeromonu = tałMax;
                    infoSiec[i, j].wagaLacza = grafWejsciowy[i, j];
                }

            //Zbior sciezek w danej iteracji
            Stog zbiorSciezek;
            Sciezka najlepszaGlobalnaSciezka;
            int licznik = 0;
            int wezel;
            Sciezka tmp;
            List<int> tabuList;
            List<PrawdPlusWezel> zbiorSasiadow;
            do
            {

                zbiorSciezek = new Stog();
                tabuList = new List<int>();
                zbiorSasiadow = new List<PrawdPlusWezel>();
                Random rr = new Random();
                double r = 0, sumaR = 0;
                for (int i = 0; i < n; i++)
                {
                    wezel = wezelStartowy;
                    tmp = new Sciezka();

                    for(int j = 0; j < wymiarMacierzyBok; j++)
                    {
                        if(infoSiec[wezel, j].wagaLacza != 0 && !tabuList.Contains(j))
                        {
                            r = Math.Pow(infoSiec[wezel, j].stezenieFeromonu, alfa) * Math.Pow(infoSiec[wezel, j].wagaLacza, beta);
                            zbiorSasiadow.Add(new PrawdPlusWezel() { prawdopodobienstwo = r, nrWezla = j});
                            sumaR += r;
                        }
                    }
                    foreach (PrawdPlusWezel p in zbiorSasiadow)
                        p.prawdopodobienstwo = p.prawdopodobienstwo / sumaR;



                    r = rr.NextDouble();

                }

                licznik++;
            } while (licznik < liczbaIteracji);


        }

        public static double funkcjaHeurystyczna()
        {
            double wsp = 0.5;


            return wsp;
        }
    }
}
