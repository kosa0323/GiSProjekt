using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

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
    /// Clase encargada de generar un fichero de logs
    /// 
    /// Version: 1.1
    ///
    /// </summary>
    class LogWriter
    {
        private static readonly string path = @"" + Application.StartupPath;//Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
        private static LogWriter instance = null;

        private LogWriter()
        {

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static LogWriter GetInstance()
        {
            if (instance == null)
                instance = new LogWriter();
            return instance;
        }

        //[MethodImpl(MethodImplOptions.Synchronized)]
        public void AddText(string log)
        {
            DateTime date = DateTime.Now;

            try
            {
                var sw = new StreamWriter(path + "\\Log_" + date.Day + "_" + date.Month + "_" + date.Year + ".txt", true);
                sw.WriteLine("[" + date.Day + "-" + date.Month + "-" + date.Year + " " + date.Hour + ":" + date.Minute + ":" + date.Second + "] " + log);

                sw.Close();
            }
            catch (IOException e)
            {
                e.GetType();
            }

        }

    }
}