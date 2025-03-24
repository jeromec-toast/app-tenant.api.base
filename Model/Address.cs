using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static Tenant.API.Base.Core.Enum;
using Tenant.API.Base.Attribute;
using System.Linq;

namespace Tenant.API.Base.Model
{
    [Table("TN_ADDRESS")]
    public class Address
    {
        #region Variables

        [JsonIgnore, Column("Pk")]
        public long Id { get; set; }
        [JsonIgnore, JsonProperty("tenantId"), PatchIgnore]
        public string TenantId { get; set; }
        [JsonIgnore, JsonProperty("locationId"), PatchIgnore]
        public string LocationId { get; set; }
        [JsonIgnore, JsonProperty("entityId"), PatchIgnore]
        public string EntityId { get; set; }
        [Required, JsonProperty("type")]
        public virtual string Type { get; set; }
        [Required, JsonProperty("address1")]
        public string Address1 { get; set; }
        [JsonProperty("address2")]
        public string Address2 { get; set; }
        [Required, JsonProperty("city")]
        public string City { get; set; }
        [Required, JsonProperty("state")]
        public string State { get; set; }
        [Required, JsonProperty("zip")]
        public string Zip { get; set; }
        [JsonProperty("attention")]
        public string Attention { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("company")]
        public string Company { get; set; }
        [JsonProperty("country"), NotMapped]
        public string Country { get; set; }
        [JsonProperty("fullAddress"), NotMapped]
        public string FullAddress { get
            {
                return this.GetFullAddress();
            }
        }
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Tenant.API.Base.Model.Address"/> class.
        /// </summary>
        public Address()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Tenant.API.Base.Model.Address"/> class.
        /// </summary>
        /// <param name="type">Type.</param>
        public Address(Core.Enum.Address.Type type)
        {
            this.Type = type.ToString();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the type.
        /// </summary>
        /// <param name="type">Type.</param>
        public void SetType(Core.Enum.Address.Type type) {
            this.Type = type.ToString();
        }
        #region GetAddress as a full String
        public string GetFullAddress()
        {
            var array = new[] { this.Address1, this.Address2, this.City, this.State,this.Country};
           
            string Address = string.Join(", ", array.Where(s => !string.IsNullOrEmpty(s)));
            var addrArray = new[] { Address, this.Zip };
            var fullAddress = string.Join(" -", addrArray.Where(s => !string.IsNullOrEmpty(s)));
            return fullAddress;
        }

        #endregion
        #endregion

        #region Overridden Methods

        /// <summary>
        /// Serves as a hash function for a <see cref="T:xcapibase.Core.XcBase"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="T:xcapibase.Core.XcBase"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="T:xcapibase.Core.XcBase"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="T:xcapibase.Core.XcBase"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Address address = obj as Address;

            if (address == null)
                return false;
            else
                return (this.Type.Equals(address.Type));
        }

        #endregion
    }
}
