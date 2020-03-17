using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using static CitizenFX.Core.Native.API;
using Newtonsoft.Json;
using NuovaGM.Client.gmPrincipale.MenuGm;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Client.MenuNativo;
using CitizenFX.Core.Native;
using Newtonsoft.Json.Linq;

namespace NuovaGM.Client.Telefono
{
	public enum ModelPhone : int
	{
		Micheal = 0,
		Franklin = 1,
		Trevor = 2,
		Prologue = 4
	}

	//prop_phone_proto
	//npcphone

	static class PhoneMainClient
	{
		public static Phone Phone;
		public static void Init()
		{
			Client.GetInstance.RegisterEventHandler("lprp:setupPhoneClientUser", new Action<string>(Setup));
			Client.GetInstance.RegisterEventHandler("lprp:phone_start", new Action<string>(StartApp));
		}

		private static async void Setup(string JsonTelefono) 
		{
			if (JsonTelefono != "{\"phone_data\":[]}")
				Phone = new Phone(JsonConvert.DeserializeObject<Phone>(JsonTelefono));
			else
				Phone = new Phone();
			Client.GetInstance.RegisterTickHandler(ControlloApertura);
		}

		public static async Task ControlloApertura()
		{
			if (!HUD.MenuPool.IsAnyMenuOpen() && !Game.IsPaused && !Banking.BankingClient.InterfacciaAperta && !Game.Player.IsAiming && (!Game.PlayerPed.IsAiming || !Game.PlayerPed.IsAimingFromCover))
			{
				if (!Phone.Visible)
				{
					if (Game.IsControlJustPressed(0, Control.Phone))
						Phone.OpenPhone();
				}
				else
				{
					if (Phone.currentApp == null) { return; }
					if (IsPedRunningMobilePhoneTask(Game.PlayerPed.Handle))
					{
						Game.DisableControlThisFrame(0, Control.Sprint);

						SetMobilePhonePosition(60f, -21f - Phone.VisibleAnimProgress, -60f);
						if(Phone.currentApp.Name != "Messaggi")
							SetMobilePhoneRotation(-90f, Phone.VisibleAnimProgress * 2f, 0f, 0);

						if (Phone.VisibleAnimProgress > 0)
							Phone.VisibleAnimProgress -= 3;

						var time = World.CurrentDayTime;
						Phone.Scaleform.CallFunction("SET_TITLEBAR_TIME", time.Hours, time.Minutes);

						Phone.Scaleform.CallFunction("SET_SLEEP_MODE", Phone.SleepMode);
						Phone.Scaleform.CallFunction("SET_THEME", Phone.getCurrentCharPhone().Theme);
						Phone.Scaleform.CallFunction("SET_BACKGROUND_IMAGE", Phone.getCurrentCharPhone().Wallpaper);
						Phone.SetSoftKeys(2, 19);
						var playerPos = Game.PlayerPed.Position;
						Phone.Scaleform.CallFunction("SET_SIGNAL_STRENGTH", GetZoneScumminess(GetZoneAtCoords(playerPos.X, playerPos.Y, playerPos.Z)));


						if (GetFollowPedCamViewMode() == 4)
							Phone.Scale = 0f;
						else
							Phone.Scale = 300f;
						SetMobilePhoneScale(Phone.Scale);
						int renderId = 0;
						GetMobilePhoneRenderId(ref renderId);
						SetTextRenderId(renderId);
						DrawScaleformMovie(Phone.Scaleform.Handle, 0.0998f, 0.1775f, 0.1983f, 0.364f, 255, 255, 255, 255, 0);
						SetTextRenderId(1);
						if (Phone.currentApp.OverrideBack)
						{
							Phone.IsBackOverriddenByApp = true;
						}
						else
						{
							Phone.IsBackOverriddenByApp = false;
						}

						if (Game.IsControlJustPressed(1, Control.PhoneCancel))
						{
							if (Phone.IsBackOverriddenByApp)
							{
								Phone.IsBackOverriddenByApp = false;
							}
							else
							{
								await KillApp();
							}
						}
					}
				}
			}	
		}

