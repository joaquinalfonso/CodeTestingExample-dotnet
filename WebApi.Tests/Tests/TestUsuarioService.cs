using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApi.Servicios;

namespace WebApi.Servicios.Tests
{
    [TestClass()]
    public class TestUsuarioService
    {
        [TestMethod]
        public void EsUsuarioValido_UsuarioValido()
        {
            Assert.IsTrue(new UsuarioService().EsUsuarioValido("Usuario"));
        }

        [TestMethod]
        public void EsUsuarioValido_UsuarioVacioNoValido()
        {
            Assert.IsFalse(new UsuarioService().EsUsuarioValido(""));
        }

        [TestMethod]
        public void EsUsuarioValido_UsuarioNullNoValido()
        {
            Assert.IsFalse(new UsuarioService().EsUsuarioValido(null));
        }
    }
}