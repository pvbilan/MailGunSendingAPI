using System;
using System.Net;
using System.Net.Mail;
using RestSharp;

namespace MailGunSendingAPI
{
    class Program
    {
        private static String MailGunAPIUrl = "https://api.mailgun.net/v3";
        private static String MailGunAPIKey = "key-..";
        private static String MailGunDomainName = "e.example.com";

        static void Main(string[] args)
        {
            using (MailMessage mailMessage = new MailMessage())
            {
                IRestResponse mailGunResponse = SendViaMailGunAPI(mailMessage);

                if (mailGunResponse.StatusCode == HttpStatusCode.OK)
                {
                    Console.WriteLine(String.Format("Email was sent successfully"));
                }
                else
                {
                    Console.WriteLine(String.Format("Email wasn't sent!"));
                    Console.WriteLine(String.Format("MailGun Status Code: {0};", mailGunResponse.StatusCode.ToString()));
                    Console.WriteLine(String.Format("MailGun Status Description: {0};", mailGunResponse.StatusDescription));
                    Console.WriteLine(String.Format("Error Message: {0};", mailGunResponse.ErrorMessage));
                    Console.WriteLine(String.Format("Error Exception: {0};", mailGunResponse.ErrorException));
                }
            }
        }

        static IRestResponse SendViaMailGunAPI(MailMessage mailMessage)
        {
            RestClient client = new RestClient();
            client.BaseUrl = new Uri(MailGunAPIUrl);
            client.Authenticator = new HttpBasicAuthenticator("api", MailGunAPIKey);

            RestRequest request = new RestRequest();
            request.AddParameter("domain", MailGunDomainName, ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", mailMessage.From);
            request.AddParameter("to", mailMessage.To);
            request.AddParameter("subject", mailMessage.Subject);
            request.AddParameter("html", mailMessage.Body);
            request.Method = Method.POST;

            return client.Execute(request);
        }
    }
}