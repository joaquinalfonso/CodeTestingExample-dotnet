using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApi.Controllers;
using Tests.Mocks;
using WebApi.Negocio;
using System.Web.Http;
using System.Net;
using System.Globalization;
using System.Threading;
using System.Web.Hosting;
using System.IO;
using WebApi.Comun;

namespace WebApi.Controller.Tests
{

    [TestClass]
    public class TestTranscripcionesController
    {
        private ITranscripcionesBO GetTranscripcionesBOMock()
        {
            return new TranscripcionesBOMock();
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

        #region CU1 PostTranscripcion

        [TestMethod]
        public void PostTranscripcion_SinHeaderLogin()
        {
            TranscripcionesController controller = new TranscripcionesController(GetTranscripcionesBOMock());

            ((ApiController)controller).Request = new HttpRequestMessage();
            ((ApiController)controller).Request.SetConfiguration(new HttpConfiguration());

            HttpResponseMessage respuesta = controller.PostTranscripcion().GetAwaiter().GetResult(); ;

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.Unauthorized);

        }

        [TestMethod]
        public void PostTranscripcion_LoginVacio()
        {
            TranscripcionesController controller = new TranscripcionesController(GetTranscripcionesBOMock());

            ((ApiController)controller).Request = new HttpRequestMessage();
            ((ApiController)controller).Request.SetConfiguration(new HttpConfiguration());

            ((ApiController)controller).Request.Headers.Add("Login", "");

            HttpResponseMessage respuesta = controller.PostTranscripcion().GetAwaiter().GetResult(); ;

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.Unauthorized);

        }

