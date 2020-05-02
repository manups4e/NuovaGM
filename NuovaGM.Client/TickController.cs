using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using NuovaGM.Client.Banking;
using NuovaGM.Client.Businesses;
using NuovaGM.Client.Giostre;
using NuovaGM.Client.gmPrincipale;
using NuovaGM.Client.gmPrincipale.MenuGm;
using NuovaGM.Client.gmPrincipale.Status;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.Interactions;
using NuovaGM.Client.Lavori;
using NuovaGM.Client.Lavori.Whitelistati.Medici;
using NuovaGM.Client.ListaPlayers;
using NuovaGM.Client.Manager;
using NuovaGM.Client.Negozi;
using NuovaGM.Client.Personale;
using NuovaGM.Client.Sport;
using NuovaGM.Client.Veicoli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuovaGM.Client.Lavori.Whitelistati.Polizia;
using NuovaGM.Client.Lavori.Generici.Rimozione;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Client.Lavori.Generici.Cacciatore;
using NuovaGM.Client.Lavori.Generici.Pescatore;
using NuovaGM.Client.Proprietà.Hotel;

namespace NuovaGM.Client
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
			TickHUD.Add(EventiPersonalMenu.MostramiSoldi);

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

			// TICK A PIEDI \\
			TickAPiedi.Add(BankingClient.Markers);
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
			TickAppartamento.Add(Letti.Letto);

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
			Client.Instance.AddTick(KingOfAllTicks);
			/*
			Client.Instance.AddTick(BankingClient.Markers);
			Client.Instance.AddTick(PompeDiBenzinaClient.BusinessesPumps);
			Client.Instance.AddTick(Death.Injuried);
			Client.Instance.AddTick(NegozioAbitiClient.OnTick);
			Client.Instance.AddTick(NegoziClient.OnTick);
			Client.Instance.AddTick(BarberClient.Sedie);
			Client.Instance.AddTick(VeicoliClient.MostraMenuAffitto);
			Client.Instance.AddTick(Main.NewTick);
			Client.Instance.AddTick(Main.MainTick);
			Client.Instance.AddTick(Main.Armi);
			Client.Instance.AddTick(Main.Recoil);
			Client.Instance.AddTick(PersonalMenu.attiva);
			Client.Instance.AddTick(FuelClient.FuelCount);
			Client.Instance.AddTick(FuelClient.FuelTruck);
			Client.Instance.AddTick(MediciMainClient.MarkersNonMedici);
			Client.Instance.AddTick(StatsNeeds.Aggiornamento);
			Client.Instance.AddTick(StatsNeeds.Conseguenze);
			Client.Instance.AddTick(StatsNeeds.Agg);
			Client.Instance.AddTick(EventiPersonalMenu.MostramiStatus);
			Client.Instance.AddTick(EventiPersonalMenu.MostramiSoldi);
			*/
		}

		private static async Task KingOfAllTicks()
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
			if (CheckAppartamento(GetInteriorFromGameplayCam()))
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
	}
}
