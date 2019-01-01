using System;
using System.Drawing;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SC.ObjectCloner;

namespace SC.ObjectCLoner.Tests
{
    [TestClass]
    public class BasicTests
    {
        [TestMethod]
        public void BasicClone_SimplePropertiesAreCopied()
        {
            var test = new SimpleTestData()
            {
                StringProp = "One",
                BoolProp = true,
                EnumProp = SimpleEnum.Red,
                FloatProp = 1.5,
                IntProp = 2,
                StructProp = new Point(2, 3)
            };
            
            var oc = ObjectCloneFactory.CreateCloner(test);
            var newObj = oc.Clone();

            Assert.AreNotSame(oc, newObj);
            Assert.AreEqual(test.StringProp, newObj.StringProp);
            Assert.AreEqual(test.BoolProp, newObj.BoolProp);
            Assert.AreEqual(test.EnumProp, newObj.EnumProp);
            Assert.AreEqual(test.FloatProp, newObj.FloatProp);
            Assert.AreEqual(test.IntProp, newObj.IntProp);
            Assert.AreEqual(test.StructProp, newObj.StructProp);
        }     
    }
}
