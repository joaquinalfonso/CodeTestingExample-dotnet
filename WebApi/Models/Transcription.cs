using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
   

    public class Transcription
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime FechaHoraRecepcion { get; set; }
        public string LoginUsuario { get; set; }
        public string NombreArchivo { get; set; }
        public int Estado { get; set; }
        public DateTime? FechaHoraTranscripcion { get; set; }
        public string MensajeError { get; set; }
    }

    public class WebApiDBContext : DbContext
    {
        public WebApiDBContext()
            : base("name=WebApiDBContext")
        {
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder
        //    base.OnModelCreating(modelBuilder);
           
        //}

        //protected override void OnModelCreating(ModuleBuilder modelBuilder)
        //{
        //    modelBuilder.HasSequence<int>("OrderNumbers");
        //}

        public DbSet<Transcription> Transcriptions { get; set; }
    }

}