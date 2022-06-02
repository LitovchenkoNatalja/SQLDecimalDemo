namespace DecimalDemoProject.DataAccess
{
    using DecimalDemoProject.Services;
    using Microsoft.Extensions.Options;
    using System.Data;
    using System.Data.SqlClient;
    using System.Data.SqlTypes;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IMathCalculationsQueryHandler
    {
        Task<MathOperations> HandleAsync(DecimalValuesRequest request, CancellationToken cancellationToken = default);
    }
    public class MathCalculationsQueryHandler: IMathCalculationsQueryHandler
    {
        private DbSettings _dbSettings;

        public MathCalculationsQueryHandler(IOptions<DbSettings> dbSettings) =>
            _dbSettings = dbSettings.Value;

        public async Task<MathOperations> HandleAsync(DecimalValuesRequest request, CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                connection.Open();
                using (var cmd = PrepareCommandForMathCalculations(connection, request))
                {
                    using (var dr = await cmd.ExecuteReaderAsync(cancellationToken))
                    {
                        return BuildMathOperationsResults(dr);
                    }
                }
            }
        }

        private SqlCommand PrepareCommandForMathCalculations(SqlConnection connection, DecimalValuesRequest request)
        {
            string ExecMathCalculations = $@"EXEC MathCalculations @number1 = @number1, @number2 = @number2;";

            var cmd = new SqlCommand(ExecMathCalculations, connection);
            cmd.Parameters.Add(new SqlParameter("@number1", SqlDbType.Decimal) { Value = request.Number1.Value });
            cmd.Parameters.Add(new SqlParameter("@number2", SqlDbType.Decimal) { Value = request.Number2.Value });
            return cmd;
        }

        private MathOperations BuildMathOperationsResults(SqlDataReader reader)
        {
            if (!reader.HasRows)
                return new MathOperations();

            if (!reader.Read())
                return new MathOperations();

            var mathOperations = new MathOperations
            {
                Addition = reader.GetSqlDecimal(0),
                Subtraction = reader.GetSqlDecimal(1),
                Multiplication = reader.GetSqlDecimal(2),
                Division = reader.GetSqlDecimal(3)
            };
            return mathOperations;
        }
    }
}
