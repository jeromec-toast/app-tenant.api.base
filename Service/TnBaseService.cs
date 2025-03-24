using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Tenant.API.Base.Context;
using Tenant.API.Base.Core;
using Tenant.API.Base.Email;
using Tenant.API.Base.Factory;
using Tenant.API.Base.Model.Audit;
using Tenant.API.Base.Model.Security;
using Tenant.API.Base.Repository;
using Tenant.API.Base.Util;

namespace Tenant.API.Base.Service
{
    public abstract class TnBaseService
    {

        #region Variables

        public string AccessToken { get; set; }
        public string ApiBaseUrl { get; set; }
        public ILogger Logger { get; set; }
        public IConfiguration Configuration { get; set; }
        public Repository.TnAudit XcAuditRepository { get; set; }
        public Repository.TnValidation XcValidationRepository { get; set; }

        #endregion

        #region Properties

        public string TenantId { get; set; }
        public string LocationId { get; set; }
        public string UserId { get; set; }
        public string UserRole { get; set; }

        #endregion

        #region Constructor to initialize Audit time

        /// <summary>
        /// Init XcAuditTime repository
        /// </summary>
        /// <param name="xcAuditTime"></param>
        public TnBaseService(Repository.TnAudit xcAudit, Repository.TnValidation xcValidation)
        {
            this.XcAuditRepository = xcAudit;
            this.XcValidationRepository = xcValidation;
        }

        #endregion

        #region Public Methods



        /// <summary>
        /// Generates the number.
        /// </summary>
        /// <returns>The number.</returns>
        public string GenerateUniqueNumber()
        {
            return TnUtil.GenerateUniqueNumber();
        }

        /// <summary>
        /// Gets the mail context.
        /// </summary>
        /// <returns>The mail context.</returns>
        public MailContext GetMailContext()
        {
            return new MailContext()
            {
                From = Configuration["EmailConfig:connection:From"],
                UserName = Configuration["EmailConfig:connection:UserName"],
                IsBodyHtml = Convert.ToBoolean(Configuration["EmailConfig:connection:IsBodyHtml"]),
                Password = TnUtil.EncryptionHelper.EnDecrypt(Configuration["EmailConfig:connection:Password"], true),
                Host = Configuration["EmailConfig:connection:Host"],
                Port = Convert.ToInt32(Configuration["EmailConfig:connection:Port"]),
                EnableSsl = Convert.ToBoolean(Configuration["EmailConfig:connection:EnableSsl"]),
                UseDefaultCredentials = Convert.ToBoolean(Configuration["EmailConfig:connection:UseDefaultCredentials"]),
            };
        }

        /// <summary>
        /// Gets the s3 context.
        /// </summary>
        /// <returns>The s3 context.</returns>
        //public S3Context GetS3Context()
        //{
        //    return new S3Context(this.TenantId,
        //                        this.LocationId,
        //                        this.UserId,
        //                        this.Configuration["AWS:AccessKey"],
        //                        this.Configuration["AWS:SecretKey"],
        //                         this.Configuration["AWS:S3:BucketName"]);
        //}

        ///// <summary>
        ///// Gets the s3 context.
        ///// </summary>
        ///// <returns>The s3 context.</returns>
        ///// <param name="bucketName">Bucket name.</param>
        //public S3Context GetS3Context(string bucketName)
        //{
        //    return new S3Context(this.TenantId,
        //                        this.LocationId,
        //                        this.UserId,
        //                        this.Configuration["AWS:AccessKey"],
        //                        this.Configuration["AWS:SecretKey"],
        //                         bucketName);
        //}

        ///// <summary>
        ///// Gets the DynamoDb context.
        ///// </summary>
        ///// <returns>The DynamoDb context.</returns>
        //public DynamoDbContext GetDynamoDbContext()
        //{
        //    return new DynamoDbContext(
        //                        this.Configuration["AWS:AccessKey"],
        //                        this.Configuration["AWS:SecretKey"],
        //                        this.Configuration["AWS:Region"]
        //                       );
        //}
        ///// <summary>
        ///// Gets the SQS context.
        ///// </summary>
        ///// <returns>The SQS context.</returns>
        //public SQSContext GetSQSContext()
        //{
        //    return new SQSContext(
        //                        this.Configuration["AWS:AccessKey"],
        //                        this.Configuration["AWS:SecretKey"],
        //                        this.Configuration["AWS:Region"]
        //                       );
        //}

