﻿using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using TheLastPlanet.Client.Banking;
using TheLastPlanet.Client.Businesses;
using TheLastPlanet.Client.Giostre;
using TheLastPlanet.Client.Core;
using TheLastPlanet.Client.Core.CharCreation;
using TheLastPlanet.Client.Core.Status;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Interactions;
using TheLastPlanet.Client.Lavori;
using TheLastPlanet.Client.Lavori.Whitelistati.Medici;
using TheLastPlanet.Client.ListaPlayers;
using TheLastPlanet.Client.Manager;
using TheLastPlanet.Client.Negozi;
using TheLastPlanet.Client.Personale;
using TheLastPlanet.Client.Sport;
using TheLastPlanet.Client.Veicoli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheLastPlanet.Client.Lavori.Whitelistati.Polizia;
using TheLastPlanet.Client.Lavori.Generici.Rimozione;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.Lavori.Generici.Cacciatore;
using TheLastPlanet.Client.Lavori.Generici.Pescatore;
using TheLastPlanet.Client.Proprietà.Hotel;
using Logger;
using Newtonsoft.Json;
using TheLastPlanet.Client.Lavori.Whitelistati.VenditoreAuto;
using TheLastPlanet.Client.Core;

namespace TheLastPlanet.Client
{
	static class TickController
	{
		public static List<Func<Task>> TickAPiedi = new List<Func<Task>>();
		public static List<Func<Task>> TickVeicolo = new List<Func<Task>>();
		public static List<Func<Task>> TickAppartamento = new List<Func<Task>>();
		public static List<Func<Task>> TickHUD = new List<Func<Task>>();
		public static List<Func<Task>> TickGenerici = new List<Func<Task>>();
		public static List<Func<Task>> TickPolizia = new List<Func<Task>>();
		public static List<Func<Task>> TickMedici = new List<Func<Task>>();


