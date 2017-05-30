namespace Ximo.Utilities
{
    public static class PrivateReflectionDynamicObjectExtensions
    {
        /// <summary>
        ///     Converts an object to a dynamic type.
        /// </summary>
        /// <param name="instance">The instance of the object whose type is to be converted.</param>
        /// <returns>The instance represented with a dynamic type.</returns>
        public static dynamic AsDynamic(this object instance)
        {
            return PrivateReflectionDynamicObject.WrapObjectIfNeeded(instance);
        }
    }
}