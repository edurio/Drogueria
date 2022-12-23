using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;
using System.Net;

namespace TestMail
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EnviarConfirmarEmail("eduardo.rios@erex.cl", "Eduardo Ríos", "Test Mensajería", string.Empty);
        }

        public void EnviarConfirmarEmail(string correo, string nombre, string subject, string html)
        {
            MailAddress from = new MailAddress("info@mensajeriabackline.cl", "Mensajería Droguerías");
            MailAddress to = new MailAddress(correo, nombre);
            MailMessage mail = new MailMessage(from, to);

            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;  //pueden probar con los puertos arriba disponibles              
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.ServicePoint.MaxIdleTime = 1;
            client.Timeout = 12000;
            client.Credentials = new NetworkCredential("info@mensajeriabackline.cl", "csrwxypjbgnmruho");
            mail.IsBodyHtml = true;
            mail.Subject = subject;
            mail.Body = html;

            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                //DAL.LogDAL.LogInsertar(new Log() { Tipo = "Error", Modulo = "Login", Descripcion = "Envio mail a" + usuario.Correo, Error = ex.InnerException.Message });
            }
            mail.Dispose();
        }
    }
}
