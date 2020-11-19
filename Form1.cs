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

        private void button1_Click(object sender, EventArgs e)
        {
            Task.Delay(100);
            Client r = new Client(8080, textBox1.Text);
            richTextBox1.Text += r.Output.ToString();
            //ClientServerExecute client = new ClientServerExecute();
            //client.Execute(textBox1.Text);
         
            //if (client.Validate())
            //{
            //    richTextBox1.Text = client.Output.ToString();
            //}
            //else
            //{
            //    foreach (string t in ClientServerExecute.Errors)
            //        richTextBox1.Text += t + "\n";
            //}
            ////ClientServerExecute ex = new ClientServerExecute(textBox1.Text);
            //ex.StartService(8080, textBox1.Text);
            //if (ex.Validate())
            //{
            //    richTextBox1.Text = ex.Output.ToString();
            //}
            //else
            //{
            //    richTextBox1.Text = "ERROR";
            //}
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            textBox1.Text = "";
        }
    }
}
