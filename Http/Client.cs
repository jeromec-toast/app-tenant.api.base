using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Tenant.API.Base.Model.Security;

namespace Tenant.API.Base.Http
{
    public class Client
    {
        #region Variables

        private ValidationContext ValidationContext;
        private IConfiguration Configuration;
        public Rest.Client RestClient;
        public Rest.HttpRequestObject httpRequestObject;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Tenant.API.Base.Http.Client"/> class.
        /// </summary>
        /// <param name="validationContext">Validation context.</param>
        public Client(IConfiguration configuration, ValidationContext validationContext)
        {
            this.ValidationContext = validationContext;
            this.Configuration = configuration;
            this.RestClient = new Rest.Client();
        }

        #endregion

        #region Public Methods

        #region Without Token

        /// <summary>
        /// Get the specified baseUrl and uri.
        /// </summary>
        /// <returns>The get.</returns>
        /// <param name="baseUrl">Base URL.</param>
        /// <param name="uri">URI.</param>
        public string Get(string baseUrl, string uri, bool isToken = true, Dictionary<string, string> headers = null)
        {
            try
            {
                //get token
                string token = string.Empty;

                //get request uri
                var requestUri = this.GetRequestUri($"{baseUrl}/{uri}");

                this.httpRequestObject = new Rest.HttpRequestObject();

                if (httpRequestObject.Headers == null)
                {
                    httpRequestObject.Headers = new Dictionary<string, string>();
                }

                if (isToken)
                {
                    token = GetTenantToken();

                    httpRequestObject.Headers.Add("Authorization", $"Bearer {token}");
                    httpRequestObject.RequestContentType = "application/json";
                }
                else
                {
                    //Add headers
                    foreach (var header in headers)
                    {
                        httpRequestObject.Headers.Add(header.Key, header.Value);
                    }
                }

                httpRequestObject.URL = requestUri.ToString();
                httpRequestObject.REQUESTTYPE = Rest.Constant.GET;

                //make a get call
                var response = RestClient.ExecuteAsync(httpRequestObject).Result;

                string data = response.Content;

                if (response.IsSuccessful)
                    return data;
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    throw new KeyNotFoundException(data);
                else
                    throw new Exception(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Update the content to the given baseUrl
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="uri"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public string Put(string baseUrl, string uri, string content)
        {
            try
            {
                //get token
                string token = string.Empty;

                token = GetTenantToken();

                //get request uri
                var requestUri = this.GetRequestUri($"{baseUrl}/{uri}");

                this.httpRequestObject = new Rest.HttpRequestObject();

                if (httpRequestObject.Headers == null)
                {
                    httpRequestObject.Headers = new Dictionary<string, string>();
                }

                httpRequestObject.Headers.Add("Authorization", $"Bearer {token}");
                httpRequestObject.URL = requestUri.ToString();
                httpRequestObject.RequestContentType = "application/json";
                httpRequestObject.REQUESTTYPE = Rest.Constant.PUT;
                httpRequestObject.Content = content;

                //make a post call
                var response = RestClient.ExecuteAsync(httpRequestObject).Result;

                string data = response.Content;

                if (response.IsSuccessful)
                    return data;
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    throw new KeyNotFoundException(data);
                else
                    throw new Exception(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Update the content to the given baseUrl
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="uri"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public string Patch(string baseUrl, string uri, string content)
        {
            try
            {
                //get token
                string token = string.Empty;

                token = GetTenantToken();

                //get request uri
                var requestUri = this.GetRequestUri($"{baseUrl}/{uri}");

                this.httpRequestObject = new Rest.HttpRequestObject();

                if (httpRequestObject.Headers == null)
                {
                    httpRequestObject.Headers = new Dictionary<string, string>();
                }

                httpRequestObject.Headers.Add("Authorization", $"Bearer {token}");
                httpRequestObject.URL = requestUri.ToString();
                httpRequestObject.RequestContentType = "application/json";
                httpRequestObject.REQUESTTYPE = Rest.Constant.PATCH;
                httpRequestObject.Content = content;

                //make a post call
                var response = RestClient.ExecuteAsync(httpRequestObject).Result;

                string data = response.Content;

                if (response.IsSuccessful)
                    return data;
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    throw new KeyNotFoundException(data);
                else
                    throw new Exception(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Post the content to the given baseUrl
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="uri"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public string Post(string baseUrl, string uri, string content)
        {
            try
            {
                //get token
                string token = string.Empty;

                token = GetTenantToken();

                //get request uri
                var requestUri = this.GetRequestUri($"{baseUrl}/{uri}");

                this.httpRequestObject = new Rest.HttpRequestObject();

                if (httpRequestObject.Headers == null)
                {
                    httpRequestObject.Headers = new Dictionary<string, string>();
                }

                httpRequestObject.Headers.Add("Authorization", $"Bearer {token}");
                httpRequestObject.URL = requestUri.ToString();
                httpRequestObject.RequestContentType = "application/json";
                httpRequestObject.REQUESTTYPE = Rest.Constant.POST;
                httpRequestObject.Content = content;

                //make a post call
                var response = RestClient.ExecuteAsync(httpRequestObject).Result;

                string data = response.Content;

                if (response.IsSuccessful)
                    return data;
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    throw new KeyNotFoundException(data);
                else
                    throw new Exception(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Post the content to the given baseUrl
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="uri"></param>
        /// <param name="content"></param>
        /// <returns></returns>       
        public string Post(string baseUrl, string uri, bool isToken = true, Dictionary<string, string> headers = null, string content = null)
        {
            try
            {
                //get token
                string token = string.Empty;

                //get request uri
                var requestUri = this.GetRequestUri($"{baseUrl}/{uri}");

                this.httpRequestObject = new Rest.HttpRequestObject();

                if (httpRequestObject.Headers == null)
                {
                    httpRequestObject.Headers = new Dictionary<string, string>();
                }

                if (isToken)
                {
                    token = GetTenantToken();
                    httpRequestObject.Headers.Add("Authorization", $"Bearer {token}");
                    httpRequestObject.RequestContentType = "application/json";
                }
                else
                {
                    //Add headers
                    foreach (var header in headers)
                    {
                        httpRequestObject.Headers.Add(header.Key, header.Value);
                    }
                }

                //HttpContent
                if (String.IsNullOrEmpty(content))
                    content = "";

                httpRequestObject.URL = requestUri.ToString();
                httpRequestObject.RequestContentType = "application/json";
                httpRequestObject.REQUESTTYPE = Rest.Constant.POST;
                httpRequestObject.Content = content;

                //make a get call
                var response = RestClient.ExecuteAsync(httpRequestObject).Result;

                string data = response.Content;

                if (response.IsSuccessful)
                    return data;
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    throw new KeyNotFoundException(data);
                else
                    throw new Exception(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Call external api
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ApiBaseUrl"></param>
        /// <param name="newClient"></param>
        /// <param name="requestUrl"></param>
        /// <param name="isToken"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public List<T> Call<T>(string ApiBaseUrl, string requestUrl, bool isToken = true, Dictionary<string, string> headers = null)
        {
            try
            {
                //Calling api
                string json = this.Get(ApiBaseUrl, $"{requestUrl}", isToken, headers);
                Model.ApiResult apiResult = JsonConvert.DeserializeObject<Model.ApiResult>(json);
                List<T> result = JsonConvert.DeserializeObject<List<T>>(apiResult.Data.ToString());

                string Exception = apiResult.Exception;

                //api exception checking
                if (Exception != null)
                    throw new System.Exception($"Error in external api call ({ApiBaseUrl}): {Exception}");

                //return 
                return result;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region WithToken

        /// <summary>
        /// Get 
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="uri"></param>
        /// <param name="isToken"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public string GetWithToken(string baseUrl, string uri, bool isToken = true, string token = null)
        {
            try
            {
                //get token
                string _token = string.Empty;

                if (isToken && !string.IsNullOrEmpty(token))
                {
                    _token = token;
                }
                else
                {
                    _token = GetTenantToken();
                }

                //get request uri
                var requestUri = this.GetRequestUri($"{baseUrl}/{uri}");

                this.httpRequestObject = new Rest.HttpRequestObject();

                if (httpRequestObject.Headers == null)
                {
                    httpRequestObject.Headers = new Dictionary<string, string>();
                }

                httpRequestObject.Headers.Add("Authorization", $"Bearer {_token}");
                httpRequestObject.URL = requestUri.ToString();
                httpRequestObject.RequestContentType = "application/json";
                httpRequestObject.REQUESTTYPE = Rest.Constant.GET;

                //make a post call
                var response = RestClient.ExecuteAsync(httpRequestObject).Result;

                string data = response.Content;

                if (response.IsSuccessful)
                    return data;
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    throw new KeyNotFoundException(data);
                else
                    throw new Exception(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Post the content to the given baseUrl
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="uri"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public string PostWithToken(string baseUrl, string uri, string content, bool isToken = true, string token = null)
        {
            try
            {
                //get token
                string _token = string.Empty;

                if (isToken && !string.IsNullOrEmpty(token))
                {
                    _token = token;
                }
                else
                {
                    _token = GetTenantToken();
                }

                //get request uri
                var requestUri = this.GetRequestUri($"{baseUrl}/{uri}");

                this.httpRequestObject = new Rest.HttpRequestObject();

                if (httpRequestObject.Headers == null)
                {
                    httpRequestObject.Headers = new Dictionary<string, string>();
                }

                httpRequestObject.Headers.Add("Authorization", $"Bearer {_token}");
                httpRequestObject.URL = requestUri.ToString();
                httpRequestObject.RequestContentType = "application/json";
                httpRequestObject.REQUESTTYPE = Rest.Constant.POST;
                httpRequestObject.Content = content;

                //make a post call
                var response = RestClient.ExecuteAsync(httpRequestObject).Result;

                string data = response.Content;

                if (response.IsSuccessful)
                    return data;
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    throw new KeyNotFoundException(data);
                else
                    throw new Exception(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Call with token
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ApiBaseUrl"></param>
        /// <param name="requestUrl"></param>
        /// <param name="isToken"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public List<T> CallWithToken<T>(string ApiBaseUrl, string requestUrl, bool isToken = true, string token = null)
        {
            try
            {
                //Calling api
                string json = this.GetWithToken(ApiBaseUrl, $"{requestUrl}", isToken, token);
                Model.ApiResult apiResult = JsonConvert.DeserializeObject<Model.ApiResult>(json);
                List<T> result = (apiResult.Data != null) ? JsonConvert.DeserializeObject<List<T>>(apiResult.Data.ToString()) : null;

                string Exception = apiResult.Exception;

                //api exception checking
                if (Exception != null)
                    throw new System.Exception($"Error in external api call ({ApiBaseUrl}): {Exception}");

                //return 
                return result;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Put with token
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="uri"></param>
        /// <param name="content"></param>
        /// <param name="isToken"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public string PutWithToken(string baseUrl, string uri, string content, bool isToken = true, string token = null)
        {
            try
            {
                //get token
                string _token = string.Empty;

                if (isToken && !string.IsNullOrEmpty(token))
                {
                    _token = token;
                }
                else
                {
                    _token = GetTenantToken();
                }

                //get request uri
                var requestUri = this.GetRequestUri($"{baseUrl}/{uri}");

                this.httpRequestObject = new Rest.HttpRequestObject();

                if (httpRequestObject.Headers == null)
                {
                    httpRequestObject.Headers = new Dictionary<string, string>();
                }

                httpRequestObject.Headers.Add("Authorization", $"Bearer {_token}");
                httpRequestObject.URL = requestUri.ToString();
                httpRequestObject.RequestContentType = "application/json";
                httpRequestObject.REQUESTTYPE = Rest.Constant.PUT;
                httpRequestObject.Content = content;

                //make a post call
                var response = RestClient.ExecuteAsync(httpRequestObject).Result;

                string data = response.Content;

                if (response.IsSuccessful)
                    return data;
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    throw new KeyNotFoundException(data);
                else
                    throw new Exception(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Patch with token
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="uri"></param>
        /// <param name="content"></param>
        /// <param name="isToken"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public string PatchWithToken(string baseUrl, string uri, string content, bool isToken = true, string token = null)
        {
            try
            {
                //get token
                string _token = string.Empty;

                if (isToken && !string.IsNullOrEmpty(token))
                {
                    _token = token;
                }
                else
                {
                    _token = GetTenantToken();
                }

                //get request uri
                var requestUri = this.GetRequestUri($"{baseUrl}/{uri}");

                this.httpRequestObject = new Rest.HttpRequestObject();

                if (httpRequestObject.Headers == null)
                {
                    httpRequestObject.Headers = new Dictionary<string, string>();
                }

                httpRequestObject.Headers.Add("Authorization", $"Bearer {_token}");
                httpRequestObject.URL = requestUri.ToString();
                httpRequestObject.RequestContentType = "application/json";
                httpRequestObject.REQUESTTYPE = Rest.Constant.PATCH;
                httpRequestObject.Content = content;

                //make a post call
                var response = RestClient.ExecuteAsync(httpRequestObject).Result;

                string data = response.Content;

                if (response.IsSuccessful)
                    return data;
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    throw new KeyNotFoundException(data);
                else
                    throw new Exception(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the token.
        /// </summary>
        /// <returns>The token.</returns>
        /// <param name="baseUrl">Base URL.</param>
        public string GetToken(string baseUrl)
        {
            //convert to JSON
            string validationContextJson = JsonConvert.SerializeObject(this.ValidationContext);

            //get request uri
            var requestUri = this.GetRequestUri($"{baseUrl}/token");

            //assign httpRequestObject
            httpRequestObject.URL = requestUri.ToString();
            httpRequestObject.REQUESTTYPE = Rest.Constant.POST;
            httpRequestObject.Content = validationContextJson;
            httpRequestObject.RequestContentType = "application/json";

            //make a post call
            var response = RestClient.ExecuteAsync(httpRequestObject).Result;

            string data = response.Content;

            if (response.IsSuccessful)
                return data;
            else
                throw new Exception(data);
        }

        /// <summary>
        /// Get request uri
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public Uri GetRequestUri(string uri)
        {
            string pattern = "/+";
            string replacement = "/";
            Regex rgx = new Regex(pattern);
            string result = rgx.Replace(uri, replacement);

            result = result.Replace(":/", "://");

            return new Uri(result);
        }

        /// <summary>
        /// Get tenant token
        /// </summary>
        /// <returns></returns>
        private string GetTenantToken()
        {
            //get the api url from config
            string baseUrl = this.Configuration["APIUrl:Authentication:Query"];

            this.httpRequestObject = new Rest.HttpRequestObject();

            //Add internal flag as true.Since, TenantId,LocationId,UserId and UserRole validation is not needed.
            //NOTE: If it is needed to validate the TenantId,LocationId,UserId and UserRole,Remove the internal flag and give proper context.
            this.ValidationContext.Internal = Core.Constant.Internal;

            //convert to JSON
            string validationContextJson = JsonConvert.SerializeObject(this.ValidationContext);

            //get request uri
            var requestUri = this.GetRequestUri($"{baseUrl}/tenant-token");

            //assign httpRequestObject
            httpRequestObject.URL = requestUri.ToString();
            httpRequestObject.REQUESTTYPE = Rest.Constant.POST;
            httpRequestObject.Content = validationContextJson;
            httpRequestObject.RequestContentType = "application/json";

            //make a post call
            var response = RestClient.ExecuteAsync(httpRequestObject).Result;

            string data = response.Content;

            if (response.IsSuccessful)
                return data;
            else
                throw new Exception(data);
        }

        #endregion
    }
}
