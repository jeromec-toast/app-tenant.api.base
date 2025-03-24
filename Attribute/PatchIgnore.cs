using System;
using System.Collections.Generic;
using System.Text;

namespace Tenant.API.Base.Attribute
{
    public class PatchIgnore : System.Attribute
    {
        public override object TypeId => base.TypeId;

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="T:Tenant.API.Base.Attribute.PatchIgnore"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="T:Tenant.API.Base.Attribute.PatchIgnore"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="T:Tenant.API.Base.Attribute.PatchIgnore"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="T:Tenant.API.Base.Attribute.PatchIgnore"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Ises the default attribute.
        /// </summary>
        /// <returns><c>true</c>, if default attribute was ised, <c>false</c> otherwise.</returns>
        public override bool IsDefaultAttribute()
        {
            return base.IsDefaultAttribute();
        }

        /// <summary>
        /// Match the specified obj.
        /// </summary>
        /// <returns>The match.</returns>
        /// <param name="obj">Object.</param>
        public override bool Match(object obj)
        {
            return base.Match(obj);
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:Tenant.API.Base.Attribute.PatchIgnore"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:Tenant.API.Base.Attribute.PatchIgnore"/>.</returns>
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
