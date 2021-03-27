using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Logger;
using Newtonsoft.Json;
using TheLastPlanet.Client.Core.PlayerChar;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.Internal.Events;
using TheLastPlanet.Shared.Snowflakes;

namespace TheLastPlanet.Client.SessionCache
{
	public static class Cache
    {
        private static bool _inVeh;
        private static bool _inPausa;
        //public static PlayerCache MyPlayer { get; private set; }
        public static ClientId MyPlayer { get; private set; }
        public static Char_data CurrentChar { get => MyPlayer.User.CurrentChar; }
        public static List<ClientId> GiocatoriOnline = new();

        public static async Task InitPlayer()
        {
            var pippo = await Client.Instance.Eventi.Get<Tuple<Snowflake, User>>("lprp:setupUser");
            MyPlayer = new()
            {
                Player = Game.Player,
                Handle = Game.Player.ServerId,
                Ped = new Ped(API.PlayerPedId()),
                Id = pippo.Item1,
            };
			MyPlayer.User = new (pippo.Item2);
            MyPlayer.Identifiers = MyPlayer.User.Identifiers.ToArray();
            Client.Instance.AddTick(TickStatiPlayer);
            await Task.FromResult(0);
        }

        public static async Task Loaded()
        {
            while (MyPlayer == null || MyPlayer!= null && !MyPlayer.Ready)
            {
                await BaseScript.Delay(0);
            }
        }
        
        public static async Task TickStatiPlayer()
        {
            await Loaded();
            await BaseScript.Delay(200);

            #region Check Veicolo

            if (MyPlayer.Ped.IsInVehicle())
            {
                if (!_inVeh)
                {
                    MyPlayer.User.StatiPlayer.InVeicolo = true;
                    _inVeh = true;
                }
            }
            else
            {
                if (_inVeh)
                {
                    MyPlayer.User.StatiPlayer.InVeicolo = false;
                    _inVeh = false;
                }
            }

            #endregion

            #region Check Pausa

            if (Game.IsPaused || HUD.MenuPool.PauseMenus.Any(x => x.Visible))
            {
                if (!_inPausa)
                {
                    _inPausa = true;
                    MyPlayer.User.StatiPlayer.InPausa = true;
                }
            }
            else
            {
                if (_inPausa)
                {
                    _inPausa = false;
                    MyPlayer.User.StatiPlayer.InPausa = false;
                }
            }

            #endregion

            #region Posizione

            if (!MyPlayer.User.status.Spawned) return;
            if (MyPlayer.User.StatiPlayer.Istanza.Stanziato) return;
            MyPlayer.User.Posizione = new Position(MyPlayer.Ped.Position, MyPlayer.Ped.Heading);

            #endregion

            await Task.FromResult(0);
        }
    }
}