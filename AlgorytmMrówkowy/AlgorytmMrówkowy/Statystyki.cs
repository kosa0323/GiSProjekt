using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorytmMrówkowy
{
    class Statystyki
    {
        string sciezkaDoGrafu;
        double alfa;
        double beta;
        double n;
        double ro;
        double pDaszkiem;
        double tałMin;
        double tałMax;
        int fLambda;
        int wezelStartowy;
        int wezelKoncowy;
        int liczbaIteracji;
        double epsilon;
        double wspolczynnikRownania5;

        public void wczytajParametryZPliku(string plik)
        {
            //Encoding enc = Encoding.GetEncoding("Windows-1250");
            using (FileStream fs = new FileStream(plik + ".txt", FileMode.Open))
            using (StreamReader sr = new StreamReader(fs/*, enc*/))
            {
                //CultureInfo pt = CultureInfo.GetCultureInfo()

                sciezkaDoGrafu = sr.ReadLine().Split(' ')[2];
                alfa = Convert.ToDouble((sr.ReadLine()).Split(' ')[1], CultureInfo.GetCultureInfo("tr-TR"));
                beta = Convert.ToDouble((sr.ReadLine()).Split(' ')[1], CultureInfo.GetCultureInfo("tr-TR"));
                n = Convert.ToDouble((sr.ReadLine()).Split(' ')[1], CultureInfo.GetCultureInfo("tr-TR"));
                ro = Convert.ToDouble((sr.ReadLine()).Split(' ')[1], CultureInfo.GetCultureInfo("tr-TR"));
                pDaszkiem = Convert.ToDouble((sr.ReadLine()).Split(' ')[1], CultureInfo.GetCultureInfo("tr-TR"));
                tałMin = Convert.ToDouble((sr.ReadLine()).Split(' ')[1], CultureInfo.GetCultureInfo("tr-TR"));
                tałMax = Convert.ToDouble((sr.ReadLine()).Split(' ')[1], CultureInfo.GetCultureInfo("tr-TR"));
                fLambda = Convert.ToInt32((sr.ReadLine()).Split(' ')[1], CultureInfo.GetCultureInfo("tr-TR"));
                wezelStartowy = Convert.ToInt32((sr.ReadLine()).Split(' ')[1], CultureInfo.GetCultureInfo("tr-TR"));
                wezelKoncowy = Convert.ToInt32((sr.ReadLine()).Split(' ')[1], CultureInfo.GetCultureInfo("tr-TR"));
                liczbaIteracji = Convert.ToInt32((sr.ReadLine()).Split(' ')[1], CultureInfo.GetCultureInfo("tr-TR"));
                epsilon = Convert.ToDouble((sr.ReadLine()).Split(' ')[1], CultureInfo.GetCultureInfo("tr-TR"));
                wspolczynnikRownania5 = Convert.ToDouble((sr.ReadLine()).Split(' ')[1], CultureInfo.GetCultureInfo("tr-TR"));
            }
        }


        public void wykonajAlgorytm(string plikKonfiguracujny)
        {
            wczytajParametryZPliku(plikKonfiguracujny);
            double[,] graf = ModułObsługiPlików.OdczytajZPliku(sciezkaDoGrafu);
            Stog s = AlgorytmMrówkowy.WykonajAlgorytm(graf,
                                                        alfa, beta,
                                                        n,
                                                        ro,
                                                        pDaszkiem,
                                                        tałMin,
                                                        tałMax,
                                                        fLambda,
                                                        wezelStartowy,
                                                        wezelKoncowy,
                                                        liczbaIteracji,
                                                        epsilon,
                                                        wspolczynnikRownania5);
            Console.WriteLine("Algorytm wykonany: " + plikKonfiguracujny);
            TimeSpan czasWykonywania = AlgorytmMrówkowy.elapsedMs;

            Encoding enc = Encoding.GetEncoding("Windows-1250");
            using (FileStream fs = new FileStream("WynikNowyPróba" + plikKonfiguracujny + ".txt", FileMode.Create))
            using (StreamWriter sw = new StreamWriter(fs, enc))
            {
                sw.WriteLine("**********Parametry grafu*********");
                sw.WriteLine("Liczba węzłów: " + graf.GetLength(0));
                sw.WriteLine("********Parametry algorytmu********");

                using (FileStream ff = new FileStream(plikKonfiguracujny + ".txt", FileMode.Open))
                using (StreamReader sr = new StreamReader(ff, enc))
                {
                    sw.Write(sr.ReadToEnd());
                }
                sw.WriteLine();
                sw.WriteLine("***********Wynik*************");

                sw.WriteLine("Koszt rozwiązania: " + s.Max().koszt);
                
                sw.WriteLine("Czas wykonywania algorytmu[mm:ss:fff]: " + new DateTime(czasWykonywania.Ticks).ToString("mm:ss.fff"));
            }
            Console.WriteLine("Wynik zapisany: " + plikKonfiguracujny);
        }

        public static void GenerujPlikiDoTestów(string nazwaPliku, string nazwaGrafu, int liczbaMrówek, int liczbaIteracji, double pstwoRozwiązanie, double minimalneStężenie, double maksymalneStężenie, int częstotliwość, int węzełKońcowy, int węzełPoczątkowy, double Epsilon, double współczynnik)
        {
            int[] wartosciAlfa = new int[5] { 0, 1, 2, 3, 4 };
            int[] wartosciBeta = new int[6] { 0, 1, 2, 4, 6, 8 };
            double[] wartościRo = new double[5] { 0.01, 0.04, 0.15, 0.35, 0.5 };
            int nazwa = 0;
            for (int i = 0; i < wartosciAlfa.Length; i++)
            {
                for (int j = 0; j < wartosciBeta.Length; j++)
                {
                    for (int k = 0; k < wartościRo.Length; k++)
                    {
                        using (FileStream fs = new FileStream(nazwaPliku + nazwa + ".txt", FileMode.OpenOrCreate, FileAccess.ReadWrite))
                        using (StreamWriter sw = new StreamWriter(fs))
                        {
                            sw.WriteLine("Graf wejściowy: " + nazwaGrafu);
                            sw.WriteLine("Alfa: " + wartosciAlfa[i]);
                            sw.WriteLine("Beta: " + wartosciBeta[j]);
                            sw.WriteLine("LiczbaMrówek: " + liczbaMrówek);
                            sw.WriteLine("WspółczynnikParowania: " + wartościRo[k]);
                            sw.WriteLine("PrawdopodobieństwoZnalezieniaRozwiazania: " + pstwoRozwiązanie);
                            sw.WriteLine("MinimalnyPoziomStężeniaFeromonów: " + minimalneStężenie);
                            sw.WriteLine("MaksymalnyPoziomStężeniaFeromonów: " + maksymalneStężenie);
                            sw.WriteLine("Częstotliwość: " + częstotliwość);
                            sw.WriteLine("WezelPoczatkowy: " + węzełPoczątkowy);
                            sw.WriteLine("WezelKoncowy: " + węzełKońcowy);
                            sw.WriteLine("LiczbaIteracji: " + liczbaIteracji);
                            sw.WriteLine("Epsilon: " + Epsilon);
                            sw.WriteLine("WspółczynnikRównania5: " + współczynnik);
                        }
                        nazwa++;
                    }
                }
            }

        }
        public void wykonaj()
        {

        }
        public void wykonajKilkukrotnieAlgorytm(int ile)
        {
            for(int i = 300; i < ile + 300; i++)
            {
                Console.WriteLine("Wykonuję algorytm nr: " + i);
                wykonajAlgorytm("Parametry" + i);
            }
        }

        public static void sprawdzeniePoprawnosci(int liczbaIteracji)
        {
            double[,] graf = ModułObsługiPlików.OdczytajZPliku("próba200");
            double kosztDijkstry = AlgorytmDijkstry.wykonajAlgorytm(graf, 56, 155);
            System.Console.WriteLine("Disjkstra: " + kosztDijkstry);

            using (FileStream fs = new FileStream("sprawdzenieStats"+ liczbaIteracji + ".txt", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                for (int i = 0; i < 100; i++)
                {
                    Stog s = AlgorytmMrówkowy.WykonajAlgorytm(graf, 4, 2, 200, 0.04, 0.000005, 0.005, 1, 15, 56, 155, liczbaIteracji, 0.00002, 1.5);
                    System.Console.WriteLine("Mrówki: " + s.Max().koszt);
                    sw.WriteLine(s.Max().koszt);
                }
            }
        }
    }
}
