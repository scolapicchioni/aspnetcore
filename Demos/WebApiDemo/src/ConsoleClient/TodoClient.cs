using ConsoleClient.Models;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleClient
{
    public class TodoClient
    {
        public string BaseUri { get; set; } = "http://localhost:5000/api/todo";

        public async Task<List<TodoItem>> GetAll()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(BaseUri);
                var serializer = new DataContractJsonSerializer(typeof(List<TodoItem>));
                var s = await response.Content.ReadAsStreamAsync();
                var items = serializer.ReadObject(s) as List<TodoItem>;
                return items;
            }
        }

        public async Task<TodoItem> Find(string key)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{BaseUri}/{key}");
                var serializer = new DataContractJsonSerializer(typeof(TodoItem));
                var s = await response.Content.ReadAsStreamAsync();
                var item = serializer.ReadObject(s) as TodoItem;
                return item;
            }
        }

        public async Task Add(TodoItem item) {
            using (var client = new HttpClient())
            {
                var ms = new MemoryStream();
                var serializer = new DataContractJsonSerializer(typeof(TodoItem));
                serializer.WriteObject(ms, item);
                StringContent content = new StringContent(Encoding.UTF8.GetString(ms.ToArray()),Encoding.UTF8,"application/json");
                var response = await client.PostAsync(BaseUri,content);
            }
        }

        public async Task Update(string key, TodoItem item)
        {
            using (var client = new HttpClient())
            {
                var ms = new MemoryStream();
                var serializer = new DataContractJsonSerializer(typeof(TodoItem));
                serializer.WriteObject(ms, item);
                StringContent content = new StringContent(Encoding.UTF8.GetString(ms.ToArray()), Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"{BaseUri}/{key}", content);
            }
        }

        public async Task Delete(string key)
        {
            using (var client = new HttpClient())
            {
                var response = await client.DeleteAsync($"{BaseUri}/{key}");
            }
        }

    }
}
