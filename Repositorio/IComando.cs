using System.Data.Entity;

namespace Repositorio
{
    public interface IComando
    {
       void Ejecutar(DbContext contexto);
    }
}
