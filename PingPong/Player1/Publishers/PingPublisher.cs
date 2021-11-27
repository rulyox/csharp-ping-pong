using System;
using System.Threading.Tasks;
using PingPong.Machine;
using PingPong.Player2.Events;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Local;

namespace PingPong.Player1.Publishers
{
    public class PingPublisher: ITransientDependency
    {
        private readonly ILocalEventBus _localEventBus;

        public PingPublisher(ILocalEventBus localEventBus)
        {
            _localEventBus = localEventBus;
        }
        
        private static readonly Lazy<PingPublisher> Lazy = new(() => new PingPublisher(Common.EventBus));
        
        public static PingPublisher Instance { get { return Lazy.Value; } }
        
        public virtual async Task Publish(int id)
        {
            await _localEventBus.PublishAsync(
                new PingEvent(id)
            );
        }
    }
}
