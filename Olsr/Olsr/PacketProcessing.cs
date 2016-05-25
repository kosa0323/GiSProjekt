using System;
using System.Net;

namespace OLSR.OLSR.Packets
{
    /// <summary>
    /// 
    /// Created by  APIF Moviquity S.A.
    /// 
    /// http://www.moviquity.com/
    /// 
    /// Clase encargada de almacenar los paquetes que han sido procesados
    ///
    /// Version: 1.1
    ///
    /// </summary>
    class PacketProcessing
    {
        # region Variables
        private readonly IPAddress D_Addr;//Originator address
        private readonly int D_seq_num;//message sequence number
        private long D_time;//tiempo en el que expira la tupla
        #endregion

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="address">Direccion IP que origina el paquete</param>
        /// <param name="sequence">Nuemro secuencia del paquete</param>
        /// <param name="retransited">Indicador de reenvio</param>
        public PacketProcessing(IPAddress address,int sequence, bool retransited)
        {
            D_Addr = address;
            D_seq_num = sequence;
            D_retransmited_ = retransited;

            D_time = DateTime.Now.AddSeconds(OLSRConstants.DUP_HOLD_TIME).Ticks;
        }

        /// <summary>
        /// Metodo encargado de actualizar la informacion del paquete
        /// </summary>
        public void Update()
        {
            D_time = DateTime.Now.AddSeconds(OLSRConstants.DUP_HOLD_TIME).Ticks;
        }

        # region Getters & Setters

        public IPAddress D_Addr_
        {
            get { return D_Addr; }
        }

        public int D_seq_num_
        {
            get { return D_seq_num; }
        }

        public bool D_retransmited_ { get; set; }

        public long D_time_ { get; set; }

        #endregion
    }
}