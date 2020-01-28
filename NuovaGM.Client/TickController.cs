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
using NuovaGM.Client.Lavori.Whitelistati.Polizia;
using NuovaGM.Client.ListaPlayers;
using NuovaGM.Client.Manager;
using NuovaGM.Client.Negozi;
using NuovaGM.Client.Personale;
using NuovaGM.Client.Sport;
using NuovaGM.Client.Veicoli;
using NuovaGM.Client.weather;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuovaGM.Client
{
	static class TickController
	{
		private static bool InUnVeicolo = false;
		private static bool HideHud = false;
		private static bool InAppartamento = false;
		public static void Init()
		{
			Client.GetInstance.RegisterEventHandler("lprp:onPlayerSpawn", new Action(Spawnato));
		}

		private static async void Spawnato()
		{
			Client.GetInstance.RegisterTickHandler(KinkOfTheTicks);
			Client.GetInstance.RegisterTickHandler(BankingClient.Markers);
			Client.GetInstance.RegisterTickHandler(PompeDiBenzinaClient.BusinessesPumps);
			Client.GetInstance.RegisterTickHandler(Death.Injuried);
			Client.GetInstance.RegisterTickHandler(StatsNeeds.Aggiornamento);
			Client.GetInstance.RegisterTickHandler(StatsNeeds.Conseguenze);
			Client.GetInstance.RegisterTickHandler(StatsNeeds.Agg);
			Client.GetInstance.RegisterTickHandler(NegozioAbitiClient.OnTick);
			Client.GetInstance.RegisterTickHandler(EventiPersonalMenu.MostramiStatus);
			Client.GetInstance.RegisterTickHandler(EventiPersonalMenu.MostramiSoldi);
			Client.GetInstance.RegisterTickHandler(BarberClient.Sedie);
			Client.GetInstance.RegisterTickHandler(VeicoliClient.MostraMenuAffitto);
			Client.GetInstance.RegisterTickHandler(Main.NewTick);
			Client.GetInstance.RegisterTickHandler(Main.MainTick);
			Client.GetInstance.RegisterTickHandler(Main.Armi);
			Client.GetInstance.RegisterTickHandler(Main.Recoil);
			Client.GetInstance.RegisterTickHandler(PersonalMenu.attiva);
			Client.GetInstance.RegisterTickHandler(PoliziaMainClient.MarkersPolizia);
			Client.GetInstance.RegisterTickHandler(PoliziaMainClient.MainTickPolizia);
			Client.GetInstance.RegisterTickHandler(FuelClient.FuelCount);
			Client.GetInstance.RegisterTickHandler(FuelClient.FuelTruck);
			if (JobManager.Polizia.Config.AbilitaBlipVolanti)
				Client.GetInstance.RegisterTickHandler(PoliziaMainClient.AbilitaBlipVolanti);
		}

		private static async Task KinkOfTheTicks()
		{
			if (Game.PlayerPed.IsInVehicle())
			{
				if (!InUnVeicolo)
				{
					Client.GetInstance.RegisterTickHandler(VehicleDamage.OnTick);
					if (VehicleDamage.torqueMultiplierEnabled || VehicleDamage.preventVehicleFlip || VehicleDamage.limpMode)
						Client.GetInstance.RegisterTickHandler(VehicleDamage.IfNeeded);
					Client.GetInstance.RegisterTickHandler(VeicoliClient.Lux);
					Client.GetInstance.RegisterTickHandler(VeicoliClient.gestioneVeh);
					Client.GetInstance.RegisterTickHandler(Prostitute.LoopProstitute);
					Client.GetInstance.RegisterTickHandler(Prostitute.ControlloProstitute);

					Client.GetInstance.RegisterTickHandler(EffettiRuote.ControlloRuote);
					Client.GetInstance.RegisterTickHandler(EffettiRuote.WheelGlow);

					Client.GetInstance.DeregisterTickHandler(NegozioAbitiClient.OnTick);
					Client.GetInstance.DeregisterTickHandler(BarberClient.Sedie);
					Client.GetInstance.DeregisterTickHandler(NegoziClient.OnTick);
					Client.GetInstance.DeregisterTickHandler(VeicoliClient.MostraMenuAffitto);
					InUnVeicolo = true;
				}
			}
			else
			{
				if (InUnVeicolo)
				{
					Client.GetInstance.DeregisterTickHandler(VehicleDamage.OnTick);
					if (VehicleDamage.torqueMultiplierEnabled || VehicleDamage.preventVehicleFlip || VehicleDamage.limpMode)
						Client.GetInstance.DeregisterTickHandler(VehicleDamage.IfNeeded);
					Client.GetInstance.DeregisterTickHandler(VeicoliClient.Lux);
					Client.GetInstance.DeregisterTickHandler(VeicoliClient.gestioneVeh);
					Client.GetInstance.DeregisterTickHandler(Prostitute.LoopProstitute);
					Client.GetInstance.DeregisterTickHandler(Prostitute.ControlloProstitute);

					Client.GetInstance.DeregisterTickHandler(EffettiRuote.ControlloRuote);
					Client.GetInstance.DeregisterTickHandler(EffettiRuote.WheelGlow);

					Client.GetInstance.RegisterTickHandler(NegozioAbitiClient.OnTick);
					Client.GetInstance.RegisterTickHandler(BarberClient.Sedie);
					Client.GetInstance.RegisterTickHandler(BankingClient.Markers);
					Client.GetInstance.RegisterTickHandler(NegoziClient.OnTick);
					Client.GetInstance.RegisterTickHandler(VeicoliClient.MostraMenuAffitto);
					InUnVeicolo = false;
				}
			}
			if (EventiPersonalMenu.DoHideHud)
			{
				if (!HideHud)
				{
					Client.GetInstance.DeregisterTickHandler(EventiPersonalMenu.MostramiStatus);
					Client.GetInstance.DeregisterTickHandler(EventiPersonalMenu.MostramiSoldi);
					HideHud = true;
				}
			}
			else
			{
				if (HideHud)
				{
					Client.GetInstance.RegisterTickHandler(EventiPersonalMenu.MostramiStatus);
					Client.GetInstance.RegisterTickHandler(EventiPersonalMenu.MostramiSoldi);
					HideHud = false;
				}
			}
			if (CheckAppartamento(GetInteriorFromGameplayCam()))
			{
				if (!InAppartamento)
				{
					Client.GetInstance.RegisterTickHandler(Docce.ControlloDocceVicino);
					Client.GetInstance.RegisterTickHandler(Docce.Docceeee);
					Client.GetInstance.RegisterTickHandler(Televisioni.Televisione);
					Client.GetInstance.RegisterTickHandler(Letti.Letto);
					InAppartamento = true;
				}
			}
			else
			{
				Client.GetInstance.DeregisterTickHandler(Docce.ControlloDocceVicino);
				Client.GetInstance.DeregisterTickHandler(Docce.Docceeee);
				Client.GetInstance.DeregisterTickHandler(Televisioni.Televisione);
				Client.GetInstance.DeregisterTickHandler(Letti.Letto);
				InAppartamento = false;
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
