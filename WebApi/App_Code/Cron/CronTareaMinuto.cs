using Quartz;
using System.Threading.Tasks;
using WebApi.Negocio;

namespace WebApi.Cron
{
    public class CronTareaMinuto : IJob
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private ITranscripcionesService bo;

        public CronTareaMinuto()
        {
            this.bo = new TranscripcionesService();
        }

        public CronTareaMinuto(ITranscripcionesService transcripcionesBO)
        {
            this.bo = transcripcionesBO;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await Task.Run(() =>
            {
                //logger.Trace("Tarea minuto");
            });
        }
    }

}