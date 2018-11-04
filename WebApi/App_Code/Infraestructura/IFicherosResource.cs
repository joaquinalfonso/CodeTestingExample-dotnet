using System.Web;

namespace WebApi.Infraestructura
{

    // Interface que define las operaciones de almacenamiento en disco.

    public interface IFicherosResource
    {
        bool ExisteFicheroTranscritoTxt(int id);
        string ObtenerFicheroTranscritoTxt(int id);
        void GrabarFicheroTextoTranscrito(int id, string textoTranscrito);
        byte[] ObtenerFicheroMp3(int id);
        void GrabarFicheroMp3(HttpPostedFile postedFile, int idTranscripcion);
    }
}