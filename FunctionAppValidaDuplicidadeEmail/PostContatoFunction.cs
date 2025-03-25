using FunctionAppValidaDuplicidadeEmail.Interface;
using FunctionAppValidaDuplicidadeEmail.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FunctionAppValidaDuplicidadeEmail
{
    public class PostContatoFunction
    {
        private readonly ILogger<PostContatoFunction> _logger;
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly IContatoRepository _contatoRepository;

        public PostContatoFunction(ILogger<PostContatoFunction> logger, IDbConnectionFactory dbConnectionFactory, IContatoRepository contatoRepository)
        {
            _logger = logger;
            _dbConnectionFactory = dbConnectionFactory;
            _contatoRepository = contatoRepository;
        }

        [Function("CheckDuplicateEmailFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "contato/valida-duplicidade")] HttpRequest req)
        {
            _logger.LogInformation("Iniciando execução da função CheckDuplicateEmailFunction.");

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                ContatoRequest? data = JsonSerializer.Deserialize<ContatoRequest>(requestBody);
                string? email = data?.Email;

                if (string.IsNullOrEmpty(email))
                {
                    _logger.LogError("Email não fornecido ou é nulo.");
                    return new BadRequestObjectResult("Email não fornecido ou é nulo.");
                }

                using IDbConnection dbConnection = _dbConnectionFactory.CreateConnection();
                dbConnection.Open();
                _logger.LogInformation("Conexão com o banco de dados estabelecida.");
                var count = await _contatoRepository.CheckContatoExistsByEmailAsync(dbConnection, email);
                if (count > 0)
                {
                    _logger.LogInformation("Email {Email} já está em uso.", email);
                    return new OkObjectResult(new { IsDuplicate = true });
                }
                else
                {
                    _logger.LogInformation("Email {Email} não está em uso.", email);
                    return new OkObjectResult(new { IsDuplicate = false });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao executar a função.");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