		public static async void StartApp(string app)
		{
			if (app == "Main")
			{
				await KillApp();
				if (Phone.currentApp != null)
				{
					Phone.currentApp.Kill();
					Client.GetInstance.DeregisterTickHandler(Phone.currentApp.Tick);
				}
				Phone.currentApp = Phone.mainApp;
			}
			else if (Phone.apps.Exists(x => x.Name == app))
			{
				Client.GetInstance.DeregisterTickHandler(Phone.mainApp.Tick);
				Phone.currentApp = Phone.apps.FirstOrDefault(x => x.Name == app);
			}

			Phone.currentApp.Initialize(Phone);
			Client.GetInstance.RegisterTickHandler(Phone.currentApp.Tick);

			Debug.WriteLine($"CurrentApp = {Phone.currentApp.Name}");
		}

		public static async Task KillApp()
		{
			if (Phone.currentApp != null)
			{
				Debug.WriteLine($"Killing App {Phone.currentApp.Name}");
				Client.GetInstance.DeregisterTickHandler(Phone.currentApp.Tick);
				Phone.currentApp.Kill();

				var lastApp = Phone.currentApp;
				Phone.currentApp = null;

				if (lastApp.Name == "Main")
				{
					foreach (var app in Phone.apps)
					{
						app.Kill();
						Client.GetInstance.DeregisterTickHandler(app.Tick);
					}
					Phone.ClosePhone();
				}
				else
				{
					Game.PlaySound("Menu_Navigate", "Phone_SoundSet_Default");
					StartApp("Main");
				}
			}

			await Task.FromResult(0);
		}

	}

	public enum SoftKeyIcon
	{
		Blank = 1,
		Select = 2,
		Pages = 3,
		Back = 4,
		Call = 5,
		Hangup = 6,
		HangupHuman = 7,
		Week = 8,
		Keypad = 9,
		Open = 10,
		Reply = 11,
		Delete = 12,
		Yes = 13,
		No = 14,
		Sort = 15,
		Website = 16,
		Police = 17,
		Ambulance = 18,
		Fire = 19,
		Pages2 = 20
	}

	public sealed class Wallpapers
	{
		public static readonly string iFruitDefault = "Phone_Wallpaper_ifruitdefault";
		public static readonly string BadgerDefault = "Phone_Wallpaper_badgerdefault";
		public static readonly string Bittersweet = "Phone_Wallpaper_bittersweet_b";
		public static readonly string PurpleGlow = "Phone_Wallpaper_purpleglow";
		public static readonly string GreenSquares = "Phone_Wallpaper_greensquares";
		public static readonly string OrangeHerringBone = "Phone_Wallpaper_orangeherringbone";
		public static readonly string OrangeHalftone = "Phone_Wallpaper_orangehalftone";
		public static readonly string GreenTriangles = "Phone_Wallpaper_greentriangles";
		public static readonly string GreenShards = "Phone_Wallpaper_greenshards";
		public static readonly string BlueAngles = "Phone_Wallpaper_blueangles";
		public static readonly string BlueShards = "Phone_Wallpaper_blueshards";
		public static readonly string BlueTriangles = "Phone_Wallpaper_bluetriangles";
		public static readonly string BlueCircles = "Phone_Wallpaper_bluecircles";
		public static readonly string Diamonds = "Phone_Wallpaper_diamonds";
		public static readonly string GreenGlow = "Phone_Wallpaper_greenglow";
		public static readonly string Orange8Bit = "Phone_Wallpaper_orange8bit";
		public static readonly string OrangeTriangles = "Phone_Wallpaper_orangetriangles";
		public static readonly string PurpleTartan = "Phone_Wallpaper_purpletartan";
	}

}
