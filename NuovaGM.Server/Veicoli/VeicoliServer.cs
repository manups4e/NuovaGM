﻿using System;
using CitizenFX.Core;
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
	}
}
