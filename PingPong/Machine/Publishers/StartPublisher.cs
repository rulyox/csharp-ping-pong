using System.Threading.Tasks;
using PingPong.Player1.Events;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Local;

namespace PingPong.Machine.Publishers
{
    public class StartPublisher: ITransientDependency
    {
        private readonly ILocalEventBus _localEventBus;

        public StartPublisher(ILocalEventBus localEventBus)
        {
            _localEventBus = localEventBus;
        }
        
        public virtual async Task Publish()
        {
            await _localEventBus.PublishAsync(
                new StartEvent()
            );
        }
    }
}
