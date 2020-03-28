using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
 
namespace NuovaGM.Server.Veicoli
{
	static class GiostreServer
	{
		public static async void Init()
		{
			Server.Instance.RegisterEventHandler("lprp:ruotapanoramica:syncState", new Action<Player, string, int>(SyncRuotaPan));
			Server.Instance.RegisterEventHandler("lprp:ruotapanoramica:RuotaFerma", new Action<bool>(FermaRuota));
			Server.Instance.RegisterEventHandler("lprp:ruotapanoramica:aggiornaCabine", new Action<int, int>(AggiornaCabine));
			Server.Instance.RegisterEventHandler("lprp:ruotapanoramica:playerScende", new Action<Player, int, int>(RuotaScende));
			Server.Instance.RegisterEventHandler("lprp:ruotapanoramica:playerSale", new Action<Player, int, int>(RuotaSale));
			Server.Instance.RegisterEventHandler("lprp:ruotapanoramica:aggiornaGradient", new Action<Player, int>(AggiornaGradient));
			Server.Instance.RegisterEventHandler("lprp:montagnerusse:playerScende", new Action<Player, int>(MontagneScende));
			Server.Instance.RegisterEventHandler("lprp:montagnerusse:playerSale", new Action<Player, int, int, int>(MontagneSale));
			Server.Instance.RegisterEventHandler("lprp:montagnerusse:syncState", new Action<Player, string>(SyncMontagne));
			Server.Instance.RegisterEventHandler("lprp:montagnerusse:syncCarrelli", new Action<int, int>(SyncCarrelli));
			Server.Instance.RegisterEventHandler("omni:cablecar:host:sync", new Action<Player, int, string>(SyncFunivia));
		}

		private static void AggiornaGradient([FromSource] Player player, int gradient)
		{
			BaseScript.TriggerClientEvent("lprp:ruotapanoramica:aggiornaGradient", gradient);
		}

		public static void SyncFunivia([FromSource] Player p, int index, string state)
		{
			BaseScript.TriggerClientEvent("omni:cablecar:forceState", index, state);
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

		public static void MontagneSale([FromSource] Player p, int player, int index, int carrello) => BaseScript.TriggerClientEvent("lprp:montagnerusse:playerSale", player, index, carrello);

		public static void MontagneScende([FromSource] Player p, int player) => BaseScript.TriggerClientEvent("lprp:montagnerusse:playerScende", player);

		public static void SyncCarrelli(int Carrello, int Occupato) => BaseScript.TriggerClientEvent("lprp:montagnerusse:syncCarrelli", Carrello, Occupato);
	}
}
