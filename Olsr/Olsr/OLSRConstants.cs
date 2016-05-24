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
    /// Clase donde almacenamos las variables constantes de OLSR con sus valores propuestos
    /// (Ver seccion 18 RFC 3626)
    ///
    /// Version: 1.1
    ///
    /// </summary>
    class OLSRConstants
    {
        public static float C =  1 / 16F; //Variable constante C 
       
        public const int OLRS_PORT = 698; //Puerto para comunicarse utilizando OLSR

        public const int PACKET_HEADER_LENGTH = 4;
        
        # region "Emission Intervals (seconds)"

        public static int HELLO_INTERVAL = 2; 
        public static int REFRESH_INTERVAL = 2; 
        public static int TC_INTERVAL = 5;

        #endregion

        # region "Holding Time (seconds)"

        public static int NEIGHB_HOLD_TIME = 3 * REFRESH_INTERVAL;
        public static int TOP_HOLD_TIME = 3 * TC_INTERVAL;
        public static int DUP_HOLD_TIME = 30;

        #endregion

        # region "Message Types"

        public const int HELLO_MESSAGE = 1;
        public const int TC_MESSAGE = 2;

        #endregion

        # region "Link Types"

        public const int UNSPEC_LINK = 0;
        public const int ASYM_LINK = 1;
        public const int SYM_LINK = 2;
        public const int LOST_LINK = 3;

        #endregion

        # region "Neighbor Types"

        public const int NOT_NEIGH = 0;
        public const int SYM_NEIGH = 1;
        public const int MPR_NEIGH = 2;

        #endregion

        # region "Link Hysteresis"

        public const double HYST_THRESHOLD_HIGH = 0.8;
        public const double HYST_THRESHOLD_LOW = 0.3;
        public const double HYST_SCALING = 0.5;

        #endregion

        # region "Willingness"

        public const int WILL_NEVER = 0;
        public const int WILL_LOW = 1;
        public const int WILL_DEFAULT = 3;
        public const int WILL_HIGH = 6;
        public const int WILL_ALWAYS = 7;

        #endregion

        # region "Misc. Constants"

        public const int TC_REDUNDANCY = 0;
        public const int MPR_COVERAGE= 1;
        public static double MAX_JITTER = HELLO_INTERVAL/4;

        #endregion

        # region "N_status Constants"

        public const int NOT_SYM = 0;
        public const int SYM = 1;

        #endregion

        public static void UpdateParameters()
        {
            NEIGHB_HOLD_TIME = 3 * REFRESH_INTERVAL;
            TOP_HOLD_TIME = 3 * TC_INTERVAL;
            MAX_JITTER = HELLO_INTERVAL / 4;
        }
    }
}