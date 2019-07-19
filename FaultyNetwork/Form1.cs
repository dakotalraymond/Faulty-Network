using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using System.Text;

namespace FaultyNetwork
{
    public partial class Form1 : Form
    {
        private Thread client1Thread;
        private Thread client2Thread;
        private bool done = false;
        private UdpClient client1;
        private UdpClient client2;
        private IPEndPoint client1Ep = null;
        private IPEndPoint client2Ep = null;


        public Form1()
        {
            InitializeComponent();
            
            client1 = new UdpClient(1982);
            client2 = new UdpClient(1983);

            client1Thread = new Thread(new ThreadStart(Client1Handler));
            client2Thread = new Thread(new ThreadStart(Client2Handler));

            client1Thread.Start();
            client2Thread.Start();

            PrintText("Listening for client 1 on port 1982");
            PrintText("Listening for client 2 on port 1983");
        }

        private delegate void StringDelegate(string message);
        private void PrintText(string message)
        {
            if (printCheckBox.Checked)
            {
                if (outputBox.InvokeRequired)
                {
                    outputBox.Invoke(new StringDelegate(PrintText), new object[] { message });
                }
                else
                {
                    outputBox.AppendText(message + "\r\n");
                }
            }
        }

        private void PrintTextAlt(string message)
        {
            Invoke(new Action<string>(m =>
            {
                m = message;
                // do stuff
            }));
        }

        private byte[] corruptThis(byte[] message)
        {
            int corruptRate = (int)corruptionRateBox.Value;
            int dropRate = (int)dropRateBox.Value;

            Random bob = new Random();

            if(bob.Next(1, 101) > dropRate)
            {
                // Keep this message
               
                for (int i = 0; i < message.Length; ++i)
                {
                    if (bob.Next(1, 101) < corruptRate)
                    {
                        // corrupt this byte
                        int shift = bob.Next(1, 9);
                        message[i] ^= (byte)(0x01 << shift);
                        PrintText(string.Format("Corrupted bit {0} of byte {1}", shift, i));
                    }
                }                
            }
            else
            {
                message = null;
                PrintText("Dropped message");
            }

            return message;
        }

        private void Client1Handler()
        {                        
            while (!done)
            {
                byte[] message = client1.Receive(ref client1Ep);

                if (message.Length > 1 && message[0] != '~')
                {
                    string text = Encoding.ASCII.GetString(message);
                    PrintText("Client1: " + text);

                    message = corruptThis(message);

                    if (message != null && client2Ep != null)
                    {
                        client2.Send(message, message.Length, client2Ep);
                    }
                }
            }
        }

        private void Client2Handler()
        {
            while (!done)
            {
                byte[] message = client2.Receive(ref client2Ep);
                if (message.Length > 1 && message[0] != '~')
                {
                    string text = Encoding.ASCII.GetString(message);
                    PrintText("Client2: " + text);

                    message = corruptThis(message);

                    if (message != null && client1Ep != null)
                    {
                        client1.Send(message, message.Length, client1Ep);
                    }
                }
            }           
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            done = true;
            UdpClient temp = new UdpClient();

            byte[] unblockMessage = new byte[1];
            unblockMessage[0] = (byte)'~';
            temp.Send(unblockMessage, unblockMessage.Length, new IPEndPoint(IPAddress.Loopback, 1982));temp.Send(unblockMessage, unblockMessage.Length, new IPEndPoint(IPAddress.Loopback, 1982));
            temp.Send(unblockMessage, unblockMessage.Length, new IPEndPoint(IPAddress.Loopback, 1982));temp.Send(unblockMessage, unblockMessage.Length, new IPEndPoint(IPAddress.Loopback, 1983));
        }
    }
}
