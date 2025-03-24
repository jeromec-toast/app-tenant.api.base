using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Tenant.API.Base.Model.Audit
{
    public class Enum
    {
        /// <summary>
        /// OperationType Enum
        /// </summary>
        public class OperationType
        {
            public enum Type
            {
                [Description("Add")]
                ADD,
                [Description("Update")]
                UPDATE,
                [Description("Reconcilation")]
                RECONCILATION,
                [Description("Read")]
                READ,
                [Description("Extract")]
                DOWNLOAD,
                [Description("Delete")]
                DELETE
            }
        }
    }
}
