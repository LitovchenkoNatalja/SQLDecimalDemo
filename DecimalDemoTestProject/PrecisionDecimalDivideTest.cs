namespace DecimalDemoTestProject
{
    using DecimalDemoProject;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PrecisionDecimalDivideTest
    {
        [TestMethod]
        public void Divide_Correct_Values_Returns_Its_Division()
        {
            PrecisionDecimal operand1 = new PrecisionDecimal(965.3345M, 7, 4);
            PrecisionDecimal operand2 = new PrecisionDecimal(0.28M, 2, 2);
            decimal expectedValue = 3447.6232142M;
            PrecisionDecimal actualDeciamal = operand1 / operand2;
            Assert.AreEqual(expectedValue, actualDeciamal.Value);
        }

        [TestMethod]
        public void Divide_Values_Whis_Total_Scale_More_Than_Possible_Returns_Its_Division()
        {
            PrecisionDecimal operand1 = new PrecisionDecimal(965.3345M, 38, 20);
            PrecisionDecimal operand2 = new PrecisionDecimal(0.28M, 38, 22);
            decimal expectedValue = 3447.623214M;
            PrecisionDecimal actualDeciamal = operand1 / operand2;
            Assert.AreEqual(expectedValue, actualDeciamal.Value);
        }
    }
}
