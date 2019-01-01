namespace SC.ObjectCloner.Interfaces
{
    /// <summary>
    /// Describe an interface for performing an operation against a property if a condition is matched
    /// </summary>
    internal interface IFilteredPerformer: IPerformer
    {
        /// <summary>
        /// Checks the property info against a predicate function
        /// </summary>
        /// <param name="sourcePropertyInfo">Property info to test the predicate function against</param>
        /// <returns>True if the specified property info matches the predicate</returns>        
        bool PropertyMatchesPredicate(ClonePropertyInfo<object> sourcePropertyInfo);
    }
}