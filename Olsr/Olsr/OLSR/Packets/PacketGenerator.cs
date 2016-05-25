using System;
using OLSR.Configuration;
using OLSR.OLSR.Packets.Messages;

namespace OLSR.OLSR.Packets
{
    /// <summary>
    /// 
    /// Created by  APIF Moviquity S.A.
    /// 
    /// http://www.moviquity.com/
    /// 
    /// Clase encargada de gestionar los paquetes antes de enviarlos y cuando
    /// los recibimos
    /// 
    /// Los paquetes estan formados por:
    /// 
    ///     * Header --> Packet length + Packet Sequence Number  [ 32bits (4bytes) ]
    ///     * Messages
    /// 
    ///
    /// Version: 1.1
    ///
    /// </summary>
    class PacketGenerator
    {
        //Tamaño de la Cabecera del Packete
        private const int PACKET_HEADER_LENGTH = 4;

        /// <summary>
        /// Metodo estatico encargado de generar un paquete con la informacion de los mensajes
        /// recibidos en la cola de envio
        /// </summary>
        /// <param name="queue">Mensajes encolados para el envio</param>
        /// <param name="sequence">Numero secuencia paquetes</param>
        /// <returns>byte[] con la informacion del paquete y los mensajes</returns>
        public static byte[] GeneratePacket(Message[] queue, int sequence)
        {
            byte[] messages = Convert2ByteArray(queue);

            byte[] packet = CalculatePacketSize(messages.Length);

            packet = OLSRFunctions.AddInt2Bytes(sequence, packet);

            byte[] aux = new byte[packet.Length + messages.Length];

            Array.Copy(packet, aux, packet.Length);
            Array.Copy(messages, 0, aux, packet.Length, messages.Length);
            return aux;
        }
        
        /// <summary>
        /// Metodo encargado de transformar el listado de mensajes 
        /// </summary>
        /// <param name="msgList"></param>
        /// <returns></returns>
        private static byte[] Convert2ByteArray(Message[] msgList )
        {
            byte[] packet = new byte[0];

            foreach (Message message in msgList)
            {
                //Message type
                packet = OLSRFunctions.AddIntByte(message.TypeMessage, packet);
                // Vtime
                packet = OLSRFunctions.AddValidTime(message.ValidTime, packet);
                //Message size
                packet = OLSRFunctions.AddInt2Bytes(message.MsgSize, packet);
                //Origintor Address
                packet = OLSRFunctions.AddIPAddress(message.OriginatorAdd, packet);
                //TTL
                packet = OLSRFunctions.AddIntByte(message.TTL, packet);
                //HOP
                packet = OLSRFunctions.AddIntByte(message.Hop, packet);
                //MessageSec
                packet = OLSRFunctions.AddInt2Bytes(message.MsgSequence, packet);
                //Mess
                byte[] pack = new byte[packet.Length + message.InfoMessage.Length];

                Array.Copy(packet, pack, packet.Length);
                Array.Copy(message.InfoMessage,0,pack,packet.Length,message.InfoMessage.Length);
                
                packet = pack;
            }

            return packet;
        }

        /// <summary>
        /// Metodo encargado de calcular el tamaño del paquete que estamos generando 
        /// </summary>
        /// <param name="tamPackets">Tamaño de los paquetes que se quieren enviar</param>
        /// <returns>Nuevo byte[]</returns>
        private static byte[] CalculatePacketSize(int tamPackets)
        {
            int tam = PACKET_HEADER_LENGTH + tamPackets;

            string myHex = String.Format("{0:X4}", tam);

            byte[] value = Converter.String2ByteArray(myHex);

            return value;
        }

    }
}