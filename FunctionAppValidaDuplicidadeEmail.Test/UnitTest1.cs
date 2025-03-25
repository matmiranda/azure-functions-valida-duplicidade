using FunctionAppValidaDuplicidadeEmail.Interface;
using FunctionAppValidaDuplicidadeEmail.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Data;
using System.Text;
using System.Text.Json;

namespace FunctionAppValidaDuplicidadeEmail.Test
{
    public class Tests
    {
        private Mock<ILogger<PostContatoFunction>> _loggerMock;
        private Mock<IDbConnectionFactory> _dbConnectionFactoryMock;
        private Mock<IContatoRepository> _contatoRepositoryMock;
        private PostContatoFunction _function;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<PostContatoFunction>>();
            _dbConnectionFactoryMock = new Mock<IDbConnectionFactory>();
            _contatoRepositoryMock = new Mock<IContatoRepository>();
            _function = new PostContatoFunction(_loggerMock.Object, _dbConnectionFactoryMock.Object, _contatoRepositoryMock.Object);
        }

        [Test]
        public async Task Run_EmailExists_ReturnsIsDuplicateTrue()
        {
            // Arrange
            var email = "test@example.com";
            var request = new ContatoRequest { Email = email };
            var requestBody = JsonSerializer.Serialize(request);
            var httpRequest = new DefaultHttpContext().Request;
            httpRequest.Body = new MemoryStream(Encoding.UTF8.GetBytes(requestBody));

            _contatoRepositoryMock
                .Setup(repo => repo.CheckContatoExistsByEmailAsync(It.IsAny<IDbConnection>(), email))
                .ReturnsAsync(1);

            _dbConnectionFactoryMock
                .Setup(factory => factory.CreateConnection())
                .Returns(new Mock<IDbConnection>().Object);

            // Act
            var result = await _function.Run(httpRequest) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            var jsonString = JsonSerializer.Serialize(result.Value);
            var value = JsonSerializer.Deserialize<Dictionary<string, bool>>(jsonString);
            Assert.That(value, Is.Not.Null);
            Assert.That(value["IsDuplicate"], Is.True);
        }

        [Test]
        public async Task Run_EmailDoesNotExist_ReturnsIsDuplicateFalse()
        {
            // Arrange
            var email = "test@example.com";
            var request = new ContatoRequest { Email = email };
            var requestBody = JsonSerializer.Serialize(request);
            var httpRequest = new DefaultHttpContext().Request;
            httpRequest.Body = new MemoryStream(Encoding.UTF8.GetBytes(requestBody));

            _contatoRepositoryMock
                .Setup(repo => repo.CheckContatoExistsByEmailAsync(It.IsAny<IDbConnection>(), email))
                .ReturnsAsync(0);

            _dbConnectionFactoryMock
                .Setup(factory => factory.CreateConnection())
                .Returns(new Mock<IDbConnection>().Object);

            // Act
            var result = await _function.Run(httpRequest) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            var jsonString = JsonSerializer.Serialize(result.Value);
            var value = JsonSerializer.Deserialize<Dictionary<string, bool>>(jsonString);
            Assert.That(value, Is.Not.Null);
            Assert.That(value["IsDuplicate"], Is.False);
        }
    }
}
