using CitizenFX.Core;
using NuovaGM.Server.gmPrincipale;
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
