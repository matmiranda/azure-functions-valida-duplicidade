using Dapper;
using FunctionAppValidaDuplicidadeEmail.Interface;
using System.Data;

namespace FunctionAppValidaDuplicidadeEmail.Repository
{
    public class ContatoRepository : IContatoRepository
    {
        public async Task<int> CheckContatoExistsByEmailAsync(IDbConnection dbConnection, string email)
        {
            var sql = "SELECT 1 FROM Contatos WHERE Email = @Email";
            return await dbConnection.ExecuteScalarAsync<int>(sql, new { Email = email });
        }
    }
}
