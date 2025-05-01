using Contatos.Modelos.Modelos;
using Contatos.Services.Services.MessageBus;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Contatos.Services.Services.Producer;

public class IncluirContatoProducer : IIncluirContatoProducer
{
    #region Constants

    private const string QUEUE_NAME = "INCLUIR_CONTATO";

    #endregion

    #region Properties

    private readonly IMessageBus _messageBus;
    private readonly ILogger<IncluirContatoProducer> _logger;

    #endregion

    #region Constructors

    public IncluirContatoProducer(IMessageBus messageBus, ILogger<IncluirContatoProducer> logger)
    {
        _messageBus = messageBus;
        _logger = logger;
    }

    #endregion

    #region Methods

    public void Publish(Contato model)
    {
        _logger.LogInformation($"Producer > Publish > Contato > {QUEUE_NAME}");

        var message = System.Text.Json.JsonSerializer.Serialize(model);
        var body = Encoding.UTF8.GetBytes(message);

        _messageBus.Publish(QUEUE_NAME, body);
    }

    #endregion    
}