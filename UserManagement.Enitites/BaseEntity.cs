using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Enitites
{
    public class BaseEntity
    {

        [JsonProperty(PropertyName = "id")]
        public Guid EntityId { get; set; }
        public string ItemId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
