using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using Newtonsoft.Json;
using TheLastPlanet.Server.Core;


namespace TheLastPlanet.Server.Lavori.Whitelistati
{
	static class MediciServer
	{
		private static List<int> Morti = new List<int>();
		public static void Init()
		{
			Server.Instance.AddEventHandler("lprp:onPlayerSpawn", new Action<Player>(Spawnato));
			Server.Instance.AddEventHandler("lprp:onPlayerDeath", new Action<Player>(PlayerMorto));
			Server.Instance.AddEventHandler("lprp:medici:rimuoviDaMorti", new Action<Player>(PlayerVivo));
		}

		private static void PlayerMorto([FromSource] Player player)
		{
			if (!Morti.Contains(Convert.ToInt32(player.Handle)))
			{
				Morti.Add(Convert.ToInt32(player.Handle));
				BaseScript.TriggerClientEvent("lprp:medici:aggiungiPlayerAiMorti", Convert.ToInt32(player.Handle));
			}
		}

		private static void PlayerVivo([FromSource] Player player)
		{
			if (Morti.ToList().Contains(Convert.ToInt32(player.Handle)))
			{
				Morti.Remove(Convert.ToInt32(player.Handle));
				BaseScript.TriggerClientEvent("lprp:medici:rimuoviPlayerAiMorti", Convert.ToInt32(player.Handle));
			}
		}

		private static void Spawnato([FromSource] Player player)
		{
			for (int i=0; i<Morti.Count; i++)
				player.TriggerEvent("lprp:medici:aggiungiPlayerAiMorti", Morti[i]);
		}
	}
}
