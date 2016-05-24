using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using OLSR.OLSR.Packets.Messages.Neighbors;

namespace OLSR.OLSR
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
    /// Clase donde se registran todos las variables necesarias para la aplicacion OLSR
    ///
    /// Version: 1.1
    ///
    /// </summary>
    class OLSRParameters
    {
        public static Socket SocketControler;

        public static IPAddress Originator_Addr;

        public const byte Reserved = 0x00;//Atributo constante

        public static ArrayList TopologySet = new ArrayList();//Listado de TCs

        public static ArrayList LinksList = new ArrayList();//Listado de interfaces proximas

        public static ArrayList NeighborList = new ArrayList();//Listado de vecinos

        public static ArrayList SecondHopNeighborList = new ArrayList();//Listado de vecinos de segundo salto

        public static ArrayList MPRSet = new ArrayList();//Listado de MPRs
        public static ArrayList MPRSelectorSet = new ArrayList();//Listado de interfaces que me sececcionan como MPR

        public static ArrayList RoutingSet = new ArrayList();//Tabla de Rutas

        public static ArrayList BlockedIPs = new ArrayList();//Ips bloqueadas para pruebas

        public static String Language = null; //Idioma

        //aleatorio de 00 00 a FF FF
        private static readonly Random Rand = new Random();
        private static int PackSequence = Rand.Next(4096, 65535);
        private static int MsgSequence = Rand.Next(4096, 65535);
        private static int ANSN = Rand.Next(4096, 65535);

        /// <summary>
        /// Metodo encargado de generar el numero de secuencia del paquete
        /// </summary>
        /// <returns>Entero con el valor del numero</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static int GetPacketSequence()
        {
            PackSequence = PackSequence + 1;
            if (PackSequence > 65535)
                PackSequence = 4096;
            return PackSequence;
        }

        /// <summary>
        /// Metodo encargado de generar el numero de secuencia del mensaje
        /// </summary>
        /// <returns>Entero con el valor del numero</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static int GetMessageSequence()
        {
            MsgSequence = MsgSequence + 1;
            if (MsgSequence > 65535)
                MsgSequence = 4096;
            return MsgSequence;
        }

        /// <summary>
        /// Metodo encargado de generar ANSN para los mensajes
        /// </summary>
        /// <returns>Entero con el valor del numero</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static int GetANSN()
        {
            ANSN = ANSN + 1;
            if (ANSN > 65535)
                ANSN = 4096;
            return ANSN;
        }

        /// <summary>
        /// Metodo encargado de devolver el valor del estado del vecino
        /// </summary>
        /// <param name="add">Direccion ip del vecino</param>
        /// <returns>Entero con el valor del estado del vecino</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static int GetNeighborStatus(IPAddress add)
        {
            lock (MPRSet)
            {
                if (MPRSet.Contains(add))
                    return OLSRConstants.MPR_NEIGH;    
            }

            lock (NeighborList)
            {
                foreach (Neighbor neig in NeighborList)
                {
                    if (neig.GetN_neighbor_iface_addr().Equals(add))
                    {
                        if (neig.GetN_Status() == OLSRConstants.SYM)
                            return OLSRConstants.SYM_NEIGH;
                        break;
                    }
                }
            }

            return OLSRConstants.NOT_NEIGH;
        }
    }
}