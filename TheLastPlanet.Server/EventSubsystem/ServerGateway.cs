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
using TheLastPlanet.Shared.Internal.Extensions;

namespace TheLastPlanet.Server.Internal.Events
{
    public class ServerGateway : BaseGateway
    {
        protected override Task GetDelayedTask(int milliseconds = 0)
        {
            return BaseScript.Delay(milliseconds);
        }

        protected override void TriggerImpl(string pipeline, int target, ISerializable payload)
        {
            if (target != ClientId.Global.Handle)
                Funzioni.GetPlayerFromId(target).TriggerEvent(pipeline, payload.Serialize());
            else
                BaseScript.TriggerClientEvent(pipeline, payload.Serialize());
        }

        private Dictionary<int, string> _signatures = new Dictionary<int, string>();

        public ServerGateway()
        {
			Server.Instance.AddEventHandler(EventConstant.SignaturePipeline, new Action<string>(GetSignature));
			Server.Instance.AddEventHandler(EventConstant.InboundPipeline, new Action<string, string>(Inbound));
			Server.Instance.AddEventHandler(EventConstant.OutboundPipeline, new Action<string, string>(Outbound));
        }

        private void GetSignature([FromSource] string source)
        {
            try
            {
                var client = (ClientId)source;
                if (_signatures.ContainsKey(client.Handle))
                {
                    Server.Logger.Warning( $"Il player {client} [ServerID: {client.Handle}] ha tentato di ottenere la firma digitale illegalmente.");

                    return;
                }

                var holder = new byte[256];

                using (var service = new RNGCryptoServiceProvider())
                {
                    service.GetBytes(holder);
                }

                var signature = BitConverter.ToString(holder).Replace("-", "").ToLower();
                _signatures.Add(client.Handle, signature);
                client.Player.TriggerEvent(EventConstant.SignaturePipeline, signature);
            }
            catch (Exception ex)
            {
                Server.Logger.Error( ex.ToString());
            }
        }

        private async void Inbound([FromSource] string source, string serialized)
        {
            try
            {
                var client = (ClientId)source;

                if (!_signatures.TryGetValue(client.Handle, out var signature)) return;

                var message = EventMessage.Deserialize(serialized);

                if (message.Signature != signature)
                {
                    Server.Logger.Warning( 
                        $"[{EventConstant.InboundPipeline}, {client.Handle}, {message.Signature}] Il Player {client.Player.Name} ha una firma digitale non valida, possibile intento maligno?");

                    return;
                }

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
                Server.Logger.Error( ex.ToString());
            }
        }

        private void Outbound([FromSource] string source, string serialized)
        {
            try
            {
                var client = (ClientId)source;
                if (!_signatures.TryGetValue(client.Handle, out var signature)) return;

                var response = EventResponseMessage.Deserialize(serialized);

                if (response.Signature != signature)
                {
                    Server.Logger.Warning(
                        $"[{EventConstant.OutboundPipeline}, {client.Handle}, {response.Signature}] Il Player {client.Player.Name} ha una firma digitale non valida, possibile intento maligno?");

                    return;
                }

                ProcessOutbound(response);
            }
            catch (Exception ex)
            {
                Server.Logger.Error( ex.ToString());
            }
        }

        public void Send(Player player, string endpoint, params object[] args) => Send(Convert.ToInt32(player.Handle), endpoint, args);
        public void Send(ClientId client, string endpoint, params object[] args) => Send(client.Handle, endpoint, args);
        public void Send(List<Player> players, string endpoint, params object[] args) => Send(players.Select(x => Convert.ToInt32(x.Handle)).ToList(), endpoint, args);
        public void Send(List<ClientId> clients, string endpoint, params object[] args) => Send(clients.Select(x=> x.Handle).ToList(), endpoint, args);

        public void Send(List<int> targets, string endpoint, params object[] args)
		{
            for(int i=0; i<targets.Count; i++) Send(targets[i], endpoint, args);
		}

        public void Send(int target, string endpoint, params object[] args)
        {
            SendInternal(target, endpoint, args);
        }

        public Task<T> Get<T>(Player player, string endpoint, params object[] args) => Get<T>(Convert.ToInt32(player.Handle), endpoint, args);
        public Task<T> Get<T>(ClientId client, string endpoint, params object[] args) => Get<T>(client.Handle, endpoint, args);

        public async Task<T> Get<T>(int target, string endpoint, params object[] args)
        {
            return await GetInternal<T>(-1, endpoint, args);
        }
    }
}