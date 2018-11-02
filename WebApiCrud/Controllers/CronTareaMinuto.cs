using Quartz;
using System.Threading.Tasks;

namespace WebApiCrud.Controllers
{
    public class CronTareaMinuto : IJob
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private ITranscripcionesBO bo;

        public CronTareaMinuto()
        {
            this.bo = new TranscripcionesBO();
        }

        public CronTareaMinuto(ITranscripcionesBO transcripcionesBO)
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