using System;
using System.Configuration;

namespace SalesForceAPI
{
    class Program
    {
        private static SalesForceClient CreateClient()
        {
            return new SalesForceClient
            {
                Username = ConfigurationManager.AppSettings["username"],
                Password = ConfigurationManager.AppSettings["password"],
                Token = ConfigurationManager.AppSettings["token"],
                ClientId = ConfigurationManager.AppSettings["clientId"],
                ClientSecret = ConfigurationManager.AppSettings["clientSecret"]
            };
        }
        static void Main(string[] args)
        {
            var client = CreateClient();
            client.login();
            Console.WriteLine(client.Query("select rating from account"));
        }
    }
}
