using Quartz;
using System.Threading.Tasks;
using WebApi.Servicios;

namespace WebApi.Cron
{

    // Clase para definir la tarea que ejecuta el cron cada hora.

    public class CronTareaHora : IJob
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public ITranscripcionesService transcripcionesService { private get; set; }

        public CronTareaHora()
        {
            this.transcripcionesService = new TranscripcionesService();
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await Task.Run(() =>
            {
                logger.Trace("Ejecutando tarea hora");
                transcripcionesService.ProcesarTranscripcionesPendientes();
            });
        }
    }

}