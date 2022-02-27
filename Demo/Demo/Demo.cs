using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIDemo
{
    public class Demo<T>
    {
        public async Task<ListOfUsersDTO> GetUsers(string endpoint)
        {
            var user = new APIHelper<ListOfUsersDTO>();
            var url = user.SetUrl(endpoint);
            var request = user.CreateGetRequest();
            var response = await user.GetResponse(url, request);

            ListOfUsersDTO content = user.GetContent<ListOfUsersDTO>(response);
            return content;
        }

        public async Task<CreateUserDTO> CreateUser(string endpoint, dynamic jsonObject)
        {
            var user = new APIHelper<CreateUserDTO>();

            var url = user.SetUrl(endpoint);
            var jsonRequest = user.Serialize(jsonObject);
            var request = user.CreatePostRequest(endpoint, jsonRequest);

            RestResponse response = await user.GetResponse(url, request);
            CreateUserDTO content = user.GetContent<CreateUserDTO>(response);

            return content;
        }
    }
}