using System.Data;

namespace FunctionAppValidaDuplicidadeEmail.Interface
{
    public interface IContatoRepository
    {
        Task<int> CheckContatoExistsByEmailAsync(IDbConnection dbConnection, string email);
    }
}
