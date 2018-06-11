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
    /// <summary>
    /// Test class that defines the tests for the <code>wj.DataBinding.ObservableCollectionEx</code> 
    /// class.
    /// </summary>
    [TestFixture]
    public class ObservableCollectionExTests
    {
        #region Helper Classes

        /// <summary>
        /// Helper class used to count the number of times the <code>CollectionChanged</code> 
        /// event is raised.
        /// </summary>
        private class CollectionChangedHandler
        {
            #region Properties

            /// <summary>
            /// Gets the number of times the <code>CollectionChanged</code> event was raised 
            /// during the course of testing.
            /// </summary>
            public int EventCount { get; private set; } = 0;
            #endregion

            #region Constructors

            /// <summary>
            /// Creates a new intance of this test class and binds to the <code>CollectionChanged</code> 
            /// event of the provided collection.
            /// </summary>
            /// <param name="collection">Collection object to monitor.</param>
            public CollectionChangedHandler(INotifyCollectionChanged collection)
            {
                collection.CollectionChanged += CollectionChangedFn;
            }
            #endregion

            #region Event Handlers

            /// <summary>
            /// Increases the <code>EventCount</code> property value.
            /// </summary>
            /// <param name="sender">Originator of the event.</param>
            /// <param name="e">Event data.</param>
            private void CollectionChangedFn(object sender, NotifyCollectionChangedEventArgs e)
            {
                ++EventCount;
            }
            #endregion
        }
        #endregion

        #region Methods

        /// <summary>
        /// Creates a new <code>CollectionChangedHandler</code> object using the provided 
        /// collection as source of events.
        /// </summary>
        /// <param name="collection">Collection to monitor.</param>
        /// <returns>A new <code>CollectionChangedHandler</code> object arranged to monitor the 
        /// <code>CollectionChanged</code> event of the provided collection.</returns>
        private CollectionChangedHandler SetupNotifyCollectionChangedHandler(INotifyCollectionChanged collection)
        {
            return new CollectionChangedHandler(collection);
        }
        #endregion

        #region Tests

        /// <summary>
        /// Tests basic functionality:  CollectionChanged must not be raised while notification is 
        /// frozen.
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
            collection.UnfreezeCollectionNotifications();

            //Assert.
            Assert.That(handler.EventCount == 1, $"CollectionChanged event was raised {handler.EventCount} time(s) when the count was expected to be 1.");
        }

        /// <summary>
        /// Tests that unfreezing the notification of change too much throws an exception.
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
                    collection.UnfreezeCollectionNotifications();
                }
            });
            Assert.That(unfreezeCount - freezeCount == 1, $"Unfreezing did throw as expected, but did not do it at the expected iteration ({unfreezeCount}).");
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
            collection.UnfreezeCollectionNotifications();

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
            collection.UnfreezeCollectionNotifications();

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
            collection.UnfreezeCollectionNotifications();
            collection.FreezeCollectionNotifications();

            //Assert.
            Assert.That(!collection.CollectionChangedWhileFrozen, $"The property {nameof(ObservableCollectionEx<object>.CollectionChangedWhileFrozen)} was not reset to False on re-freeze.");
        }
        #endregion
    }
}
