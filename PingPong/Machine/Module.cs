using Volo.Abp.Autofac;
using Volo.Abp.EventBus;
using Volo.Abp.Modularity;

namespace PingPong.Machine
{
    [DependsOn(typeof(AbpEventBusModule), typeof(AbpAutofacModule))]
    public class Module: AbpModule
    {
        
    }
}
