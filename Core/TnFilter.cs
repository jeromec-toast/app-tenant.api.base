using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tenant.API.Base.Core
{
    public abstract class TnFilter
    {
        [JsonProperty("fromDate")]
        public DateTime FromDate { get; set; }
        [JsonProperty("toDate")]
        public DateTime ToDate { get; set; }
        [JsonProperty("locations")]
        public string[] Locations { get; set; }
        [JsonProperty("vendors")]
        public string[] Vendors { get; set; }
        [JsonProperty("statuses")]
        public string[] Statuses { get; set; }
        [JsonProperty("limit")]
        public string Limit { get; set; }
        [JsonProperty("attributes")]
        public string[] Attributes { get; set; }
    }
}
