using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wj.DataBinding.NUnitTests
{
    [TestFixture]
    public class ObservableCollectionExTests
    {
        private class CollectionChangedHandler
        {
            #region Properties
            public int EventCount { get; private set; } = 0;
            #endregion

            public CollectionChangedHandler(INotifyCollectionChanged collection)
            {
                collection.CollectionChanged += CollectionChangedFn;
            }

            private void CollectionChangedFn(object sender, NotifyCollectionChangedEventArgs e)
            {
                Debug.Print($"Collection changed:  {e.Action}");
                ++EventCount;
            }
        }

        #region Methods
        private CollectionChangedHandler SetupNotifyCollectionChangedHandler(INotifyCollectionChanged collection)
        {
            return new CollectionChangedHandler(collection);
        }
        #endregion

        #region Tests

        /// <summary>
        /// Tests basic functionality:  CollectionChanged must not be raised on collection change.
        /// </summary>
        [Test]
        public void Freeze()
        {
            //Arrange.
            ObservableCollectionEx<object> collection = new ObservableCollectionEx<object>();
            CollectionChangedHandler handler = SetupNotifyCollectionChangedHandler(collection);

            //Act.
            collection.FreezeCollectionNotifications();
            collection.Add(new object());

            //Assert.
            Assert.That(handler.EventCount == 0, $"CollectionChanged event was raised {handler.EventCount} time(s) when it was allegedly frozen.");
        }

        /// <summary>
        /// Tests automatic event firing on event firing unfreezing.
        /// </summary>
        [Test]
        public void AutoFireCollectionChangedOnUnfreeze()
        {
            //Arrange.
            ObservableCollectionEx<object> collection = new ObservableCollectionEx<object>();
            CollectionChangedHandler handler = SetupNotifyCollectionChangedHandler(collection);

            //Act.
            collection.FreezeCollectionNotifications();
            collection.Add(new object());
            collection.UnfreezCollectionNotifications();

            //Assert.
            Assert.That(handler.EventCount == 1, $"CollectionChanged event was raised {handler.EventCount} time(s) when the count was expected to be 1.");
        }

        /// <summary>
        /// Tests that unfreezing the events too much throws an exception.
        /// </summary>
        [Test]
        public void UnfreezingImbalanceThrows()
        {
            //Arrange.
            ObservableCollectionEx<object> collection = new ObservableCollectionEx<object>();
            Random r = new Random();
            int freezeCount = r.Next(5, 30);
            int unfreezeCount = 0;

            //Act.
            for (int i = 0; i < freezeCount; ++i)
            {
                collection.FreezeCollectionNotifications();
            }

            //Assert.
            Assert.Throws<InvalidOperationException>(() =>
            {
                for (int i = 0; i <= freezeCount; ++i)
                {
                    ++unfreezeCount;
                    collection.UnfreezCollectionNotifications();
                }
            });
            Assert.That(unfreezeCount - freezeCount == 1, $"Unfreezing did throw as expected, but did not do it at the expected iteration ({unfreezeCount}.");
        }

        /// <summary>
        /// Tests if the collection is properly aware of changes while frozen.
        /// </summary>
        [Test]
        public void CollectionChangedDetectionWhileFrozen()
        {
            //Arrange.
            ObservableCollectionEx<object> collection = new ObservableCollectionEx<object>();
            CollectionChangedHandler handler = SetupNotifyCollectionChangedHandler(collection);

            //Act.
            collection.FreezeCollectionNotifications();
            collection.Add(new object());

            //Assert.
            Assert.That(collection.CollectionChangedWhileFrozen, $"The collection changed while frozen but the change was not reported in the {nameof(ObservableCollectionEx<object>.CollectionChangedWhileFrozen)} property.");
        }

        /// <summary>
        /// Tests if the collection changed flag is reset to false after the automatically raised 
        /// <code>CollectionChanged</code> event.
        /// </summary>
        [Test]
        public void CollectionChangedFlagResetWithAutoRaiseEvent()
        {
            //Arrange.
            ObservableCollectionEx<object> collection = new ObservableCollectionEx<object>();

            //Act.
            collection.FreezeCollectionNotifications();
            collection.Add(new object());
            collection.UnfreezCollectionNotifications();

            //Assert.
            Assert.That(!collection.CollectionChangedWhileFrozen, $"The collection did not reset the {nameof(ObservableCollectionEx<object>.CollectionChangedWhileFrozen)} property to false automatically after unfreezing.");
        }

        /// <summary>
        /// Tests if the collection changed flag remains true after unfreezing event raising. 
        /// </summary>
        [Test]
        public void CollectionChangedFlagNotResetWithoutAutoRaiseEvent()
        {
            //Arrange.
            ObservableCollectionEx<object> collection = new ObservableCollectionEx<object>(false);

            //Act.
            collection.FreezeCollectionNotifications();
            collection.Add(new object());
            collection.UnfreezCollectionNotifications();

            //Assert.
            Assert.That(collection.CollectionChangedWhileFrozen, $"The property {nameof(ObservableCollectionEx<object>.CollectionChangedWhileFrozen)} did not properly report true after unfreezing.");
        }

        /// <summary>
        /// Tests that the collection changed flag resets to false when event raising is frozen 
        /// again.
        /// </summary>
        [Test]
        public void CollectionChangedFlagResetOnReFreeze()
        {
            //Arrange.
            ObservableCollectionEx<object> collection = new ObservableCollectionEx<object>(false);

            //Act.
            collection.FreezeCollectionNotifications();
            collection.Add(new object());
            collection.UnfreezCollectionNotifications();
            collection.FreezeCollectionNotifications();

            //Assert.
            Assert.That(!collection.CollectionChangedWhileFrozen, $"The property {nameof(ObservableCollectionEx<object>.CollectionChangedWhileFrozen)} was not reset to False on re-freeze.");
        }
        #endregion
    }
}
