namespace Repositorio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Jugada",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Empresa = c.String(unicode: false),
                        Usuario = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Movimiento",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false, precision: 0),
                        PrecioUnitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PrecioNeto = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Cantidad = c.Int(nullable: false),
                        Tipo = c.Int(nullable: false),
                        Jugada_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Movimiento", "FK_dbo.Movimiento_dbo.Jugada_Jugada_Id");
            DropIndex("Movimiento", new[] { "Jugada_Id" });
            DropTable("Movimiento");
            DropTable("Jugada");
        }
    }
}
