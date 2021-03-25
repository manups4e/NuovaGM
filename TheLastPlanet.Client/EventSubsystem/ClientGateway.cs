using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using Logger;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.Internal.Diagnostics;
using TheLastPlanet.Shared.Internal.Events;
using TheLastPlanet.Shared.Internal.Events.Message;
using TheLastPlanet.Shared.Internal.Events.Response;

namespace TheLastPlanet.Client.Internal.Events
{
    public class ClientGateway : BaseGateway
    {
        public List<NetworkMessage> Buffer { get; } = new List<NetworkMessage>();

        protected override Task GetDelayedTask(int milliseconds = 0)
        {
            return BaseScript.Delay(milliseconds);
        }

        protected override async void TriggerImpl(string pipeline, int target, ISerializable payload)
        {
            RequireServerTarget(target);

            // wait if the signature has not yet been delivered
            while (_signature == null)
            {
                await BaseScript.Delay(10);
            }

            // set the payload signature
            payload.Signature = _signature;
            BaseScript.TriggerServerEvent(pipeline, payload.Serialize());
        }

        private string _signature;

        public ClientGateway()
        {
            Client.Instance.AddEventHandler(EventConstant.InboundPipeline, new Action<string>(Inbound));
            Client.Instance.AddEventHandler(EventConstant.OutboundPipeline, new Action<string>(Outbound));
            Client.Instance.AddEventHandler(EventConstant.SignaturePipeline, new Action<string>(TakeSignature));

            BaseScript.TriggerServerEvent(EventConstant.SignaturePipeline);
        }

        private void TakeSignature(string signature)
        {
            try
            {
                _signature = signature;
            }
            catch (Exception ex)
            {
                Log.Printa(LogType.Error,  ex.ToString());
            }
        }

        private async void Inbound(string serialized)
        {
            try
            {
                var message = serialized.FromJson<EventMessage>();

                await ProcessInboundAsync(message, new ServerSource());
            }
            catch (Exception ex)
            {
                Log.Printa(LogType.Error, ex.ToString());
            }
        }

        private void Outbound(string serialized)
        {
            try
            {
                var response = EventResponseMessage.Deserialize(serialized);

                ProcessOutbound(response);
            }
            catch (Exception ex)
            {
                Log.Printa(LogType.Error, ex.ToString());
            }
        }

        public void Send(string endpoint, params object[] args)
        {
            SendInternal(-1, endpoint, args);
        }

        public async Task<T> Get<T>(string endpoint, params object[] args)
        {
            return await GetInternal<T>(-1, endpoint, args);
        }

        private void RequireServerTarget(int endpoint)
        {
            if (endpoint != -1) throw new Exception($"Il client può solo inviare al server (arg {nameof(endpoint)} non coincide con -1)");
        }
    }
}