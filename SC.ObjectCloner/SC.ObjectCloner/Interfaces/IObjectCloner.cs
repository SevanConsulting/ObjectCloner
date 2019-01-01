using System;
using System.Linq.Expressions;

namespace SC.ObjectCloner.Interfaces
{
    /// <summary>
    /// Interface describing an object cloner
    /// </summary>
    /// <typeparam name="TObj">Type of the object to clone</typeparam>
    public interface IObjectCloner<TObj>
    {
        /// <summary>
        /// Select a single property to be processed as part of the clone operation
        /// </summary>
        /// <typeparam name="TProperty">Type of the property to be selected</typeparam>
        /// <param name="propertySelector">Function returning the property to process</param>
        /// <returns>An interface that allows operations to be performed on the selected property</returns>
        IClonePropertyOperations<TProperty> SelectProperty<TProperty>(Expression<Func<TObj, TProperty>> propertySelector);

        /// <summary>
        /// Specify a predicate to match a selection of properties which will be processed as part of the clone operation
        /// </summary>
        /// <param name="propertyFilterPredicate">A predicate returning true if the specified ClonePropertyInfo should be processed during the clone operation</param>
        /// <returns>An interface that allows operations to be performed on the selected properties</returns>
        IClonePropertyOperations<object> SelectProperties(Func<ClonePropertyInfo<object>, bool> propertyFilterPredicate);

        /// <summary>
        /// Perform the clone operation
        /// </summary>
        /// <returns>A clone of the source object modified according to the setup</returns>
        TObj Clone();
    }
}