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
    /// Test class that defines the tests for the <code>wj.DataBinding.Container</code> class.
    /// </summary>
    [TestFixture]
    public class ContainerTests
    {
        #region Helper Classes

        /// <summary>
        /// Test class used to provide an object to contain in a class that derives from the 
        /// <code>wj.DataBinding.Container</code> class.
        /// </summary>
        public class TestClass : NotifyPropertyChanged
        {
            #region Properties

            /// <summary>
            /// Backing field for the <code>Id</code> property.
            /// </summary>
            private long m_id;

            /// <summary>
            /// Gets or sets a <code>long</code> test value.
            /// </summary>
            public long Id
            {
                get { return m_id; }
                set { SaveAndNotify(ref m_id, value); }
            }

            /// <summary>
            /// Backing field for the <code>Name</code> property.
            /// </summary>
            private string m_name;

            /// <summary>
            /// Gets or sets a <code>string</code> test value.
            /// </summary>
            public string Name
            {
                get { return m_name; }
                set { SaveAndNotify(ref m_name, value); }
            }
            #endregion
        }

        /// <summary>
        /// Test class that inherits from <code>wj.DataBinding.Container</code> and adds a property 
        /// to the contained object.
        /// </summary>
        private class DerivedContainer : Container<TestClass>
        {
            #region Properties

            /// <summary>
            /// Backing field for the <code>DerProperty</code> property.
            /// </summary>
            private bool m_derProperty;

            /// <summary>
            /// Gets or sets a <code>bool</code> test value.
            /// </summary>
            public bool DerProperty
            {
                get { return m_derProperty; }
                set { SaveAndNotify(ref m_derProperty, value); }
            }
            #endregion

            #region Constructors

            /// <summary>
            /// Creates a new instance of the test class using the provided data object as the 
            /// object provider of properties.
            /// </summary>
            /// <param name="data">The contained object meant to provide properties for data 
            /// binding.</param>
            public DerivedContainer(TestClass data)
                : base(data)
            { }
            #endregion
        }
        #endregion

        #region Test Data

        /// <summary>
        /// Gets the test data used to check correct property owner.
        /// </summary>
        private static IEnumerable PropertyOwnerTestData
        {
            get
            {
                TestClass data = new TestClass();
                Container<TestClass> containedData = new Container<TestClass>(data);
                DerivedContainer derived = new DerivedContainer(data);
                yield return new TestCaseData(containedData, nameof(TestClass.Id))
                    .Returns(data).SetName($"Owner of Property {nameof(TestClass.Id)} is contained data");
                yield return new TestCaseData(containedData, nameof(TestClass.Name))
                    .Returns(data).SetName($"Owner of Property {nameof(TestClass.Name)} is contained data.");
                yield return new TestCaseData(containedData, nameof(Container<TestClass>.DataObject))
                    .Returns(containedData).SetName($"Owner of Property {nameof(Container<TestClass>.DataObject)} is container.");
                yield return new TestCaseData(derived, nameof(DerivedContainer.DerProperty))
                    .Returns(derived).SetName($"Owner of Property {nameof(DerivedContainer.DerProperty)} is derived container.");
            }
        }
        #endregion

        #region Tests

        /// <summary>
        /// Tests that no extra properties are added.
        /// </summary>
        [Test]
        public void NoExtraPropertiesAdded()
        {
            //Arrange.
            TestClass data = new TestClass();
            Container<TestClass> containedData = new Container<TestClass>(data);

            //Act.
            Attribute[] atts = new Attribute[] { new BrowsableAttribute(true) };
            PropertyDescriptorCollection dataProps = TypeDescriptor.GetProperties(data, atts);
            PropertyDescriptorCollection containedProps = TypeDescriptor.GetProperties(containedData, atts);

            //Assert.
            string reason = null;
            Assert.That(() =>
            {
                bool equal = dataProps.Count == containedProps.Count;
                if (equal)
                {
                    IEqualityComparer<PropertyDescriptor> eqComparer = new PropertyDescriptorEqComparer();
                    foreach (PropertyDescriptor pd in dataProps)
                    {
                        if (!containedProps.OfType<PropertyDescriptor>().Contains(pd, eqComparer))
                        {
                            equal = false;
                            reason = $"The data object provides a property descriptor for a property named '{pd.Name}' but a corresponding one was not found in the data container.";
                            break;
                        }
                    }
                }
                else
                {
                    reason = $"The data property count is {dataProps.Count} and differs from the container property count, {containedProps.Count}.";
                }
                return equal;
            }, reason);
        }

        /// <summary>
        /// Tests that <code>PropertyChanged</code> events from the data object are raised by the 
        /// container.
        /// </summary>
        [Test]
        public void DataPropertyChangedIsRaisedOnContainer()
        {
            //Arrange.
            TestClass data = new TestClass();
            Container<TestClass> containedData = new Container<TestClass>(data);
            int eventCount = 0;
            containedData.PropertyChanged += (o, e) => ++eventCount;

            //Act.
            data.Name = nameof(DataPropertyChangedIsRaisedOnContainer);

            //Assert.
            Assert.That(eventCount == 1, $"It was expected that the PropertyChanged event be raised once, but the count was {eventCount}.");
        }

        /// <summary>
        /// Tests that the implicit cast operator works and returns the same object given in the 
        /// constructor of the <code>wj.DataBinding.Container</code> class.
        /// </summary>
        [Test]
        public void ImplicitCastOperator()
        {
            //Arrange.
            TestClass data = new TestClass();
            Container<TestClass> containedData = new Container<TestClass>(data);

            //Act.
            TestClass castData = containedData;

            //Assert.
            Assert.That(Object.ReferenceEquals(data, castData), "The assigned data object and the cast data object are not the same.");
        }
        #endregion

        /// <summary>
        /// Tests that the correct property owner is returned every time.
        /// </summary>
        /// <param name="containedData">The object provider of property descriptors.</param>
        /// <param name="propertyName">The property name to test ownership on.</param>
        /// <returns>The object owner of the property.</returns>
        [Test]
        [TestCaseSource(nameof(PropertyOwnerTestData))]
        public object CorrectPropertyOwner(Container<TestClass> containedData, string propertyName)
        {
            //Act.
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(containedData);
            ICustomTypeDescriptor ctd = containedData as ICustomTypeDescriptor;

            //Assert.
            return ctd.GetPropertyOwner(props[propertyName]);
        }
    }
}
