using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using Newtonsoft.Json;


namespace SalesForceAPI
{
    class SalesForceClient
    {
        private const string loginendPoint = "https://login.salesforce.com/services/oauth2/token";
        private const string apiEndpoint = "/services/data/v51.0/";

        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AuthToken { get; set; }
        public string InstanceUrl { get; set; }

        static SalesForceClient()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

        }

        public void login()
        {
            String jsonResponse;
            using(var client=new HttpClient())
            {
                var request = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "grant_type","password"},
                    { "client_id",ClientId},
                    { "client_secret",ClientSecret},
                    { "username",Username},
                    { "password",Password + Token}

                });
                request.Headers.Add("X-PreetyPrint", "1");
                var response = client.PostAsync(loginendPoint, request).Result;
                jsonResponse = response.Content.ReadAsStringAsync().Result;
                
            }
            Console.WriteLine($"Response:{jsonResponse}");
            var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResponse);            
            AuthToken = values["access_token"];
            InstanceUrl = values["instance_url"];
            Console.WriteLine("AuthToken=" + AuthToken);
            Console.WriteLine("InstanceURL=" + InstanceUrl);

        }

        public string Query(string soqlQuery)
        {
            using(var client=new HttpClient())
            {
                string restRequest = InstanceUrl + apiEndpoint + "query?q=" + soqlQuery;
                var request = new HttpRequestMessage(HttpMethod.Get, restRequest);
                request.Headers.Add("Authorization", "Bearer " + AuthToken);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Add("X-PrettyPrint", "1");
                var response = client.SendAsync(request).Result;

                return response.Content.ReadAsStringAsync().Result;


            }

        }

    }
}
