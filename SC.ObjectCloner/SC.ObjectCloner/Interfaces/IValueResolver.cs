namespace SC.ObjectCloner.Interfaces
{
    /// <summary>
    /// Describe a class that can resolve the value of a property
    /// </summary>
    internal interface IValueResolver
    {
        /// <summary>
        /// Force evaluation of the property value
        /// </summary>
        /// <typeparam name="TObj">Type of the object containing the property</typeparam>
        /// <param name="srcObject">The object to resolve the property value against</param>
        void ResolvePropertyValue<TObj>(TObj srcObject);
    }
}