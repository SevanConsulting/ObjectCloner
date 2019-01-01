using System.Reflection;
using SC.ObjectCloner.Interfaces;

namespace SC.ObjectCloner
{
    /// <summary>
    /// Describe information about a property
    /// </summary>
    /// <typeparam name="TProp">Type of the property</typeparam>
    public class ClonePropertyInfo<TProp>: IValueResolver
    {
        /// <summary>
        /// The path to the property represented by this ClonePropertyInfo instance
        /// </summary>
        public string PropertyPath { get; }

        /// <summary>
        /// PropertyDescriptor describing the property being cloned
        /// </summary>
        public PropertyInfo Info { get; }

        /// <summary>
        /// The resolved source value of the property
        /// </summary>
        public TProp PropertyValue { get; private set; }

        /// <summary>
        /// Construct a new ClonePropertyInfo instance
        /// </summary>
        /// <param name="propertyPath">Path to this property instance</param>
        /// <param name="propertyInfo">PropertyInfo object for this property instance</param>
        public ClonePropertyInfo(string propertyPath, PropertyInfo propertyInfo)
        {
            PropertyPath = propertyPath;
            Info = propertyInfo;
        }

        /// <summary>
        /// Force evaluation of the property value
        /// </summary>
        /// <remarks>Explicit, internal interface implementation to hide from public consumption</remarks>
        /// <typeparam name="TObj">Type of the object containing the property</typeparam>
        /// <param name="srcObject">The object to resolve the property value against</param>
        void IValueResolver.ResolvePropertyValue<TObj>(TObj srcObject)
        {
            PropertyValue = (TProp)Info.GetValue(srcObject);
        }
    }
}