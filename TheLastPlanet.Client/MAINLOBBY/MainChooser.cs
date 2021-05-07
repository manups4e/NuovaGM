using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.IPLs.dlc_smuggler;
using TheLastPlanet.Client.MenuNativo;
using TheLastPlanet.Client.SessionCache;
using TheLastPlanet.Shared;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client.MAINLOBBY
{
	internal static class MainChooser
	{
		private static BucketMarker RP_Marker = new(new Marker(MarkerType.VerticalCylinder, new Vector3(-1266.863f, -3013.068f, -49.0f), new Vector3(10f, 10f, 1f), Colors.RoyalBlue), "", "mp_mission_name_freemode_199");
		private static BucketMarker Mini_Marker = new(new Marker(MarkerType.VerticalCylinder, new Vector3(-1280.206f, -3021.234f, -49.0f), new Vector3(10f, 10f, 1f), Colors.ForestGreen), "", "mp_mission_name_freemode_1999");
		private static BucketMarker Gare_Marker = new(new Marker(MarkerType.VerticalCylinder, new Vector3(-1267.147f, -3032.353f, -49.0f), new Vector3(10f, 10f, 1f), Colors.MediumPurple), "", "mp_mission_name_freemode_19999");
		private static BucketMarker Nego_Marker = new(new Marker(MarkerType.VerticalCylinder, new Vector3(-1251.566f, -3032.304f, -49.0f), new Vector3(10f, 10f, 1f), Colors.Orange), "", "mp_mission_name_freemode_199999");
		private static ParticleEffectsAssetNetworked SpawnParticle = new("scr_powerplay");
		private static Scaleform Warning = new("POPUP_WARNING");

		public static void Init()
		{
			Client.Instance.AddTick(Entra);
			Client.Instance.AddTick(DrawMarkers);
			Client.Instance.AddTick(Scaleform);
		}

		public static void Stop()
		{
			Client.Instance.RemoveTick(Entra);
			Client.Instance.RemoveTick(DrawMarkers);
		}

		private static Vector3 posRP = Vector3.Zero;
		private static Vector3 posMini = Vector3.Zero;
		private static Vector3 posGare = Vector3.Zero;
		private static Vector3 posNego = Vector3.Zero;

		private static async Task DrawMarkers()
		{
			await Cache.Loaded();

			if (posRP == Vector3.Zero)
			{
				posRP = await RP_Marker.Marker.Position.GetVector3WithGroundZ();
				RP_Marker.Marker.Position = posRP;
			}

			if (posMini == Vector3.Zero)
			{
				posMini = await Mini_Marker.Marker.Position.GetVector3WithGroundZ();
				Mini_Marker.Marker.Position = posMini;
			}

			if (posGare == Vector3.Zero)
			{
				posGare = await Gare_Marker.Marker.Position.GetVector3WithGroundZ();
				Gare_Marker.Marker.Position = posGare;
			}

			if (posNego == Vector3.Zero)
			{
				posNego = await Nego_Marker.Marker.Position.GetVector3WithGroundZ();
				Nego_Marker.Marker.Position = posNego;
			}

			RP_Marker.Scaleform.CallFunction("SET_MISSION_INFO", "Immergiti nella simulazione!", "~b~Server RolePlay~w~", "", "", "", "", "0/256", "", "", "");
			Mini_Marker.Scaleform.CallFunction("SET_MISSION_INFO", "Minigiochi a squadre o singoli!", "~g~Minigiochi~w~", "", "", "", "", "0/64", "", "", "");
			Gare_Marker.Scaleform.CallFunction("SET_MISSION_INFO", "Gareggia contro tutti!", "~p~Gare~w~", "", "", "", "", "0/64", "", "", "");
			Nego_Marker.Scaleform.CallFunction("SET_MISSION_INFO", "Non influisce sul server RolePlay!", "~o~Negozio~w~", "", "", "", "", "", "", "", "");
			RP_Marker.Draw();
			Mini_Marker.Draw();
			Gare_Marker.Draw();
			Nego_Marker.Draw();

			if (RP_Marker.Marker.IsInMarker)
			{
				HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per entrare nel mondo del ~b~RolePlay~w~");

				if (Input.IsControlJustPressed(Control.Context))
				{
					Screen.Fading.FadeOut(500);
					await BaseScript.Delay(500);
					Warning.CallFunction("SHOW_POPUP_WARNING", 1000f, "~b~Server RolePlay~w~", "Caricamento...", "", true, 0, "The Last Planet ver. 5.78.995");
					Screen.Fading.FadeIn(1);
					await BaseScript.Delay(3000);
					Warning.CallFunction("SHOW_POPUP_WARNING", 3000f, "~b~Server RolePlay~w~", "Caricamento completato!", "", true, 0, "The Last Planet ver. 5.78.995");
					await BaseScript.Delay(3000);
					Screen.Fading.FadeOut(1);
					Warning.CallFunction("HIDE_POPUP_WARNING", 2000f);
					await RolePlay.Initializer.Init();
					Stop();
					await Task.FromResult(0);
				}
			}

			if (Mini_Marker.Marker.IsInMarker)
			{
				HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per entrare nel mondo dei ~g~Minigiochi~w~");

				if (Input.IsControlJustPressed(Control.Context))
				{
					Screen.Fading.FadeOut(500);
					await BaseScript.Delay(500);
					Warning.CallFunction("SHOW_POPUP_WARNING", 1000f, "~g~Server Minigiochi~w~", "Caricamento...", "", true, 0, "The Last Planet ver. 5.78.995");
					Screen.Fading.FadeIn(1);
					await BaseScript.Delay(3000);
					Warning.CallFunction("SHOW_POPUP_WARNING", 3000f, "~g~Server Minigiochi~w~", "Caricamento completato!", "", true, 0, "The Last Planet ver. 5.78.995");
					await BaseScript.Delay(3000);
					Screen.Fading.FadeOut(1);
					Warning.CallFunction("HIDE_POPUP_WARNING", 2000f);
					Screen.Fading.FadeIn(1000);
					await Task.FromResult(0);
				}
			}

			if (Gare_Marker.Marker.IsInMarker)
			{
				HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per entrare nel mondo delle ~p~Gare~w~");

				if (Input.IsControlJustPressed(Control.Context))
				{
					Screen.Fading.FadeOut(500);
					await BaseScript.Delay(500);
					Warning.CallFunction("SHOW_POPUP_WARNING", 1000f, "~p~Server Gare~w~", "Caricamento...", "", true, 0, "The Last Planet ver. 5.78.995");
					Screen.Fading.FadeIn(1);
					await BaseScript.Delay(3000);
					Warning.CallFunction("SHOW_POPUP_WARNING", 3000f, "~p~Server Gare~w~", "Caricamento completato!", "", true, 0, "The Last Planet ver. 5.78.995");
					await BaseScript.Delay(3000);
					Screen.Fading.FadeOut(1);
					Warning.CallFunction("HIDE_POPUP_WARNING", 2000f);
					Screen.Fading.FadeIn(1000);
					await Task.FromResult(0);
				}
			}

			if (Nego_Marker.Marker.IsInMarker)
			{
				HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per entrare nel ~o~Negozio~w~");

				if (Input.IsControlJustPressed(Control.Context))
				{
					Screen.Fading.FadeOut(500);
					await BaseScript.Delay(500);
					Warning.CallFunction("SHOW_POPUP_WARNING", 1000f, "~o~Negozio~w~", "Caricamento...", "", true, 0, "The Last Planet ver. 5.78.995");
					Screen.Fading.FadeIn(1);
					await BaseScript.Delay(3000);
					Warning.CallFunction("SHOW_POPUP_WARNING", 3000f, "~o~Negozio~w~", "Caricamento completato!", "", true, 0, "The Last Planet ver. 5.78.995");
					await BaseScript.Delay(3000);
					Screen.Fading.FadeOut(1);
					Warning.CallFunction("HIDE_POPUP_WARNING", 2000f);
					Screen.Fading.FadeIn(1000);
					await Task.FromResult(0);
				}
			}
		}

		private static async Task Scaleform()
		{
			Warning.Render2D();
			await Task.FromResult(0);
		}

		#region INGRESSO SERVER

		private static bool _firstTick = true;

		private static async Task Entra()
		{
			if (NetworkIsSessionStarted() || _firstTick)
			{
				_firstTick = false;
				PlayerSpawned();
				Client.Instance.RemoveTick(Entra);
			}

			await Task.FromResult(0);
		}

		private static async void PlayerSpawned()
		{
			Screen.Fading.FadeOut(800);
			while (!Screen.Fading.IsFadedOut) await BaseScript.Delay(1000);
			await Cache.InitPlayer();
			while (!NetworkIsPlayerActive(Cache.MyPlayer.Player.Handle)) await BaseScript.Delay(0);
			BaseScript.TriggerServerEvent("lprp:coda: playerConnected");
			Client.Instance.NuiManager.SendMessage(new { resname = GetCurrentResourceName() });
			await Cache.MyPlayer.Player.ChangeModel(new Model(PedHash.FreemodeMale01));
			Cache.MyPlayer.UpdatePedId();
			Cache.MyPlayer.Ped.IsPositionFrozen = false;
			Cache.MyPlayer.Player.IgnoredByPolice = true;
			Cache.MyPlayer.Player.DispatchsCops = false;
			NetworkSetTalkerProximity(-1000f);
			Screen.Hud.IsRadarVisible = false;
			CharSelect();
		}

		public static async void CharSelect()
		{
			SpawnParticle.Request();
			SmugglerHangar.LoadDefault();
			while (!SpawnParticle.IsLoaded) await BaseScript.Delay(0);
			Cache.MyPlayer.Player.CanControlCharacter = true;
			if (Cache.MyPlayer.Ped.IsVisible) NetworkFadeOutEntity(Cache.MyPlayer.Ped.Handle, true, false);
			Cache.MyPlayer.Ped.Position = await new Vector3(-1266.726f, -2986.766f, -48f).GetVector3WithGroundZ();
			Cache.MyPlayer.Ped.Heading = 176.1187f;
			await Cache.MyPlayer.Player.ChangeModel(new Model(PedHash.FreemodeMale01));
			Cache.MyPlayer.UpdatePedId();
			Cache.MyPlayer.Ped.Style.SetDefaultClothes();
			while (!await Cache.MyPlayer.Player.ChangeModel(new Model(PedHash.FreemodeMale01))) await BaseScript.Delay(50);
			Cache.MyPlayer.UpdatePedId();
			Ped p = Cache.MyPlayer.Ped;
			p.Style.SetDefaultClothes();
			p.SetDecor("TheLastPlanet2019fighissimo!yeah!", p.Handle);
			await Cache.Loaded();
			Cache.MyPlayer.User.StatiPlayer.Istanza.Istanzia("Ingresso");
			await BaseScript.Delay(100);
			Cache.MyPlayer.Player.State.Set("Pausa", new { Attivo = false }, true);
			Screen.Fading.FadeIn(1000);
			NetworkFadeInEntity(Cache.MyPlayer.Ped.Handle, true);
			ShutdownLoadingScreen();
			ShutdownLoadingScreenNui();
			SpawnParticle.StartNonLoopedOnEntityNetworked("scr_powerplay_beast_appear", Cache.MyPlayer.Ped);
		}

		#endregion
	}
}