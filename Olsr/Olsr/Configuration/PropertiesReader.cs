using System.Windows.Forms;
using System.Xml;

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
    /// Version: 1.1
    ///
    /// </summary>
    class PropertiesReader
    {

        private readonly string PATH = @"" + Application.StartupPath + "\\config.xml";
        private const string READSETTINGS = "Read_Settings";
        private const string DEFAULTSETTINGS = "Default_Settings";
        private string SELECTED_NODO = "";
        private const string SEPARATOR = "/";
        private readonly XmlDocument configurationDocument = null;

        #region "nodeNames"

        private string HELLO = "hello";
        private string TC = "tc";
        private string REFRESH = "refresh";
        private string LANGUAGE = "language";

        #endregion

        private static PropertiesReader instance = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public PropertiesReader()
        {
            configurationDocument = new XmlDocument();
            configurationDocument.Load(PATH);
        }

        /// <summary>
        /// Metodo encargado de devolver la instancia de la clase o crear una nueva
        /// </summary>
        /// <returns>PropertiesReader</returns>
        public static PropertiesReader getInstance()
        {
            if (instance == null)
                instance = new PropertiesReader();
            return instance;
        }

        /// <summary>
        /// Metodo que devuelve el valor buscado en el fichero de configuracion
        /// </summary>
        /// <param name="seccion">Zona donde buscamos el texto</param>
        /// <param name="clave">Texto que buscamos</param>
        /// <returns>Texto encontrado</returns>
        private string getValue(string seccion, string clave)
        {
            SELECTED_NODO = configurationDocument.LastChild.Name;
            string path = SELECTED_NODO + SEPARATOR + seccion + SEPARATOR + clave;

            string value = configurationDocument.SelectSingleNode(path).InnerText;

            return value;
        }

        /// <summary>
        /// Metodo encargado de guardar el valor del texto 
        /// </summary>
        /// <param name="clave">Variable donde guardamos</param>
        /// <param name="value">Valor que guardamos</param>
        private void setValue(string clave, string value)
        {
            SELECTED_NODO = configurationDocument.LastChild.Name;
            string path = SELECTED_NODO + SEPARATOR + READSETTINGS + SEPARATOR + clave;
            configurationDocument.SelectSingleNode(path).InnerText = value;
        }

        /// <summary>
        /// Guarda los cambios en el fichero
        /// </summary>
        public void saveFile()
        {
            configurationDocument.Save(PATH);
        }

        #region "get current values"

        public string getCurrentHelloInterval()
        {
            return getValue(READSETTINGS, HELLO);
        }

        public string getCurrentTCInterval()
        {
            return getValue(READSETTINGS, TC);
        }

        public string getCurrentRefreshInterval()
        {
            return getValue(READSETTINGS, REFRESH);
        }

        public string getCurrentLanguage()
        {
            return getValue(READSETTINGS, LANGUAGE);
        }

        #endregion

        #region "get default values"

        public string getDefaultHelloInterval()
        {
            return getValue(DEFAULTSETTINGS, HELLO);
        }

        public string getDefaultTCInterval()
        {
            return getValue(DEFAULTSETTINGS, TC);
        }

        public string getDefaultRefreshInterval()
        {
            return getValue(DEFAULTSETTINGS, REFRESH);
        }

        public string getDefaultLanguage()
        {
            return getValue(DEFAULTSETTINGS, LANGUAGE);
        }

        #endregion

        #region "set new values"

        public void setHelloInterval(string value)
        {
            setValue(HELLO, value);
        }

        public void setTCInterval(string value)
        {
            setValue(TC, value);
        }

        public void setRefreshInterval(string value)
        {
            setValue(REFRESH, value);
        }

        public void setLanguage(string value)
        {
            setValue(LANGUAGE, value);
        }

        #endregion

    }
}