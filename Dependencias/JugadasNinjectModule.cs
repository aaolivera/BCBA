using Ninject.Modules;
using Repositorio;
using Servicios;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Dependencias
{
    public class JugadasNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<DbContext>().To<BCBADbContext>().InScope(ctx => OperationContext.Current);
            Bind<IRepositorio>().To<RepositorioEF>().InScope(ctx => OperationContext.Current);
            Bind<IServicioRepositorio>().To<ServicioRepositorio>().InSingletonScope();

        }
    }
}
