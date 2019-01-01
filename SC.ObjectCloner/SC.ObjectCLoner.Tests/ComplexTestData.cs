using System;
using System.Collections;
using System.Collections.Generic;

namespace SC.ObjectCLoner.Tests
{
         
    public class ClonerTestBase
    {
        private static int _nextId = -1;

        public int Id { get; set; }

        protected int GetNextId()
        {
            return _nextId--;
        }

        public ClonerTestBase()
        {
            Id = GetNextId();
        }
    }

    public class Generic<T>: ClonerTestBase
    {
        public T TypedProperty { get; set; }
    }

    public class Person: ClonerTestBase
    {        
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public byte[] Photo { get; set; }        
        public Address CurrentAddress { get; set; }
    }

    public class Address: ClonerTestBase
    {
        public string Line1 { get; set; }
    }

    public class ParentedCollectionContainer: ClonerTestBase
    {
        public IList<ParentedCollectionMember> Collection { get; set; }
    }

    public class ParentedCollectionMember: ClonerTestBase
    {
        public ParentedCollectionContainer ParentContainer { get; set; }

        public string StringValue { get; set; }

    }

}