using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using TheLastPlanet.Client.Banking;
using TheLastPlanet.Client.Businesses;
using TheLastPlanet.Client.Core;
using TheLastPlanet.Client.Core.Status;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.Interactions;
using TheLastPlanet.Client.Lavori.Generici.Cacciatore;
using TheLastPlanet.Client.Lavori.Generici.Pescatore;
using TheLastPlanet.Client.Lavori.Generici.Rimozione;
using TheLastPlanet.Client.Lavori.Whitelistati.Medici;
using TheLastPlanet.Client.Lavori.Whitelistati.Polizia;
using TheLastPlanet.Client.Lavori.Whitelistati.VenditoreAuto;
using TheLastPlanet.Client.Negozi;
using TheLastPlanet.Client.Personale;
using TheLastPlanet.Client.SessionCache;
using TheLastPlanet.Client.Veicoli;

namespace TheLastPlanet.Client.Handlers
{
	internal static class TickController
	{
		public static List<Func<Task>> TickAPiedi = new();
		public static List<Func<Task>> TickVeicolo = new();
		public static List<Func<Task>> TickAppartamento = new();
		public static List<Func<Task>> TickHUD = new();
		public static List<Func<Task>> TickGenerici = new();
		public static List<Func<Task>> TickPolizia = new();
		public static List<Func<Task>> TickMedici = new();

		private static bool _inUnVeicolo;
		private static bool _hideHud;
		private static bool _inAppartamento;
		private static bool _polizia;
		private static bool _medici;
		private static int _timer;

		public static void Init()
		{
			ClientSession.Instance.AddTick(HUD.Menus);
			ClientSession.Instance.AddEventHandler("lprp:onPlayerSpawn", new Action(Spawnato));
			// TICK HUD \\
			TickHUD.Add(EventiPersonalMenu.MostramiStatus);

			// TICK GENERICI \\ ATTIVI SEMPRE
			TickGenerici.Add(StatsNeeds.GestioneStatsSkill);
			//TickGenerici.Add(StatsNeeds.Agg);
			TickGenerici.Add(Main.MainTick);
			TickGenerici.Add(PersonalMenu.attiva);
			TickGenerici.Add(FuelClient.FuelCount);
			TickGenerici.Add(FuelClient.FuelTruck);
			TickGenerici.Add(PompeDiBenzinaClient.BusinessesPumps);
			TickGenerici.Add(ProximityChat.Prossimità);

			// TICK A PIEDI \\
			TickAPiedi.Add(BankingClient.ControlloATM);
			//TickAPiedi.Add(BankingClient.Markers);
			TickAPiedi.Add(Death.Injuried);
			//TickAPiedi.Add(NegozioAbitiClient.OnTick);
			//TickAPiedi.Add(NegoziClient.OnTick);
			TickAPiedi.Add(BarberClient.Sedie);
			//TickAPiedi.Add(VeicoliClient.MostraMenuAffitto);
			TickAPiedi.Add(MediciMainClient.MarkersNonMedici);
			TickAPiedi.Add(RimozioneClient.InizioLavoro);
			TickAPiedi.Add(Macchinette.VendingMachines);
			TickAPiedi.Add(Macchinette.ControlloMachines);
			TickAPiedi.Add(PickupsClient.PickupsMain);
			TickAPiedi.Add(Spazzatura.CestiSpazzatura);
			TickAPiedi.Add(Spazzatura.ControlloSpazzatura);
			TickAPiedi.Add(CacciatoreClient.ControlloCaccia);
			TickAPiedi.Add(PescatoreClient.ControlloPesca);
			//TickAPiedi.Add(Hotels.ControlloHotel);
			TickAPiedi.Add(Proprietà.Manager.MarkerFuori);
			TickAPiedi.Add(DivaniEPosizioniSedute.CheckSedia);
			TickAPiedi.Add(DivaniEPosizioniSedute.SedieSiedi);
			TickAPiedi.Add(CarDealer.Markers);
			TickAPiedi.Add(LootingDead.Looting);

			// TICK NEL VEICOLO \\
			TickVeicolo.Add(VehicleDamage.OnTick);
			if (VehicleDamage.torqueMultiplierEnabled || VehicleDamage.preventVehicleFlip || VehicleDamage.limpMode) TickVeicolo.Add(VehicleDamage.IfNeeded);
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
			if (ClientSession.Impostazioni.Lavori.Polizia.Config.AbilitaBlipVolanti) TickPolizia.Add(PoliziaMainClient.AbilitaBlipVolanti);

			// TICK MEDICI \\
			TickMedici.Add(MediciMainClient.MarkersMedici);
			TickMedici.Add(MediciMainClient.BlipMorti);
		}

		private static void Spawnato()
		{
			TickGenerici.ForEach(x => ClientSession.Instance.AddTick(x));
			TickAPiedi.ForEach(x => ClientSession.Instance.AddTick(x));
			TickHUD.ForEach(x => ClientSession.Instance.AddTick(x));
			ClientSession.Instance.AddTick(TickHandler);
		}

