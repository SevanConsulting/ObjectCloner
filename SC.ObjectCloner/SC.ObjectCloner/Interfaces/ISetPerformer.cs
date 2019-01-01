namespace SC.ObjectCloner.Interfaces
{
    /// <summary>
    /// Interface describing an object that can have a performer set against it
    /// </summary>
    internal interface ISetPerformer
    {
        /// <summary>
        /// Attach an operation to the property info
        /// </summary>
        /// <param name="performer">Operation to perform</param>
        void SetOperation(IPerformer performer);
    }
}