using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorytmMrówkowy
{
    class Sciezka : IComparable
    {
        public List<int> listaWierzcholkow;
        public double koszt;

        public Sciezka()
        {
            listaWierzcholkow = new List<int>();
            koszt = 0;
        }

        public int CompareTo(object obj)
        {
            Sciezka s = (Sciezka)obj;
            return koszt.CompareTo(s.koszt)*-1;
        }
    }
}
