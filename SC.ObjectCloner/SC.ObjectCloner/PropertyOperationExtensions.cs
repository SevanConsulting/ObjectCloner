using System;
using SC.ObjectCloner.Interfaces;
using SC.ObjectCloner.Operations;

namespace SC.ObjectCloner
{
    /// <summary>
    /// Extension operations for processing properties
    /// </summary>
    public static class PropertyOperationExtensions
    {
        /// <summary>
        /// Amend the value of the cloned property
        /// </summary>
        /// <typeparam name="TProp">Type of the property to operate on</typeparam>
        /// <param name="propertyInfo">Information about the source property</param>
        /// <param name="newValueFactory">A function to provide a new value for the property, given the existing value as a parameter</param>
        public static void Amend<TProp>(this IClonePropertyOperations<TProp> propertyInfo, Func<ClonePropertyInfo<object>, TProp> newValueFactory)
        {
            var operation = new Amend<TProp>(newValueFactory);
            ((ISetPerformer)propertyInfo).SetOperation(operation);
        }

        /// <summary>
        /// Set the value of the cloned property to the existing object referenced by the source property
        /// </summary>
        /// <typeparam name="TProp">Type of the property to operate on</typeparam>
        /// <param name="propertyInfo">Information about the source property  </param>
        public static void UseExistingReference<TProp>(this IClonePropertyOperations<TProp> propertyInfo) where TProp : class
        {
            var operation = new UseExistingReference();
            ((ISetPerformer)propertyInfo).SetOperation(operation);
        }

        /// <summary>
        /// Leave the value of the cloned property as the default value after construction
        /// </summary>
        /// <typeparam name="TProp">Type of the property to operate on</typeparam>
        /// <param name="propertyInfo">Information about the source property  </param>
        public static void KeepDefaultValue<TProp>(this IClonePropertyOperations<TProp> propertyInfo)
        {
            var operation = new Skip();
            ((ISetPerformer)propertyInfo).SetOperation(operation);
        }
    }
}