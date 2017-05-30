using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ximo.Utilities;

namespace Ximo.Tests.Utilities
{
    /// <summary>
    ///     Test for testing <see cref="GuidFactory" />
    /// </summary>
    [TestClass]
    public class GuidFactoryTests
    {
        /// <summary>
        ///     Tests if generated guids are sequential.
        /// </summary>
        [TestMethod]
        public void TestIfGeneratedGuidsAreSequential()
        {
            var arraySize = 1000;
            var array = new Guid[arraySize];
            array[0] = GuidFactory.NewGuidComb();

            for (var i = 1; i < arraySize; i++)
            {
                array[i] = GuidFactory.NewGuidComb();
                Assert.AreEqual(-1, array[i - 1].CompareTo(array[i]));
            }
        }
    }
}