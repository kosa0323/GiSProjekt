using System;
using System.Collections;
using System.Net;
using OLSR.Configuration;
using OLSR.OLSR.Packets.Messages.Links;
using OLSR.OLSR.Packets.Messages.MPR;
using OLSR.OLSR.Packets.Messages.Neighbors;
using OLSR.OLSR.RoutingTable;
using OLSR.Screens;


namespace OLSR.OLSR.Packets.Messages.HELLO
{
    /// <summary>
    /// 
    /// Created by  APIF Moviquity S.A.
    /// 
    /// http://www.moviquity.com/
    /// 
    /// Clase encargada de formar mensajes del tipo HELLO (Ver seccion 6 RFC 3626)
    /// 
    ///   0                   1                   2                   3
    ///   0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
    ///  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///  |          Reserved             |     Htime     |  Willingness  |
    ///  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///  |   Link Code   |   Reserved    |       Link Message Size       |
    ///  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///  |                  Neighbor Interface Address                   |
    ///  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///  |                  Neighbor Interface Address                   |
    ///  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    /// 
    ///
    /// Version: 1.1
    ///
    /// </summary>
    class HelloMessage
    {
        private const int HELLO_MESSAGE_HEADER = 4;//Tamaño Reserved + Htime + Willingness
        
        /// <summary>
        /// Constructor de mensajes Hello
        /// </summary>
        /// 
        public static byte[] GenerateHelloMessage()
        {
            RemoveTime2HopNeighbors();
            //Comprobar si algun link a desaparecido
            var messageHello = new byte[0];

            messageHello = OLSRFunctions.AddReservedByte(messageHello);
            messageHello = OLSRFunctions.AddReservedByte(messageHello);
            messageHello = OLSRFunctions.GenerateHtime(messageHello);
            messageHello = OLSRFunctions.AddIntByte(OLSRConstants.WILL_DEFAULT, messageHello);
            messageHello = LinkCodeMessages(messageHello);

            RemoveLostLinks();
            
            RemoveMPRSelectorLinks();
            
            UpdateNodes();
            
            return messageHello;
        }

        /// <summary>
        /// Metodo que se recorre el listado de MPR's y borra todos los que no tengan validez
        /// </summary>
        private static void RemoveMPRSelectorLinks()
        {
            lock (OLSRParameters.MPRSelectorSet)
            {
                for (int i = OLSRParameters.MPRSelectorSet.Count - 1; i > -1; i--)
                {
                    var selector = (MPRSelector)OLSRParameters.MPRSelectorSet[i];
                    if (selector.MS_time_ < DateTime.Now.Ticks)
                    {
                        OLSRParameters.MPRSelectorSet.RemoveAt(i);
                    }
                }
                OLSRParameters.MPRSelectorSet.TrimToSize();
            }
        }

        /// <summary>
        /// Metodo que se recorre el Listado de links en busca de links perdidos y los borra
        /// </summary>
        private static void RemoveLostLinks()
        {
            lock (OLSRParameters.LinksList)
            {
                for (int i = OLSRParameters.LinksList.Count - 1; i > -1; i--)
                {
                    LinkSensing sensing = (LinkSensing)OLSRParameters.LinksList[i];
                    if (sensing.GetL_Time() < DateTime.Now.Ticks)
                    {
                        if (sensing.active)
                        {
                            sensing.TFLLN = DateTime.Now.Ticks;
                            sensing.PLP++;
                            sensing.consecutiveHello = 0;
                            LogWriter.GetInstance().AddText("Sensing with: "+ sensing.L_neighbor_iface_addr+ " PLP: "+ sensing.PLP );
                        }
                        if (sensing.PLP == 10)
                        {
                            sensing.PLP = 2;
                            LogWriter.GetInstance().AddText("Reset PLP of: " + sensing.L_neighbor_iface_addr );
                        }
                        sensing.active = false;

                        RemoveLostNeighbors(sensing.L_neighbor_iface_addr);
                        //OLSRParameters.LinksList.RemoveAt(i);
                    }
                }
                OLSRParameters.LinksList.TrimToSize();
            }
        }

