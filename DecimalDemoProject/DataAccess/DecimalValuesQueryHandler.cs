namespace DecimalDemoProject.DataAccess
{
    using Microsoft.Extensions.Options;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Data.SqlTypes;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IDecimalValuesQueryHandler
    {
        Task<List<SqlDecimal>> HandleAsync(CancellationToken cancellationToken = default);
    }

    public class DecimalValuesQueryHandler: IDecimalValuesQueryHandler
    {
        private readonly string GetDecimalValuesQuery = @"SELECT * FROM DecimalValue";

        private DbSettings _dbSettings;

        public DecimalValuesQueryHandler(IOptions<DbSettings> dbSettings) =>
            _dbSettings = dbSettings.Value;

        public async Task<List<SqlDecimal>> HandleAsync(CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                connection.Open();
                using (var cmd = new SqlCommand(GetDecimalValuesQuery, connection))
                {
                    using (var dr = await cmd.ExecuteReaderAsync(cancellationToken))
                    {
                        if (!dr.HasRows)
                            return null;

                        List<SqlDecimal> sqlDecimals = new List<SqlDecimal>();
                        while (dr.Read())
                        {
                            sqlDecimals.Add(dr.GetSqlDecimal(0));
                        }
                        return sqlDecimals;
                    }
                }
            }
        }
    }
}
