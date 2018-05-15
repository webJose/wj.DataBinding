using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace wj.DataBinding
{
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler ev = PropertyChanged;
            if (ev != null)
            {
                ev(this, new PropertyChangedEventArgs(propertyName));
            }
        }

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