        /// <summary>
        /// Metodo que se recorre el Listado de neighbors en busca de los perdidos por
        /// la ip del link
        /// </summary>
        /// <param name="address">IPAddress del link</param>
        private static void RemoveLostNeighbors(IPAddress address)
        {
            lock (OLSRParameters.NeighborList)
            {
                for (int i = OLSRParameters.NeighborList.Count - 1; i > -1; i--)
                {
                    Neighbor neig = (Neighbor)OLSRParameters.NeighborList[i];
                    if (neig.GetN_neighbor_iface_addr().Equals(address))
                    {
                        RemoveLost2HopNeighbors(neig.GetN_neighbor_iface_addr());
                        OLSRParameters.NeighborList.RemoveAt(i);
                    }
                }
                OLSRParameters.NeighborList.TrimToSize();
            }
        }

        /// <summary>
        /// Metodo que se recorre el Listado de 2 hop neighbors en busca de los perdidos por
        /// tiempo
        /// </summary>
        private static void RemoveTime2HopNeighbors()
        {
            
            lock (OLSRParameters.SecondHopNeighborList)
            {
                
                for (int i = OLSRParameters.SecondHopNeighborList.Count - 1; i > -1; i--)
                {

                
                    SecondHopNeighbor neig2hop = (SecondHopNeighbor)OLSRParameters.SecondHopNeighborList[i];
                    if (DateTime.Now.Ticks > neig2hop.GetN_Time())
                    {
                        OLSRParameters.SecondHopNeighborList.RemoveAt(i);
                    }
                    
                }
                
                OLSRParameters.SecondHopNeighborList.TrimToSize();
            }
        }

        /// <summary>
        /// Metodo que se recorre el Listado de 2 hop neighbors en busca de los perdidos por
        /// la ip del neighbor
        /// </summary>
        /// <param name="address">IPAddress del neighbor</param>
        private static void RemoveLost2HopNeighbors(IPAddress address)
        {
            lock (OLSRParameters.SecondHopNeighborList)
            {
                for (int i = OLSRParameters.SecondHopNeighborList.Count - 1; i > -1; i--)
                {
                    SecondHopNeighbor neig2hop = (SecondHopNeighbor)OLSRParameters.SecondHopNeighborList[i];
                    if (neig2hop.GetN_neighbor_main_addr().Equals(address))
                    {
                        OLSRParameters.SecondHopNeighborList.RemoveAt(i);
                    }
                    /*else if (neig2hop.GetN_2hop_addr().Equals(address))
                    {
                        OLSRParameters.SecondHopNeighborList.RemoveAt(i);
                    }*/
                }
                OLSRParameters.SecondHopNeighborList.TrimToSize();
            }
        }

        /// <summary>
        /// Metodo encargado de generar los mensajes entre las etiquetas LinkCode's
        /// </summary>
        /// <param name="message">byte[] con la informacion del mensaje</param>
        /// <returns>byte[] con el mensaje</returns>
        private static byte[] LinkCodeMessages(byte[] message)
        {
            //Comprobamos si hay nodos vecinos en la lista de links
            lock (OLSRParameters.LinksList)
            {
                if (OLSRParameters.LinksList.Count > 0)
                {   //Si hay nodos recorremos el listado y vamos generando un nuevo mensaje en
                    //por cada link que tenemos
                    foreach (LinkSensing sensing in OLSRParameters.LinksList)
                    {
                        message = OLSRFunctions.GenerateLinkCode(sensing.GetLinkType(), OLSRParameters.GetNeighborStatus(sensing.L_neighbor_iface_addr), message);
                        message = OLSRFunctions.AddReservedByte(message);

                        byte[] ips = sensing.L_neighbor_iface_addr.GetAddressBytes();
                        message = CalculateSizeMessage(message, ips.Length);

                        var msg = new byte[message.Length + ips.Length];
                        Array.Copy(message, msg, message.Length);
                        Array.Copy(ips, 0, msg, message.Length, ips.Length);

                        message = msg;
                    }
                }
            }
            return message;
        }

