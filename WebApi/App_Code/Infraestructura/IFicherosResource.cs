using System.Web;

namespace WebApi.Infraestructura
{

    // Interface que define las operaciones de almacenamiento en disco.

    public interface IFicherosResource
    {
        bool ExisteFicheroTranscritoTxt(string nombreFichero);
        string ObtenerFicheroTranscritoTxt(string nombreFichero);
        void GrabarFicheroTextoTranscrito(string nombreFichero, string textoTranscrito);
        byte[] ObtenerFicheroMp3(string nombreFichero);
        void GrabarFicheroMp3(HttpPostedFile postedFile, string nombreFichero);
    }
}