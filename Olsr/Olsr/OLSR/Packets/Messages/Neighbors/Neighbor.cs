using System;
using System.Net;
using OLSR.OLSR.Packets.Messages.Links;

namespace OLSR.OLSR.Packets.Messages.Neighbors
{
    /// <summary>
    /// 
    /// Created by  APIF Moviquity S.A.
    /// 
    /// http://www.moviquity.com/
    ///
    /// Clase encargada de controlar la informacion de los vecinos directos 
    ///
    /// Version: 1.1
    ///
    /// </summary>
    class Neighbor
    {
        #region Variables
        //Direccion IP del vecino
        private readonly IPAddress N_neighbor_main_addr;
        private int N_Status;
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="link">Link a partir del cual crear el vecino</param>
        public Neighbor(LinkSensing link)
        {
            N_neighbor_main_addr = link.L_neighbor_iface_addr;
            UpdateNeighbor(link.GetL_SYM_Time());
        } 
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="link">Link a partir del cual crear el vecino</param>
        public Neighbor(IPAddress pLinkNeighAdd, long pTime)
        {
            N_neighbor_main_addr = pLinkNeighAdd;
            UpdateNeighbor(pTime);
        }
        
        /// <summary>
        /// Metodo encargado de actualizar un Neighbor
        /// </summary>
        /// <param name="time">Tiempo hasta que el link es valido</param>
        public void UpdateNeighbor(double time)
        {
            if (time >= DateTime.Now.Ticks)
                N_Status = OLSRConstants.SYM;
            else
                N_Status = OLSRConstants.NOT_SYM;
        }

        # region getters
        public IPAddress GetN_neighbor_iface_addr()
        {
            return N_neighbor_main_addr;
        }

        public int GetN_Status()
        {
            return N_Status;
        }
        #endregion
    }
}