        /// <summary>
        /// Metodo encargado de calcular el tamaño del mensaje que estamos generando, entre LinkCode's
        /// </summary>
        /// <param name="message">byte array con la informacion del mensaje</param>
        /// <param name="AddSize">Tamaño en bytes</param>
        /// <returns>Nuevo byte[] con el mensaje, añadido el tamaño del mensaje 2bytes</returns>
        private static byte[] CalculateSizeMessage(byte[] message, int AddSize)
        {
            //Calculamos el tamaño, sabiendo que vamos a añadir 2 bytes por el tamaño del
            //mensaje + el numero de bytes (AddSize) de las interfaces - 4 bytes porque el
            //tamaño del mensaje se calcula entre etiquetas "Link Code"

            //int tam = 2 + message.Length - HELLO_MESSAGE_HEADER + AddSize;
            int tam = HELLO_MESSAGE_HEADER + AddSize;
            string myHex = String.Format("{0:X4}", tam);

            byte[] value = Converter.String2ByteArray(myHex);

            byte[] msg = new byte[message.Length + value.Length];
            Array.Copy(message, msg, message.Length);
            Array.Copy(value, 0, msg, message.Length, value.Length);

            return msg;
        }

        /// <summary>
        /// Metodo encargado de procesar la informacion del mensaje recibido
        /// </summary>
        /// <param name="message">Mensaje recibido</param>
        public static void ProcessMessage(Message message)
        {
            if (message.InfoMessage.Length > HELLO_MESSAGE_HEADER)
            {
                ArrayList linkCode = SplitLinkCodeMessages(message.InfoMessage, true);
                
                for (int x = 0; x < linkCode.Count; x++)
                {
                    var l = new Link((byte[])linkCode[x]);
                    ProcessLinkMessage(l, message.ValidTime, message.OriginatorAdd);
                }
            }
            else
                ProcessLinkMessage(null, message.ValidTime, message.OriginatorAdd);
        }

        /// <summary>
        /// Metodo recursivo encargado de separar el mensaje desde las etiquetas "LinkCode", la informacion
        /// se ira almacenando en un byte[] 
        /// </summary>
        /// <param name="message">byte[] con la informcion del mensaje completo</param>
        /// <param name="header">Boolean que indica si solamente estamos recibiendo la parte de
        /// las etiquetas LinkCode</param>
        /// <returns>Listado de byte[] con la informacion de cada uno de los links que vienen en el mensaje</returns>
        private static ArrayList SplitLinkCodeMessages(byte[] message, bool header)
        {
            var linkCodes = new ArrayList();

            if (header) //Comprobamos si tenemos que separar la cabecera del mensaje HELLO
            {
                //Reserved|Reserved|HTime|Willingness
                var links = new byte[message.Length - HELLO_MESSAGE_HEADER];

                Array.Copy(message, HELLO_MESSAGE_HEADER, links, 0, links.Length);

                message = links;
            }
            //Recuperamos el tamaño
            int size = message[2] & 0xFF;
            size <<= 8;
            size |= message[3] & 0xFF;

            if (size == message.Length)
                linkCodes.Add(message);
            else
            {
                var link = new byte[size];
                var linkList = new byte[message.Length - size];

                Array.Copy(message, 0, link, 0, size);
                Array.Copy(message, size, linkList, 0, linkList.Length);

                linkCodes.Add(link);
                linkCodes.AddRange(SplitLinkCodeMessages(linkList, false));

            }
            return linkCodes;
        }

