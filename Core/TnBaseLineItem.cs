using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;


namespace Tenant.API.Base.Core
{
    public class TnBaseLineItem : TnBaseItem
    {
        #region Properties

        [JsonProperty("tenantCatalogId")]
        public virtual string TenantCatalogId { get; set; }
        [JsonProperty("productCode")]
        public virtual string ProductCode { get; set; }
        [Required, JsonProperty("description")]
        public virtual string Description { get; set; }
        [JsonProperty("uom")]
        public virtual string Uom { get; set; }
        [JsonProperty("pkSize")]
        public virtual string PkSize { get; set; }
        [JsonProperty("quantity")]
        public virtual double Quantity { get; set; }
        [JsonProperty("unitPrice")]
        public virtual decimal UnitPrice { get; set; }
        [JsonProperty("discount")]
        public virtual decimal Discount { get; set; }
        [Required, JsonProperty("total")]
        public virtual decimal Total { get; set; }

        #endregion
    }
}
