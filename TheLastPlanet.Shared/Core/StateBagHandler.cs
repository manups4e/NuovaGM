using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Text;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.Internal.Events.Attributes;

namespace Impostazioni.Shared.Core
{
    public delegate void RoleplayStateBagChaged(int userId, string type, bool value);
    public delegate void PlayerStateBagChaged(int userId, string type, bool value);
    public delegate void InstanceBagChanged(int userId, InstanceBag value);
    public delegate void EntityStateBagChaged(int entity, string type, bool value);

    public class StateBagsHandler
    {
        public event RoleplayStateBagChaged OnRoleplayStateBagChange;
        public event PlayerStateBagChaged OnPlayerStateBagChange;
        public event EntityStateBagChaged OnEntityStateBagChange;
        public event InstanceBagChanged OnInstanceBagChange;

        private readonly Logger.Log logger = new();
        public StateBagsHandler()
        {
            API.AddStateBagChangeHandler("", "", new Action<string, string, dynamic, dynamic, bool>((bagName, key, value, _unused, replicated) =>
            {
                if (replicated) return;

                var entType = bagName.Substring(0, bagName.IndexOf(':'));
                int userId = Convert.ToInt32(bagName.Substring(bagName.IndexOf(':') + 1));

                logger.Warning($"{bagName}, {key}, {value}, {_unused}, {replicated}");
                switch (entType)
                {
                    case "player":
                        //ClientId player = Server.Instance.Clients[userId];
                        var modeType = key.Contains(":") ? key.Substring(0, key.IndexOf(':')) : key;
                        var state = key.Substring(key.IndexOf(':') + 1);
                        switch (modeType)
                        {
                            case "PlayerStates":
                                switch (state)
                                {
                                    case "InPausa":
                                        bool res = (value as byte[]).FromBytes<bool>();
                                        OnPlayerStateBagChange?.Invoke(userId, state, res);
                                        break;
                                }
                                break;
                            case "RolePlayStates":
                                switch (state)
                                {
                                    case "Svenuto":
                                    case "Ammanettato":
                                    case "InCasa":
                                    case "InServizio":
                                    case "FinDiVita":
                                    case "InVeicolo":
                                        bool res = (value as byte[]).FromBytes<bool>();
                                        OnRoleplayStateBagChange?.Invoke(userId, state, res);
                                        break;
                                }
                                break;
                            case "PlayerInstance":
                                { 
                                    InstanceBag inst = (value as byte[]).FromBytes<InstanceBag>();
                                    OnInstanceBagChange?.Invoke(userId, inst);
                                    break;
                                }
                        }
                        break;
                    case "entity":
                        break;
                }
            }));

            OnRoleplayStateBagChange += (a, b, c) =>
            {
                logger.Debug($"{a}, {b}, {c}");
            };

            OnInstanceBagChange += (a, b) =>
            {
                logger.Debug($"{a}, {b.ToJson()}");
            };
        }
    }
}
