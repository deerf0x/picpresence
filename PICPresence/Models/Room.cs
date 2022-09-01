using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PICPresence.Models
{
    public class Room
    {
        [JsonProperty("@collectionId")]
        public string CollectionId { get; set; }

        [JsonProperty("@collectionName")]
        public string CollectionName { get; set; }

        [JsonProperty("created")]
        public string Created { get; set; }

        [JsonProperty("currentCapacity")]
        public int CurrentCapacity { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("maxCapacity")]
        public int MaxCapacity { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("updated")]
        public string Updated { get; set; }
    }

}
