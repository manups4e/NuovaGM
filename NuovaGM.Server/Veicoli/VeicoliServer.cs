using System;
using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core;
using Logger;
using Newtonsoft.Json;
using TheLastPlanet.Server.Core;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.SistemaEventi;
using TheLastPlanet.Shared.Veicoli;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Server.Veicoli
{
	internal static class VeicoliServer
	{
		private static string lasthost = "";

		public static void Init()
		{
			Server.Instance.AddEventHandler("lprp:onPlayerSpawn", new Action<Player>(onPlayerSpawn));
			Server.Instance.AddEventHandler("lprp:lvc_TogIndicState_s", new Action<Player, int>(lvc_TogIndicState_s));
			Server.Instance.AddEventHandler("lprp:SilentSiren", new Action<Player, bool>(SilentSiren));
			Server.Instance.AddEventHandler("brakes:add_rear", new Action<int>(AddRear));
			Server.Instance.AddEventHandler("brakes:add_front", new Action<int>(AddFront));
			Server.Instance.AddEventHandler("brakes:rem_rear", new Action<int>(RemRear));
			Server.Instance.AddEventHandler("brakes:rem_front", new Action<int>(RemFront));
			Server.Instance.AddEventHandler("lprp:vehInGarage", new Action<Player, string, bool, string>(InGarage));
			Server.Instance.Eventi.Attach("lprp:caricaVeicoli", new AsyncEventCallback(async a =>
			{
				Player player = Funzioni.GetPlayerFromId(a.Sender);
				IEnumerable<OwnedVehicle> vehs = await MySQL.QueryListAsync<OwnedVehicle>("SELECT * FROM owned_vehicles WHERE discord = @disc AND char_id = @pers", new { disc = player.GetLicense(Identifier.Discord), pers = player.GetCurrentChar().FullName });
				List<OwnedVehicle> ownedVehicles = vehs.ToList();

				if (ownedVehicles.Count <= 0) return player.GetCurrentChar().CurrentChar.Veicoli;
				foreach (dynamic veh in ownedVehicles) player.GetCurrentChar().CurrentChar.Veicoli.Add(new OwnedVehicle(veh));

				return player.GetCurrentChar().CurrentChar.Veicoli;
			}));
		}

		public static async void onPlayerSpawn([FromSource] Player p)
		{
			await BaseScript.Delay(0);

			if (p.Handle == "1")
			{
				Debug.WriteLine("train timeout activated.");
				await BaseScript.Delay(15000);
				activateTrain();
				//BaseScript.TriggerClientEvent("lprp:getHost", host);
			}
		}

		public static void SilentSiren([FromSource] Player player, bool toggle) { BaseScript.TriggerClientEvent("lprp:updateSirens", player.Handle, toggle); }

		public static void lvc_TogIndicState_s([FromSource] Player player, int newstate) { BaseScript.TriggerClientEvent("lprp:lvc_TogIndicState_c", player.Handle, newstate); }

		public static void activateTrain() { BaseScript.TriggerClientEvent("lprp:spawntrain"); }

		private static void AddRear(int veh) { BaseScript.TriggerClientEvent("cBrakes:add_rear", veh); }
		private static void AddFront(int veh) { BaseScript.TriggerClientEvent("cBrakes:add_front", veh); }
		private static void RemRear(int veh) { BaseScript.TriggerClientEvent("cBrakes:rem_rear", veh); }
		private static void RemFront(int veh) { BaseScript.TriggerClientEvent("cBrakes:rem_front", veh); }

		private static async void InGarage([FromSource] Player p, string plate, bool inGarage, string props)
		{
			p.GetCurrentChar().CurrentChar.Veicoli.FirstOrDefault(x => x.Targa == plate).Garage.InGarage = inGarage;

			if (inGarage)
			{
				p.GetCurrentChar().CurrentChar.Veicoli.FirstOrDefault(x => x.Targa == plate).DatiVeicolo.props = props.DeserializeFromJson<VehProp>(true);
				await Server.Instance.Execute("Update owned_vehicles set Garage = @gar, vehicle_data = @dat WHERE targa = @t", new { gar = p.GetCurrentChar().CurrentChar.Veicoli.FirstOrDefault(x => x.Targa == plate).Garage.SerializeToJson(), dat = p.GetCurrentChar().CurrentChar.Veicoli.FirstOrDefault(x => x.Targa == plate).DatiVeicolo.SerializeToJson(includeEverything: true), t = plate });
			}
			else
			{
				await Server.Instance.Execute("Update owned_vehicles set Garage = @gar WHERE targa = @t", new { gar = p.GetCurrentChar().CurrentChar.Veicoli.FirstOrDefault(x => x.Targa == plate).Garage.SerializeToJson(), t = plate });
			}

			p.TriggerEvent("lprp:sendUserInfo", p.GetCurrentChar().Characters.SerializeToJson(includeEverything: true), p.GetCurrentChar().char_current, p.GetCurrentChar().group);
		}
	}
}