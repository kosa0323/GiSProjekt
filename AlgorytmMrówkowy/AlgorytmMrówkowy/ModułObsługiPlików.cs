using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AlgorytmMrówkowy
{
    class ModułObsługiPlików
    {
        public static void ZapiszDoPliku(double[,] graf, string nazwaPliku)
        {
            using (FileStream fs = new FileStream(@"C:\grafy\" + nazwaPliku + ".txt", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine("# liczba wierzchołków, wierchołki numerowane sa od 0");
                sw.WriteLine("WIERZCHOŁKI = " + graf.GetLength(0));
                sw.WriteLine("# czwórka liczb to odpowiednio: id, wierzchołek początkowy, wierzchołek końcowy, waga");
                int id = 0;
                for (int i = 0; i < graf.GetLength(0); i++)
                {
                    for (int j = 0; j < graf.GetLength(0); j++)
                    {
                        if (j > i && graf[i, j] != 0)
                        {
                            sw.WriteLine(id + " " + i + " " + j + " " + graf[i, j]);
                            id++;
                        }
                    }
                }
            }
        }
        public static double[,] OdczytajZPliku(string nazwaPliku)
        {
            double[,] graf;
            using (FileStream fs = new FileStream(@"C:\grafy\" + nazwaPliku + ".txt", FileMode.Open))
            using (StreamReader sr = new StreamReader(fs))
            {

                string linia;
                while ((linia = sr.ReadLine()) != null && !linia.Contains("WIERZCHOŁKI")) ;
                linia = Regex.Match(linia, @"\d+").Value;
                int liczbaWierzchołków = Int32.Parse(linia);
                graf = new double[liczbaWierzchołków, liczbaWierzchołków];
                int k = 0;
                linia = sr.ReadLine();
                while ((linia = sr.ReadLine()) != null && linia != "")
                {
                    string[] tmp = linia.Split(' ');
                    graf[Convert.ToInt32(tmp[1]), Convert.ToInt32(tmp[2])] = Convert.ToDouble(tmp[3]);
                    graf[Convert.ToInt32(tmp[1]), Convert.ToInt32(tmp[2])] = Convert.ToDouble(tmp[3]);
                }
            }

            return graf;
        }
    }
}
