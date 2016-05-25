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
            //double[,] grafKońcowy = new double[20, 20];
            //double[,] grafKońcowy2 = new double[20, 20];
            //grafKońcowy = Generator.GenerujGraf(20, 3, grafPoczątkowy, 0, 20);
           // ModułObsługiPlików.ZapiszDoPliku(grafKońcowy, "próba");
    double[,] graf=        ModułObsługiPlików.OdczytajZPliku("graf");
            AlgorytmDijkstry.wykonajAlgorytm(graf,5,7);
        }
    }
}
