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
  //         Encoding enc = Encoding.GetEncoding("Windows-1250");
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


        public double wykonajAlgorytm(string nazwaGrafu,string plikKonfiguracujny)
        {
            wczytajParametryZPliku(plikKonfiguracujny);
            sciezkaDoGrafu = nazwaGrafu;
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

            TimeSpan czasWykonywania = AlgorytmMrówkowy.elapsedMs;
            return s.Max().koszt;
           /* Encoding enc = Encoding.GetEncoding("Windows-1250");
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
            }*/
        }

     /*   public void wykonajKilkukrotnieAlgorytm(int ile)
        {
            for(int i = 0; i < ile; i++)
            {
                wykonajAlgorytm("Parametry" + i);
            }
        }*/
    }
}
