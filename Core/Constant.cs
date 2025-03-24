using System;
using System.Collections.Generic;
using System.Text;

namespace Tenant.API.Base.Core
{
    public class Constant
    {
        #region General

        public class General
        {
            public const string TENANT_ID = "tenantId";
            public const string LOCATION_ID = "locationId";
            public const string GUID = "guid";
        }

        #endregion

        #region UserId

        /// <summary>
        /// User.
        /// </summary>
        public class User
        {
            public const string SYSTEM_USER = "0";
        }

        #endregion

        #region Date

        /// <summary>
        /// Date.
        /// </summary>
        public class Date
        {
            public const string SPERATOR = ":";
            public const string FORMAT = "MM-dd-yyyy";
            public const string FORMAT_DATE_TIME = "MM-dd-yyyy HH:mm:ss";
            public const string FORMAT_DATE_TIME_2 = "yyyy-MM-ddThh:mm:ss";
            public const string FORMAT_DATE_TIME_MS = "yyyy-MM-ddThh:mm:ss";
            public const string FORMAT_DATE_YEAR = "yyyy-MM-ddThh:mm:ss";
        }

        #endregion

        #region Jwt

        /// <summary>
        /// Jwt.
        /// </summary>
        public class Jwt
        {
            public const string CLAIM_USER_ID = "userId";
            public const string CLAIM_USER_ROLE = "userRole";
            public const string CLAIM_TENANT_ID = "tenantId";
            public const string CLAIM_LOCATION_ID = "locationId";
        }

        #endregion

        #region S3

        /// <summary>
        /// Jwt.
        /// </summary>
        public class S3
        {
            public class Metadata
            {
                public const string TENANT = "x-amz-meta-Tenant";
                public const string LOCATION = "x-amz-meta-Location";
                public const string DOCUMENT_ID = "x-amz-meta-document-id";
                public const string FILE_NAME = "x-amz-meta-filename";
                public const string USER_ID = "x-amz-meta-user";
                public const string DOCTYPE = "x-amz-meta-doctype";
            }
        }

        #endregion

        #region RegionName

        /// <summary>
        /// RegionName.
        /// </summary>
        public class RegionName
        {
            public class Metadata
            {
                public const string DEVELOPMENT = "ap-south-1";
                public const string INT = "us-west-1";
                public const string PREPROD = "us-east-2";
                public const string PRODUCTION = "us-east-1";
                public const string DEMONEW = "us-west-2";
            }
        }

        #endregion

        #region SecretManager

        /// <summary>
        /// SecretManager.
        /// </summary>
        public class SecretManager
        {
            public class Development
            {
                public const string secretJson = "dev/xc/secret-json";
            }
            public class Int
            {
                public const string secretJson = "int/xc/secret-json";
            }
            public class Preprod
            {
                public const string secretJson = "preprod/xc/secret-json";
            }
            public class Demo
            {
                public const string secretJson = "demo/xc/secret-json";
            }
            public class Prod
            {
                public const string secretJson = "prod/xc/secret-json";
            }
        }

        #endregion

        #region Token

        public const bool Internal = true;

        #endregion


    }
}
