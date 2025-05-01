using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using Contatos.Consumer.API.Services.Consumers;
using Contatos.Dados.Banco;
using Contatos.Dados.Repositories;
using OpenTelemetry.Trace;
using Contatos.Consumer.Services.Consumers;

var builder = WebApplication.CreateBuilder(args);

#region Configurações

builder.Services.AddOpenTelemetry()
    .WithMetrics(opt =>
        opt
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("OpenRemoteManage.ContatosConsumer"))
            .AddMeter(builder.Configuration.GetValue<string>("OpenRemoteManageMeterName"))
            .AddAspNetCoreInstrumentation()
            .AddRuntimeInstrumentation()
            .AddProcessInstrumentation()
            .AddOtlpExporter(opts =>
            {
                opts.Endpoint = new Uri(builder.Configuration["Otel:Endpoint"]);
            })
    )
    .WithTracing(opt =>
        opt
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("OpenRemoteManage.ContatosConsumer"))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddOtlpExporter(opts =>
            {
                opts.Endpoint = new Uri(builder.Configuration["Otel:Endpoint"]);
            }));//TESTAR

#endregion

#region Dependency Injection

// Add services to the container.

// Context
builder.Services.AddDbContext<ContatosContext>();

// Repository
builder.Services.AddScoped<IContatoRepository, ContatoRepository>();

// Bus
builder.Services.AddSingleton<RabbitMQConnectionManager>();

builder.Services.AddHostedService<IncluirContatoConsumer>();
builder.Services.AddHostedService<AtualizarContatoConsumer>();
builder.Services.AddHostedService<DeletarContatoConsumer>();

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
