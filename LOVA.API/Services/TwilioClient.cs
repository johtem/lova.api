﻿using LOVA.API.Services;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Twilio.Clients;
using Twilio.Http;
using SystemHttpClient = System.Net.Http.HttpClient;


namespace LOVA.API.Services
{
public class TwilioClient : ITwilioRestClient
    {
        private readonly ITwilioRestClient _innerClient;

        public TwilioClient(IConfiguration config, SystemHttpClient httpClient)
        {
            // customize the underlying HttpClient
            httpClient.DefaultRequestHeaders.Add("X-Custom-Header", "CustomTwilioRestClient-Demo");

            _innerClient = new TwilioRestClient(
                config["Twilio:AccountSid"],
                config["Twilio:AuthToken"],
                httpClient: new SystemNetHttpClient(httpClient));
        }

        public Response Request(Request request) => _innerClient.Request(request);
        public Task<Response> RequestAsync(Request request) => _innerClient.RequestAsync(request);
        public string AccountSid => _innerClient.AccountSid;
        public string Region => _innerClient.Region;
        public Twilio.Http.HttpClient HttpClient => _innerClient.HttpClient;
    }
}
