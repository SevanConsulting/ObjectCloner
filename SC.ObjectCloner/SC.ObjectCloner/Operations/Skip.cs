using SC.ObjectCloner.Interfaces;

namespace SC.ObjectCloner.Operations
{
    /// <summary>
    /// Skip operation
    /// </summary>    
    internal class Skip: IPerformer
    {        
        /// <summary>
        /// Perform an operation on a property against the target object
        /// </summary>
        /// <typeparam name="TObj">Type of the operation to perform the operation against</typeparam>
        /// <param name="targetObject">Target object</param>
        /// <param name="sourcePropertyInfo">Source property to perform the operation against</param>
        public void Perform<TObj>(TObj targetObject, ClonePropertyInfo<object> sourcePropertyInfo)
        {
            // do nothing
        }
    }
}