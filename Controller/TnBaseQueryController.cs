using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Tenant.API.Base.Attribute;
using Tenant.API.Base.Model;
using Tenant.API.Base.Service;

namespace Tenant.API.Base.Controller
{
    public abstract class TnBaseQueryController<T> : TnBaseController<T> where T : TnBaseService
    {
        #region Constructor
        public TnBaseQueryController(T service, IConfiguration configuration, ILoggerFactory loggerFactory) : base(service, configuration, loggerFactory)
        {
        }
        #endregion

        #region Abstract action method
        /// <summary>
        /// Get TimeAudit
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{guid}/time-audit")]
        public abstract IActionResult GetTimeAudit([FromRoute]string guid);

        /// <summary>
        /// Get general audit list by entity id
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="orderByDesc">Order by Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("audit/{entityId}")]
        public virtual IActionResult GetAudit([FromRoute]string entityId, [FromQuery(Name = "order-by-desc")]bool orderByDesc = false)
        {
            try
            {
                this.Logger.LogInformation($"getting audit for entityId {entityId}");

                //local varaible
                List<Model.Audit.Audit> audits = new List<Model.Audit.Audit>();

                //TODO: Call service to get list of audit by entity Id and order by desc 
                audits = this.Service.GetAudits(entityId, orderByDesc);
                    
                return StatusCode(StatusCodes.Status200OK, new ApiResult() { Data = audits });    
                
            }
            catch (System.Exception ex)
            {
                API.Base.Model.Exception modelException = new API.Base.Model.Exception(ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResult() { Exception = modelException });
            }
        }
        #endregion
    }
}