		private static bool InUnVeicolo = false;
		private static bool HideHud = false;
		private static bool InAppartamento = false;
		private static bool Polizia = false;
		private static bool Medici = false;
		public static void Init()
		{
			Client.Instance.AddTick(HUD.Menus);
			Client.Instance.AddEventHandler("lprp:onPlayerSpawn", new Action(Spawnato));
			// TICK HUD \\

			TickHUD.Add(EventiPersonalMenu.MostramiStatus);

			// TICK GENERICI \\ ATTIVI SEMPRE
			TickGenerici.Add(StatsNeeds.Aggiornamento);
			TickGenerici.Add(StatsNeeds.Conseguenze);
			TickGenerici.Add(StatsNeeds.Agg);
			TickGenerici.Add(Main.NewTick);
			TickGenerici.Add(Main.MainTick);
			TickGenerici.Add(Main.Armi);
			TickGenerici.Add(PersonalMenu.attiva);
			TickGenerici.Add(Main.Recoil);
			TickGenerici.Add(FuelClient.FuelCount);
			TickGenerici.Add(FuelClient.FuelTruck);
			TickGenerici.Add(PompeDiBenzinaClient.BusinessesPumps);
			TickGenerici.Add(aggiornaPl);

			// TICK A PIEDI \\
			TickAPiedi.Add(BankingClient.ControlloATM);
			//TickAPiedi.Add(BankingClient.Markers);
			TickAPiedi.Add(Death.Injuried);
			TickAPiedi.Add(NegozioAbitiClient.OnTick);
			TickAPiedi.Add(NegoziClient.OnTick);
			TickAPiedi.Add(BarberClient.Sedie);
			TickAPiedi.Add(VeicoliClient.MostraMenuAffitto);
			TickAPiedi.Add(MediciMainClient.MarkersNonMedici);
			TickAPiedi.Add(RimozioneClient.InizioLavoro);
			TickAPiedi.Add(Macchinette.VendingMachines);
			TickAPiedi.Add(Macchinette.ControlloMachines);
			TickAPiedi.Add(PickupsClient.PickupsMain);
			TickAPiedi.Add(Spazzatura.CestiSpazzatura);
			TickAPiedi.Add(Spazzatura.ControlloSpazzatura);
			TickAPiedi.Add(CacciatoreClient.ControlloCaccia);
			TickAPiedi.Add(PescatoreClient.ControlloPesca);
			TickAPiedi.Add(Hotels.ControlloHotel);
			TickAPiedi.Add(Proprietà.Manager.MarkerFuori);
			TickAPiedi.Add(DivaniEPosizioniSedute.CheckSedia);
			TickAPiedi.Add(DivaniEPosizioniSedute.SedieSiedi);
			TickAPiedi.Add(CarDealer.Markers);



			// TICK NEL VEICOLO \\
			TickVeicolo.Add(VehicleDamage.OnTick);
			if (VehicleDamage.torqueMultiplierEnabled || VehicleDamage.preventVehicleFlip || VehicleDamage.limpMode)
				TickVeicolo.Add(VehicleDamage.IfNeeded);
			TickVeicolo.Add(VeicoliClient.Lux);
			TickVeicolo.Add(VeicoliClient.gestioneVeh);
			TickVeicolo.Add(Prostitute.LoopProstitute);
			TickVeicolo.Add(Prostitute.ControlloProstitute);
			TickVeicolo.Add(EffettiRuote.ControlloRuote);
			TickVeicolo.Add(EffettiRuote.WheelGlow);

			// TICK APPARTAMENTO \\
			TickAppartamento.Add(DivaniEPosizioniSedute.DivaniCasa);
			TickAppartamento.Add(Docce.ControlloDocceVicino);
			TickAppartamento.Add(Docce.Docceeee);
			TickAppartamento.Add(Letti.ControlloLetti);
			TickAppartamento.Add(Proprietà.Manager.MarkerDentro);

			// TICK POLIZIA \\
			TickPolizia.Add(PoliziaMainClient.MarkersPolizia);
			TickPolizia.Add(PoliziaMainClient.MainTickPolizia);
			if (Client.Impostazioni.Lavori.Polizia.Config.AbilitaBlipVolanti)
				TickPolizia.Add(PoliziaMainClient.AbilitaBlipVolanti);

			// TICK MEDICI \\
			TickMedici.Add(MediciMainClient.MarkersMedici);
			TickMedici.Add(MediciMainClient.BlipMorti);
		}

		private static async void Spawnato()
		{
			TickGenerici.ForEach(x => Client.Instance.AddTick(x));
			TickAPiedi.ForEach(x => Client.Instance.AddTick(x));
			TickHUD.ForEach(x => Client.Instance.AddTick(x));
			Client.Instance.AddTick(TickHandler);
		}

