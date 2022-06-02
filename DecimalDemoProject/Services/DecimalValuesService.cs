namespace DecimalDemoProject.Services
{
    using DecimalDemoProject.DataAccess;
    using System;
    using System.Data.SqlTypes;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IDecimalValuesService
    {
        Task<ResultModel> CalculationsAsync(DecimalValuesRequest request, CancellationToken cancellationToken = default);
        System.Collections.Generic.List<long> Test(int count = 1000);
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
            request.Precision1 = request.Precision1 ?? 24;
            request.Precision2 = request.Precision2 ?? 24;
            request.Scale1 = request.Scale1 ?? 10;
            request.Scale2 = request.Scale2 ?? 10;

            SqlDecimal number1;
            SqlDecimal number2;

            if (!request.Number1.HasValue && !request.Number2.HasValue)
            {
                var numbersList = await _decimalValuesQueryHandler.HandleAsync(cancellationToken);
                number1 = numbersList[0];
                number2 = numbersList[1];
                request.Number1 = number1.Value;
                request.Number2 = number2.Value;
            }
            else
            {
                number1 = SqlDecimal.ConvertToPrecScale(new SqlDecimal(request.Number1.Value), request.Precision1.Value, request.Scale1.Value);
                number2 = SqlDecimal.ConvertToPrecScale(new SqlDecimal(request.Number2.Value), request.Precision2.Value, request.Scale2.Value);
            }

            var spOperationsInSqlDecimal = await _mathCalculationsQueryHandler.HandleAsync(request, cancellationToken);

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

        public System.Collections.Generic.List<long> Test(int count = 1000)
        {
            var rnd = new Random();
            long time1 = 0;
            long time2 = 0;
            for(int i = 0; i<count; i++)
            {
                var val1 = rnd.NextDecimal();
                var val2 = rnd.NextDecimal();

                System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
                stopWatch.Start();
                var number1 = SqlDecimal.ConvertToPrecScale(new SqlDecimal(val1), 28, 10);
                var number2 = SqlDecimal.ConvertToPrecScale(new SqlDecimal(val2), 28, 10);
                var operations = new MathOperations
                {
                    Addition = number1 + number2,
                    Subtraction = number1 - number2,
                    Multiplication = number1 * number2,
                    Division = number1 / number2
                };
                stopWatch.Stop();
                time1 += stopWatch.ElapsedTicks;
                stopWatch.Reset();
                stopWatch.Start();
                var number11 = val1;
                var number22 = val2;
                var Addition = number11 + number22;
                var Subtraction = number11 - number22;
                var Multiplication = number11 * number22;
                var Division = number11 / number22;
               
                stopWatch.Stop();
                time2 += stopWatch.ElapsedTicks;
            }
            return new System.Collections.Generic.List<long>() { time1, time2 };
        }
    }
}
