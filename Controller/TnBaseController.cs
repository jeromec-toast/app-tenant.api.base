using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Tenant.API.Base.Context;
using Tenant.API.Base.Filter;
using Tenant.API.Base.Service;

namespace Tenant.API.Base.Controller
{
    [Authorize]
    [ServiceFilter(typeof(ApiActionFilter))]
    [EnableCors("AllowAllOrigins")]
    public abstract class TnBaseController<T> : ControllerBase where T : TnBaseService
    {
        #region Variables

        public IConfiguration Configuration { get; set; }
        public ILogger Logger { get; set; }

        public string AccessToken { get; set; }
        public string TenantId { get; set; }
        public string LocationId { get; set; }
        public string UserId { get; set; }
        public string UserRole { get; set; }
        public T Service { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="T:XtraChef.API.Base.Controller.XcBaseController`1"/> class.
        /// </summary>
        public TnBaseController()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:XtraChef.API.Base.Controller.XcBaseController`1"/> class.
        /// </summary>
        /// <param name="service">Service.</param>
        /// <param name="configuration">Configuration.</param>
        /// <param name="loggerFactory">Logger factory.</param>
        public TnBaseController(T service, IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            Logger = loggerFactory.CreateLogger(this.GetType());

            //create a service instance
            Service = service;
            Service.Logger = loggerFactory.CreateLogger<T>();
            Service.Configuration = configuration;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <returns>The version.</returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public string GetVersion()
        {
            return Startup.TnBaseStartup.Version;
        }

        /// <summary>
        /// Creates the token.
        /// </summary>
        /// <returns>The token.</returns>
        /// <param name="validationContext">Validation context.</param>
        [AllowAnonymous]
        [HttpPost]
        [Route("token")]
        [Produces("text/plain")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual IActionResult Token([FromBody]Model.Security.ValidationContext validationContext)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    //bool isValid = false;
                    //string message;
                    Logger.LogInformation($"Valid token parameters: {validationContext}");

                    //generate a token
                    string token = this.GenerateToken(validationContext);

                   

                    //return token
                    return StatusCode(StatusCodes.Status200OK, token);

                    ////Validate context
                    //isValid = this.Service.ValidateContext(validationContext, out message);

                    ////Commented and in future will reimplement the validation
                    //if (isValid || validationContext.UserId.Equals("-1"))
                    //{

                    //    //generate a token
                    //    string token = this.GenerateToken(validationContext);

                    //    //return token
                    //    return StatusCode(StatusCodes.Status200OK, token);
                    //}
                    //else
                    //{
                    //    return StatusCode(StatusCodes.Status400BadRequest, message);
                    //}
                }
                else
                {
                    //return error
                    BadRequestObjectResult badObjectResult = new BadRequestObjectResult(ModelState);
                    this.Logger.LogError(badObjectResult.ToString());

                    return StatusCode(StatusCodes.Status400BadRequest, badObjectResult);
                }
            }
            catch (Exception ex)
            {
                /*BadRequestObjectResult badObjectResult = new BadRequestObjectResult(ModelState);*/
                this.Logger.LogError($"error in generating token ({ex.Message}): {ex.InnerException}");

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Version this instance.
        /// </summary>
        /// <returns>The version.</returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("version")]
        public IActionResult Version()
        {
            var version = this.GetVersion();
            return StatusCode(StatusCodes.Status200OK, version);
        }

        /// <summary>
        /// Version this instance.
        /// </summary>
        /// <returns>The version.</returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("id")]
        public IActionResult GenerateUniqueId()
        {
            var id = this.Service.GenerateUniqueNumber();
            return StatusCode(StatusCodes.Status200OK, id);
        }

        /// <summary>
        /// Get documnet number
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("document-number")]
        public virtual IActionResult GetDocumentNumber()
        {
            return StatusCode(StatusCodes.Status200OK, Util.TnUtil.GenerateUniqueNumber());
        }


        #endregion

        #region Private Methods

        /// <summary>
        /// Generates the token.
        /// </summary>
        /// <returns>The token.</returns>
        /// <param name="validationContext">Validation context.</param>
        private string GenerateToken(Model.Security.ValidationContext validationContext)
        {
            var claims = new[] {
                new Claim(Core.Constant.Jwt.CLAIM_USER_ID, validationContext.UserId),
                new Claim(Core.Constant.Jwt.CLAIM_USER_ROLE, validationContext.UserRole),
                new Claim(Core.Constant.Jwt.CLAIM_TENANT_ID, validationContext.TenantId),
                new Claim(Core.Constant.Jwt.CLAIM_LOCATION_ID, validationContext.LocationId)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(this.Configuration["Jwt:Issuer"],
                                             this.Configuration["Jwt:Issuer"],
                                             claims,
                                             expires: DateTime.Now.AddMinutes(Double.Parse(this.Configuration["Jwt:Expiration"])),
                                             signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #endregion
    }
}
