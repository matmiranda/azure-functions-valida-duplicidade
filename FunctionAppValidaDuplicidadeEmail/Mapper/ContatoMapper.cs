using FunctionAppValidaDuplicidadeEmail.Entity;
using FunctionAppValidaDuplicidadeEmail.Response;

namespace FunctionAppValidaDuplicidadeEmail.Mapper
{
    public static class ContatoMapper
    {
        public static ContatosGetResponse ContatoPorId(ContatoEntity contatoEntity)
        {
            return new ContatosGetResponse
            {
                Id = contatoEntity.Id,
                Nome = contatoEntity.Nome,
                Telefone = contatoEntity.Telefone,
                Email = contatoEntity.Email,
                DDD = contatoEntity.DDD,
                Regiao = contatoEntity.Regiao.ToString()
            };
        }
    }
}
