using System;
using System.Threading.Tasks;
using PingPong.Player1.Publishers;
using Quartz;

namespace PingPong.Player1.Jobs
{
    public class StartJob: IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            
            Console.WriteLine("Player1: Start");

            await PingPublisher.Instance.Publish(1);
        }
    }
}
