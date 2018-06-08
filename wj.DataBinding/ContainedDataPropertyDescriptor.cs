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

        public override bool IsReadOnly
        {
            get { return OriginalPD.IsReadOnly; }
        }

        public override Type PropertyType
        {
            get { return OriginalPD.PropertyType; }
        }
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
        private Container<TData> AsContainerEntity(object component)
        {
            return component as Container<TData>;
        }

        /// <summary>
        /// Assumes the given object is a container object in order to cast it and then return 
        /// its contained data object.
        /// </summary>
        /// <param name="component">The object to cast and extract the object from.</param>
        /// <returns>The contained data object.</returns>
        private TData ContainedObject(object component)
        {
            return AsContainerEntity(component).DataObject;
        }

        public override bool CanResetValue(object component)
        {
            return OriginalPD.CanResetValue(ContainedObject(component));
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
            return OriginalPD.GetValue(ContainedObject(component));
        }

        public override void ResetValue(object component)
        {
            OriginalPD.ResetValue(ContainedObject(component));
        }

        public override void SetValue(object component, object value)
        {
            OriginalPD.SetValue(ContainedObject(component), value);
        }

        public override bool ShouldSerializeValue(object component)
        {
            return OriginalPD.ShouldSerializeValue(ContainedObject(component));
        }
        #endregion
    }
}
