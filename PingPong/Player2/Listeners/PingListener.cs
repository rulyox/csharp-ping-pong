using System;
using System.Threading;
using System.Threading.Tasks;
using PingPong.Player2.Jobs;
using Quartz;
using Quartz.Impl.Triggers;

namespace PingPong.Player2.Listeners
{
    public class PingListener: IJobListener
    {
        public string Name => "listener_ping";
        
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
            if (e is {InnerException: ExceptionC or ExceptionD})
            {
                var message = "";
                if (e.InnerException is ExceptionC) message = "Error C";
                if (e.InnerException is ExceptionD) message = "Error D";
                
                var id = context.JobDetail.JobDataMap.GetIntValue("id");
                Console.WriteLine(message + ": Ping " + id + " failed and retrying...");
                
                // add new trigger to job
                SimpleTriggerImpl retryTrigger = new SimpleTriggerImpl(Guid.NewGuid().ToString());
                retryTrigger.JobKey = context.JobDetail.Key;
                retryTrigger.StartTimeUtc = DateBuilder.NextGivenSecondDate(DateTime.Now, 10);
                await context.Scheduler.ScheduleJob(retryTrigger);
            }
        }
    }
}
