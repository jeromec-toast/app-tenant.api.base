using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenant.API.Base.Context;
using Tenant.API.Base.Model;
using Tenant.API.Base.Model.Audit;

namespace Tenant.API.Base.Repository
{
    public class TnAudit
    {
        #region Variables
        public ILogger Logger { get; set; }
        public Context.TnAudit DbContext;

        public TnAudit(Context.TnAudit DbContext, ILoggerFactory loggerFactory)
        {
            this.Logger = loggerFactory.CreateLogger(this.GetType());
            this.DbContext = DbContext;
        }

        #endregion

        #region Internal method

        #region General audit

        /// <summary>
        /// Add audit
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<string> UpdateAudit(Model.Audit.Audit entity)
        {
            return await this.AddAudit(entity);
        }

        /// <summary>
        /// Add audit
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<string> AddAudit(Model.Audit.Audit entity)
        {
            try
            {
                Logger.LogInformation($"Adding an audit entry (entity : {entity.EntityType} | entityId : {entity.EntityId} | description : {entity.Description})");

                //saving audit
                this.DbContext.Audits.Add(entity);
                await this.DbContext.SaveChangesAsync();

                Logger.LogInformation($"Audit entry is added");

                return entity.Guid;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        #region TimeAudit

        /// <summary>
        /// Add TimeAudit
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        internal async Task<string> AddTimeAudit(Model.Audit.TimeAudit entity)
        {
            try
            {
                //Logger
                this.Logger.LogInformation($"Calling AddTimeAudit");

                //saving Time audit  
                this.DbContext.TimeAudits.Add(entity);
                await this.DbContext.SaveChangesAsync();

                //Logger
                this.Logger.LogInformation($"Called AddTimeAudit");
                return entity.Guid;

            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Get list of time audit 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        internal async Task<List<TimeAudit>> GetTimeAudit(Model.Audit.Filter filter)
        {
            try
            {
                //Logger 
                this.Logger.LogInformation($"Calling GetTimeAudits");

                //Local variable
                List<Model.Audit.TimeAudit> timeAudits;

                //Building query
                IQueryable<Model.Audit.TimeAudit> query = this.DbContext.TimeAudits.AsQueryable();

                //Adding EntityId filter
                if (filter.EntityId != null && filter.EntityId.Count() > 0)
                    query = query.Where(x => filter.EntityId.Contains(x.EntityId));

                //Adding EntityType filter
                if (filter.Entitytype != null && filter.Entitytype.Count() > 0)
                    query = query.Where(x => filter.Entitytype.Contains(x.EntityType));

                //Adding ActionType filter
                if (filter.ActionType != null && filter.ActionType.Count() > 0)
                    query = query.Where(x => filter.ActionType.Contains(x.Action));

                //Adding UserId filter
                if (filter.UserId != null && filter.UserId.Count() > 0)
                    query = query.Where(x => filter.UserId.Contains(x.UserId));

                //Executing query
                timeAudits = await query.ToListAsync();

                //Order by 
                timeAudits = timeAudits.OrderBy(x => x.Time).ToList();

                //Logger
                this.Logger.LogInformation($"Called GetTimeAudits");

                //return TimeAudit list
                return timeAudits;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        internal async Task<List<Model.Audit.Audit>> GetAudit(string entityId, bool orderByDesc= false)
        {
            try
            {
                this.Logger.LogInformation($"calling GetAudit repository");

                //local variable
                List<Model.Audit.Audit> audits = null;

                //Building query
                IQueryable<Model.Audit.Audit> query = this.DbContext.Audits.Where(x => x.EntityId.ToUpper().Equals(entityId.ToUpper())).AsQueryable();

                //orderBy filter
                if (orderByDesc)
                    //descending
                    query = query.OrderByDescending(x => x.Created);
                else
                    //ascending 
                    query = query.OrderBy(x => x.Created);

                //Executing qery
                audits = await query.ToListAsync();

                this.Logger.LogInformation($"called GetAudit repository");

                //return autids list
                return audits;
            }
            catch (System.Exception ex)
            {
                this.Logger.LogError($"error on Getaudit repository");
                throw ex;
            }
        }

        //internal  DateTime GetRegionTime(DateTime createdDate, long locationId,string spName)
        //{
        //    try
        //    {
        //        this.Logger.LogInformation($"calling GetRegionTime repository");

        //        //Building query
        //        SqlParameter createdDateParam = new SqlParameter
        //        {
        //            ParameterName = "@UPLOADED_ON",
        //            Value = createdDate
        //        };

        //        SqlParameter locationIdParam = new SqlParameter
        //        {
        //            ParameterName = "@LOCATION_ID",
        //            Value = locationId
        //        };


        //        //get region time 
        //        var regiontime= this.DbContext.regionTimes.FromSqlRaw($"exec {spName} @UPLOADED_ON,@LOCATION_ID", createdDateParam,locationIdParam).AsEnumerable().FirstOrDefault();


        //        this.Logger.LogInformation($"called GetRegionTime repository");

        //        //return autids list
        //        return regiontime.REGION_TIME;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        this.Logger.LogError($"error on GetRegionTime repository");
        //        throw ex;
        //    }
        //}
        
        #endregion
        #endregion


    }
}
