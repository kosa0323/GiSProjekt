using System.Diagnostics;

namespace OLSR.OLSR.RoutingTable
{
    /// <summary>
    /// 
    /// Created by  APIF Moviquity S.A.
    /// 
    /// http://www.moviquity.com/
    /// 
    /// Clase encargada de añadir las nuevas entradas en la tabla de rutas
    /// 
    /// Version: 1.1
    ///
    /// </summary>
    class ChangeRoutingTable
    {
        /// <summary>
        /// Metodo encargado de añadir una ruta a la tabla de rutas del sistema
        /// </summary>
        /// <param name="IPAddRoute">IP a añadir en la tabla</param>
        /// <param name="gateway">Puerta enlace</param>
        public static void AddRoute2RoutingTable(string IPAddRoute,string gateway)
        {
            if (!(IPAddRoute.Equals(OLSRParameters.Originator_Addr.ToString()) || gateway.Equals(OLSRParameters.Originator_Addr.ToString())))
            {
                Process route = new Process();
                if (!IPAddRoute.Equals(gateway))
                {                    
                    route.StartInfo.FileName = "route";
                    if (!IPAddRoute.Equals("0.0.0.0"))
                        route.StartInfo.Arguments = "ADD " + IPAddRoute + " MASK 255.255.255.255 " + gateway ;
                    else
                    {
                        DeleteRoute2RoutingTable("0.0.0.0");                        
                        route.StartInfo.Arguments = "ADD " + IPAddRoute + " MASK 0.0.0.0 " + gateway ;
                    }
                    route.StartInfo.CreateNoWindow = true;
                    route.StartInfo.UseShellExecute = false;
                    route.Start();

                    route.Close();
                }
                //if (IPAddRoute.EndsWith(".1") && !OLSRParameters.Originator_Addr.ToString().EndsWith(".1"))
                //{
                //    DeleteRoute2RoutingTable("0.0.0.0");

                //    route = new Process();
                //    route.StartInfo.FileName = "route";
                //    route.StartInfo.Arguments = "ADD 0.0.0.0 MASK 0.0.0.0 " + gateway;
                //    route.Start();

                //    route.Close();
                //}
            }

        }
        
        /// <summary>
        /// Metodo encargado de borrar las entradas en la tabla de rutas cuya ip se corresponda 
        /// </summary>
        /// <param name="IPRemoveRoute">IP a borrar</param>
        public static void DeleteRoute2RoutingTable(string IPRemoveRoute)
        {
            if (!IPRemoveRoute.Equals("127.0.0.0") && !IPRemoveRoute.Equals("255.255.255.255"))
            {
                Process route = new Process();
                route.StartInfo.CreateNoWindow = true;
                route.StartInfo.UseShellExecute = false;
                route.StartInfo.FileName = "route";                
                route.StartInfo.Arguments = "DELETE " + IPRemoveRoute;               
                
                route.Start();

                route.Close();
            }
        } 

    }
}