using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tenant.API.Base.Context;
using Tenant.API.Base.Core;

namespace Tenant.API.Base.Repository
{
    public abstract class TnBaseCommandRepository<T1, T2> where T1 : TnBase where T2 : TnBaseContext
    {
        #region Variables

        public ILogger Logger { get; set; }
        public T2 DbContext { get; set; }

        #endregion

        #region Constructor

        public TnBaseCommandRepository(T2 dbContext, ILoggerFactory loggerFactory)
        {
            this.DbContext = dbContext;
            this.Logger = loggerFactory.CreateLogger(this.GetType());
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert the specified entity.
        /// </summary>
        /// <returns>The insert.</returns>
        /// <param name="entity">Entity.</param>
        public abstract Task<string> Insert(T1 entity);

        /// <summary>
        /// Update the specified entity.
        /// </summary>
        /// <returns>The update.</returns>
        /// <param name="entity">Entity.</param>
        public abstract Task<string> Update(T1 entity);

        /// <summary>
        /// Delete the specified entity.
        /// </summary>
        /// <returns>The delete.</returns>
        /// <param name="entity">Entity.</param>
        public abstract Task<string> Delete(T1 entity);

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
