using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Tenant.API.Base.Attribute;
using Tenant.API.Base.Model;
using Tenant.API.Base.Service;

namespace Tenant.API.Base.Controller
{
    
    public abstract class TnBaseCommandController<T> : TnBaseController<T> where T : TnBaseService
    {

        #region Constructor
        public TnBaseCommandController(T service, IConfiguration configuration, ILoggerFactory loggerFactory) : base(service, configuration, loggerFactory)
        {
        }
        #endregion

        #region Abstract action method
        /// <summary>
        /// Auditing the process time
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        
        [HttpPost]
        [Route("{guid}/time-audit/{actionType}")]
        public abstract IActionResult LogAuditTime([FromRoute]string guid, [FromRoute]string actionType);

        [HttpPost]
        [Route("{guid}/time-audit")]
        public abstract IActionResult LogAuditTime([FromRoute]string guid, [FromBody]Model.Audit.TimeAudit timeAudit);
        #endregion

    }
}
