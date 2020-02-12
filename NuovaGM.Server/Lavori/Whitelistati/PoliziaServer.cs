using CitizenFX.Core;
using Newtonsoft.Json;
using NuovaGM.Server.gmPrincipale;
using NuovaGM.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static NuovaGM.Shared.Veicoli.Modifiche;

namespace NuovaGM.Server.Lavori.Whitelistati
{
	static class PoliziaServer
	{
		static List<VeicoloLavorativoEAffitto> NonPersonali = new List<VeicoloLavorativoEAffitto>();
		static List<VeicoloPersonale> Personali = new List<VeicoloPersonale>();
		static List<VeicoloPol> Polizia = new List<VeicoloPol>();
		static List<VeicoloPol> Medici = new List<VeicoloPol>();

		public static void Init()
		{
			Server.GetInstance.RegisterEventHandler("lprp:registraVeicoloLavorativoENon", new Action<string>(RegistraVeicoloLavoroEAffitto));
			Server.GetInstance.RegisterEventHandler("lprp:rimuoviVeicoloLavorativoENon", new Action<string>(RimuoviVeicoloLavoroEAffitto));
			Server.GetInstance.RegisterEventHandler("lprp:registraVeicoloPersonale", new Action<string>(RegistraVeicoloPersonale));
			Server.GetInstance.RegisterEventHandler("lprp:rimuoviVeicoloPersonale", new Action<string>(RimuoviVeicoloPersonale));
			Server.GetInstance.RegisterEventHandler("lprp:polizia:AggiungiVehMedici", new Action<string>(AggiungiVehMedici));
			Server.GetInstance.RegisterEventHandler("lprp:polizia:RimuoviVehMedici", new Action<string>(RimuoviVehMedici));
			Server.GetInstance.RegisterEventHandler("lprp:polizia:AggiungiVehPolizia", new Action<string>(AggiungiVehPolizia));
			Server.GetInstance.RegisterEventHandler("lprp:polizia:RimuoviVehPolizia", new Action<string>(RimuoviVehPolizia));
			Server.GetInstance.RegisterEventHandler("lprp:polizia:ammanetta/smanetta", new Action<Player, int>(AmmanettaSmanetta));

			//Server.GetInstance.RegisterTickHandler(AggiornamentoClient);

		}

		public static void PlaccaServer([FromSource] Player p, int target)
		{
			Player targetPlayer = Server.GetInstance.GetPlayers[target];
			BaseScript.TriggerClientEvent(targetPlayer, "lprp:police:placcato", p.Handle);
			BaseScript.TriggerClientEvent(p, "lprp:police:placca");
		}

		public static void RegistraVeicoloLavoroEAffitto(string jsonVeicolo)
		{
			NonPersonali.Add(JsonConvert.DeserializeObject<VeicoloLavorativoEAffitto>(jsonVeicolo));
		}

		private static void RimuoviVeicoloLavoroEAffitto(string jsonVeicolo)
		{
			foreach (var veicolo in NonPersonali)
			{
				if (veicolo == JsonConvert.DeserializeObject<VeicoloLavorativoEAffitto>(jsonVeicolo))
				{
					NonPersonali.Remove(veicolo);
				}
			}
		}

		public static void RegistraVeicoloPersonale(string jsonVeicolo)
		{
			Personali.Add(JsonConvert.DeserializeObject<VeicoloPersonale>(jsonVeicolo));
		}

		private static void RimuoviVeicoloPersonale(string jsonVeicolo)
		{
			foreach (var veicolo in Personali)
			{
				if (veicolo == JsonConvert.DeserializeObject<VeicoloPersonale>(jsonVeicolo))
				{
					Personali.Remove(veicolo);
				}
			}
		}

		private static async void AggiungiVehPolizia(string jsonVeicolo)
		{
			VeicoloPol agg = JsonConvert.DeserializeObject<VeicoloPol>(jsonVeicolo);
			if (!Polizia.Contains(agg))
				Polizia.Add(agg);
		}

		private static async void RimuoviVehPolizia(string jsonVeicolo)
		{
			VeicoloPol agg = JsonConvert.DeserializeObject<VeicoloPol>(jsonVeicolo);
			if (Polizia.Contains(agg))
				Polizia.Remove(agg);
		}

		private static async void AggiungiVehMedici(string jsonVeicolo)
		{
			VeicoloPol agg = JsonConvert.DeserializeObject<VeicoloPol>(jsonVeicolo);
			if (!Medici.Contains(agg))
				Medici.Add(agg);
		}

		private static async void RimuoviVehMedici(string jsonVeicolo)
		{
			VeicoloPol agg = JsonConvert.DeserializeObject<VeicoloPol>(jsonVeicolo);
			if (Medici.Contains(agg))
				Medici.Remove(agg);
		}


		static bool firstTick = true;
		public static async Task AggiornamentoClient()
		{
			if (firstTick)
			{
				firstTick = false;
				dynamic result = await Server.GetInstance.Query($"SELECT * FROM veicolipersonali");
				for (int i = 0; i < result.Count; i++)
					Personali.Add(new VeicoloPersonale(result[i].IsVehicleStored, result[i].CharOwner, result[i].identifier, result[i].vehiclename, result[i].plate, result[i].NameOwner, result[i].NAssicurazione, JsonConvert.DeserializeObject<VehProp>(result[i].mods)));
			}
			BaseScript.TriggerClientEvent("lprp:police:aggiornaListe", JsonConvert.SerializeObject(NonPersonali), JsonConvert.SerializeObject(Personali));
			await BaseScript.Delay(60000);
		}

		public static async void AmmanettaSmanetta([FromSource] Player p, int target)
		{
			Player targ = Funzioni.GetPlayerFromId(target);
			User player = Funzioni.GetUserFromPlayerId(p.Handle);
			if (player.CurrentChar.job.name == "Polizia")
				targ.TriggerEvent("lprp:polizia:ammanetta/smanetta");
			else
				p.Drop("Hai tentato di ammanettare un Player senza permesso!");
		}
	}
}
