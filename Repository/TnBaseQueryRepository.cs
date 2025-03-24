using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tenant.API.Base.Context;
using Tenant.API.Base.Core;
using XtraChef.API.Base.Context;

namespace Tenant.API.Base.Repository
{
    public abstract class TnBaseQueryRepository<T1, T2> where T1 : TnBase where T2: TnReadOnlyContext
    {
        #region Variables

        public ILogger Logger { get; set; }
        public T2 DbContext { get; set; }

        #endregion

        #region Constructor

        public TnBaseQueryRepository(T2 dbContext, ILoggerFactory loggerFactory)
        {
            this.DbContext = dbContext;

            this.Logger = loggerFactory.CreateLogger(this.GetType());
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <returns>The by identifier.</returns>
        /// <param name="tenantId">Tenant identifier.</param>
        /// <param name="locationId">Location identifier.</param>
        /// <param name="id">Identifier.</param>
        public abstract Task<T1> GetById(string tenantId, string id);
        
        #endregion
    }
}
