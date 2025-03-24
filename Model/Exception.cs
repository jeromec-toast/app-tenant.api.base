using System;
namespace Tenant.API.Base.Model
{
    public class Exception:SystemException
    {
        public Exception(string message) : base(message) 
        {
            
        }
    }
}
