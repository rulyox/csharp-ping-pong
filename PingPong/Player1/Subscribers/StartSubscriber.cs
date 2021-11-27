using System.Threading.Tasks;
using PingPong.Machine;
using PingPong.Player1.Events;
using PingPong.Player1.Jobs;
using Quartz;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace PingPong.Player1.Subscribers
{
    public class StartSubscriber: ILocalEventHandler<StartEvent>, ITransientDependency
    {
        public async Task HandleEventAsync(StartEvent eventData)
        {
            IJobDetail job = JobBuilder.Create<StartJob>()
                .WithIdentity("job_start", "group")
                .RequestRecovery()
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger_start", "group")
                .StartNow()
                .WithSimpleSchedule()
                .Build();
            
            await Common.Scheduler.ScheduleJob(job, trigger);
        }
    }
}
