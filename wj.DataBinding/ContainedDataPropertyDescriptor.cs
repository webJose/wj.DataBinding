using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wj.DataBinding
{
    public class ContainedDataPropertyDescriptor<TData> : PropertyDescriptor
        where TData : INotifyPropertyChanged
    {
        #region Properties
        private PropertyDescriptor OriginalPD { get; set; }
        #endregion

        #region Constructors
        public ContainedDataPropertyDescriptor(PropertyDescriptor originalPd)
            : base(originalPd.Name, originalPd.Attributes.OfType<Attribute>().ToArray())
        {
            OriginalPD = originalPd;
        }
        #endregion

        #region Methods
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
