using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Tenant.API.Base.Core;

namespace Tenant.API.Base.Model.Audit
{
    public class Filter 
    {
        [JsonProperty("actionType")]
        public string[] ActionType { get; set; }

        [JsonProperty("entitytype")]
        public string[] Entitytype { get; set; }

        [JsonProperty("entityId")]
        public string[] EntityId { get; set; }

        [JsonProperty("userId")]
        public string[] UserId { get; set; }
    }
}
