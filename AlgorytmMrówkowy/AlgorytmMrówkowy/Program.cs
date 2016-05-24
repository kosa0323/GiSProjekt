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
            double[,] grafKońcowy = new double[10, 10];
            double[,] grafKońcowy2 = new double[10, 10];
            grafKońcowy = Generator.GenerujGraf(10, 3, grafPoczątkowy, 0, 20);
            ModułObsługiPlików.ZapiszDoPliku(grafKońcowy, "próba10");
            ModułObsługiPlików.OdczytajZPliku("próba10");


            //Stog s = AlgorytmMrówkowy.WykonajAlgorytm;

            


        }
    }
}
