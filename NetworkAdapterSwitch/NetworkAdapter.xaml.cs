using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NetworkAdapterSwitch
{
    public delegate void NetworkAdapterChangeStatusEventHandler(NetworkAdapterConnection NetworkAdapterConnection, bool NetworkAdapterEnable);
    /// <summary>
    /// NetworkAdapter.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class NetworkAdapter : UserControl
    {
        private string nAdapterName;
        private bool nAdapterEnabled;
        private NetworkAdapterConnection nAdapterConnection;
        public NetworkAdapter(string NetworkAdapterName)
        {
            InitializeComponent();

            this.DataContext = this;
            this.nAdapterName = NetworkAdapterName;
            this.nAdapterConnection = new NetworkAdapterConnection(this.nAdapterName);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        public string NetworkAdapterName
        {
            set
            {
                this.nAdapterName = value;
                OnPropertyChanged("NetworkAdapterName");
            }
            get
            {
                return this.nAdapterName;
            }
        }
        public bool NetworkAdapterEnabled
        {
            set
            {
                this.nAdapterEnabled = value;
                if (this.nAdapterEnabled)
                {
                    this.ButtonEnable.Value = "Enabled";
                }
                else
                {
                    this.ButtonEnable.Value = "Disabled";
                }
            }
            get
            {
                return this.nAdapterEnabled;
            }
        }

        private void ButtonEnable_Click(object sender, RoutedEventArgs e)
        {
            bool nAdapterSwitch = !this.NetworkAdapterEnabled;
            nAdapterConnection.SetNetworkAdapterStatus(nAdapterSwitch);
            this.NetworkAdapterEnabled = nAdapterSwitch;
        }
    }
}
