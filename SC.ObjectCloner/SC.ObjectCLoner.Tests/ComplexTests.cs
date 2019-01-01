using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SC.ObjectCloner;

namespace SC.ObjectCLoner.Tests
{
    [TestClass]
    public class ComplexTests
    {
        [TestMethod]
        public void GenericObject_NoOptions_ObjectIsCloned()
        {
            var source = new Generic<string>()
            {
                TypedProperty = "Hello"
            };

            var cloner = ObjectCloneFactory.CreateCloner(source);
            var target = cloner.Clone();
            
            Assert.AreNotEqual(source, target);
            Assert.AreEqual("Hello", target.TypedProperty);
        }


        [TestMethod]
        public void ComplexObject_NoOptions_ObjectIsDeepCloned()
        {
            var sourceObject = new Person()
            {
                DateOfBirth = new DateTime(2001, 12, 24),
                Name = "Test1",
                Photo = new byte[1] {1},
                CurrentAddress = new Address()
                {
                    Line1 = "Somewhere"
                }
            };

            var cloner = ObjectCloneFactory.CreateCloner(sourceObject);
            var clonedObject = cloner.Clone();

            Assert.AreNotSame(sourceObject, clonedObject);
            Assert.AreNotSame(sourceObject.CurrentAddress, clonedObject.CurrentAddress);

            Assert.AreEqual(sourceObject.CurrentAddress.Id, clonedObject.CurrentAddress.Id);
            Assert.AreEqual(sourceObject.Id, clonedObject.Id);
            Assert.AreEqual(clonedObject.Name, sourceObject.Name);
            Assert.AreEqual(clonedObject.CurrentAddress.Line1, sourceObject.CurrentAddress.Line1);
            Assert.AreEqual(clonedObject.DateOfBirth, sourceObject.DateOfBirth);
            CollectionAssert.AreEqual(clonedObject.Photo, sourceObject.Photo);
        }

        [TestMethod]
        public void ComplexObject_SkipSourceId_SourceIdRetainsDefaultValue()
        {
            var sourceObject = new Person()
            {
                DateOfBirth = new DateTime(2001, 12, 24),
                Name = "Test1",
                Photo = new byte[1] {1},
                CurrentAddress = new Address()
                {
                    Line1 = "Somewhere"
                }
            };

        var cloner = ObjectCloneFactory.CreateCloner(sourceObject);
        cloner.SelectProperty(c => c.Id).KeepDefaultValue();
        var clonedObject = cloner.Clone();

        Assert.AreNotEqual(sourceObject.Id, clonedObject);        

        // Address Id was not skipped, it should be cloned
        Assert.AreEqual(sourceObject.CurrentAddress.Id, clonedObject.CurrentAddress.Id);        
        }

        [TestMethod]
        public void ComplexObject_SkipAllNamedId_AllIdPropertiesRetainDefaultValues()
        {
            var sourceObject = new Person()
            {
                DateOfBirth = new DateTime(2001, 12, 24),
                Name = "Test1",
                Photo = new byte[1] { 1 },
                CurrentAddress = new Address()
                {
                    Line1 = "Somewhere"
                }
            };

            var cloner = ObjectCloneFactory.CreateCloner(sourceObject);
            cloner.SelectProperties(p => p.Info.Name == "Id").KeepDefaultValue();
            var clonedObject = cloner.Clone();

            Assert.AreNotEqual(sourceObject.Id, clonedObject);            
            Assert.AreNotEqual(sourceObject.CurrentAddress.Id, clonedObject.CurrentAddress.Id);
        }

        [TestMethod]
        public void ComplexObject_UseExistingAddressReference_ClonePointsToExistingReference()
        {
            var address = new Address()
            {
                Line1="Test Address"
            };

            var sourceObject = new Person()
            {
                DateOfBirth = new DateTime(2001, 12, 24),
                Name = "Test1",
                Photo = new byte[1] { 1 },
                CurrentAddress = address
            };

            var cloner = ObjectCloneFactory.CreateCloner(sourceObject);
            cloner.SelectProperty(c => c.CurrentAddress).UseExistingReference();
            var clonedObject = cloner.Clone();

            Assert.AreSame(sourceObject.CurrentAddress, clonedObject.CurrentAddress);            
        }


        [TestMethod]
        public void ComplexObject_AmendName_CloneNameIsAmended()
        {
            var sourceObject = new Person()
            {
                DateOfBirth = new DateTime(2001, 12, 24),
                Name = "Test1",
                Photo = new byte[1] { 1 },
                CurrentAddress = new Address()
                {
                    Line1 = "Somewhere"
                }
            };

            var cloner = ObjectCloneFactory.CreateCloner(sourceObject);
            cloner.SelectProperty(c => c.Name).Amend(srcVal => "Copy of "+srcVal.PropertyValue);
            var clonedObject = cloner.Clone();

            Assert.AreEqual("Copy of Test1", clonedObject.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectCloneException))]
        public void SelfReferentialStructure_BasicClone_DetectsAndThrowsException()
        {
            var parent = new ParentedCollectionContainer();
            var srcList = new List<ParentedCollectionMember>();
            parent.Collection = srcList;

            srcList.Add(new ParentedCollectionMember() {StringValue = "One", ParentContainer = parent});
            srcList.Add(new ParentedCollectionMember() {StringValue = "Two", ParentContainer = parent});                        

            var cloner = ObjectCloneFactory.CreateCloner(parent);

            var clone = cloner.Clone();
            
        }
    }

}