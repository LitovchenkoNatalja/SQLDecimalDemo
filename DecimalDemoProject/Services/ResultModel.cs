namespace DecimalDemoProject.Services
{
    public class ResultMathOperations
    {
        public decimal Addition { get; set; }
        public decimal Subtraction { get; set; }
        public decimal Multiplication { get; set; }
        public decimal Division { get; set; }
    }

    public class ResultModel
    {
        public decimal Number1 { get; set; }
        public decimal Number2 { get; set; }
        public ResultMathOperations CSharpOperations { get; set; }
        public ResultMathOperations SQLOperations { get; set; }
        public bool SameAddition { get; set; }
        public bool SameSubtraction { get; set; }
        public bool SameMultiplication { get; set; }
        public bool SameDivision { get; set; }
    }
}
