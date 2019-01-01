using Microsoft.VisualStudio.TestTools.UnitTesting;
using SC.ObjectCloner;
using SC.ObjectCloner.Interfaces;
using SC.ObjectCloner.Operations;
using System;

namespace SC.ObjectCLoner.Tests
{
    [TestClass]
    public class ClonerOperations
    {
        [TestMethod]
        public void CloneOperation_Skip_DoesNothing()
        {
            var source = CreateTestCloneObject();            
            var target = new Person();

            var operation = new Skip();

            var info = ExpressionHelpers.GetPropertyInfo<Person, string>(x => x.Name);
            var srcPi = new ClonePropertyInfo<object>("", info);
            ((IValueResolver)srcPi).ResolvePropertyValue(source);

            operation.Perform(target, srcPi);
            Assert.IsNull(target.Name);
        }

        [TestMethod]
        public void CloneOperation_UseExisting_UsesExistingObject()
        {
            var source = CreateTestCloneObject();
            var target = new Person();

            var operation = new UseExistingReference();
            var info = ExpressionHelpers.GetPropertyInfo<Person, Address>(x => x.CurrentAddress);
            var srcPi = new ClonePropertyInfo<object>("", info);
            ((IValueResolver)srcPi).ResolvePropertyValue(source);

            operation.Perform(target, srcPi);
            Assert.AreSame(source.CurrentAddress, target.CurrentAddress);
        }

        [TestMethod]
        public void CloneOperation_AmendString_AmendsValue()
        {
            var source = CreateTestCloneObject();
            var target = new Person();

            var info = ExpressionHelpers.GetPropertyInfo<Person, string>(x => x.Name);
            var srcPi = new ClonePropertyInfo<object>("", info);
            ((IValueResolver)srcPi).ResolvePropertyValue(source);

            var operation = new Amend<string>(s => s.PropertyValue+"_new");
            operation.Perform(target, srcPi);
            Assert.AreEqual(source.Name+"_new", target.Name);
        }

        [TestMethod]
        public void CloneOperation_AmendStruct_AmendsValue()
        {
            var source = CreateTestCloneObject();
            var target = new Person();

            var info = ExpressionHelpers.GetPropertyInfo<Person, DateTime>(x => x.DateOfBirth);
            var srcPi = new ClonePropertyInfo<object>("", info);
            ((IValueResolver)srcPi).ResolvePropertyValue(source);

            var operation = new Amend<object>(currentVal => ((DateTime)currentVal.PropertyValue).AddDays(1));
            operation.Perform(target, srcPi);
            Assert.AreEqual(source.DateOfBirth.AddDays(1), target.DateOfBirth);
        }

        public static Person CreateTestCloneObject()
        {
            var person = new Person()
            {
                Id = 1,
                DateOfBirth = new DateTime(2001, 12, 24),
                Name = "Test1",
                Photo = new byte[1] { 1 },
                CurrentAddress = new Address()
                {
                    Id = 2,
                    Line1 = "Somewhere"
                }
            };

            return person;
        }

    }
}