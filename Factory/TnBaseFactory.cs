using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Tenant.API.Base.Core;
using Tenant.API.Base.Model.Audit;

namespace Tenant.API.Base.Factory
{
    public abstract class TnBaseFactory<T> where T : TnBase
    {
        #region Variables

        protected ILoggerFactory LoggerFactory { get; set; }
        public ILogger Logger { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Create the specified tenantId, locationId and createdBy.
        /// </summary>
        /// <returns>The create.</returns>
        /// <param name="tenantId">Tenant identifier.</param>
        /// <param name="locationId">Location identifier.</param>
        /// <param name="createdBy">Created by.</param>
        public T CreateInstance(string tenantId, string createdBy)
        {
            //current date time
            DateTime now = Util.TnUtil.Date.GetCurrentTime();

            //create an instance of the entity
            T entity = Activator.CreateInstance<T>();

            //set guid
            entity.Guid = Guid.NewGuid().ToString();

            //set tenant id
            entity.TenantId = tenantId;

            //set created by and last modified by
            entity.CreatedBy = entity.LastModifiedBy = createdBy;
            entity.Created = entity.LastModified = now;

            //set operation
            entity.Operation = Core.Enum.Operation.ADD;

            //return the entity
            return entity;
        }

        /// <summary>
        /// Creates the instance using model
        /// </summary>
        /// <returns>The instance.</returns>
        /// <param name="tenantId">Tenant identifier.</param>
        /// <param name="locationId">Location identifier.</param>
        /// <param name="createdBy">Created by.</param>
        /// <param name="model">Model.</param>
        public T CreateInstance(string tenantId, string createdBy, T model)
        {
            T entity = this.CreateInstance(tenantId, createdBy);

            //raise OnEntityInitializeAction
            entity.OnEntityInitializeAction(model);

            return entity;
        }

        /// <summary>
        /// Creates the audit.
        /// </summary>
        /// <returns>The audit.</returns>
        /// <param name="entityStatus">Entity status.</param>
        /// <param name="entityId">Entity identifier.</param>
        /// <param name="entityType">Entity type.</param>
        /// <param name="description">Description.</param>
        /// <param name="operation">Operation.</param>
        /// <param name="severity">Severity.</param>
        /// <param name="isInternal">If set to <c>true</c> is internal.</param>
        public Audit CreateAudit(string entityId, string entityStatus, string userId, string entityType, string description, Core.Enum.Operation operation, int severity, bool isInternal)
        {
            Audit audit = new Audit();
            audit.Guid = System.Guid.NewGuid().ToString();
            audit.Created = Util.TnUtil.Date.GetCurrentTime();
            audit.CreatedBy = userId;
            audit.EntityStatus = entityStatus;
            audit.EntityId = entityId;
            audit.EntityType = entityType;
            audit.Internal = isInternal;
            audit.OperationType = operation.ToString();
            audit.Severity = severity;
            audit.Description = description;

            return audit;
        }

        #endregion
    }
}
