using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace UserActorService
{
    public class StatefulActor<T>  : Actor where T : class ,new ()
    {
        protected readonly ActorState<T> State;

        protected StatefulActor(ActorService actorService, ActorId actorId) : base(actorService, actorId)
        {
            State = new ActorState<T>(nameof(T),StateManager);
        }

    }

    

    public class ActorState<T>
    {
        private readonly string name;
        private readonly IActorStateManager stateManager;
        private T state;

        public ActorState(string name,IActorStateManager stateManager)
        {
            this.name = name;
            this.stateManager = stateManager;
        }

        public async Task<T> Get()
        {
            var conditionalValue = await stateManager.TryGetStateAsync<T>(name);
            state = conditionalValue.HasValue ? conditionalValue.Value : default(T);
            return state;
        }

        public async Task Replace(T newState)
        {
            await stateManager.SetStateAsync(name, newState);
            state = newState;
        }

        public async Task SaveChangesAsync()
        {
            await stateManager.SetStateAsync(name, state);
        }
    }
    
    
}