using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Actors.Runtime;
using UserActorService.Interfaces;

namespace UsersAPI.Controllers
{
    [ServiceRequestActionFilter]
    [RoutePrefix("api/users")]
    public class UserController : ApiController
    {
        [HttpGet]
        [Route("health")]
        public IHttpActionResult GetHealthCheck()
        {
            return Ok("Users OK");
        }

        [HttpPost]
        [Route("notify/{userId}")]
        public async Task<IHttpActionResult> NotifyUser(Guid userId)
        {
            var userActor = GetUser(userId);
            var acknowledgement = await userActor.NotifyAsync(new Message("Hello User"));
            var name = await userActor.GetNameAsync();
            var ack = $"user {name} set ack message {acknowledgement.AckMessage}";
            return Ok(ack);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IHttpActionResult> Register(User user)
        {
            var userId = Guid.NewGuid();
            var userActor = GetUser(userId);
            await userActor.SetNameAsync(user.Name);
            return Created($"users/{userId}", string.Empty);
        }

        private static IUserActor GetUser(Guid userId)
        {
            return ActorProxy.Create<IUserActor>(new ActorId(userId));
        }
    }
}
