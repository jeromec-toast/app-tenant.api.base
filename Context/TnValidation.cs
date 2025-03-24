using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tenant.API.Base.Context
{
    public class TnValidation: TnBaseContext
    {
        #region Context property
        public DbSet<Model.Validation.Tenant> Tenant { get; set; }

        public DbSet<Model.Validation.User> User { get; set; }

        #endregion

        #region ValueConverter
        //long to string converter
        readonly ValueConverter<string, long> longToStringConverter = new ValueConverter<string, long>(
            v => Convert.ToInt64(v),
            v => v.ToString());
        #endregion

        #region Constructor
        public TnValidation(DbContextOptions<Context.TnValidation> options) : base(options)
        {

        }
        #endregion

        #region ModelBuilder
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Set Tenant
            this.SetTenantProperties(modelBuilder);

            //Set User
            this.SetUserProperties(modelBuilder);
        }
        #endregion

        #region Setting Property

        /// <summary>
        /// set tenant properties
        /// </summary>
        /// <param name="modelBuilder"></param>
        private void SetTenantProperties(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Model.Validation.Tenant>()
                        .HasKey(x => new { x.TenantId });

            modelBuilder.Entity<Model.Validation.Tenant>()
                        .Property(x => x.TenantId)
                        .HasColumnName("TENANT_ID")
                        .HasConversion(longToStringConverter);

        }
        /// <summary>
        /// set user properties
        /// </summary>
        /// <param name="modelBuilder"></param>
        private void SetUserProperties(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Model.Validation.User>()
                        .HasKey(x => new { x.UserId });

            modelBuilder.Entity<Model.Validation.User>()
                        .Property(x => x.UserId)
                        .HasColumnName("USER_ID")
                        .HasConversion(longToStringConverter);
            modelBuilder.Entity<Model.Validation.User>()
                      .Property(x => x.TenantId)
                      .HasColumnName("TENANT_ID")
                      .HasConversion(longToStringConverter);
        }
        #endregion
    }
}
