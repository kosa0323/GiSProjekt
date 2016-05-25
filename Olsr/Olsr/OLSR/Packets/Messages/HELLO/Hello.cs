using System.Collections;
using System.Threading;
using System;
using OLSR.Communication;
using OLSR.Configuration;
using OLSR.OLSR.RoutingTable;

namespace OLSR.OLSR.Packets.Messages.HELLO
{
    /// <summary>
    /// 
    /// Created by  APIF Moviquity S.A.
    /// 
    /// http://www.moviquity.com/
    /// 
    /// Hilo encargado de generar los mensajes hello
    ///
    /// Version: 1.1
    ///
    /// </summary>
    class Hello
    {
        //Instacia de la clase
        private static Hello Instance = null;

        //Hilo encargado de generar los mensajes
        private Thread Message_Thread;

        //Variable que nos informa del estado del hilo
        private bool RUN;

        /// <summary>
        /// Metodo que devuelve la instancia de la clase
        /// </summary>
        /// <returns></returns>
        public static Hello GetInstance()
        {
            if (Instance == null)
                Instance = new Hello();
            return Instance;
        }

        /// <summary>
        /// Metodo encargado de iniciar el hilo que genera los mensajes tipo
        /// HELLO
        /// </summary>
        public void StartThread()
        {
            Message_Thread = new Thread(ThreadRun) {IsBackground = true};
            RUN = true;
            Message_Thread.Start();
        }

        /// <summary>
        /// Metodo encargado de parar el hilo que genera los mensajes tipo
        /// HELLO
        /// </summary>
        public void StopThread()
        {
            if (RUN)
            {
                RUN = false;
                Message_Thread.Abort();
            }

            Message_Thread = null;
            Instance = null;
        }

        /// <summary>
        /// Metodo encargado de la ejecucion del Thread
        /// </summary>
        private void ThreadRun()
        {
            while (RUN)
            {
                try
                {
                    byte[] messageHello = HelloMessage.GenerateHelloMessage();

                    if (messageHello != null && messageHello.Length > 0)
                    {

                        var msg = new Message(OLSRParameters.Originator_Addr, OLSRConstants.HELLO_MESSAGE, messageHello);

                        ArrayList newRoutes = RoutingTableCalculation.GetInstance().CalculateTableRoute();
                        RoutingTableCalculation.GetInstance().ModifyRoutingTable(newRoutes);

                        InfoSender.GetInstance().AddMessage2Queue(msg);

                    }
                }
                catch (Exception ex)
                {
                    LogWriter.GetInstance().AddText("ERROR Hello - ThreadRun : " + ex);
                }

                Message_Thread.Join(OLSRConstants.HELLO_INTERVAL * 1000);//Intervalo para enviar HELLO Message
            }
        }
    }
}