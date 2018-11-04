using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Tests.Mocks;
using WebApi.Models;

namespace WebApi.Negocio.Tests
{
    [TestClass]
    public class TestTranscripcionesBO
    {
        private List<Transcripcion> datos = new List<Transcripcion>
            {
                new Transcripcion { Id = 1, FechaHoraRecepcion = new DateTime(2018, 11, 1, 8, 30, 0), LoginUsuario = "Usuario1", NombreArchivo = "Archivo1.mp3", Estado = TipoEstadoTranscripcion.PENDIENTE.ToString() },
                new Transcripcion { Id = 2, FechaHoraRecepcion = new DateTime(2018, 11, 2, 8, 30, 0), LoginUsuario = "Usuario1", NombreArchivo = "Archivo2.mp3", Estado = TipoEstadoTranscripcion.PENDIENTE.ToString() },
                new Transcripcion { Id = 3, FechaHoraRecepcion = new DateTime(2018, 11, 3, 8, 30, 0), LoginUsuario = "Usuario2", NombreArchivo = "Archivo3.mp3", Estado = TipoEstadoTranscripcion.PENDIENTE.ToString() },
            };

        private Mock<VocaliEntities> ObtenerDBMock()
        {
            var data = datos.AsQueryable();

            var mockSet = new Mock<DbSet<Transcripcion>>();
            mockSet.As<IQueryable<Transcripcion>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Transcripcion>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Transcripcion>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Transcripcion>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<VocaliEntities>();
            mockContext.Setup(c => c.Transcripciones).Returns(mockSet.Object);

            return mockContext;
        }
        

        [TestMethod]
        public void ObtenerNuevoIdTranscripcion_OK()
        {
            var bo = new TranscripcionesBO(ObtenerDBMock().Object);
            int id = bo.ObtenerNuevoIdTranscripcion();
                
            Assert.AreEqual(id, 4);
        }
    }
}
