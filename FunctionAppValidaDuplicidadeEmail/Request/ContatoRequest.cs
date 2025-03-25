using System.Text.Json.Serialization;

namespace FunctionAppValidaDuplicidadeEmail.Request
{
    public class ContatoRequest
    {
        [JsonPropertyName("email")]
        public required string Email { get; set; }
    }
}
