using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace XtraChef.API.Base.Context
{
    public abstract class TnReadOnlyContext : DbContext
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="T:XtraChef.API.Base.Context.XcReadOnlyContext"/> class.
        /// </summary>
        /// <param name="options">Options.</param>
        public TnReadOnlyContext(DbContextOptions options) : base(options)
        {
            //Added no tracking
            this.ChangeTracker.AutoDetectChangesEnabled = false;
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        #endregion

        #region Overridden Methods

        /// <summary>
        /// On the model creating.
        /// </summary>
        /// <param name="modelBuilder">Model builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base constructor
            base.OnModelCreating(modelBuilder);

        }

        #region Not Implemented Exception 

        /// <summary>
        /// Save changes
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Save changes async
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Save changes
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess"></param>
        /// <returns></returns>
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Save changes async
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        #endregion

        #endregion
    }
}
