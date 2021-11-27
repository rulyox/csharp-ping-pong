using System;
using System.Threading.Tasks;
using PingPong.Player2.Publishers;
using Quartz;

namespace PingPong.Player2.Jobs
{
    public class PingJob: IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
            
                // Intentional random errors
                // Error C: 5%, Error D: 5%
                Random r = new Random();
                int num = r.Next(1, 101);
                if (num is >= 1 and <= 5) throw new ExceptionC();
                if (num is >= 6 and <= 10) throw new ExceptionD();
            
                JobDataMap dataMap = context.JobDetail.JobDataMap;
                int id = dataMap.GetIntValue("id");
            
                Console.WriteLine("Player2: Ping " + id);
            
                await PongPublisher.Instance.Publish(id + 1);   
            }
            catch (Exception e) when (e is ExceptionC or ExceptionD)
            {
                throw new JobExecutionException(e, false);
            }
        }
    }
}
