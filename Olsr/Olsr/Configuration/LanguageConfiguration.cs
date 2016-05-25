using System;
using System.Xml;
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
    /// Clase con los metodos comunes para manejar diferentes idiomas
    /// 
    /// Version: 1.1
    /// 
    /// </summary>
    public class LanguageConfiguration
    {
        private string PATH = @"" + Application.StartupPath + "\\Languages.xml";
        private string SEPARATOR = "/";
        private string SELECTED_NODO = "";
        private XmlDocument languageDocument = null;
        private static LanguageConfiguration instance =null;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public LanguageConfiguration()
        {
            languageDocument = new XmlDocument();
            languageDocument.Load(PATH);
        }

        /// <summary>
        /// Creates if does not exist an instance of LanguageConfiguration
        /// </summary>
        /// <returns>LanguageConfiguration</returns>
        public static LanguageConfiguration getInstance()
        {
            if (instance == null)
                instance = new LanguageConfiguration();
            return instance;
        }

        /// <summary>
        /// This function returns the value of the text from the
        /// specific control in the selected language
        /// </summary>
        /// <param name="seccion">The selected language</param>
        /// <param name="clave">The name of the control</param>
        /// <returns>The text value of the control</returns>
        public string getText(string seccion, string clave)
        {
            SELECTED_NODO = languageDocument.LastChild.Name;
            string path = SELECTED_NODO + SEPARATOR + seccion + SEPARATOR + clave;

            string value = languageDocument.SelectSingleNode(path).InnerText;

            return value;
        }
    }
}
