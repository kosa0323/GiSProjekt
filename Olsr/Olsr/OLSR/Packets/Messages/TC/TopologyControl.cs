using System;
using System.Threading;
using OLSR.Communication;
using OLSR.Configuration;

namespace OLSR.OLSR.Packets.Messages.TC
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
    class TopologyControl
    {
        #region "Variables"

        //Instacia de la clase
        private static TopologyControl Instance = null;

        //Hilo encargado de generar los mensajes
        private Thread Message_Thread;

        //Variable que nos informa del estado del hilo
        private bool RUN;

        #endregion

        /// <summary>
        /// Metodo que devuelve la instancia de la clase
        /// </summary>
        /// <returns></returns>
        public static TopologyControl GetInstance()
        {
            if (Instance == null)
                Instance = new TopologyControl();
            return Instance;
        }

        /// <summary>
        /// Metodo encargado de iniciar el hilo que genera los mensajes tipo
        /// TopologyControl
        /// </summary>
        public void StartThread()
        {
            Message_Thread = new Thread(ThreadRun) {IsBackground = true};
            RUN = true;
            Message_Thread.Start();
        }

        /// <summary>
        /// Metodo encargado de parar el hilo que genera los mensajes tipo
        /// TopologyControl
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
                if (OLSRParameters.MPRSelectorSet.Count > 0)
                {
                    try
                    {
                        byte[] messageTC = TopologyControlMessage.GenerateTCMessage(OLSRParameters.GetANSN());

                        if (messageTC != null && messageTC.Length > 0)
                        {
                            var msg = new Message(OLSRParameters.Originator_Addr, OLSRConstants.TC_MESSAGE, messageTC);

                            InfoSender.GetInstance().AddMessage2Queue(msg);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogWriter.GetInstance().AddText("ERROR TopologyControl: " + ex);
                    }
                }
                Message_Thread.Join(OLSRConstants.TC_INTERVAL * 1000);//Intervalo para enviar TopologyControl Message
            }
        }
    }
}