        /// <summary>
        /// Gets the SQS Context.
        /// </summary>
        /// <returns>The SQS Context.</returns>
        /// <param name="queueUrl">Queue URL.</param>
        //public SQSContext GetSQSContext(string queueUrl)
        //{
        //    return new SQSContext(
        //                        this.Configuration["AWS:AccessKey"],
        //                        this.Configuration["AWS:SecretKey"],
        //                        this.Configuration["AWS:Region"],
        //                        queueUrl
        //                       );
        //}

        /// <summary>
        /// Gets the validation context.
        /// </summary>
        /// <returns>The validation context.</returns>
        public ValidationContext GetValidationContext()
        {
            return new ValidationContext()
            {
                TenantId = this.TenantId,
                LocationId = this.LocationId,
                UserId = this.UserId,
                UserRole = this.UserRole
            };
        }


        #endregion

        //#region Region Time Convertor
        ///// <summary>
        ///// Get Time based on region 
        ///// </summary>
        ///// <param name="createdDate"></param>
        ///// <param name="locationId"></param>
        ///// <returns></returns>
        //public DateTime GetRegionTime(DateTime createdDate, long locationId)
        //{
        //    try
        //    {
        //        this.Logger.LogInformation($"calling GetRegionTime");

        //        //local variable
        //        string spName = this.Configuration["StoredProcedure:GetRegionTime"];

        //        var regionTime = this.XcAuditRepository.GetRegionTime(createdDate, locationId, spName);

        //        this.Logger.LogInformation($"called GetRegionTime");

        //        //return 
        //        return regionTime;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        this.Logger.LogError($"error on GetRegionTime");
        //        throw ex;
        //    }
        //}
        //#endregion

        #region Audit public service method

        /// <summary>
        /// Get list of audit by entity id 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="orderByDesc"></param>
        /// <returns></returns>
        public virtual List<Model.Audit.Audit> GetAudits(string entityId, bool orderByDesc = false)
        {
            try
            {
                this.Logger.LogInformation($"calling GetAudits");

                //local variable
                List<Model.Audit.Audit> audits = null;

                audits = this.XcAuditRepository.GetAudit(entityId, orderByDesc).Result;

                this.Logger.LogInformation($"called GetAudits");

                //return 
                return audits;
            }
            catch (System.Exception ex)
            {
                this.Logger.LogError($"error on GetAudits");
                throw ex;
            }
        }

