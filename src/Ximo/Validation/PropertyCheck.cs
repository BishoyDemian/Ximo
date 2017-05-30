using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Ximo.Validation
{
    /// <summary>
    ///     Class with validation utilities to be used in code contract fashion for validating properties.
    /// </summary>
    /// <remarks>
    ///     The class uses <seealso cref="System.Runtime.CompilerServices" /> to detect the name of the property being
    ///     validated. Under the hood it uses the same logic in the <see cref="Check" /> class.
    /// </remarks>
    [DebuggerStepThrough]
    public static class PropertyCheck
    {
        /// <summary>
        ///     Enforces that an property value is not null.
        /// </summary>
        /// <param name="value">The property value to be checked.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="propertyName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="propertyName" /> is empty or contains only whitespace characters.</exception>
        /// <exception cref="ValidationException"><paramref name="value" /> is <c>null</c>.</exception>
        public static object NotNull(object value, [CallerMemberName] string propertyName = "")
        {
            return Check.NotNull(value, propertyName);
        }

        /// <summary>
        ///     Enforces that a <see cref="string" /> value is not <c>null</c> or empty, i.e. length is 0.
        /// </summary>
        /// <param name="value">The property value to be checked.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="propertyName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="propertyName" /> is empty or contains only whitespace characters.</exception>
        /// <exception cref="ValidationException"><paramref name="value" /> is <c>null</c> or empty.</exception>
        public static string NotNullOrEmpty(string value, [CallerMemberName] string propertyName = "")
        {
            return Check.NotNullOrEmpty(value, propertyName);
        }

        /// <summary>
        ///     Enforces that a <see cref="string" /> value is not <c>null</c>, empty or contains only whitespace characters.
        /// </summary>
        /// <param name="value">The property value to be checked.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="propertyName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="propertyName" /> is empty or contains only whitespace characters.</exception>
        /// <exception cref="ValidationException">
        ///     <paramref name="value" /> is <c>null</c>, empty or contains only whitespace
        ///     characters.
        /// </exception>
        public static string NotNullOrWhitespace(string value, [CallerMemberName] string propertyName = "")
        {
            return Check.NotNullOrWhitespace(value, propertyName);
        }

        /// <summary>
        ///     Enforces that the length of a <see cref="string" /> value does not exceed a specified maximum
        ///     number of characters.
        /// </summary>
        /// <param name="value">The property value to be checked.</param>
        /// <param name="maximumLength">The maximum length allowed for the <see cref="string" /> value.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="propertyName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="propertyName" /> is empty or contains only whitespace characters.</exception>
        /// <exception cref="ArgumentException"><paramref name="maximumLength" /> is less or equal to 0.</exception>
        /// <exception cref="ValidationException">
        ///     <paramref name="value" /> length exceeds the <paramref name="maximumLength" /> of
        ///     characters specified.
        /// </exception>
        public static string MaxLength(string value, int maximumLength, [CallerMemberName] string propertyName = "")
        {
            return Check.MaxLength(value, maximumLength, propertyName);
        }

        /// <summary>
        ///     Enforces that the length of a <see cref="string" /> value has at least a specified minimum
        ///     number of characters.
        /// </summary>
        /// <param name="value">The property value to be checked.</param>
        /// <param name="minimumLength">The minimum length allowed for the <see cref="string" /> value.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="propertyName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="propertyName" /> is empty or contains only whitespace characters.</exception>
        /// <exception cref="ArgumentException"><paramref name="minimumLength" /> is less than 0.</exception>
        /// <exception cref="ValidationException">
        ///     <paramref name="value" /> length falls below the
        ///     <paramref name="minimumLength" /> of characters specified.
        /// </exception>
        public static string MinLength(string value, int minimumLength, [CallerMemberName] string propertyName = "")
        {
            return Check.MinLength(value, minimumLength, propertyName);
        }

        /// <summary>
        ///     Enforces that the length of a <see cref="string" /> value has at least a specified minimum
        ///     number of characters and does not exceed a specified maximum number of characters.
        /// </summary>
        /// <param name="value">The property value to be checked.</param>
        /// <param name="minimumLength">The minimum length allowed for the <see cref="string" /> value.</param>
        /// <param name="maximumLength">The maximum length allowed for the <see cref="string" /> value.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="propertyName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="propertyName" /> is empty or contains only whitespace characters.</exception>
        /// <exception cref="ArgumentException">
        ///     The <paramref name="maximumLength" /> value is less than or equal to 0
        ///     (and/or)
        ///     The <paramref name="minimumLength" /> value is less than 0
        ///     (and/or)
        ///     The <paramref name="maximumLength" /> value is less than The <paramref name="minimumLength" /> specified.
        /// </exception>
        /// <exception cref="ValidationException">
        ///     <paramref name="value" /> length is less than the <paramref name="minimumLength" /> specified or exceeds the
        ///     <paramref name="maximumLength" /> specified.
        /// </exception>
        public static string StringLength(string value, int minimumLength, int maximumLength,
            [CallerMemberName] string propertyName = "")
        {
            return Check.StringLength(value, minimumLength, maximumLength, propertyName);
        }

        /// <summary>
        ///     Enforces that a <see cref="string" /> value represents a valid email.
        /// </summary>
        /// <param name="value">The property value to be checked.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="propertyName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="propertyName" /> is empty or contains only whitespace characters.</exception>
        /// <exception cref="ValidationException">
        ///     <paramref name="value" /> is <c>null</c>, empty, contains only whitespace characters or
        ///     not a valid email.
        /// </exception>
        public static string Email(string value, [CallerMemberName] string propertyName = "")
        {
            return Check.Email(value, propertyName);
        }

        /// <summary>
        ///     Enforces that a value falls within a specified range.
        /// </summary>
        /// <typeparam name="T">The type of the value to compare.</typeparam>
        /// <param name="value">The property value to be checked.</param>
        /// <param name="minimum">The minimum value of the specified range.</param>
        /// <param name="maximum">The maximum value of the specified range.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="propertyName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="propertyName" /> is empty or contains only whitespace characters.</exception>
        /// <exception cref="ValidationException">
        ///     <paramref name="value" /> does not fall within the specified range. It is
        ///     either less than <paramref name="minimum" /> value or larger than <paramref name="maximum" /> value specified.
        /// </exception>
        public static T Range<T>(T value, T minimum, T maximum, [CallerMemberName] string propertyName = "")
            where T : IComparable, IComparable<T>
        {
            return Check.Range(value, minimum, maximum, propertyName);
        }

        /// <summary>
        ///     Enforces that a value does not exceed a specified maximum.
        /// </summary>
        /// <typeparam name="T">The type of the value to compare.</typeparam>
        /// <param name="value">The property value to be checked.</param>
        /// <param name="maximum">The maximum value allowed.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="propertyName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="propertyName" /> is empty or contains only whitespace characters.</exception>
        /// <exception cref="ValidationException">
        ///     <paramref name="value" /> exceeds the specified <paramref name="maximum" />
        ///     value.
        /// </exception>
        public static T Maximum<T>(T value, T maximum, [CallerMemberName] string propertyName = "")
            where T : IComparable, IComparable<T>
        {
            return Check.Maximum(value, maximum, propertyName);
        }

        /// <summary>
        ///     Enforces that a value is not less than a specified minimum.
        /// </summary>
        /// <typeparam name="T">The type of the value to compare.</typeparam>
        /// <param name="value">The property value to be checked.</param>
        /// <param name="minimum">The minimum value allowed.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="propertyName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="propertyName" /> is empty or contains only whitespace characters.</exception>
        /// <exception cref="ValidationException">
        ///     <paramref name="value" /> is less than the specified <paramref name="minimum" />
        ///     value.
        /// </exception>
        public static T Minimum<T>(T value, T minimum, [CallerMemberName] string propertyName = "")
            where T : IComparable, IComparable<T>
        {
            return Check.Minimum(value, minimum, propertyName);
        }


        /// <summary>
        ///     Enforces that a <see cref="string" /> value matches a specified regular expression pattern.
        /// </summary>
        /// <param name="value">The property value to be checked.</param>
        /// <param name="regularExpression">The regular expression used for evaluation of the value.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="regexOptions">(Optional) Provides enumerated values to use to set regular expression options.</param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="propertyName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="propertyName" /> is empty or contains only whitespace characters.</exception>
        /// <exception cref="ValidationException">
        ///     <paramref name="regularExpression" /> is <c>null</c>, empty, contains only whitespace characters
        ///     OR
        ///     <paramref name="value" /> is <c>null</c> or does not match the <paramref name="regularExpression" /> pattern.
        /// </exception>
        public static string Regex(string value, string regularExpression,
            [CallerMemberName] string propertyName = "",
            RegexOptions regexOptions = RegexOptions.None)
        {
            return Check.Regex(value, regularExpression, propertyName, regexOptions);
        }

        /// <summary>
        ///     Enforces that a <see cref="string" /> value is a valid url.
        /// </summary>
        /// <param name="value">The property value to be checked.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="propertyName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="propertyName" /> is empty or contains only whitespace characters.</exception>
        /// <exception cref="ValidationException">
        ///     <paramref name="value" /> is <c>null</c>, empty, contains only whitespace characters or not a
        ///     valid URL.
        /// </exception>
        public static string Url(string value, [CallerMemberName] string propertyName = "")
        {
            return Check.Url(value, propertyName);
        }

        /// <summary>
        ///     Enforces that a <see cref="Guid" /> value is not <see cref="Guid.Empty" />.
        /// </summary>
        /// <param name="value">The property value to be checked.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="propertyName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="propertyName" /> is empty or contains only whitespace characters.</exception>
        /// <exception cref="ValidationException"><paramref name="value" /> is <c>Guid.Empty</c></exception>
        public static Guid NotEmpty(Guid value, [CallerMemberName] string propertyName = "")
        {
            return Check.NotEmpty(value, propertyName);
        }

        /// <summary>
        ///     Enforces that the provided value to satisfy the specified predicate.
        /// </summary>
        /// <typeparam name="T">The type of the value to be tested.</typeparam>
        /// <param name="value">The property value to be checked.</param>
        /// <param name="predicate">The predicate which the value has to satisfy.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="propertyName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="propertyName" /> is empty or contains only whitespace characters.</exception>
        /// <exception cref="ValidationException">
        ///     The <paramref name="predicate" /> is <c>null</c> or the value did not satisfy the
        ///     specified predicate.
        /// </exception>
        public static T Requires<T>([NoEnumeration] T value, Predicate<T> predicate,
            [CallerMemberName] string propertyName = "")
        {
            return Check.Requires(value, predicate, propertyName);
        }
    }
}