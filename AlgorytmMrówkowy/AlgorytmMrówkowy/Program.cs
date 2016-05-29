using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorytmMrówkowy
{
    class Program
    {
        static void Main(string[] args)
        {
            new Statystyki().wykonajKilkukrotnieAlgorytm(5);
            /*double[,] grafPoczątkowy = new double[3, 3];
            for (int i = 0; i < grafPoczątkowy.GetLength(0); i++)
            {
                for (int j = 0; j < grafPoczątkowy.GetLength(0); j++)
                {
                    if (j > i)
                    {
                        Random r = new Random(Guid.NewGuid().GetHashCode());
                        double wagaLosowa = 20 * r.NextDouble();
                        grafPoczątkowy[i, j] = wagaLosowa;
                        grafPoczątkowy[j, i] = wagaLosowa;
                    }
                }
            }
            double[,] grafKońcowy = new double[100, 100];
            double[,] grafKońcowy2 = new double[100, 100];
            grafKońcowy = Generator.GenerujGraf(1000, 3, grafPoczątkowy, 0, 10);
            //ModułObsługiPlików.ZapiszDoPliku(grafKońcowy, "próba1000");
            double[,] graf = ModułObsługiPlików.OdczytajZPliku("próba160");
            double kosztDijkstry = AlgorytmDijkstry.wykonajAlgorytm(graf, 23, 124);
            System.Console.WriteLine("Disjkstra: " + kosztDijkstry);
            


            //Stog s = AlgorytmMrówkowy.WykonajAlgorytm(graf, 2, 1, 300, 0.08, 0.1, 1, 100, 10, 30, 67, 100, 0.1, 2);
            //System.Console.WriteLine("Mrówki: " + s.Max().koszt);
            // Stog s = AlgorytmMrówkowy.WykonajAlgorytm(graf, 10, 2, 400, 0.007, 0.00001, 0, 0.03, 10, 30, 75, 300, 0.001, 25);
            //    Stog s = AlgorytmMrówkowy.WykonajAlgorytm(graf, 12, 0.2, 400, 0.006, 0.00001, 0, 0.03, 10, 30, 75, 300, 0.0000009, 25);
            Stog s = AlgorytmMrówkowy.WykonajAlgorytm(graf, 5, 2, 100, 0.11, 0.00001, 0, 0.1, 10, 23, 124, 100, 0.00000000000000000000000000000000000000000000009, 25);
            System.Console.WriteLine("Mrówki: " + s.Max().koszt);
            System.Console.WriteLine("Disjkstra: " + kosztDijkstry);

            System.Console.ReadKey();*/
        }
    }
}
