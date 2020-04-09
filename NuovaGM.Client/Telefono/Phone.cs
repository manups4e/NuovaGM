using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using Newtonsoft.Json;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.Telefono.Apps;
using NuovaGM.Client.Telefono.Models;
using NuovaGM.Shared;
using static CitizenFX.Core.Native.API;

namespace NuovaGM.Client.Telefono
{
	public class Phone
	{
		public Scaleform Scaleform = new Scaleform("CELLPHONE_IFRUIT");
		public bool Visible = false;
		public bool SleepMode = false;
		public List<Phone_data> phone_data = new List<Phone_data>();
		public List<App> apps;
		public App mainApp;
		public App currentApp = null;
		public int VisibleAnimProgress;
		public bool IsBackOverriddenByApp;
		public bool InCall = false;
		public float Scale = 0;

		public Phone()
		{
			phone_data.Add(new Phone_data());
			apps = new List<App>()
			{
				new Contacts(this), new Messages(this), new QuickSave(this), new Settings(this)
			};
			mainApp = new MainMenu(this, apps);
		}

		public Phone(dynamic result)
		{
			apps = new List<App>()
			{
				new Contacts(this), new Messages(this), new QuickSave(this), new Settings(this)
			};
			for (int i = 0; i < JsonConvert.DeserializeObject(result.phone_data).Count; i++)
				phone_data.Add(new Phone_data(JsonConvert.DeserializeObject(result.phone_data)[i]));
			mainApp = new MainMenu(this, apps);
		}

		public async void OpenPhone()
		{
			Game.PlaySound("Pull_Out", "Phone_SoundSet_Default");
			CreateMobilePhone((int)ModelPhone.Micheal);
			PhoneMainClient.StartApp("Main");
			Game.PlayerPed.SetConfigFlag(242, false);
			Game.PlayerPed.SetConfigFlag(243, false);
			Game.PlayerPed.SetConfigFlag(244, true);
			VisibleAnimProgress = 21;
			N_0x83a169eabcdb10a2(PlayerPedId(), getCurrentCharPhone().Theme);
			await BaseScript.Delay(20);
			if (GetFollowPedCamViewMode() == 4)
				Scale = 0f;
			else
				Scale = 300f;
			SetMobilePhoneScale(Scale);
			Visible = true;
			while (!Scaleform.IsLoaded)
			{
				Scaleform = new Scaleform("CELLPHONE_IFRUIT");
				await BaseScript.Delay(10);
			}
		}

		public void ClosePhone()
		{
			Scaleform.CallFunction("SHUTDOWN_MOVIE");
			DestroyMobilePhone();
			Visible = false;
			Game.PlayerPed.SetConfigFlag(242, true);
			Game.PlayerPed.SetConfigFlag(243, true);
			Game.PlayerPed.SetConfigFlag(244, false);
			Scaleform.Dispose();
		}

		public Phone_data getCurrentCharPhone()
		{
			for (int i = 0; i < phone_data.Count; i++)
			{
				if (Game.Player.GetPlayerData().char_current - 1 == phone_data[i].id - 1)
					return phone_data[i];
			}
			return null;
		}

		public void SetSoftKeys(int index, int icon)
		{
			Scaleform.CallFunction("SET_SOFT_KEYS", index, true, icon);
		}
	}
}
