using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Tenant.API.Base.Model.Security;
using static Tenant.API.Base.Core.Enum;

namespace Tenant.API.Base.Http
{
    public class TPICLient : Client
    {
        public TPICLient(IConfiguration configuration, ValidationContext validationContext) : base(configuration, validationContext)
        {
        }

        /// <summary>
        /// Post
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="uri"></param>
        /// <param name="content"></param>
        /// <param name="isToken"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public virtual Model.ApiResult Post(string baseUrl, string uri, string content, bool isToken = true, Dictionary<string, string> headers = null)
        {
            try
            {
                //get token
                string token = string.Empty;

                //get request uri
                var requestUri = GetRequestUri($"{baseUrl}{uri}");

                this.httpRequestObject = new Rest.HttpRequestObject();

                if (httpRequestObject.Headers == null)
                {
                    httpRequestObject.Headers = new Dictionary<string, string>();
                }

                if (isToken)
                {
                    token = GetToken(baseUrl);

                    httpRequestObject.Headers.Add("Authorization", $"Bearer {token}");
                    httpRequestObject.RequestContentType = "application/json";
                }
                else
                {
                    // Add headers
                    foreach (var header in headers)
                    {
                        if (header.Key == "Basic")
                        {
                            httpRequestObject.Headers.Add("Authorization", $"{header.Key} {header.Value}");
                        }
                        else
                        {
                            httpRequestObject.Headers.Add(header.Key, header.Value);
                        }
                    }

                }
         

                httpRequestObject.URL = requestUri.ToString();
                httpRequestObject.RequestContentType = "application/x-www-form-urlencoded";
                httpRequestObject.REQUESTTYPE = Rest.Constant.POST;
                httpRequestObject.Content = headers;

                //make a get call
                var response = RestClient.ExecuteAsync(httpRequestObject).Result;

                string data = response.Content;

                if (response.IsSuccessful)
                    return new Model.ApiResult() { Data = data };
                else
                    return new Model.ApiResult() { Exception = data };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get the specified baseUrl and uri.
        /// </summary>
        /// <returns>The get.</returns>
        /// <param name="baseUrl">Base URL.</param>
        /// <param name="uri">URI.</param>
        public virtual string Get(string baseUrl, string uri, bool isToken = true, Dictionary<string, string> headers = null)
        {
            try
            {
                //get token
                string token = string.Empty;

                //get request uri
                var requestUri = GetRequestUri($"{baseUrl}/{uri}");

                this.httpRequestObject = new Rest.HttpRequestObject();

                if (httpRequestObject.Headers == null)
                {
                    httpRequestObject.Headers = new Dictionary<string, string>();
                }

                if (isToken)
                {
                    token = GetToken(baseUrl);
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
                else
                    throw new Exception(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

    public class Miles : TPICLient
    {
        public Miles(IConfiguration configuration, Model.Security.ValidationContext validationContext) : base(configuration, validationContext)
        {
        }

        /// <summary>
        /// Miles.com Post the content to the given baseUrl
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="uri"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public override Model.ApiResult Post(string baseUrl, string uri, string content, bool isToken = true, Dictionary<string, string> headers = null)
        {
            try
            {
                //get token
                string token = string.Empty;

                //get request uri
                var requestUri = GetRequestUri($"{baseUrl}{uri}");

                this.httpRequestObject = new Rest.HttpRequestObject();

                if (httpRequestObject.Headers == null)
                {
                    httpRequestObject.Headers = new Dictionary<string, string>();
                }

                if (isToken)
                {
                    token = GetToken(baseUrl);
                    httpRequestObject.Headers.Add("Authorization", $"Bearer {token}");
                }
                else
                {
                    //Add headers
                    foreach (var header in headers)
                    {
                        if (header.Key == HttpHeaderType.Basic.ToString() || header.Key == HttpHeaderType.Bearer.ToString())
                        {
                            httpRequestObject.Headers.Add("Authorization", $"{header.Key} {header.Value}");
                        }
                        else
                        {
                            httpRequestObject.Headers.Add(header.Key, header.Value);
                        }

                    }
                }

                httpRequestObject.URL = requestUri.ToString();
                httpRequestObject.RequestContentType = "application/json";
                httpRequestObject.REQUESTTYPE = Rest.Constant.POST;
                httpRequestObject.Content = content;

                //make a get call
                var response = RestClient.ExecuteAsync(httpRequestObject).Result;

                string data = response.Content;

                if (response.IsSuccessful)
                    return new Model.ApiResult() { Data = data };
                else
                    return new Model.ApiResult() { Exception = data };//Exception(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Miles.com Post the content to the given baseUrl
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="uri"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public Model.ApiResult PostToken(string baseUrl, string uri, string content, bool isToken = true, Dictionary<string, string> headers = null)
        {
            try
            {
                //get token
                string token = string.Empty;

                //get request uri
                var requestUri = GetRequestUri($"{baseUrl}{uri}");

                this.httpRequestObject = new Rest.HttpRequestObject();

                if (httpRequestObject.Headers == null)
                {
                    httpRequestObject.Headers = new Dictionary<string, string>();
                }

                if (isToken)
                {
                    token = GetToken(baseUrl);
                    httpRequestObject.Headers.Add("Authorization", $"Bearer {token}");
                    httpRequestObject.RequestContentType = "application/json";
                }
                else
                {
                    //Add headers
                    foreach (var header in headers)
                    {
                        if (header.Key == HttpHeaderType.Basic.ToString() || header.Key == HttpHeaderType.Bearer.ToString())
                        {
                            httpRequestObject.Headers.Add("Authorization", $"{header.Key} {header.Value}");
                        }
                        else
                        {
                            httpRequestObject.Headers.Add(header.Key, header.Value);
                        }
                    }
                }

                httpRequestObject.URL = requestUri.ToString();
                httpRequestObject.RequestContentType = "application/x-www-form-urlencoded";
                httpRequestObject.REQUESTTYPE = Rest.Constant.POST;
                httpRequestObject.Content = headers;

                //make a get call
                var response = RestClient.ExecuteAsync(httpRequestObject).Result;

                string data = response.Content;

                if (response.IsSuccessful)
                    return new Model.ApiResult() { Data = data };
                else
                    return new Model.ApiResult() { Exception = data };//Exception(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get the specified baseUrl and uri.
        /// </summary>
        /// <returns>The get.</returns>
        /// <param name="baseUrl">Base URL.</param>
        /// <param name="uri">URI.</param>
        public override string Get(string baseUrl, string uri, bool isToken = true, Dictionary<string, string> headers = null)
        {
            try
            {
                //get token
                string token = string.Empty;

                //get request uri
                var requestUri = GetRequestUri($"{baseUrl}/{uri}");

                this.httpRequestObject = new Rest.HttpRequestObject();

                if (isToken)
                {
                    token = GetToken(baseUrl);
                    httpRequestObject.Headers.Add("Authorization", $"Bearer {token}");
                    httpRequestObject.RequestContentType = "application/json";
                }
                else
                {
                    //Add headers
                    foreach (var header in headers)
                    {
                        if (header.Key == HttpHeaderType.Basic.ToString() || header.Key == HttpHeaderType.Bearer.ToString())
                        {
                            httpRequestObject.Headers.Add("Authorization", $"{header.Key} {header.Value}");
                        }
                        else
                        {
                            httpRequestObject.Headers.Add(header.Key, header.Value);
                        }
                    }
                }

                httpRequestObject.URL = requestUri.ToString();
                httpRequestObject.REQUESTTYPE = Rest.Constant.GET;

                //make a get call
                var response = RestClient.ExecuteAsync(httpRequestObject).Result;

                string data = response.Content;

                if (response.IsSuccessful)
                    return data;
                else
                    throw new Exception(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

    public class Intacct : TPICLient
    {
        public Intacct(IConfiguration configuration, Model.Security.ValidationContext validationContext) : base(configuration, validationContext)
        {
        }

        /// <summary>
        /// Intacct Post the content to the given baseUrl
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="uri"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public override Model.ApiResult Post(string baseUrl, string uri, string content, bool isToken = true, Dictionary<string, string> headers = null)
        {
            try
            {
                //get token
                string token = string.Empty;

                //get request uri
                var requestUri = GetRequestUri($"{baseUrl}");

                this.httpRequestObject = new Rest.HttpRequestObject();

                if (httpRequestObject.Headers == null)
                {
                    httpRequestObject.Headers = new Dictionary<string, string>();
                }

                httpRequestObject.URL = requestUri.ToString();
                httpRequestObject.RequestContentType = "application/xml";
                httpRequestObject.REQUESTTYPE = Rest.Constant.POST;
                httpRequestObject.Content = content;

                //make a post call
                var response = RestClient.ExecuteAsync(httpRequestObject).Result;

                string data = response.Content;

                if (response.IsSuccessful)
                    return new Model.ApiResult() { Data = data };

                else
                    return new Model.ApiResult() { Exception = data };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}
