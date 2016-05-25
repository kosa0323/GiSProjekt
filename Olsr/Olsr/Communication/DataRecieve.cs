using System.Net;

namespace OLSR.Communication
{
    /// <summary>
    /// 
    /// Created by  APIF Moviquity S.A.
    /// 
    /// http://www.moviquity.com/
    /// 
    /// Developers:
    /// 
    ///     Alberto Martinez Garcia
    ///     Francisco Abril Bucero 
    ///     Jose Manuel Lopez Garcia
    /// 
    /// Clase donde se almacena la informcion recibida por broadcast y la interfaz de procedencia
    /// del mensaje
    ///
    /// Version: 1.1
    /// 
    /// </summary>
    class DataRecieve
    {
        # region Variables

        private IPAddress fromAddress = null;
        private byte[] data = null;

        #endregion

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="pAddress">Direccion de procedencia del paquete</param>
        /// <param name="pData">Informacion que contiene el paquete</param>
        public DataRecieve (IPAddress pAddress, byte [] pData)
        {
            data = pData;
            fromAddress = pAddress;
        }

        # region Getters

        public IPAddress GetFromAddress()
        {
            return fromAddress; 
        }

        public byte[] GetData()
        {
            return data;
        }

        #endregion

    }
}
