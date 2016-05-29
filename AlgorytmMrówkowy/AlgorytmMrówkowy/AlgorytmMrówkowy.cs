﻿using System;
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
    class AlgorytmMrówkowy
    {
        public static TimeSpan elapsedMs = TimeSpan.MinValue;
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
            int stagnacja = 0;
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
            double pomLambda = 0.05;//0.025
            double[] pomMaxTał = new double[infoSiec.GetLength(1)], pomMinTał = new double[infoSiec.GetLength(1)];
            double[,] tablica = new double[infoSiec.GetLength(1), infoSiec.GetLength(1)];// Tablica którą używamy przy punkcie stagnacji
            bool czyStagnacja = false;

            var watch = System.Diagnostics.Stopwatch.StartNew();
            

            do
            {
                for (int i = 0; i < infoSiec.GetLength(1); i++)
                {
                    pomMaxTał[i] = 0;
                    pomMinTał[i] = 99999999999;
                }

                liczIncydZWarun = 0;

                for (int i = 0; i < infoSiec.GetLength(0); i++)
                {
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
                }
                //Console.WriteLine(pomMinTał[50]);
                //Console.WriteLine(pomMaxTał[50]);
                for (int i = 0; i < infoSiec.GetLength(1); i++)
                {
                    for (int j = 0; j < infoSiec.GetLength(1); j++)
                    {
                        if (infoSiec[i, j].wagaLacza != 0 && infoSiec[i, j].stezenieFeromonu > pomLambda * (pomMaxTał[i] - pomMinTał[i]) + pomMinTał[i])
                        {
                            liczIncydZWarun++;//nie wchodzi do punktu stagnacji
                        }
                    }
                }
                Console.WriteLine(liczIncydZWarun);
                double deltaTał = 0;
                //Jeśli jesteśmy w punkcie stagnacji
                /************************************     zamknąłem klamre ifa *****************************************/
                czyStagnacja = false;
                double tmp2 = (double)liczIncydZWarun / (double)infoSiec.GetLength(1);
                if (licznik != 0)
                {
                    if (tmp2 < epsilon)//jeśli znajdujesz się w punkcie stagnacji(równanie6)
                    {
                        stagnacja++;
                        czyStagnacja = true;
                        for (int i = 0; i < infoSiec.GetLength(1); i++)
                            for (int j = 0; j < infoSiec.GetLength(1); j++)
                            {
                                deltaTał = (tałMax - infoSiec[i, j].stezenieFeromonu) * wspolczynnikRownania5;//delta tal ij (t)
                                tablica[i, j] = infoSiec[i, j].stezenieFeromonu + deltaTał;//delta tal ij (t+1)+delta ij  tal(t)
                            }
                    }
                }// do tej pory ok
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
                            if (infoSiec[wezel, j].wagaLacza != 0 && !tabuList.Contains(j))//spróbować bez uwzględniania tabu list
                            {
                                r = Math.Pow(infoSiec[wezel, j].stezenieFeromonu, alfa) * Math.Pow((1 / (infoSiec[wezel, j].wagaLacza)), beta); //Równanie 2 bez dzielenia przez sumę.
                                zbiorSasiadow.Add(new PrawdPlusWezel() { prawdopodobienstwo = r, nrWezla = j });
                                sumaR += r;
                            }
                        }
                        if(sumaR == 0 && zbiorSasiadow.Count != 0)
                        {
                            string saf = "df";
                        }
                        double sumaPrawd = 0;//sprawdzenie czy się sumują do jedynki
                        foreach (PrawdPlusWezel p in zbiorSasiadow)
                        {
                            p.prawdopodobienstwo = p.prawdopodobienstwo / sumaR; // Dokończenie Równania nr 2.
                            sumaPrawd += p.prawdopodobienstwo;
                        }

                        if(sumaPrawd != 1)
                        {
                            string afgs = "asd";
                        }
                        double poprz = 0;
                        // Losowanie liczby z przedziału (0, 1) o rozkładdzie jednostajnym
                        r = rr.NextDouble();
                        // Wybór kolejnego wierzchołka.
                        foreach (PrawdPlusWezel p in zbiorSasiadow)
                        {
                            //p.prawdopodobienstwo = p.prawdopodobienstwo / sumaR;
                            if (r >= poprz && r < poprz + p.prawdopodobienstwo)
                            {
                                tmp.listaWierzcholkow.Add(p.nrWezla); //Jeśli wylosowano ten wierzchołek to dodajmy go do ścieżki.
                                tmp.koszt += infoSiec[wezel, p.nrWezla].wagaLacza;
                                wezel = p.nrWezla;
                                tabuList.Add(wezel);
                                break;
                            }
                            poprz = p.prawdopodobienstwo;
                        }
                        /*************** w przypadku zablokowania mrówki ustawiamy wartość rozwiązania na nieskończność ***********************************/
                        if ((zbiorSasiadow.Count == 0 && wezel != wezelKoncowy) || (zbiorSasiadow.Count != 0 && sumaR == 0))
                        {
                            tmp.koszt = Double.PositiveInfinity;
                            break;
                        }
                        else if(wezelKoncowy == wezel)
                        {
                            int sprawdz = 0;
                        }
                    } while (wezel != wezelKoncowy);

                    //Dodanie ścieżki do stogu.
                    zbiorSciezek.Insert(tmp);
                }
                #endregion

                //Dodaję każde najlepsze rozwiązanie z iteracji do najlepszych globalnych rozwiązań.
                najlepszeGlobalnieSciezki.Insert(zbiorSciezek.Max());

                //Aktualizacja stężenia feromonów.

                Info[,] grafPomocniczy = new Info[infoSiec.GetLength(0), infoSiec.GetLength(0)];
                double noweStezenie = 0;
                if (licznik % fLambda==0)//nie koniecznie równe wystarczy że będzie podzielne przez flambda poprawić
                {
                    Sciezka s = najlepszeGlobalnieSciezki.Max();
                    for (int i = 0; i < s.listaWierzcholkow.Count - 1; i++)
                    {
                        //if(infoSiec[s.listaWierzcholkow[i], s.listaWierzcholkow[i + 1]].stezenieFeromonu != 0)
                        {
                            noweStezenie = (1 - ro) * infoSiec[s.listaWierzcholkow[i], s.listaWierzcholkow[i + 1]].stezenieFeromonu + ro * (1 / s.koszt);
                            if (infoSiec[s.listaWierzcholkow[i], s.listaWierzcholkow[i + 1]].stezenieFeromonu == 0)
                            {
                                string supa = "ads";
                            }
                            grafPomocniczy[s.listaWierzcholkow[i], s.listaWierzcholkow[i + 1]].stezenieFeromonu = noweStezenie;
                            grafPomocniczy[s.listaWierzcholkow[i + 1], s.listaWierzcholkow[i]].stezenieFeromonu = noweStezenie;
                            //                        infoSiec[s.listaWierzcholkow[i], s.listaWierzcholkow[i + 1]].stezenieFeromonu = noweStezenie;
                            /*********************** graf nieskierowany trzeba dodać tu i tu analogicznie w elsie**************************************************************/
                            //                      infoSiec[s.listaWierzcholkow[i+1], s.listaWierzcholkow[i ]].stezenieFeromonu = noweStezenie;//graf nieskierowany
                        }
                    }
                   
                }
                else
                {
                    Sciezka s = zbiorSciezek.Max();
                    for (int i = 0; i < s.listaWierzcholkow.Count - 1; i++)
                    {
                        //if (infoSiec[s.listaWierzcholkow[i], s.listaWierzcholkow[i + 1]].stezenieFeromonu != 0)
                        {
                            noweStezenie = (1 - ro) * infoSiec[s.listaWierzcholkow[i], s.listaWierzcholkow[i + 1]].stezenieFeromonu + ro * (1 / s.koszt);
                            if (infoSiec[s.listaWierzcholkow[i], s.listaWierzcholkow[i + 1]].stezenieFeromonu == 0)
                            {
                                string supa = "ads";
                            }
                            grafPomocniczy[s.listaWierzcholkow[i], s.listaWierzcholkow[i + 1]].stezenieFeromonu = noweStezenie;
                            grafPomocniczy[s.listaWierzcholkow[i + 1], s.listaWierzcholkow[i]].stezenieFeromonu = noweStezenie;
                            //                    infoSiec[s.listaWierzcholkow[i], s.listaWierzcholkow[i + 1]].stezenieFeromonu = noweStezenie;
                            //                  infoSiec[s.listaWierzcholkow[i+1], s.listaWierzcholkow[i ]].stezenieFeromonu = noweStezenie;
                        }
                    }
                }

               
                for (int i = 0; i < infoSiec.GetLength(0); i++)
                {
                    for (int j = 0; j < infoSiec.GetLength(0); j++)
                    {
                        infoSiec[i, j].stezenieFeromonu = infoSiec[i, j].stezenieFeromonu * (1-ro);
                    }
                }

                for (int i = 0; i < infoSiec.GetLength(0); i++)
                {
                    for (int j = 0; j < infoSiec.GetLength(0); j++)
                    {
                        if(grafPomocniczy[i,j].stezenieFeromonu!=0)
                            infoSiec[i, j].stezenieFeromonu = grafPomocniczy[i,j].stezenieFeromonu;
                    }
                }





                //Stężenie feromonów musi zawierać się w określonych granicach.
                for (int i = 0; i < infoSiec.GetLength(1); i++)//to jest ok
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

                tałMax = (1 / (1 - ro)) * (1 / najlepszeGlobalnieSciezki.Max().koszt); // Równanie 10. sprawdzic tez
                /****************************************************zmianiłem n na info.GetLength(0) (liczba mrówek na liczbe wierzchiłków) ****************************************/
                tałMin = (tałMax * (1 - (Math.Sqrt(pDaszkiem)) * infoSiec.GetLength(0))) / ((infoSiec.GetLength(0) / 2 - 1) * (Math.Sqrt(pDaszkiem)) * infoSiec.GetLength(0)); //Równanie 11. n to liczba wierzchołków a nie mrówek

               if (czyStagnacja)
                    for (int i = 0; i < infoSiec.GetLength(1); i++)
                        for (int j = 0; j < infoSiec.GetLength(1); j++)
                            infoSiec[i, j].stezenieFeromonu = tablica[i, j];
                            
                licznik++;
            
            
            } while (licznik < liczbaIteracji);

            watch.Stop();
            elapsedMs = watch.Elapsed;
            return najlepszeGlobalnieSciezki;
        }
    }
}
