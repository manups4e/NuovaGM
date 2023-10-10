using System.Threading.Tasks;
using TheLastPlanet.Client.Handlers;

namespace TheLastPlanet.Client.GameMode.ROLEPLAY.Interactions
{
    internal class MapLooking
    {
        public static bool wasmenuopen = false;
        public static void Init()
        {
            TickController.TickOnFoot.Add(MapLook);
        }
        public static void Stop()
        {
            Client.Instance.RemoveTick(MapLook);
        }
        public static async Task MapLook()
        {
            bool InPausa = PlayerCache.MyPlayer.Status.PlayerStates.Paused;
            if ((InPausa || MenuHandler.IsAnyPauseMenuOpen) && !wasmenuopen)
            {
                PlayerCache.MyPlayer.Ped.Weapons.Select(WeaponHash.Unarmed);
                TaskStartScenarioInPlace(PlayerPedId(), "WORLD_HUMAN_TOURIST_MAP", 0, false);
                wasmenuopen = true;
            }
            else if (!(InPausa || MenuHandler.IsAnyPauseMenuOpen) && wasmenuopen)
            {
                PlayerCache.MyPlayer.Ped.Task.ClearAll();
                PlayerCache.MyPlayer.Ped.Task.ClearSecondary();
                wasmenuopen = false;
            }
            await BaseScript.Delay(1000);
        }
    }
}
