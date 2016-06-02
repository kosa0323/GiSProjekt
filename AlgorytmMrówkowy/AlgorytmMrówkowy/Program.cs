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
            double[,] grafPoczątkowy = new double[3, 3];
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
            int []tablicaMrówek = new int [9] {5,10,40,80,100,140,160,180,200 };
            double[,] grafKońcowy; //= new double[5000, 5000];
        //    double[,] grafKońcowy2 = new double[5000, 5000];
  //          grafKońcowy = Generator.GenerujGraf(50, 3, grafPoczątkowy, 0, 1);
            //ModułObsługiPlików.ZapiszDoPliku(grafKońcowy, "próba50");
            double[,] graf = ModułObsługiPlików.OdczytajZPliku("próba100");
       //     double kosztDijkstry = AlgorytmDijkstry.wykonajAlgorytm(graf, 8, 69);
         //         System.Console.WriteLine("Disjkstra: " + kosztDijkstry);
            //        Stog s = AlgorytmMrówkowy.WykonajAlgorytm(graf, 3,2, 50, 0.015, 0.00001, 0.5, 1.2, 10, 8,37 , 1500, 0.25, 1);
            //          System.Console.WriteLine("Mrówki: " + s.Max().koszt);

            for (int j = 0; j < 100; j++)
            {


               
                    Stog s = AlgorytmMrówkowy.WykonajAlgorytm(graf, 2, 1, 50, 0.15, 0.00009, 0.001, 1.1, 15, 8, 69,50, 0.25, 1.5);
                    using (FileStream fs = new FileStream("100razy100wierzch50iteracji2" + ".txt", FileMode.Append, FileAccess.Write))
                    using (StreamWriter sw = new StreamWriter(fs))
                    {

                        sw.WriteLine("wynik: " + s.Max().koszt);

                    }
                
            }
            //Console.ReadKey();
            /*for (int i = 1; i <= 6; i++)
            {
                double[,] graf12 = ModułObsługiPlików.OdczytajZPliku("nowy"+i);
                double kosztDijkstry2 = AlgorytmDijkstry.wykonajAlgorytm(graf, 8, 37);
                System.Console.WriteLine("Disjkstra: " + kosztDijkstry2);

            }*/

//              ModułObsługiPlików.GenerujPlikiDoTestów("próba52","proba50", 50,800, 0.00001, 0.5,1.2,15,37,8,0.25,1.5);
            /*for (int i = 0; i < 150; i++)
            {
                Statystyki s = new Statystyki();
                s.wykonajAlgorytm("próba51"+i);
            }
           for (int i = 0; i < 150; i++)
            {
                Statystyki s = new Statystyki();
                s.wykonajAlgorytm("próba52" + i);
            }
            for (int i = 0; i < 150; i++)
            {
                Statystyki s = new Statystyki();
                s.wykonajAlgorytm("próba101" + i);
            }
            for (int i = 0; i < 150; i++)
            {
                Statystyki s = new Statystyki();
                s.wykonajAlgorytm("próba102" + i);
            }*/
        }
    }
}
