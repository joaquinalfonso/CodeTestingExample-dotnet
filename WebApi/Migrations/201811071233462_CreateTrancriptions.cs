namespace WebApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTrancriptions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Transcriptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FechaHoraRecepcion = c.DateTime(nullable: false),
                        LoginUsuario = c.String(),
                        NombreArchivo = c.String(),
                        Estado = c.Int(nullable: false),
                        FechaHoraTranscripcion = c.DateTime(),
                        MensajeError = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Transcriptions");
        }
    }
}
