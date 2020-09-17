using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using SharpUpdate;
using System.Reflection;

namespace AceRPLauncher
{
    public partial class Form1 : Form
    {
        private SharpUpdater updater;
        public string ipSRV = "aceroleplay.xyz";
        public string DiscordLink = "https://discord.gg/WnCsrXK";
        public string ts3IP = "aceroleplay.xyz";
        int mov;
        int movX;
        int movY;
        public Form1()
        {
            InitializeComponent();
            updater = new SharpUpdater(Assembly.GetExecutingAssembly(), this, new Uri("http://aceroleplay.xyz/launcher/version.xml"));
            updater.DoUpdate();
            label5.Text = ProductVersion;
        }
    

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private string responseFromServer;
        private string status;
        private string svCon;
        bool ipServ = false;
        private void Form1_Load(object sender, EventArgs e)
        {

            svCon = "http://" + ipSRV + "/players.json";
            WebRequest request = WebRequest.Create(svCon);
            request.Credentials = CredentialCache.DefaultCredentials;
            ipServ = isExist(svCon);
            if (ipServ)
            {
                WebResponse response = request.GetResponse();
                status = ((HttpWebResponse)response).StatusDescription;
                Console.WriteLine(status);

                using (Stream dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    responseFromServer = reader.ReadToEnd();
                    Console.WriteLine(responseFromServer);


                }


                response.Close();
                label2.Text = "Online";
                listBox1.Visible = true;
                panel3.BackColor = Color.LawnGreen;
                Player[] item = JsonConvert.DeserializeObject<Player[]>(responseFromServer);
                var countPlayers = item.Count();
                label1.Text = countPlayers.ToString() + " Cidadãos Conectados";
                listBox1.Items.Clear();

                foreach (Player name in item)
                    listBox1.Items.Add(name.name);






            }
            else
            {
                label2.Text = "Offline";
                panel3.BackColor = Color.Red;
                label1.Text = "Visite o nosso Discord ou Teamspeak para mais informações";
                button2.Visible = false;
                listBox1.Visible = false;
                panel4.Visible = false;
                label4.Visible = false;
            }

    }


        private bool isExist(string ipSRV)
        {
            WebRequest webRequest = HttpWebRequest.Create(svCon);
            webRequest.Method = "HEAD";
            try
            {
                using (WebResponse webResponse = webRequest.GetResponse())
                {
                    return true;
                }
            }
            catch //(WebException ex)
            {
                return false;
            }
        }

        public class Player
        {
            public string endpoint { get; set; }
            public int id { get; set; }
            public string[] identifiers { get; set; }
            public string name { get; set; }
            public int ping { get; set; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Process[] steam = Process.GetProcessesByName("steam");
            if (steam.Length == 0)
            {
                MessageBox.Show("Abre a steam antes de entrares no servidor.");
                Close();
            }
            else
            {

                System.Diagnostics.Process.Start($"fivem://connect/" + ipSRV);
                Thread.Sleep(3000);
                Close();


            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mov = 1;
            movX = e.X;
            movY = e.Y;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if(mov == 1)
            {
                this.SetDesktopLocation(MousePosition.X - movX, MousePosition.Y - movY);
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mov = 0 ;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(DiscordLink);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("ts3server://" + ts3IP);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://"+ts3IP);
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            button1.ForeColor = Color.Red;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.ForeColor = Color.White;
        }
    }
}
