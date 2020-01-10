using CitizenFX.Core;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using System.Threading.Tasks;

namespace NuovaGM.Client.Veicoli
{
	static class Assicurazione
	{
		public static void Init()
		{

		}

		private static async Task ControlloAssicurazione()
		{
			if (Game.PlayerPed.IsInVehicle() && Game.PlayerPed.CurrentVehicle.Driver != Game.PlayerPed)
			{
				if (Game.PlayerPed.CurrentVehicle.IsOnFire || Game.PlayerPed.CurrentVehicle.IsDead)
				{
					HUD.ShowAdvancedNotification("Assicurazione", "Versamento Indennizzo", "Dato che era nel veicolo alla distruzione ma non era guidatore, le verrà rimborsata una parte del costo del veicolo.", NotificationIcon.MorsMutual, IconType.DollarIcon);
				}

				if (Game.PlayerPed.CurrentVehicle.Driver == Game.PlayerPed && Game.PlayerPed.CurrentVehicle.Speed < 10)
				{
					if (Game.PlayerPed.CurrentVehicle.IsDead || Game.PlayerPed.CurrentVehicle.IsOnFire)
					{
						HUD.ShowAdvancedNotification("Assicurazione", "Versamento Indennizzo", "Dato che al momento dell'incidente il veicolo non eccedeva il limite di velocità e non è stato causa principale dell'incidente stesso, le verrà rimborsata una parte del costo del veicolo.", NotificationIcon.MorsMutual, IconType.DollarIcon);
					}
				}
				else if (Game.PlayerPed.CurrentVehicle.Driver == Game.PlayerPed && Game.PlayerPed.CurrentVehicle.Speed > 10)
				{
					if (Game.PlayerPed.CurrentVehicle.IsDead || Game.PlayerPed.CurrentVehicle.IsOnFire)
					{
						HUD.ShowAdvancedNotification("Assicurazione", "Versamento Indennizzo", "Dato che al momento dell'incidente il veicolo eccedeva il limite di velocità non è stato possibile rimborsare un indennizzo per il suddetto veicolo.", NotificationIcon.MorsMutual, IconType.DollarIcon);
					}
				}
			}
		}
	}
}
