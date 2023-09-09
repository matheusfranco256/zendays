using Quartz;
using Quartz.Impl;
using ZenDays.Service.Services;

namespace ZenDays.API.Quartz
{
    public class QuartzInitializer
    {
        public static async Task Initialize()
        {
            // Crie uma instância do Scheduler
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            IScheduler scheduler = await schedulerFactory.GetScheduler();

            // Inicie o Scheduler
            await scheduler.Start();

            // Crie uma tarefa de verificação de férias
            IJobDetail verificaFeriasJob = JobBuilder.Create<VerificaFeriasjob>()
                .WithIdentity("verificaFeriasJob", "jobGroup")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("verificaFeriasJob", "jobGroup")
            .StartNow()
            .WithDailyTimeIntervalSchedule(builder =>
                builder.WithIntervalInHours(24)
                       .OnEveryDay()
                       .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(0, 0))
            )
            .Build();

            // Agende a tarefa com o trigger
            await scheduler.ScheduleJob(verificaFeriasJob, trigger);
        }
    }
}
