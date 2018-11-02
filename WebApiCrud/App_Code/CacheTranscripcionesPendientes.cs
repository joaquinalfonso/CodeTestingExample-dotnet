using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WebApiCrud.Models;

namespace WebApiCrud.App_Code
{
    public class CacheTranscripcionesPendientes
    {
        private ReaderWriterLockSlim bloqueo = new ReaderWriterLockSlim();

        private List<Transcripciones> transcripciones;


        public CacheTranscripcionesPendientes(List<Transcripciones> transcripciones)
        {
            this.transcripciones = transcripciones;
        }

        public int Count
        {
            get
            {
                bloqueo.EnterReadLock();
                try
                {
                    return transcripciones.Count;
                }
                finally
                {
                    bloqueo.ExitReadLock();
                }

            }
        }

        private int Pendientes
        {
            get
            {
                return transcripciones.Count(x => x.Estado == TipoEstadoTranscripcion.PENDIENTE.ToString());
            }
        }

        public bool HayTranscripcionesPendientes
        {
            get
            {
                bloqueo.EnterWriteLock();

                try
                {
                    return Pendientes > 0;
                }
                finally
                {
                    bloqueo.ExitWriteLock();
                }


            }
        }


        public Transcripciones ObtenerSiguienteTranscripcionPendiente()
        {
            bloqueo.EnterWriteLock();

            Transcripciones transcripcion = null;

            try
            {
                transcripcion = transcripciones
                    .FindAll(x => x.Estado == TipoEstadoTranscripcion.PENDIENTE.ToString())
                    .OrderBy(x => x.FechaHoraRecepcion)
                    .FirstOrDefault();

                if (transcripcion != null)
                    transcripcion.Estado = TipoEstadoTranscripcion.EN_PROGRESO.ToString();

                return transcripcion;
            }
            finally
            {
                bloqueo.ExitWriteLock();
            }

        }



        ~CacheTranscripcionesPendientes()
        {
            if (bloqueo != null) bloqueo.Dispose();
        }
    }
}