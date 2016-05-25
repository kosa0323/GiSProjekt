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
            grafKońcowy = Generator.GenerujGraf(100, 3, grafPoczątkowy, 0, 30);
            ModułObsługiPlików.ZapiszDoPliku(grafKońcowy, "próba100");*/
            double[,] graf = ModułObsługiPlików.OdczytajZPliku("próba100");
            double kosztDijkstry = AlgorytmDijkstry.wykonajAlgorytm(graf, 30, 67);
            System.Console.WriteLine("Disjkstra: " + kosztDijkstry);
            


            Stog s = AlgorytmMrówkowy.WykonajAlgorytm(graf, 2, 1, 300, 0.08, 0.1, 1, 100, 10, 30, 67, 100, 0.1, 2);

            System.Console.WriteLine("Mrówki: " + s.Max().koszt);



            System.Console.ReadKey();
        }
    }
}
