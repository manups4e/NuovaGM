using CitizenFX.Core;
using TheLastPlanet.Server.Core;
using TheLastPlanet.Shared;
using System;

namespace TheLastPlanet.Server.Appartamenti
{
	internal static class AppartamentiServer
	{
		public static void Init()
		{
			ServerSession.Instance.AddEventHandler("lprp:citofonaAlPlayer", new Action<Player, int, string>(Citofono));
			ServerSession.Instance.AddEventHandler("lprp:citofono:puoEntrare", new Action<Player, int, string>(PuòEntrare));
			ServerSession.Instance.AddEventHandler("lprp:caricaAppartamenti", new Action<Player>(CaricaProprietà));
			ServerSession.Instance.AddEventHandler("lprp:entraGarageConProprietario", new Action<Player, int, Vector3>(EntraConProprietario));
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
			dynamic result = await ServerSession.Instance.Query("SELECT * FROM proprietà WHERE DiscordId = @id AND Personaggio = @pers", new { id = p.GetLicense(Identifier.Discord), pers = p.GetCurrentChar().FullName });
			if (result.Count > 0)
				foreach (dynamic ap in result)
					p.GetCurrentChar().CurrentChar.Proprietà.Add(ap.Name);
			//p.TriggerEvent("lprp:sendUserInfo", p.GetCurrentChar().Characters.SerializeToJson(includeEverything: true), p.GetCurrentChar().char_current, p.GetCurrentChar().group);
		}

		private static void EntraConProprietario([FromSource] Player p, int serverId, Vector3 pos)
		{
			Player sd = Funzioni.GetPlayerFromId(serverId);
			sd.TriggerEvent("lprp:entraGarageConProprietario", pos);
		}
	}
}