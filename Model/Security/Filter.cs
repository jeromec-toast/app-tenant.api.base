using System;
using System.Collections.Generic;

namespace Tenant.API.Base.Model.Security
{
    public class Filter
    {
        public static List<string> AnonymousActions = new List<string> { "Token", "Version", "GenerateUniqueId" };
    }
}
