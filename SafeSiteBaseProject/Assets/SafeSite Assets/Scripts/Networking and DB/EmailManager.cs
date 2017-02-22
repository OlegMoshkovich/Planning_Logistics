using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;


public class EmailManager : MonoBehaviour
{
    public string toEmail { get; set; }
    public string fromEmail = "pablo@hcsafety.com";
    public string body { get; set; }
    public string subject = "SafeSite Report";
    public string filePath;
   
    public void sendEmail()
    {
        MailMessage mail = new MailMessage();

        mail.From = new MailAddress(fromEmail);
        mail.To.Add(toEmail);
        mail.Subject = subject;
        mail.Body = body;
        if (filePath != null)
        {
            Debug.Log("Filepath: " + filePath);
            Attachment gif = new Attachment(filePath);
            mail.Attachments.Add(gif);
        }
        

        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 587;
        smtpServer.Credentials = new System.Net.NetworkCredential("pablo@hcsafety.com", "1Computer5") as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback =
            delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            { return true; };
        smtpServer.Send(mail);
        Debug.Log("success");
        filePath = null;

    }
}