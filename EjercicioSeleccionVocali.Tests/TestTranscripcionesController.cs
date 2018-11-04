using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApi.Controllers;
using EjercicioSeleccionVocali.Tests.Mocks;
using WebApi.Negocio;
using System.Web.Http;
using System.Net;
using System.Globalization;
using System.Threading;
using System.Web.Hosting;
using System.IO;

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

        [TestMethod]
        public void GetAllTranscripciones_SinHeaderLoginDebeDevolverNoAutorizado()
        {
            TranscripcionesController controller = new TranscripcionesController(GetTranscripcionesBOMock());

            ((ApiController)controller).Request = new HttpRequestMessage();
            ((ApiController)controller).Request.SetConfiguration(new HttpConfiguration());

            HttpResponseMessage respuesta = controller.GetTranscripciones();

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.Unauthorized);
            
        }

        [TestMethod]
        public void GetAllTranscripciones_LoginVacioDebeDevolverNoAutorizado()
        {
            TranscripcionesController controller = new TranscripcionesController(GetTranscripcionesBOMock());

            ((ApiController)controller).Request = new HttpRequestMessage();
            ((ApiController)controller).Request.SetConfiguration(new HttpConfiguration());

            ((ApiController)controller).Request.Headers.Add("Login", "");

            HttpResponseMessage respuesta = controller.GetTranscripciones();

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.Unauthorized);

        }


        [TestMethod]
        public void GetAllTranscripciones_FormatoFechasIncorrecto()
        {
            TranscripcionesController controller = new TranscripcionesController(GetTranscripcionesBOMock());

            ((ApiController)controller).Request = new HttpRequestMessage();
            ((ApiController)controller).Request.SetConfiguration(new HttpConfiguration());

            ((ApiController)controller).Request.Headers.Add("Login", "Pepe");

            HttpResponseMessage respuesta = controller.GetTranscripciones("xxxxxx");

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.BadRequest);

        }

        [TestMethod]
        public void GetAllTranscripciones_OK()
        {
            TranscripcionesController controller = new TranscripcionesController(GetTranscripcionesBOMock());

            ((ApiController)controller).Request = new HttpRequestMessage();
            ((ApiController)controller).Request.SetConfiguration(new HttpConfiguration());

            ((ApiController)controller).Request.Headers.Add("Login", "Pepe");

            HttpResponseMessage respuesta = controller.GetTranscripciones();

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.OK);
            //Assert.AreEqual(respuesta.Content.Value.Count, 3);

        }



        [TestMethod]
        public void GetAllTranscripciones_LoginOKDebeDevolverNoAutorizado()
        {
            TranscripcionesController controller = new TranscripcionesController(GetTranscripcionesBOMock());

            ((ApiController)controller).Request = new HttpRequestMessage();
            ((ApiController)controller).Request.SetConfiguration(new HttpConfiguration());

            ((ApiController)controller).Request.Headers.Add("Login", "Juan");

            HttpResponseMessage respuesta = controller.GetTranscripciones();

            Assert.AreEqual(respuesta.StatusCode, HttpStatusCode.Unauthorized);

        }

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
