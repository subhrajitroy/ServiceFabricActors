using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace UserActorService
{
    [StatePersistence(StatePersistence.Persisted)]
    internal class AdminActor : UserActor
    {
        public AdminActor(ActorService actorService, ActorId actorId) : base(actorService, actorId)
        {
        }
    }
}