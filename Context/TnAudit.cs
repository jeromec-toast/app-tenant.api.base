using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Tenant.API.Base.Context
{
    public class TnAudit : TnBaseContext
    {

        #region Context property
        public DbSet<Model.Audit.TimeAudit> TimeAudits { get; set; }
        public DbSet<Model.Audit.Audit> Audits { get; set; }
        #endregion


        #region Constructor
        public TnAudit(DbContextOptions<Context.TnAudit> options) : base(options)
        {

        }
        #endregion

        #region Overridden method
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        #endregion
    }
}