        /// <summary>
        /// Logs the audit time.
        /// </summary>
        /// <returns>The audit time.</returns>
        /// <param name="guid">GUID.</param>
        /// <param name="docType">Document type.</param>
        /// <param name="actionType">Action type.</param>
        /// <param name="status">Status.</param>
        public virtual string LogAuditTime(string guid, string docType, string actionType, string status = "")
        {
            try
            {
                //logger
                this.Logger.LogInformation("Calling LogAuditTime");

                //Local variable
                Model.Audit.TimeAudit timeAudit = new Model.Audit.TimeAudit();

                timeAudit.LogAuditTime(guid, docType, actionType, this.UserId, status);

                string result = this.XcAuditRepository.AddTimeAudit(timeAudit).Result;

                //logger
                this.Logger.LogInformation("Called LogAuditTime");

                return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Logs the audit.
        /// </summary>
        /// <returns>The audit.</returns>
        /// <param name="audit">Audit.</param>
        public string LogAudit(Audit audit)
        {
            try
            {
                //logger
                this.Logger.LogInformation("Calling LogAudit");

                string result = this.XcAuditRepository.AddAudit(audit).Result;

                //logger
                this.Logger.LogInformation("Called LogAudit");

                return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get time audit log
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public virtual Model.Audit.TimeAudits GetLogTimeAudit(string entityId)
        {
            //Logger
            this.Logger.LogInformation($"Calling GetLogTimeAudit");

            //Local variable
            List<Model.Audit.TimeAudit> timeAuditList;
            Model.Audit.TimeAudits timeAudit = new Model.Audit.TimeAudits();

            //preparing filter
            Model.Audit.Filter filter = new Model.Audit.Filter() { EntityId = new string[] { entityId } };

            //Get TimeAudit list
            timeAuditList = this.XcAuditRepository.GetTimeAudit(filter).Result;

            //Get Process time
            timeAudit.AddItems(timeAuditList);

            //Logger
            this.Logger.LogInformation($"Called GetLogTimeAudit");

            return timeAudit;
        }

        /// <summary>
        /// udpate general audit
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityType"></param>
        /// <param name="operationType"></param>
        /// <param name="description"></param>
        /// <param name="auditStatus"></param>
        /// <param name="system"></param>
        /// <param name="userId"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public string UpdateAudit(string entityId, string entityType, Model.Audit.Enum.OperationType.Type operationType, string description, string auditStatus, bool system = true, string userId = "", string json = null)
        {
            try
            {
                //Checking Audit on or off in configuration
                bool IsAuditingRequired = true;
                if (!string.IsNullOrEmpty(Configuration["AuditingRequired"]))
                {
                    IsAuditingRequired = Convert.ToBoolean(Configuration["AuditingRequired"]);
                }
                if (IsAuditingRequired) 
                {
                //Local variable
                API.Base.Model.Audit.Audit audit = new API.Base.Model.Audit.Audit();

                if (!string.IsNullOrEmpty(entityId) && !string.IsNullOrEmpty(entityType) && !string.IsNullOrEmpty(description) && !string.IsNullOrEmpty(auditStatus))
                {

                    //Guid
                    audit.Guid = System.Guid.NewGuid().ToString();
                    userId = string.IsNullOrEmpty(userId) ? this.UserId : userId;

                    //audit.TenantId = this.TenantId;
                    audit.EntityStatus = auditStatus;
                    audit.EntityId = entityId;
                    audit.Severity = 100;
                    audit.OperationType = operationType.ToString();
                    audit.Internal = system;
                    audit.Description = description;
                    audit.EntityType = entityType;
                    audit.Json = json;
                    //Details of when & who
                    audit.Created = TnUtil.Date.GetCurrentTime();
                    audit.CreatedBy = userId;

                    string rsponse = this.XcAuditRepository.UpdateAudit(audit).Result;

                    return "Success";
                }
                else
                {
                    this.Logger.LogWarning("required field ( entityId, entityType, description, auditStatus ) cannot be empty");
                    throw new MissingFieldException("required field ( entityId, entityType, description, auditStatus ) cannot be empty");
                }
            }
                return "Success";
            }
            catch (Exception ex)
            {
                this.Logger.LogError($"error while updating the audit ({ex.Message})");
                throw ex;
            }

        }

        #endregion
        #region Validate context public service method

        /// <summary>
        /// validate the tenant,location and user in the context
        /// </summary>
        /// <param name="validationContext"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool ValidateContext(ValidationContext validationContext, out string message)
        {
            try
            {
                this.Logger.LogInformation($"calling ValidateContext");

                //local variable
                Model.Validation.User user = null;

                //Get User 
                user = this.XcValidationRepository.GetUser(validationContext.TenantId, validationContext.UserId).Result;

                
                if ( user != null)
                {
                    message = "Input data is valid";
                    this.Logger.LogInformation($"called ValidateContext: {message}");
                    return true;
                }
                else
                {
                   
                    message = "Invalid input data";
                    this.Logger.LogInformation($"called ValidateContext: {message}");
                    return false;
                }


            }
            catch (System.Exception ex)
            {
                this.Logger.LogError($"error on ValidateContext");
                throw ex;
            }
        }
        #endregion
    }
}
