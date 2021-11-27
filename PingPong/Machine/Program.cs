using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PingPong.Machine.Publishers;
using PingPong.Player1.Listeners;
using PingPong.Player2.Listeners;
using Quartz;
using Quartz.Impl.Matchers;
using Volo.Abp;
using Volo.Abp.EventBus.Local;

namespace PingPong.Machine
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (var application = AbpApplicationFactory.Create<Module>(options => { options.UseAutofac(); }))
            {
                application.Initialize();

                Common.EventBus = application.ServiceProvider.GetRequiredService<LocalEventBus>();
                
                var properties = new NameValueCollection();
                Common.Scheduler = await SchedulerBuilder.Create(properties)
                    .UseDefaultThreadPool(x => x.MaxConcurrency = 5)
                    .UsePersistentStore(x =>
                    {
                        x.UseProperties = true;
                        x.UseSQLite("Data Source=quartznet.db;");
                        x.UseJsonSerializer();
                    })
                    .BuildScheduler();
                await Common.Scheduler.Start();
                Common.Scheduler.ListenerManager.AddJobListener(new PingListener(), GroupMatcher<JobKey>.GroupEquals("ping"));
                Common.Scheduler.ListenerManager.AddJobListener(new PongListener(), GroupMatcher<JobKey>.GroupEquals("pong"));

                while (true)
                {
                    var input = Console.ReadLine();
                    if (input is "s")
                    {
                        // start
                        StartPublisher publisher = new StartPublisher(Common.EventBus);
                        await publisher.Publish();
                    }
                    else if (input is "e")
                    {
                        // end
                        await Common.Scheduler.Clear();
                        break;
                    }    
                }
                
                await Common.Scheduler.Shutdown();
                application.Shutdown();
            }
        }
    }
}
