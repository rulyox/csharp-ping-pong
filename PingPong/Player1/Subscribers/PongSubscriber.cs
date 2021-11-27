using System.Threading.Tasks;
using PingPong.Machine;
using PingPong.Player1.Events;
using PingPong.Player1.Jobs;
using Quartz;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace PingPong.Player1.Subscribers
{
    public class PongSubscriber: ILocalEventHandler<PongEvent>, ITransientDependency
    {
        public async Task HandleEventAsync(PongEvent eventData)
        {
            IJobDetail job = JobBuilder.Create<PongJob>()
                .WithIdentity("job_" + eventData.Id, "pong")
                .UsingJobData("id", eventData.Id.ToString())
                .RequestRecovery()
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger_" + eventData.Id, "pong")
                .StartNow()
                .WithSimpleSchedule()
                .Build();
            
            await Common.Scheduler.ScheduleJob(job, trigger);
        }
    }
}
