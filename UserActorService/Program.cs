using System;
using System.Diagnostics;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace UserActorService
{
    internal static class Program
    {
        /// <summary>
        /// This is the entry point of the service host process.
        /// </summary>
        private static void Main()
        {
            try
            {
                ActorRuntime.RegisterActorAsync<UserActor>(
                   (context, actorType) => new UserActorService(context, actorType)).GetAwaiter().GetResult();
                ActorRuntime.RegisterActorAsync<AdminActor>(
                    (context, actorType) => new AdminActorService(context, actorType)
                    ).GetAwaiter().GetResult();
                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception e)
            {
                ActorEventSource.Current.ActorHostInitializationFailed(e.ToString());
                throw;
            }
        }
    }


}
