using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Tests.Mocks;
using WebApi.Models;
using WebApi.Servicios;

namespace WebApi.Servicios.Tests
{
    [TestClass]
    public class TestTranscripcionesService
    {
        private List<Transcription> datos = new List<Transcription>
            {
                new Transcription { Id = 1, FechaHoraRecepcion = new DateTime(2018, 11, 1, 8, 30, 0), LoginUsuario = "Usuario1", NombreArchivo = "Archivo1.mp3", Estado = (int)TipoEstadoTranscripcion.PENDIENTE },
                new Transcription { Id = 2, FechaHoraRecepcion = new DateTime(2018, 11, 2, 8, 30, 0), LoginUsuario = "Usuario1", NombreArchivo = "Archivo2.mp3", Estado = (int)TipoEstadoTranscripcion.PENDIENTE },
                new Transcription { Id = 3, FechaHoraRecepcion = new DateTime(2018, 11, 3, 8, 30, 0), LoginUsuario = "Usuario2", NombreArchivo = "Archivo3.mp3", Estado = (int)TipoEstadoTranscripcion.PENDIENTE },
            };

        private Mock<WebApiDBContext> ObtenerDBMock()
        {
            var data = datos.AsQueryable();

            var mockSet = new Mock<DbSet<Transcription>>();
            mockSet.As<IQueryable<Transcription>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Transcription>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Transcription>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Transcription>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<WebApiDBContext>();
            mockContext.Setup(c => c.Transcriptions).Returns(mockSet.Object);

            return mockContext;
        }
        
        
    }
}
