using System.Collections;
using System.Net;
using OLSR.OLSR.Packets.Messages.Neighbors;
using OLSR.Configuration;

namespace OLSR.OLSR.Packets.Messages.MPR
{
    /// <summary>
    /// 
    /// Created by  APIF Moviquity S.A.
    /// 
    /// http://www.moviquity.com/
    /// 
    ///
    /// Version: 1.1
    ///
    /// </summary>
    class MPRCompute
    {
        private static ArrayList N;
        private static ArrayList N2;

        private static readonly ArrayList neigborDegrees = new ArrayList();
        private static readonly ArrayList neighborReachabilities = new ArrayList();
        
        /// <summary>
        /// 
        /// </summary>
        public static void Compute()
        {
            
            neigborDegrees.Clear();
            neighborReachabilities.Clear();
            
            lock (OLSRParameters.NeighborList)
            {
                N = (ArrayList)OLSRParameters.NeighborList.Clone();
            }

            
            lock (OLSRParameters.SecondHopNeighborList)
            {
                N2 = (ArrayList)OLSRParameters.SecondHopNeighborList.Clone();
            }
            /*foreach (Neighbor neig in N)
            {
                neigborDegrees.Add(DegreeOfANode(neig));
            }
            System.Console.WriteLine("********* neigborDegrees -->" + neigborDegrees.Count);*/
            //MIRAMOS SI ALGÚN VECINO A DOS SALTOS EXISTE EN LOS VECINOS A UN SALTO Y BORRAMOS DE VECINOS A DOS SALTOS.
            foreach (Neighbor neig in N)
            {
                int numDeleted = 0;
                for (int i = 0; i < N2.Count + numDeleted; i++) //SecondHopNeighbor SecondHopNeig in N2)
                {
                    var secondHopNeig = (SecondHopNeighbor)N2[i - numDeleted];
                    if (secondHopNeig.GetN_2hop_addr().Equals(neig.GetN_neighbor_iface_addr()))
                    {
                        if (neig.GetN_Status() == OLSRConstants.SYM)
                        {
                            N2.Remove(secondHopNeig);
                            numDeleted++;
                        }
                    }
                }
            }

            var ExclusiveSecondHopNeighborList = new ArrayList();
            foreach (SecondHopNeighbor node in N2)
                if (!ExclusiveSecondHopNeighborList.Contains(node.GetN_2hop_addr()))
                    ExclusiveSecondHopNeighborList.Add(node.GetN_2hop_addr());

            lock (OLSRParameters.MPRSet)
            {
                OLSRParameters.MPRSet.Clear();    
            }
            
            foreach (IPAddress node in ExclusiveSecondHopNeighborList)
            {
                int neighborLinks = 0;
                IPAddress ip = null;
                foreach (SecondHopNeighbor secondHopnode in N2)
                    if (node.Equals(secondHopnode.GetN_2hop_addr()))
                    {
                        neighborLinks++;
                        ip = secondHopnode.GetN_neighbor_main_addr();
                    }
                if (neighborLinks == 1)
                {
                    lock (OLSRParameters.MPRSet)
                    {
                        OLSRParameters.MPRSet.Add(ip);
                    }

                    N2 = Delete2HopNeigbors(N2, ip);
                }
            }

            while (N2.Count != 0)
            {
                foreach (Neighbor neig in N)
                    neighborReachabilities.Add(ReachabilityOfANode(neig, N2));
                
                int maxReachability = -1;
                int MPRselected = -1;
                MPRselected = maxReachability = GetMax(neighborReachabilities);

                ArrayList tmpNeigh = new ArrayList();
                //if (GetNumberofElements(neighborReachabilities, maxReachability) != 1)
                //{                    
                    foreach (Neighbor neig in N)
                    {
                        if (((int)ReachabilityOfANode(neig, N2)) == maxReachability)
                        {
                            tmpNeigh.Add(neig);
                        }
                    }
                    foreach (Neighbor neig in tmpNeigh)
                    {
                        neigborDegrees.Add(DegreeOfANode(neig));
                    }
                    MPRselected = GetMax(neigborDegrees);

                /*}
                else
                {
                    
                    foreach (Neighbor neig in N)
                    {
                        if (((int)ReachabilityOfANode(neig, N2)) == maxReachability)
                        {
                            neigborDegrees.Add(DegreeOfANode(neig));
                        }
                    }
                }*/

                if (GetIndexOf(neigborDegrees, MPRselected) != -1)
                {
                    IPAddress selectedIp =
                        ((Neighbor)tmpNeigh[GetIndexOf(neigborDegrees, MPRselected)]).GetN_neighbor_iface_addr();


                    lock (OLSRParameters.MPRSet)
                    {
                        OLSRParameters.MPRSet.Add(selectedIp);
                    }

                    N2 = Delete2HopNeigbors(N2, selectedIp);
                }
                tmpNeigh = null;
                neigborDegrees.Clear();
                neighborReachabilities.Clear();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="neig"></param>
        /// <returns></returns>
        private static object DegreeOfANode(Neighbor neig)
        {
            int degree = 0;
            foreach (SecondHopNeighbor SecondHopNeig in N2)
                if (neig.GetN_neighbor_iface_addr().Equals(SecondHopNeig.GetN_neighbor_main_addr()))
                {
                    bool existInN = false;
                    foreach (Neighbor FirstHopNode in N)
                        if (FirstHopNode.GetN_neighbor_iface_addr().Equals(SecondHopNeig.GetN_2hop_addr()))
                            existInN = true;
                    if (!existInN)
                        degree++;
                }
            return degree;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reachabilities"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        private static decimal GetNumberofElements(ArrayList reachabilities, int number)
        {
            int numberofElements = 0;
            foreach (int numero in reachabilities)
                if (numero == number)
                    numberofElements++;
            return numberofElements;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="num"></param>
        private static int GetIndexOf(ArrayList array, int num)
        {
            for (int i = 0; i < array.Count; i++)
                if ((int)array[i] == num)
                    return i;
            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reachabilities"></param>
        private static int GetMax(ArrayList reachabilities)
        {
            int max = -1;
            foreach (int numero in reachabilities)
                if (numero > max)
                    max = numero;
            return max;
        }              

        /// <summary>
        /// 
        /// </summary>
        /// <param name="neig"></param>
        /// <param name="N2"></param>
        /// <returns></returns>
        private static object ReachabilityOfANode(Neighbor neig, ArrayList N2)
        {
            int degree = 0;
            foreach (SecondHopNeighbor SecondHopNeig in N2)
                if (neig.GetN_neighbor_iface_addr().Equals(SecondHopNeig.GetN_neighbor_main_addr()))
                    degree++;
            return degree;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n2"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        private static ArrayList Delete2HopNeigbors(ArrayList n2, IPAddress ip)
        {
            ArrayList n2Temp = new ArrayList();
            
            foreach (SecondHopNeighbor node in n2)
                if (node.GetN_neighbor_main_addr().Equals(ip))
                    n2Temp.Add(node);

            int x = 0;
            
            for (x = n2.Count -1 ; x > -1; x--)
            {
                SecondHopNeighbor node = (SecondHopNeighbor)n2[x];
                foreach (SecondHopNeighbor node2 in n2Temp)
                    if (node2.GetN_2hop_addr().Equals(node.GetN_2hop_addr()))
                    {
                        n2.RemoveAt(x);
                        n2.TrimToSize();
                        break;
                    }
            }
            
            return n2;
        }

    }
}