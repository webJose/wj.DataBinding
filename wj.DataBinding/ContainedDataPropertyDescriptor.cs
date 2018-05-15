using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wj.DataBinding
{
    /// <summary>
    /// Custom property descriptor class that helps the <code>wj.DataBinding.Container</code> 
    /// class present the properties of the contained data object as its own.
    /// </summary>
    /// <typeparam name="TData">The type of data object to be contained.  It must implement the 
    /// <code>System.ComponentModel.INotifyPropertyChanged</code> interface.</typeparam>
    public class ContainedDataPropertyDescriptor<TData> : PropertyDescriptor
        where TData : INotifyPropertyChanged
    {
        #region Properties
        /// <summary>
        /// Gets or sets the original property descriptor object.
        /// </summary>
        private PropertyDescriptor OriginalPD { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        /// <param name="originalPd">The original property descriptor of the object.</param>
        public ContainedDataPropertyDescriptor(PropertyDescriptor originalPd)
            : base(originalPd.Name, originalPd.Attributes.OfType<Attribute>().ToArray())
        {
            OriginalPD = originalPd;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Casts the given object as a container object.
        /// </summary>
        /// <param name="component">The object to cast.</param>
        /// <returns>The same object cast to the needed type.</returns>
        private Container<TData> AsSelectedEntity(object component)
        {
            return component as Container<TData>;
        }

        public override bool CanResetValue(object component)
        {
            Container<TData> selEntity = AsSelectedEntity(component);
            return OriginalPD.CanResetValue(selEntity.DataObject);
        }

        public override Type ComponentType
        {
            get
            {
                return typeof(Container<TData>);
            }
        }

        public override object GetValue(object component)
        {
            Container<TData> selEntity = AsSelectedEntity(component);
            return OriginalPD.GetValue(selEntity.DataObject);
        }

        public override bool IsReadOnly
        {
            get { return OriginalPD.IsReadOnly; }
        }

        public override Type PropertyType
        {
            get { return OriginalPD.PropertyType; }
        }

        public override void ResetValue(object component)
        {
            Container<TData> selEntity = AsSelectedEntity(component);
            OriginalPD.ResetValue(selEntity.DataObject);
        }

        public override void SetValue(object component, object value)
        {
            Container<TData> selEntity = AsSelectedEntity(component);
            OriginalPD.SetValue(selEntity.DataObject, value);
        }

        public override bool ShouldSerializeValue(object component)
        {
            Container<TData> selEntity = AsSelectedEntity(component);
            return OriginalPD.ShouldSerializeValue(selEntity.DataObject);
        }
        #endregion
    }
}
