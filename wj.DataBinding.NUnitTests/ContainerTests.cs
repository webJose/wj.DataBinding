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
    [TestFixture]
    public class ContainerTests
    {
        public class TestClass : NotifyPropertyChanged
        {
            #region Properties
            private long m_id;
            public long Id
            {
                get { return m_id; }
                set { SaveAndNotify(ref m_id, value); }
            }

            private string m_name;
            public string Name
            {
                get { return m_name; }
                set { SaveAndNotify(ref m_name, value); }
            }
            #endregion
        }

        private class DerivedContainer : Container<TestClass>
        {
            #region Properties
            private bool m_derProperty;
            public bool DerProperty
            {
                get { return m_derProperty; }
                set { SaveAndNotify(ref m_derProperty, value); }
            }
            #endregion

            #region Constructors
            public DerivedContainer(TestClass data)
                : base(data)
            { }
            #endregion
        }

        #region Test Data
        private static IEnumerable PropertyNameAndPdTypes
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

        [Test]
        [TestCaseSource(nameof(PropertyNameAndPdTypes))]
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
