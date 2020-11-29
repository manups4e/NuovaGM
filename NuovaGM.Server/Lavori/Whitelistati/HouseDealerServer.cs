using CitizenFX.Core;
using Logger;
using Newtonsoft.Json.Linq;
using TheLastPlanet.Server.Core;
using TheLastPlanet.Server.Proprietà;
using TheLastPlanet.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheLastPlanet.Server.Lavori.Whitelistati
{
	static class HouseDealerServer
	{
		public static void Init()
		{
			Server.Instance.AddEventHandler("housedealer:vendi", new Action<Player, bool, int, string, int>(Vendi));
		}

		private static async void Vendi([FromSource] Player p, bool venduto, int target, string jsonProperty, int prezzo)
		{
			Player acquirente = Funzioni.GetPlayerFromId(target);
			KeyValuePair<string, JContainer> casa = jsonProperty.Deserialize<KeyValuePair<string, JContainer>>();
			SoldProperty prop = new SoldProperty(acquirente.GetLicense(Identifier.Discord), acquirente.GetCurrentChar().FullName, "Appartamento", !venduto, prezzo, casa.Key, casa.Value["Label"].Value<string>(), "{}", "{}", "{}", "{}", DateTime.Now.AddDays(7), DateTime.Now);
			await Server.Instance.Execute("INSERT INTO proprietà Values(@disc, @pers, @tipo, @aff, @pr, @name, @label, @gar, @gua, @inv, @arm, @boll, @acq)", new
			{
				disc = prop.DiscordId,
				pers = prop.Personaggio,
				tipo = prop.Tipo,
				aff = prop.InAffitto,
				pr = prop.Prezzo,
				name = prop.Name,
				label = prop.Label,
				gar = prop.Garage,
				gua = prop.Guardaroba,
				inv = prop.Inventario,
				arm = prop.Armeria,
				boll = prop.Bollette,
				acq = prop.Acquisto
			});
			p.GetCurrentChar().showNotification($"Hai {(venduto ? "venduto" : "affittato")} ~y~{prop.Label}~w~ a ~o~{acquirente.GetCurrentChar().FullName}~w~.");
			acquirente.GetCurrentChar().showNotification($"Hai {(venduto ? "comprato" : "affittato")} un nuovo appartamento: ~y~{prop.Label}~w~.");
		}
	}
}
