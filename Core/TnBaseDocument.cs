using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Tenant.API.Base.Model;
using Tenant.API.Base.Util;
using static Tenant.API.Base.Core.Enum;
using System.Reflection;
using System.Linq;
using Tenant.API.Base.Core;

namespace Tenant.API.Base.Core
{
    public abstract class TnBaseDocument<T> : TnBase where T : TnBaseItem 
    {
        #region Properties

        [JsonIgnore, NotMapped]
        public string DocumentType { get; set; }

        [JsonProperty("status")]
        public virtual string Status { get; set; }

        [JsonProperty("addresses")]
        public List<Model.Address> Addresses { get; set; }

        [Required, JsonProperty("items")]
        public List<T> Items { get; set; }

        [JsonProperty("note")]
        public virtual Note Note { get; set; }

        [JsonIgnore, NotMapped]
        public override Enum.EntityType.Type Type { get { return Enum.EntityType.Type.DOCUMENT; } }

        [JsonProperty("audit")]
        public virtual List<Model.Audit.Audit> Audits { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="T:xcapibase.Core.XcBaseDocument"/> class.
        /// </summary>
        public TnBaseDocument()
        {
            this.Items = new List<T>();
            this.Addresses = new List<Model.Address>();
            this.DocumentType = this.getDocumentType();
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Validates the document.
        /// </summary>
        public abstract string getDocumentType();

        /// <summary>
        /// Validates the document.
        /// </summary>
        public abstract void ValidateDocument();

        /// <summary>
        /// Validates the item.
        /// </summary>
        /// <param name="item">Item.</param>
        public abstract void ValidateItem(T item);

        #endregion

        #region Item Methods

        /// <summary>
        /// Adds the line item.
        /// </summary>
        /// <param name="item">Item.</param>
        public T AddItem(T item)
        {
            //validate item
            this.ValidateItem(item);

            //create id
            item.Guid = System.Guid.NewGuid().ToString();

            //adding document id
            item.DocumentId = this.Guid;

            //adding tenant id
            item.TenantId = this.TenantId;

            //adding item to collection
            this.Items.Add(item);

            return item;
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <returns>The item.</returns>
        /// <param name="id">Identifier.</param>
        public T GetItem(string id)
        {
            return this.Items.Find(x => x.Id.Equals(id));
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <returns>The items.</returns>
        public List<T> GetItems()
        {
            return this.Items;
        }

        /// <summary>
        /// Removes the item.
        /// </summary>
        /// <param name="item">Item.</param>
        public void RemoveItem(T item)
        {
            TnBase baseEntity = item as TnBase;
            baseEntity.Operation = Enum.Operation.DELETE;

            //remove existing item.Will remove any existing item whose Id matches with the item Id
            this.Items.Remove(item);
        }

        /// <summary>
        /// Updates the item.
        /// </summary>
        /// <param name="item">Item.</param>
        public T UpdateItem(T item)
        {
            //validate item
            this.ValidateItem(item);

            //set operation
            TnBase baseEntity = item as TnBase;
            baseEntity.Operation = Enum.Operation.DELETE;

            //remove existing item. Will remove any existing item whose Id matches with the item Id
            this.Items.Remove(item);

            //add item
            this.Items.Add(item);

            return item;
        }

        #endregion

        #region Address Methods

        /// <summary>
        /// Gets the address.
        /// </summary>
        /// <returns>The address.</returns>
        /// <param name="type">Type.</param>
        public Model.Address GetAddress(Enum.Address.Type type)
        {
            return this.Addresses.Find(x => x.Type.Equals(type.ToString(), StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Adds the address.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="addressData">Address data.</param>
        public void AddAddress(Enum.Address.Type type, Model.Address addressData)
        {
            //check if address exists of the same type
            Model.Address address = this.GetAddress(type);

            if (address != null)
                throw new FieldAccessException($"Address already exists of type '{type.ToString()}'");

            //add a new address if does not exists
            addressData.Type = type.ToString();
            addressData.EntityId = this.Guid;
            addressData.TenantId = this.TenantId;

            this.Addresses.Add(addressData);
        }

        #endregion

        #region Note Method

        /// <summary>
        /// Adds the note.
        /// </summary>
        /// <param name="comment">Comment.</param>
        public void AddNote(string comment)
        {
            DateTime now = TnUtil.Date.GetCurrentTime();

            this.Note = new Note();
            this.Note.TenantId = this.TenantId;
            this.Note.Created = this.Note.LastModified = now;
            this.Note.CreatedBy = this.Note.LastModifiedBy = this.LastModifiedBy;
            this.Note.Description = comment;
        }

        /// <summary>
        /// Adds the note.
        /// </summary>
        /// <param name="comment">Comment.</param>
        public void UpdateNote(string comment)
        {
            this.Note.LastModified = TnUtil.Date.GetCurrentTime();
            this.Note.LastModifiedBy = this.LastModifiedBy;
            this.Note.Description = comment;
        }

        /// <summary>
        /// Removes the note.
        /// </summary>
        public void RemoveNote()
        {
            this.Note = null;
        }

        #endregion

        #region Audit Methods

        /// <summary>
        /// General Audit
        /// </summary>
        /// <typeparam name="T2">Enum</typeparam>
        /// <param name="type"></param>
        /// <param name="system"></param>
        /// <param name="userId"></param>
        /// <param name="description"></param>
        public Model.Audit.Audit Audit<T2>(string type, bool system, string userId, string description)
        {
            //Local variable
            API.Base.Model.Audit.Audit audit = new API.Base.Model.Audit.Audit();

            //Guid
            audit.Guid = System.Guid.NewGuid().ToString();

            audit.TenantId = this.TenantId;
            audit.EntityStatus = TnUtil.GetEnumDescription<T>(this.Status);
            audit.EntityId = this.Guid;
            audit.Severity = 100;
            audit.OperationType = type;
            audit.Internal = system;
            audit.Description = description;

            //Details of when & who
            audit.Created = Util.TnUtil.Date.GetCurrentTime();
            audit.CreatedBy = userId;

            //adding Audit
            if (this.Audits == null)
                this.Audits = new List<API.Base.Model.Audit.Audit>();
            this.Audits.Add(audit);

            return audit;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Validate this instance.
        /// </summary>
        public void Validate()
        {
            //validate items
            this.Items.ForEach(x => this.ValidateItem(x));

                
            //validate document
            this.ValidateDocument();
        }

        /// <summary>
        /// Set Delete Status
        /// </summary>
        /// <param name="userId"></param>
        public void Delete(string userId)
        {
            this.Status = "DELETED";
            this.Operation = Enum.Operation.DELETE;
        }

        #endregion
    }
}
