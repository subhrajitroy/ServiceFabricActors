using System;
using System.Fabric;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace UserActorService
{
    public class AdminActorService : ActorService
    {
        public AdminActorService(StatefulServiceContext context, ActorTypeInformation actorTypeInfo, Func<ActorService, ActorId, ActorBase> actorFactory = null, Func<ActorBase, IActorStateProvider, IActorStateManager> stateManagerFactory = null, IActorStateProvider stateProvider = null, ActorServiceSettings settings = null) 
            : base(context, actorTypeInfo, actorFactory, stateManagerFactory, stateProvider, settings)
        {
        }
    }

    public class UserActorService : ActorService
    {
        public UserActorService(StatefulServiceContext context, ActorTypeInformation actorTypeInfo, Func<ActorService, ActorId, ActorBase> actorFactory = null, Func<ActorBase, IActorStateProvider, IActorStateManager> stateManagerFactory = null, IActorStateProvider stateProvider = null, ActorServiceSettings settings = null) :
            base(context, actorTypeInfo, actorFactory, stateManagerFactory, new FileBasedStateProvider(), settings)
        {
        }
    }
}