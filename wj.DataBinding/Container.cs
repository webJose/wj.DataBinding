using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wj.DataBinding
{
    public class Container<TData> : NotifyPropertyChanged, ICustomTypeDescriptor
        where TData : INotifyPropertyChanged
    {
        #region Static Section
        private static bool NoCustomTypeDescription = true;
        #endregion

        #region Properties
        private bool m_selected;
        public bool Selected
        {
            get { return m_selected; }
            set { SaveAndNotify(ref m_selected, value); }
        }

        private TData m_dataObject;
        [Browsable(false)]
        public TData DataObject
        {
            get { return m_dataObject; }
            set { SaveAndNotify(ref m_dataObject, value); }
        }
        #endregion

        #region Constructors
        public Container(TData dataObject)
            : base()
        {
            DataObject = dataObject;
            DataObject.PropertyChanged += DataObjectPropertyChangedHandler;
        }
        #endregion

        #region Methods
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
