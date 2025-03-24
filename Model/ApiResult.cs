using System;
using Newtonsoft.Json;

namespace Tenant.API.Base.Model
{
    public class ApiResult
    {
        [JsonProperty("data")]
        public dynamic Data { get; set; }

        [JsonProperty("exception")]
        public dynamic Exception { get; set; }

    }
}
