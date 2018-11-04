using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WebApi.INVOX
{

    // Mockup que simula el servicio de transcripcion INVOXMedical

    public class INVOXMedicalMock
    {

        private List<string> FicherosTextoPredefinidos;
        private int probabilidadError;

        public INVOXMedicalMock(int probabilidadError = 5) // 5%
        {
            string rutaFicherosTextoPredefinidos = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/INVOXMockTxtPredefinidos/");

            FicherosTextoPredefinidos = ListaFicherosDeCarpeta(rutaFicherosTextoPredefinidos);
            this.probabilidadError = probabilidadError;

        }

        public string TranscribirFicheroMp3(string login, byte[] ficheroMp3)
        {
            ProvocarErrorAleatorio(probabilidadError);
            int numeroAleatorio = ObtenerNumeroAleatorio(FicherosTextoPredefinidos.Count);
            return ObtenerFichero(FicherosTextoPredefinidos[numeroAleatorio]);
        }

        private void ProvocarErrorAleatorio(int probabilidadError)
        {
            Random r = new Random(DateTime.Now.Millisecond);

            int numeroAleatorio = r.Next(100);

            if (numeroAleatorio < probabilidadError)
                throw new Exception("Error al hacer la transcripción");
        }

        private int ObtenerNumeroAleatorio(int numeroMaximo)
        {
            Random r = new Random(DateTime.Now.Millisecond);

            int numeroAleatorio = r.Next(numeroMaximo);

            return numeroAleatorio;
        }

        private string ObtenerFichero(string rutaFichero)
        {
            string contenidoFicheroTxt = "";
            using (StreamReader sr = new StreamReader(rutaFichero, System.Text.Encoding.UTF8, false))
            {
                contenidoFicheroTxt = sr.ReadToEnd().Replace("\r\n", " ");
            }
            return contenidoFicheroTxt;
        }

        private static List<string> ListaFicherosDeCarpeta(string rutaCarpeta)
        {
            List<string> listaFicheros = new List<string>();

            DirectoryInfo directory = new DirectoryInfo(rutaCarpeta);
            FileInfo[] ficheros = directory.GetFiles("*.txt");

            ficheros.ToList().ForEach(fichero => listaFicheros.Add(fichero.FullName));


            return listaFicheros;
        }


    }
}