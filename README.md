# Introduction 
Object cloning is deceptively complicated. Do you deep copy everything or make references to existing objects? What do you do about IDs? How do you handle self-referential structures?

The C# [ICloneable](https://docs.microsoft.com/en-us/dotnet/api/system.icloneable?view=netframework-4.7.2) interface is woefully inadequate. This project attempts to do better. (Also, I wanted to write a fluent interface and it seemed like a good excuse). 

# Installation

Grab the code, build the project and reference it. When I get around to it, there'll be a Nuget. Probably. 

# Usage

### Simple deep clone

    var sourceObject = new Person(){...}
    var cloner = ObjectCloneFactory.CreateCloner(sourceObject);
    var clonedObject = cloner.Clone();
	
### Skip cloning Id properties
A common pattern is to assign Id properties during object creation. Usually you'd want copies of these objects to have new IDs, not copies of the original ones. This will use the default value for any property called *"Id"* wherever it occurs in the object structure:

    var sourceObject = new Person(){...}
    var cloner = ObjectCloneFactory.CreateCloner(sourceObject);
    cloner.SelectProperties(p => p.Info.Name == "Id").KeepDefaultValue();
    var clonedObject = cloner.Clone();


### Multiple operations

Clone a `Person` object and...
- Use the default (newly created) Id
- Prepend *"Copy of "* to the value of the property called *"Name"*
- Use the existing object reference of the *Address* property from the source, instead of cloning a new identical address

        var cloner = ObjectCloneFactory.CreateCloner(sourceObject);
        cloner.SelectProperty(c => c.Id).KeepDefaultValue();
        cloner.SelectProperty(c => c.Name).Amend(srcVal => "Copy of "+srcVal.PropertyValue);
        cloner.SelectProperty(c => c.CurrentAddress).UseExistingReference();
        var clonedObject = cloner.Clone();

# Known Issues
- Self referential structures (lists containing items that reference the parent list) are not copied. They are detected and an exception is thrown which is (slightly) better than letting it recurse into a stack overflow.
- Objects to be cloned need a default constructor. Anything that needs constructor parameters can't be cloned.

# Performance
Let's just say performance was not one of the original design goals. Apart from the reflection overhead it also creates a bunch of temporary objects that need to be cleaned up which is fine for small objects but quite nasty if you're cloning large lists of complex objects. There's almost certainly room for improvement there. 
