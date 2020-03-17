using System;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;


namespace NuovaGM.Server.Veicoli
{
	static class VeicoliServer
	{

		static string lasthost = "";
		public static void Init()
		{
			Server.GetInstance.RegisterEventHandler("lprp:onPlayerSpawn", new Action<Player>(onPlayerSpawn));
			Server.GetInstance.RegisterEventHandler("lprp:lvc_TogIndicState_s", new Action<Player, int>(lvc_TogIndicState_s));
			Server.GetInstance.RegisterEventHandler("lprp:SilentSiren", new Action<Player, bool>(SilentSiren));
			Server.GetInstance.RegisterEventHandler("omni:cablecar:host:sync", new Action<Player, int, string>(SyncFunivia));
			Server.GetInstance.RegisterEventHandler("lprp:ruotapanoramica:syncState", new Action<Player, string, int>(SyncRuotaPan));
			Server.GetInstance.RegisterEventHandler("lprp:ruotapanoramica:RuotaFerma", new Action<bool>(FermaRuota));
			Server.GetInstance.RegisterEventHandler("lprp:ruotapanoramica:aggiornaCabine", new Action<int, int>(AggiornaCabine));
			Server.GetInstance.RegisterEventHandler("lprp:ruotapanoramica:playerScende", new Action<Player, int, int>(RuotaScende));
			Server.GetInstance.RegisterEventHandler("lprp:ruotapanoramica:playerSale", new Action<Player, int, int>(RuotaSale));
			Server.GetInstance.RegisterEventHandler("lprp:montagnerusse:playerScende", new Action<Player, int>(MontagneScende));
			Server.GetInstance.RegisterEventHandler("lprp:montagnerusse:playerSale", new Action<Player, int, int>(MontagneSale));
			Server.GetInstance.RegisterEventHandler("lprp:montagnerusse:syncState", new Action<Player, string>(SyncMontagne));
			Server.GetInstance.RegisterEventHandler("lprp:montagnerusse:syncCarrelli", new Action<int, int>(SyncCarrelli));


			Server.GetInstance.RegisterEventHandler("brakes:add_rear", new Action<int>(AddRear));
			Server.GetInstance.RegisterEventHandler("brakes:add_front", new Action<int>(AddFront));
			Server.GetInstance.RegisterEventHandler("brakes:rem_rear", new Action<int>(RemRear));
			Server.GetInstance.RegisterEventHandler("brakes:rem_front", new Action<int>(RemFront));
			
		}
		public static async void onPlayerSpawn([FromSource] Player p)
		{
			string host = GetHostId();
			if (lasthost != host)
			{
				lasthost = host;

				Debug.WriteLine("train timeout activated.");
				await BaseScript.Delay(15000);
				activateTrain();
			}
			BaseScript.TriggerClientEvent("lprp:getHost", host);
		}

		public static void SilentSiren([FromSource]Player player, bool toggle) => BaseScript.TriggerClientEvent("lprp:updateSirens", player.Handle, toggle);

		public static void lvc_TogIndicState_s([FromSource] Player player, int newstate) => BaseScript.TriggerClientEvent("lprp:lvc_TogIndicState_c", player.Handle, newstate);

		public static void activateTrain() => BaseScript.TriggerClientEvent("lprp:spawntrain");

		public static void SyncFunivia([FromSource] Player p, int index, string state)
		{
			string host = GetHostId();
			if (p.Handle == host)
			{
				BaseScript.TriggerClientEvent("omni:cablecar:forceState", index, state);
			}
		}

		public static void SyncRuotaPan([FromSource] Player p, string state, int Player)
		{
			BaseScript.TriggerClientEvent("lprp:ruotapanoramica:forceState", state);
		}

		public static void SyncMontagne([FromSource] Player p, string state)
		{
			BaseScript.TriggerClientEvent("lprp:montagnerusse:forceState", state);
		}

		public static void AggiornaCabine(int cabina, int player) => BaseScript.TriggerClientEvent("lprp:ruotapanoramica:aggiornaCabine", cabina, player);

		public static void FermaRuota(bool stato) => BaseScript.TriggerClientEvent("lprp:ruotapanoramica:FermaRuota", stato);

		public static void RuotaSale([FromSource] Player p, int player, int cabina) => BaseScript.TriggerClientEvent("lprp:ruotapanoramica:playerSale", player, cabina);

		public static void RuotaScende([FromSource] Player p, int player, int cabina) => BaseScript.TriggerClientEvent("lprp:ruotapanoramica:playerScende", player, cabina);

		public static void MontagneSale([FromSource] Player p, int player, int index) => p.TriggerEvent("lprp:montagnerusse:playerSale", index);

		public static void MontagneScende([FromSource] Player p, int player) => p.TriggerEvent("lprp:montagnerusse:playerScende");

		public static void SyncCarrelli(int Carrello, int Occupato) => BaseScript.TriggerClientEvent("lprp:montagnerusse:syncCarrelli", Carrello, Occupato);

		private static async void AddRear(int veh) 
		{ 
			BaseScript.TriggerClientEvent("cBrakes:add_rear", veh); 
		}
		private static async void AddFront(int veh) 
		{ 
			BaseScript.TriggerClientEvent("cBrakes:add_front", veh);
		}
		private static async void RemRear(int veh) 
		{ 
			BaseScript.TriggerClientEvent("cBrakes:rem_rear", veh);
		}
		private static async void RemFront(int veh) 
		{ 
			BaseScript.TriggerClientEvent("cBrakes:rem_front", veh);
		}
	}
}
