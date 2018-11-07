using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WebApi.Models;

namespace WebApi.Servicios
{

    // Clase que almacena una lista de transcripciones pendientes de procesar.
    // Sirve transcripciones a los procesos concurrentes con bloqueo de lectura.

    public class CacheTranscripcionesPendientes
    {
        private ReaderWriterLockSlim bloqueo = new ReaderWriterLockSlim();

        private List<Transcription> transcripciones;


        public CacheTranscripcionesPendientes(List<Transcription> transcripciones)
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
                return transcripciones.Count(x => x.Estado == (int)TipoEstadoTranscripcion.PENDIENTE);
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


        public Transcription ObtenerSiguienteTranscripcionPendiente()
        {
            bloqueo.EnterWriteLock();

            Transcription transcripcion = null;

            try
            {
                transcripcion = transcripciones
                    .FindAll(x => x.Estado == (int)TipoEstadoTranscripcion.PENDIENTE)
                    .OrderBy(x => x.FechaHoraRecepcion)
                    .FirstOrDefault();

                if (transcripcion != null)
                    transcripcion.Estado = (int)TipoEstadoTranscripcion.EN_PROGRESO;

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