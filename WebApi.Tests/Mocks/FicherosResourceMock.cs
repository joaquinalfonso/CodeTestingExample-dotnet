using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WebApi.Infraestructura;

namespace Tests.Mocks
{
    public class FicherosResourceMock : IFicherosResource
    {
        public bool ExisteFicheroTranscritoTxt(int id)
        {
            throw new NotImplementedException();
        }

        public void GrabarFicheroMp3(HttpPostedFile postedFile, int idTranscripcion)
        {
            throw new NotImplementedException();
        }

        public void GrabarFicheroTextoTranscrito(int id, string textoTranscrito)
        {
            throw new NotImplementedException();
        }

        public byte[] ObtenerFicheroMp3(int id)
        {
            throw new NotImplementedException();
        }

        public string ObtenerFicheroTranscritoTxt(int id)
        {
            throw new NotImplementedException();
        }
    }
}
