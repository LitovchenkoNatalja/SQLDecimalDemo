namespace DecimalDemoTestProject
{
    using DecimalDemoProject;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PrecisionDecimalMultiplyTest
    {
        [TestMethod]
        public void Multiply_Correct_Values_Returns_Its_Multiplication()
        {
            PrecisionDecimal operand1 = new PrecisionDecimal(965.3345M, 7, 4);
            PrecisionDecimal operand2 = new PrecisionDecimal(64.28M, 4, 2);
            decimal expectedValue = 62051.70166M;
            PrecisionDecimal actualDeciamal = operand1 * operand2;
            Assert.AreEqual(expectedValue, actualDeciamal.Value);
        }

        [TestMethod]
        public void Multiply_Values_Whis_Total_Scale_More_Than_Possible_Returns_Its_Multiplication()
        {
            PrecisionDecimal operand1 = new PrecisionDecimal(965.12345678901234567891M, 38, 20);
            PrecisionDecimal operand2 = new PrecisionDecimal(64.2812345678901234567891M, 38, 22);
            decimal expectedValue = 62039.327313M;
            PrecisionDecimal actualDeciamal = operand1 * operand2;
            Assert.AreEqual(expectedValue, actualDeciamal.Value);
        }
    }
}
