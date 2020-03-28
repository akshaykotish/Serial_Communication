using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;

namespace Titanium_Corp
{
    public partial class Form1 : Form
    {
        bool isConnected = false;
        string[] ports;
        SerialPort port;

        void Read_Aurdino()
        {
            if (isConnected)
            {
                if (checkBox2.Checked)
                {
                    port.Write("#USSN\n");
                }
                if(checkBox6.Checked)
                {
                    //port.Write("#BSSN\n");
                }
                if (checkBox3.Checked)
                {
                    try
                    {
                        string data_rx = port.ReadLine();
                        data_rx.Replace('\r', ' ');
                        string output = "";
                        for (int i = 5; i < data_rx.Length; i++)
                        {
                            if (data_rx[i] == '\r')
                            {
                                break;
                            }
                            else
                            {
                                output = output + data_rx[i];
                            }
                        }
                        if(data_rx[0] == 'U')
                        {
                            UltraSonic_Value(output);
                        }
                        if (data_rx[1] == 'B')
                        {
                            Buzzer_Value(output);
                        }
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.ToString());
                    }
                }
            }
        }

        public void UltraSonic_Value(string value)
        {
            if(Digital_Ultarsonic_Lbl.InvokeRequired)
            {
                this.Digital_Ultarsonic_Lbl.BeginInvoke((MethodInvoker)delegate ()
                {
                    this.Digital_Ultarsonic_Lbl.Text = value;
                });
            }
            else
            {
                Digital_Ultarsonic_Lbl.Text = value;
            }
        }

        public void Buzzer_Value(string value)
        {
            if (Digital_Buzzer_Lbl.InvokeRequired)
            {
                this.Digital_Buzzer_Lbl.BeginInvoke((MethodInvoker)delegate ()
                {
                    this.Digital_Buzzer_Lbl.Text = value;
                });
            }
            else
            {
                Digital_Buzzer_Lbl.Text = value;
            }

            Digital_Buzzer_Lbl.BackColor = Color.Red;
        }



        public Form1()
        {
            InitializeComponent();
            Disable_Controls();
            Get_Availanle_COM_Ports();

            foreach(string port in ports)
            {
                comboBox1.Items.Add(port);
                if(ports[0] != null)
                {
                    comboBox1.SelectedItem = ports[0];
                }
            }

           

        }

        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data_rx = port.ReadExisting();
            UltraSonic_Value(data_rx);
        }

        public void Get_Availanle_COM_Ports()
        {
            ports = SerialPort.GetPortNames();
        }

        public void Disable_Controls()
        {
            checkBox1.Enabled = false;
        }


        public void Enable_Controls()
        {
            checkBox1.Enabled = true;
        }


        public void Connect_To_Aurdino()
        {
            isConnected = true;
            string selectedPort = comboBox1.GetItemText(comboBox1.SelectedItem);
            port = new SerialPort(selectedPort, 9600, Parity.None, 8, StopBits.One);
            port.Open();
            port.Write("#START\n");
            button1.Text = "Disconnect";
            Enable_Controls();
            //port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
        }

        public void Disconnect_From_Aurdino()
        {
            isConnected = false;
            port.Write("#STOP\n");
            port.Close();
            button1.Text = "Connect";
            Disable_Controls();
        }


        private void resetDefaults()
        {
            checkBox1.Checked = false;

        }
        private void button1_Click(object sender, EventArgs e)
        {
            if(!isConnected)
            {
                Connect_To_Aurdino();
            }
            else
            {
                Disconnect_From_Aurdino();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(isConnected)
            {
                if(checkBox1.Checked)
                {
                    port.Write("#LEDON\n");
                }
                else
                {
                    port.Write("#LEDOF\n");
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Thread recieve_rx = new Thread(new ThreadStart(Read_Aurdino));
            recieve_rx.Start();
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
