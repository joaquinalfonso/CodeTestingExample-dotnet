using Quartz;
using System.Threading.Tasks;
using WebApi.Negocio;

namespace WebApi.Cron
{
    public class CronTareaDia : IJob
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private ITranscripcionesService bo;

        public CronTareaDia()
        {
            this.bo = new TranscripcionesService();
        }

        public CronTareaDia(ITranscripcionesService transcripcionesBO)
        {
            this.bo = transcripcionesBO;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await Task.Run(() =>
            {
                logger.Info("Ejecutando tarea diaria");
                bo.ProcesarTranscripcionesPendientes();
            });
        }
    }
}