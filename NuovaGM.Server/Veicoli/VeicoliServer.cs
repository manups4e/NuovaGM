using System;
using CitizenFX.Core;
using Logger;
using Newtonsoft.Json;
using NuovaGM.Server.gmPrincipale;
using NuovaGM.Shared;
using NuovaGM.Shared.Veicoli;
using static CitizenFX.Core.Native.API;


namespace NuovaGM.Server.Veicoli
{
	static class VeicoliServer
	{

		static string lasthost = "";
		public static void Init()
		{
			Server.Instance.AddEventHandler("lprp:onPlayerSpawn", new Action<Player>(onPlayerSpawn));
			Server.Instance.AddEventHandler("lprp:lvc_TogIndicState_s", new Action<Player, int>(lvc_TogIndicState_s));
			Server.Instance.AddEventHandler("lprp:SilentSiren", new Action<Player, bool>(SilentSiren));
			Server.Instance.AddEventHandler("brakes:add_rear", new Action<int>(AddRear));
			Server.Instance.AddEventHandler("brakes:add_front", new Action<int>(AddFront));
			Server.Instance.AddEventHandler("brakes:rem_rear", new Action<int>(RemRear));
			Server.Instance.AddEventHandler("brakes:rem_front", new Action<int>(RemFront));
			Server.Instance.AddEventHandler("lprp:caricaVeicoli", new Action<Player>(CaricaVeicoli));
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

		public static void SilentSiren([FromSource]Player player, bool toggle) => BaseScript.TriggerClientEvent("lprp:updateSirens", player.Handle, toggle);

		public static void lvc_TogIndicState_s([FromSource] Player player, int newstate) => BaseScript.TriggerClientEvent("lprp:lvc_TogIndicState_c", player.Handle, newstate);

		public static void activateTrain() => BaseScript.TriggerClientEvent("lprp:spawntrain");


		private static void AddRear(int veh) 
		{ 
			BaseScript.TriggerClientEvent("cBrakes:add_rear", veh); 
		}
		private static void AddFront(int veh) 
		{ 
			BaseScript.TriggerClientEvent("cBrakes:add_front", veh);
		}
		private static void RemRear(int veh) 
		{ 
			BaseScript.TriggerClientEvent("cBrakes:rem_rear", veh);
		}
		private static void RemFront(int veh) 
		{ 
			BaseScript.TriggerClientEvent("cBrakes:rem_front", veh);
		}

		private static async void CaricaVeicoli([FromSource] Player p)
		{
			try
			{
				dynamic vehs = await Server.Instance.Query("Select * from owned_vehicles where discord = @disc and char_id = @pers", new
				{
					disc = License.GetLicense(p, Identifier.Discord),
					pers = p.GetCurrentChar().CurrentChar.id
				});
				if (vehs != null && vehs.Count > 0)
					foreach (var veh in vehs)
						p.GetCurrentChar().CurrentChar.Veicoli.Add(new OwnedVehicle(veh.targa, (veh.vehicle_data as string).Deserialize<VehicleData>(), veh.in_garage, veh.stato));
			}
			catch(Exception e)
			{
				Log.Printa(LogType.Error, "Errore per il player " + p.Name + "\n" + e.ToString());
			}
		}
	}
}