		private static async Task TickHandler()
		{
			if (Game.PlayerPed.IsInVehicle())
			{
				if (!InUnVeicolo)
				{
					TickAPiedi.ForEach(x => Client.Instance.RemoveTick(x));
					TickVeicolo.ForEach(x => Client.Instance.AddTick(x));
					InUnVeicolo = true;
				}
			}
			else
			{
				if (InUnVeicolo)
				{
					TickVeicolo.ForEach(x => Client.Instance.RemoveTick(x));
					TickAPiedi.ForEach(x => Client.Instance.AddTick(x));
					VehHUD.NUIBuckled(false);
					InUnVeicolo = false;
				}
			}
			if (EventiPersonalMenu.DoHideHud)
			{
				if (!HideHud)
				{
					TickHUD.ForEach(x => Client.Instance.RemoveTick(x));
					HideHud = true;
				}
			}
			else
			{
				if (HideHud)
				{
					TickHUD.ForEach(x => Client.Instance.AddTick(x));
					HideHud = false;
				}
			}
			if (Game.Player.GetPlayerData().Istanza.Stanziato)
			{
				if (!InAppartamento)
				{
					TickAPiedi.ForEach(x => Client.Instance.RemoveTick(x));
					// verrà aggiunta gestione garage
					TickAppartamento.ForEach(x => Client.Instance.AddTick(x));
					InAppartamento = true;
				}
			}
			else
			{
				if (InAppartamento)
				{
					TickAppartamento.ForEach(x => Client.Instance.RemoveTick(x));
					// verrà aggiunta gestione garage
					TickAPiedi.ForEach(x => Client.Instance.AddTick(x));
					InAppartamento = false;
				}
			}

			if (Game.Player.GetPlayerData().CurrentChar.job.name.ToLower() == "polizia")
			{
				if (Medici)
				{
					Client.Instance.RemoveTick(MediciMainClient.MarkersMedici);
					foreach (var morto in MediciMainClient.Morti)
						morto.Value.Delete();
					if (MediciMainClient.Morti.Count > 0)
					{
						MediciMainClient.Morti.Clear();
						Client.Instance.RemoveTick(MediciMainClient.BlipMorti);
					}
					Medici = false;
				}
				if (!Polizia)
				{
					Client.Instance.AddTick(PoliziaMainClient.MarkersPolizia);
					Client.Instance.AddTick(PoliziaMainClient.MainTickPolizia);
					if (Client.Impostazioni.Lavori.Polizia.Config.AbilitaBlipVolanti)
						Client.Instance.AddTick(PoliziaMainClient.AbilitaBlipVolanti);
					Polizia = true;
				}
			}
			else if (Game.Player.GetPlayerData().CurrentChar.job.name.ToLower() == "medico")
			{
				if (Polizia)
				{
					Client.Instance.RemoveTick(PoliziaMainClient.MarkersPolizia);
					Client.Instance.RemoveTick(PoliziaMainClient.MainTickPolizia);
					if (Client.Impostazioni.Lavori.Polizia.Config.AbilitaBlipVolanti)
						Client.Instance.RemoveTick(PoliziaMainClient.AbilitaBlipVolanti);
					Polizia = false;
				}
				if (!Medici)
				{
					Client.Instance.AddTick(MediciMainClient.MarkersMedici);
					Client.Instance.AddTick(MediciMainClient.BlipMorti);
					Medici = true;
				}
			}
			else if (Game.Player.GetPlayerData().CurrentChar.job.name.ToLower() != "medico" && Game.Player.GetPlayerData().CurrentChar.job.name.ToLower() != "polizia")
			{
				if (Polizia)
				{
					Client.Instance.RemoveTick(PoliziaMainClient.MarkersPolizia);
					Client.Instance.RemoveTick(PoliziaMainClient.MainTickPolizia);
					if (Client.Impostazioni.Lavori.Polizia.Config.AbilitaBlipVolanti)
						Client.Instance.RemoveTick(PoliziaMainClient.AbilitaBlipVolanti);
					Polizia = false;
				}
				if (Medici)
				{
					Client.Instance.RemoveTick(MediciMainClient.MarkersMedici);
					foreach (var morto in MediciMainClient.Morti)
						morto.Value.Delete();
					if (MediciMainClient.Morti.Count > 0)
					{
						MediciMainClient.Morti.Clear();
						Client.Instance.RemoveTick(MediciMainClient.BlipMorti);
					}
					Medici = false;
				}
			}
		}
		private static bool CheckAppartamento(int iParam1)
		{
			switch (iParam1)
			{
				case 227329:
				case 227585:
				case 206337:
				case 208385:
				case 207361:
				case 207873:
				case 208129:
				case 207617:
				case 206081:
				case 146689:
				case 147201:
				case 146177:
				case 227841:
				case 206593:
				case 207105:
				case 146945:
				case 145921:
				case 143873:
				case 243201:
				case 148225:
				case 144641:
				case 144129:
				case 144385:
				case 141825:
				case 141569:
				case 145409:
				case 145665:
				case 143617:
				case 143105:
				case 142593:
				case 141313:
				case 147969:
				case 142849:
				case 143361:
				case 144897:
				case 145153:
				case 149761:
					return true;
			}
			return false;
		}

		private static async Task aggiornaPl()
		{
			await BaseScript.Delay(5000);
			await Eventi.AggiornaPlayers();
		}
	}
}
