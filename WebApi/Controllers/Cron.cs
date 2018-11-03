

using System;
using System.Collections.Specialized;
using System.Threading.Tasks;

using Quartz;
using Quartz.Impl;


namespace WebApi.Controllers
{
    public class Cron
    {

        private static async Task IniciarYProgramar()
        {
            try
            {
                // Grab the Scheduler instance from the Factory
                IScheduler scheduler = await ObtenerScheduler();

                IJobDetail tareaMinuto = JobBuilder.Create<CronTareaMinuto>().Build();
                IJobDetail tareaHora = JobBuilder.Create<CronTareaHora>().Build();
                IJobDetail tareaDia = JobBuilder.Create<CronTareaDia>().Build();

                ITrigger triggerMinuto = ObtenerTriggerMinuto();
                ITrigger triggerHora = ObtenerTriggerHora();
                ITrigger triggerDia = ObtenerTriggerDia(0, 0);

                // Iniciar 
                await scheduler.Start();
                // Programar las tareas
                await scheduler.ScheduleJob(tareaMinuto, triggerMinuto);
                await scheduler.ScheduleJob(tareaHora, triggerHora);
                await scheduler.ScheduleJob(tareaDia, triggerDia);

                // and last shut down the scheduler when you are ready to close your program
                //await scheduler.Shutdown();
            }
            catch (SchedulerException se)
            {
                await Console.Error.WriteLineAsync(se.ToString());
            }
        }

        private static async Task<IScheduler> ObtenerScheduler()
        {
            NameValueCollection props = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" }
                };
            StdSchedulerFactory factory = new StdSchedulerFactory(props);
            IScheduler scheduler = await factory.GetScheduler();
            return scheduler;
        }

        private static ITrigger ObtenerTriggerDia(int hora, int minuto)
        {
            return TriggerBuilder.Create()
                                .WithIdentity("triggerDiario", "group1")
                                 .WithDailyTimeIntervalSchedule
                                   (s =>
                                      s.WithIntervalInHours(24)
                                     .OnEveryDay()
                                     .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(hora, minuto))
                                   )
                                 .Build();
        }

        private static ITrigger ObtenerTriggerHora()
        {
            return TriggerBuilder.Create()
                                .WithIdentity("triggerHora", "group1")
                                .StartNow()
                                .WithSimpleSchedule(x => x
                                    .WithIntervalInHours(1)
                                    .RepeatForever())
                                .Build();
        }

        private static ITrigger ObtenerTriggerMinuto()
        {
            return TriggerBuilder.Create()
                                .WithIdentity("triggerMinuto", "group1")
                                .StartNow()
                                .WithSimpleSchedule(x => x
                                    .WithIntervalInMinutes(1)
                                    .RepeatForever())
                                .Build();
        }

        public static void Iniciar()
        {
            IniciarYProgramar().GetAwaiter().GetResult();
        }

    }




}
