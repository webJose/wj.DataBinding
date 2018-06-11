using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wj.DataBinding.NUnitTests
{
    /// <summary>
    /// Test class that defines the tests for the <code>wj.DataBinding.NotifyPropertyChanged</code> 
    /// class.
    /// </summary>
    [TestFixture]
    public class NotifyPropertyChangedTests
    {
        #region Helper Classes

        /// <summary>
        /// Test class with a single property backed up with a backing field of the same data type.
        /// </summary>
        private class NpcClass : NotifyPropertyChanged
        {
            #region Properties

            /// <summary>
            /// Backing field for the <code>Data</code> property.
            /// </summary>
            private object m_data;

            /// <summary>
            /// Gets or sets a <code>Object</code> test value.
            /// </summary>
            public object Data
            {
                get { return m_data; }
                set { SaveAndNotify(ref m_data, value); }
            }
            #endregion

            #region Constructors

            /// <summary>
            /// Creates a new instance of this test class with the specified intial value for the 
            /// test property.
            /// </summary>
            /// <param name="initialPropertyValue">The test property's initial value.</param>
            public NpcClass(object initialPropertyValue)
                : base()
            {
                m_data = initialPropertyValue;
            }
            #endregion
        }

        /// <summary>
        /// Test class with a single property of a non-nullable data type backed up by a nullable 
        /// backing field.
        /// </summary>
        private class NullableNpcClass : NotifyPropertyChanged
        {
            #region Properties

            /// <summary>
            /// Backing field for the <code>Data</code> property.
            /// </summary>
            private DateTime? m_data;

            /// <summary>
            /// Gets or sets a <code>DateTime</code> test value.
            /// </summary>
            public DateTime Data
            {
                get { return m_data.HasValue ? m_data.Value : default(DateTime); }
                set { SaveAndNotify(ref m_data, value); }
            }
            #endregion

            #region Constructors

            /// <summary>
            /// Creates a new instance of this test class using the provided value as the initial 
            /// value of the test property's backing field.
            /// </summary>
            /// <param name="initialStorageValue">Initial value for the test property's backing 
            /// field.</param>
            public NullableNpcClass(DateTime? initialStorageValue)
            {
                m_data = initialStorageValue;
            }
            #endregion
        }

        /// <summary>
        /// Utility class that counts the number of times the <code>PropertyChanged</code> event 
        /// is fired on the given object.
        /// </summary>
        private class PropertyChangedHandler
        {
            #region Properties

            /// <summary>
            /// Gets the number of times the <code>PropertyChanged</code> event was raised during 
            /// the course of the test.
            /// </summary>
            public int EventCount { get; private set; } = 0;
            #endregion

            #region Constructors

            /// <summary>
            /// Creates a new instance of this test class and bounds to the <code>PropertyChanged</code> 
            /// event on the given object.
            /// </summary>
            /// <param name="npc">The object provider of the <code>PropertyChanged</code> event.</param>
            public PropertyChangedHandler(INotifyPropertyChanged npc)
            {
                npc.PropertyChanged += PropertyChanged;
            }
            #endregion

            #region Event Handlers

            /// <summary>
            /// Increases the <code>EventCount</code> property value.
            /// </summary>
            /// <param name="sender">The originator of the <code>PropertyChanged</code> event.</param>
            /// <param name="e">The event data.</param>
            private void PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                ++EventCount;
            }
            #endregion
        }
        #endregion

        #region Test Data

        /// <summary>
        /// Gets an enumerable object that returns the test data for the standard version of the 
        /// <code>wj.DataBinding.NotifyPropertyChanged.SaveAndNotify</code> function.
        /// </summary>
        private static IEnumerable TestData
        {
            get
            {
                yield return new TestCaseData(null, new object())
                    .Returns(1)
                    .SetName("Different Value Assignment");
                object value = new object();
                yield return new TestCaseData(value, value)
                    .Returns(0)
                    .SetName("Same Value Assignment");
            }
        }

        /// <summary>
        /// Gets an enumerable object that returns the test data for the version of the 
        /// <code>wj.DataBinding.NotifyPropertyChanged.SaveAndNotify</code> function used for 
        /// properties with a nullable backing field and non-nullable return types.
        /// </summary>
        private static IEnumerable NullableBackingFieldTestData
        {
            get
            {
                yield return new TestCaseData(null, DateTime.Now)
                    .Returns(1)
                    .SetName("Null to Non-Null Value");
                yield return new TestCaseData(DateTime.MinValue, DateTime.Now)
                    .Returns(1)
                    .SetName("Non-Null to Different Non-Null");
                yield return new TestCaseData(DateTime.MaxValue, DateTime.MaxValue)
                    .Returns(0)
                    .SetName("Non-Null to Same Non-Null");
                yield return new TestCaseData(null, default(DateTime))
                    .Returns(1)
                    .SetName("Null to Non-Null Default Value");
            }
        }
        #endregion

        #region Tests

        /// <summary>
        /// Tests the overload of <code>wj.DataBinding.NotifyPropertyChanged.SaveAndNotify</code> 
        /// considered standard where both the backing field and the property return value are of 
        /// the same data type.
        /// </summary>
        /// <param name="initialValue">The intitial value of the test property.</param>
        /// <param name="value">The value to assign while listening to the 
        /// <code>PropertyChanged</code> event.</param>
        /// <returns>The number of times the <code>PropertyChanged</code> event was raised.</returns>
        [Test]
        [TestCaseSource(nameof(TestData))]
        public int MatchingBackingFieldAndPropertyReturnType(object initialValue, object value)
        {
            //Arrange.
            NpcClass npc = new NpcClass(initialValue);
            PropertyChangedHandler handler = new PropertyChangedHandler(npc);

            //Act.
            npc.Data = value;

            //Assert.
            return handler.EventCount;
        }

        /// <summary>
        /// Tests the overload of <code>wj.DataBinding.NotifyPropertyChanged.SaveAndNotify</code> 
        /// used with properties with nullable backing fields and non-nullable return types.
        /// </summary>
        /// <param name="initialValue">The intitial value of the test property.</param>
        /// <param name="value">The value to assign while listening to the 
        /// <code>PropertyChanged</code> event.</param>
        /// <returns>The number of times the <code>PropertyChanged</code> event was raised.</returns>
        [Test]
        [TestCaseSource(nameof(NullableBackingFieldTestData))]
        public int SaveNonNullableInNullable(DateTime? initialValue, DateTime value)
        {
            //Arrange.
            NullableNpcClass npc = new NullableNpcClass(initialValue);
            PropertyChangedHandler handler = new PropertyChangedHandler(npc);

            //Act.
            npc.Data = value;

            //Assert.
            return handler.EventCount;
        }
        #endregion
    }
}
