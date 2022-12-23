using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace Drogueria
{
    public class Mensajeria
    {
        public static void EnviarConfirmarEmail(string correo, string nombre, string subject, string html, string ruta)
        {
            var destinatarios = DAL.DestinatarioCorreoDAL.ObtenerDestinatarios();


            MailMessage mail = new MailMessage();
            MailAddress from = new MailAddress("info@mensajeriabackline.cl", "Mensajería Droguerías");
            

            //Agrego los destinatarios
            foreach (var d in destinatarios)
            {
                mail.To.Add(d.Correo);
            }

            mail.From = from;

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

            if (ruta != null && ruta.Trim().Length > 0)
            {
                mail.Attachments.Add(new Attachment(ruta));
            }

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