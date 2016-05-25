using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using OLSR.Configuration;
using OLSR.OLSR;
using OLSR.OLSR.Packets;
using OLSR.OLSR.Packets.Messages;
using OLSR.OLSR.Packets.Messages.HELLO;
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
    /// Clase encargada de formar el paquete donde se van a enviar los mensajes
    /// (Ver seccion 3 RFC 3626)
    /// 
    ///   0                   1                   2                   3
    ///   0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
    ///  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///  |         Packet Length         |    Packet Sequence Number     |
    ///  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///  |  Message Type |     Vtime     |         Message Size          |
    ///  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///  |                      Originator Address                       |
    ///  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///  |  Time To Live |   Hop Count   |    Message Sequence Number    |
    ///  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///  |                                                               |
    ///  :                            MESSAGE                            :
    ///  |                                                               |
    ///  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    /// 
    ///
    /// Version: 1.1
    ///
    /// </summary>
    class InfoSender
    {
        #region Variables

        //Instancia de la clase
        private static InfoSender instance = null;

        //Variable donde se van a almacenar todos los mensajes sin enviar
        private readonly List<Message> Queue;

        //Hilo desde donde vamos a enviar los mensajes
        private Thread SenderThread;

        //Variable que nos informa del estado del hilo
        private bool RUN;

        //Variable donde almacenamos la ip de broadcast donde vamos a enviar los mensajes generados
        private readonly IPAddress ipBroadcast;

        //Variable donde almacenamos la direccion a la cual enviamos los mensajes - broadcast por defecto
        private readonly EndPoint DirDestino = null;

        #endregion     

        /// <summary>
        /// Constructor
        /// </summary>
        protected InfoSender()
        {
            Queue = new List<Message>();

            if (OLSRParameters.Originator_Addr != null)
            {
                byte[] ip = OLSRParameters.Originator_Addr.GetAddressBytes();
                ip[3] = 255;

                ipBroadcast = new IPAddress(ip);

                //Contiene la dirección de Broadcast y el puerto utilizado
                DirDestino = new IPEndPoint(ipBroadcast, OLSRConstants.OLRS_PORT);
            }

        }

        /// <summary>
        /// Devuelve la instancia de la clase
        /// </summary>
        /// <returns></returns>
        public static InfoSender GetInstance()
        {
            if (instance == null)
                instance = new InfoSender();
            return instance;
        }

        /// <summary>
        /// Metodo encargado de iniciar el hilo que genera los paquetes
        /// </summary>
        public void StartThread()
        {
            RUN = true;

            SenderThread = new Thread(Run) {IsBackground = true};
            
            SenderThread.Start();

            //Arrancamos todos los hilos para envio de mensajes
            Hello.GetInstance().StartThread();
            TopologyControl.GetInstance().StartThread();
        }

        /// <summary>
        /// Metodo encargado de parar el hilo que genera los paquetes
        /// </summary>
        public void StopThread()
        {
            if (RUN)
            {
                RUN = false;
                SenderThread.Abort();
            }

            //Paramos todos los hilos para envio de mensajes
            Hello.GetInstance().StopThread();
            TopologyControl.GetInstance().StopThread();
            
            SenderThread = null;
            instance = null;
            
        }

        /// <summary>
        /// Metodo encargado de la ejecucion del Hilo
        /// </summary>
        private void Run()
        {
            while (RUN)
            {
                if (Queue.Count > 0)
                {
                    
                    lock (Queue)
                    {    
                        var newQueue = new Message[Queue.Count];
                        Queue.CopyTo(newQueue);
                        Queue.Clear();

                        try
                        {
                            SendBroadcastMessage(PacketGenerator.GeneratePacket(newQueue, OLSRParameters.GetPacketSequence()));
                        }
                        catch (Exception ex)
                        {
                            LogWriter.GetInstance().AddText("ERROR InfoSender - Run: "+ ex);
                           
                        }
                    }

                }

                SenderThread.Join(1000);//Intervalo de tiempo con el que miramos si hay mensajes por enviar
            }
        }

        /// <summary>
        /// Metodo sincrono encargado de añadir mensajes al la cola de envio
        /// </summary>
        /// <param name="message">Nuevo mensaje que se desea enviar</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddMessage2Queue(Message message)
        {
            lock (Queue)
            {
                Queue.Add(message);   
            }
        }

        /// <summary>
        /// Metodo encargado del envio a la direccion de broadcast del mensaje generado
        /// </summary>
        /// <param name="packet">Array de bytes hexadecimal que vamos a enviar</param>
        private void SendBroadcastMessage (byte [] packet)
        {
            //Envía los datos
            try
            {
                OLSRParameters.SocketControler.SendTo(packet, packet.Length, SocketFlags.None, DirDestino);
            }
            catch (Exception ex)
            {
                LogWriter.GetInstance().AddText("ERROR InfoSender - SendBroadcastMessage : " + ex);
            }

        }
    }
}