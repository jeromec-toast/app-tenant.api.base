using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.JsonPatch.Adapters;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Tenant.API.Base.Attribute;
using System.Linq;
using Newtonsoft.Json.Serialization;

namespace Tenant.API.Base.Adapter
{
    public sealed class JsonPatchAdapter<T> : IObjectAdapter where T : class
    {

        #region Public property

        private readonly IEnumerable<string> _patchIgnoreProperties;

        private readonly Type _patchIgnoreType = typeof(PatchIgnore);

        private ObjectAdapter _objectAdapter => new ObjectAdapter(new DefaultContractResolver(), null);

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of JsonPatchAdapter.
        /// </summary>
        public JsonPatchAdapter()
        {
            var sourceType = typeof(T);

            _patchIgnoreProperties = from property in sourceType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                     where property.CustomAttributes.Any(customAttribute => customAttribute.AttributeType == _patchIgnoreType)
                                     select $"/{property.Name.ToLower()}";
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Add Operation
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="objectToApplyTo"></param>
        public void Add(Operation operation, object objectToApplyTo)
        {
            if (!_patchIgnoreProperties.Any(c => c.Equals(operation.path, StringComparison.InvariantCultureIgnoreCase)))
            {
                _objectAdapter.Add(operation, objectToApplyTo);
            }
            else
            {
                throw new NotSupportedException($"The operation [{operation.op}] not supported for [{operation.path}].");
            }
        }

        /// <summary>
        /// Copy Operation
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="objectToApplyTo"></param>
        public void Copy(Operation operation, object objectToApplyTo)
        {
            //_objectAdapter.Copy(operation, objectToApplyTo);
            throw new NotSupportedException($"The operation [{operation.op}] not supported for [{operation.path}].");
        }

        /// <summary>
        /// Move Operation
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="objectToApplyTo"></param>
        public void Move(Operation operation, object objectToApplyTo)
        {
            //_objectAdapter.Move(operation, objectToApplyTo);
            throw new NotSupportedException($"The operation [{operation.op}] not supported for [{operation.path}].");
        }

        /// <summary>
        /// Remove Operation
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="objectToApplyTo"></param>
        public void Remove(Operation operation, object objectToApplyTo)
        {
            if (!_patchIgnoreProperties.Any(c => c.Equals(operation.path, StringComparison.InvariantCultureIgnoreCase)))
            {
                _objectAdapter.Remove(operation, objectToApplyTo);
            }
            else
            {
                throw new NotSupportedException($"The operation {operation.op} not allowed for the : {operation.path}.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="objectToApplyTo"></param>
        public void Replace(Operation operation, object objectToApplyTo)
        {
            if (!_patchIgnoreProperties.Any(c => c.Equals(operation.path, StringComparison.InvariantCultureIgnoreCase)))
            {
                _objectAdapter.Replace(operation, objectToApplyTo);
            }
            else
            {
                throw new NotSupportedException($"The operation {operation.op} not allowed for the : {operation.path}.");
            }
        }

        #endregion
    }
}
