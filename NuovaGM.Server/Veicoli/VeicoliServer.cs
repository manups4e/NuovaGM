using System;
using System.Linq;
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
			Server.Instance.AddEventHandler("lprp:vehInGarage", new Action<Player, string, bool>(InGarage));
			Server.Instance.RegisterServerCallback("caricaVeicoli", new Action<Player, Delegate, dynamic>(CaricaVeicoli));
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

		private static async void CaricaVeicoli([FromSource] Player p, Delegate cb, dynamic _)
		{
			try
			{
				dynamic vehs = await Server.Instance.Query("SELECT * FROM owned_vehicles WHERE discord = @disc AND char_id = @pers", new
				{
					disc = p.GetLicense(Identifier.Discord),
					pers = p.GetCurrentChar().FullName
				});
				if (vehs.Count > 0)
				{
					p.GetCurrentChar().CurrentChar.Veicoli.Clear();
					foreach (var veh in vehs)
					{
						p.GetCurrentChar().CurrentChar.Veicoli.Add(new OwnedVehicle(veh));
					}
				}
				p.TriggerEvent("lprp:sendUserInfo", p.GetCurrentChar().char_data.Serialize(includeEverything: true), p.GetCurrentChar().char_current, p.GetCurrentChar().group);
				cb.DynamicInvoke(p.GetCurrentChar().CurrentChar.Veicoli.Serialize(includeEverything: true));
			}
			catch (Exception e)
			{
				Log.Printa(LogType.Error, "Errore per il player " + p.Name + "\n" + e.ToString());
				cb.DynamicInvoke("");
			}
		}

		private static async void InGarage([FromSource] Player p, string plate, bool inGarage)
		{
			p.GetCurrentChar().CurrentChar.Veicoli.FirstOrDefault(x => x.Targa == plate).Garage.InGarage = false;
			await Server.Instance.Execute("Update owned_vehicles set Garage = @gar WHERE targa = @t", new
			{
				gar = p.GetCurrentChar().CurrentChar.Veicoli.FirstOrDefault(x => x.Targa == plate).Garage.Serialize(includeEverything: true),
				t = plate
			});
			p.TriggerEvent("lprp:sendUserInfo", p.GetCurrentChar().char_data.Serialize(includeEverything: true), p.GetCurrentChar().char_current, p.GetCurrentChar().group);
		}
	}
}
