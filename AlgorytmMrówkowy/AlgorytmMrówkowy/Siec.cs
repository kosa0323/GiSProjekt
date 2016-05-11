using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorytmMrówkowy
{
    class Siec
    {
        List<double>[,] macierzWag = new List<double>[50, 50];
        

        public void WczytajSiec(string path)
        {
            macierzWag[0, 0] = new List<double>();
            macierzWag[0, 0].Add(12123.543);
        }

        public void GenerujSiec()
        {

        }
    }
}
