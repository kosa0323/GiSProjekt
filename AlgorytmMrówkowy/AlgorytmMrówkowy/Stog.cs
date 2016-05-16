using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorytmMrówkowy
{
    class Stog
    {
        private IList<Sciezka> data;
        public int HeapSize;

        public Stog()
        {
            data = new List<Sciezka>();
            //dodaję zerowy element ponieważ zaczynamy wypełniać tablicę od indeksu 1
            data.Add(new Sciezka() { koszt = 0, listaWierzcholkow = null});
        }

        private void Swap(int index0, int index1)
        {
            Sciezka aux = data[index0];
            data[index0] = data[index1];
            data[index1] = aux;
        }

        public void Insert(Sciezka n)
        {
            HeapSize++;
            data.Add(n);
            int Index = HeapSize;
            while (Index > 1)
            {
                if (n.CompareTo(data[Index / 2]) == 1) Swap(Index, Index / 2); //n > data...
                else break;
                Index = Index / 2;
            }
        }

        public void MoveDownHeap(int topIndex)
        {
            int index = topIndex;
            Sciezka n = data[topIndex];
            while (index * 2 <= HeapSize)
            {
                int indexGreater;
                if ((index * 2 < HeapSize) && (data[index * 2 + 1].CompareTo(data[index * 2]) == 1)) //>
                    indexGreater = index * 2 + 1;
                else
                    indexGreater = index * 2;
                if (n.CompareTo(data[indexGreater]) == -1) // <
                    Swap(index, indexGreater);
                else break;
                index = indexGreater;
            }
        }

        public void DeleteMax()
        {
            data[1] = data[HeapSize];
            data.RemoveAt(HeapSize);
            HeapSize--;
            MoveDownHeap(1);
        }

        public void Construct(int index)
        {
            if (2 * index <= HeapSize / 2) Construct(2 * index);
            if (2 * index + 1 <= HeapSize / 2) Construct(2 * index + 1);
            MoveDownHeap(index);
        }

        public void Check()
        {
            for (int i = 1; i <= HeapSize / 2; i++)
            {
                if (data[i].CompareTo(data[2 * i]) == -1) throw new Exception("Error in Heap"); // <
                if (2 * i + 1 <= HeapSize)
                    if (data[i].CompareTo(data[2 * i + 1]) == -1) throw new Exception("Error in Heap"); // <
            }
        }
        public Sciezka Max()
        {
            return data[1];
        }
    }
}
