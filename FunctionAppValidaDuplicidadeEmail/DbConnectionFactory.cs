using FunctionAppValidaDuplicidadeEmail.Interface;
using MySqlConnector;
using System.Data;

namespace FunctionAppValidaDuplicidadeEmail
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        public IDbConnection CreateConnection()
        {
            string? connectionString = Environment.GetEnvironmentVariable("MYSQLCONNECTIONSTRING");
            connectionString = "Server=server-fiap.mysql.database.azure.com;Port=3306;Database=dbfiap;Uid=fiap;Pwd=Mysql123@;";
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("MYSQLCONNECTIONSTRING não está definida nas variáveis de ambiente.");
            }
            return new MySqlConnection(connectionString);
        }
    }
}
