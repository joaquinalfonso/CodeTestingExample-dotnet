using Quartz;
using System.Threading.Tasks;

namespace WebApiCrud.Controllers
{
    public class CronTareaDia : IJob
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private ITranscripcionesBO bo;

        public CronTareaDia()
        {
            this.bo = new TranscripcionesBO();
        }

        public CronTareaDia(ITranscripcionesBO transcripcionesBO)
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