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
        //Klasa opisująca stan łącza.
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
    /// <summary>
    /// Klasa do opisu sąsiednich wierzchołków.
    /// </summary>
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
        /// <param name="epsilon">Parametr określający punkt satgnacji.</param>
        /// <param name="wspolczynnikRownanie5">Co tu pisac.</param>
        static public Stog WykonajAlgorytm(double[,] grafWejsciowy,
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
                                            int liczbaIteracji,
                                            double epsilon,
                                            double wspolczynnikRownania5)
        {
            int wymiarMacierzyDol = grafWejsciowy.GetLength(0);
            int wymiarMacierzyBok = grafWejsciowy.GetLength(1);
            //Tablica przechowująca informacje o grafie
            Info[,] infoSiec = new Info[wymiarMacierzyDol, wymiarMacierzyBok];


            for (int i = 0; i < grafWejsciowy.GetLength(1); i++)
                for (int j = 0; j < grafWejsciowy.GetLength(1); j++)
                {
                    infoSiec[i, j].stezenieFeromonu = tałMax;
                    infoSiec[i, j].wagaLacza = grafWejsciowy[i, j];
                }

            //Zbior sciezek w danej iteracji
            Stog zbiorSciezek; //Zbiór ścieżek z jednej iteracji.
            Stog najlepszeGlobalnieSciezki = new Stog();
            int licznik = 0;
            int wezel;
            Sciezka tmp;
            List<int> tabuList;
            List<PrawdPlusWezel> zbiorSasiadow;
            Random rr = new Random();
            int liczIncydZWarun = 0;
            double pomLambda = 0.05;
            double[] pomMaxTał = new double[infoSiec.GetLength(1)], pomMinTał = new double[infoSiec.GetLength(1)];
            double[,] tablica = new double[infoSiec.GetLength(1), infoSiec.GetLength(1)];// Tablica którą używamy przy punkcie stagnacji
            bool czyStagnacja = false;
            do
            {
                for (int i = 0; i < infoSiec.GetLength(1); i++)
                {
                    pomMaxTał[i] = 0;
                    pomMinTał[i] = 99999999999;
                }

                liczIncydZWarun = 0;

                for (int i = 0; i < infoSiec.GetLength(0); i++)
                    for (int j = 0; j < infoSiec.GetLength(1); j++)
                    {
                        if (infoSiec[i, j].wagaLacza != 0)
                        {
                            if (pomMaxTał[i] < infoSiec[i, j].stezenieFeromonu)
                                pomMaxTał[i] = infoSiec[i, j].stezenieFeromonu;
                            if (pomMinTał[i] > infoSiec[i, j].stezenieFeromonu)
                                pomMinTał[i] = infoSiec[i, j].stezenieFeromonu;
                        }
                    }

                for (int i = 0; i < infoSiec.GetLength(1); i++)
                {
                    for (int j = 0; j < infoSiec.GetLength(1); j++)
                    {
                        if (infoSiec[i, j].wagaLacza != 0 && infoSiec[i, j].stezenieFeromonu > pomLambda * (pomMaxTał[i] - pomMinTał[i]) + pomMinTał[i])
                        {
                            liczIncydZWarun++;
                        }
                    }
                }
                double deltaTał = 0;
                //Jeśli jesteśmy w punkcie stagnacji
                czyStagnacja = false;
                if (liczIncydZWarun / infoSiec.GetLength(1) < epsilon)
                {
                    czyStagnacja = true;
                    for (int i = 0; i < infoSiec.GetLength(1); i++)
                        for (int j = 0; j < infoSiec.GetLength(1); j++)
                        {
                            deltaTał = (tałMax - infoSiec[i, j].stezenieFeromonu) * wspolczynnikRownania5;
                            tablica[i, j] = infoSiec[i, j].stezenieFeromonu + deltaTał;
                        }

                    zbiorSciezek = new Stog();
                    double r = 0, sumaR = 0;

                    #region Dla każdej mrówki znajdź scieżkę
                    for (int i = 0; i < n; i++)
                    {
                        wezel = wezelStartowy;
                        tmp = new Sciezka();
                        tmp.listaWierzcholkow.Add(wezelStartowy);
                        tabuList = new List<int>() { wezel };
                        //Wybór ścieżki dla konkretnej mrówki.
                        do
                        {
                            zbiorSasiadow = new List<PrawdPlusWezel>();
                            // Obliczenie prawdopodobieństw przejść do sąsiednich wierzchołków.
                            sumaR = 0;
                            for (int j = 0; j < wymiarMacierzyBok; j++)
                            {
                                if (infoSiec[wezel, j].wagaLacza != 0 && !tabuList.Contains(j))
                                {
                                    r = Math.Pow(infoSiec[wezel, j].stezenieFeromonu, alfa) * Math.Pow(1 / (infoSiec[wezel, j].wagaLacza), beta); //Równanie 2 bez dzielenia przez sumę.
                                    zbiorSasiadow.Add(new PrawdPlusWezel() { prawdopodobienstwo = r, nrWezla = j });
                                    sumaR += r;
                                }
                            }
                            foreach (PrawdPlusWezel p in zbiorSasiadow)
                                p.prawdopodobienstwo = p.prawdopodobienstwo / sumaR; // Dokończenie Równania nr 2.



                            double poprz = 0;
                            // Losowanie liczby z przedziału (0, 1) o rozkładdzie jednostajnym
                            r = rr.NextDouble();
                            // Wybór kolejnego wierzchołka.
                            foreach (PrawdPlusWezel p in zbiorSasiadow)
                            {
                                //p.prawdopodobienstwo = p.prawdopodobienstwo / sumaR;
                                if (r >= poprz && r < p.prawdopodobienstwo)
                                {
                                    tmp.listaWierzcholkow.Add(p.nrWezla); //Jeśli wylosowano ten wierzchołek to dodajmy go do ścieżki.
                                    tmp.koszt += infoSiec[wezel, p.nrWezla].wagaLacza;
                                    wezel = p.nrWezla;
                                    tabuList.Add(wezel);
                                    break;
                                }
                                poprz = p.prawdopodobienstwo;
                            }
                        } while (wezel != wezelKoncowy && zbiorSasiadow.Count != 0);

                        //Dodanie ścieżki do stogu.
                        zbiorSciezek.Insert(tmp);
                    }
                    #endregion

                    //Dodaję każde najlepsze rozwiązanie z iteracji do najlepszych globalnych rozwiązań.
                    najlepszeGlobalnieSciezki.Insert(zbiorSciezek.Max());

                    //Aktualizacja stężenia feromonów.
                    double noweStezenie = 0;
                    if (licznik == fLambda)
                    {
                        Sciezka s = najlepszeGlobalnieSciezki.Max();
                        for (int i = 0; i < s.listaWierzcholkow.Count - 1; i++)
                        {
                            noweStezenie = (1 - ro) * infoSiec[s.listaWierzcholkow[i], s.listaWierzcholkow[i + 1]].stezenieFeromonu + ro * (1 / s.koszt);
                            infoSiec[s.listaWierzcholkow[i], s.listaWierzcholkow[i + 1]].stezenieFeromonu = noweStezenie;
                        }
                    }
                    else
                    {
                        Sciezka s = zbiorSciezek.Max();
                        for (int i = 0; i < s.listaWierzcholkow.Count - 1; i++)
                        {
                            noweStezenie = (1 - ro) * infoSiec[s.listaWierzcholkow[i], s.listaWierzcholkow[i + 1]].stezenieFeromonu + ro * (1 / s.koszt);
                            infoSiec[s.listaWierzcholkow[i], s.listaWierzcholkow[i + 1]].stezenieFeromonu = noweStezenie;
                        }
                    }

                    //Stężenie feromonów musi zawierać się w określonych granicach.
                    for (int i = 0; i < infoSiec.GetLength(1); i++)
                        for (int j = 0; j < infoSiec.GetLength(1); j++)
                        {
                            if (infoSiec[i, j].stezenieFeromonu < tałMin)
                            {
                                infoSiec[i, j].stezenieFeromonu = tałMin;
                            }
                            else if (infoSiec[i, j].stezenieFeromonu > tałMax)
                            {
                                infoSiec[i, j].stezenieFeromonu = tałMax;
                            }
                        }

                    tałMax = (1 / (1 - ro)) * (1 / najlepszeGlobalnieSciezki.Max().koszt); // Równanie 10.
                    tałMin = (tałMax * (1 - (Math.Sqrt(pDaszkiem)) * n)) / ((n / 2 - 1) * (Math.Sqrt(pDaszkiem)) * n); //Równanie 11.

                    if(czyStagnacja)
                        for (int i = 0; i < infoSiec.GetLength(1); i++)
                            for (int j = 0; j < infoSiec.GetLength(1); j++)
                                infoSiec[i, j].stezenieFeromonu = tablica[i, j];

                    licznik++;
                }

            } while (licznik < liczbaIteracji);

            return najlepszeGlobalnieSciezki;
        }
    }
}
