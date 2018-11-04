using Quartz;
using System.Threading.Tasks;
using WebApi.Servicios;

namespace WebApi.Cron
{
    // Clase para definir la tarea que ejecuta el cron diariamente.

    public class CronTareaDia : IJob
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public ITranscripcionesService transcripcionesService { private get; set; }

        public CronTareaDia()
        {
            this.transcripcionesService = new TranscripcionesService();
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await Task.Run(() =>
            {
                logger.Info("Ejecutando tarea diaria");
                transcripcionesService.ProcesarTranscripcionesPendientes();
            });
        }
    }
}