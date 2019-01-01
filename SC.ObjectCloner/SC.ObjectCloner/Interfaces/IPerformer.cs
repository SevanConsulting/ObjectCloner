namespace SC.ObjectCloner.Interfaces
{
    /// <summary>
    /// Interface describing a class that can perform a property operation
    /// </summary>
    internal interface IPerformer
    {
        /// <summary>
        /// Perform an operation on a property against the target object
        /// </summary>
        /// <typeparam name="TObj">Type of the operation to perform the operation against</typeparam>
        /// <param name="targetObject">Target object</param>
        /// <param name="sourcePropertyInfo">Source property to perform the operation against</param>
        void Perform<TObj>(TObj targetObject, ClonePropertyInfo<object> sourcePropertyInfo);
    }
}