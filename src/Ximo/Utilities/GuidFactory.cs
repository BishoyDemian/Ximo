using System;

namespace Ximo.Utilities
{
    /// <summary>
    ///     Contains utility methods for creating <see cref="Guid" />.
    /// </summary>
    public static class GuidFactory
    {
        private static readonly long BaseDateTicks = new DateTime(1900, 1, 1).Ticks;

        /// <summary>
        ///     Generate a new <see cref="Guid" /> using the comb algorithm.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Guid generated using a strategy suggested by Jimmy Nilsson
        ///         <a href="http://www.informit.com/articles/article.asp?p=25862">article</a>
        ///     </para>
        /// </remarks>
        /// <returns>A new <see cref="Guid" />.</returns>
        public static Guid NewGuidComb()
        {
            byte[] guidArray = Guid.NewGuid().ToByteArray();

            DateTime now = DateTime.UtcNow;

            // Get the days and milliseconds which will be used to build the byte string 
            TimeSpan days = new TimeSpan(now.Ticks - BaseDateTicks);
            TimeSpan msecs = now.TimeOfDay;

            // Convert to a byte array 
            // Note that SQL Server is accurate to 1/300th of a millisecond so we divide by 3.333333 
            byte[] daysArray = BitConverter.GetBytes(days.Days);
            byte[] msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));

            // Reverse the bytes to match SQL Servers ordering 
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            // Copy the bytes into the guid 
            Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);

            return new Guid(guidArray);
        }
    }
}