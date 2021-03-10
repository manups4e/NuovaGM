using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using Logger;
using TheLastPlanet.Client.Core.PlayerChar;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Shared;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client.Cache
{
    public static class Cache
    {
        private static bool _inVeh;
        private static bool _inPausa;
        public static PlayerCache MyPlayer { get; private set; }
        public static Dictionary<string, Character> GiocatoriOnline = new();

        public static async Task InitPlayer()
        {
            MyPlayer = await PlayerCache.CreatePlayerCache();
            MyPlayer.SetCharacter(await Client.Instance.Eventi.Request<Character>("lprp:setupUser"));
            await Loading();
            Client.Instance.AddTick(TickStatiPlayer);
            await Task.FromResult(0);
        }

        public static async Task Loading()
        {
            while (MyPlayer == null || MyPlayer != null && !MyPlayer.Ready)
            {
                await BaseScript.Delay(100);
            }
        }
        
        public static async Task TickStatiPlayer()
        {
            await Loading();
            await BaseScript.Delay(20);

            #region Check Veicolo

            if (MyPlayer.Ped.IsInVehicle())
            {
                if (!_inVeh)
                {
                    MyPlayer.Character.StatiPlayer.InVeicolo = true;
                    _inVeh = true;
                }
            }
            else
            {
                if (_inVeh)
                {
                    MyPlayer.Character.StatiPlayer.InVeicolo = false;
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
                    MyPlayer.Character.StatiPlayer.InPausa = true;
                }
            }
            else
            {
                if (_inPausa)
                {
                    _inPausa = false;
                    MyPlayer.Character.StatiPlayer.InPausa = false;
                }
            }

            #endregion

            await Task.FromResult(0);
        }
    }
}