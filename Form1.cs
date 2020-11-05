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
            ClientServerExecute ex = new ClientServerExecute();
            ex.StartServiceAsync(8080, textBox1.Text);
            richTextBox1.Text = ex.Output.ToString();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            textBox1.Text = "";
        }
    }
}
