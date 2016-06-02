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
        public static void GenerujPlikiDoTestów(string nazwaPliku,string nazwaGrafu,int liczbaMrówek, int liczbaIteracji, double pstwoRozwiązanie, double minimalneStężenie, double maksymalneStężenie,int częstotliwość, int węzełKońcowy, int węzełPoczątkowy, double Epsilon, double współczynnik )
        {
            int[] wartosciAlfa = new int[5] { 0, 1, 2, 3, 4 };
            int[] wartosciBeta = new int[6] { 0, 1, 2, 4, 6, 8 };
            double[] wartościRo = new double[5] { 0.01, 0.04, 0.15, 0.35, 0.5 };
            int nazwa = 0;
            for (int i = 0; i < wartosciAlfa.Length; i++)
            {
                for (int j=0;j<wartosciBeta.Length;j++)
                {
                    for (int k=0;k<wartościRo.Length;k++)
                    {
                        using (FileStream fs = new FileStream(nazwaPliku+nazwa+".txt", FileMode.OpenOrCreate, FileAccess.ReadWrite))
                        using (StreamWriter sw = new StreamWriter(fs))
                        {
                            sw.WriteLine("Graf wejściowy: "+nazwaGrafu);
                            sw.WriteLine("Alfa: " + wartosciAlfa[i]);
                            sw.WriteLine("Beta: " + wartosciBeta[j]);
                            sw.WriteLine("LiczbaMrówek: " + liczbaMrówek);
                            sw.WriteLine("WspółczynnikParowania: "+wartościRo[k]);
                            sw.WriteLine("PrawdopodobieństwoZnalezieniaRozwiazania: " + pstwoRozwiązanie);
                            sw.WriteLine("MinimalnyPoziomStężeniaFeromonów: " + minimalneStężenie);
                            sw.WriteLine("MaksymalnyPoziomStężeniaFeromonów: " + maksymalneStężenie);
                            sw.WriteLine("Częstotliwość: " + częstotliwość);
                            sw.WriteLine("WezelPoczatkowy: " + węzełPoczątkowy);
                            sw.WriteLine("WezelKoncowy: "+węzełKońcowy);
                            sw.WriteLine("LiczbaIteracji: "+liczbaIteracji);
                            sw.WriteLine("Epsilon: " + Epsilon);
                            sw.WriteLine("WspółczynnikRównania5: " + współczynnik);
                        }
                        nazwa++;
                    }
                }
            }
            
        }

        public static void ZapiszDoPliku(double[,] graf, string nazwaPliku)
        {
            using (FileStream fs = new FileStream(nazwaPliku + ".txt", FileMode.OpenOrCreate, FileAccess.ReadWrite))
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
            using (FileStream fs = new FileStream(nazwaPliku + ".txt", FileMode.Open))
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
                    graf[Convert.ToInt32(tmp[2]), Convert.ToInt32(tmp[1])] = Convert.ToDouble(tmp[3]);
                }
            }

            return graf;
        }
    }
}
