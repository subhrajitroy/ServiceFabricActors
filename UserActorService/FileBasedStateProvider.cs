using System;
using System.Collections.Generic;
using System.Fabric;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Query;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Data;
using Newtonsoft.Json.Linq;

namespace UserActorService

{

    public class FileBasedStateProvider : IActorStateProvider

    {

        private readonly IActorStateProvider kvsActorStateProvider;



        public FileBasedStateProvider()

        {

            kvsActorStateProvider = new KvsActorStateProvider();

        }



        public void Initialize(StatefulServiceInitializationParameters initializationParameters)

        {

            kvsActorStateProvider.Initialize(initializationParameters);

        }



        public Task<IReplicator> OpenAsync(ReplicaOpenMode openMode, IStatefulServicePartition partition, CancellationToken cancellationToken)

        {

            return kvsActorStateProvider.OpenAsync(openMode, partition, cancellationToken);

        }



        public Task ChangeRoleAsync(ReplicaRole newRole, CancellationToken cancellationToken)

        {

            return kvsActorStateProvider.ChangeRoleAsync(newRole, cancellationToken);

        }



        public Task CloseAsync(CancellationToken cancellationToken)

        {

            return kvsActorStateProvider.CloseAsync(cancellationToken);

        }



        public void Abort()

        {

            throw new NotImplementedException();

        }



        public Task BackupAsync(Func<BackupInfo, CancellationToken, Task<bool>> backupCallback)

        {

            throw new NotImplementedException();

        }



        public Task BackupAsync(BackupOption option, TimeSpan timeout, CancellationToken cancellationToken, Func<BackupInfo, CancellationToken, Task<bool>> backupCallback)

        {

            throw new NotImplementedException();

        }



        public Task RestoreAsync(string backupFolderPath)

        {

            throw new NotImplementedException();

        }



        public Task RestoreAsync(string backupFolderPath, RestorePolicy restorePolicy, CancellationToken cancellationToken)

        {

            throw new NotImplementedException();

        }



        public Func<CancellationToken, Task<bool>> OnDataLossAsync { get; set; }

        public void Initialize(ActorTypeInformation actorTypeInformation)

        {

            throw new NotImplementedException();

        }



        public Task ActorActivatedAsync(ActorId actorId, CancellationToken cancellationToken = new CancellationToken())

        {

            var orgId = actorId.GetGuidId();

            var actorStateDataFile = ActorStateDataFile(orgId);

            if (!File.Exists(actorStateDataFile))

            {

                File.WriteAllText(actorStateDataFile, $"\"Id\":\"{orgId}\"");

            }

            return kvsActorStateProvider.ActorActivatedAsync(actorId, cancellationToken);

        }



        private static string ActorStateDataFile(Guid orgId)

        {

            var actorStateDataFile = $"C:\\Data\\{orgId}.data";

            return actorStateDataFile;

        }



        public Task ReminderCallbackCompletedAsync(ActorId actorId, IActorReminder reminder,

            CancellationToken cancellationToken = new CancellationToken())

        {

            throw new NotImplementedException();

        }



        public Task<T> LoadStateAsync<T>(ActorId actorId, string stateName,

            CancellationToken cancellationToken = new CancellationToken())

        {

            var jObject = GetActorStateAsJObject(actorId);

            return Task.FromResult(jObject.GetValue(stateName).ToObject<T>());

        }



        private static JObject GetActorStateAsJObject(ActorId actorId)

        {

            var json = File.ReadAllText(ActorStateDataFile(actorId.GetGuidId()));

            var jObject = JObject.Parse(json);

            return jObject;

        }



        public Task SaveStateAsync(ActorId actorId, IReadOnlyCollection<ActorStateChange> stateChanges,

            CancellationToken cancellationToken = new CancellationToken())

        {

            var jObject = GetActorStateAsJObject(actorId);

            foreach (var actorStateChange in stateChanges)

            {

                switch (actorStateChange.ChangeKind)

                {

                    case StateChangeKind.Add:

                    case StateChangeKind.Update:

                        jObject[actorStateChange.StateName] = JToken.FromObject(actorStateChange.Value);

                        break;

                    case StateChangeKind.Remove:

                        jObject.Remove(actorStateChange.StateName);

                        break;

                    case StateChangeKind.None:

                        break;

                    default:

                        throw new ArgumentOutOfRangeException();

                }

            }

            File.WriteAllText(ActorStateDataFile(actorId.GetGuidId()), jObject.ToString());

            return kvsActorStateProvider.SaveStateAsync(actorId, stateChanges, cancellationToken);

        }



        public Task<bool> ContainsStateAsync(ActorId actorId, string stateName,

            CancellationToken cancellationToken = new CancellationToken())

        {

            return kvsActorStateProvider.ContainsStateAsync(actorId, stateName, cancellationToken);

        }



        public Task RemoveActorAsync(ActorId actorId, CancellationToken cancellationToken = new CancellationToken())

        {

            return kvsActorStateProvider.RemoveActorAsync(actorId, cancellationToken);

        }



        public Task<IEnumerable<string>> EnumerateStateNamesAsync(ActorId actorId, CancellationToken cancellationToken = new CancellationToken())

        {

            return kvsActorStateProvider.EnumerateStateNamesAsync(actorId, cancellationToken);

        }



        public Task<PagedResult<ActorId>> GetActorsAsync(int numItemsToReturn, ContinuationToken continuationToken, CancellationToken cancellationToken)

        {

            return kvsActorStateProvider.GetActorsAsync(numItemsToReturn, continuationToken, cancellationToken);

        }



        public Task SaveReminderAsync(ActorId actorId, IActorReminder reminder,

            CancellationToken cancellationToken = new CancellationToken())

        {

            return kvsActorStateProvider.SaveReminderAsync(actorId, reminder, cancellationToken);

        }



        public Task DeleteReminderAsync(ActorId actorId, string reminderName,

            CancellationToken cancellationToken = new CancellationToken())

        {

            return kvsActorStateProvider.DeleteReminderAsync(actorId, reminderName, cancellationToken);

        }



        public Task<IActorReminderCollection> LoadRemindersAsync(CancellationToken cancellationToken = new CancellationToken())

        {

            return kvsActorStateProvider.LoadRemindersAsync(cancellationToken);

        }

    }
}