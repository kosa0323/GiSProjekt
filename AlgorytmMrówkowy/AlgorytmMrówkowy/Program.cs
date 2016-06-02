using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorytmMrówkowy
{
    class Program
    {
        static void Main(string[] args)
        {

            //Statystyki.GenerujPlikiDoTestów("Parametry", "próba200", 200, 800, 0.001, 0.005, 1, 15, 156, 26, 0.00002, 1.5);
            //Statystyki.GenerujPlikiDoTestów("Parametry", "próba200", 200, 200, 0.001, 0.005, 1, 15, 156, 26, 0.00002, 1.5);
            //new Statystyki().wykonajKilkukrotnieAlgorytm(5);
            /*double[,] grafPoczątkowy = new double[3, 3];
            for (int i = 0; i < grafPoczątkowy.GetLength(0); i++)
            {
                for (int j = 0; j < grafPoczątkowy.GetLength(0); j++)
                {
                    if (j > i)
                    {
                        Random r = new Random(Guid.NewGuid().GetHashCode());
                        double wagaLosowa = r.NextDouble();
                        grafPoczątkowy[i, j] = wagaLosowa;
                        grafPoczątkowy[j, i] = wagaLosowa;
                    }
                }
            }
            double[,] grafKońcowy;//= new double[10000, 10000];
            //double[,] grafKońcowy2 = new double[10000, 10000];
            grafKońcowy = Generator.GenerujGraf(500, 3, grafPoczątkowy, 0, 1);

            ModułObsługiPlików.ZapiszDoPliku(grafKońcowy, "próba500");
            double[,] graf = ModułObsługiPlików.OdczytajZPliku("próba500");
            double kosztDijkstry = AlgorytmDijkstry.wykonajAlgorytm(graf, 56, 433);
            System.Console.WriteLine("Disjkstra: " + kosztDijkstry);
            



            //  Stog s = AlgorytmMrówkowy.WykonajAlgorytm(graf, 10, 2, 400, 0.007, 0.00001, 0, 0.03, 10, 30, 75, 300, 0.001, 25);
            //    Stog s = AlgorytmMrówkowy.WykonajAlgorytm(graf, 12, 0.2, 400, 0.006, 0.00001, 0, 0.03, 10, 30, 75, 300, 0.0000009, 25);
            for(int i = 30; i < 60; i++)
            {
                Stog s = AlgorytmMrówkowy.WykonajAlgorytm(graf, 4, 2, 200, 0.04, 0.000005, 0.005, 1, 15, 56, 433, 50, 0.00002, 1.5);
                System.Console.WriteLine("Mrówki: " + s.Max().koszt);
            }
            
            Console.WriteLine("Startuję!");
            //new Statystyki().wykonajKilkukrotnieAlgorytm(300);
            System.Console.WriteLine("Wykoanno");*/

            //Statystyki.sprawdzeniePoprawnosci(50);
            //Statystyki.sprawdzeniePoprawnosci(200);
            //Statystyki.sprawdzeniePoprawnosci(300);


            //ModułObsługiPlików.ZapiszDoPlikuYenSAlgorithm("próba200");

            //double[,] graf = ModułObsługiPlików.OdczytajZPliku("próba200");
            //double kosztDijkstry = AlgorytmDijkstry.wykonajAlgorytm(graf, 55, 156);
            //System.Console.WriteLine("Disjkstra: " + kosztDijkstry);

            int[] tablicaMrówek = new int[5] { 5, 10, 30, 50, 100};
            double[,] grafKońcowy; //= new double[5000, 5000];
                                   //    double[,] grafKońcowy2 = new double[5000, 5000];
                                   //          grafKońcowy = Generator.GenerujGraf(50, 3, grafPoczątkowy, 0, 1);
                                   //ModułObsługiPlików.ZapiszDoPliku(grafKońcowy, "próba50");
            double[,] graf = ModułObsługiPlików.OdczytajZPliku("próba50");
            double kosztDijkstry = AlgorytmDijkstry.wykonajAlgorytm(graf, 8, 37);
            //      System.Console.WriteLine("Disjkstra: " + kosztDijkstry);
            //        Stog s = AlgorytmMrówkowy.WykonajAlgorytm(graf, 3,2, 50, 0.015, 0.00001, 0.5, 1.2, 10, 8,37 , 1500, 0.25, 1);
            //          System.Console.WriteLine("Mrówki: " + s.Max().koszt);

            for (int j = 0; j < 100; j++)
            {
                for (int i = 0; i < tablicaMrówek.Length; i++)
                {
                    Stog s = AlgorytmMrówkowy.WykonajAlgorytm(graf, 1, 1, tablicaMrówek[i], 0.35, 0.00001, 0.5, 1.2, 15, 8, 37, 10, 0.25, 1.5);

                    //       System.Console.WriteLine("Liczba mrówek: " + tablicaMrówek[i] + "wynik: " + s.Max().koszt);
                    using (FileStream fs = new FileStream("LiczbamrowekgrafIter10Graf50" + ".txt", FileMode.Append, FileAccess.Write))
                    using (StreamWriter sw = new StreamWriter(fs))
                    {

                        sw.WriteLine("Liczba mrówek: " + tablicaMrówek[i] + " wynik: " + s.Max().koszt);

                    }
                }
            }
            System.Console.WriteLine("koniec");
            System.Console.ReadKey();
        }
    }
}
