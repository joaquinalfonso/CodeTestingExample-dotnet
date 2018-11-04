using Quartz;
using System.Threading.Tasks;
using WebApi.Negocio;

namespace WebApi.Cron
{
    public class CronTareaHora : IJob
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private ITranscripcionesBO bo;

        public CronTareaHora()
        {
            this.bo = new TranscripcionesBO();
        }

        public CronTareaHora(ITranscripcionesBO transcripcionesBO)
        {
            this.bo = transcripcionesBO;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await Task.Run(() =>
            {
                logger.Trace("Ejecutando tarea hora");
                bo.ProcesarTranscripcionesPendientes();
            });
        }
    }

}