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
            Encoding enc = Encoding.GetEncoding("Windows-1250");
            using (FileStream fs = new FileStream(plik + ".txt", FileMode.Open))
            using (StreamReader sr = new StreamReader(fs, enc))
            {
                //CultureInfo pt = CultureInfo.GetCultureInfo()

                sciezkaDoGrafu = sr.ReadLine().Split(' ')[1];
                alfa = Convert.ToDouble((sr.ReadLine()).Split(' ')[1], CultureInfo.InvariantCulture);
                beta = Convert.ToDouble((sr.ReadLine()).Split(' ')[1], CultureInfo.InvariantCulture);
                n = Convert.ToDouble((sr.ReadLine()).Split(' ')[1], CultureInfo.InvariantCulture);
                ro = Convert.ToDouble((sr.ReadLine()).Split(' ')[1], CultureInfo.InvariantCulture);
                pDaszkiem = Convert.ToDouble((sr.ReadLine()).Split(' ')[1], CultureInfo.InvariantCulture);
                tałMin = Convert.ToDouble((sr.ReadLine()).Split(' ')[1], CultureInfo.InvariantCulture);
                tałMax = Convert.ToDouble((sr.ReadLine()).Split(' ')[1], CultureInfo.InvariantCulture);
                fLambda = Convert.ToInt32((sr.ReadLine()).Split(' ')[1], CultureInfo.InvariantCulture);
                wezelStartowy = Convert.ToInt32((sr.ReadLine()).Split(' ')[1], CultureInfo.InvariantCulture);
                wezelKoncowy = Convert.ToInt32((sr.ReadLine()).Split(' ')[1], CultureInfo.InvariantCulture);
                liczbaIteracji = Convert.ToInt32((sr.ReadLine()).Split(' ')[1], CultureInfo.InvariantCulture);
                epsilon = Convert.ToDouble((sr.ReadLine()).Split(' ')[1], CultureInfo.InvariantCulture);
                wspolczynnikRownania5 = Convert.ToDouble((sr.ReadLine()).Split(' ')[1], CultureInfo.InvariantCulture);
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

            TimeSpan czasWykonywania = AlgorytmMrówkowy.elapsedMs;

            Encoding enc = Encoding.GetEncoding("Windows-1250");
            using (FileStream fs = new FileStream("Wynik" + plikKonfiguracujny + ".txt", FileMode.Create))
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
        }

        public void wykonajKilkukrotnieAlgorytm(int ile)
        {
            for(int i = 0; i < ile; i++)
            {
                wykonajAlgorytm("Parametry" + i);
            }
        }
    }
}
