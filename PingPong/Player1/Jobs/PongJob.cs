using System;
using System.Threading.Tasks;
using PingPong.Player1.Publishers;
using Quartz;

namespace PingPong.Player1.Jobs
{
    public class PongJob: IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(1));

                // Intentional random errors
                // Error A: 5%, Error B: 5%
                Random r = new Random();
                int num = r.Next(1, 101);
                if (num is >= 1 and <= 5) throw new ExceptionA();
                if (num is >= 6 and <= 10) throw new ExceptionB();
            
                JobDataMap dataMap = context.JobDetail.JobDataMap;
                int id = dataMap.GetIntValue("id");
            
                Console.WriteLine("Player1: Pong " + id);

                await PingPublisher.Instance.Publish(id + 1);
            }
            catch (Exception e) when (e is ExceptionA or ExceptionB)
            {
                throw new JobExecutionException(e, false);
            }
        }
    }
}
