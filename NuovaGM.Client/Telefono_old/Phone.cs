﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using Newtonsoft.Json;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Telefono.Apps;
using TheLastPlanet.Client.Telefono.Models;
using TheLastPlanet.Shared;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client.Telefono
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
			Scaleform = new Scaleform("CELLPHONE_IFRUIT");
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
			Scaleform = new Scaleform("CELLPHONE_IFRUIT");
		}

		public async void OpenPhone()
		{
			Scaleform = new Scaleform("CELLPHONE_FACADE");
			Game.PlaySound("Pull_Out", "Phone_SoundSet_Default");
			CreateMobilePhone((int)ModelPhone.Micheal);
			PhoneMainClient.StartApp("Main");
			Game.PlayerPed.SetConfigFlag(242, false);
			Game.PlayerPed.SetConfigFlag(243, false);
			Game.PlayerPed.SetConfigFlag(244, true);
			VisibleAnimProgress = 21;
			N_0x83a169eabcdb10a2(PlayerPedId(), getCurrentCharPhone().Theme);
			if (GetFollowPedCamViewMode() == 4)
				Scale = 0f;
			else
				Scale = 300f;
			SetMobilePhoneScale(Scale);
			Visible = true;
			Client.Instance.AddTick(Screen);
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

		private async Task Screen()
		{
			Game.DisableControlThisFrame(0, Control.Sprint);

			if (VisibleAnimProgress > 0)
				VisibleAnimProgress -= 3;

			Scaleform.CallFunction("SET_TITLEBAR_TIME", World.CurrentDayTime.Hours, World.CurrentDayTime.Minutes);

			Scaleform.CallFunction("SET_SLEEP_MODE", SleepMode);
			Scaleform.CallFunction("SET_THEME", getCurrentCharPhone().Theme);
			Scaleform.CallFunction("SET_BACKGROUND_IMAGE", getCurrentCharPhone().Wallpaper);
			SetSoftKeys(2, 19);
			var playerPos = Game.Player.GetPlayerData().posizione.ToVector3();
			Scaleform.CallFunction("SET_SIGNAL_STRENGTH", GetZoneScumminess(GetZoneAtCoords(playerPos.X, playerPos.Y, playerPos.Z)));

			if (GetFollowPedCamViewMode() == 4)
				Scale = 0f;
			else
				Scale = 300f;
			if (currentApp != null)
			{
				if (currentApp.OverrideBack)
					IsBackOverriddenByApp = true;
				else
					IsBackOverriddenByApp = false;
				if (currentApp.Name != "Messaggi")
					SetMobilePhoneRotation(-90f, VisibleAnimProgress * 2f, 0f, 0);
				else
					SetMobilePhonePosition(60f, -21f - VisibleAnimProgress, -60f);
			}
			SetMobilePhoneScale(Scale);
			int renderId = 0;
			GetMobilePhoneRenderId(ref renderId);
			SetTextRenderId(renderId);
			DrawScaleformMovie(Scaleform.Handle, 0.0998f, 0.1775f, 0.1983f, 0.364f, 255, 255, 255, 255, 0);
			SetTextRenderId(1);
		}
	}
}