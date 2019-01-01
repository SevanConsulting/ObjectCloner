using SC.ObjectCloner.Interfaces;

namespace SC.ObjectCloner
{
    /// <summary>
    /// Factory for creating a strongly typed object cloner
    /// </summary>
    public static class ObjectCloneFactory
    {
        /// <summary>
        /// Create a cloner for the specified object
        /// </summary>
        /// <typeparam name="T">Type of object to clone</typeparam>
        /// <param name="srcObject">Source object to clone</param>
        /// <returns>A cloner that will create a copy of the source object</returns>
        public static IObjectCloner<T> CreateCloner<T>(T srcObject) where T : new()
        {
            return new ObjectCloner<T>(srcObject);
        }
    }
}