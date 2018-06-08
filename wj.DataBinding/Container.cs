using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wj.DataBinding
{
    /// <summary>
    /// Class that extends another for the purposes of data binding.  Inheritance is not used and 
    /// instead it fakes property descriptors and containment to make databound controls think 
    /// classes that inherit from this one also contain the properties of the contained object.
    /// </summary>
    /// <typeparam name="TData">The type of contained object.  It must implement 
    /// <code>System.ComponentModel.INotifyPropertyChanged</code>.</typeparam>
    public class Container<TData> : NotifyPropertyChanged, ICustomTypeDescriptor
        where TData : INotifyPropertyChanged
    {
        #region Static Section

        /// <summary>
        /// Boolean value used in the implementation of <code>System.ComponentModel.ICustomTypeDescriptor</code> 
        /// in case implementation ever changes.  Currently the documentation of the base class 
        /// used to implement the interface contradicts the intuitive parameter name in all methods.
        /// 
        /// In case that ever changes (unlikely, granted) all that it takes is to change the value 
        /// here.
        /// </summary>
        private const bool NoCustomTypeDescription = true;
        #endregion

        #region Properties

        /// <summary>
        /// Storage variable for the <code>wj.DataBinding.Container.DataObject</code> property.
        /// </summary>
        private TData m_dataObject;

        /// <summary>
        /// Gets or sets the data object to contain.
        /// </summary>
        [Browsable(false)]
        public TData DataObject
        {
            get { return m_dataObject; }
            set
            {
                if (m_dataObject != null)
                {
                    m_dataObject.PropertyChanged -= DataObjectPropertyChangedHandler;
                }
                SaveAndNotify(ref m_dataObject, value);
                if (m_dataObject != null)
                {
                    m_dataObject.PropertyChanged += DataObjectPropertyChangedHandler;
                }
            }
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        /// <param name="dataObject">The data object to contain.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "Caused by the raising of the PropertyChanged event in NotifyPropertyChanged for the DataObject property.")]
        public Container(TData dataObject)
            : base()
        {
            DataObject = dataObject;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Forwards property change notifications of the contained data object.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">The property change event data.</param>
        private void DataObjectPropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(e.PropertyName);
        }
        #endregion

        #region ICustomTypeDescriptor
        AttributeCollection ICustomTypeDescriptor.GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, NoCustomTypeDescription);
        }

        string ICustomTypeDescriptor.GetClassName()
        {
            return TypeDescriptor.GetClassName(this, NoCustomTypeDescription);
        }

        string ICustomTypeDescriptor.GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, NoCustomTypeDescription);
        }

        TypeConverter ICustomTypeDescriptor.GetConverter()
        {
            return TypeDescriptor.GetConverter(this, NoCustomTypeDescription);
        }

        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, NoCustomTypeDescription);
        }

        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, NoCustomTypeDescription);
        }

        object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, NoCustomTypeDescription);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, NoCustomTypeDescription);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            return TypeDescriptor.GetEvents(this, NoCustomTypeDescription);
        }

        /// <summary>
        /// Creates a collection of property descriptors that contain both this object's property 
        /// descriptors as well as exchanged property descriptors for the contained data object.
        /// </summary>
        /// <param name="nativePds">The collection of property descriptors that belong to this 
        /// object.</param>
        /// <param name="dataObjectPds">The collection of original property descriptors obtained 
        /// from the contained data object.</param>
        /// <returns>A collection of property descriptor objects that represent both the 
        /// properties of this object as well as the contained data object.</returns>
        private PropertyDescriptorCollection CreateMergedPropertyDescriptorCollection(PropertyDescriptorCollection nativePds, PropertyDescriptorCollection dataObjectPds)
        {
            PropertyDescriptorCollection pds = new PropertyDescriptorCollection(nativePds.OfType<PropertyDescriptor>().ToArray());
            foreach (PropertyDescriptor pd in dataObjectPds)
            {
                pds.Add(new ContainedDataPropertyDescriptor<TData>(pd));
            }
            return pds;
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
        {
            PropertyDescriptorCollection nativePds = TypeDescriptor.GetProperties(this, attributes, NoCustomTypeDescription);
            PropertyDescriptorCollection dataObjectPds = TypeDescriptor.GetProperties(DataObject, attributes);
            return CreateMergedPropertyDescriptorCollection(nativePds, dataObjectPds);
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            PropertyDescriptorCollection nativePds = TypeDescriptor.GetProperties(this, NoCustomTypeDescription);
            PropertyDescriptorCollection dataObjectPds = TypeDescriptor.GetProperties(DataObject);
            return CreateMergedPropertyDescriptorCollection(nativePds, dataObjectPds);
        }

        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
        {
            if (pd is ContainedDataPropertyDescriptor<TData>)
            {
                return DataObject;
            }
            return this;
        }
        #endregion

        #region Operators
        public static implicit operator TData(Container<TData> op)
        {
            return op.DataObject;
        }
        #endregion
    }
}
