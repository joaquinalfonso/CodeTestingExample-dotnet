using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WebApi.Infraestructura;

namespace Tests.Mocks
{

    // Mock para testing que implementa la capa de operaciones de almacenamiento en disco.
    // Implementa la interface IFicherosResource

    public class FicherosResourceMock : IFicherosResource
    {
        public bool ExisteFicheroTranscritoTxt(string nombreFichero)
        {
            throw new NotImplementedException();
        }

        public void GrabarFicheroMp3(HttpPostedFile postedFile, string nombreFichero)
        {
            throw new NotImplementedException();
        }

        public void GrabarFicheroTextoTranscrito(string nombreFichero, string textoTranscrito)
        {
            throw new NotImplementedException();
        }

        public byte[] ObtenerFicheroMp3(string nombreFichero)
        {
            throw new NotImplementedException();
        }

        public string ObtenerFicheroTranscritoTxt(string nombreFichero)
        {
            throw new NotImplementedException();
        }
    }
}
