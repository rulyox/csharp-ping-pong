using System;
using System.Threading;
using System.Threading.Tasks;
using PingPong.Player1.Jobs;
using Quartz;
using Quartz.Impl.Triggers;

namespace PingPong.Player1.Listeners
{
    public class PongListener: IJobListener
    {
        public string Name => "listener_pong";
        
        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken ct)
        {
            return Task.FromResult<object>(null);
        }

        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken ct)
        {
            return Task.FromResult<object>(null);
        }

        public async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException e, CancellationToken ct)
        {
            if (e is {InnerException: ExceptionA or ExceptionB})
            {
                var message = "";
                if (e.InnerException is ExceptionA) message = "Error A";
                if (e.InnerException is ExceptionB) message = "Error B";
                
                var id = context.JobDetail.JobDataMap.GetIntValue("id");
                Console.WriteLine(message + ": Pong " + id + " failed and retrying...");
                
                // add new trigger to job
                SimpleTriggerImpl retryTrigger = new SimpleTriggerImpl(Guid.NewGuid().ToString());
                retryTrigger.JobKey = context.JobDetail.Key;
                retryTrigger.StartTimeUtc = DateBuilder.NextGivenSecondDate(DateTime.Now, 10);
                await context.Scheduler.ScheduleJob(retryTrigger);
            }
        }
    }
}
