using System;
using System.Collections;
using OLSR.OLSR.Packets.Messages;

namespace OLSR.OLSR.Packets
{
    /// <summary>
    /// 
    /// Created by  APIF Moviquity S.A.
    /// 
    /// http://www.moviquity.com/
    ///
    /// Clase encargada de leer la informacion que llega en los paquetes OLSR 
    ///
    /// Version: 1.1
    ///
    /// </summary>
    class PacketReader
    {
        #region Variables
        
        private int length;

        public int Length
        {
            get { return length; }
        }
        private int sequenceNumber;

        public int SequenceNumber
        {
            get { return sequenceNumber; }
        }

        private ArrayList messages;

        public ArrayList Messages
        {
            get { return messages; }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data">byte[] con la informacion del paquete recibido</param>
        public PacketReader(byte[] data)
        {
            SplitPacket(data);
        }

        /// <summary>
        /// Metodo encargado de partir el paquete y cargar los mensajes
        /// </summary>
        /// <param name="info">byte[] con la informacion del paquete</param>
        private void SplitPacket(byte[] info)
        {
            length = info[0] & 0xFF;
            length <<= 8;
            length |= info[1] & 0xFF;

            sequenceNumber |= info[2] & 0xFF;
            sequenceNumber <<= 8;
            sequenceNumber |= info[3] & 0xFF;

            var msgs = new byte[info.Length-4];
            Array.Copy(info, 4, msgs, 0, msgs.Length);

            messages = ExtractMessages(msgs);
        }

        /// <summary>
        /// Buscamos y generamos listado de mensajes sabiendo que el tamaño del
        /// mensaje aparece en la posicion 2-3 del array recibido
        /// </summary>
        /// <param name="msgs">byte[] con la informacion de los mensajes</param>
        /// <returns>Listado de mensajes que llegan en el paquete</returns>
        private static ArrayList ExtractMessages(byte[] msgs)
        {
            var array = new ArrayList();

            int size = msgs[2] & 0xFF;
            size <<= 8;
            size |= msgs[3] & 0xFF;

            if(msgs.Length == size)
            {
                var newMsg = new Message(msgs);
                array.Add(newMsg);
            }
            else
            {
                var aux = new byte[size];
                var aux2 = new byte[msgs.Length - size];
                
                Array.Copy(msgs,0,aux,0,aux.Length);
                Array.Copy(msgs,aux.Length,aux2,0,aux2.Length);

                var newMsg = new Message(aux);
                array.Add(newMsg);

                array.AddRange(ExtractMessages(aux2));
            }
            return array;
        }
    }
}