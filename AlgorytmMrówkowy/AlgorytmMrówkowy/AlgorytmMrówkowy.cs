using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorytmMrówkowy
{
    static class AlgorytmMrówkowy
    {
        /// <summary>
        /// Funkcja wywołująca realizująca algorytm mrówkowy(Max-Min).
        /// </summary>
        /// <param name="grafWejsciowy">Graf Wejściowy</param>
        /// <param name="alfa">Współczynnik określający znaczenie stężenia feromonów.</param>
        /// <param name="beta">Współczynnik sterujący atrakcyjnością łącza.</param>
        /// <param name="n">Liczba mrówek</param>
        /// <param name="ro">Współczynnik parowania.</param>
        /// <param name="pDaszkiem">Prawdopodobieństwo uzyskania najlepszego wyniku.</param>
        /// <param name="tałMin">Minimalny poziom stężenia feromonu.</param>
        /// <param name="tałMax">Maksymalny poziom stężęnia feromonu.</param>
        /// <param name="fLambda">Częstotliwość aktualizacji stężenia feromonów globalną ścieżką.</param>
        /// <param name="wezelStartowy">Węzeł początkowy.</param>
        /// <param name="wezelKoncowy">Węzeł końcowy.</param>
        static public void WykonajAlgorytm( double[,] grafWejsciowy, 
                                            double alfa, 
                                            double beta,
                                            double n,
                                            double ro,
                                            double pDaszkiem,
                                            double tałMin,
                                            double tałMax,
                                            int fLambda,
                                            int wezelStartowy,
                                            int wezelKoncowy)
        {
            //Inicjalizacja poziomu stężęnia feromonów.
            double[,] stezenieFeromonow = new double[grafWejsciowy.GetLength(1), grafWejsciowy.GetLength(2)];
            for (int i = 0; i < grafWejsciowy.GetLength(1); i++)
                for (int j = 0; j < grafWejsciowy.GetLength(2); j++)
                    stezenieFeromonow[i, j] = tałMax;

            
            Stog zbiorSciezek = new Stog();
            Sciezka najlepszaGlobalnaSciezka;

        }
    }
}
