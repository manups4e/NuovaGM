using System.Threading.Tasks;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.Telefono.Models;


namespace TheLastPlanet.Client.Telefono.Apps
{
    class QuickSave : App
    {
        public QuickSave(Phone phone) : base("Salvataggio Rapido", 43, phone)
        {

        }

        private static bool FirstTick = true;
        public override async Task Tick()
        {
            if (FirstTick)
            {
                FirstTick = false;
                await BaseScript.Delay(100);
                return;
            }
        }

        public override async void Initialize(Phone phone)
        {
            Phone = phone;
            BaseScript.TriggerServerEvent("lprp:salvaPlayer");
            BaseScript.TriggerEvent("lprp:phone_start", "Main");
            await BaseScript.Delay(5000);
            HUD.ShowNotification("Salvataggio Completato", ColoreNotifica.GreenDark);

        }

        public override void Kill()
        {
        }
    }
}
