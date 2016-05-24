using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Windows.Forms;
using OLSR.Communication;
using OLSR.Configuration;
using OLSR.OLSR;
using OLSR.OLSR.RoutingTable;

namespace OLSR.Screens
{
    public partial class StartScreen : Form
    {
        private readonly ControlInvoker controlInvoker = null;

        private string PATH = @"" + Application.StartupPath + "\\blockedIPs.txt";

        public static StartScreen instance = null;

        public static StartScreen GetInstance()
        {
            if (instance == null)
                instance = new StartScreen();
            return instance;
        }

        public ControlInvoker GetControlInvoker()
        {
            return controlInvoker;
        }

        public StartScreen()
        {
            controlInvoker = new ControlInvoker();
            controlInvoker.SetControl(this);

            //Variable de objeto que contiene el socket
            OLSRParameters.SocketControler = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            //Separamos el puerto 698 para usarlo en nuestra aplicación
            OLSRParameters.SocketControler.Bind(new IPEndPoint(IPAddress.Any, OLSRConstants.OLRS_PORT));

            //Habilitamos la opción Broadcast para el socket
            OLSRParameters.SocketControler.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);

            InitializeComponent();

            //Recargamos listado de interfaces con las encontradas en el equipo
            LoadInterfaces();

            //Cargamos parametros de configuracion
            txtHello.Text = PropertiesReader.getInstance().getCurrentHelloInterval();
            txtTc.Text = PropertiesReader.getInstance().getCurrentTCInterval();
            txtRefresh.Text = PropertiesReader.getInstance().getCurrentRefreshInterval();

            OLSRParameters.Language = PropertiesReader.getInstance().getCurrentLanguage();

            LoadLanguage();
        }

        private void LoadLanguage()
        {
            if (OLSRParameters.Language.Equals("Italian") || OLSRParameters.Language.Equals("Italiano"))
            {
                OLSRParameters.Language = "Italian";
                RelaodLanguage();
            }
            else if (OLSRParameters.Language.Equals("English") || OLSRParameters.Language.Equals("Inglese"))
            {
                OLSRParameters.Language = "English";
                RelaodLanguage();
            }
        }

        private void RelaodLanguage()
        {
            lblTitle.Text = LanguageConfiguration.getInstance().getText(OLSRParameters.Language, "title");
            lblInterList.Text = LanguageConfiguration.getInstance().getText(OLSRParameters.Language, "olsrifaces");
            chckDebug.Text = LanguageConfiguration.getInstance().getText(OLSRParameters.Language, "olsrtest");
            bttStart.Text = LanguageConfiguration.getInstance().getText(OLSRParameters.Language, "olsrstart");
            bttStop.Text = LanguageConfiguration.getInstance().getText(OLSRParameters.Language, "olsrstop");

            groupBox1.Text = LanguageConfiguration.getInstance().getText(OLSRParameters.Language, "paramstitle");
            label9.Text = LanguageConfiguration.getInstance().getText(OLSRParameters.Language, "paramshello");
            label10.Text = LanguageConfiguration.getInstance().getText(OLSRParameters.Language, "paramstc");
            label11.Text = LanguageConfiguration.getInstance().getText(OLSRParameters.Language, "paramsrefresh");
            label4.Text = LanguageConfiguration.getInstance().getText(OLSRParameters.Language, "paramslanguage");

            String textLenguages = LanguageConfiguration.getInstance().getText(OLSRParameters.Language, "paramslanguages");
            String[] languages = textLenguages.Split(',');
            cmbLanguage.Items.Clear();
            for (int x = 0; x < languages.Length; x++)
            {
                cmbLanguage.Items.Add(languages[x]);
            }

            if (OLSRParameters.Language.Equals("English"))
                cmbLanguage.SelectedIndex = 0;
            else
                cmbLanguage.SelectedIndex = 1;

            bttRestore.Text = LanguageConfiguration.getInstance().getText(OLSRParameters.Language, "paramsrestore");
            bttSave.Text = LanguageConfiguration.getInstance().getText(OLSRParameters.Language, "paramssave");

            label1.Text = LanguageConfiguration.getInstance().getText(OLSRParameters.Language, "neighborlink");
            label2.Text = LanguageConfiguration.getInstance().getText(OLSRParameters.Language, "neighborsecond");
            label3.Text = LanguageConfiguration.getInstance().getText(OLSRParameters.Language, "mprlist");
        }

        /// <summary>
        /// Metodo encargado de rellenar el listado de interfaces con las interfaces 
        /// disponibles en nuestra máquina
        /// </summary>
        private void LoadInterfaces()
        {
            //borramos el listado de interfaces
            lstInterfaces.Items.Clear();

            //leemos todas las interfaces de red de nuestra máquina
            NetworkInterface[] netInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            //recorremos las interfaces y las tratamos, omitiendo 127.0.0.1
            foreach (NetworkInterface net in netInterfaces)
            {
                try
                {
                    if (net.GetIPProperties().UnicastAddresses.Count > 0)
                    {
                        IPAddress ip = net.GetIPProperties().UnicastAddresses[0].Address;
                        if (!ip.Equals(IPAddress.Loopback))
                            lstInterfaces.Items.Add(ip);
                    }
                }
                catch (Exception)
                { }
            }
        }

