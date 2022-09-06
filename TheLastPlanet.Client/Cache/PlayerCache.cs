using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheLastPlanet.Client.Core.PlayerChar;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.Handlers;

using TheLastPlanet.Shared.PlayerChar;
using FxEvents.Shared.Snowflakes;

namespace TheLastPlanet.Client.Cache
{
    public static class PlayerCache
    {
        internal static bool _inVeh;
        private static bool _inPausa;
        private static SharedTimer _checkTimer;

        public static PlayerClient MyPlayer { get; private set; }
        public static Char_data CurrentChar => MyPlayer.User.CurrentChar;
        public static ModalitaServer ModalitàAttuale = ModalitaServer.Lobby;


        public static async Task InitPlayer()
        {
            var pippo = await EventDispatcher.Get<Tuple<Snowflake, BasePlayerShared>>("lprp:setupUser");
            Client.Logger.Warning(pippo.ToJson());
            MyPlayer = new PlayerClient(pippo);
            _checkTimer = new(5000);
            Client.Instance.AddTick(TickStatus);
            await Task.FromResult(0);
            VehicleChecker.OnPedEnteredVehicle += OnPedEnteredVehicle;
            VehicleChecker.OnPedLeftVehicle += OnPedLeftVehicle;
        }

        private static void OnPedLeftVehicle(Ped ped, Vehicle vehicle, VehicleSeat seatIndex)
        {
            if (ped.Handle == MyPlayer.Ped.Handle)
            {
                MyPlayer.Status.PlayerStates.InVeicolo = false;
                _inVeh = false;
            }
        }

        private static void OnPedEnteredVehicle(Ped ped, Vehicle vehicle, VehicleSeat seat)
        {
            if (ped.Handle == MyPlayer.Ped.Handle)
            {
                MyPlayer.Status.PlayerStates.InVeicolo = true;
                _inVeh = true;
            }
        }

        public static async Task Loaded()
        {
            while (MyPlayer == null || MyPlayer != null && !MyPlayer.Ready) await BaseScript.Delay(0);
        }

        public static async Task TickStatus()
        {
            #region Posizione
            // TODO: non salvare position nel db se siamo in un interior

            MyPlayer.Posizione = new Position(MyPlayer.Ped.Position, MyPlayer.Ped.Rotation);
            #endregion

            #region Check Pausa

            if (!_inPausa)
            {
                if (Game.IsPaused || HUD.MenuPool.IsAnyPauseMenuOpen)
                {
                    _inPausa = true;
                    MyPlayer.Status.PlayerStates.InPausa = true;
                }
            }
            else
            {
                if (!Game.IsPaused & !HUD.MenuPool.IsAnyPauseMenuOpen)
                {
                    _inPausa = false;
                    MyPlayer.Status.PlayerStates.InPausa = false;
                }
            }

            #endregion

            /*
            if (_checkTimer.IsPassed)
            {
                if (MyPlayer.Status.Istanza.Instance != "IngressoRoleplay")
                {
                    Eventi.AggiornaPlayers();
                }
            }
            */
            await Task.FromResult(0);
        }
    }
}