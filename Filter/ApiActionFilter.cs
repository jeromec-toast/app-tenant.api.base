using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Tenant.API.Base.Attribute;
using Tenant.API.Base.Controller;
using Tenant.API.Base.Model;
using Tenant.API.Base.Service;

namespace Tenant.API.Base.Filter
{
    public class ApiActionFilter : System.Attribute, IActionFilter
    {
        #region Variables

        private ActionExecutingContext _executingContext;
        private ILogger Logger;
        private string _ActionName;
        private string _ControllerName;
        private Dictionary<string, string> _RouteValues = new Dictionary<string, string>();

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Tenant.API.Base.Filter.ApiActionFilter"/> class.
        /// </summary>
        /// <param name="loggerFactory">Logger factor.</param>
        public ApiActionFilter(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(this.GetType());
        }

        #endregion

        #region Overridden Methods

        /// <summary>
        /// Ons the action executed.
        /// </summary>
        /// <param name="context">Context.</param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            dynamic xcController = context.Controller;
        }

        /// <summary>
        /// Ons the action executing.
        /// </summary>
        /// <param name="context">Context.</param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // getting controller instance
            dynamic controller = context.Controller;

            this._executingContext = context;
            Logger.LogInformation("Request IP: " + context.HttpContext.Connection.RemoteIpAddress.ToString());

            var accessToken = context.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            controller.AccessToken = controller.Service.AccessToken = accessToken;

            //getting route attributes
            this.GetRouteAttributes(context.RouteData);

            ////log route values
            //string parameters = this.RouteToString(); 
            //Debug.WriteLine($"Calling - controller:\"{this._ControllerName}\", action:\"{this._ActionName}\", params:{parameters}");

            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            //getting claims from token
            //claims are being set in Service.cs upon successful validation
            var claims = context.HttpContext.User.Claims;
            foreach (Claim claim in claims)
            {
                if (claim.Type.Equals(Core.Constant.Jwt.CLAIM_USER_ID))
                    dictionary[Core.Constant.Jwt.CLAIM_USER_ID] = claim.Value;
                else if (claim.Type.Equals(Core.Constant.Jwt.CLAIM_USER_ROLE))
                    dictionary[Core.Constant.Jwt.CLAIM_USER_ROLE] = claim.Value;
                else if (claim.Type.Equals(Core.Constant.Jwt.CLAIM_TENANT_ID))
                    dictionary[Core.Constant.Jwt.CLAIM_TENANT_ID] = claim.Value;
                else if (claim.Type.Equals(Core.Constant.Jwt.CLAIM_LOCATION_ID))
                    dictionary[Core.Constant.Jwt.CLAIM_LOCATION_ID] = claim.Value;
            }

