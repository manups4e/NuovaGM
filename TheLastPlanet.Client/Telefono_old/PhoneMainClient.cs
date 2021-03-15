using System;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using Newtonsoft.Json;
using TheLastPlanet.Client.Core.Utility.HUD;
using Logger;
using TheLastPlanet.Shared;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Telefono.Models;

namespace TheLastPlanet.Client.Telefono
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
			ClientSession.Instance.AddEventHandler("lprp:setupPhoneClientUser", new Action<string>(Setup));
			ClientSession.Instance.AddEventHandler("lprp:phone_start", new Action<string>(StartApp));
		}

		private static void Setup(string JsonTelefono) 
		{
			Log.Printa(LogType.Debug, JsonTelefono);
			if (JsonTelefono != "{\"phone_data\":[]}" && !string.IsNullOrEmpty(JsonTelefono) && !string.IsNullOrWhiteSpace(JsonTelefono))
				Phone = new Phone(JsonTelefono.DeserializeFromJson<Phone>());
			else
				Phone = new Phone();
			ClientSession.Instance.AddTick(ControlloApertura);
		}

		public static async Task ControlloApertura()
		{
			Ped ped = CachePlayer.Cache.MyPlayer.Ped;
			if (!(HUD.MenuPool.IsAnyMenuOpen || Game.IsPaused || Banking.BankingClient.InterfacciaAperta || ped.IsAiming || ped.IsAimingFromCover || ped.IsShooting))
			{
				if (Input.IsControlJustPressed(Control.Phone) && !IsPedRunningMobilePhoneTask(ped.Handle))
				{
					Phone.OpenPhone();
					Phone.currentApp = Phone.mainApp;
				}
				if (IsPedRunningMobilePhoneTask(ped.Handle))
				{
					if (Input.IsControlJustPressed(Control.PhoneCancel))
					{
						if (Phone.IsBackOverriddenByApp)
							Phone.IsBackOverriddenByApp = false;
						else
							KillApp();
					}
				}
			}
		}

		public static void StartApp(string app)
		{
			if (app == "Main")
			{
				KillApp();
				if (Phone.currentApp != null)
				{
					Phone.currentApp.Kill();
					ClientSession.Instance.RemoveTick(Phone.currentApp.Tick);
				}
				Phone.currentApp = Phone.mainApp;
			}
			else if (Phone.apps.Exists(x => x.Name == app))
			{
				ClientSession.Instance.RemoveTick(Phone.mainApp.Tick);
				Phone.currentApp = Phone.apps.FirstOrDefault(x => x.Name == app);
			}

			Phone.currentApp.Initialize(Phone);
			ClientSession.Instance.AddTick(Phone.currentApp.Tick);

			Log.Printa(LogType.Debug, $"CurrentApp = {Phone.currentApp.Name}");
		}

		public static void KillApp()
		{
			if (Phone.currentApp != null)
			{
				Log.Printa(LogType.Debug, $"Killing App {Phone.currentApp.Name}");
				ClientSession.Instance.RemoveTick(Phone.currentApp.Tick);
				Phone.currentApp.Kill();

				App lastApp = Phone.currentApp;
				Phone.currentApp = null;

				if (lastApp.Name == "Main")
				{
					foreach (App app in Phone.apps)
					{
						app.Kill();
						ClientSession.Instance.RemoveTick(app.Tick);
					}
					Phone.ClosePhone();
				}
				else
				{
					Game.PlaySound("Menu_Navigate", "Phone_SoundSet_Default");
					StartApp("Main");
				}
			}
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
