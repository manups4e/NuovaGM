using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using TheLastPlanet.Client.Cache;
using TheLastPlanet.Client.Core.PlayerChar;
using TheLastPlanet.Client.Core.Utility.HUD;

namespace TheLastPlanet.Client.CachePlayer
{
    public static class Cache
    {
        private static bool _inVeh;
        private static bool _inPausa;
        public static PlayerCache MyPlayer { get; private set; }
        public static Dictionary<string, User> GiocatoriOnline = new();

        public static async Task InitPlayer()
        {
            MyPlayer = new PlayerCache();
            var pippo = await Client.Instance.Eventi.Request<User>("lprp:setupUser");
            MyPlayer.SetCharacter(pippo);
            Client.Instance.AddTick(TickStatiPlayer);
            await Task.FromResult(0);
        }

        public static async Task Loaded()
        {
            while (MyPlayer == null || MyPlayer != null && !MyPlayer.Ready)
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

            await Task.FromResult(0);
        }
    }
}