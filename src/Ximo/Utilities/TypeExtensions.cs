using System;
using System.Reflection;
using Ximo.Validation;

namespace Ximo.Utilities
{
    /// <summary>
    ///     Class containing <see cref="Type" /> utilities.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        ///     Returns the default value for a specified <see cref="Type" />.
        /// </summary>
        /// <param name="type">The <see cref="Type" /> to return the default value for.</param>
        /// <returns>The default value.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="type" /> is <c>null</c>.</exception>
        public static object DefaultValue(this Type type)
        {
            Check.NotNull(type, nameof(type));

            if (type.GetTypeInfo().IsValueType && Nullable.GetUnderlyingType(type) == null)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }
    }
}