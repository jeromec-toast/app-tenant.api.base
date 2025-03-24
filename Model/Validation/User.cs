using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Tenant.API.Base.Model.Validation
{
    [Table("TN_USER")]
    public class User
    {
        public string TenantId { get; set; }

        public string UserId { get; set; }

        public bool Active { get; set; }
        
    }
}
