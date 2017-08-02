using System.Collections.Generic;
using System.Data.Entity;

namespace Repositorio
{
    public interface IConsulta<TEntidad>
    {
       List<TEntidad> Ejecutar(DbContext contexto);
    }
}
