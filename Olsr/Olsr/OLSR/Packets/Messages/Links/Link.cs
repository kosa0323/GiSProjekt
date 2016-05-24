using System;
using System.Collections;
using System.Net;

namespace OLSR.OLSR.Packets.Messages.Links
{
    /// <summary>
    /// 
    /// Created by  APIF Moviquity S.A.
    /// 
    /// http://www.moviquity.com/
    /// 
    ///
    /// Version: 1.1
    ///
    /// </summary>
    class Link
    {

        # region variables
        //Variable donde guardamos el link type
        public int link { get; set; }
        //Variable donde guardamos el neighbor type
        public int neighbor { get; set; }
        //Listado donde almacenamos las interfaces que llegan en el mensaje
        public ArrayList ifaces_add { get; set; }

        #endregion

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="linkInfo">Byte [] con la informacion del link code</param>
        public Link(byte[] linkInfo)
        {
            SplitLinkInfo(linkInfo);
        }

        /// <summary>
        /// Metodo encargado de partir el mensaje LinkCode recibido y obtener su informacion
        /// </summary>
        /// <param name="info">byte[] con la informacion del linkCode</param>
        private void SplitLinkInfo(byte[] info)
        {
            int linkCode = info[0] & 0xFF;//Recuperamos LinkCode

            link = linkCode & 0x03;//Separamos Link type
            neighbor = (linkCode >> 2) & 0x03;//Separamos Neighbor type

            int size = info[2] & 0xFF;//Recuperamos tamaño
            size <<= 8;
            size |= info[3] & 0xFF;

            var ifaces = new byte[info.Length - 4];//Recuperamos Interfaces

            Array.Copy(info,4,ifaces,0,ifaces.Length);

            ifaces_add = NeighborIfacesParser(ifaces);
        }

        /// <summary>
        /// Metodo encargado de parsear el array de byte recibido a objetos ipAddress 
        /// almacenados en una lista
        /// </summary>
        /// <param name="ifaces">byte[] con la informacion de las interfaces</param>
        /// <returns>Listado de IPs</returns>
        private ArrayList NeighborIfacesParser(byte[] ifaces)
        {
            var list = new ArrayList();
            int nIfaces = ifaces.Length / 4;

            for (int x = 0; x < nIfaces; x++)
            {
                int iface = 4 * x;

                var ip = new[] { ifaces[0 + iface], ifaces[1 + iface], ifaces[2 + iface], ifaces[3 + iface] };
                var address = new IPAddress(ip);
                list.Add(address);
            }

            return list;
        }
    }
}