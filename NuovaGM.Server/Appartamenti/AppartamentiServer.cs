using CitizenFX.Core;
using Logger;
using Newtonsoft.Json;
using NuovaGM.Server.gmPrincipale;
using NuovaGM.Shared;
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
			Server.Instance.AddEventHandler("lprp:caricaAppartamenti", new Action<Player>(CaricaProprietà));
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

		private static async void CaricaProprietà([FromSource] Player p)
		{
			dynamic result = await Server.Instance.Query("SELECT * FROM proprietà WHERE DiscordId = @id AND Personaggio = @pers", new
			{
				id = p.GetLicense(Identifier.Discord),
				pers = p.GetCurrentChar().FullName
			});
			if (result.Count > 0)
				foreach (var ap in result)
					p.GetCurrentChar().CurrentChar.Proprietà.Add(ap.Name);
			p.TriggerEvent("lprp:sendUserInfo", p.GetCurrentChar().char_data.Serialize(), p.GetCurrentChar().char_current, p.GetCurrentChar().group);
		}
	}
}
