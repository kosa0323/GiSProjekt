using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorytmMrówkowy
{
    class Koszt : IComparable<Koszt>
    {
        #region variables
        public int idWierzchołka { get; set; }
        public double odległośćOdWierzchołkaŹródłowego { get; set; }
        #endregion
        public Koszt(int id, double odległość)
        {
            idWierzchołka = id;
            odległośćOdWierzchołkaŹródłowego = odległość;
        }
        public Koszt() { }
        public int CompareTo(Koszt innyWierzchołek)
        {
            if (this.odległośćOdWierzchołkaŹródłowego > innyWierzchołek.odległośćOdWierzchołkaŹródłowego)
                return -1;
            if (this.odległośćOdWierzchołkaŹródłowego < innyWierzchołek.odległośćOdWierzchołkaŹródłowego)
                return 1;
            else
                return 0;
        }
    }
}
