namespace DecimalDemoProject.Services
{
    using DecimalDemoProject.DataAccess;
    using System.Data.SqlTypes;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IDecimalValuesService
    {
        Task<ResultModel> CalculationsAsync(DecimalValuesRequest request, CancellationToken cancellationToken = default);
    }

    public class DecimalValuesService: IDecimalValuesService
    {
        private readonly IDecimalValuesQueryHandler _decimalValuesQueryHandler;
        private readonly IMathCalculationsQueryHandler _mathCalculationsQueryHandler;

        public DecimalValuesService (
            IDecimalValuesQueryHandler decimalValuesQueryHandler,
            IMathCalculationsQueryHandler mathCalculationsQueryHandler)
        {
            _decimalValuesQueryHandler = decimalValuesQueryHandler;
            _mathCalculationsQueryHandler = mathCalculationsQueryHandler;
        }

        public async Task<ResultModel> CalculationsAsync(DecimalValuesRequest request, CancellationToken cancellationToken = default)
        {
            SqlDecimal number1;
            SqlDecimal number2;

            if (!request.Number1.HasValue && !request.Number2.HasValue)
            {
                var numbersList = await _decimalValuesQueryHandler.HandleAsync(cancellationToken);
                number1 = numbersList[0];
                number2 = numbersList[1];
            }
            else
            {
                number1 = SqlDecimal.ConvertToPrecScale(new SqlDecimal(request.Number1.Value), request.Precision1.Value, request.Scale1.Value);
                number2 = SqlDecimal.ConvertToPrecScale(new SqlDecimal(request.Number2.Value), request.Precision2.Value, request.Scale2.Value);
            }

            var spOperationsInSqlDecimal = await _mathCalculationsQueryHandler.HandleAsync(number1, number2, cancellationToken);

            var spOperations = new ResultMathOperations
            {
                Addition = (decimal)spOperationsInSqlDecimal.Addition,
                Subtraction = (decimal)spOperationsInSqlDecimal.Subtraction,
                Multiplication = (decimal)spOperationsInSqlDecimal.Multiplication,
                Division = (decimal)spOperationsInSqlDecimal.Division
            };
            var operations = new ResultMathOperations
            {
                Addition = (decimal)(number1 + number2),
                Subtraction = (decimal)(number1 - number2),
                Multiplication = (decimal)(number1 * number2),
                Division = (decimal)(number1 / number2)
            };
            var result = new ResultModel
            {
                Number1 = number1.Value,
                Number2 = number2.Value,
                SQLOperations = spOperations,
                CSharpOperations = operations,
                SameAddition = spOperations.Addition == operations.Addition,
                SameSubtraction = spOperations.Subtraction == operations.Subtraction,
                SameMultiplication = spOperations.Multiplication == operations.Multiplication,
                SameDivision = spOperations.Division == operations.Division
            };
            return result;
        }
    }
}
