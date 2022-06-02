namespace DecimalDemoProject.Services
{
    using System.Data.SqlTypes;

    public class MathOperations
    {
        public SqlDecimal Addition { get; set; }
        public SqlDecimal Subtraction { get; set; }
        public SqlDecimal Multiplication { get; set; }
        public SqlDecimal Division { get; set; }
    }
}
