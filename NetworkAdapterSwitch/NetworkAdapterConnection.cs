using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace NetworkAdapterSwitch
{
    public class NetworkAdapterConnection
    {
        private const string NetworkAdapterSearchQuery = @"SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionId != NULL";
        private const string NetworkAdapterSelectQueryFormat = @"SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionId = '{0}'";
        private ManagementObjectSearcher NetworkAdapterSearcher { set; get; }

        public string NetworkAdapterName { set; get; }

        public NetworkAdapterConnection(string NetworkAdapterName)
        {
            this.NetworkAdapterName = NetworkAdapterName;
            SelectQuery nAdapterSelectQuery = new SelectQuery(string.Format(this.GenNetworkAdapterSelectQuery(this.NetworkAdapterName)));
            NetworkAdapterSearcher = new ManagementObjectSearcher(nAdapterSelectQuery);
        }
        protected string NetworkAdapterSelectQuery()
        {
            return this.GenNetworkAdapterSelectQuery(this.NetworkAdapterName);
        }
        protected string GenNetworkAdapterSelectQuery(string NetworkAdapterName)
        {
            return string.Format(NetworkAdapterSelectQueryFormat, NetworkAdapterName);
        }
        public void SetNetworkAdapterStatus(bool AdapterEnable)
        {
            foreach (ManagementObject nAdapterObject in this.NetworkAdapterSearcher.Get())
            {
                object[] param = new object[0];
                string methodName;
                if (AdapterEnable)
                {
                    methodName = "Enable";
                }
                else
                {
                    methodName = "Disable";
                }
                nAdapterObject.InvokeMethod(methodName, param);
            }
        }
        public Dictionary<string,bool> GetNetworkAdapterStatus()
        {
            Dictionary<string, bool> nAdapters = new Dictionary<string, bool>();

            foreach (ManagementObject nAdapterObject in this.NetworkAdapterSearcher.Get())
            {
                nAdapters.Add(nAdapterObject["NetConnectionId"].ToString(), (ushort)nAdapterObject["NetConnectionStatus"] == 2);
            }
            return nAdapters;
        }

        public static Dictionary<string, bool> GetNetworkAdapters()
        {
            SelectQuery networkAdapterSelectQuery = new SelectQuery(NetworkAdapterSearchQuery);
            ManagementObjectSearcher networkAdapterSearcher = new ManagementObjectSearcher(networkAdapterSelectQuery);
            Dictionary<string, bool> networkAdapterDict = new Dictionary<string, bool>();

            foreach (ManagementObject nAdapterObject in networkAdapterSearcher.Get())
            {
                networkAdapterDict.Add(nAdapterObject["NetConnectionId"].ToString(), (ushort)nAdapterObject["NetConnectionStatus"] == 2);
            }
            return networkAdapterDict;
        }
    }
}
