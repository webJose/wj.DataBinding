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
            PropertyChangedEventHandler ev = PropertyChanged;
            if (ev != null)
            {
                ev(this, new PropertyChangedEventArgs(propertyName));
            }
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
            bool changed = !Object.Equals(storage, value);
            if (changed)
            {
                storage = value;
                RaisePropertyChanged(propertyName);
            }
            return changed;
        }
        #endregion
    }
}
