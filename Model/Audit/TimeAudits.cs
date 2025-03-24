using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tenant.API.Base.Util;
using static Tenant.API.Base.Core.Enum.AuditAction.TimeAudit;

namespace Tenant.API.Base.Model.Audit
{
    public class TimeAudits
    {
        [JsonIgnore]
        public List<Model.Audit.TimeAudit> timeAudits { get; set; }

        [JsonProperty("entityId")]
        public string EntityId { get; set; }

        [JsonProperty("entityType")]
        public string EntityType { get; set; }

        [JsonProperty("totalTime")]
        public long TotalTime { get; set; }

        [JsonProperty("actions")]
        public List<AuditAction> actions { get; set; }


        /// <summary>
        /// Add TimeAudit
        /// </summary>
        /// <param name="timeAuditList"></param>
        internal void AddItems(List<TimeAudit> timeAuditList)
        {

            try
            {
                //Local variable
                List<Model.Audit.AuditAction> processTimeAuditList = new List<AuditAction>();

                //TODO Calculate the audit time
                this.EntityId = timeAuditList.First().EntityId;
                this.EntityType = timeAuditList.First().EntityType;

                //Getting time audit data for each process
                this.actions = GetDocAudit(timeAuditList);

                //Sum of each process time
                this.TotalTime = actions.Select(x => x.TotalTime).Sum();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get document "Time Audit" data.
        /// </summary>
        /// <param name="timeAuditList"></param>
        /// <returns></returns>
        private List<AuditAction> GetDocAudit(List<TimeAudit> timeAuditList)
        {
            try
            {
                //Local variable
                List<AuditAction> auditActionList = new List<AuditAction>();
                List<string> startList = new List<string>();
                List<string> endList = new List<string>();

                //Get distinct start list
                startList = (from actions in timeAuditList
                             where actions.Action.ToUpper().Contains("START")
                             select actions.Action).Distinct().ToList();

                //Get distinct end list
                endList = (from actions in timeAuditList
                           where actions.Action.ToUpper().Contains("END")
                           select actions.Action).Distinct().ToList();


                foreach (string action in startList)
                {
                    AuditAction auditAction;
                    string actionStart = action;

                    //Getting process name
                    string processName = actionStart.ToUpper().Replace("_START", string.Empty);

                    //Finding the action end for the given process
                    string actionEnd = endList.Where(x => x.Equals($"{processName}_END")).FirstOrDefault();

                    //if the actionEnd has the value then only allow to process
                    if (!string.IsNullOrEmpty(actionEnd))
                    {
                        //Geting time
                        long startTime = timeAuditList.Where(x => x.Action.ToUpper().Equals(actionStart)).Select(x => x.Time).FirstOrDefault();
                        long endtime = timeAuditList.Where(x => x.Action.ToUpper().Equals(actionEnd)).Select(x => x.Time).LastOrDefault();

                        auditAction = new AuditAction();

                        auditAction.Action = processName;
                        auditAction.StartTime = TnUtil.Date.ParseDate(startTime);
                        auditAction.EndTime = TnUtil.Date.ParseDate(endtime);
                        auditAction.TotalTime = endtime - startTime;

                        //Adding to the collection
                        auditActionList.Add(auditAction);
                    }
                }

                return auditActionList;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }

    public class AuditAction
    {
        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("startTime"), JsonConverter(typeof(TnUtil.Date.TnDateTimeConverter))]
        public DateTime StartTime { get; set; }

        [JsonProperty("endTime"), JsonConverter(typeof(TnUtil.Date.TnDateTimeConverter))]
        public DateTime EndTime { get; set; }

        [JsonProperty("totalTime")]
        public long TotalTime { get; set; }
    }
}
