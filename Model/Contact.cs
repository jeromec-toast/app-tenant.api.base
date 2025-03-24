using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using static Tenant.API.Base.Core.Enum;

namespace Tenant.API.Base.Model
{
    public class Contact
    {
        [Required, JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("phones")]
        public Dictionary<Core.Enum.Phone.Type, Phone> Phones { get; set; }

    }
}
