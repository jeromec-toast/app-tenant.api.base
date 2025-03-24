using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static Tenant.API.Base.Core.Enum;

namespace Tenant.API.Base.Model
{
    public class Phone
    {
        #region Variables

        [Required, JsonProperty("type"), JsonConverter(typeof(StringEnumConverter))]
        public Core.Enum.Phone.Type Type { get; set; }
        [Required, JsonProperty("number")]
        public string Number { get; set; }
        [Required, JsonProperty("primary")]
        public bool Primary { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Tenant.API.Base.Model.Phone"/> class.
        /// </summary>
        public Phone() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Tenant.API.Base.Model.Phone"/> class.
        /// </summary>
        /// <param name="type">Type.</param>
        public Phone(Core.Enum.Phone.Type type) {
            this.Type = type;
        }

        #endregion
    }
}
