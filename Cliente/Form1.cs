using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace Cliente
{
    public partial class Form1 : Form
    {
        static private NetworkStream stream;
        static private StreamWriter streamEscritor;
        static private StreamReader streamLector;
        static private TcpClient Cliente = new TcpClient();
        private delegate void DAddItem(String s);

        public Form1()
        {
            InitializeComponent();
            Conectar();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddItem("Enviando...");
            string texto = "@citasA@"+ ','+ textBox1.Text + ',' + dateTimePicker1.Value + ',' + textBox2.Text;
            enviar(texto);
        }

        private void AddItem(String s)
        {
            listBox1.Items.Add(s);
        }

        private void enviar(string text)
        {
            streamEscritor.WriteLine(text);
            streamEscritor.Flush();
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
        }

        void Listen()
        {
            string read = string.Empty;

            while (Cliente.Connected)
            {
                try
                {
                    read = streamLector.ReadLine();
                    this.Invoke(new DAddItem(AddItem), read);
                }
                catch
                {
                    Console.WriteLine("Algo pasa!!!");
                    
                }
                
            }
        }

        void Conectar()
        {
            try
            {
                Cliente.Connect("192.168.8.185", 5000);
                if (Cliente.Connected)
                {
                    Thread t = new Thread(Listen);

                    stream = Cliente.GetStream();
                    streamEscritor = new StreamWriter(stream);
                    streamLector = new StreamReader(stream);

                    streamEscritor.WriteLine("Conectado");
                    streamEscritor.Flush();

                    t.Start();
                }
                else
                {
                    MessageBox.Show("Servidor no disponible");
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Servidor no disponible");
                Application.Exit();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            AddItem("Recibiendo...");
            enviar("@citasV@");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
