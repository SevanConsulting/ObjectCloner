using SC.ObjectCloner.Interfaces;

namespace SC.ObjectCloner.Operations
{
    /// <summary>
    /// Use existing reference operation
    /// </summary>    
    internal class UseExistingReference: IPerformer
    {
        /// <summary>
        /// Perform an operation on a property against the target object
        /// </summary>        
        /// <param name="targetObject">Newly cloned object</param>
        /// <param name="sourcePropertyInfo">Property info on the source object to operate against</param>
        public void Perform<TObj>(TObj targetObject, ClonePropertyInfo<object> sourcePropertyInfo)
        {
            var srcValue = sourcePropertyInfo.PropertyValue;
            sourcePropertyInfo.Info.SetValue(targetObject, srcValue);
        }
    }
}