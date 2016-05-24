using System;
using System.Collections;
using System.Net;
using OLSR.Configuration;
using OLSR.OLSR.Packets.Messages.Links;
using OLSR.OLSR.Packets.Messages.Neighbors;
using OLSR.OLSR.Packets.Messages.TC;

namespace OLSR.OLSR.RoutingTable
{
    /// <summary>
    /// 
    /// Created by  APIF Moviquity S.A.
    /// 
    /// http://www.moviquity.com/
    /// 
    /// Clase encargada de calcular las entradas en la tabla de rutas
    ///
    /// Version: 1.1
    /// 
    /// </summary>
    class RoutingTableCalculation
    {

        # region Variables 

        public static ArrayList RoutingSet;//Tabla de Rutas

        public static RoutingTableCalculation instance = null;

        #endregion

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        protected RoutingTableCalculation()
        {
            RoutingSet = new ArrayList();
        }

        /// <summary>
        /// Metodo encargado de devolver la instancia de la clase
        /// </summary>
        /// <returns></returns>
        public static RoutingTableCalculation GetInstance()
        {
            if (instance == null)
                instance = new RoutingTableCalculation();
            return instance;
        }

        /// <summary>
        /// Metodo encargado de organizar las entradas en la tabla de rutas
        /// </summary>
        public ArrayList CalculateTableRoute()
        {
            ArrayList routes = new ArrayList();
            /// punto 2 RFC
            routes.AddRange(AddRoutingSymetricEntries());

            /// punto 3 RFC
            routes.AddRange(Add2HopRoutingEntries());
            
            /// punto 3(2) RFC
            routes.AddRange(AddOtherRoutingEntries(routes));

            foreach (Route route in routes)
            {
                if (route.R_dest_addr_.ToString().EndsWith(".1") && !OLSRParameters.Originator_Addr.ToString().EndsWith(".1"))
                {
                    routes.Add(new Route(IPAddress.Parse("0.0.0.0"), route.R_next_addr_, route.R_dist_, route.R_iface_addr_));
                    break;
                }
            }

            return routes;
        }

        /// <summary>
        /// Metodo encargado de borrar las entradas creadas en la tabla de rutas
        /// </summary>
        public void DeleteExistRoutingTable()
        {
            //Borramos la tabla de rutas existente
            lock (OLSRParameters.RoutingSet)
            {
                foreach (Route route in OLSRParameters.RoutingSet)
                {
                    ChangeRoutingTable.DeleteRoute2RoutingTable(route.R_dest_addr_.ToString());
                }
                OLSRParameters.RoutingSet.Clear();
            }
        }

        /// <summary>
        /// Metodo encargado de añadir las rutas de los vecinos simetricos
        /// </summary>
        private ArrayList AddRoutingSymetricEntries()
        {
            ArrayList routing = new ArrayList();
            ArrayList neighList = null;
            lock (OLSRParameters.NeighborList)
            {
                neighList = (ArrayList)OLSRParameters.NeighborList.Clone();
            }
                foreach (Neighbor neig in neighList)
                {
                    if (neig.GetN_Status() == OLSRConstants.SYM_NEIGH)
                    {
                        LinkSensing linkSens = SearchLinkByNeighbor(neig);
                        if (linkSens != null && linkSens.GetL_Time() >= DateTime.Now.Ticks)
                        {
                            routing.Add(new Route(linkSens.L_neighbor_iface_addr,
                                                                    linkSens.L_neighbor_iface_addr, 1,
                                                                    linkSens.L_local_iface_addr_));
                        }
                    }
                }
            

            return routing;

        }

