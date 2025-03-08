using Contatos.Dados.Banco;
using Contatos.Dados.Repositories;
using Contatos.Services.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ContatosContext>();

builder.Services.AddMemoryCache();
builder.Services.AddTransient<ICacheService, MemCacheService>();
builder.Services.AddTransient<IContatoService, ContatoService>();
builder.Services.AddTransient<IContatoRepository, ContatoRepository>();

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