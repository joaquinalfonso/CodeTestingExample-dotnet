using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Hosting;

using Tests.Mocks;
using WebApi.Controllers;
using WebApi.Servicios;

namespace WebApi.Controller.Tests
{

    [TestClass]
    public class TestTranscripcionesController
    {
        private ITranscripcionesService ObtenerTranscripcionesServiceMock()
        {
            return new TranscripcionesServiceMock();
        }

        [TestInitialize]
        public void InicializaContexto()
        {
            string culture = "es-ES";
            CultureInfo newCultureInfo = CultureInfo.CreateSpecificCulture(culture);
            Thread.CurrentThread.CurrentUICulture = newCultureInfo;

            SimpleWorkerRequest request = new SimpleWorkerRequest("", "", "", null, new StringWriter());
            HttpContext context = new HttpContext(request);
            HttpContext.Current = context;

            
        }

        private TranscripcionesController ObtenerTranscripcionesController()
        {
            TranscripcionesController controller = new TranscripcionesController();
            controller.transcripcionesService = ObtenerTranscripcionesServiceMock();

            ((ApiController)controller).Request = new HttpRequestMessage();
            ((ApiController)controller).Request.SetConfiguration(new HttpConfiguration());

            return controller;
        }

        #region CU1 PostTranscripcion

        [TestMethod]
        public void PostTranscripcion_SinHeaderLogin()
        {
            TranscripcionesController controller = ObtenerTranscripcionesController();

            HttpResponseMessage respuesta = controller.PostTranscripcion().GetAwaiter().GetResult(); ;

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.Unauthorized);
        }

