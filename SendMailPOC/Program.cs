using System;
using System.Net;
using System.Net.Mail;
using System.Security;

namespace SendMailPOC
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Send Mail Start");
            try
            {
                MailMessage mail = getMailMessage();

                SmtpClient client = getSmtpClient();

                Console.WriteLine("Sending mail");
                client.Send(mail);
                Console.WriteLine("Send Mail Successful");
                Console.WriteLine("Based on size of attachment it may take some time to be delivered");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Occured");
                Console.WriteLine(ex.ToString());
            }
            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();
        }

        static MailMessage getMailMessage()
        {
            MailMessage mail = new MailMessage();

            MailAddress fromMailAddress = getMailAddress("From");
            MailAddress toMailAddress = getMailAddress("To");

            string subject = getInputFromConsole("mail subject");
            string body = getInputFromConsole("mail body");

            bool isToAttachFiles = bool.Parse(getInputFromConsole("'is to attach file?' (true/false)"));
                
            mail.To.Add(toMailAddress);
            mail.From = fromMailAddress;
            mail.Subject = subject;
            mail.Body = body;

            if (isToAttachFiles)
            {
                Attachment attachment = new Attachment(getInputFromConsole("attachment file path"));
                mail.Attachments.Add(attachment);
            }

            return mail;
        }

        static MailAddress getMailAddress(string role)
        {
            string emailAdd = getInputFromConsole($"{role} email address");
            return new MailAddress(emailAdd);
        }

        static SmtpClient getSmtpClient()
        {
            SmtpClient client = new SmtpClient();
            string hostName = getInputFromConsole("host name");
            int port = Int32.Parse(getInputFromConsole("port number (numeric only)"));
            bool isSSLEnabled = bool.Parse(getInputFromConsole("'SSL Enable?' (true/false)"));
            
            client.Port = port;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.EnableSsl = isSSLEnabled;
            client.Host = hostName;
            
            client.Credentials = getNetworkCredential();

            return client;
        }

        static NetworkCredential getNetworkCredential()
        {
            Console.WriteLine("\n*****Provide Log-in details*****");
            string userName = getInputFromConsole("user-name/e-mail address");
            SecureString password = getInputFromConsoleSecure("password");
            NetworkCredential networkCredential = new NetworkCredential(userName, password);
            return networkCredential;
        }

        static string getInputFromConsole(string field)
        {
            Console.WriteLine($"\nEnter value for {field}");
            return Console.ReadLine();
        }

        static SecureString getInputFromConsoleSecure(string field)
        {
            Console.WriteLine($"\nEnter value for {field} - This is Secure Medium");
            SecureString secureString = new SecureString();
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                // Backspace Should Not Work
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    secureString.AppendChar(key.KeyChar);
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace || key.Key == ConsoleKey.Escape)
                    {
                        Console.WriteLine($"Re-Enter whole value for {field} ");
                        return getInputFromConsoleSecure(field);
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        Console.WriteLine();
                        break;
                    }
                }
            } while (true);
            return secureString;
        }
    }
}
