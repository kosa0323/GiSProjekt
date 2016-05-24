using System.Net;

namespace OLSR.OLSR.RoutingTable
{
    /// <summary>
    /// 
    /// Created by  APIF Moviquity S.A.
    /// 
    /// http://www.moviquity.com/
    ///
    /// Clase donde de almacena la informacion de la ruta 
    ///
    /// Version: 1.1
    ///
    /// </summary>
    class Route
    {
        # region Variables

        private readonly IPAddress R_dest_addr;
        private readonly IPAddress R_next_addr;
        private readonly IPAddress R_iface_addr;
        private readonly int R_dist;

        #endregion

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="dest">Direcion IP de Origen</param>
        /// <param name="next">Direcion IP de Gateway</param>
        /// <param name="dist">Distancia</param>
        /// <param name="iface">Interface</param>
        public Route(IPAddress dest, IPAddress next, int dist, IPAddress iface)
        {
            R_dest_addr = dest;
            R_next_addr = next;
            R_iface_addr = iface;
            R_dist = dist;
        }


        # region Getters & Setters

        public int R_dist_
        {
            get { return R_dist; }
        }

        public IPAddress R_iface_addr_
        {
            get { return R_iface_addr; }
        }

        public IPAddress R_next_addr_
        {
            get { return R_next_addr; }
        }

        public IPAddress R_dest_addr_
        {
            get { return R_dest_addr; }
        }

        #endregion

    }
}