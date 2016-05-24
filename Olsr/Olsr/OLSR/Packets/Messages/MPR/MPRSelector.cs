using System;
using System.Net;

namespace OLSR.OLSR.Packets.Messages.MPR
{
    /// <summary>
    /// 
    /// Created by  APIF Moviquity S.A.
    /// 
    /// http://www.moviquity.com/
    /// 
    /// Clase encargada de controlar los nodos seleccionados como MPR's
    ///
    /// Version: 1.1
    ///
    /// </summary>
    class MPRSelector
    {
        # region Varibles
        private readonly IPAddress MS_main_addr;
        private long MS_time;
        #endregion

        /// <summary>
        /// Contructor de la clase
        /// </summary>
        /// <param name="mainAdd">Direccion IP</param>
        /// <param name="validTime">Tiempo de validez</param>
        public MPRSelector(IPAddress mainAdd, double validTime)
        {
            MS_main_addr = mainAdd;
            MS_time = DateTime.Now.AddSeconds(validTime).Ticks;
        }

        /// <summary>
        /// Metodo encargado de actualizar la validez de un MPR
        /// </summary>
        /// <param name="validTime">Tiempo de validez</param>
        public void Update(double validTime)
        {
            MS_time = DateTime.Now.AddSeconds(validTime).Ticks;
        }

        # region getters
        public long MS_time_
        {
            get { return MS_time; }
        }
        
        public IPAddress MS_main_addr_
        {
            get { return MS_main_addr; }
        }

        #endregion

    }
}