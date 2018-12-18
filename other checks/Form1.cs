using System;
using System.Net;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace other_checks
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //Подключе
        [DllImport("iphlpapi.dll", ExactSpelling = true)] 
        public static extern int SendARP(int destIp, int srcIP, byte[] macAddr, ref uint physicalAddrLen);
        private void button1_Click(object sender, EventArgs e)
        {
            string name, mac;
            string ip = "192.168.0.106";

            //Определение имени
            IPAddress a = IPAddress.Parse(ip);
            try
            {
                IPHostEntry entry = Dns.GetHostByAddress(a);
                name = entry.HostName;
            }
            catch { name = "Не удалось определить DNS"; };

            //Определение mac-адреса
            byte[] macAddr = new byte[6];
            uint macAddrLen = (uint)macAddr.Length;

            if (SendARP(BitConverter.ToInt32(a.GetAddressBytes(), 0), 0, macAddr, ref macAddrLen) == 0)
            {
                string[] str = new string[(int)macAddrLen];
                for (int i = 0; i < macAddrLen; i++)
                    str[i] = macAddr[i].ToString("x2");

                mac = string.Join(":", str);
            }
            else
                mac = " Не удалось определить";
            label1.Text = "Имя: " + name + " Mac-Адрес: "+mac;
        }
    }
}
