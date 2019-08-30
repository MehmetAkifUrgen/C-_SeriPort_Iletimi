using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.IO.Ports;// Seri Port için kütüphane
using System.Threading;

namespace Project1
{
    public partial class Form1 : Form
    {
        private string data;
        

        public Form1()
        {
            InitializeComponent();
            bagkesButton.Enabled = false;
            testButton.Enabled = false;
            textBox1.Enabled = false;
            textBox2.Enabled = false;

        }
       
        private void sp_DataReceived(object sender,SerialDataReceivedEventArgs e) // sürekli veri okuması için gereken fonksiyon
        {
            data = serialPort1.ReadExisting();
            this.Invoke(new EventHandler(DisplayText));
        }


        private void DisplayText(object sender, EventArgs e)// textBox'a yazdıran fonksiyon
        {
            textBox2.AppendText(data);
            
        }


        public void Form1_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames(); //Bağlı seri portları diziye aktardık
            foreach (string portAdi in ports)
            {
                comboBox1.Items.Add(portAdi);
            }
            if (serialPort1.IsOpen)
            {
                
                serialPort1.Handshake = Handshake.None;
                serialPort1.ReadTimeout = 500;
                serialPort1.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);// Read Etmek için fonksiyonumuzu sürekli çağırıyoruz.
            }
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen)
            {
                serialPort1.PortName = comboBox1.SelectedItem.ToString(); //combobox ta seçilen seri porta seçme
            }
            
        }

        private void baglanButton_Click(object sender, EventArgs e)//bağlantı butonu
        {
            try
            {
                serialPort1.Open();
                progressBar1.Value = 100;
                
                baglanButton.Enabled = false;
                bagkesButton.Enabled = true;
                testButton.Enabled = true;
                textBox1.Enabled = true;
                textBox2.Enabled = true;
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message);
            }
            
        }

        private void bagkesButton_Click(object sender, EventArgs e)//bağlantı kesme butonu
        {
            try
            {
                serialPort1.Close();
                progressBar1.Value = 0;
                bagkesButton.Enabled = false;
                baglanButton.Enabled = true;
                
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
            
        }
        private void testButton_Click(object sender, EventArgs e)// yazdırma butonu
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Write(textBox1.Text);
                textBox1.Clear();
            }

        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Henüz Etkin Değil");
        }
    }
}
