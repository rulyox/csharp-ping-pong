using System.Threading.Tasks;
using PingPong.Machine;
using PingPong.Player2.Events;
using PingPong.Player2.Jobs;
using Quartz;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace PingPong.Player2.Subscribers
{
    public class PingSubscriber: ILocalEventHandler<PingEvent>, ITransientDependency
    {
        public async Task HandleEventAsync(PingEvent eventData)
        {
            IJobDetail job = JobBuilder.Create<PingJob>()
                .WithIdentity("job_" + eventData.Id, "ping")
                .UsingJobData("id", eventData.Id.ToString())
                .RequestRecovery()
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger_" + eventData.Id, "ping")
                .StartNow()
                .WithSimpleSchedule()
                .Build();
            
            await Common.Scheduler.ScheduleJob(job, trigger);
        }
    }
}
