using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Tenant.API.Base.Core;

namespace Tenant.API.Base.Model.Audit
{
    [Table("XC_AUDIT")]
    public class Audit : TnBase
    {
        #region Public property


        [JsonProperty("internal")]
        public bool Internal { get; set; }

        [JsonProperty("severity")]
        public int Severity { get; set; }

        [JsonProperty("entityId")]
        public string EntityId { get; set; }

        [JsonProperty("entityStatus")]
        public string EntityStatus { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("type"), Column("Type")]
        public string OperationType { get; set; }

        [JsonProperty("entityType"), Column("EntityType")]
        public string EntityType { get; set; }

        [JsonProperty("json"), Column("Json")]
        public virtual string Json { get; set; }
        #endregion

        #region Overridden property
        [JsonIgnore, NotMapped]
        public override long Id { get; set; }

        //[JsonConverter(typeof(Util.XcUtil.Date.XcDateTimeConverter))]
        //public override DateTime Created { get; set; }

        [JsonIgnore, NotMapped]
        public override string TenantId { get; set; }

        [JsonIgnore, NotMapped]
        public override DateTime LastModified { get; set; }

        [JsonIgnore, NotMapped]
        public override string LastModifiedBy { get; set; }
        #endregion

        #region Public Method

        //public virtual Model.Audit.Audit SetAudit<T>(Model.Audit.Enum.OperationType type, string currentStatus, bool system, string description)
        //{

        //    //Local variable
        //    string status = string.Empty;
        //    Model.Audit.Audit audit = new Model.Audit.Audit();

        //    if (!system)
        //    {
        //        //Get enum description for user readablity
        //        status = XcUtil.GetEnumDescription<T>(currentStatus);
        //    }
        //    else
        //        status = currentStatus;

        //    //Setting audit properties
        //    audit.Guid = System.Guid.NewGuid().ToString();
        //    audit.TenantId = this.TenantId;
        //    audit.EntityStatus = status;
        //    audit.EntityId =

        //    return null;
        //}
        #endregion

    }
}
