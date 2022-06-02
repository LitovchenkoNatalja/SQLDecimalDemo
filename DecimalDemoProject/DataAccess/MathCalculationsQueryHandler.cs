namespace DecimalDemoProject.DataAccess
{
    using Microsoft.Extensions.Options;
    using System.Data;
    using System.Data.SqlClient;
    using System.Data.SqlTypes;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Transactions;

    public interface IMathCalculationsQueryHandler
    {
        Task<MathOperations> HandleAsync(SqlDecimal number1, SqlDecimal number2, CancellationToken cancellationToken = default);
    }
    public class MathCalculationsQueryHandler: IMathCalculationsQueryHandler
    {
        private DbSettings _dbSettings;

        public MathCalculationsQueryHandler(IOptions<DbSettings> dbSettings) =>
            _dbSettings = dbSettings.Value;

        public async Task<MathOperations> HandleAsync(SqlDecimal number1, SqlDecimal number2, CancellationToken cancellationToken = default)
        {
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
                {
                    connection.Open();
                    await DropMathCalculationsStoredProcedure(connection);
                    await CreateMathCalculationsStoredProcedure(connection, number1.Precision, number1.Scale, number2.Precision, number2.Scale);
                    using (var cmd = PrepareCommandForMathCalculations(connection, number1, number2))
                    {
                        using (var dr = await cmd.ExecuteReaderAsync(cancellationToken))
                        {
                            var result = BuildMathOperationsResults(dr);
                            transactionScope.Complete();
                            return result;
                        }
                    }
                }
            }
        }

        private async Task DropMathCalculationsStoredProcedure(SqlConnection connection)
        {
            string dropMathCalculations = @"
                IF EXISTS(SELECT 1 FROM sys.procedures 
                    WHERE Name = 'MathCalculations')
                BEGIN
                    DROP PROCEDURE dbo.MathCalculations
                END";
            using (var cmd = new SqlCommand(dropMathCalculations, connection))
            {
                await cmd.ExecuteNonQueryAsync();
            }
        }

        private async Task CreateMathCalculationsStoredProcedure(SqlConnection connection, byte precision1, byte scale1, byte precision2, byte scale2)
        {
            string createMathCalculations = @$"
                CREATE PROCEDURE MathCalculations @number1 decimal({precision1}, {scale1}), @number2 decimal({precision2}, {scale2})
                AS
                    SELECT @number1 + @number2 AS Addition, @number1 -@number2 AS Subtraction, @number1 *@number2 AS Multiplication, @number1 / @number2 AS Division";
            using (var cmd = new SqlCommand(createMathCalculations, connection))
            {
                await cmd.ExecuteNonQueryAsync();
            }
        }

        private SqlCommand PrepareCommandForMathCalculations(SqlConnection connection, SqlDecimal number1, SqlDecimal number2)
        {
            string execMathCalculations = $@"EXEC MathCalculations @number1 = @number1, @number2 = @number2;";

            var cmd = new SqlCommand(execMathCalculations, connection);
            cmd.Parameters.Add(new SqlParameter("@number1", SqlDbType.Decimal) { SqlValue = number1 });
            cmd.Parameters.Add(new SqlParameter("@number2", SqlDbType.Decimal) { SqlValue = number2 });
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
