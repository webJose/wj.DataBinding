using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wj.DataBinding.NUnitTests
{
    /// <summary>
    /// Determines whether two <code>System.ComponentModel.PropertyDescriptor</code> objects are 
    /// to be considered equal by comparing the corresponding <code>Name</code> properties.
    /// </summary>
    internal class PropertyDescriptorEqComparer : IEqualityComparer<PropertyDescriptor>
    {
        #region IEqualityComparer<PropertyDescriptor>
        public bool Equals(PropertyDescriptor x, PropertyDescriptor y)
        {
            if (x == null && y == null) return true;
            if (x == null || y == null) return false;
            return x.Name.Equals(y.Name);
        }

        public int GetHashCode(PropertyDescriptor obj)
        {
            return obj.Name.GetHashCode();
        }
        #endregion
    }
}
