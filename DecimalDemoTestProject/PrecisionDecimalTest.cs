using DecimalDemoProject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DecimalDemoTestProject
{
    [TestClass]
    public class PrecisionDecimalTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_Presigion_More_Than_38_ArgumentException()
        {
            PrecisionDecimal precisionDecimal = new PrecisionDecimal(12.2M, 39, 1);
        }

        [TestMethod]
        public void Constructor_Presigion_Is_38_Constructed()
        {
            decimal expectedValue = 12.2M;
            PrecisionDecimal precisionDecimal = new PrecisionDecimal(12.2M, 38, 1);
            Assert.AreEqual(precisionDecimal.Value, expectedValue);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_Presigion_Less_Than_0_ArgumentException()
        {
            PrecisionDecimal precisionDecimal = new PrecisionDecimal(12.2M, -10, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_Presigion_Is_0_ArgumentException()
        {
            PrecisionDecimal precisionDecimal = new PrecisionDecimal(12.2M, 0, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_Scale_Less_Than_0_ArgumentException()
        {
            PrecisionDecimal precisionDecimal = new PrecisionDecimal(12.2M, 10, -12);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_Scale_More_Than_Presigion_ArgumentException()
        {
            PrecisionDecimal precisionDecimal = new PrecisionDecimal(12.2M, 10, 38);
        }

        [TestMethod]
        public void Constructor_Scale_Equals_To_Presigion_Constructed()
        {
            decimal expectedValue = 0.2M;
            PrecisionDecimal precisionDecimal = new PrecisionDecimal(0.2M, 1, 1);
            Assert.AreEqual(precisionDecimal.Value, expectedValue);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_Integral_Part_Of_Value_More_Than_Indicated_ArgumentException()
        {
            PrecisionDecimal precisionDecimal = new PrecisionDecimal(123.2M, 2, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_Integral_Part_Of_Value_More_Than_Indicated_After_Rounding_ArgumentException()
        {
            PrecisionDecimal precisionDecimal = new PrecisionDecimal(0.988M, 1, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_Integral_Part_Of_Value_More_Than_Indicated_When_Scale_Equals_To_Presigion_ArgumentException()
        {
            PrecisionDecimal precisionDecimal = new PrecisionDecimal(1.2M, 2, 2);
        }

        [TestMethod]
        public void Constructor_Value_Scale_More_Than_Indicated_Result_Is_Rounded_ToEven_And_Constructed()
        {
            decimal expectedValue = 0.9M;
            PrecisionDecimal precisionDecimal = new PrecisionDecimal(0.944M, 1, 1);
            Assert.AreEqual(precisionDecimal.Value, expectedValue);
        }

        [TestMethod]
        public void Constructor_Correct_Value_Constructed()
        {
            decimal expectedValue = 12.2M;
            PrecisionDecimal precisionDecimal = new PrecisionDecimal(12.2M, 4, 2);
            Assert.AreEqual(precisionDecimal.Value, expectedValue);
        }
    }
}
