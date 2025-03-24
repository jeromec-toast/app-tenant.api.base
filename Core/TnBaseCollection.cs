using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tenant.API.Base.Model;
using Tenant.API.Base.Core;

namespace Tenant.API.Base.Core
{
    public abstract class TnBaseCollection<T> where T : TnBase
    {
        #region Variables

        public List<T> Items { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Tenant.API.Base.Core.XcBaseCollection`1"/> class.
        /// </summary>
        public TnBaseCollection() 
        {
            this.Items = new List<T>();    
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the item.
        /// </summary>
        /// <param name="item">Item.</param>
        public void AddItem(T item) 
        {
            this.Items.Add(item);    
        }

        /// <summary>
        /// Adds the items.
        /// </summary>
        /// <param name="items">Items.</param>
        public void AddItems(List<T> items) 
        {
            this.Items.AddRange(items);
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <returns>The item.</returns>
        /// <param name="id">Identifier.</param>
        public T GetItem(string id) 
        {
            if(this.Items != null)
            {
                return this.Items.Find(x => x.Guid.Equals(id, StringComparison.OrdinalIgnoreCase));
            }
            return null;
        }

        /// <summary>
        /// Group the specified groupBy.
        /// </summary>
        /// <returns>The group.</returns>
        /// <param name="groupBy">Group by.</param>
        public Dictionary<object, List<T>> Group(string groupBy) 
        {
            if (this.Items != null)
            {
                if (this.Items.Count > 0)
                {
                    PropertyInfo propertyInfo = this.Items[0].GetType().GetProperty(groupBy);

                    return this.Items.GroupBy(x => propertyInfo.GetValue(x, null))
                        .ToDictionary(x => x.Key, x => x.Select(y => y).ToList());
                }
            }
            return null;
        }

        #endregion

    }
}
