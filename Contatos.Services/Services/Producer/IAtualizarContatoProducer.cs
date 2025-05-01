using Contatos.Modelos.Modelos;

namespace Contatos.Services.Services.Producer;

public interface IAtualizarContatoProducer
{
    void Publish(Contato model);
}