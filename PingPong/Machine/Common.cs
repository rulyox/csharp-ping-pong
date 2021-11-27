using Quartz;
using Volo.Abp.EventBus.Local;

namespace PingPong.Machine
{
    public static class Common
    {
        public static IScheduler Scheduler { get; set; }
        public static ILocalEventBus EventBus { get; set; }
    }
}