        /// <summary>
        /// Buscamos link asociado al vecino
        /// </summary>
        /// <param name="neighbor">Informacion del vecino</param>
        /// <returns>Link</returns>
        private LinkSensing SearchLinkByNeighbor(Neighbor neighbor)
        {
            lock (OLSRParameters.LinksList)
            {
                foreach (LinkSensing sensing in OLSRParameters.LinksList)
                {
                    if (sensing.L_neighbor_iface_addr.Equals(neighbor.GetN_neighbor_iface_addr()))
                    {
                        return sensing;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Metodo encargado de añadir rutas de segundo salto en la tabla de rutas
        /// </summary>
        private ArrayList Add2HopRoutingEntries()
        {
            ArrayList routing = new ArrayList();
            ArrayList N2Hops;
            lock (OLSRParameters.SecondHopNeighborList)
            {
                N2Hops = (ArrayList)OLSRParameters.SecondHopNeighborList.Clone();
            }
            
            lock (OLSRParameters.NeighborList)
            {
                //Borramos de la tabla (copia) de vecinos a segundo salto, todas las entradas
                //que existen en la tabla de vecinos cuyo estado es simetrico
                foreach (Neighbor neighbor in OLSRParameters.NeighborList)
                {
                    int numDeleted = 0;
                    for (int i = 0; i < N2Hops.Count + numDeleted; i++)
                    {
                        var neig2Hop = (SecondHopNeighbor)N2Hops[i - numDeleted];
                        if (neig2Hop.GetN_2hop_addr().Equals(neighbor.GetN_neighbor_iface_addr()))
                        {
                            if (neighbor.GetN_Status() == OLSRConstants.SYM)
                            {
                                N2Hops.Remove(neig2Hop);
                                numDeleted++;
                            }
                        }
                    }
                }

                //Recorremos el listado de vecinos y la tabla (copia) de vecinos a segundo salto
                //buscando que vecinos estan asociados para asi añadir una nueva entrada a la
                //tabla de rutas
                foreach (Neighbor neighbor in OLSRParameters.NeighborList)
                {
                    foreach (SecondHopNeighbor neig2Hop in N2Hops)
                    {
                        if (neig2Hop.GetN_neighbor_main_addr().ToString().Equals(neighbor.GetN_neighbor_iface_addr().ToString()))
                        {
                            routing.Add(new Route(neig2Hop.GetN_2hop_addr(),
                                                                    neighbor.GetN_neighbor_iface_addr(), 2,
                                                                    neighbor.GetN_neighbor_iface_addr()));                           
                        }
                    }
                }
                
            }
            return routing;
        }

        /// <summary>
        /// Metodo encargado de cambiar la tabla de rutas con las nuevas rutas calculadas
        /// </summary>
        public void ModifyRoutingTable(ArrayList pRoutes)
        {

            LogWriter.GetInstance().AddText(" NEWRoutes --> "+ pRoutes.Count);
            foreach (Route route in pRoutes)
            {
                LogWriter.GetInstance().AddText(" * newRoute - Dest: " + route.R_dest_addr_+" - Next: "+route.R_next_addr_+" - IF: "+route.R_iface_addr_+" - Dist: "+route.R_dist_);
            }
            LogWriter.GetInstance().AddText(" ExistRoutes --> " + OLSRParameters.RoutingSet.Count);
            foreach (Route route in OLSRParameters.RoutingSet)
            {
                LogWriter.GetInstance().AddText(" * ExistRoutes - Dest: " + route.R_dest_addr_ + " - Next: " + route.R_next_addr_ + " - IF: " + route.R_iface_addr_ + " - Dist: " + route.R_dist_);
            }

            if (OLSRParameters.RoutingSet.Count==0)
            {
                foreach (Route route in pRoutes)
                {
                    ChangeRoutingTable.AddRoute2RoutingTable(route.R_dest_addr_.ToString(), route.R_next_addr_.ToString());
                }
            }
            else
            {
                lock (OLSRParameters.RoutingSet)
                {
                    foreach (Route route in pRoutes)
                    {
                        if (!ExistRoute(route, OLSRParameters.RoutingSet))
                        {
                            ChangeRoutingTable.AddRoute2RoutingTable(route.R_dest_addr_.ToString(), route.R_next_addr_.ToString());
                        }

                    }

                    foreach (Route route in OLSRParameters.RoutingSet)
                    {
                        if (!ExistRoute(route, pRoutes) && !route.R_dest_addr_.ToString().Equals("0.0.0.0"))
                            ChangeRoutingTable.DeleteRoute2RoutingTable(route.R_dest_addr_.ToString());
                    }

                    foreach (Route route in OLSRParameters.RoutingSet)
                    { 
                        if(route.R_dest_addr_.ToString().Equals("0.0.0.0"))
                        {
                            if (!route.R_next_addr_.ToString().EndsWith(".1"))
                            {
                                char[] ch = new char[1];
                                ch[0] = '.';
                                string[] ipDigits = OLSRParameters.Originator_Addr.ToString().Split(ch);
                                string tmpSt = ipDigits[0] + "." + ipDigits[1] + "." + ipDigits[2] + ".1";
                                ChangeRoutingTable.AddRoute2RoutingTable(tmpSt, route.R_next_addr_.ToString());
                            }
                            else 
                            {
                                char[] ch = new char[1];
                                ch[0] = '.';
                                string[] ipDigits = OLSRParameters.Originator_Addr.ToString().Split(ch);
                                string tmpSt = ipDigits[0] + "." + ipDigits[1] + "." + ipDigits[2] + ".1";
                                ChangeRoutingTable.DeleteRoute2RoutingTable(tmpSt);
                            }
                            break;
                        }
                    }
                    
                }
            }

            OLSRParameters.RoutingSet = pRoutes;

        }

        private bool ExistRoute(Route pRoute, ArrayList pRoutes)
        {

            foreach (Route route in pRoutes)
            {
                if (route.R_dest_addr_.Equals(pRoute.R_dest_addr_) &&
                    route.R_dist_ == pRoute.R_dist_ && route.R_iface_addr_.Equals(pRoute.R_iface_addr_) &&
                    route.R_next_addr_.Equals(pRoute.R_next_addr_))
                    return true;

            }

            return false;
        }

        /// <summary>
        /// Metodo encargado de añadir a la tabla de rutas las rutas de nodos a mas de 2 saltos
        /// </summary>
        private ArrayList AddOtherRoutingEntries(ArrayList pRoutes)
        {
            ArrayList routing = new ArrayList();
            lock (OLSRParameters.TopologySet)
            {
                foreach (Topology tc in OLSRParameters.TopologySet)
                {
                    var exist = false;

                    foreach (Route route in pRoutes)
                    {
                        if (tc.T_destination_addr_.Equals(route.R_dest_addr_))
                        {
                            exist = true;
                            break;
                        }
                    }

                    if (!exist)
                    {
                        for (int i = 0; i < pRoutes.Count; i++)
                        {
                            var route = (Route)pRoutes[i];

                            if (tc.T_last_addr_.Equals(route.R_dest_addr_) && route.R_dist_>= 2)
                            {

                                //Busca en la tabla de rutas la ruta con R_dest_addr == T_last_addr
                                IPAddress next = SearchNextAddress(tc.T_last_addr_, pRoutes);
                                
                                routing.Add(new Route(tc.T_destination_addr_,
                                                                        next, route.R_dist_ + 1,
                                                                        tc.T_last_addr_));                                
                            }
                        }
                    }
                }
            }
            return routing;
        }

        /// <summary>
        /// Metodo encargado de buscar el gateway de una ruta a mas de 2 saltos conociendo 
        /// la ruta de destino
        /// </summary>
        /// <param name="last_addr_">Ruta de destino</param>
        /// <returns>IPAddress con la informacion del gateway</returns>
        private IPAddress SearchNextAddress(IPAddress last_addr_, ArrayList pRoutes)
        {
            foreach (Route route in pRoutes)
            {
                if (route.R_dest_addr_.Equals(last_addr_))
                    return route.R_next_addr_;
            }
            return null;
        }
    }
}