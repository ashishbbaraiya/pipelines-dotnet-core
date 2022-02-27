using DocumentFormat.OpenXml.Bibliography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace APIDemo
{
    public class APIHelper<T>
    {
        public RestClient restClient;
        public RestRequest restRequest;
        public string baseURL = "https://reqres.in/";

        public RestClient SetUrl(string endpoint)
        {
            var url = Path.Combine(baseURL, endpoint);
            var restClient = new RestClient(url);

            return restClient;
        }


        public RestRequest CreateGetRequest()
        {
            RestRequest restRequest = new RestRequest();
            //restRequest.AddHeader("Accept", "application/json");
            //restRequest.RequestFormat = DataFormat.Json;
            return restRequest;
        }

        public RestRequest CreateDeleteRequest(string endpoint)
        {
            RestRequest restRequest = new RestRequest(endpoint, Method.Delete);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.RequestFormat = DataFormat.Json;
            return restRequest;
        }

        public RestRequest CreatePostRequest(string endpoint, string jsonArray)
        {
            var restRequest = new RestRequest(endpoint, Method.Post);

            //// FOLLOWING 2 LINES ARE NOT WORKING. FOUND ALTERNATIVE FOR NOW.
            //restRequest.AddHeader("Accept", "application/json");
            //restRequest.AddParameter("application/json", jsonArray, ParameterType.RequestBody);

            //// THIS IS THE ALTERNATIVE WAY.
            JObject o = JObject.Parse(jsonArray);
            foreach (JProperty property in o.Properties())
            {
                restRequest.AddParameter(property.Name.ToString(), property.Value.ToString());
                Console.WriteLine(property.Name + " - " + property.Value);
            }

            return restRequest;
        }

        public RestRequest CreatePutRequest(string endpoint, string jsonArray)
        {
            var restRequest = new RestRequest(endpoint, Method.Put);

            JObject o = JObject.Parse(jsonArray);
            foreach (JProperty property in o.Properties())
            {
                restRequest.AddParameter(property.Name.ToString(), property.Value.ToString());
                Console.WriteLine(property.Name + " - " + property.Value);
            }

            return restRequest;
        }

        public async Task<RestResponse> GetResponse(RestClient client, RestRequest request)
        {
            return await client.ExecuteAsync(request);
        }

        public DTO GetContent<DTO>(RestResponse response)
        { 
            var content = response.Content;
            DTO dtoObject = JsonConvert.DeserializeObject<DTO>(content);
            return dtoObject;
        }

        public string Serialize(dynamic content)
        {
            string serializeObject = JsonConvert.SerializeObject(content,Formatting.Indented);
            return serializeObject;
        }
    }
}
