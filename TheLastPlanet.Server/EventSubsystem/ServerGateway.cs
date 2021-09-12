using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Logger;
using TheLastPlanet.Server.Core;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.Internal.Events;
using TheLastPlanet.Shared.Internal.Events.Message;
using TheLastPlanet.Shared.Internal.Events.Serialization;
using TheLastPlanet.Shared.TypeExtensions;
using TheLastPlanet.Shared.Internal.Events.Serialization.Implementations;

namespace TheLastPlanet.Server.Internal.Events
{
    public class ServerGateway : BaseGateway
    {
        protected override ISerialization Serialization { get; }
        private Dictionary<int, string> _signatures = new();

        public ServerGateway()
        {
            Serialization = new BinarySerialization();
            DelayDelegate = async delay => await BaseScript.Delay(delay);
            PushDelegate = Push;
            Server.Instance.AddEventHandler(EventConstant.SignaturePipeline, new Action<string>(GetSignature));
			Server.Instance.AddEventHandler(EventConstant.InboundPipeline, new Action<string, byte[]>(Inbound));
			Server.Instance.AddEventHandler(EventConstant.OutboundPipeline, new Action<string, byte[]>(Outbound));
        }

        public void Push(string pipeline, ISource source, byte[] buffer)
        {
            if (source.Handle != new ServerId().Handle)
                BaseScript.TriggerClientEvent(Funzioni.GetPlayerFromId(source.Handle), pipeline, buffer);
            else
                BaseScript.TriggerClientEvent(pipeline, buffer);
        }


        private void GetSignature([FromSource] string source)
        {
            try
            {
                var client = (ClientId)source;

                if (_signatures.ContainsKey(client.Handle))
                {
                   Server.Logger.Info($"Client {client} tried acquiring event signature more than once.");

                    return;
                }

                var holder = new byte[128];

                using (var service = new RNGCryptoServiceProvider())
                {
                    service.GetBytes(holder);
                }

                var signature = BitConverter.ToString(holder).Replace("-", "").ToLower();

                _signatures.Add(client.Handle, signature);
                BaseScript.TriggerClientEvent(Funzioni.GetPlayerFromId(client.Handle), EventConstant.SignaturePipeline, signature);
            }
            catch (Exception ex)
            {
                Server.Logger.Error(ex.ToString());
            }
        }

        private async void Inbound([FromSource] string source, byte[] buffer)
        {
            try
            {
                var client = (ClientId)source;

                if (!_signatures.TryGetValue(client.Handle, out var signature)) return;

                using var context = new SerializationContext(EventConstant.InboundPipeline, null, Serialization, buffer);

                var message = context.Deserialize<EventMessage>();


                if (!VerifySignature(client, message, signature)) return;

                try
                {
                    await ProcessInboundAsync(message, client);
                }
                catch (TimeoutException)
                {
                    API.DropPlayer(client.Handle.ToString(), $"Operation timed out: {message.Endpoint.ToBase64()}");
                }
            }
            catch (Exception ex)
            {
                Server.Logger.Error(ex.ToString());
            }
        }

        public bool VerifySignature(ISource source, IMessage message, string signature)
        {
            if (message.Signature == signature) return true;

            Server.Logger.Error($"[{message.Endpoint}] Client {source} had invalid event signature, aborting:");
            Server.Logger.Error($"[{message.Endpoint}] \tSupplied Signature: {message.Signature}");
            Server.Logger.Error($"[{message.Endpoint}] \tActual Signature: {message.Signature}");

            return false;
        }

        private void Outbound([FromSource] string source, byte[] buffer)
        {
            try
            {
                var client = (ClientId)source;

                if (!_signatures.TryGetValue(client.Handle, out var signature)) return;

                using var context = new SerializationContext(EventConstant.OutboundPipeline, null, Serialization, buffer);

                var response = context.Deserialize<EventResponseMessage>();

                if (!VerifySignature(client, response, signature)) return;

                ProcessOutbound(response);
            }
            catch (Exception ex)
            {
                Server.Logger.Error(ex.ToString());
            }
        }

        public void Send(Player player, string endpoint, params object[] args) => Send(Convert.ToInt32(player.Handle), endpoint, args);
        public void Send(ClientId client, string endpoint, params object[] args) => Send(client.Handle, endpoint, args);
        public void Send(List<Player> players, string endpoint, params object[] args) => Send(players.Select(x => Convert.ToInt32(x.Handle)).ToList(), endpoint, args);
        public void Send(List<ClientId> clients, string endpoint, params object[] args) => Send(clients.Select(x => x.Handle).ToList(), endpoint, args);

        public void Send(List<int> targets, string endpoint, params object[] args)
		{
            for(int i=0; i<targets.Count; i++) Send(targets[i], endpoint, args);
		}

        public async void Send(int target, string endpoint, params object[] args)
        {
            await SendInternal(EventFlowType.Straight, new ClientId(target), endpoint, args);
        }

        public Task<T> Get<T>(Player player, string endpoint, params object[] args) where T : class =>
            Get<T>(Convert.ToInt32(player.Handle), endpoint, args);

        public Task<T> Get<T>(ClientId client, string endpoint, params object[] args) where T : class =>
            Get<T>(client.Handle, endpoint, args);

        public async Task<T> Get<T>(int target, string endpoint, params object[] args) where T : class
        {
            return await GetInternal<T>((ClientId)target, endpoint, args);
        }
    }
}