        [TestMethod]
        public void PostTranscripcion_LoginVacio()
        {
            TranscripcionesController controller = ObtenerTranscripcionesController();

            
            ((ApiController)controller).Request.Method = HttpMethod.Post;
            ((ApiController)controller).Request.Headers.Add("Login", "");

            HttpResponseMessage respuesta = controller.PostTranscripcion().GetAwaiter().GetResult(); ;

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.Unauthorized);

        }

        [TestMethod]
        public void PostTranscripcion_LoginOKSinFicheroMp3()
        {
            TranscripcionesController controller = ObtenerTranscripcionesController();

            ((ApiController)controller).Request.Method = HttpMethod.Post;
            ((ApiController)controller).Request.Headers.Add("Login", "Usr1");

            HttpResponseMessage respuesta = controller.PostTranscripcion().GetAwaiter().GetResult(); ;

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.BadRequest);

        }

        #endregion

        #region CU2 GetTranscripciones

        [TestMethod]
        public void GetTranscripciones_SinHeaderLogin()
        {
            TranscripcionesController controller = ObtenerTranscripcionesController();

            HttpResponseMessage respuesta = controller.GetTranscripciones();

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.Unauthorized);

        }

        [TestMethod]
        public void GetTranscripciones_LoginVacio()
        {
            TranscripcionesController controller = ObtenerTranscripcionesController();

            ((ApiController)controller).Request.Headers.Add("Login", "");

            HttpResponseMessage respuesta = controller.GetTranscripciones();

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.Unauthorized);

        }

        [TestMethod]
        public void GetTranscripciones_FechaDesdeNullFechaHastaNull()
        {
            TranscripcionesController controller = ObtenerTranscripcionesController();

            ((ApiController)controller).Request.Headers.Add("Login", "Usr1");

            HttpResponseMessage respuesta = controller.GetTranscripciones(null, null);

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.BadRequest);

        }

        [TestMethod]
        public void GetTranscripciones_FechaDesdeNull()
        {
            TranscripcionesController controller = ObtenerTranscripcionesController();

            ((ApiController)controller).Request.Headers.Add("Login", "Usr1");

            HttpResponseMessage respuesta = controller.GetTranscripciones(null, "");

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.BadRequest);

        }

        [TestMethod]
        public void GetTranscripciones_FechaHastaNull()
        {
            TranscripcionesController controller = ObtenerTranscripcionesController();

            ((ApiController)controller).Request.Headers.Add("Login", "Usr1");

            HttpResponseMessage respuesta = controller.GetTranscripciones("", null);

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.BadRequest);

        }

        [TestMethod]
        public void GetTranscripciones_FechaDesdeVaciaFechaHastaVacia()
        {
            TranscripcionesController controller = ObtenerTranscripcionesController();

            ((ApiController)controller).Request.Headers.Add("Login", "Usr1");

            HttpResponseMessage respuesta = controller.GetTranscripciones("", "");

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.OK);

        }

        [TestMethod]
        public void GetTranscripciones_FechaDesdeIncorrecta()
        {
            TranscripcionesController controller = ObtenerTranscripcionesController();

            ((ApiController)controller).Request.Headers.Add("Login", "Usr1");

            HttpResponseMessage respuesta = controller.GetTranscripciones("xxxxxx");

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.BadRequest);

        }

        [TestMethod]
        public void GetTranscripciones_FechaHastaIncorrecta()
        {
            TranscripcionesController controller = ObtenerTranscripcionesController();

            ((ApiController)controller).Request.Headers.Add("Login", "Usr1");

            HttpResponseMessage respuesta = controller.GetTranscripciones("", "xxxxxx");

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.BadRequest);

        }

        [TestMethod]
        public void GetTranscripciones_LoginOK()
        {
            TranscripcionesController controller = ObtenerTranscripcionesController();

            ((ApiController)controller).Request.Headers.Add("Login", "Usr1");

            HttpResponseMessage respuesta = controller.GetTranscripciones();

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.OK);

            //TODO
            //string lista = await respuesta.Content.ReadAsStringAsync();
            //ReadAsAsync<List<TranscripcionDTO>>()
            //Assert.AreEqual(()respuesta.Content), 3);

        }

        [TestMethod]
        public void GetTranscripciones_FechaDesdeErronea()
        {
            TranscripcionesController controller = ObtenerTranscripcionesController();

            ((ApiController)controller).Request.Headers.Add("Login", "Usr1");

            HttpResponseMessage respuesta = controller.GetTranscripciones("01-01-2010");

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.BadRequest);

        }

        [TestMethod]
        public void GetTranscripciones_FechaHastaErronea()
        {
            TranscripcionesController controller = ObtenerTranscripcionesController();

            ((ApiController)controller).Request.Headers.Add("Login", "Usr1");

            HttpResponseMessage respuesta = controller.GetTranscripciones("", "01-01-2010");

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.BadRequest);

        }

        [TestMethod]
        public void GetTranscripciones_FechaDesdeOK()
        {
            TranscripcionesController controller = ObtenerTranscripcionesController();

            ((ApiController)controller).Request.Headers.Add("Login", "Usr1");

            HttpResponseMessage respuesta = controller.GetTranscripciones("2018-01-01T08:30");

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.OK);

        }

        [TestMethod]
        public void GetTranscripciones_FechaHastaOK()
        {
            TranscripcionesController controller = ObtenerTranscripcionesController();

            ((ApiController)controller).Request.Headers.Add("Login", "Usr1");

            HttpResponseMessage respuesta = controller.GetTranscripciones("", "2018-01-01T08:30");

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.OK);

        }

        #endregion

        #region CU3 GetTranscripcion

        [TestMethod]
        public void GetTranscripcion_SinHeaderLogin()
        {
            TranscripcionesController controller = ObtenerTranscripcionesController();

            HttpResponseMessage respuesta = controller.GetTranscripcion(0);

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.Unauthorized);

        }

        [TestMethod]
        public void GetTranscripcion_LoginVacio()
        {
            TranscripcionesController controller = ObtenerTranscripcionesController();

            ((ApiController)controller).Request.Headers.Add("Login", "");

            HttpResponseMessage respuesta = controller.GetTranscripcion(0);

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.Unauthorized);

        }

        [TestMethod]
        public void GetTranscripcion_LoginOKTranscripcionNoEncontrada()
        {
            TranscripcionesController controller = ObtenerTranscripcionesController();

            ((ApiController)controller).Request.Headers.Add("Login", "Usr1");

            HttpResponseMessage respuesta = controller.GetTranscripcion(-1);
            
            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.NotFound);

        }

        [TestMethod]
        public void GetTranscripcion_LoginOKTranscripcionPendiente()
        {
            TranscripcionesController controller = ObtenerTranscripcionesController();

            ((ApiController)controller).Request.Headers.Add("Login", "Usr1");

            HttpResponseMessage respuesta = controller.GetTranscripcion(-2);

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.BadRequest);

        }

        [TestMethod]
        public void GetTranscripcion_LoginOKTranscripcionErronea()
        {
            TranscripcionesController controller = ObtenerTranscripcionesController();

            ((ApiController)controller).Request.Headers.Add("Login", "Usr1");

            HttpResponseMessage respuesta = controller.GetTranscripcion(-3);

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.BadRequest);

        }

        [TestMethod]
        public void GetTranscripcion_LoginOKTranscripcionFinalizada()
        {
            TranscripcionesController controller = ObtenerTranscripcionesController();

            ((ApiController)controller).Request.Headers.Add("Login", "Usr1");

            HttpResponseMessage respuesta = controller.GetTranscripcion(0);

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.OK);

        }

        #endregion
        
    }

}
