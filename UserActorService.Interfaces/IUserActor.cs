using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace UserActorService.Interfaces
{
    /// <summary>
    ///     This interface defines the methods exposed by an actor.
    ///     Clients use this interface to interact with the actor that implements it.
    /// </summary>
    public interface IUserActor : IActor
    {
        Task<Acknowledgement> NotifyAsync(Message message);
        Task SetNameAsync(string userName);
        Task<string> GetNameAsync();
    }
}