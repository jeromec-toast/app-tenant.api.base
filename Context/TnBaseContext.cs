using System;
using Microsoft.EntityFrameworkCore;
using Tenant.API.Base.Core;
using Tenant.API.Base.Model;

namespace Tenant.API.Base.Context
{
    public abstract class TnBaseContext: DbContext
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Tnapibase.Context.XcBaseContext"/> class.
        /// </summary>
        /// <param name="options">Options.</param>
        public TnBaseContext(DbContextOptions options) : base(options) { }

        #endregion

        #region Overridden Methods

        /// <summary>
        /// Ons the model creating.
        /// </summary>
        /// <param name="modelBuilder">Model builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base constructor
            base.OnModelCreating(modelBuilder);

        }
        #endregion

    }
}
