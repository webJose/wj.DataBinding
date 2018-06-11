using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wj.DataBinding
{
    /// <summary>
    /// Observable collection that has allows the consumer to selectively freeze collection change 
    /// notifications in order to allow for faster execution of data loading processes.
    /// </summary>
    /// <typeparam name="T">The type of object contained in this collection.</typeparam>
    public class ObservableCollectionEx<T> : ObservableCollection<T>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the freeze count.  Events are not frozen when the value is zero.
        /// </summary>
        protected int FreezeCount { get; set; } = 0;

        /// <summary>
        /// Gets a Boolean value that indicates if the collection changed while event raising was 
        /// frozen last, but only if the collection is not set up for automatic event raising.  If 
        /// it is set up for automatic event raising on unfreezing, a <code>CollectionChanged</code> 
        /// event is fired and this property's value is reset to false.  The value also resets to 
        /// false whenever the event raising becomes frozen once more.
        /// </summary>
        public bool CollectionChangedWhileFrozen { get; private set; } = false;

        /// <summary>
        /// Gets or sets a Boolean value that determines if a <code>CollectionChanged</code> event 
        /// is automatically fired when the collection change events are unfrozen and the 
        /// collection reported change while its event raising was frozen.
        /// </summary>
        public bool AutoFireEventOnUnfreeze { get; set; }
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of this class using the optionally-provided value to configure 
        /// automatic event firing.
        /// </summary>
        /// <param name="autoFireEventOnUnfreeze">Optional.  It is assumed to be true if not 
        /// provided and whenever it is true, a collection change event is automatically fired 
        /// whenever the collection change events become unfrozen.</param>
        public ObservableCollectionEx(bool autoFireEventOnUnfreeze = true)
            : base()
        {
            AutoFireEventOnUnfreeze = autoFireEventOnUnfreeze;
        }

        /// <summary>
        /// Creates a new instance of this class using the optionally-provided value to configure 
        /// automatic event firing.
        /// </summary>
        /// <param name="collection">A collection containing data to be used to initialize this 
        /// collection's contents.</param>
        /// <param name="autoFireEventOnUnfreeze">Optional.  It is assumed to be true if not 
        /// provided and whenever it is true, a collection change event is automatically fired 
        /// whenever the collection change events become unfrozen.</param>
        public ObservableCollectionEx(IEnumerable<T> collection, bool autoFireEventOnUnfreeze = true)
            : base(collection)
        {
            AutoFireEventOnUnfreeze = autoFireEventOnUnfreeze;
        }

        /// <summary>
        /// Creates a new instance of this class using the optionally-provided value to configure 
        /// automatic event firing.
        /// </summary>
        /// <param name="list">A list containing data to be used to initialize this collection's 
        /// contents.</param>
        /// <param name="autoFireEventOnUnfreeze">Optional.  It is assumed to be true if not 
        /// provided and whenever it is true, a collection change event is automatically fired 
        /// whenever the collection change events become unfrozen.</param>
        public ObservableCollectionEx(List<T> list, bool autoFireEventOnUnfreeze = true)
            : base(list)
        {
            AutoFireEventOnUnfreeze = autoFireEventOnUnfreeze;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Freezes collection change notifications.  It can be called multiple times without 
        /// issues but then the corresponding unfreeze method needs to be called the exact number 
        /// of times in order to actually unfreeze collection change notifications.
        /// </summary>
        public virtual void FreezeCollectionNotifications()
        {
            ++FreezeCount;
            CollectionChangedWhileFrozen = false;
        }

        /// <summary>
        /// Unfreezes collection change notifications.  It needs to be called as many times as 
        /// the freezing function was called, not more and no less.  If it is called a less times, 
        /// the events will remain freezed; if called more times an <code>InvalidOperationException</code> 
        /// exception is thrown.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown whenever this function is called 
        /// more times than the corresponding freezing function.</exception>
        public virtual void UnfreezeCollectionNotifications()
        {
            if (FreezeCount <= 0)
            {
                throw new InvalidOperationException("Cannot unfreeze collection notifications because they are not frozen.");
            }
            --FreezeCount;
            if (FreezeCount == 0 && AutoFireEventOnUnfreeze && CollectionChangedWhileFrozen)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                CollectionChangedWhileFrozen = false;
            }
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (FreezeCount > 0)
            {
                //Event notfication is frozen.  Do not raise the event, but mark the change.
                CollectionChangedWhileFrozen = true;
                return;
            }
            base.OnCollectionChanged(e);
        }
        #endregion
    }
}
