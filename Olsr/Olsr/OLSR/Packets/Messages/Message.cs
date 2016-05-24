using System;
using System.Net;

namespace OLSR.OLSR.Packets.Messages
{
    /// <summary>
    /// 
    /// Created by  APIF Moviquity S.A.
    /// 
    /// http://www.moviquity.com/
    /// 
    /// Clase donde almacenamos la informacion y el tipo de mensaje que enviamos o recibimos
    /// 
    /// El mensaje esta formado por :
    /// 
    ///     * Header --> MsgType + vtime + MsgSize + OriginatorAdd + TTL + HopCount +
    ///                  Message Sequence Number [ 96 bits | 12 bytes ]
    /// 
    ///     * Message
    /// 
    ///
    /// Version: 1.1
    ///
    /// </summary>
    public class Message
    {
        #region Variables

        //Tamaño cabecera del mensaje
        private const int MESSAGE_HEADER_LENGTH = 12;
        
        //Variable que nos indica el tipo del mensaje
        public int TypeMessage { get; set; }
        //Variable que nos indica del tiempo de validez del mensaje
        public double ValidTime { get; set; }
        //Variable con el tamaño del mensaje
        public int MsgSize { get; set; }
        //Variable donde almacenamos la direccion ip de quien genera el mensaje
        public IPAddress OriginatorAdd { get; set; }
        //Variable que no indica el tiempo de vida del mensaje
        public int TTL { get; set; }
        //Variable que nos indica el numero de saltos del mensaje
        public int Hop { get; set; }
        //Numero de secuencia del mensaje
        public int MsgSequence { get; set; }
        //byte[] con la informacion del mensaje generado(HELLO,TC)
        public byte[] InfoMessage { get; set; }

        //Variable donde almacenamos la direccion ip de quien genera el mensaje
        public IPAddress ReceivedAddress { get; set; }

        #endregion

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="add">Direccion IP desde la que se genera el mensaje</param>
        /// <param name="type">Tipo de mensaje generado</param>
        /// <param name="msg">Informacion del mensaje generado</param>
        public Message(IPAddress add, int type, byte[] msg)
        {

            OriginatorAdd = add;
            TypeMessage = type;
            InfoMessage = msg;
            
            MsgSequence = OLSRParameters.GetMessageSequence();

            MessageGenerator();
        }

        /// <summary>
        /// Metodo encargado de dar valor a las variables de la cabecera del mensaje 
        /// conociendo el tipo de mensaje que se va a enviar
        /// </summary>
        private void MessageGenerator()
        {
            switch (TypeMessage)
            {
                case OLSRConstants.HELLO_MESSAGE:
                    ValidTime = OLSRConstants.NEIGHB_HOLD_TIME;
                    TTL = 1;
                    break;
                case OLSRConstants.TC_MESSAGE:
                    ValidTime = OLSRConstants.TOP_HOLD_TIME;
                    TTL = 255;
                    break;
                default:
                    break;
            }

            Hop = 0;

            CalculateMessageSize();

        }

        /// <summary>
        /// Metodo encargado de calcular el tamaño del mensaje que estamos enviando entre MessageType 
        /// </summary>
        private void CalculateMessageSize()
        {
            MsgSize = MESSAGE_HEADER_LENGTH + InfoMessage.Length;
        }

        /// <summary>
        /// constructor al que se le pas el tipo de mensaje y el mensaje
        /// </summary>
        /// <param name="Info">Datos del mensaje</param>
        public Message(byte[] Info)
        {
            SplitMessages(Info);
        }

        /// <summary>
        /// Metodo encargado de separar los mensajes 
        /// </summary>
        /// <param name="info">byte[] con los datos del mensaje</param>
        private void SplitMessages(byte[] info)
        {
            TypeMessage = info[0] & 0xFF;

            ValidTime = CalculateValidTime(info[1] & 0x0F, (info[1] >> 4) & 0x0F);

            MsgSize = info[2] & 0xFF;
            MsgSize <<= 8;
            MsgSize |= info[3] & 0xFF;

            var ip = new[] { info[4], info[5], info[6], info[7] };
            OriginatorAdd = new IPAddress(ip);

            TTL = info[8] & 0xFF;

            Hop = info[9] & 0xFF;

            MsgSequence = info[10] & 0xFF;
            MsgSequence <<= 8;
            MsgSequence |= info[11] & 0xFF;

            InfoMessage = new byte[info.Length - 12];

            Array.Copy(info, 12, InfoMessage, 0, InfoMessage.Length);

        }

        /// <summary>
        /// Metodo encargado de calcular el tiempo de validez
        /// </summary>
        /// <param name="minValue">Entero con el valor de los 4 bits ultimos</param>
        /// <param name="maxValue">Entero con el valor de los 4 bits primeros</param>
        /// <returns></returns>
        private static double CalculateValidTime(int minValue, int maxValue)
        {
            return OLSRConstants.C * (1 + (maxValue / 16F)) * Math.Pow(2, minValue);
        }

    }
}