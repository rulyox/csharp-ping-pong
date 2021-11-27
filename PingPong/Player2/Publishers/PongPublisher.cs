using System;
using System.Threading.Tasks;
using PingPong.Machine;
using PingPong.Player1.Events;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Local;

namespace PingPong.Player2.Publishers
{
    public class PongPublisher: ITransientDependency
    {
        private readonly ILocalEventBus _localEventBus;

        public PongPublisher(ILocalEventBus localEventBus)
        {
            _localEventBus = localEventBus;
        }
        
        private static readonly Lazy<PongPublisher> Lazy = new(() => new PongPublisher(Common.EventBus));
        
        public static PongPublisher Instance { get { return Lazy.Value; } }
        
        public virtual async Task Publish(int id)
        {
            await _localEventBus.PublishAsync(
                new PongEvent(id)
            );
        }
    }
}
