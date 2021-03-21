﻿using System;
using System.Collections.Generic;
using CitizenFX.Core;
using TheLastPlanet.Server.Core;
using TheLastPlanet.Shared;

namespace TheLastPlanet.Server.Lavori.Whitelistati
{
	static class PoliziaServer
	{
		static List<VeicoloLavorativoEAffitto> NonPersonali = new List<VeicoloLavorativoEAffitto>();
		static List<VeicoloPol> Polizia = new List<VeicoloPol>();
		static List<VeicoloPol> Medici = new List<VeicoloPol>();

		public static void Init()
		{
			ServerSession.Instance.AddEventHandler("lprp:registraVeicoloLavorativoENon", new Action<string>(RegistraVeicoloLavoroEAffitto));
			ServerSession.Instance.AddEventHandler("lprp:rimuoviVeicoloLavorativoENon", new Action<string>(RimuoviVeicoloLavoroEAffitto));
			ServerSession.Instance.AddEventHandler("lprp:registraVeicoloPersonale", new Action<string>(RegistraVeicoloPersonale));
			ServerSession.Instance.AddEventHandler("lprp:rimuoviVeicoloPersonale", new Action<string>(RimuoviVeicoloPersonale));
			ServerSession.Instance.AddEventHandler("lprp:polizia:AggiungiVehMedici", new Action<string>(AggiungiVehMedici));
			ServerSession.Instance.AddEventHandler("lprp:polizia:RimuoviVehMedici", new Action<string>(RimuoviVehMedici));
			ServerSession.Instance.AddEventHandler("lprp:polizia:AggiungiVehPolizia", new Action<string>(AggiungiVehPolizia));
			ServerSession.Instance.AddEventHandler("lprp:polizia:RimuoviVehPolizia", new Action<string>(RimuoviVehPolizia));
			ServerSession.Instance.AddEventHandler("lprp:polizia:ammanetta_smanetta", new Action<Player, int>(AmmanettaSmanetta));
			ServerSession.Instance.AddEventHandler("lprp:polizia:accompagna", new Action<Player, int, int>(Accompagna));
			ServerSession.Instance.AddEventHandler("lprp:polizia:mettiVeicolo", new Action<Player, int>(MettiVeh));
			ServerSession.Instance.AddEventHandler("lprp:polizia:esciVeicolo", new Action<Player, int>(TogliVeh));
			//Server.Instance.AddEventHandler("lprp:polizia:ammanetta_smanetta", new Action<Player, int>(AmmanettaSmanetta));

			//Server.Instance.AddTick(AggiornamentoClient);

		}

		public static void PlaccaServer([FromSource] Player p, int target)
		{
			Player targetPlayer = ServerSession.Instance.GetPlayers[target];
			BaseScript.TriggerClientEvent(targetPlayer, "lprp:police:placcato", p.Handle);
			BaseScript.TriggerClientEvent(p, "lprp:police:placca");
		}

		public static void RegistraVeicoloLavoroEAffitto(string jsonVeicolo)
		{
			NonPersonali.Add(jsonVeicolo.FromJson<VeicoloLavorativoEAffitto>());
		}

		private static void RimuoviVeicoloLavoroEAffitto(string jsonVeicolo)
		{
			foreach (VeicoloLavorativoEAffitto veicolo in NonPersonali)
			{
				if (veicolo == jsonVeicolo.FromJson<VeicoloLavorativoEAffitto>())
				{
					NonPersonali.Remove(veicolo);
				}
			}
		}

		public static void RegistraVeicoloPersonale(string jsonVeicolo)
		{
			//Personali.Add(JsonConvert.DeserializeObject<VeicoloPersonale>(jsonVeicolo));
		}

		private static void RimuoviVeicoloPersonale(string jsonVeicolo)
		{
			/*
			foreach (var veicolo in Personali)
			{
				if (veicolo == JsonConvert.DeserializeObject<VeicoloPersonale>(jsonVeicolo))
				{
					Personali.Remove(veicolo);
				}
			}
			*/
		}

		private static void AggiungiVehPolizia(string jsonVeicolo)
		{
			VeicoloPol agg = jsonVeicolo.FromJson<VeicoloPol>();
			if (!Polizia.Contains(agg))
				Polizia.Add(agg);
		}

		private static void RimuoviVehPolizia(string jsonVeicolo)
		{
			VeicoloPol agg = jsonVeicolo.FromJson<VeicoloPol>();
			if (Polizia.Contains(agg))
				Polizia.Remove(agg);
		}

		private static void AggiungiVehMedici(string jsonVeicolo)
		{
			VeicoloPol agg = jsonVeicolo.FromJson<VeicoloPol>();
			if (!Medici.Contains(agg))
				Medici.Add(agg);
		}

		private static void RimuoviVehMedici(string jsonVeicolo)
		{
			VeicoloPol agg = jsonVeicolo.FromJson<VeicoloPol>();
			if (Medici.Contains(agg))
				Medici.Remove(agg);
		}

		/*
				static bool firstTick = true;
				public static async Task AggiornamentoClient()
				{
					if (firstTick)
					{
						firstTick = false;
						dynamic result = await Server.Instance.Query($"SELECT * FROM veicolipersonali");
						await BaseScript.Delay(0);
						for (int i = 0; i < result.Count; i++)
							Personali.Add(new VeicoloPersonale(result[i].IsVehicleStored, result[i].CharOwner, result[i].identifier, result[i].vehiclename, result[i].plate, result[i].NameOwner, result[i].NAssicurazione, JsonConvert.DeserializeObject<VehProp>(result[i].mods)));
					}
					BaseScript.TriggerClientEvent("lprp:police:aggiornaListe", JsonConvert.SerializeObject(NonPersonali), JsonConvert.SerializeObject(Personali));
					await BaseScript.Delay(60000);
				}
		*/
		public static void AmmanettaSmanetta([FromSource] Player p, int target)
		{
			Player targ = Funzioni.GetPlayerFromId(target);
			if (p.GetCurrentChar().CurrentChar.Job.name.ToLower() == "polizia")
				targ.TriggerEvent("lprp:polizia:ammanetta_smanetta");
			else
				p.Drop("Hai tentato di ammanettare un Player senza permesso!");
		}

		public static void Accompagna([FromSource] Player p, int target, int ped)
		{
			Player targ = Funzioni.GetPlayerFromId(target);
			if (p.GetCurrentChar().CurrentChar.Job.name.ToLower() == "polizia")
				targ.TriggerEvent("lprp:polizia:accompagna", ped);
			else
				p.Drop("Hai tentato di trasportare un Player senza permesso!");
		}

		public static void MettiVeh([FromSource] Player p, int target)
		{
			Player targ = Funzioni.GetPlayerFromId(target);
			if (p.GetCurrentChar().CurrentChar.Job.name.ToLower() == "polizia")
				targ.TriggerEvent("lprp:polizia:mettiveh");
			else
				p.Drop("Hai tentato di mettere un Player in un veicolo senza permesso!");
		}
		public static void TogliVeh([FromSource] Player p, int target)
		{
			Player targ = Funzioni.GetPlayerFromId(target);
			if (p.GetCurrentChar().CurrentChar.Job.name.ToLower() == "polizia")
				targ.TriggerEvent("lprp:polizia:togliveh");
			else
				p.Drop("Hai tentato di togliere un Player da un veicolo senza permesso!");
		}
	}
}