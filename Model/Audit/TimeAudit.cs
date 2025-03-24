using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Tenant.API.Base.Model.Audit
{
    [Table("TN_TIME_AUDIT")]
    public class TimeAudit
    {
        [JsonProperty("id"), Key]
        public long? Pk { get; set; }
        [JsonProperty("guid")]
        public string Guid { get; set; }
        [JsonProperty("entityId")]
        public string EntityId { get; set; }
        [JsonProperty("entityType"), MaxLength(50)]
        public string EntityType { get; set; }
        [JsonProperty("action"), MaxLength(300)]
        public string Action { get; set; }
        [JsonProperty("time")]
        public long Time { get; set; }
        [JsonProperty("userId")]
        public string UserId { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }



        #region Constructor
        public TimeAudit()
        {
            this.Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// Updating the AuditTime aggregate
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="docType"></param>
        /// <param name="actionType"></param>
        internal void LogAuditTime(string guid, string docType, string actionType, string userId, string status)
        {
            try
            {
                this.Guid = System.Guid.NewGuid().ToString();
                this.EntityId = guid;
                this.EntityType = docType;
                this.Action = actionType;
                this.UserId = userId;
                this.Status = status;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// papulate audit time 
        /// </summary>
        /// <param name="timeAudits"></param>
        internal void GetAuditTime(List<TimeAudit> timeAudits)
        {
            //Setting Root info


        }



        #endregion
    }
}
