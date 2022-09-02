using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PICPresence.Models
{
    public class History
    {
        [JsonProperty("@collectionId")]
        public string CollectionId { get; set; }

        [JsonProperty("@collectionName")]
        public string CollectionName { get; set; }

        [JsonProperty("created")]
        public string Created { get; set; }

        [JsonProperty("entered")]
        public bool Entered { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("room")]
        public string RoomId { get; set; }

        [JsonProperty("updated")]
        public string Updated { get; set; }
    }
}
