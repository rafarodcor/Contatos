using Contatos.Modelos.Modelos;

namespace Contatos.Services.Services.Producer;
public interface IDeletarContatoProducer
{
    void Publish(Contato model);
}