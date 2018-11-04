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

        private List<Transcripcion> transcripciones;


        public CacheTranscripcionesPendientes(List<Transcripcion> transcripciones)
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


        public Transcripcion ObtenerSiguienteTranscripcionPendiente()
        {
            bloqueo.EnterWriteLock();

            Transcripcion transcripcion = null;

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