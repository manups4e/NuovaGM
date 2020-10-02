using CitizenFX.Core;
using NuovaGM.Server.gmPrincipale;
using NuovaGM.Shared;
using NuovaGM.Shared.Veicoli;
using System;
using System.Collections.Generic;
using System.Text;

namespace NuovaGM.Server.Lavori.Whitelistati
{
	static class CarDealerServer
	{
		public static void Init()
		{
			Server.Instance.AddEventHandler("lprp:cardealer:attivaCatalogoAlcuni", new Action<Player, List<int>>(CatalogoAlcuni));
			Server.Instance.AddEventHandler("lprp:cardealer:cambiaVehCatalogo", new Action<Player, List<int>, string>(CambiaVeh));
			Server.Instance.AddEventHandler("lprp:cardealer:vendiVehAMe", new Action<Player, string>(VendiAMe));
		}

		private static async void VendiAMe([FromSource] Player p, string JsonVeh)
		{
			OwnedVehicle veh = JsonVeh.Deserialize<OwnedVehicle>();
			await Server.Instance.Execute("INSERT INTO owned_vehicles VALUES (@disc, @name, @charname, @plate, @vehN, @data, @garage, @state)", new
			{
				disc = p.GetLicense(Identifier.Discord),
				name = p.Name,
				charname = p.GetCurrentChar().FullName,
				plate = veh.Targa,
				vehN = veh.DatiVeicolo.props.Name,
				data = veh.DatiVeicolo.Serialize(),
				garage = veh.InGarage,
				state = veh.Stato,
			});
		}

		private static void CatalogoAlcuni([FromSource] Player p, List<int> players)
		{
			p.TriggerEvent("lprp:cardealer:catalogoAlcuni", true, players);
			players.ForEach(x => Funzioni.GetPlayerFromId(x).TriggerEvent("lprp:cardealer:catalogoAlcuni", false, players));
		}
		private static void CambiaVeh([FromSource] Player p, List<int> players, string vehName)
		{
			p.TriggerEvent("lprp:cardealer:catalogoAlcuni", true, vehName);
			players.ForEach(x => Funzioni.GetPlayerFromId(x).TriggerEvent("lprp:cardealer:catalogoAlcuni", false, vehName));
		}
	}
}
