using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ximo.Validation;

namespace Ximo.Utilities
{
    /// <summary>
    ///     A utility that helps cache the property information of an object.
    /// </summary>
    /// <remarks>
    ///     Used for avoiding redundant repetitive reflection on objects of the same type.
    /// </remarks>
    public static class ReferenceObjectPropertyCache
    {
        private static readonly ConcurrentDictionary<string, IEnumerable<PropertyInfo>> PropertyCache =
            new ConcurrentDictionary<string, IEnumerable<PropertyInfo>>();

        /// <summary>
        ///     Gets the properties for a reference type.
        /// </summary>
        /// <param name="referenceObjectType">Type of the reference object.</param>
        /// <returns>The properties meta data.</returns>
        public static IEnumerable<PropertyInfo> GetProperties(Type referenceObjectType)
        {
            Check.NotNull(referenceObjectType, nameof(referenceObjectType));

            if (!PropertyCache.ContainsKey(referenceObjectType.FullName))
            {
                var objectType = referenceObjectType.GetTypeInfo();

                PropertyCache[referenceObjectType.FullName] = objectType
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(
                        property =>
                            property.GetIndexParameters().Length == 0 &&
                            property.CanRead &&
                            !property.Name.Equals("HasValue") &&
                            (property.PropertyType.GetTypeInfo().IsValueType ||
                             property.PropertyType == typeof(string)))
                    .ToList();
            }
            return PropertyCache[referenceObjectType.FullName];
        }
    }
}