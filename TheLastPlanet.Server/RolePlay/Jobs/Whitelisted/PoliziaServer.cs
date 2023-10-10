using System;
using System.Collections.Generic;
using TheLastPlanet.Server.Core;

namespace TheLastPlanet.Server.Jobs.Whitelisted
{
    static class PoliziaServer
    {
        static List<JobVeh_Rent> NonPersonal = new List<JobVeh_Rent>();
        static List<VehiclePolice> Polizia = new List<VehiclePolice>();
        static List<VehiclePolice> Medics = new List<VehiclePolice>();

        public static void Init()
        {
            Server.Instance.AddEventHandler("lprp:registraVeicoloLavorativoENon", new Action<string>(RegisterVehicleWorkAndRent));
            Server.Instance.AddEventHandler("lprp:rimuoviVeicoloLavorativoENon", new Action<string>(RemoveVehicleWorkAndRent));
            Server.Instance.AddEventHandler("lprp:registraVeicoloPersonale", new Action<string>(RegisterPersonalVehicle));
            Server.Instance.AddEventHandler("lprp:rimuoviVeicoloPersonale", new Action<string>(RemovePersonalVehicle));
            Server.Instance.AddEventHandler("lprp:polizia:AggiungiVehMedici", new Action<string>(AddVehMedics));
            Server.Instance.AddEventHandler("lprp:polizia:RimuoviVehMedici", new Action<string>(RemoveVehMedics));
            Server.Instance.AddEventHandler("lprp:polizia:AggiungiVehPolizia", new Action<string>(AddVehPolice));
            Server.Instance.AddEventHandler("lprp:polizia:RimuoviVehPolizia", new Action<string>(RemoveVehPolice));
            Server.Instance.AddEventHandler("lprp:polizia:ammanetta_smanetta", new Action<Player, int>(Cuff_Uncuff));
            Server.Instance.AddEventHandler("lprp:polizia:accompagna", new Action<Player, int, int>(Drag));
            Server.Instance.AddEventHandler("lprp:polizia:mettiVeicolo", new Action<Player, int>(PutInVeh));
            Server.Instance.AddEventHandler("lprp:polizia:esciVeicolo", new Action<Player, int>(RemoveFromVeh));
            //Server.Instance.AddEventHandler("lprp:polizia:ammanetta_smanetta", new Action<Player, int>(AmmanettaSmanetta));

            //Server.Instance.AddTick(AggiornamentoClient);

        }

        public static void PlaccaServer([FromSource] Player p, int target)
        {
            Player targetPlayer = Server.Instance.GetPlayers[target];
            BaseScript.TriggerClientEvent(targetPlayer, "lprp:police:placcato", p.Handle);
            BaseScript.TriggerClientEvent(p, "lprp:police:placca");
        }

        public static void RegisterVehicleWorkAndRent(string jsonVeicolo)
        {
            NonPersonal.Add(jsonVeicolo.FromJson<JobVeh_Rent>());
        }

        private static void RemoveVehicleWorkAndRent(string jsonVeicolo)
        {
            foreach (JobVeh_Rent veicolo in NonPersonal)
            {
                if (veicolo == jsonVeicolo.FromJson<JobVeh_Rent>())
                {
                    NonPersonal.Remove(veicolo);
                }
            }
        }

        public static void RegisterPersonalVehicle(string jsonVeicolo)
        {
            //Personali.Add(JsonConvert.DeserializeObject<VeicoloPersonale>(jsonVeicolo));
        }

        private static void RemovePersonalVehicle(string jsonVeicolo)
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

        private static void AddVehPolice(string jsonVeicolo)
        {
            VehiclePolice agg = jsonVeicolo.FromJson<VehiclePolice>();
            if (!Polizia.Contains(agg))
                Polizia.Add(agg);
        }

        private static void RemoveVehPolice(string jsonVeicolo)
        {
            VehiclePolice agg = jsonVeicolo.FromJson<VehiclePolice>();
            if (Polizia.Contains(agg))
                Polizia.Remove(agg);
        }

        private static void AddVehMedics(string jsonVeicolo)
        {
            VehiclePolice agg = jsonVeicolo.FromJson<VehiclePolice>();
            if (!Medics.Contains(agg))
                Medics.Add(agg);
        }

        private static void RemoveVehMedics(string jsonVeicolo)
        {
            VehiclePolice agg = jsonVeicolo.FromJson<VehiclePolice>();
            if (Medics.Contains(agg))
                Medics.Remove(agg);
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
        public static void Cuff_Uncuff([FromSource] Player p, int target)
        {
            Player targ = Functions.GetPlayerFromId(target);
            if (p.GetCurrentChar().CurrentChar.Job.Name.ToLower() == "police")
                targ.TriggerEvent("lprp:polizia:ammanetta_smanetta");
            else
                p.Drop("You attempted to handcuff a Player without permission!");
        }

        public static void Drag([FromSource] Player p, int target, int ped)
        {
            Player targ = Functions.GetPlayerFromId(target);
            if (p.GetCurrentChar().CurrentChar.Job.Name.ToLower() == "police")
                targ.TriggerEvent("lprp:polizia:accompagna", ped);
            else
                p.Drop("You attempted to transport a Player without permission!");
        }

        public static void PutInVeh([FromSource] Player p, int target)
        {
            Player targ = Functions.GetPlayerFromId(target);
            if (p.GetCurrentChar().CurrentChar.Job.Name.ToLower() == "pol" +
                "ice")
                targ.TriggerEvent("lprp:polizia:mettiveh");
            else
                p.Drop("You attempted to put a Player in a vehicle without permission!");
        }
        public static void RemoveFromVeh([FromSource] Player p, int target)
        {
            Player targ = Functions.GetPlayerFromId(target);
            if (p.GetCurrentChar().CurrentChar.Job.Name.ToLower() == "pol" +
                "ice")
                targ.TriggerEvent("lprp:polizia:togliveh");
            else
                p.Drop("You attempted to remove a Player from a vehicle without permission!");
        }
    }
}
