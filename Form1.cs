using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading;
using System.IO;

namespace MassSMS
{
    public partial class Form1 : Form
    {
        private Boolean cancel = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("For a gmail to have access to a \n" +
                "third-party application access must be granted.\n" +
                "Go to MyAccount > Sign-in & security\n" +
                " > Connected apps & sites > Allow less secure apps");
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            textBox3.Text = openFileDialog1.FileName;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                buttonSend.Enabled = false;
                const Int32 BufferSize = 128;
                using (var fileStream = File.OpenRead(textBox3.Text))
                {
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                    {
                        var listSize = File.ReadLines(textBox3.Text).Count();
                        progressBar1.Value = 0;
                        progressBar1.Maximum = listSize;
                        String line;
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            send(line);
                           progressBar1.Value++;
                        }
                       buttonSend.Enabled = true;
                    }
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                buttonSend.Enabled = true;
            }
        }

        private void send(String addr)
        {
            try
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Timeout = 750;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = true;
                client.Credentials = new NetworkCredential(textBox1.Text, textBox2.Text);
                MailMessage msg = new MailMessage();
                msg.To.Add(addr);
                msg.From = new MailAddress(textBox1.Text);
                msg.Subject = textBox4.Text;
                msg.Body = richTextBox1.Text;
                client.Send(msg);
                
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
        }
    }
}
