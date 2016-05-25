using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using System.Runtime.CompilerServices;
using OLSR.Configuration;
using OLSR.OLSR;
using OLSR.OLSR.Packets;
using OLSR.OLSR.Packets.Messages;
using OLSR.OLSR.Packets.Messages.HELLO;
using OLSR.OLSR.Packets.Messages.Neighbors;
using OLSR.OLSR.Packets.Messages.TC;

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
    /// Clase encargada de recibir la informacion broadcast
    ///
    /// Version: 1.2
    /// 
    /// </summary>
    class InfoReceiver
    {
        #region Variables

        //Instancia de la clase
        private static InfoReceiver instance = null;

        //Variable donde almacenamos los mensajes que vamos recibiendo
        private readonly ArrayList Queue;

        //Variable que contiene al hilo encargado de recibir los datos
        private Thread ReceiverThread;
        private bool RUN;

        //Variable que contiene al hilo encargado de leer los datos
        private Thread ReaderThread;
        private bool READER;

        //Variable donde guardamos el listado de mensajes recibidos
        private readonly List<PacketProcessing> DuplicateSet;

        //Variable que contiene al hilo encargado de eliminar los mensajes procesados por su tiempo
        private Thread CheckedThread = null;

        //Cambio Fran Variable que controla el Overflow del Buffer
        private byte[] overBuffer = new byte[0];

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        protected InfoReceiver()
        {
            Queue = new ArrayList();
            DuplicateSet = new List<PacketProcessing>();
        }

        /// <summary>
        /// Devuelve la instancia de la clase
        /// </summary>
        /// <returns></returns>
        public static InfoReceiver GetInstance()
        {
            if (instance == null)
                instance = new InfoReceiver();
            return instance;
        }

        /// <summary>
        /// Metodo encargado de iniciar los hilos que reciben y procesan los mensajes
        /// </summary>
        public void StartThread()
        {
            ReceiverThread = new Thread(ThreadRun) {IsBackground = true};
            RUN = true;
            ReceiverThread.Start();
        }

        /// <summary>
        /// Metodo encargado de parar los hilos encargados de recibir y procesar los paquetes
        /// recibidos
        /// </summary>
        public void StopThread()
        {
            if (RUN)
            {
                RUN = false;
                ReceiverThread.Abort();
            }

            if (READER)
            {
                READER = false;
                ReaderThread.Abort();
            }

            if (CheckedThread!=null)
                CheckedThread.Abort();

            ReceiverThread = null;
            ReaderThread = null;
            CheckedThread = null;
            instance = null;
        }

        /// <summary>
        /// Metodo ejecucion del hilo
        /// </summary>
        private void ThreadRun()
        {
            //Variable para obtener la IP de la máquína remitente
            var RemoteIP = new IPEndPoint(IPAddress.Broadcast, 0);
            //Variable para almacenar la IP temporalmente
            var ReceivedIP = (EndPoint)RemoteIP;

            //Mientras el inidicador de salida sea verdadero
            while (RUN)
            {
                int availableBytes = OLSRParameters.SocketControler.Available;
                if (availableBytes > 0)
                {
                    var saveOverBuffer = new byte[availableBytes];

                    OLSRParameters.SocketControler.ReceiveFrom(saveOverBuffer, ref ReceivedIP);

                    var BytesReceived = new byte[availableBytes + overBuffer.Length];

                    Array.Copy(overBuffer, 0, BytesReceived, 0, overBuffer.Length);
                    Array.Copy(saveOverBuffer, 0, BytesReceived, overBuffer.Length, BytesReceived.Length);
                    overBuffer = new byte[0];
                    ArrayList packets = SearchCompletePackets(BytesReceived);

                    for (int x = 0; x < packets.Count; x++)
                    {
                        PacketReader  packet = (PacketReader) packets[x];

                        for (int i = 0; i < packet.Messages.Count; i++)
                        {
                            var m = (Message)packet.Messages[i];
                            m.ReceivedAddress = ((IPEndPoint)ReceivedIP).Address;

                            int position = SearchMessageInQueue(m.TypeMessage, m.ReceivedAddress);

                            if( position != -1 )
                            {
                                lock (Queue)
                                {
                                    Queue.RemoveAt(position);
                                    Queue.Insert(position,m);
                                }
                            }
                            else
                            {
                                lock (Queue)
                                {
                                    Queue.Add(m);
                                }
                            }
                        }
                    }

                    if (!READER)
                    {
                        ReaderThread = new Thread(ProcessPacket) {IsBackground = true};
                        READER = true;
                        ReaderThread.Start();
                    }
                }
                Thread.CurrentThread.Join(10);//TODO - Paramos el hilo Comprobar si se puede reducir el tiempo
            }
        }

        private int SearchMessageInQueue(int message, IPAddress address)
        {
            lock (Queue)
            {
                for (int x = 0; x < Queue.Count; x++ )
                {
                    var msg = (Message)Queue[x];
                    if (msg.TypeMessage == message && msg.ReceivedAddress.Equals(address))
                        return x;
                }
            }
            return -1;
        }

        /// <summary>
        /// Metodo encargado de buscar el tipo de mensaje recibido y enviarlo a su clase determinada
        /// para procesar la informacion 
        /// </summary>
        private void ProcessPacket()
        {
            while (Queue.Count > 0)
            {
                Message msg;

                lock (Queue)
                {
                    msg = (Message)Queue[0];
                    Queue.RemoveAt(0);                    
                }

                try
                {
                    ProcessBasicMessage(msg);
                }
                catch (Exception ex)
                {
                    LogWriter.GetInstance().AddText("ERROR: "+ex.StackTrace); 
                    ex.GetType();
                }

                Thread.CurrentThread.Join(10);//TODO - Paramos el hilo Comprobar si se puede reducir el tiempo
            }

            READER = false;

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void ProcessBasicMessage(Message m)
        {
            if (!OLSRParameters.BlockedIPs.Contains(m.ReceivedAddress))//SOLO IPS NO BLOQUEADAS
            {
                if (m.TTL > 0 && !m.OriginatorAdd.Equals(OLSRParameters.Originator_Addr))
                {
                    //Buscar si el mensaje a sido procesado
                    PacketProcessing tuple = ExistProcessPacket(m.OriginatorAdd, m.MsgSequence);
                    if (tuple == null)
                    {
                        //Añadir PacketProcessing
                        tuple = new PacketProcessing(m.OriginatorAdd, m.MsgSequence, false);

                        switch (m.TypeMessage)
                        {
                            case OLSRConstants.HELLO_MESSAGE:            
                                HelloMessage.ProcessMessage(m);
                                break;
                            case OLSRConstants.TC_MESSAGE:
                                TopologyControlMessage.ProcessMessage(m);
                                break;
                            default:
                                break;
                        }


                        lock (DuplicateSet)
                        {
                            DuplicateSet.Add(tuple);
                        }

                    }
                    else
                    {
                        tuple.Update();
                    }

                    // Forwarding
                    DefaultForwarding(m);

                    if (DuplicateSet.Count == 1)
                    {
                        CheckedThread = new Thread(CheckedProcessedPackets) { IsBackground = true };
                        CheckedThread.Start();
                    }
                }
            }
        }

        /// <summary>
        /// Metodo encargado de buscar los paquetes completos recibidos
        /// </summary>
        /// <param name="info">byte[] datos del paquete</param>
        /// <returns>Lista de paquetes recibidos</returns>
        private ArrayList SearchCompletePackets(byte[] info)
        {
            var packs = new ArrayList();

            //Calculamos longitud del paquete
            int length = 0;
            length |= info[0] & 0xFF;
            length <<= 8;
            length |= info[1] & 0xFF;

            if (length > OLSRConstants.PACKET_HEADER_LENGTH)
            {
                if (length == info.Length)
                {
                    var p = new PacketReader(info);
                    packs.Add(p);
                }
                else if (length < info.Length)
                {

                    var packet = new byte[length];
                    var aux = new byte[info.Length - length];

                    Array.Copy(info, 0, packet, 0, length);
                    Array.Copy(info, length, aux, 0, info.Length - length);

                    var p = new PacketReader(packet);
                    packs.Add(p);

                    ArrayList other = SearchCompletePackets(aux);
                    if (other.Count > 0)
                        packs.AddRange(other);
                }
                else 
                {
                    overBuffer = info;
                }
            }

            return packs;
        }

        /// <summary>
        /// Metodo encargado de procesar la informacion del paquete recibido
        /// </summary>
        /// <param name="p">Datos del paquete</param>
        /// <param name="receiveAddr">Direccion de donde procede el paquete</param>
        //[MethodImpl(MethodImplOptions.Synchronized)]
        //private void ProcessBasicPacket(PacketReader p, IPAddress receiveAddr)
        //{
        //    for (int i = 0; i < p.Messages.Count; i++)
        //    {
        //        var m = (Message)p.Messages[i];

        //        m.ReceivedAddress = receiveAddr;

        //        if (m.TTL > 0 && !m.OriginatorAdd.Equals(OLSRParameters.Originator_Addr))
        //        {
        //            //Buscar si el mensaje a sido procesado
        //            PacketProcessing tuple = ExistProcessPacket(m.OriginatorAdd, m.MsgSequence);
        //            if (tuple == null)
        //            {
        //                //Añadir PacketProcessing
        //                tuple = new PacketProcessing(m.OriginatorAdd, m.MsgSequence, false);

        //                switch (m.TypeMessage)
        //                {
        //                    case OLSRConstants.HELLO_MESSAGE:
        //                        if (!OLSRParameters.BlockedIPs.Contains(m.OriginatorAdd))//SOLO IPS NO BLOQUEADAS
        //                            HelloMessage.ProcessMessage(m);
        //                        break;
        //                    case OLSRConstants.TC_MESSAGE:
        //                        TopologyControlMessage.ProcessMessage(m);
        //                        break;
        //                    default:
        //                        break;
        //                }


        //                lock (DuplicateSet)
        //                {
        //                    DuplicateSet.Add(tuple);
        //                }

        //            }
        //            else
        //            {
        //                tuple.Update();
        //            }

        //            // Forwarding
        //            DefaultForwarding(m);

        //            if (DuplicateSet.Count == 1)
        //            {
        //                CheckedThread = new Thread(CheckedProcessedPackets) { IsBackground = true };
        //                CheckedThread.Start();
        //            }
        //        }

        //    }
        //}

        /// <summary>
        /// Metodo encargado del reenvio de los mensajes
        /// </summary>
        /// <param name="msg">Mensaje que queremos reenviar</param>
        private void DefaultForwarding(Message msg)
        {
            //Si la interfaz de envio no esta en la lista de vecinos no hacemos nada

            lock (OLSRParameters.NeighborList)
            {
                if (SearchNeighbor(msg.ReceivedAddress) && msg.TypeMessage != OLSRConstants.HELLO_MESSAGE)
                {
                    
                    lock (DuplicateSet)
                    {
                        foreach (PacketProcessing processing in DuplicateSet) // Siempre va a existir la Tupla en la tabla
                        {
                            if (processing.D_Addr_.Equals(msg.OriginatorAdd) && processing.D_seq_num_ == msg.MsgSequence && !processing.D_retransmited_)
                            {
                                
                                lock (OLSRParameters.MPRSet)
                                {
                                    //Si la interfaz esta en el MPRSelectorSet y el ttl es mayor de 1 el mensaje es retransmitido
                                    if (OLSRParameters.MPRSet.Contains(msg.ReceivedAddress) && msg.TTL > 1)
                                    {
                                        //MSG puede transmitirse
                                        msg.TTL--;
                                        msg.Hop++;

                                        InfoSender.GetInstance().AddMessage2Queue(msg);
                                        //Enviamos msg sin modificar parametros de la cabecera
                                        processing.D_retransmited_ = true;
                                    }
                                }
                                processing.D_time_ = DateTime.Now.AddSeconds(OLSRConstants.DUP_HOLD_TIME).Ticks;
                            }
                        }
                    }
                }

            }


        }

        /// <summary>
        /// Metodo encargado de recorrer el listado de vecinos e informar si existe el vecino con la direccion
        /// IP recibida por parametro
        /// </summary>
        /// <param name="address">Direccion IP que queremos buscar</param>
        /// <returns>True si existe el vecino, falso si no existe</returns>
        private bool SearchNeighbor(IPAddress address)
        {
            bool exist = false;

            lock (OLSRParameters.NeighborList)
            {

                foreach (Neighbor neig in OLSRParameters.NeighborList)
                {
                    if (neig.GetN_neighbor_iface_addr().Equals(address))
                    {
                        exist = true;
                        break;
                    }
                }

            }

            return exist;
        }

        /// <summary>
        /// Hilo encargado de eliminar los paquetes en los que haya expirado su tiempo
        /// </summary>
        private void CheckedProcessedPackets()
        {
            while (DuplicateSet.Count > 0)
            {
                
                lock (DuplicateSet)
                {

                    foreach (PacketProcessing processing in DuplicateSet)
                    {
                        if (processing.D_time_ <= DateTime.Now.Ticks)
                        {
                            DuplicateSet.Remove(processing);
                            
                            break;
                        }
                    }
                }
                

                Thread.CurrentThread.Join(OLSRConstants.HELLO_INTERVAL*100);
            }
        }

        /// <summary>
        /// Metodo encargado de buscar si el mensaje recibido ha sido procesado o no
        /// </summary>
        /// <param name="add">Direccion destino del paquete</param>
        /// <param name="sequence">numero de secuencia del paquete</param>
        /// <returns>Devuele si el mensaje tiene q ser procesado o no</returns>
        private PacketProcessing ExistProcessPacket(IPAddress add, int sequence)
        {
            if (DuplicateSet != null)
            {     
                lock (DuplicateSet)
                {
                    
                    foreach (PacketProcessing processing in DuplicateSet)
                    {
                        if (processing.D_Addr_.Equals(add) && processing.D_seq_num_ == sequence)
                        {
                            return processing;
                        }
                    }
                    
                }
            }
            return null;
        }
    }
}