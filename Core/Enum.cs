using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Tenant.API.Base.Core
{
    #region Operation

    public class Enum
    {
        public enum Operation
        {
            ADD,
            DELETE,
            UPDATE
        }

        #endregion

        #region Address

        /// <summary>
        /// Address.
        /// </summary>
        public class Address
        {
            /// <summary>
            /// Address type.
            /// </summary>
            public enum Type
            {
                [Description("Home")]
                HOME,
                [Description("Work")]
                WORK,
                [Description("Shipping")]
                SHIPPING,
                [Description("Billing")]
                BILLING,
                [Description("Other")]
                OTHER
            }
        }

        #endregion

        #region Phone

        /// <summary>
        /// Phone.
        /// </summary>
        public class Phone
        {
            /// <summary>
            /// Phone type.
            /// </summary>
            public enum Type
            {
                [Description("Home")]
                HOME,
                [Description("Work")]
                WORK,
                [Description("Cell")]
                CELL,
                [Description("Other")]
                OTHER
            }
        }

        #endregion

        #region User

        /// <summary>
        /// User.
        /// </summary>
        public class User
        {
            /// <summary>
            /// UserRoles
            /// </summary>
            public enum Role
            {
                [Description("System Admin")]
                SystemAdmin = 1,
                [Description("Tenant Admin")]
                TenantAdmin = 2,
                [Description("Executive")]
                Executive = 3,
                [Description("Data Entry Operator")]
                DataEntryOperator = 4,
                [Description("Restaurant Manager")]
                RestaurantManager = 5,
                [Description("Invoice Capture")]
                InvoiceCaputre = 6,
                [Description("Restaurant User")]
                RestaurantUser = 8
            }
        }

        #endregion

        #region Entity Type
        /// <summary>
        /// Entity Model type or node type
        /// </summary>
        public class EntityType
        {
            public enum Type
            {
                [Description("Unknown")]
                UNKNOWN,
                [Description("Document")]
                DOCUMENT,
                [Description("Item")]
                ITEM,
                [Description("Address")]
                ADDRESS,
                [Description("Note")]
                NOTE,
                [Description("Parent")]
                PARENT,
                [Description("Child")]
                CHILD
            }
        }
        #endregion

        /// <summary>
        /// Audit Enum
        /// </summary>
        public class AuditAction
        {
            public enum TimeAudit
            {

                UPLOAD_START,
                UPLOAD_END,

                DATA_ENTRY_START,
                DATA_ENTRY_END,

                PAGE_SEPARATION_START,
                PAGE_SEPARATION_END,

                REVIEW_START,
                REVIEW_END

            }
        }

        #region HttpHeader Type
        /// <summary>
        /// Http Header type AuthenticationHeaderValue
        /// </summary>
        public enum HttpHeaderType
        {
            Basic,
            Bearer
        }

        #endregion

    }
}