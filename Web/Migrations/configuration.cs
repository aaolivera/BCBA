using System.Data.Entity.Migrations;
using Web.Models;

namespace Web.Migrations
{
    class Configuration
    
        : DbMigrationsConfiguration<ApplicationDbContext>

{
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            SetSqlGenerator("MySql.Data.MySqlClient", new MySql.Data.Entity.MySqlMigrationSqlGenerator());
            CodeGenerator = new MySql.Data.Entity.MySqlMigrationCodeGenerator(); //this line was missing, so now the migrations does not contains the prefix "dbo"

        }
    }
}