        [TestMethod]
        public void PostTranscripcion_LoginOKSinFichero()
        {
            TranscripcionesController controller = new TranscripcionesController(GetTranscripcionesBOMock());

            ((ApiController)controller).Request = new HttpRequestMessage();
            ((ApiController)controller).Request.SetConfiguration(new HttpConfiguration());

            ((ApiController)controller).Request.Headers.Add("Login", "Usr1");

            HttpResponseMessage respuesta = controller.PostTranscripcion().GetAwaiter().GetResult(); ;

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.BadRequest);

        }

        [TestMethod]
        public void PostTranscripcion_LoginOKSinFicheroMp3()
        {
            TranscripcionesController controller = new TranscripcionesController(GetTranscripcionesBOMock());

            ((ApiController)controller).Request = new HttpRequestMessage();
            ((ApiController)controller).Request.SetConfiguration(new HttpConfiguration());

            ((ApiController)controller).Request.Headers.Add("Login", "Usr1");

            //HttpContext.Current

            //((ApiController)controller).Request.Content.

            HttpResponseMessage respuesta = controller.PostTranscripcion().GetAwaiter().GetResult(); ;

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.BadRequest);

        }

        #endregion


        #region CU2 GetTranscripciones

        [TestMethod]
        public void GetTranscripciones_SinHeaderLogin()
        {
            TranscripcionesController controller = new TranscripcionesController(GetTranscripcionesBOMock());

            ((ApiController)controller).Request = new HttpRequestMessage();
            ((ApiController)controller).Request.SetConfiguration(new HttpConfiguration());

            HttpResponseMessage respuesta = controller.GetTranscripciones();

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.Unauthorized);

        }

        [TestMethod]
        public void GetTranscripciones_LoginVacio()
        {
            TranscripcionesController controller = new TranscripcionesController(GetTranscripcionesBOMock());

            ((ApiController)controller).Request = new HttpRequestMessage();
            ((ApiController)controller).Request.SetConfiguration(new HttpConfiguration());

            ((ApiController)controller).Request.Headers.Add("Login", "");

            HttpResponseMessage respuesta = controller.GetTranscripciones();

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.Unauthorized);

        }

        [TestMethod]
        public void GetTranscripciones_CadenaDesdeNullCadenaHastaNull()
        {
            TranscripcionesController controller = new TranscripcionesController(GetTranscripcionesBOMock());

            ((ApiController)controller).Request = new HttpRequestMessage();
            ((ApiController)controller).Request.SetConfiguration(new HttpConfiguration());

            ((ApiController)controller).Request.Headers.Add("Login", "Usr1");

            HttpResponseMessage respuesta = controller.GetTranscripciones(null, null);

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.BadRequest);

        }

        [TestMethod]
        public void GetTranscripciones_CadenaDesdeNull()
        {
            TranscripcionesController controller = new TranscripcionesController(GetTranscripcionesBOMock());

            ((ApiController)controller).Request = new HttpRequestMessage();
            ((ApiController)controller).Request.SetConfiguration(new HttpConfiguration());

            ((ApiController)controller).Request.Headers.Add("Login", "Usr1");

            HttpResponseMessage respuesta = controller.GetTranscripciones(null, "");

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.BadRequest);

        }

        [TestMethod]
        public void GetTranscripciones_CadenaHastaNull()
        {
            TranscripcionesController controller = new TranscripcionesController(GetTranscripcionesBOMock());

            ((ApiController)controller).Request = new HttpRequestMessage();
            ((ApiController)controller).Request.SetConfiguration(new HttpConfiguration());

            ((ApiController)controller).Request.Headers.Add("Login", "Usr1");

            HttpResponseMessage respuesta = controller.GetTranscripciones("", null);

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.BadRequest);

        }

        [TestMethod]
        public void GetTranscripciones_CadenaDesdeVaciaCadenaHastaVacia()
        {
            TranscripcionesController controller = new TranscripcionesController(GetTranscripcionesBOMock());

            ((ApiController)controller).Request = new HttpRequestMessage();
            ((ApiController)controller).Request.SetConfiguration(new HttpConfiguration());

            ((ApiController)controller).Request.Headers.Add("Login", "Usr1");

            HttpResponseMessage respuesta = controller.GetTranscripciones("", "");

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.OK);

        }

        [TestMethod]
        public void GetTranscripciones_CadenaDesdeIncorrecta()
        {
            TranscripcionesController controller = new TranscripcionesController(GetTranscripcionesBOMock());

            ((ApiController)controller).Request = new HttpRequestMessage();
            ((ApiController)controller).Request.SetConfiguration(new HttpConfiguration());

            ((ApiController)controller).Request.Headers.Add("Login", "Usr1");

            HttpResponseMessage respuesta = controller.GetTranscripciones("xxxxxx");

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.BadRequest);

        }

        [TestMethod]
        public void GetTranscripciones_CadenaHastaIncorrecta()
        {
            TranscripcionesController controller = new TranscripcionesController(GetTranscripcionesBOMock());

            ((ApiController)controller).Request = new HttpRequestMessage();
            ((ApiController)controller).Request.SetConfiguration(new HttpConfiguration());

            ((ApiController)controller).Request.Headers.Add("Login", "Usr1");

            HttpResponseMessage respuesta = controller.GetTranscripciones("", "xxxxxx");

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.BadRequest);

        }

        [TestMethod]
        public void GetTranscripciones_LoginOK()
        {
            TranscripcionesController controller = new TranscripcionesController(GetTranscripcionesBOMock());

            ((ApiController)controller).Request = new HttpRequestMessage();
            ((ApiController)controller).Request.SetConfiguration(new HttpConfiguration());

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
            TranscripcionesController controller = new TranscripcionesController(GetTranscripcionesBOMock());

            ((ApiController)controller).Request = new HttpRequestMessage();
            ((ApiController)controller).Request.SetConfiguration(new HttpConfiguration());

            ((ApiController)controller).Request.Headers.Add("Login", "Usr1");

            HttpResponseMessage respuesta = controller.GetTranscripciones("01-01-2010");

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.BadRequest);

        }

        [TestMethod]
        public void GetTranscripciones_FechaHastaErronea()
        {
            TranscripcionesController controller = new TranscripcionesController(GetTranscripcionesBOMock());

            ((ApiController)controller).Request = new HttpRequestMessage();
            ((ApiController)controller).Request.SetConfiguration(new HttpConfiguration());

            ((ApiController)controller).Request.Headers.Add("Login", "Usr1");

            HttpResponseMessage respuesta = controller.GetTranscripciones("", "01-01-2010");

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.BadRequest);

        }

        [TestMethod]
        public void GetTranscripciones_FechaDesdeOK()
        {
            TranscripcionesController controller = new TranscripcionesController(GetTranscripcionesBOMock());

            ((ApiController)controller).Request = new HttpRequestMessage();
            ((ApiController)controller).Request.SetConfiguration(new HttpConfiguration());

            ((ApiController)controller).Request.Headers.Add("Login", "Usr1");

            HttpResponseMessage respuesta = controller.GetTranscripciones("2018-01-01T08:30");

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.OK);

        }

        [TestMethod]
        public void GetTranscripciones_FechaHastaOK()
        {
            TranscripcionesController controller = new TranscripcionesController(GetTranscripcionesBOMock());

            ((ApiController)controller).Request = new HttpRequestMessage();
            ((ApiController)controller).Request.SetConfiguration(new HttpConfiguration());

            ((ApiController)controller).Request.Headers.Add("Login", "Usr1");

            HttpResponseMessage respuesta = controller.GetTranscripciones("", "2018-01-01T08:30");

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.OK);

        }

        #endregion

        #region CU3 GetTranscripcion

        [TestMethod]
        public void GetTranscripcion_SinHeaderLogin()
        {
            TranscripcionesController controller = new TranscripcionesController(GetTranscripcionesBOMock());

            ((ApiController)controller).Request = new HttpRequestMessage();
            ((ApiController)controller).Request.SetConfiguration(new HttpConfiguration());

            HttpResponseMessage respuesta = controller.GetTranscripcion(0);

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.Unauthorized);

        }

        [TestMethod]
        public void GetTranscripcion_LoginVacio()
        {
            TranscripcionesController controller = new TranscripcionesController(GetTranscripcionesBOMock());

            ((ApiController)controller).Request = new HttpRequestMessage();
            ((ApiController)controller).Request.SetConfiguration(new HttpConfiguration());

            ((ApiController)controller).Request.Headers.Add("Login", "");

            HttpResponseMessage respuesta = controller.GetTranscripcion(0);

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.Unauthorized);

        }

        [TestMethod]
        public void GetTranscripcion_LoginOKTranscripcionNoEncontrada()
        {
            TranscripcionesController controller = new TranscripcionesController(GetTranscripcionesBOMock());

            ((ApiController)controller).Request = new HttpRequestMessage();
            ((ApiController)controller).Request.SetConfiguration(new HttpConfiguration());

            ((ApiController)controller).Request.Headers.Add("Login", "Usr1");

            HttpResponseMessage respuesta = controller.GetTranscripcion(-1);
            
            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.NotFound);

        }

        [TestMethod]
        public void GetTranscripcion_LoginOKTranscripcionPendiente()
        {
            TranscripcionesController controller = new TranscripcionesController(GetTranscripcionesBOMock());

            ((ApiController)controller).Request = new HttpRequestMessage();
            ((ApiController)controller).Request.SetConfiguration(new HttpConfiguration());

            ((ApiController)controller).Request.Headers.Add("Login", "Usr1");

            HttpResponseMessage respuesta = controller.GetTranscripcion(-2);

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.BadRequest);

        }

        [TestMethod]
        public void GetTranscripcion_LoginOKTranscripcionErronea()
        {
            TranscripcionesController controller = new TranscripcionesController(GetTranscripcionesBOMock());

            ((ApiController)controller).Request = new HttpRequestMessage();
            ((ApiController)controller).Request.SetConfiguration(new HttpConfiguration());

            ((ApiController)controller).Request.Headers.Add("Login", "Usr1");

            HttpResponseMessage respuesta = controller.GetTranscripcion(-3);

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.BadRequest);

        }

        [TestMethod]
        public void GetTranscripcion_LoginOKTranscripcionFinalizada()
        {
            TranscripcionesController controller = new TranscripcionesController(GetTranscripcionesBOMock());

            ((ApiController)controller).Request = new HttpRequestMessage();
            ((ApiController)controller).Request.SetConfiguration(new HttpConfiguration());

            ((ApiController)controller).Request.Headers.Add("Login", "Usr1");

            HttpResponseMessage respuesta = controller.GetTranscripcion(0);

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.OK);

        }

        #endregion

        //[TestMethod]
        //public async Task GetAllTranscripcionesAsync_DebeDevolverTodasTranscripciones()
        //{
        //    var testTranscripciones = GetTestTranscripciones();
        //    var controller = new TranscripcionesController(testTranscripciones);

        //    var result = await controller.GetAllTranscripcionesAsync() as List<Transcripcion>;
        //    Assert.AreEqual(testTranscripciones.Count, result.Count);
        //}

        /*
        [TestMethod]
        public void GetTranscripcion_DebeDevolverTranscripcionCorrecta()
        {
            var testTranscripciones = GetTestTranscripciones();
            var controller = new TranscripcionesController(testTranscripciones);

            var result = controller.GetTranscripcion("T1") as OkNegotiatedContentResult<Transcripcion>;
            Assert.IsNotNull(result);
            Assert.AreEqual(testTranscripciones[0].Id, result.Content.Id);
        }
        */

        //[TestMethod]
        //public async Task GetTranscripcionAsync_DebeDevolverTranscripcionCorrecta()
        //{
        //    var testTranscripciones = GetTestTranscripciones();
        //    var controller = new TranscripcionesController(testTranscripciones);

        //    var result = await controller.GetTranscripcionAsync("T1") as OkNegotiatedContentResult<Transcripcion>;
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(testTranscripciones[0].Id, result.Content.Id);
        //}

        /*
    [TestMethod]
    public void GetTranscripcion_DebeNoEncontrarTranscripcion()
    {
        var controller = new TranscripcionesController(GetTestTranscripciones());

        var result = controller.GetTranscripcion("T999");
        Assert.IsInstanceOfType(result, typeof(NotFoundResult));
    }
    */

    }

}
