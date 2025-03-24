using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Tenant.API.Base.Core;
using Tenant.API.Base.Attribute;
namespace Tenant.API.Base.Model
{
    [Table("XC_NOTE")]
    public class Note
    {
        #region Properties
        [JsonIgnore, Column("Pk"), Key]
        public virtual long Id { get; set; }
        [JsonIgnore, JsonProperty("tenantId"), PatchIgnore]
        public virtual string TenantId { get; set; }
        [JsonIgnore, JsonProperty("locationId"), PatchIgnore]
        public virtual string LocationId { get; set; }
        [JsonProperty("created"), PatchIgnore]
        public DateTime Created { get; set; }
        [JsonProperty("createdBy"), PatchIgnore]
        public string CreatedBy { get; set; }
        [JsonProperty("lastModified"), PatchIgnore]
        public DateTime LastModified { get; set; }
        [JsonProperty("lastModifiedBy"), PatchIgnore]
        public string LastModifiedBy { get; set; }
        [JsonIgnore, PatchIgnore]
        public string EntityId { get; set; }
        [Required, JsonProperty("description")]
        public string Description { get; set; }

        #endregion
    }
}
