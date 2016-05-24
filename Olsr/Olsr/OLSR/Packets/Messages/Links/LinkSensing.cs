using System;
using System.Collections;
using System.Net;
using OLSR.Configuration;
using OLSR.OLSR.Packets.Messages.Neighbors;

namespace OLSR.OLSR.Packets.Messages.Links
{
    /// <summary>
    /// 
    /// Created by  APIF Moviquity S.A.
    /// 
    /// http://www.moviquity.com/ 
    ///
    /// Version: 1.1
    ///
    /// </summary>
    class LinkSensing
    {
        #region variables
        //Direccion IP que a recibido la informacion del mensaje HELLO
        private readonly IPAddress L_local_iface_addr;
        
        //Direccion IP desde la que se envia el mensaje HELLO
        public IPAddress L_neighbor_iface_addr { get; set; }

        private long L_SYM_Time;
        private long L_ASYM_Time;
        private long L_Time;
        public long TFLLN = 0;
        public int PLP = 1;
        public int consecutiveHello = 0;
        public int consecutiveHelloTFLLN = 0;
        public bool active = false;
        private int CONSECUTIVE_HELLO = 60;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="localAdd">Cadena con la direccion ip que ha recibido el mensaje</param>
        /// <param name="originatorAdd">Cadena con la direccion ip desde la que se ha enviado el mensaje</param>
        /// <param name="vTime">Tiempo de validez del mensaje</param>
        public LinkSensing(IPAddress localAdd, IPAddress originatorAdd, double vTime)
        {
            active = true;

            L_local_iface_addr = localAdd;
            L_neighbor_iface_addr = originatorAdd;

            L_SYM_Time = DateTime.Now.AddSeconds(- 1).Ticks;

            L_Time = DateTime.Now.AddSeconds(vTime).Ticks;

            PLP = 1;
            consecutiveHello = 0;
            
            HelloMessageProcessing(OLSRConstants.UNSPEC_LINK,vTime,new ArrayList());

        }
        
        /// <summary>
        /// Metodo encargado de procesar la informacion del mensaje HELLO recibida
        /// </summary>
        /// <param name="LinkValue">Valor del Link type</param>
        /// <param name="vTime">Tiempo de validez del mensaje</param>
        /// <param name="ifaces">Listado de interfaces del mensaje hello</param>
        public void HelloMessageProcessing(int LinkValue, double vTime, ArrayList ifaces)
        {
            bool rejectNeighbor = false;
            lock (OLSRParameters.SecondHopNeighborList)
            {
                foreach (SecondHopNeighbor second in OLSRParameters.SecondHopNeighborList)
                {
                    if (second.GetN_2hop_addr().Equals(this.L_neighbor_iface_addr))
                    {
                        foreach (LinkSensing lsTemp in OLSRParameters.LinksList)
                        {
                            if (lsTemp.L_neighbor_iface_addr.Equals(second.GetN_neighbor_main_addr()))
                            {
                                if (this.TFLLN > lsTemp.TFLLN)
                                {
                                    rejectNeighbor = true;
                                    consecutiveHelloTFLLN++;
                                    if (consecutiveHelloTFLLN == CONSECUTIVE_HELLO)
                                    {
                                        this.TFLLN = 0;
                                        consecutiveHelloTFLLN = 0;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (!rejectNeighbor)
            {
                L_ASYM_Time = DateTime.Now.AddSeconds(vTime).Ticks;
                if (ifaces.Contains(L_local_iface_addr))
                {
                    active = true;
                    consecutiveHello++;
                    if (LinkValue == OLSRConstants.LOST_LINK)
                        L_SYM_Time = DateTime.Now.AddSeconds(-1).Ticks;
                    else if (LinkValue == OLSRConstants.SYM_LINK || LinkValue == OLSRConstants.ASYM_LINK)
                    {
                        if (PLP == consecutiveHello)
                        {
                            LogWriter.GetInstance().AddText("Sensing: " + L_neighbor_iface_addr + " PLP = consecutiveHello = " + PLP);
                            L_SYM_Time = DateTime.Now.AddSeconds(vTime).Ticks;
                            L_Time = L_SYM_Time + (OLSRConstants.NEIGHB_HOLD_TIME * 1000);
                        }
                        else if (consecutiveHello > PLP)
                        {
                            LogWriter.GetInstance().AddText("Sensing: " + L_neighbor_iface_addr + " PLP > consecutiveHello " + PLP);
                            L_SYM_Time = DateTime.Now.AddSeconds(vTime).Ticks;
                            L_Time = L_SYM_Time + (OLSRConstants.NEIGHB_HOLD_TIME * 1000);
                            consecutiveHello = 0;
                            PLP = 1;
                        }
                    }
                }

                bool exist = false;

                lock (OLSRParameters.NeighborList)
                {
                    foreach (Neighbor n in OLSRParameters.NeighborList)
                    {
                        if (n.GetN_neighbor_iface_addr().Equals(L_neighbor_iface_addr))
                        {
                            exist = true;
                            break;
                        }
                    }

                    if (!exist)
                        OLSRParameters.NeighborList.Add(new Neighbor(this));
                }

                L_Time = Math.Max(L_Time, L_ASYM_Time);
            }
        }

        # region getters

        /// <summary>
        /// Devuelve el Link Type
        /// </summary>
        /// <returns>Link Type</returns>
        public int GetLinkType()
        {
            long current = DateTime.Now.Ticks;


            if (L_SYM_Time >= current)
                return OLSRConstants.SYM_LINK;
            if (L_ASYM_Time >= current && L_SYM_Time < current)
                return OLSRConstants.ASYM_LINK;
            if (L_ASYM_Time < current && L_SYM_Time < current)
                return OLSRConstants.LOST_LINK;

            return OLSRConstants.UNSPEC_LINK;
        }

        /// <summary>
        /// Devuelve el L_SYM_Time
        /// </summary>
        /// <returns>L_SYM_Time</returns>
        public long GetL_SYM_Time()
        {
            return L_SYM_Time;
        }

        public long GetL_Time()
        {
            return L_Time;
        }

        public IPAddress L_local_iface_addr_
        {
            get { return L_local_iface_addr; }
        }

        # endregion

    }
}