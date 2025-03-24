using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema  ;
using System.Text;
using Newtonsoft.Json;
using Tenant.API.Base.Model;
using Tenant.API.Base.Util;
using Microsoft.AspNetCore.JsonPatch.Adapters;
using Tenant.API.Base.Attribute;
using static Tenant.API.Base.Core.Enum;
using System.Collections.Generic;

namespace Tenant.API.Base.Core
{
    public abstract class TnBase
    {
        #region Variables

        [JsonIgnore, NotMapped]
        public StringBuilder StringBuilder = new StringBuilder();

        [JsonIgnore, NotMapped]
        public Enum.Operation Operation;

        [JsonIgnore, NotMapped]
        public virtual Enum.EntityType.Type Type { get; set; }

        #endregion

        #region Properties

        [JsonProperty("id"), Column("Pk")]
        public virtual long Id { get; set; }
        [JsonProperty("guid"), Key, PatchIgnore]
        public virtual string Guid { get; set; }
        [JsonProperty("tenantId"), PatchIgnore]
        public virtual string TenantId { get; set; }
        [JsonProperty("created"), JsonConverter(typeof(TnUtil.Date.TnDateTimeConverter)), PatchIgnore]
        public virtual DateTime Created { get; set; }
        [JsonProperty("createdBy"), PatchIgnore]
        public virtual string CreatedBy { get; set; }
        [JsonProperty("lastModified"), JsonConverter(typeof(TnUtil.Date.TnDateTimeConverter)), PatchIgnore]
        public virtual DateTime LastModified { get; set; }
        [JsonProperty("lastModifiedBy"), PatchIgnore]
        public virtual string LastModifiedBy { get; set; }

        #endregion

        #region Virtual Methods

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public virtual void OnEntityInitializeAction<T>(T model) { }

        /// <summary>
        /// Ons the entity update action.
        /// </summary>
        /// <param name="entityUpdates">Entity updates.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public virtual void OnEntityUpdateAction<T>(T entityUpdates) { }

        /// <summary>
        /// Ons the entity delete.
        /// </summary>
        public virtual void OnEntityDeleteAction() { }

        /// <summary>
        /// Ons update validation
        /// </summary>
        public virtual void OnEntityModelUpdateValidation() { }


        #endregion

        #region Public Methods

        /// <summary>
        /// Tos the json.
        /// </summary>
        /// <returns>The json.</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// Tos the json.
        /// </summary>
        /// <returns>The json.</returns>
        public dynamic FromJson(string json)
        {
            return JsonConvert.DeserializeObject(json);
        }

        /// <summary>
        /// Update the specified userId and jsonPatches.
        /// </summary>
        /// <returns>The update.</returns>
        /// <param name="userId">User identifier.</param>
        /// <param name="jsonPatches">Json patches.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public void Update<T>(string userId, T jsonPatches)
        {


            this.LastModified = TnUtil.Date.GetCurrentTime();
            this.LastModifiedBy = userId;
            this.Operation = Enum.Operation.UPDATE;


            //update entity
            this.OnEntityUpdateAction(jsonPatches);
        }

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

            TnBase tnBase = obj as TnBase;

            if (tnBase == null)
                return false;
            else if (this.Guid == null)
                return true;
            else
                return this.Guid.Equals(tnBase.Guid) && this.Type.Equals(tnBase.Type);
        }

        #endregion


    }
}
