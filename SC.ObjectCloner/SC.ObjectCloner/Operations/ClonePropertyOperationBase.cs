using System;
using SC.ObjectCloner.Interfaces;

namespace SC.ObjectCloner.Operations
{
    /// <summary>
    /// Handle performing operations on a property
    /// </summary>
    internal abstract class ClonePropertyOperationBase<TProperty>: IClonePropertyOperations<TProperty>, IPerformer, ISetPerformer
    {
        /// <summary>
        /// The operation to perform
        /// </summary>
        protected IPerformer Performer;

        /// <summary>
        /// Perform an operation on a property against the target object
        /// </summary>
        /// <typeparam name="TObj">Type of the operation to perform the operation against</typeparam>
        /// <param name="targetObject">Target object</param>
        /// <param name="sourcePropertyInfo">Source property to perform the operation against</param>
        public void Perform<TObj>(TObj targetObject, ClonePropertyInfo<object> sourcePropertyInfo)
        {
            if (Performer == null)
            {
                throw new InvalidOperationException(
                    $"No action specified to be taken on property {sourcePropertyInfo.PropertyPath}. Choose KeepDefaultValue() to skip cloning the property.");
            }
            Performer?.Perform(targetObject, sourcePropertyInfo);
        }

        /// <summary>
        /// Attach an operation to the property info
        /// </summary>
        /// <param name="performer">Operation to perform</param>
        void ISetPerformer.SetOperation(IPerformer performer)
        {
            Performer = performer;
        }
    }
}