using FunctionAppValidaDuplicidadeEmail.Interface;
using FunctionAppValidaDuplicidadeEmail.Repository;
using FunctionAppValidaDuplicidadeEmail;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// Registrar as interfaces e implementações
builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddSingleton<IContatoRepository, ContatoRepository>();

builder.Build().Run();
