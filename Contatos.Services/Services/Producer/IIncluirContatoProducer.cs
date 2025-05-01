using Contatos.Modelos.Modelos;

namespace Contatos.Services.Services.Producer;

public interface IIncluirContatoProducer
{
    void Publish(Contato model);
}