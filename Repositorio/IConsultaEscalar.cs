using System.Data.Entity;

namespace Repositorio
{
    public interface IConsultaEscalar<TEntidad>
    {
       TEntidad Ejecutar(DbContext contexto);
    }
}
