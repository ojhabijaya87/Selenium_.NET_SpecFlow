using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ApiLibrary
{
    public class ApiHelper
    {
        private static Task<RestResponse> response;

        public static Task<RestResponse> CreateRequest(object payload, string method, RestClient restClient, string resource)
        {
            RestRequest request = new RestRequest();
            var dict = new Dictionary<string, string>
            {
                { "Accept", "application/json" }
            };
            request.AddHeaders(dict);
            request.Resource = resource;
            switch (method)
            {
                case Methods.GET:
                    request.Method = Method.Get;
                    response = restClient.ExecuteAsync(request);
                    break;
                case Methods.POST:
                    request.AddJsonBody(JsonConvert.SerializeObject(payload));
                    //request.Method = Method.Post;
                    response = restClient.PostAsync(request);
                    break;
                case Methods.PUT:
                    //request.AddJsonBody(payload);
                    request.AddJsonBody(JsonConvert.SerializeObject(payload));
                    //request.Method = Method.Put;
                    response = restClient.PutAsync(request);
                    break;
                case Methods.DELETE:
                    request.AddJsonBody(payload);
                    request.Method = Method.Delete;
                    response = restClient.ExecuteAsync(request);
                    break;
            }
            
             
            return response;
        }

      
    }

    public static class Methods
    {
        public const string GET = "GET";
        public const string POST = "POST";
        public const string PUT = "PUT";
        public const string DELETE = "DELETE";
    }
}
