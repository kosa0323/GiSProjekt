using System;
using System.Net;

namespace OLSR.OLSR.Packets.Messages.TC
{
    /// <summary>
    /// 
    /// Created by  APIF Moviquity S.A.
    /// 
    /// http://www.moviquity.com/
    /// 
    /// Clase encargada de almacenar toda la informacion de los mensajes TC 
    /// 
    /// Version: 1.1
    ///
    /// </summary>
    class Topology
    {
        # region Variables

        private readonly IPAddress T_destination_addr;
        private readonly IPAddress T_last_addr;
        private readonly int T_Sec;
        private long T_time;

        #endregion

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="dest">IP destino</param>
        /// <param name="last">IP por la que se llega al destino</param>
        /// <param name="ansn">numero ANSN</param>
        /// <param name="validTime">Tiempo de valided</param>
        public Topology(IPAddress dest, IPAddress last,int ansn, double validTime)
        {
            T_destination_addr = dest;
            T_last_addr = last;
            T_Sec = ansn;
            T_time = DateTime.Now.AddSeconds(validTime).Ticks;
        }

        /// <summary>
        /// Metodo encargado de actualizar el tiempo de validez del mensaje
        /// </summary>
        /// <param name="validTime"></param>
        public void Update(double validTime)
        {
            T_time = DateTime.Now.AddSeconds(validTime).Ticks;
        }

        #region getters & setters

        public int T_Sec_
        {
            get { return T_Sec; }
        }
        
        public IPAddress T_last_addr_
        {
            get { return T_last_addr; }
        }

        public IPAddress T_destination_addr_
        {
            get { return T_destination_addr; }
        }

        # endregion

    }
}