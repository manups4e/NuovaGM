using System.Threading.Tasks;
using TheLastPlanet.Client.Core.Utility.HUD;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Veicoli
{
    internal static class Assicurazione
    {
        public static void Init() { }

        private static async Task ControlloAssicurazione()
        {
            Ped playerPed = Cache.PlayerCache.MyPlayer.Ped;

            if (Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVeicolo && playerPed.CurrentVehicle.Driver != playerPed)
            {
                if (playerPed.CurrentVehicle.IsOnFire || playerPed.CurrentVehicle.IsDead) HUD.ShowAdvancedNotification("Assicurazione", "Versamento Indennizzo", "Dato che era nel veicolo alla distruzione ma non era guidatore, le verrà rimborsata una parte del costo del veicolo.", NotificationIcon.MorsMutual, TipoIcona.DollarIcon);

                if (playerPed.CurrentVehicle.Driver == playerPed && playerPed.CurrentVehicle.Speed < 10)
                {
                    if (playerPed.CurrentVehicle.IsDead || playerPed.CurrentVehicle.IsOnFire) HUD.ShowAdvancedNotification("Assicurazione", "Versamento Indennizzo", "Dato che al momento dell'incidente il veicolo non eccedeva il limite di velocità e non è stato causa principale dell'incidente stesso, le verrà rimborsata una parte del costo del veicolo.", NotificationIcon.MorsMutual, TipoIcona.DollarIcon);
                }
                else if (playerPed.CurrentVehicle.Driver == playerPed && playerPed.CurrentVehicle.Speed > 10)
                {
                    if (playerPed.CurrentVehicle.IsDead || playerPed.CurrentVehicle.IsOnFire) HUD.ShowAdvancedNotification("Assicurazione", "Versamento Indennizzo", "Dato che al momento dell'incidente il veicolo eccedeva il limite di velocità non è stato possibile rimborsare un indennizzo per il suddetto veicolo.", NotificationIcon.MorsMutual, TipoIcona.DollarIcon);
                }
            }
        }
    }
}