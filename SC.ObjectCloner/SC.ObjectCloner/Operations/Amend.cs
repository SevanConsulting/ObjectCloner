using System;
using SC.ObjectCloner.Interfaces;

namespace SC.ObjectCloner.Operations
{
    /// <summary>
    /// Amend a property on the target object using a new value factory
    /// </summary>
    internal class Amend<TProperty>: IPerformer
    {
        private readonly Func<ClonePropertyInfo<object>, TProperty> _newValueFactory;

        public Amend(Func<ClonePropertyInfo<object>, TProperty> newValueFactory)
        {
            _newValueFactory = newValueFactory;
        }

        /// <summary>
        /// Perform an operation on a property against the target object
        /// </summary>
        /// <typeparam name="TObj">Type of the operation to perform the operation against</typeparam>
        /// <param name="targetObject">Target object</param>
        /// <param name="sourcePropertyInfo">Source property to perform the operation against</param>
        public void Perform<TObj>(TObj targetObject, ClonePropertyInfo<object> sourcePropertyInfo)
        {
            var newValue = _newValueFactory.Invoke(sourcePropertyInfo);
            sourcePropertyInfo.Info.SetValue(targetObject, newValue);
        }
    }
}