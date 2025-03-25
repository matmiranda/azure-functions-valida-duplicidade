using System.Data;

namespace FunctionAppValidaDuplicidadeEmail.Interface
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
