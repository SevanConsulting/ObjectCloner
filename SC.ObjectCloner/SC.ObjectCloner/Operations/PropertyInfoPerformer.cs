using System;
using SC.ObjectCloner.Interfaces;

namespace SC.ObjectCloner.Operations
{
    /// <summary>
    /// Perform an operation against a property, where the property matches a predicate
    /// </summary>
    internal class PropertyInfoFilterPerformer<TProperty>: ClonePropertyOperationBase<TProperty>, IFilteredPerformer
    {
        private readonly Func<ClonePropertyInfo<object>, bool> _propertyFilterPredicate;

        /// <summary>
        /// Construct a new PropertyInfoFilterPerformer
        /// </summary>
        /// <param name="propertyFilterPredicate">A predicate returning true if the operation should be performed</param>
        internal PropertyInfoFilterPerformer(Func<ClonePropertyInfo<object>, bool> propertyFilterPredicate)
        {
            _propertyFilterPredicate = propertyFilterPredicate;
        }

        /// <summary>
        /// Checks the property info against a predicate function
        /// </summary>
        /// <param name="sourcePropertyInfo">Property info to test the predicate function against</param>
        /// <returns>True if the specified property info matches the predicate</returns>        
        public bool PropertyMatchesPredicate(ClonePropertyInfo<object> sourcePropertyInfo)
        {
            return _propertyFilterPredicate.Invoke(sourcePropertyInfo);
        }
    }

    /// <summary>
    /// Perform an operation against a specified property
    /// </summary>
    internal class PropertyInfoPerformer<TProp>: ClonePropertyOperationBase<TProp>
    {
        /// <summary>
        /// Information about the selected property
        /// </summary>
        public ClonePropertyInfo<TProp> PropertyDescriptor { get; }

        /// <summary>
        /// Construct a new PropertyInfoPerformer
        /// </summary>
        /// <param name="propertyInfo">Property to perform the operation against</param>
        public PropertyInfoPerformer(ClonePropertyInfo<TProp> propertyInfo)
        {
            PropertyDescriptor = propertyInfo;
        }
    }
}