using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using SC.ObjectCloner.Interfaces;
using SC.ObjectCloner.Operations;

[assembly: InternalsVisibleTo("SC.ObjectCloner.Tests")]

namespace SC.ObjectCloner
{
    /// <summary>
    /// Clone an existing object instance using rules and reflection
    /// </summary>
    /// <typeparam name="TObj">Type of the object to clone</typeparam>
    public class ObjectCloner<TObj>: IObjectCloner<TObj> where TObj : new()
    {
        private readonly TObj _srcObject;
        private readonly List<IFilteredPerformer> _filteredPerformers = new List<IFilteredPerformer>();
        private readonly HashSet<int> _sourceObjectHashes = new HashSet<int>();

        internal ObjectCloner(TObj srcObject)
        {
            _srcObject = srcObject;
        }

        /// <summary>
        /// Specify a predicate to match a selection of properties which will be processed as part of the clone operation
        /// </summary>
        /// <param name="propertyFilterPredicate">A predicate returning true if the specified ClonePropertyInfo should be processed during the clone operation</param>
        /// <returns>An interface that allows operations to be performed on the selected properties</returns>
        public IClonePropertyOperations<object> SelectProperties(Func<ClonePropertyInfo<object>, bool> propertyFilterPredicate)
        {
            var performer = new PropertyInfoFilterPerformer<object>(propertyFilterPredicate);
            _filteredPerformers.Add(performer);
            return performer;
        }

        /// <summary>
        /// Select a single property to be processed as part of the clone operation
        /// </summary>
        /// <typeparam name="TProperty">Type of the property to be selected</typeparam>
        /// <param name="propertySelector">Function returning the property to process</param>
        /// <returns>An interface that allows operations to be performed on the selected property</returns>
        public IClonePropertyOperations<TProperty> SelectProperty<TProperty>(Expression<Func<TObj, TProperty>> propertySelector)
        {
            var propertyPath = propertySelector.GetPath();
            var performer = new PropertyInfoFilterPerformer<TProperty>(clonePi => clonePi.PropertyPath == propertyPath);
            _filteredPerformers.Add(performer);

            return performer;
        }
   
        /// <summary>
        /// Perform the clone operation
        /// </summary>
        /// <returns>A clone of the source object modified according to the setup</returns>
        public TObj Clone()
        {
            TObj newObj = (TObj)CopyObjectRecursive("", _srcObject);
            return newObj;
        }

        private object CopyObjectRecursive(string currentPropertyPath, object srcObject)
        {
            if (srcObject == null) return null;

            Type srcObjectType = srcObject.GetType();

            //todo - handle specific constructor requirements.
            object newObject = Activator.CreateInstance(srcObjectType);

            if (IsDirectlyAssignable(srcObjectType))
            {
                return srcObject; // for simple types
            }

            PropertyInfo[] srcPoperties = srcObjectType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo currentProperty in srcPoperties.Where(x => x.CanRead && 
                                                                             x.CanWrite &&
                                                                             !x.GetIndexParameters().Any()))
            {
                
                var propName = currentProperty.Name;
                var newPropertyPath = (currentPropertyPath + "." + propName).Trim('.');
                var sourcePi = new ClonePropertyInfo<object>(newPropertyPath, currentProperty);
                ((IValueResolver)sourcePi).ResolvePropertyValue(srcObject);

                int sourceHashCode = sourcePi.PropertyValue.GetHashCode();
                if (_sourceObjectHashes.Contains(sourceHashCode))
                {
                    throw new ObjectCloneException($"Self referential object hierarchy detected - cannot clone this structure. Object Type [{srcObject.GetType().Name}], Object path: [{newPropertyPath}]");
                }
                else
                {
                    _sourceObjectHashes.Add(sourceHashCode);
                }


                var propertyPerformer = _filteredPerformers.FirstOrDefault(pf => pf.PropertyMatchesPredicate(sourcePi));
                if (propertyPerformer != null)
                {
                    propertyPerformer.Perform(newObject, sourcePi);
                }
                else
                {
                    var propValue = sourcePi.PropertyValue;
                    ActionPropertyCopy(newObject, currentProperty, propValue, newPropertyPath);
                }
            }

            return newObject;
        }

        private void ActionPropertyCopy<TCloneType>(TCloneType clonedObject, PropertyInfo currentProperty, object sourcePropertyValue, string newPropertyPath)
        {
            Type propertyType = currentProperty.PropertyType;
            // basic types (string, int etc) get copied directly...
            if (IsDirectlyAssignable(propertyType))
            {
                currentProperty.SetValue(clonedObject, sourcePropertyValue, null);
            }
            else if (IsList(sourcePropertyValue))
            {
                // ...lists get new ones created and their members copied...
                object listClone = CopyList(propertyType, sourcePropertyValue as IList, newPropertyPath);
                currentProperty.SetValue(clonedObject, listClone, null);
            }
            else
            {
                // ...other types - i.e business objects - get a recursive clone made of them                                   
                object deepClone = CopyObjectRecursive(newPropertyPath, sourcePropertyValue);
                currentProperty.SetValue(clonedObject, deepClone, null);
            }
        }

        private object CopyList(Type propertyType, IList srcList, string newPropertyPath)
        {
            IList newObject = null;
            if (propertyType.IsArray)
            {
                Type arrayType = propertyType.GetElementType();
                Array srcArray = (Array)srcList;
                newObject = Array.CreateInstance(arrayType, srcArray.Length);
                for (int elementIndex = 0; elementIndex < srcArray.Length; elementIndex++)
                {
                    object newItem = CopyObjectRecursive(newPropertyPath, srcArray.GetValue(elementIndex));
                    newObject[elementIndex] = newItem;
                }
            }
            else
            {
                newObject = Activator.CreateInstance(srcList.GetType()) as IList;
                foreach (object listItem in srcList)
                {
                    object newListItem = CopyObjectRecursive(newPropertyPath, listItem);
                    newObject.Add(newListItem);
                }
            }

            return newObject;
        }

        private bool IsList(object srcObject)
        {
            return srcObject is IList;
        }

        /// <summary>
        /// Returns true if property can be directly assigned,i.e it's a value type or has some other specific criteria
        /// </summary>
        /// <param name="srcType"></param>
        /// <returns></returns>
        private bool IsDirectlyAssignable(Type srcType)
        {
            return srcType.IsValueType || srcType == typeof(string);
        }
    }
}