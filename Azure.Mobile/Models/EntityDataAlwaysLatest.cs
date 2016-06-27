using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;

namespace Azure.Mobile.Models
{
    public class EntityDataAlwaysLatest
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        [UpdatedAt]
        public DateTimeOffset UpdatedAt { get; set; }
    }
}

