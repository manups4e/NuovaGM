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

namespace NuovaGM.Client
{
	static class TickController
	{
		private static bool InUnVeicolo = false;
		private static bool HideHud = false;
		private static bool InAppartamento = false;
		private static bool Polizia = false;
		private static bool Medici = false;
		public static void Init()
		{
			Client.Instance.AddEventHandler("lprp:onPlayerSpawn", new Action(Spawnato));
		}

		private static async void Spawnato()
		{
			Client.Instance.AddTick(KingOfAllTicks);
			Client.Instance.AddTick(BankingClient.Markers);
			Client.Instance.AddTick(PompeDiBenzinaClient.BusinessesPumps);
			Client.Instance.AddTick(Death.Injuried);
			Client.Instance.AddTick(StatsNeeds.Aggiornamento);
			Client.Instance.AddTick(StatsNeeds.Conseguenze);
			Client.Instance.AddTick(StatsNeeds.Agg);
			Client.Instance.AddTick(NegozioAbitiClient.OnTick);
			Client.Instance.AddTick(NegoziClient.OnTick);
			Client.Instance.AddTick(EventiPersonalMenu.MostramiStatus);
			Client.Instance.AddTick(EventiPersonalMenu.MostramiSoldi);
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
		}

		private static async Task KingOfAllTicks()
		{
			ControlloLavori();
			if (Game.PlayerPed.IsInVehicle())
			{
				if (!InUnVeicolo)
				{
					Client.Instance.AddTick(VehicleDamage.OnTick);
					if (VehicleDamage.torqueMultiplierEnabled || VehicleDamage.preventVehicleFlip || VehicleDamage.limpMode)
						Client.Instance.AddTick(VehicleDamage.IfNeeded);
					Client.Instance.AddTick(VeicoliClient.Lux);
					Client.Instance.AddTick(VeicoliClient.gestioneVeh);
					Client.Instance.AddTick(Prostitute.LoopProstitute);
					Client.Instance.AddTick(Prostitute.ControlloProstitute);

					Client.Instance.AddTick(EffettiRuote.ControlloRuote);
					Client.Instance.AddTick(EffettiRuote.WheelGlow);

					Client.Instance.RemoveTick(NegozioAbitiClient.OnTick);
					Client.Instance.RemoveTick(BarberClient.Sedie);
					Client.Instance.RemoveTick(NegoziClient.OnTick);
					Client.Instance.RemoveTick(VeicoliClient.MostraMenuAffitto);
					InUnVeicolo = true;
				}
			}
			else
			{
				if (InUnVeicolo)
				{
					Client.Instance.RemoveTick(VehicleDamage.OnTick);
					if (VehicleDamage.torqueMultiplierEnabled || VehicleDamage.preventVehicleFlip || VehicleDamage.limpMode)
						Client.Instance.RemoveTick(VehicleDamage.IfNeeded);
					Client.Instance.RemoveTick(VeicoliClient.Lux);
					Client.Instance.RemoveTick(VeicoliClient.gestioneVeh);
					Client.Instance.RemoveTick(Prostitute.LoopProstitute);
					Client.Instance.RemoveTick(Prostitute.ControlloProstitute);

					Client.Instance.RemoveTick(EffettiRuote.ControlloRuote);
					Client.Instance.RemoveTick(EffettiRuote.WheelGlow);

					Client.Instance.AddTick(NegozioAbitiClient.OnTick);
					Client.Instance.AddTick(BarberClient.Sedie);
					Client.Instance.AddTick(BankingClient.Markers);
					Client.Instance.AddTick(NegoziClient.OnTick);
					Client.Instance.AddTick(VeicoliClient.MostraMenuAffitto);
					InUnVeicolo = false;
				}
			}
			if (EventiPersonalMenu.DoHideHud)
			{
				if (!HideHud)
				{
					Client.Instance.RemoveTick(EventiPersonalMenu.MostramiStatus);
					Client.Instance.RemoveTick(EventiPersonalMenu.MostramiSoldi);
					HideHud = true;
				}
			}
			else
			{
				if (HideHud)
				{
					Client.Instance.AddTick(EventiPersonalMenu.MostramiStatus);
					Client.Instance.AddTick(EventiPersonalMenu.MostramiSoldi);
					HideHud = false;
				}
			}
			if (CheckAppartamento(GetInteriorFromGameplayCam()))
			{
				if (!InAppartamento)
				{
					Client.Instance.AddTick(DivaniEPosizioniSedute.DivaniCasa);
					Client.Instance.AddTick(Docce.ControlloDocceVicino);
					Client.Instance.AddTick(Docce.Docceeee);
					Client.Instance.AddTick(Letti.Letto);
					InAppartamento = true;
				}
			}
			else
			{
				if (InAppartamento)
				{
					Client.Instance.RemoveTick(DivaniEPosizioniSedute.DivaniCasa);
					Client.Instance.RemoveTick(Docce.ControlloDocceVicino);
					Client.Instance.RemoveTick(Docce.Docceeee);
					Client.Instance.RemoveTick(Letti.Letto);
					InAppartamento = false;
				}
			}
		}

		private static async void ControlloLavori()
		{
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
