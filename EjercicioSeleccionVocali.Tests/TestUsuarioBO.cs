using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApi.Negocio;

namespace VocaliMp3Api.Controllers.Tests
{
    [TestClass()]
    public class TestUsuarioBO
    {
        [TestMethod]
        public void EsUsuarioValido_UsuarioValido()
        {
            Assert.IsTrue(new UsuarioBO().EsUsuarioValido("Usuario"));
        }

        [TestMethod]
        public void EsUsuarioValido_UsuarioVacioNoValido()
        {
            Assert.IsFalse(new UsuarioBO().EsUsuarioValido(""));
        }

        [TestMethod]
        public void EsUsuarioValido_UsuarioNullNoValido()
        {
            Assert.IsFalse(new UsuarioBO().EsUsuarioValido(null));
        }
    }
}