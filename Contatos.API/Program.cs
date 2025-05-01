using Contatos.API.Configurations;
using Contatos.Dados.Banco;
using Contatos.Dados.Repositories;
using Contatos.Services.Services.Cache;
using Contatos.Services.Services.MessageBus;
using Contatos.Services.Services.Persistence;
using Contatos.Services.Services.Producer;
using Polly.Bulkhead;
using Polly.CircuitBreaker;
using Polly.Retry;

var builder = WebApplication.CreateBuilder(args);

#region Configura��es

// Configura��o do OpenTelemetry
Configuration.ConfigureOpenTelemetry(builder);

// Configura��o das pol�ticas
var circuitBreakerPolicy = Configuration.ConfigureCircuitBreakerPolicy(builder);
var bulkheadPolicy = Configuration.ConfigureBulkheadPolicy(builder);
var retryPolicy = Configuration.ConfigureRetryPolicy(builder);

#endregion

#region Dependency Injection

// Add services to the container.

// Registre as pol�ticas no container de servi�os
builder.Services.AddSingleton((AsyncCircuitBreakerPolicy)circuitBreakerPolicy);
builder.Services.AddSingleton((AsyncBulkheadPolicy)bulkheadPolicy);
builder.Services.AddSingleton((AsyncRetryPolicy)retryPolicy);

// Context
builder.Services.AddDbContext<ContatosContext>();

// Cache
builder.Services.AddMemoryCache();
builder.Services.AddTransient<ICacheService, MemCacheService>();

// Services
builder.Services.AddTransient<IContatoService, ContatoService>();

// Repository
builder.Services.AddTransient<IContatoRepository, ContatoRepository>();

// Message Bus
builder.Services.AddScoped<IMessageBus, MessageBus>();
builder.Services.AddScoped<IIncluirContatoProducer, IncluirContatoProducer>();
builder.Services.AddScoped<IAtualizarContatoProducer, AtualizarContatoProducer>();
builder.Services.AddScoped<IDeletarContatoProducer, DeletarContatoProducer>();

#endregion

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();