using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Project.Common.Tools
{
    public static class MailSender
    {

        #region TestEmailiVeSifre
        //testemail3172@gmail.com

        //rvzhpxwpegickwtq 
        #endregion

        //Gönderim yapacak olan mail'den Google Account'a gidin oradan Security alanına girin..
        //Signing in Google 2-Step Verification bunu secin
        //Sayfanın alt tarafına dogru Application Passwords(uygulama sifreleri) Buradan bir tanesini secip password generate ettirin...
        //Cıkan pass word 4 tane kelimeden olusacak ve bosluklu olacak..O boslukları silip password'unuzu elde edebilirsiniz...Mail gönderimleriniz buradaki gibi baska bir proje üzerinden olacak ise sistemlerden sizden artık o generate ettirilen ve boslukları temizlenmiş olan sifreyi isterler...

        //    casad  sadasd  dasdasd  dasdasd
        //casadsadasddasdasddasdasd

        public static void Send(string receiver, string password = "rvzhpxwpegickwtq", string body = "Test mesajıdır", string subject = "Test", string sender = "testemail3172@gmail.com")
        {
            MailAddress senderEmail = new MailAddress(sender);
            MailAddress receiverEmail = new MailAddress(receiver);

            //Bizim Email işlemlerimiz SMTP'ye göre yapılır...
            //smtp.office365.com , smtp.outlook.com,smtp.gmail.com

            //Kullandıgınız sunucunun ayarlarını vermeden önce 2-Step verification'i yaptıgınızdan emin olmalısınız..

            SmtpClient smtpClient = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail.Address, password)
            };


            using (MailMessage message = new MailMessage(senderEmail, receiverEmail)
            {
                Subject = subject,
                Body = body
            })
            {
                smtpClient.Send(message);
            }



        }



    }
}
