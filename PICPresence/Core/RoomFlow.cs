using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PICPresence.Models;
using Newtonsoft.Json;

namespace PICPresence.Core
{
    internal class RoomFlow
    {
        private HttpClient client;
        private string url;

        public RoomFlow(string url)
        {
            client = new HttpClient();
            this.url = url;
        }

        public async Task<List<Room>> GetRoomsAsync()
        {
            List<Room> roomCollection = new List<Room>();
            try
            {
                var response = await client.GetStringAsync(url);

                roomCollection = JObject.Parse(response)["items"].ToObject<List<Room>>();

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Something went wrong: " + ex.Message);
            }
          
            return roomCollection;
        }

        public async Task<bool> Add(Room room)
        {

            var Json = JsonConvert.SerializeObject(room);

            var Data = new StringContent(Json, Encoding.UTF8, "application/json");

            using var client = new HttpClient();

            var response = await client.PostAsync(url, Data);
            var result = await response.Content.ReadAsStringAsync();

            return response.StatusCode.ToString() == "OK";
        }
    }
}
