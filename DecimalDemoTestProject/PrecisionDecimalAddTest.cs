namespace DecimalDemoTestProject
{
    using DecimalDemoProject;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PrecisionDecimalAddTest
    {
        [TestMethod]
        public void Add_Correct_Values_Returns_Its_Sum()
        {
            PrecisionDecimal operand1 = new PrecisionDecimal(965.3345M, 7, 4);
            PrecisionDecimal operand2 = new PrecisionDecimal(64.28M, 4, 2);
            decimal expectedValue = 1029.6145M;
            PrecisionDecimal actualDeciamal = operand1 + operand2;
            Assert.AreEqual(expectedValue, actualDeciamal.Value);
        }

        [TestMethod]
        public void Add_Values_With_Precision_Equals_To_38_When_Result_Precision_Less_Than_Max_Value_Returns_Its_Sum()
        {
            PrecisionDecimal operand1 = new PrecisionDecimal(965.3345M, 38, 4);
            PrecisionDecimal operand2 = new PrecisionDecimal(64.28M, 38, 2);
            decimal expectedValue = 1029.6145M;
            PrecisionDecimal actualDeciamal = operand1 + operand2;
            Assert.AreEqual(expectedValue, actualDeciamal.Value);
        }
    }
}
