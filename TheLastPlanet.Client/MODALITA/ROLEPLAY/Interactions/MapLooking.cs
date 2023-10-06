using System.Threading.Tasks;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.Handlers;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Interactions
{
    internal class MapLooking
    {
        public static bool wasmenuopen = false;
        public static void Init()
        {
            TickController.TickAPiedi.Add(Mappina);
        }
        public static void Stop()
        {
            Client.Instance.RemoveTick(Mappina);
        }
        public static async Task Mappina()
        {
            var InPausa = PlayerCache.MyPlayer.Status.PlayerStates.InPausa;
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