        /// <summary>
        /// Metodo que se encarga de recorrer el listado del links conocidos y procesar
        /// la informacion recibida
        /// </summary>
        /// <param name="l">Link donde guardamos la informacion recibida</param>
        /// <param name="time">Tiempo de validez del mensaja</param>
        /// <param name="add">IP local</param>
        private static void ProcessLinkMessage(Link l, double time, IPAddress add)
        {
            bool exist = false;
            
            lock (OLSRParameters.LinksList)
            {
                foreach (LinkSensing ls in OLSRParameters.LinksList)
                {
                    if (ls.L_neighbor_iface_addr.Equals(add))
                    {
                        if (l != null)
                        {
                            ls.HelloMessageProcessing(l.link, time, l.ifaces_add);
                            NeighborProcessing(ls, add);
                            
                            if (l.link == OLSRConstants.LOST_LINK)
                            {
                                foreach (SecondHopNeighbor secondLostLink in OLSRParameters.SecondHopNeighborList)
                                {
                                    if (secondLostLink.GetN_2hop_addr().Equals(l.ifaces_add[0]) && secondLostLink.GetN_neighbor_main_addr().Equals(add))
                                    {
                                        OLSRParameters.SecondHopNeighborList.Remove(secondLostLink);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                SecondHopNeighborProcessing(ls, add, time, l.ifaces_add, l.neighbor);
                            }
                            MPRCompute.Compute();

                            if (l.link == OLSRConstants.SYM_LINK && l.neighbor == OLSRConstants.MPR_NEIGH)
                            {
                                lock (OLSRParameters.MPRSelectorSet)
                                {
                                    bool isMPR = false;
                                    foreach (MPRSelector selector in OLSRParameters.MPRSelectorSet)
                                    {
                                        if (selector.MS_main_addr_.Equals(add))
                                        {
                                            selector.Update(time);
                                            isMPR = true;
                                            break;
                                        }

                                    }
                                    if (!isMPR)
                                    {
                                        OLSRParameters.MPRSelectorSet.Add(new MPRSelector(add, time));
                                    }
                                }
                            }
                        }
                        exist = true;
                        break;
                    }
                }

                if (!exist)
                {
                    LinkSensing newLink = new LinkSensing(OLSRParameters.Originator_Addr, add, time);
                    lock (OLSRParameters.LinksList)
                    {
                        OLSRParameters.LinksList.Add(newLink);
                    }

                    //lock (OLSRParameters.NeighborList)
                    //{
                    //    OLSRParameters.NeighborList.Add(new Neighbor(newLink));
                    //}

                    MPRCompute.Compute();
                }
            }
        }

        /// <summary>
        /// Actualiza la lista de vecinos 
        /// </summary>
        /// <param name="ls">link que se actualizó</param>
        /// <param name="add">IP local</param>
        private static void NeighborProcessing(LinkSensing ls, IPAddress add)
        {
            lock (OLSRParameters.NeighborList)
            {
                foreach (Neighbor neig in OLSRParameters.NeighborList)
                {
                    if (neig.GetN_neighbor_iface_addr().Equals(add))
                    {
                        neig.UpdateNeighbor(ls.GetL_SYM_Time());
                        break;
                    }
                } 
            }
        }

        /// <summary>
        /// Actualiza la lista de vecinos de segundo salto
        /// </summary>
        /// <param name="ls">link que se actualizó</param>
        /// <param name="add">IP del Originator del HELLO</param>
        /// <param name="time">Validation time</param>
        /// <param name="listIfaces">Listado de Interfaces Neighbourd en el HELLO Message</param>
        /// <param name="neighbor">Neighbour Type</param>
        private static void SecondHopNeighborProcessing(LinkSensing ls, IPAddress add, double time, ArrayList listIfaces, int neighbor)
        {
            if (ls.GetL_SYM_Time() >= DateTime.Now.Ticks)
            {
                foreach (IPAddress address in listIfaces)
                {
                    if ((neighbor == OLSRConstants.MPR_NEIGH) || (neighbor == OLSRConstants.SYM_NEIGH))
                    {
                        if (!address.Equals(OLSRParameters.Originator_Addr))
                        {
                            bool exist = false;

                            lock (OLSRParameters.SecondHopNeighborList)
                            {
                                foreach (SecondHopNeighbor secondHopNeig in OLSRParameters.SecondHopNeighborList)
                                {
                                    //Si existe la tupla neighbor-2hopneighbor actualizo
                                    if (secondHopNeig.GetN_neighbor_main_addr().Equals(add) &&
                                        secondHopNeig.GetN_2hop_addr().Equals(address))
                                    {
                                        secondHopNeig.UpdateSecondHopNeighbor(time);
                                        exist = true;
                                        break;
                                    }

                                }
                                //Si no, creo una nueva
                                if (!exist)
                                {
                                    OLSRParameters.SecondHopNeighborList.Add(new SecondHopNeighbor(add, address, time));
                                }
                            }
                        }
                    }
                    else
                    {
                        lock (OLSRParameters.SecondHopNeighborList)
                        {
                            foreach (SecondHopNeighbor secondHopNeig in OLSRParameters.SecondHopNeighborList)
                            {
                                //Si existe la tupla neighbor-2hopneighbor se borra
                                if (secondHopNeig.GetN_neighbor_main_addr().Equals(add) && secondHopNeig.GetN_2hop_addr().Equals(address))
                                {
                                    OLSRParameters.SecondHopNeighborList.Remove(secondHopNeig);
                                    //OLSRParameters.LinksList.Remove(ls);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Metodo encargado de la actualizacion de los listados en la pantalla llamando al 
        /// ControlInvoker
        /// </summary>
        private static void UpdateNodes()
        {
            string[] nodes;
            lock (OLSRParameters.SecondHopNeighborList)
            {
                nodes = new string[OLSRParameters.SecondHopNeighborList.Count];

                for (int x = 0; x < OLSRParameters.SecondHopNeighborList.Count; x++)
                {
                    var neig = (SecondHopNeighbor)OLSRParameters.SecondHopNeighborList[x];
                    var ip = neig.GetN_2hop_addr();
                    nodes[x] = ip.ToString();
                }                
            }
            
            StartScreen.GetInstance().GetControlInvoker().Invoke(new MethodCallInvoker(Print2HopNeighbors), nodes);

            lock (OLSRParameters.NeighborList)
            {
                nodes = new string[OLSRParameters.NeighborList.Count];
                for (int x = 0; x < OLSRParameters.NeighborList.Count; x++)
                {
                    var neig = (Neighbor)OLSRParameters.NeighborList[x];
                    var ip = neig.GetN_neighbor_iface_addr();
                    nodes[x] = ip.ToString();
                }
            }
            
            StartScreen.GetInstance().GetControlInvoker().Invoke(new MethodCallInvoker(PrintNeighbors), nodes);

            lock (OLSRParameters.MPRSet)
            {
                nodes = new string[OLSRParameters.MPRSet.Count];
                for (int x = 0; x < OLSRParameters.MPRSet.Count; x++)
                {
                    var ip = (IPAddress)OLSRParameters.MPRSet[x];
                    nodes[x] = ip.ToString();
                }  
            }

            StartScreen.GetInstance().GetControlInvoker().Invoke(new MethodCallInvoker(PrintMPR), nodes);

        }

        /// <summary>
        /// Indicamos al formulario principal los vecinos que tiene que pintar
        /// </summary>
        /// <param name="arguments"></param>
        private static void PrintNeighbors(object[] arguments)
        {
            StartScreen.GetInstance().PrintNeighbors(arguments);
        }

        /// <summary>
        /// Indicamos al formulario principal los vecinos de 2 salto que tiene que pintar
        /// </summary>
        /// <param name="arguments"></param>
        private static void Print2HopNeighbors(object[] arguments)
        {
            StartScreen.GetInstance().Print2HopNeighbors(arguments);
        }

        /// <summary>
        /// Indicamos al formulario principal los MPR's que tiene que pintar
        /// </summary>
        /// <param name="arguments"></param>
        private static void PrintMPR(object[] arguments)
        {
            StartScreen.GetInstance().PrintMPRSet(arguments);
        }
    }
}