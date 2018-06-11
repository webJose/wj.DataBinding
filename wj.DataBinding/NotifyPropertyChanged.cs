using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace wj.DataBinding
{
    /// <summary>
    /// Provides a base implementation for the <code>System.ComponentModel.INotifyPropertyChanged</code>
    /// interface, which is instrumental in data binding for Windows Forms and WPF.
    /// </summary>
    [Serializable]
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        /// <summary>
        /// Notifies subscribers whenever the value of a property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Triggers the PropertyChanged event using the provided property name.
        /// </summary>
        /// <param name="propertyName">The name of the property that allegedly changed in value.  
        /// If a value is not provided, then the value is automatically set to the name of the 
        /// method that called this function.</param>
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Compares two values and returns a Boolean value that indicates if the provided values 
        /// differ.
        /// </summary>
        /// <param name="val1">The first value to be compared.</param>
        /// <param name="val2">The second value to be compared.</param>
        /// <returns>True if the values differ; false otherwise.</returns>
        /// <remarks>Derived classes may override this function in case is needed.</remarks>
        protected virtual bool PropertyValueChanges(object val1, object val2)
        {
            return !Object.Equals(val1, val2);
        }

        /// <summary>
        /// Saves the specified value in the specified storage variable, and if the value between 
        /// storage and value differs, raises the <code>wj.DataBinding.NotifyPropertyChanged.PropertyChanged</code> 
        /// event.
        /// </summary>
        /// <typeparam name="TData">The type of data being stored.</typeparam>
        /// <param name="storage">A reference to the storage variable to use for the provided value.</param>
        /// <param name="value">The new value to store in the storage variable.</param>
        /// <param name="propertyName">The name of the property that allegedly changed in value.  
        /// If a value is not provided, then the value is automatically set to the name of the 
        /// method that called this function.</param>
        /// <returns>True if the value provided differs from the value in store in the storage 
        /// variable; false otherwise.</returns>
        protected virtual bool SaveAndNotify<TData>(ref TData storage, TData value, [CallerMemberName] string propertyName = null)
        {
            bool changed = PropertyValueChanges(storage, value);
            if (changed)
            {
                storage = value;
                RaisePropertyChanged(propertyName);
            }
            return changed;
        }

        /// <summary>
        /// Saves the specified value in the specified nullable storage variable, and if the value 
        /// between storage and value differs, raises the <code>wj.DataBinding.NotifyPropertyChanged.PropertyChanged</code> 
        /// event.  If the storage variable is null (does not have a value), it is guaranteed that 
        /// the event will be raised.
        /// </summary>
        /// <typeparam name="TData">The non-nullable type of data being stored in a nullable 
        /// storage variable.</typeparam>
        /// <param name="storage">A reference to the nullable storage variable to use for the 
        /// provided value.</param>
        /// <param name="value">The new value to store in the nullable storage variable.</param>
        /// <param name="propertyName">The name of the property that allegedly changed in value.  
        /// If a value is not provided, then the value is automatically set to the name of the 
        /// method that called this function.</param>
        /// <returns>True if the value provided differs from the value in store in the nullable 
        /// storage variable; false otherwise.  For nullable storages that are null (have no 
        /// value), the return value will always be true.</returns>
        protected virtual bool SaveAndNotify<TData>(ref TData? storage, TData value, [CallerMemberName] string propertyName = null)
            where TData : struct
        {
            bool changed = true;
            if (storage.HasValue)
            {
                changed = PropertyValueChanges(storage, value);
                if (changed)
                {
                    storage = value;
                    RaisePropertyChanged(propertyName);
                }
            }
            else
            {
                storage = value;
                RaisePropertyChanged(propertyName);
            }
            return changed;
        }
        #endregion
    }
}
