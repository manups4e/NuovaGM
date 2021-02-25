using CitizenFX.Core;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;
using TheLastPlanet.Client.Core;

namespace TheLastPlanet.Client.Veicoli
{
	static class Assicurazione
	{
		public static void Init()
		{

		}

		private static async Task ControlloAssicurazione()
		{
			if (Cache.PlayerPed.IsInVehicle() && Cache.PlayerPed.CurrentVehicle.Driver != Cache.PlayerPed)
			{
				if (Cache.PlayerPed.CurrentVehicle.IsOnFire || Cache.PlayerPed.CurrentVehicle.IsDead)
				{
					HUD.ShowAdvancedNotification("Assicurazione", "Versamento Indennizzo", "Dato che era nel veicolo alla distruzione ma non era guidatore, le verrà rimborsata una parte del costo del veicolo.", NotificationIcon.MorsMutual, IconType.DollarIcon);
				}

				if (Cache.PlayerPed.CurrentVehicle.Driver == Cache.PlayerPed&& Cache.PlayerPed.CurrentVehicle.Speed < 10)
				{
					if (Cache.PlayerPed.CurrentVehicle.IsDead || Cache.PlayerPed.CurrentVehicle.IsOnFire)
					{
						HUD.ShowAdvancedNotification("Assicurazione", "Versamento Indennizzo", "Dato che al momento dell'incidente il veicolo non eccedeva il limite di velocità e non è stato causa principale dell'incidente stesso, le verrà rimborsata una parte del costo del veicolo.", NotificationIcon.MorsMutual, IconType.DollarIcon);
					}
				}
				else if (Cache.PlayerPed.CurrentVehicle.Driver == Cache.PlayerPed&& Cache.PlayerPed.CurrentVehicle.Speed > 10)
				{
					if (Cache.PlayerPed.CurrentVehicle.IsDead || Cache.PlayerPed.CurrentVehicle.IsOnFire)
					{
						HUD.ShowAdvancedNotification("Assicurazione", "Versamento Indennizzo", "Dato che al momento dell'incidente il veicolo eccedeva il limite di velocità non è stato possibile rimborsare un indennizzo per il suddetto veicolo.", NotificationIcon.MorsMutual, IconType.DollarIcon);
					}
				}
			}
		}
	}
}
