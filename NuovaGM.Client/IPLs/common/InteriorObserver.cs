using CitizenFX.Core;
using CitizenFX.Core.UI;
using Newtonsoft.Json;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.IPLs.dlc_bikers;
using TheLastPlanet.Client.IPLs.dlc_finance;
using TheLastPlanet.Client.MenuNativo;
using TheLastPlanet.Client.Veicoli;
using TheLastPlanet.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;
using TheLastPlanet.Client.Core;

namespace TheLastPlanet.Client.IPLs
{
	static class InteriorObserver
	{
		public static async Task Observer()
		{
			IplManager.Global.CurrentInteriorId = GetInteriorAtCoords(Cache.Cache.MyPlayer.Character.posizione.ToVector3().X, Cache.Cache.MyPlayer.Character.posizione.ToVector3().Y, Cache.Cache.MyPlayer.Character.posizione.ToVector3().Z);
			if (IplManager.Global.CurrentInteriorId == 0)
				IplManager.Global.ResetInteriorVariables();
			else
			{
				IplManager.Global.Online.isInsideApartmentHi1 = (IplManager.Global.CurrentInteriorId == gta_online.GTAOApartmentHi1.InteriorId);
				IplManager.Global.Online.isInsideApartmentHi2 = (IplManager.Global.CurrentInteriorId == gta_online.GTAOApartmentHi2.InteriorId);
				IplManager.Global.Online.isInsideHouseHi1 = (IplManager.Global.CurrentInteriorId == gta_online.GTAOHouseHi1.InteriorId);
				IplManager.Global.Online.isInsideHouseHi2 = (IplManager.Global.CurrentInteriorId == gta_online.GTAOHouseHi2.InteriorId);
				IplManager.Global.Online.isInsideHouseHi3 = (IplManager.Global.CurrentInteriorId == gta_online.GTAOHouseHi3.InteriorId);
				IplManager.Global.Online.isInsideHouseHi4 = (IplManager.Global.CurrentInteriorId == gta_online.GTAOHouseHi4.InteriorId);
				IplManager.Global.Online.isInsideHouseHi5 = (IplManager.Global.CurrentInteriorId == gta_online.GTAOHouseHi5.InteriorId);
				IplManager.Global.Online.isInsideHouseHi6 = (IplManager.Global.CurrentInteriorId == gta_online.GTAOHouseHi6.InteriorId);
				IplManager.Global.Online.isInsideHouseHi7 = (IplManager.Global.CurrentInteriorId == gta_online.GTAOHouseHi7.InteriorId);
				IplManager.Global.Online.isInsideHouseHi8 = (IplManager.Global.CurrentInteriorId == gta_online.GTAOHouseHi8.InteriorId);
				IplManager.Global.Online.isInsideHouseLow1 = (IplManager.Global.CurrentInteriorId == gta_online.GTAOHouseLow1.InteriorId);
				IplManager.Global.Online.isInsideHouseMid1 = (IplManager.Global.CurrentInteriorId == gta_online.GTAOHouseMid1.InteriorId);

				// DLC: High life
				IplManager.Global.HighLife.isInsideApartment1 = (IplManager.Global.CurrentInteriorId == gta_online.HLApartment1.InteriorId);
				IplManager.Global.HighLife.isInsideApartment2 = (IplManager.Global.CurrentInteriorId == gta_online.HLApartment2.InteriorId);
				IplManager.Global.HighLife.isInsideApartment3 = (IplManager.Global.CurrentInteriorId == gta_online.HLApartment3.InteriorId);
				IplManager.Global.HighLife.isInsideApartment4 = (IplManager.Global.CurrentInteriorId == gta_online.HLApartment4.InteriorId);
				IplManager.Global.HighLife.isInsideApartment5 = (IplManager.Global.CurrentInteriorId == gta_online.HLApartment5.InteriorId);
				IplManager.Global.HighLife.isInsideApartment6 = (IplManager.Global.CurrentInteriorId == gta_online.HLApartment6.InteriorId);

				// DLC: Bikers - Clubhouses
				IplManager.Global.Biker.isInsideClubhouse1 = (IplManager.Global.CurrentInteriorId == BikerClubhouse1.InteriorId);
				IplManager.Global.Biker.isInsideClubhouse2 = (IplManager.Global.CurrentInteriorId == BikerClubhouse2.InteriorId);

				// DLC: Finance & Felony - Offices
				IplManager.Global.FinanceOffices.isInsideOffice1 = (IplManager.Global.CurrentInteriorId == FinanceOffice1.CurrentInteriorId);
				IplManager.Global.FinanceOffices.isInsideOffice2 = (IplManager.Global.CurrentInteriorId == FinanceOffice2.CurrentInteriorId);
				IplManager.Global.FinanceOffices.isInsideOffice3 = (IplManager.Global.CurrentInteriorId == FinanceOffice3.CurrentInteriorId);
				IplManager.Global.FinanceOffices.isInsideOffice4 = (IplManager.Global.CurrentInteriorId == FinanceOffice4.CurrentInteriorId);
			}
			await BaseScript.Delay(250);
		}

		public static async Task OfficeSafeDoorHandler()
		{
			int doorhandle = 0;
			dynamic office = null;

			if (IplManager.Global.FinanceOffices.isInsideOffice1) office = FinanceOffice1.Instance;
			else if (IplManager.Global.FinanceOffices.isInsideOffice2) office = FinanceOffice2.Instance;
			else if (IplManager.Global.FinanceOffices.isInsideOffice3) office = FinanceOffice3.Instance;
			else if (IplManager.Global.FinanceOffices.isInsideOffice4) office = FinanceOffice4.Instance;

			if (office != null)
			{
				doorhandle = office.Safe.GetDoorHandle(office.CurrentSafeDoors.HashL);

				//sinsitra
				if (doorhandle != 0)
				{
					if (office.Safe.IsLeftDoorOpen) office.Safe.SetDoorState("left", true);
					else office.Safe.SetDoorState("left", false);
				}

				doorhandle = office.Safe.GetDoorHandle(office.CurrentSafeDoors.HashR);

				//destra
				if (doorhandle != 0)
				{
					if (office.Safe.IsRightDoorOpen) office.Safe.SetDoorState("right", true);
					else office.Safe.SetDoorState("right", false);
				}
			}
			await BaseScript.Delay(500);
		}
	}
}