        private void bttStart_Click(object sender, EventArgs e)
        {

            if (lstInterfaces.SelectedIndex != -1)
            {
                OLSRConstants.HELLO_INTERVAL = Convert.ToInt32(txtHello.Text);
                OLSRConstants.TC_INTERVAL = Convert.ToInt32(txtTc.Text);
                OLSRConstants.REFRESH_INTERVAL = Convert.ToInt32(txtRefresh.Text);
                OLSRConstants.UpdateParameters();
                if (chckDebug.Checked)
                {
                    try
                    {
                        //Leemos IPs que queremos bloquear
                        StreamReader sr = new StreamReader(PATH);
                        string texto = sr.ReadToEnd();
                        string[] values = texto.Split(',');
                        for (int x = 0; x < values.Length; x++)
                            OLSRParameters.BlockedIPs.Add(IPAddress.Parse(values[x]));

                        sr.Close();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show(LanguageConfiguration.getInstance().getText(OLSRParameters.Language, "errortestfile"));
                    }
                }


                bttStart.Enabled = false;
                bttStop.Enabled = true;

                InfoSender.GetInstance().StartThread();

                InfoReceiver.GetInstance().StartThread();
            }

        }

        private void bttStop_Click(object sender, EventArgs e)
        {
            RoutingTableCalculation.GetInstance().DeleteExistRoutingTable();

            bttStart.Enabled = true;
            bttStop.Enabled = false;

            InfoSender.GetInstance().StopThread();

            InfoReceiver.GetInstance().StopThread();

            lock (OLSRParameters.LinksList)
                OLSRParameters.LinksList = new ArrayList();
            lock (OLSRParameters.MPRSet)
                OLSRParameters.MPRSet = new ArrayList();
            lock (OLSRParameters.NeighborList)
                OLSRParameters.NeighborList = new ArrayList();
            lock (OLSRParameters.SecondHopNeighborList)
                OLSRParameters.SecondHopNeighborList = new ArrayList();
            lock (OLSRParameters.TopologySet)
                OLSRParameters.TopologySet = new ArrayList();

            lstNeighbors.Items.Clear();
            lst2HopNeighbors.Items.Clear();
            lstMPR.Items.Clear();
        }

        private void lstInterfaces_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstInterfaces.SelectedIndex != -1)
            {
                OLSRParameters.Originator_Addr = IPAddress.Parse(lstInterfaces.SelectedItem.ToString());
                bttStart.Enabled = true;
            }
            else
                MessageBox.Show(LanguageConfiguration.getInstance().getText(OLSRParameters.Language, "selectiface"));
        }

        public void PrintNeighbors(object[] arguments)
        {
            lstNeighbors.Items.Clear();
            foreach (string arg in arguments)
            {
                lstNeighbors.Items.Add(arg);
            }

        }

        public void Print2HopNeighbors(object[] arguments)
        {
            lst2HopNeighbors.Items.Clear();
            foreach (string arg in arguments)
            {
                lst2HopNeighbors.Items.Add(arg);
            }
        }

        public void PrintMPRSet(object[] arguments)
        {
            lstMPR.Items.Clear();
            foreach (string arg in arguments)
            {
                lstMPR.Items.Add(arg);
            }
        }

        private void bttRestore_Click(object sender, EventArgs e)
        {
            txtHello.Text = PropertiesReader.getInstance().getDefaultHelloInterval();
            txtTc.Text = PropertiesReader.getInstance().getDefaultTCInterval();
            txtRefresh.Text = PropertiesReader.getInstance().getDefaultRefreshInterval();
            txtHello.Text = PropertiesReader.getInstance().getDefaultHelloInterval();
            txtTc.Text = PropertiesReader.getInstance().getDefaultTCInterval();
            txtRefresh.Text = PropertiesReader.getInstance().getDefaultRefreshInterval();
            cmbLanguage.SelectedIndex = 1;
        }

        private void bttSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            
            try
            {
                PropertiesReader.getInstance().setHelloInterval(txtHello.Text);
            }
            catch (Exception)
            {
                PropertiesReader.getInstance().setHelloInterval(PropertiesReader.getInstance().getDefaultHelloInterval());
            }

            try
            {
                PropertiesReader.getInstance().setTCInterval(txtTc.Text);
            }
            catch (Exception)
            {
                PropertiesReader.getInstance().setTCInterval(PropertiesReader.getInstance().getDefaultTCInterval());
            }

            try
            {
                PropertiesReader.getInstance().setRefreshInterval(txtRefresh.Text);
            }
            catch (Exception)
            {
                PropertiesReader.getInstance().setRefreshInterval(PropertiesReader.getInstance().getDefaultRefreshInterval());
            }

            try
            {
                PropertiesReader.getInstance().setLanguage(cmbLanguage.Text);
            }
            catch (Exception)
            {
                PropertiesReader.getInstance().setLanguage(PropertiesReader.getInstance().getDefaultLanguage());
            }

            PropertiesReader.getInstance().saveFile();

            if (!OLSRParameters.Language.Equals(PropertiesReader.getInstance().getCurrentLanguage()))
            {
                OLSRParameters.Language = PropertiesReader.getInstance().getCurrentLanguage();
                LoadLanguage();
            }
            Cursor.Current = Cursors.Default;
        }

        private void StartScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(OLSRParameters.RoutingSet.Count > 0)
            {
                foreach (Route route in OLSRParameters.RoutingSet)
                {
                    ChangeRoutingTable.DeleteRoute2RoutingTable(route.R_dest_addr_.ToString());
                }
            }
        }
    }
}
