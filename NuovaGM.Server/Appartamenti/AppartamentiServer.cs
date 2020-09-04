using CitizenFX.Core;
using Logger;
using NuovaGM.Server.gmPrincipale;
using System;
using System.Collections.Generic;
using System.Text;

namespace NuovaGM.Server.Appartamenti
{
	static class AppartamentiServer
	{
		public static void Init()
		{
			Server.Instance.AddEventHandler("lprp:citofonaAlPlayer", new Action<Player, int, string>(Citofono));
			Server.Instance.AddEventHandler("lprp:citofono:puoEntrare", new Action<Player, int, string>(PuòEntrare));
		}

		private static void Citofono([FromSource] Player citofonante, int citofonatoServerId, string app)
		{
			Player citofonato = Funzioni.GetPlayerFromId(citofonatoServerId);
			citofonato.TriggerEvent("lprp:richiestaDiEntrare", Convert.ToInt32(citofonato.Handle), app);
		}

		private static void PuòEntrare([FromSource] Player inCasa, int citofonanteServerId, string app)
		{
			Player p = Funzioni.GetPlayerFromId(citofonanteServerId);
			p.TriggerEvent("lprp:citofono:puoiEntrare", Convert.ToInt32(inCasa.Handle), app);
		}
	}
}
