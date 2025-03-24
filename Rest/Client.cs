using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace Tenant.API.Base.Rest
{
    public class Client : RestClient, IDisposable
    {
        #region Vaiables

        private bool disposedValue;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="T:XtraChef.API.Base.Rest.Client"/> class.
        /// </summary>
        /// <param name="baseURL"></param>
        public Client() : base()
        {
            JsonSerializerSettings DefaultSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DefaultValueHandling = DefaultValueHandling.Include,
                TypeNameHandling = TypeNameHandling.None,
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            };
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initialize rest request
        /// </summary>
        /// <param name="httpRequestObject"></param>
        /// <returns></returns>
        private RestRequest InitRestRequest(HttpRequestObject httpRequestObject)
        {
            var restRequest = new RestRequest(httpRequestObject.URL);

            if (httpRequestObject.URLParams != null)
            {
                foreach (var kv in httpRequestObject.URLParams)
                {
                    restRequest.AddOrUpdateParameter(kv.Key, kv.Value, ParameterType.QueryString);
                }
            }

            if (httpRequestObject.Headers != null)
            {
                foreach (var kv in httpRequestObject.Headers)
                {
                    restRequest.AddHeader(kv.Key, kv.Value);
                }
            }

            restRequest.AddHeader("content-type", httpRequestObject.RequestContentType);

            switch (httpRequestObject.REQUESTTYPE)
            {
                case "POST":
                    restRequest.Method = Method.Post;
                    SetRequestBody(httpRequestObject, restRequest);
                    break;
                case "PUT":
                    restRequest.Method = Method.Put;
                    SetRequestBody(httpRequestObject, restRequest);
                    break;
                case "PATCH":
                    restRequest.Method = Method.Patch;
                    SetRequestBody(httpRequestObject, restRequest);
                    break;
                case "DELETE":
                    restRequest.Method = Method.Delete;
                    break;
                default:
                    restRequest.Method = Method.Get;
                    break;
            }

            return restRequest;

        }

        /// <summary>
        /// Set request body
        /// </summary>
        /// <param name="httpRequestObject"></param>
        /// <param name="restRequest"></param>
        private void SetRequestBody(HttpRequestObject httpRequestObject, RestRequest restRequest)
        {
            if (httpRequestObject.Content != null)
            {
                if (httpRequestObject.RequestContentType == "application/json")
                    restRequest.AddStringBody((string)httpRequestObject.Content, DataFormat.Json);
                else if (httpRequestObject.RequestContentType == "application/xml")
                    restRequest.AddStringBody((string)httpRequestObject.Content, DataFormat.Xml);
                else if (httpRequestObject.RequestContentType == "application/x-www-form-urlencoded")
                {
                    Dictionary<string, string> parameters = ((Dictionary<string, string>)httpRequestObject.Content);

                    foreach (var item in parameters.Keys)
                    {
                        restRequest.AddParameter(item, parameters[item]);
                    }
                }
                else if (httpRequestObject.RequestContentType == "multipart/form-data")
                {
                    string _fileName = (JsonConvert.DeserializeObject(JsonConvert.SerializeObject(httpRequestObject.Content)))["FileName"];

                    restRequest.AddFile("file", (byte[])httpRequestObject.Content, _fileName, httpRequestObject.FileContentType);
                }
                else if (httpRequestObject.RequestContentType == "application/pdf")
                {
                    string _fileName = (JsonConvert.DeserializeObject(JsonConvert.SerializeObject(httpRequestObject.Content)))["FileName"];

                    restRequest.AddParameter("application/pdf", httpRequestObject.FileStream, ParameterType.RequestBody);

                }
            }
        }

        /// <summary>
        /// Get stream
        /// </summary>
        /// <param name="_file"></param>
        /// <returns></returns>
        private Stream GetStream(byte[] _file)
        {
            return new MemoryStream(_file);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Execute
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public RestResponse<T> Execute<T>(HttpRequestObject requestObject, CancellationToken cancellationToken = default)
        {
            RestRequest restRequest = InitRestRequest(requestObject);

            return this.ExecuteAsync<T>(restRequest, cancellationToken).Result;
        }

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public RestResponse Execute(HttpRequestObject requestObject)
        {
            var restRequest = InitRestRequest(requestObject);

            return ExecuteAsync(restRequest).Result;
        }

        /// <summary>
        /// Execute Async
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public Task<RestResponse> ExecuteAsync(HttpRequestObject requestObject)
        {
            var restRequest = InitRestRequest(requestObject);

            return ExecuteAsync(restRequest);
        }

        /// <summary>
        /// Execute
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public Task<RestResponse<T>> ExecuteAsync<T>(HttpRequestObject requestObject, CancellationToken cancellationToken = default)
        {
            RestRequest restRequest = InitRestRequest(requestObject);
            return this.ExecuteAsync<T>(restRequest, cancellationToken);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}