            //bypass for CreateToken method
            if (!AllowAnonymous())
            {
                //setting values in a base controller and its service
                controller.UserId = controller.Service.UserId = dictionary[Core.Constant.Jwt.CLAIM_USER_ID];
                controller.UserRole = controller.Service.UserRole = dictionary[Core.Constant.Jwt.CLAIM_USER_ROLE];
                controller.TenantId = controller.Service.TenantId = dictionary[Core.Constant.Jwt.CLAIM_TENANT_ID];
                controller.LocationId = controller.Service.LocationId = dictionary[Core.Constant.Jwt.CLAIM_LOCATION_ID];

                //setting api base url
                string apiBaseUrl = GetApiBaseUrl(context.HttpContext.Request);
                controller.Service.ApiBaseUrl = apiBaseUrl;

                //check if the method is for Tenant use only
                if (!ForTenantUseOnly())
                {
                    //validate tenant id matches with the token
                    if (_RouteValues.Count > 0 && _RouteValues.ContainsKey(Core.Constant.General.TENANT_ID))
                    {
                        if (!dictionary[Core.Constant.Jwt.CLAIM_TENANT_ID].Equals(_RouteValues[Core.Constant.General.TENANT_ID]))
                        {
                            ApiResult result = new ApiResult() { Exception = $"Unauthorized access for tenantId '{_RouteValues[Core.Constant.General.TENANT_ID]}'" };
                            this._executingContext.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                            this._executingContext.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(result));

                            //this will throw - System.InvalidOperationException: Headers are read-only, response has already started
                            //ignore the log
                            throw new UnauthorizedAccessException();
                        }
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Allows the anonymous.
        /// </summary>
        /// <returns><c>true</c>, if anonymous was allowed, <c>false</c> otherwise.</returns>
        private bool AllowAnonymous()
        {
            var endpoint = this._executingContext.HttpContext.GetEndpoint();

            return endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null;
        }

        /// <summary>
        /// Fors the xtra chef use only.
        /// </summary>
        /// <returns><c>true</c>, if xtra chef use only was fored, <c>false</c> otherwise.</returns>
        private bool ForTenantUseOnly()
        {
            //var attributes = (this._executingContext.ActionDescriptor.(true);

            var xtraChefUseOnly = (this._executingContext.ActionDescriptor as ControllerActionDescriptor).MethodInfo.CustomAttributes
                                                                                                   .Any(x => x.AttributeType == typeof(TenantUseOnly));

            if (xtraChefUseOnly)
            {
                dynamic controller = this._executingContext.Controller;
                IConfiguration configuration = controller.Configuration;

                //list of internal tenants
                var internalTenants = configuration["xtraCHEF:InternalTenants"];

                //if tenant is one of the internal tenant
                if (internalTenants != null)
                {
                    if (internalTenants.Contains(controller.TenantId))
                        return true;
                    else
                    {
                        ApiResult result = new ApiResult() { Exception = $"Unauthorized access for tenantId '{controller.TenantId}'" };
                        this._executingContext.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                        this._executingContext.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(result));

                        //this will throw - System.InvalidOperationException: Headers are read-only, response has already started
                        //ignore the log
                        throw new UnauthorizedAccessException();
                    }
                }
            }

            return false;
        }


        /// <summary>
        /// Gets the API base URL.
        /// </summary>
        /// <returns>The API base URL.</returns>
        /// <param name="request">Request.</param>
        private string GetApiBaseUrl(HttpRequest request)
        {
            /*
             * e.g. https://bpo.xtrachef.com/api.purchase-orders-command/api/1.0/purchase-orders/736176e4-c8c1-4503-b3bc-25ba1749957c/acknowledge?chksum=wnD74X0KohRx...
             * e.g. http://localhost:7000/api/1.0/purchase-orders/736176e4-c8c1-4503-b3bc-25ba1749957c/acknowledge?chksum=wnD74X0KohRx...
             */

            //http protocol and host name
            string apiBasePath = $"{request.Scheme}://{request.Host.Value}";

            //application context root
            if (!string.IsNullOrEmpty(request.PathBase))
                apiBasePath += $"{request.PathBase}";

            //Route and query path
            string[] pathElements = request.Path.Value.Split('/');

            for (int i = 0; i < pathElements.Length; i++)
            {
                if (!string.IsNullOrEmpty(pathElements[i]))
                {
                    if (pathElements[i].Equals("api"))
                    {
                        apiBasePath += $"/{pathElements[i]}/{pathElements[i + 1]}/{pathElements[i + 2]}";
                        break;
                    }
                    else
                        apiBasePath += pathElements[i];
                }
            }

            return apiBasePath;
        }

        /// <summary>
        /// Routes to string.
        /// </summary>
        /// <returns>The to string.</returns>
        /// <param name="routeData">Route data.</param>
        private void GetRouteAttributes(RouteData routeData)
        {
            //getting route values
            foreach (string key in routeData.Values.Keys)
            {
                if (key.Equals("action"))
                    this._ActionName = routeData.Values[key].ToString();
                else if (key.Equals("controller"))
                    this._ControllerName = routeData.Values[key].ToString();
                else
                    this._RouteValues[key] = routeData.Values[key].ToString();
            }
        }

        /// <summary>
        /// Routes to string.
        /// </summary>
        /// <returns>The to string.</returns>
        private string RouteToString()
        {
            //convert dictionary to string
            return JsonConvert.SerializeObject(this._RouteValues);
        }

        #endregion
    }
}
