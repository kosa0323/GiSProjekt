using System;
using System.Net;
using System.Runtime.CompilerServices;
using OLSR.Configuration;

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
    /// Clase donde se encuentran todas las funciones necesarias para el OLSR
    ///
    /// Version: 1.1
    ///
    /// </summary>
    class OLSRFunctions
    {
        /// <summary>
        /// Metodo encargado de añadir el numero de secuencia pasado en el array recibido 
        /// </summary>
        /// <param name="sec">Secuence Number </param>
        /// <param name="array">byte[] donde añadimos el numero de secuencia</param>
        /// <returns>Nuevo byte[]</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static byte[] AddInt2Bytes(int sec, byte[] array)
        {
            string myHex = String.Format("{0:X4}", sec);//Pasamos el numero entero a hexadecimal

            byte[] value = Converter.String2ByteArray(myHex);//Obtenemos el array de bytes del hexadecimal

            //Creamos un nuevo array y lo concatenamos el array
            byte[] nArray = new byte[array.Length + value.Length];

            Array.Copy(array, nArray, array.Length);
            Array.Copy(value, 0, nArray, array.Length, value.Length);

            return nArray;
        }

        /// <summary>
        /// Metodo que añade un byte pasado como entero en un byte[]
        /// </summary>
        /// <param name="nByte">Entero a añadir en el array</param>
        /// <param name="array">byte[] donde añadimos el nuevo byte</param>
        /// <returns>byte[] con el entero añadido</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static byte[] AddIntByte(int nByte, byte[] array)
        {
            var nArray = new byte[array.Length + 1];

            Array.Copy(array, nArray, array.Length);
            
            nArray[nArray.Length - 1] = (byte)nByte;

            return nArray;
        }

        /// <summary>
        /// Metodo que añade un IPAddress a byte[]
        /// </summary>
        /// <param name="ipAdd">Direccion ip a añadir</param>
        /// <param name="array">byte[] donde añadimos la direccion desde la que se genera el mensaje</param>
        /// <returns>Nuevo byte[]</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static byte[] AddIPAddress(IPAddress ipAdd, byte[] array)
        {
            byte[] ip = ipAdd.GetAddressBytes();

            var nArray = new byte[array.Length + ip.Length];

            Array.Copy(array, 0, nArray, 0, array.Length);
            Array.Copy(ip, 0, nArray, array.Length, ip.Length);

            return nArray;
        }

        /// <summary>
        /// Siguendo la formula T = C*(1+a/16)*2^b y conocido el tiempo (T) calcula el 
        /// valor de 'a' y 'b' sabiendo :
        /// 
        /// T/C=2^b ---> b = Log(T/C)/Log2
        /// 
        /// a = 16*(T/(C*(2^b))-1)
        /// 
        /// Ver punto 18.3 RFC3626
        /// 
        /// </summary>
        /// <param name="vTime">tiempo</param>
        /// <param name="packet">byte[] donde añadimos el nuevo vtime</param>
        /// <returns>Nuevo byte[]</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static byte[] AddValidTime(double vTime, byte[] packet)
        {

            double b = Math.Log(vTime / OLSRConstants.C) / Math.Log(2);
            int bEnt = (int)b;
            double a = 16 * (vTime / (OLSRConstants.C * (Math.Pow(2, bEnt))) - 1);

            int aEnt = (int)a;

            if (aEnt < 0)
                aEnt = aEnt * -1;

            if (aEnt >= 16)
            {
                aEnt = 0;
                bEnt++;
            }

            byte[] pack = new byte[packet.Length + 1];

            Array.Copy(packet, pack, packet.Length);

            byte value = (byte)(((aEnt & 0x0F) << 4) | (bEnt & 0x0F));

            pack[pack.Length - 1] = value;

            return pack;
        }

        /// <summary>
        /// Metodo encargado de añadir un byte reservado
        /// </summary>
        /// <param name="array">byte array con la informacion del mensaje</param>
        /// <returns>Nuevo byte[] con el byte reservado añadido</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static byte[] AddReservedByte(byte[] array)
        {
            var nArray = new byte[array.Length + 1];
            Array.Copy(array, nArray, array.Length);
            nArray[nArray.Length - 1] = OLSRParameters.Reserved;
            return nArray;
        }

        /// <summary>
        /// Siguendo la formula T = C*(1+a/16)*2^b y conocido el tiempo (T) calcula el 
        /// valor de 'a' y 'b' sabiendo :
        /// 
        /// T/C=2^b ---> b = Log(T/C)/Log2
        /// 
        /// a = 16*(T/(C*(2^b))-1)
        /// 
        /// Ver punto 18.3 RFC3626
        /// 
        /// </summary>
        /// <param name="array">byte array con la informacion del mensaje</param>
        /// <returns>Nuevo byte[] con el mensaje, añadido el byte Htime</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static byte[] GenerateHtime(byte[] array)
        {
            double b = Math.Log(OLSRConstants.HELLO_INTERVAL / OLSRConstants.C) / Math.Log(2);
            var bEnt = (int)b;
            double a = 16 * (OLSRConstants.HELLO_INTERVAL / (OLSRConstants.C * (Math.Pow(2, bEnt))) - 1);

            var aEnt = (int)a;

            if (aEnt < 0)
                aEnt = aEnt * -1;

            if (aEnt >= 16)
            {
                aEnt = 0;
                bEnt++;
            }

            var nArray = new byte[array.Length + 1];
            Array.Copy(array, nArray, array.Length);
            nArray[nArray.Length - 1] = (byte)(((aEnt & 0x0F) << 4) | (bEnt & 0x0F));
            return nArray;
        }

        /// <summary>
        /// Metodo encargado de formar el Link code conociendo el Link type y Neighbor type
        /// </summary>
        /// <param name="Neighbor">Entero con la informacion de los vecinos</param>
        /// <param name="Link">Entero con la informacion del tipo de link</param>
        /// <param name="array">byte array con la informacion del mensaje</param>
        /// <returns>Nuevo byte[] con el mensaje, añadido el byte LinkCode</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static byte[] GenerateLinkCode(int Link, int Neighbor, byte[] array)
        {
            var nArray = new byte[array.Length + 1];
            Array.Copy(array, nArray, array.Length);
            nArray[nArray.Length - 1] = (byte)(((Neighbor & 0x03) << 2) | (Link & 0x03));
            return nArray;
        }

    }
}