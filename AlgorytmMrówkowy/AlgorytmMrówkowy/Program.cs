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
            double[,] grafPoczątkowy = new double[3, 3];
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
            double[,] grafKońcowy = new double[500, 500];
            double[,] grafKońcowy2 = new double[500, 500];
            grafKońcowy = Generator.GenerujGraf(500, 3, grafPoczątkowy, 0, 1);
            ModułObsługiPlików.ZapiszDoPliku(grafKońcowy, "próba500");
   //         double[,] graf = ModułObsługiPlików.OdczytajZPliku("próba49");
     //       double kosztDijkstry = AlgorytmDijkstry.wykonajAlgorytm(graf, 10, 34);
      //      System.Console.WriteLine("Disjkstra: " + kosztDijkstry);
        //    Stog s = AlgorytmMrówkowy.WykonajAlgorytm(graf, 4,2, 400, 0.001, 0.000001, 0, 0.15, 10, 10,34 , 3500, 1.3, 2);
          //  System.Console.WriteLine("Mrówki: " + s.Max().koszt);



            System.Console.ReadKey();
        }
    }
}
