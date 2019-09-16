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
using System.Net;
using System.Reflection;
using System.Xml;
using HtmlAgilityPack;
using LibGit2Sharp;




namespace Project1
{
    public partial class Form1 : Form
    {
        private string data;//okunan veri için 
        
        

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
                comboBox1.Enabled = true;
                baglanButton.Enabled = false;
                bagkesButton.Enabled = true;
                testButton.Enabled = true;
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                MessageBox.Show("Bağlanıldı.");
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
            var html = @"https://github.com/MehmetAkifUrgen/C-_SeriPort_Iletimi/blob/master/Seri%20Port%20Iletimi%20v2.0/Debug/readMe.txt";
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(html);
            var node = htmlDoc.DocumentNode.SelectSingleNode("//td[@id='LC1']");

            string ver = "1.0";
            

            if (node.OuterHtml.Contains(ver))
            { MessageBox.Show("Sürüm Güncel"); }
            else
            {
                MessageBox.Show("Yeni Sürüm var!");
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                WebClient webClient = new WebClient();
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
               // webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                Uri uri = new Uri("https://github.com/MehmetAkifUrgen/C-_SeriPort_Iletimi/blob/master/Project1/bin/Debug/Project1.exe");
                try
                {
                    webClient.DownloadFileAsync(uri, @"C:\Program Files (x86)\SeriPortIletimi\Project1.exe");
                    Form1 form1 = new Form1();
                    Hide();
                    form1.Close();

                }
                catch (Exception ex)
                {

                    Console.WriteLine("Download Error :" + ex.Message.ToString());
                }


                //client.DownloadFile("https://github.com/MehmetAkifUrgen/C-_SeriPort_Iletimi/blob/master/Project1/bin/Debug/Project1.exe", @"C:\Program Files (x86)\Seri Port Iletimi v2.0");
                //System.Diagnostics.Process.Start("https://github.com/MehmetAkifUrgen/C-_SeriPort_Iletimi/raw/master/Seri%20Port%20Iletimi%20v2.0/Debug/Seri%20Port%20Iletimi%20v2.0.msi");
                // Repository.Clone("https://github.com/MehmetAkifUrgen/C-_SeriPort_Iletimi/blob/master/Project1/bin/Debug/Project1.exe\\git", @"C:\SeriPortIletimi");
            }
        }
       /* private static void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            MessageBox.Show(e.ProgressPercentage + " %");
        }*/
        private static void Completed(object sender, AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Download completed!");
            
        }


        private void baudComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen)
            {
                serialPort1.BaudRate =Convert.ToInt32(baudComboBox.SelectedItem.ToString()); //baudrate de hız seçme
            }

        }

        
    }
}
