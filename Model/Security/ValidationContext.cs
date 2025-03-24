using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Tenant.API.Base.Model.Security
{
    public class ValidationContext
    {
        [Required, JsonProperty("userId")]
        public string UserId { get; set; }
        [Required, JsonProperty("userRole")]
        public string UserRole { get; set; }
        [Required, JsonProperty("tenantId")]
        public string TenantId { get; set; }
        [Required, JsonProperty("locationId")]
        public string LocationId { get; set; }
        [JsonProperty("internal")]
        public bool Internal { get; set; }
    }
}
