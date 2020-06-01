using CitizenFX.Core;
using NuovaGM.Client.Interactions;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace NuovaGM.Client.IPLs
{
	public static class IplManager
	{
		public static GlobalIPLEnablers Global = new GlobalIPLEnablers();

		public static void EnableIpl(List<string> ipls, bool activate)
		{
			foreach (string s in ipls)
				_enableIPL(s, activate);
		}

		public static void EnableIpl(string ipls, bool activate)
		{
			_enableIPL(ipls, activate);
		}

		public static void SetIplPropState(List<int> interiorId, List<string> props, bool state, bool refresh = true)
		{
			foreach (int value in interiorId)
				foreach (string s in props)
					_setIplPropState(value, s, state, refresh);
		}

		public static void SetIplPropState(int interiorId, List<string> props, bool state, bool refresh = true)
		{
			foreach (string s in props)
				_setIplPropState(interiorId, s, state, refresh);
		}

		public static void SetIplPropState(int interiorId, string props, bool state, bool refresh = true)
		{
			_setIplPropState(interiorId, props, state, refresh);
		}

		private static void _setIplPropState(int interior, string prop, bool state, bool refresh)
		{
			if (state)
				if (!IsInteriorEntitySetActive(interior, prop)) ActivateInteriorEntitySet(interior, prop);
				else if (IsInteriorEntitySetActive(interior, prop)) DeactivateInteriorEntitySet(interior, prop);
			if (refresh) RefreshInterior(interior);
		}

		private static void _enableIPL(string s, bool activate)
		{
			if (activate)
			{
				if (!IsIplActive(s))
					RequestIpl(s);
			}
			else
				if (IsIplActive(s)) RemoveIpl(s);
		}

		public static async Task<bool> LoadStreamedTextureDict(string texture)
		{
			int time = 0;
			RequestStreamedTextureDict(texture, false);
			while (!HasStreamedTextureDictLoaded(texture))
			{
				await BaseScript.Delay(1000);
				time++;
				if (time > 5)
					return false;
			}
			return true;
		}

		public static async void DrawEmptyRect(string name, uint model)
		{
			int currentTime = 0;
			int renderId = RenderTargets.CreateNamedRenderTargetForModel(name, model);
			while (!IsNamedRendertargetRegistered(name))
			{
				await BaseScript.Delay(250);
				currentTime += 250;
				if (currentTime >= 5000) return;
			}
			if (IsNamedRendertargetRegistered(name))
			{
				SetTextRenderId(renderId);
				SetUiLayer(4);
				DrawRect(0.5f, 0.5f, 1.0f, 1.0f, 0, 0, 0, 0);
				SetTextRenderId(GetDefaultScriptRendertargetRenderId());
				ReleaseNamedRendertarget(name);
			}
		}
	}

	public class GlobalIPLEnablers
	{
		public int CurrentInteriorId = 0;
		public GTAOnline Online = new GTAOnline();
		public HighLife HighLife = new HighLife();
		public BikersClubHouse Biker = new BikersClubHouse();
		public FinanceOffices FinanceOffices = new FinanceOffices();

		public void ResetInteriorVariables()
		{
			Online.isInsideApartmentHi1 = false;
			Online.isInsideApartmentHi2 = false;
			Online.isInsideHouseHi1 = false;
			Online.isInsideHouseHi2 = false;
			Online.isInsideHouseHi3 = false;
			Online.isInsideHouseHi4 = false;
			Online.isInsideHouseHi5 = false;
			Online.isInsideHouseHi6 = false;
			Online.isInsideHouseHi7 = false;
			Online.isInsideHouseHi8 = false;
			Online.isInsideHouseLow1 = false;
			Online.isInsideHouseMid1 = false;
			Biker.isInsideClubhouse1 = false;
			Biker.isInsideClubhouse2 = false;
			FinanceOffices.isInsideOffice1 = false;
			FinanceOffices.isInsideOffice2 = false;
			FinanceOffices.isInsideOffice3 = false;
			FinanceOffices.isInsideOffice4 = false;
			HighLife.isInsideApartment1 = false;
			HighLife.isInsideApartment2 = false;
			HighLife.isInsideApartment3 = false;
			HighLife.isInsideApartment4 = false;
			HighLife.isInsideApartment5 = false;
			HighLife.isInsideApartment6 = false;
		}
	}

	public class GTAOnline
	{
		public bool isInsideApartmentHi1 = false;
		public bool isInsideApartmentHi2 = false;
		public bool isInsideHouseHi1 = false;
		public bool isInsideHouseHi2 = false;
		public bool isInsideHouseHi3 = false;
		public bool isInsideHouseHi4 = false;
		public bool isInsideHouseHi5 = false;
		public bool isInsideHouseHi6 = false;
		public bool isInsideHouseHi7 = false;
		public bool isInsideHouseHi8 = false;
		public bool isInsideHouseLow1 = false;
		public bool isInsideHouseMid1 = false;
	}
	public class HighLife
	{
		public bool isInsideApartment1 = false;
		public bool isInsideApartment2 = false;
		public bool isInsideApartment3 = false;
		public bool isInsideApartment4 = false;
		public bool isInsideApartment5 = false;
		public bool isInsideApartment6 = false;
	}
	public class BikersClubHouse
	{
		public bool isInsideClubhouse1 = false;
		public bool isInsideClubhouse2 = false;
	}
	public class FinanceOffices
	{
		public bool isInsideOffice1 = false;
		public bool isInsideOffice2 = false;
		public bool isInsideOffice3 = false;
		public bool isInsideOffice4 = false;
	}
}