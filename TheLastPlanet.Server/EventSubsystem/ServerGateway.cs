using System;
using System.Collections.Generic;
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
            if (target != -1)
                Funzioni.GetPlayerFromId(target).TriggerEvent(pipeline, payload.Serialize());
            else
                BaseScript.TriggerClientEvent(pipeline, payload.Serialize());
        }

        private Dictionary<int, string> _signatures = new Dictionary<int, string>();

        public ServerGateway()
        {
			TheLastPlanet.Server.Instance.AddEventHandler(EventConstant.SignaturePipeline, new Action<Player>(GetSignature));
			TheLastPlanet.Server.Instance.AddEventHandler(EventConstant.InboundPipeline, new Action<Player, string>(Inbound));
			TheLastPlanet.Server.Instance.AddEventHandler(EventConstant.OutboundPipeline, new Action<Player, string>(Outbound));
        }

        private void GetSignature([FromSource] Player source)
        {
            try
            {
                if (_signatures.ContainsKey(Convert.ToInt32(source.Handle)))
                {
                    Log.Printa(LogType.Warning, $"Client {source.Name} [{source.Handle}] tried obtaining event signature illegally.");

                    return;
                }

                var holder = new byte[64];

                using (var service = new RNGCryptoServiceProvider())
                {
                    service.GetBytes(holder);
                }

                var signature = BitConverter.ToString(holder).Replace("-", "").ToLower();

                _signatures.Add(Convert.ToInt32(source.Handle), signature);
                source.TriggerEvent(EventConstant.SignaturePipeline, signature);
            }
            catch (Exception ex)
            {
                Log.Printa(LogType.Error, ex.ToString());
            }
        }

        private async void Inbound([FromSource] Player source, string serialized)
        {
            try
            {
                if (!_signatures.TryGetValue(Convert.ToInt32(source.Handle), out var signature)) return;

                var message = EventMessage.Deserialize(serialized);

                if (message.Signature != signature)
                {
                    Log.Printa(LogType.Warning, 
                        $"[{EventConstant.InboundPipeline}, {source.Handle}, {message.Signature}] Client {source.Name} had invalid event signature, possible malicious intent?");

                    return;
                }

                try
                {
                    await ProcessInboundAsync(message, Convert.ToInt32(source.Handle));
                }
                catch (TimeoutException)
                {
                    API.DropPlayer(source.Handle, $"Operation timed out: {message.Endpoint.ToBase64()}");
                }
            }
            catch (Exception ex)
            {
                Log.Printa(LogType.Error, ex.ToString());
            }
        }

        private void Outbound([FromSource] Player source, string serialized)
        {
            try
            {
                if (!_signatures.TryGetValue(Convert.ToInt32(source.Handle), out var signature)) return;

                var response = EventResponseMessage.Deserialize(serialized);

                if (response.Signature != signature)
                {
                    Log.Printa(LogType.Warning, 
                        $"[{EventConstant.OutboundPipeline}, {source.Handle}, {response.Signature}] Client {source.Name} had invalid event signature, possible malicious intent?");

                    return;
                }

                ProcessOutbound(response);
            }
            catch (Exception ex)
            {
                Log.Printa(LogType.Error, ex.ToString());
            }
        }

        public void Send(Player player, string endpoint, params object[] args) => Send(Convert.ToInt32(player.Handle), endpoint, args);

        public void Send(int target, string endpoint, params object[] args)
        {
            SendInternal(target, endpoint, args);
        }

        public Task<T> Get<T>(Player player, string endpoint, params object[] args) => Get<T>(Convert.ToInt32(player.Handle), endpoint, args);

        public async Task<T> Get<T>(int target, string endpoint, params object[] args)
        {
            return await GetInternal<T>(-1, endpoint, args);
        }
    }
}