using Dominio.Entidades;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace Repositorio
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class BCBADbContext : DbContext
    {

        public BCBADbContext() : base("jugadasdb")
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<BCBADbContext, Migrations.Configuration>("jugadasdb"));
        }
        //public BCBADbContext(string connStringName) : base(connStringName) { }
        static BCBADbContext()
        {
            // static constructors are guaranteed to only fire once per application.
            // I do this here instead of App_Start so I can avoid including EF
            // in my MVC project (I use UnitOfWork/Repository pattern instead)
            DbConfiguration.SetConfiguration(new MySql.Data.Entity.MySqlEFConfiguration());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // No usamos nombres de tablas en plural. Igualmente la pluralización fucniona solo con nombres en ingles.
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            //Se mapean todas las entidades bajo el namespace Molinos.Scato.Dominio.Entidades      
            MapearAssemblyDe<Jugada>(modelBuilder, x => x.Namespace == typeof(Jugada).Namespace, 
                excluir: null);

        }

        private void MapearAssemblyDe<TEntidad>(DbModelBuilder modelBuilder, Predicate<Type> incluir, Predicate<Type> excluir)
        {
            var tiposEntidades  = typeof (TEntidad).Assembly.ExportedTypes.Where(x => incluir(x));
            if (excluir != null)
            {
                tiposEntidades = tiposEntidades.Where(x => !excluir(x));
            }

            var metodo = modelBuilder.GetType().GetMethod("Entity");
            foreach (var tipoEntidad in tiposEntidades)
            {
                metodo.MakeGenericMethod(tipoEntidad).Invoke(modelBuilder, null);
            }
        }
    }


}
