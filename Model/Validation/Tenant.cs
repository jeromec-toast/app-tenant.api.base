using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Tenant.API.Base.Model.Validation
{
    [Table("TN_TENANT")]
    public class Tenant
    {
        public string TenantId { get; set; }
        public bool Active { get; set; }
        
    }
}
