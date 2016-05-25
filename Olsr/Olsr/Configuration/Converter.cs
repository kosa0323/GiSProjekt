using System;

namespace OLSR.Configuration
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
    ///
    /// Clase con los metodos comunes para convertir los datos que se envian en el mensaje
    /// 
    /// Version: 1.1
    /// 
    /// </summary>
    class Converter
    {
        /// <summary>
        /// Metodo que recibe una cadena en formato hexadecimal y nos devuelve un array con la
        /// informacion de dicha cadena
        /// </summary>
        /// <param name="HexString">Cadena Hexadecimal a transformar</param>
        /// <returns>byte [] con la informacion de la cadena</returns>
        public static byte[] String2ByteArray(string HexString)
        {
            
            int NumberChars = HexString.Length;
            
            byte[] bytes = new byte[NumberChars / 2];
            
            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(HexString.Substring(i, 2), 16); ///TODO - EXCEPCIÓN EN ESTA LINEA
            }
            return bytes;
        }

        /// <summary>
        /// Convierte un numero entero en una cadena con su valor en hexadecimal
        /// </summary>
        /// <param name="IntValue">Numero Entero a convertir</param>
        /// <returns>Cadena con el valor exadecimal del numero pasado</returns>
        public static string Int2Hex(int IntValue)
        {
            return IntValue.ToString("X");
        }

        /// <summary>
        /// Convierte una cadena hexadecimal en un numero entero
        /// </summary>
        /// <param name="HexString">Cadena Hexadecimal a transformar</param>
        /// <returns>Numero Entero con el valor de la cadena pasada </returns>
        public static int String2Int(string HexString)
        {
            return int.Parse(HexString, System.Globalization.NumberStyles.HexNumber);
        }

        /// <summary>
        /// Metodo que convierte un array de bytes a una cadena en hexadecimal
        /// </summary>
        /// <param name="BytesValue">Array de bytes a convertir</param>
        /// <returns>cadena con informacion del array en hexadecimal</returns>
        public static string ByteArray2String(byte[] BytesValue)
        {
            string strHex = "";

            foreach (byte b in BytesValue)
                strHex = strHex + String.Format("{0:x2}", (uint)b);

            return strHex;
        }
    }
}