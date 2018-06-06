using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wj.DataBinding.NUnitTests
{
    [TestFixture]
    public class NotifyPropertyChangedTests
    {
        private class NpcClass : NotifyPropertyChanged
        {
            #region Properties
            private object m_data;
            public object Data
            {
                get { return m_data; }
                set { SaveAndNotify(ref m_data, value); }
            }
            #endregion
        }

        private class PropertyChangedHandler
        {
            #region Properties
            public int EventCount { get; private set; }
            #endregion

            #region Constructors
            public PropertyChangedHandler(INotifyPropertyChanged npc)
            {
                npc.PropertyChanged += PropertyChanged;
            }
            #endregion

            #region Event Handlers
            private void PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                ++EventCount;
            }
            #endregion
        }

        #region Tests

        /// <summary>
        /// Tests that a property change raises the <code>PropertyChanged</code> event.
        /// </summary>
        [Test]
        public void SaveAndNotifyRaisesEvent()
        {
            //Arrange.
            NpcClass npc = new NpcClass();
            PropertyChangedHandler handler = new PropertyChangedHandler(npc);

            //Act.
            npc.Data = new object();

            //Assert.
            Assert.That(handler.EventCount == 1, $"The {nameof(INotifyPropertyChanged.PropertyChanged)} event was meant to fire only once.  Fire count:  {handler.EventCount}.");
        }

        /// <summary>
        /// Tests that assigning the same value to a property does not raise the 
        /// <code>PropertyChanged</code> event.
        /// </summary>
        [Test]
        public void SaveAndNotifyDoesNotRaiseOnEqualValue()
        {
            //Arrange.
            NpcClass npc = new NpcClass();
            object data = new object();
            npc.Data = data;
            PropertyChangedHandler handler = new PropertyChangedHandler(npc);

            //Act.
            npc.Data = data;

            //Assert.
            Assert.That(handler.EventCount == 0, $"The {nameof(INotifyPropertyChanged.PropertyChanged)} event fired on value re-assignment.");
        }
        #endregion
    }
}
