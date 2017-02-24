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
    public string password = "1Computer5";
    public string smtpServerURL = "smtp.gmail.com";
   
    public void sendEmail()
    {
        MailMessage mail = new MailMessage()
        {
            From = new MailAddress(fromEmail),
            Subject = subject,
            Body = body
            
        };
        mail.To.Add(toEmail);
        if (filePath != null)
        {
            Debug.Log("Filepath: " + filePath);
            Attachment gif = new Attachment(filePath);
            mail.Attachments.Add(gif);
        }

        SmtpClient smtpServer = new SmtpClient(smtpServerURL)
        {
            Port = 587,
            Credentials = new NetworkCredential(fromEmail, password) as ICredentialsByHost,
            EnableSsl = true
        };
        ServicePointManager.ServerCertificateValidationCallback =
            delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            { return true; };
        smtpServer.Send(mail);
        Debug.Log("success");
        filePath = null;
    }
}