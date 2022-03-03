using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace Cabflix.Utils
{
    public class Email
    {
        public string EnviarEmail(){

            string msg = "Email enviado com sucesso!";

            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.Host = "smtp.londrimax.com.br";
            client.EnableSsl = true;
            client.Credentials = new System.Net.NetworkCredential("mauro@londrimax.com.br", "685ww_9%ZBhp");
            MailMessage mail = new MailMessage();
            mail.Sender = new System.Net.Mail.MailAddress("mauro@londrimax.com.br", "Mauro Andrade");
            mail.From = new MailAddress("mauro@londrimax.com.br", "Mauro Andrade");
            mail.To.Add(new MailAddress("nocnoier@hotmail.com", "RECEBEDOR"));
            mail.Subject = "Contato";
            //mail.Body = " Mensagem do site:<br/> Nome:  " + senderName.Text + "<br/> Email : " + senderEmail.Text + " <br/> Mensagem : " + message.Text;
            mail.Body = " Mensagem do site:<br/> Nome:  Mauro Andrade <br/> Email : mauro@londrimax.com.br <br/> Mensagem : Testando Email";
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            try
            {
                client.Send(mail);
                return msg;
            }
            catch (System.Exception erro)
            {
                //trata erro
                msg = "Erro: ";
                return msg + erro.Message;
            }
            finally
            {
                mail = null;
            }
        }

    }
}