		private static async Task TickHandler()
		{
			if (Cache.MyPlayer.User.StatiPlayer.InVeicolo)
			{
				if (!_inUnVeicolo)
				{
					TickAPiedi.ForEach(x => ClientSession.Instance.RemoveTick(x));
					TickVeicolo.ForEach(x => ClientSession.Instance.AddTick(x));
					_inUnVeicolo = true;
				}
			}
			else
			{
				if (_inUnVeicolo)
				{
					TickVeicolo.ForEach(x => ClientSession.Instance.RemoveTick(x));
					TickAPiedi.ForEach(x => ClientSession.Instance.AddTick(x));
					VehHud.NUIBuckled(false);
					_inUnVeicolo = false;
				}
			}

			if (Main.ImpostazioniClient.ModCinema)
			{
				if (!_hideHud)
				{
					TickHUD.ForEach(x => ClientSession.Instance.RemoveTick(x));
					ClientSession.Instance.AddTick(EventiPersonalMenu.CinematicMode);
					_hideHud = true;
				}
			}
			else
			{
				if (_hideHud)
				{
					TickHUD.ForEach(x => ClientSession.Instance.AddTick(x));
					ClientSession.Instance.RemoveTick(EventiPersonalMenu.CinematicMode);
					_hideHud = false;
				}
			}

			if (Cache.MyPlayer.User.StatiPlayer.Istanza.Stanziato)
			{
				if (!_inAppartamento)
				{
					TickAPiedi.ForEach(x => ClientSession.Instance.RemoveTick(x));
					// verrà aggiunta gestione garage
					TickAppartamento.ForEach(x => ClientSession.Instance.AddTick(x));
					_inAppartamento = true;
				}
			}
			else
			{
				if (_inAppartamento)
				{
					TickAppartamento.ForEach(x => ClientSession.Instance.RemoveTick(x));
					// verrà aggiunta gestione garage
					TickAPiedi.ForEach(x => ClientSession.Instance.AddTick(x));
					_inAppartamento = false;
				}
			}

			if (Cache.MyPlayer.User.CurrentChar.Job.name.ToLower() == "polizia")
			{
				if (_medici)
				{
					ClientSession.Instance.RemoveTick(MediciMainClient.MarkersMedici);
					foreach (KeyValuePair<Ped, Blip> morto in MediciMainClient.Morti) morto.Value.Delete();

					if (MediciMainClient.Morti.Count > 0)
					{
						MediciMainClient.Morti.Clear();
						ClientSession.Instance.RemoveTick(MediciMainClient.BlipMorti);
					}

					_medici = false;
				}

				if (!_polizia)
				{
					ClientSession.Instance.AddTick(PoliziaMainClient.MarkersPolizia);
					ClientSession.Instance.AddTick(PoliziaMainClient.MainTickPolizia);
					if (ClientSession.Impostazioni.Lavori.Polizia.Config.AbilitaBlipVolanti) ClientSession.Instance.AddTick(PoliziaMainClient.AbilitaBlipVolanti);
					_polizia = true;
				}
			}
			else if (Cache.MyPlayer.User.CurrentChar.Job.name.ToLower() == "medico")
			{
				if (_polizia)
				{
					ClientSession.Instance.RemoveTick(PoliziaMainClient.MarkersPolizia);
					ClientSession.Instance.RemoveTick(PoliziaMainClient.MainTickPolizia);
					if (ClientSession.Impostazioni.Lavori.Polizia.Config.AbilitaBlipVolanti) ClientSession.Instance.RemoveTick(PoliziaMainClient.AbilitaBlipVolanti);
					_polizia = false;
				}

				if (!_medici)
				{
					ClientSession.Instance.AddTick(MediciMainClient.MarkersMedici);
					ClientSession.Instance.AddTick(MediciMainClient.BlipMorti);
					_medici = true;
				}
			}
			else if (Cache.MyPlayer.User.CurrentChar.Job.name.ToLower() != "medico" && Cache.MyPlayer.User.CurrentChar.Job.name.ToLower() != "polizia")
			{
				if (_polizia)
				{
					ClientSession.Instance.RemoveTick(PoliziaMainClient.MarkersPolizia);
					ClientSession.Instance.RemoveTick(PoliziaMainClient.MainTickPolizia);
					if (ClientSession.Impostazioni.Lavori.Polizia.Config.AbilitaBlipVolanti) ClientSession.Instance.RemoveTick(PoliziaMainClient.AbilitaBlipVolanti);
					_polizia = false;
				}

				if (_medici)
				{
					ClientSession.Instance.RemoveTick(MediciMainClient.MarkersMedici);
					foreach (KeyValuePair<Ped, Blip> morto in MediciMainClient.Morti) morto.Value.Delete();

					if (MediciMainClient.Morti.Count > 0)
					{
						MediciMainClient.Morti.Clear();
						ClientSession.Instance.RemoveTick(MediciMainClient.BlipMorti);
					}

					_medici = false;
				}
			}

			if (Game.GameTime - _timer >= 5000)
			{
				_timer = Game.GameTime;
				await Eventi.AggiornaPlayers();
			}

			await BaseScript.Delay(250);
			// 4 volte al secondo bastano per gestire i tick.. non serve che siano tutti immediatamente attivati / disattivati
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