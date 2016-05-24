using System;
using System.Net;

namespace OLSR.OLSR.Packets.Messages.Neighbors
{
    /// <summary>
    /// 
    /// Created by  APIF Moviquity S.A.
    /// 
    /// http://www.moviquity.com/
    /// 
    /// Clase encargada de controlar la informacion de los vecinos a segundo salto
    ///
    /// Version: 1.1
    ///
    /// </summary>
    class SecondHopNeighbor
    {

        # region Variables
        //Direccion IP del vecino
        private readonly IPAddress N_neighbor_main_addr;
        private readonly IPAddress N_2hop_addr;
        private double N_time;

        # endregion

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="mainAdress">Direccion IP del vecino</param>
        /// <param name="seconHopAddress">Direccion IP del vecino a segundo salto</param>
        /// <param name="validityTime">Tiempo de validez del mensaje</param>
        public SecondHopNeighbor(IPAddress mainAdress, IPAddress seconHopAddress, double validityTime)
        {
            N_neighbor_main_addr = mainAdress;
            N_2hop_addr = seconHopAddress;
            N_time = DateTime.Now.AddSeconds(validityTime).Ticks;
        }

        /// <summary>
        /// Metodo encargado de actualizar la validez del vecino a segundo salto
        /// </summary>
        /// <param name="validityTime">Tiempo de validez</param>
        public void UpdateSecondHopNeighbor(double validityTime)
        {
            N_time = DateTime.Now.AddSeconds(validityTime).Ticks;
        }

        #region getters

        public IPAddress GetN_neighbor_main_addr()
        {
            return N_neighbor_main_addr;
        }
        public IPAddress GetN_2hop_addr()
        {
            return N_2hop_addr;
        }

        public double GetN_Time()
        {
            return N_time;
        }

        # endregion
    }
}