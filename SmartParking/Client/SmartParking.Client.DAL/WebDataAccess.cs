using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SmartParking.Client.DAL
{
    public class WebDataAccess
    {
        private string domain = "http://192.168.1.87:5002/api";

        public Task<string> GetDatas(string uri)
        {
            using (HttpClient client = new HttpClient())
            {
                var resp = client.GetAsync($"{domain}/{uri}").GetAwaiter().GetResult();
                return resp.Content.ReadAsStringAsync();
            }
        }

        public Task<string> PostDatas(string uri, Dictionary<string, HttpContent> contents)
        {
            using (HttpClient client = new HttpClient())
            {
                var resp = client.PostAsync($"{domain}/{uri}", this.GetFormData(contents)).GetAwaiter().GetResult();
                return resp.Content.ReadAsStringAsync();
            }
        }

        public Task PostDatas(string uri, HttpContent content)
        {
            using (HttpClient client = new HttpClient())
            {
                var resp = client.PostAsync($"{domain}/{uri}", content)
                                .GetAwaiter().GetResult();
                return resp.Content.ReadAsStringAsync();
            }
        }


        private MultipartFormDataContent GetFormData(Dictionary<string, HttpContent> contents)
        {
            var form = new MultipartFormDataContent();
            string boundary = $"----------{DateTime.Now.Ticks:x}";
            //form.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            form.Headers.Add("ContentType", $"muiltipart/form-data,boundary={boundary}");
            //form.Headers.Add("ContentType", $"muiltipart/form-data,boundary={boundary}");

            foreach (var p in contents)
            {
                form.Add(p.Value, p.Key);
            }

            return form;
        }
    }
}
