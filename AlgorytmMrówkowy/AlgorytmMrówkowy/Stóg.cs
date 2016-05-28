using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorytmMrówkowy
{
    class Stóg<T> where T : IComparable<T>

    {
        #region zmienne
        private T[] tablica_elementow;
        private int liczba_elementow;
        #endregion
        #region metody
        public void DodajElement(T obiekt)
        {
            if (liczba_elementow == 0)
            {
                tablica_elementow = new T[10000000];
                tablica_elementow[0] = obiekt;
                liczba_elementow++;
            }

            else
            {
                if (tablica_elementow.Length == liczba_elementow)
                    Array.Resize(ref tablica_elementow, tablica_elementow.Length + 100000);
                tablica_elementow[liczba_elementow] = obiekt;
                liczba_elementow += 1;
            }
            int i = (liczba_elementow - 1);
            T tmp;
            while (i >= 1)
            {
                if (tablica_elementow[i].CompareTo(tablica_elementow[(i - 1) / 2]) == 1)
                {
                    tmp = tablica_elementow[(i - 1) / 2];
                    tablica_elementow[(i - 1) / 2] = tablica_elementow[i];
                    tablica_elementow[i] = tmp;
                }
                else
                    break;

                i = ((i - 1) / 2);
            }

        }
        public void UsunElement()
        {
            if (liczba_elementow == 0)
            {
                Console.Write("Stóg jest pusty");

            }

            if (liczba_elementow == 1)
            {
                liczba_elementow--;

                tablica_elementow[0] = default(T);

            }
            else
            {

                tablica_elementow[0] = tablica_elementow[liczba_elementow - 1];
                tablica_elementow[liczba_elementow - 1] = default(T);
                int i = 0;
                T tmp;
                while (i < (liczba_elementow / 2))
                {
                    int tmp3 = 0;
                    if (tablica_elementow[2 * i + 1] == null && tablica_elementow[2 * i + 2] == null)
                    {
                        break;
                    }
                    if (tablica_elementow[(2 * i + 2)] == null)
                    {
                        break;
                    }
                    else if (tablica_elementow[i].CompareTo(tablica_elementow[2 * i + 1]) < 0 || tablica_elementow[i].CompareTo(tablica_elementow[2 * i + 2]) < 0)
                    {

                        if (tablica_elementow[2 * i + 1].CompareTo(tablica_elementow[2 * i + 2]) == 1)
                        {
                            tmp = tablica_elementow[2 * i + 1];
                            tablica_elementow[2 * i + 1] = tablica_elementow[i];
                            tablica_elementow[i] = tmp;
                            tmp3 = 2 * i + 1;
                        }
                        else
                        {
                            tmp = tablica_elementow[2 * i + 2];
                            tablica_elementow[2 * i + 2] = tablica_elementow[i];
                            tablica_elementow[i] = tmp;
                            tmp3 = 2 * i + 2;
                        }
                        i = tmp3;
                    }
                    else
                        i++;

                }
                liczba_elementow--;

            }

        }
        public T ZwrocElement(int x)
        {
            return tablica_elementow[x];
        }
        #endregion
        public Stóg()
        {
            liczba_elementow = 0;
        }
    }
}
