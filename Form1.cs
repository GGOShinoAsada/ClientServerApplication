using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClientServerApplication.ClientServer;


namespace ClientServerApplication
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }
        private void block(bool flag)
        {
            button1.Enabled = flag;
            button2.Enabled = flag;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Task.Delay(100);
            Client client = new Client();
            client.Connect(textBox1.Text);
            richTextBox1.Text += client.Validate() ? client.AllOutput.ToString() : client.AllErrors.ToString();
            //block(false);
            //ClientServerExecute ex = new ClientServerExecute();
            //ex.StartService(8080, textBox1.Text);
            //richTextBox1.Text += ex.Output.ToString()+"\n";
            //block(true);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            textBox1.Text = "";
        }
    }
}
