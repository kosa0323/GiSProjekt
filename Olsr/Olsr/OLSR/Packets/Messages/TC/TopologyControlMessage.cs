using System;
using System.Collections;
using System.Net;
using OLSR.OLSR.Packets.Messages.Neighbors;

namespace OLSR.OLSR.Packets.Messages.TC
{
    /// <summary>
    /// 
    /// Created by  APIF Moviquity S.A.
    /// 
    /// http://www.moviquity.com/
    /// 
    /// Clase encargada de formar mensajes del tipo TC (Ver seccion 6 RFC 3626)
    /// 
    ///   0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
    ///  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///  |              ANSN             |           Reserved            |
    ///  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///  |               Advertised Neighbor Main Address                |
    ///  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///  |               Advertised Neighbor Main Address                |
    ///  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///  |                              ...                              |
    ///  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    /// 
    /// Version: 1.1
    ///
    /// </summary>
    class TopologyControlMessage
    {

        /// <summary>
        /// Constructor de mensajes TC
        /// </summary>
        /// <param name="secANSN">Numero de secuencia</param>
        public static byte[] GenerateTCMessage(int secANSN)
        {

            var messageTC = new byte[0];//Creamos nuevo array donde almacenamos la informacion del mensaje a enviar

            messageTC = OLSRFunctions.AddInt2Bytes(secANSN,messageTC);//Añadimos numero de se secuencia al mensaje

            messageTC = OLSRFunctions.AddReservedByte(messageTC);//Añadimos bytes reservados
            messageTC = OLSRFunctions.AddReservedByte(messageTC);//Añadimos bytes reservados

            messageTC = AddNeighbors(messageTC);//Añadimos lostado de vecinos

            return messageTC;

        }

        /// <summary>
        /// Metodo encargado de añadir el listado de interfaces vecinas 
        /// </summary>
        /// <param name="tc">byte[] donde añadimos las insterfaces</param>
        /// <returns>byte[] con las interfaces</returns>
        private static byte[] AddNeighbors(byte[] tc)
        {
            lock (OLSRParameters.NeighborList)
            {
                foreach (Neighbor neig in OLSRParameters.NeighborList)
                {
                    var ip = neig.GetN_neighbor_iface_addr();
                    tc = OLSRFunctions.AddIPAddress(ip, tc);
                }
            }
            

            return tc;
        }

        /// <summary>
        /// Metodo encargado de procesar la informacion del mensaje recibido
        /// </summary>
        /// <param name="message">Mensaje recibido</param>
        public static void ProcessMessage(Message message)
        {
            int ansn = RecoverANSN(message.InfoMessage);
            ArrayList neighbors = RecoverNeighbors(message.InfoMessage);

            lock (OLSRParameters.NeighborList)
            {
                if (ExistAddressInNeigh(message.ReceivedAddress))
                {

                    if (CheckValidTopology(ansn, message.OriginatorAdd))
                    {
                        
                        foreach (IPAddress neig in neighbors)
                        {
                            lock (OLSRParameters.TopologySet)
                            {
                                bool exist = false;
                                if (OLSRParameters.TopologySet.Count > 0)
                                {
                                    foreach (Topology topology in OLSRParameters.TopologySet)
                                    {
                                        if (topology.T_destination_addr_.Equals(neig) && topology.T_last_addr_.Equals(message.OriginatorAdd))
                                        {
                                            topology.Update(message.ValidTime);
                                            exist = true;
                                        }
                                    }
                                }
                                if (!exist)
                                {
                                    OLSRParameters.TopologySet.Add(new Topology(neig, message.OriginatorAdd, ansn,
                                                                                message.ValidTime));
                                }
                            }
                        }
                    }
                }    
            }
        }

        /// <summary>
        /// Metodo encargado de comprobar si existe la direcion IP recibida en el listado de vecinos
        /// </summary>
        /// <param name="receive">Direccion IP</param>
        /// <returns>bool Informado si existe un vecino con esa direccion IP</returns>
        private static bool ExistAddressInNeigh(IPAddress receive)
        {
            lock (OLSRParameters.NeighborList)
            {
                foreach (Neighbor neig in OLSRParameters.NeighborList)
                {
                    if (neig.GetN_neighbor_iface_addr().Equals(receive))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Metodo encargado de comprobar la validez de un mensaje TC, sabiendo que:
        /// 
        /// * T_last_add == originator_add && T_seq > ansn --> mensaje fuera de orden
        /// 
        /// * T_last_add == originator_add && T_seq < ansn --> se borra tupla
        /// 
        /// </summary>
        /// <param name="ansn">Numero de mensaje TC recibido</param>
        /// <param name="origAdd">IP de quien recibimos el mensaje</param>
        /// <returns>Indicador si el mensaje es valido o no</returns>
        private static bool CheckValidTopology(int ansn, IPAddress origAdd)
        {
            lock (OLSRParameters.TopologySet)
            {
                //borramos las tuplas
                for (int i = OLSRParameters.TopologySet.Count - 1; i > -1; i--)
                {
                    var topology = (Topology)OLSRParameters.TopologySet[i];
                    if (topology.T_last_addr_.Equals(origAdd) && topology.T_Sec_ < ansn)
                    {
                        OLSRParameters.TopologySet.RemoveAt(i);
                    }
                }

                OLSRParameters.TopologySet.TrimToSize();

                //Buscamos si el mensaje esta fuera de orden
                foreach (Topology topology in OLSRParameters.TopologySet)
                {
                    if (topology.T_last_addr_.Equals(origAdd) && topology.T_Sec_ > ansn)
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Metodo encargado de recuperar la informacion del listado de vecinos recibido
        /// </summary>
        /// <param name="info">byte array con la informacion del mensaje TC</param>
        /// <returns>Listado de Interfaces vecinas</returns>
        private static ArrayList RecoverNeighbors(byte[] info)
        {
            var list = new ArrayList();

            var ifaces = new byte[info.Length - 4];

            Array.Copy(info,4,ifaces,0,ifaces.Length);

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

        /// <summary>
        /// Metodo encargado de recuperar el numero de secuencia del mensaje TC
        /// </summary>
        /// <param name="info">byte array con la informacion del mensaje TC</param>
        /// <returns>Entero con el numero de secuencia</returns>
        private static int RecoverANSN(byte[] info)
        {
            int sequence = info[0] & 0xFF;
            sequence <<= 8;
            sequence |= info[1] & 0xFF;
            return sequence;
        }
    }
}