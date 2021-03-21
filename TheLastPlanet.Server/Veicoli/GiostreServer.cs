﻿using System;
using System.Linq;
using CitizenFX.Core;

namespace TheLastPlanet.Server.Veicoli
{
	static class GiostreServer
	{
		public static void Init()
		{
			ServerSession.Instance.AddEventHandler("lprp:ruotapanoramica:syncState", new Action<Player, string, int>(SyncRuotaPan));
			ServerSession.Instance.AddEventHandler("lprp:ruotapanoramica:RuotaFerma", new Action<bool>(FermaRuota));
			ServerSession.Instance.AddEventHandler("lprp:ruotapanoramica:aggiornaCabine", new Action<int, int>(AggiornaCabine));
			ServerSession.Instance.AddEventHandler("lprp:ruotapanoramica:playerScende", new Action<Player, int, int>(RuotaScende));
			ServerSession.Instance.AddEventHandler("lprp:ruotapanoramica:playerSale", new Action<Player, int, int>(RuotaSale));
			ServerSession.Instance.AddEventHandler("lprp:ruotapanoramica:aggiornaGradient", new Action<Player, int>(AggiornaGradient));
			ServerSession.Instance.AddEventHandler("lprp:montagnerusse:playerScende", new Action<Player, int>(MontagneScende));
			ServerSession.Instance.AddEventHandler("lprp:montagnerusse:playerSale", new Action<Player, int, int, int>(MontagneSale));
			ServerSession.Instance.AddEventHandler("lprp:montagnerusse:syncState", new Action<Player, string>(SyncMontagne));
			ServerSession.Instance.AddEventHandler("lprp:montagnerusse:syncCarrelli", new Action<int, int>(SyncCarrelli));
			ServerSession.Instance.AddEventHandler("omni:cablecar:host:sync", new Action<Player, int, string>(SyncFunivia));
		}

		private static void AggiornaGradient([FromSource] Player player, int gradient)
		{
			if (ServerSession.Instance.GetPlayers.ToList().OrderBy(x => x.Handle).FirstOrDefault() == player) BaseScript.TriggerClientEvent("lprp:ruotapanoramica:aggiornaGradient", gradient);
		}

		public static void SyncFunivia([FromSource] Player p, int index, string state)
		{
			BaseScript.TriggerClientEvent("omni:cablecar:forceState", index, state);
		}

		public static void SyncRuotaPan([FromSource] Player p, string state, int Player)
		{
			if (ServerSession.Instance.GetPlayers.ToList().OrderBy(x => x.Handle).FirstOrDefault() == p) BaseScript.TriggerClientEvent("lprp:ruotapanoramica:forceState", state);
		}

		public static void SyncMontagne([FromSource] Player p, string state)
		{
			if (ServerSession.Instance.GetPlayers.ToList().OrderBy(x => x.Handle).FirstOrDefault() == p) BaseScript.TriggerClientEvent("lprp:montagnerusse:forceState", state);
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