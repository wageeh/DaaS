using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appointments.Entities
{
    public class BaseEntity
    {
        [JsonProperty(PropertyName = "id")]
        public Guid EntityId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
