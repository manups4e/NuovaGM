using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static NuovaGM.Shared.Veicoli.Modifiche;
using Newtonsoft.Json;
using NuovaGM.Server.gmPrincipale;
using NuovaGM.Shared;

namespace NuovaGM.Server.Lavori.Whitelistati
{
	static class MediciServer
	{
		private static List<int> Morti = new List<int>();
		public static void Init()
		{
			Server.Instance.RegisterEventHandler("lprp:onPlayerSpawn", new Action<Player>(Spawnato));
			Server.Instance.RegisterEventHandler("lprp:onPlayerDeath", new Action<Player>(PlayerMorto));
			Server.Instance.RegisterEventHandler("lprp:medici:rimuoviDaMorti", new Action<Player>(PlayerVivo));
		}

		private static async void PlayerMorto([FromSource] Player player)
		{
			if (!Morti.Contains(Convert.ToInt32(player.Handle)))
			{
				Morti.Add(Convert.ToInt32(player.Handle));
				BaseScript.TriggerClientEvent("lprp:medici:aggiungiPlayerAiMorti", Convert.ToInt32(player.Handle));
			}
		}

		private static async void PlayerVivo([FromSource] Player player)
		{
			if (Morti.ToList().Contains(Convert.ToInt32(player.Handle)))
			{
				Morti.Remove(Convert.ToInt32(player.Handle));
				BaseScript.TriggerClientEvent("lprp:medici:rimuoviPlayerAiMorti", Convert.ToInt32(player.Handle));
			}
		}

		private static async void Spawnato([FromSource] Player player)
		{
			for (int i=0; i<Morti.Count; i++)
				player.TriggerEvent("lprp:medici:aggiungiPlayerAiMorti", Morti[i]);
		}
	}
}
