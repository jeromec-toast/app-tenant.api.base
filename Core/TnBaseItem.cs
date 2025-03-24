using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Tenant.API.Base.Model;
using Tenant.API.Base.Util;
using Tenant.API.Base.Attribute;

namespace Tenant.API.Base.Core
{
    public abstract class TnBaseItem : TnBase
    {
        #region Overridden Properties

        [JsonIgnore, NotMapped, PatchIgnore]
        public override DateTime Created { get; set; }
        [JsonIgnore, NotMapped, PatchIgnore]
        public override string CreatedBy { get; set; }
        [JsonIgnore, NotMapped, PatchIgnore]
        public override DateTime LastModified { get; set; }
        [JsonIgnore, NotMapped, PatchIgnore]
        public override string LastModifiedBy { get; set; }
        [JsonIgnore, NotMapped]
        public override Enum.EntityType.Type Type { get { return Enum.EntityType.Type.ITEM; } }
        #endregion

        #region Properties

        [JsonProperty("documentId")]
        public virtual string DocumentId { get; set; }
        [JsonProperty("note")]
        public virtual Note Note { get; set; }

        #endregion

        #region Note Method

        /// <summary>
        /// Adds the note.
        /// </summary>
        /// <param name="comment">Comment.</param>
        /// <param name="userId">User identifier.</param>
        public void AddNote(string comment, string userId)
        {
            DateTime now = TnUtil.Date.GetCurrentTime();

            this.Note = new Note();
            this.Note.TenantId = this.TenantId;
            this.Note.Created = now;
            this.Note.CreatedBy = userId;
            this.Note.LastModified = now;
            this.Note.LastModifiedBy = userId;
            this.Note.Description = comment;
        }

        /// <summary>
        /// Updates the note.
        /// </summary>
        /// <param name="comment">Comment.</param>
        /// <param name="userId">User identifier.</param>
        public void UpdateNote(string comment, string userId)
        {
            this.Note.LastModified = TnUtil.Date.GetCurrentTime();
            this.Note.LastModifiedBy = userId;
            this.Note.Description = comment;
        }

        /// <summary>
        /// Removes the note.
        /// </summary>
        public void RemoveNote()
        {
            this.Note = null;
        }

        #endregion
    }
}
