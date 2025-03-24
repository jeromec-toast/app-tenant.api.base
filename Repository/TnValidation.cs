using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenant.API.Base.Context;
using Tenant.API.Base.Model;
using Tenant.API.Base.Model.Validation;
namespace Tenant.API.Base.Repository
{
    public class TnValidation
    {
        #region Variables
        public ILogger Logger { get; set; }
        public Context.TnValidation DbContext;

        #endregion


        public TnValidation(Context.TnValidation DbContext, ILoggerFactory loggerFactory)
        {
            this.Logger = loggerFactory.CreateLogger(this.GetType());
            this.DbContext = DbContext;
        }

       

       /// <summary>
       /// Get User for given tenantId and userId
       /// </summary>
       /// <param name="tenantId"></param>
       /// <param name="userId"></param>
       /// <returns></returns>
        public async Task<Model.Validation.User> GetUser(string tenantId, string userId)
        {
            try
            {
                //Logger
                this.Logger.LogInformation($"Calling GetUser ({tenantId},{userId}");

                //Getting Tenant Aggeregate
                //Model.Validation.User user = await this.DbContext.User.Where(x => x.TenantId.Equals(tenantId)&& x.UserId.Equals(userId)).FirstOrDefaultAsync();

                //Checking the tenant and user existance and active flag
                Model.Validation.User user =  await (from T in this.DbContext.Tenant
                            join u in this.DbContext.User on T.TenantId equals u.TenantId
                            where u.TenantId.Equals(tenantId)
                            && u.UserId.Equals(userId)
                            && u.Active.Equals(true)
                            && T.Active.Equals(true)
                            select u).FirstOrDefaultAsync();

                //Logger
                this.Logger.LogInformation($"Called GetUser ({tenantId},{userId})");

                //return
                return user;
            }
            catch (System.Exception ex)
            {
                //Error logging 
                this.Logger.LogError($"GetUser Error({ex.Message}) : {ex.InnerException}");
                throw ex;
            }
        }

       
    }
}
