﻿using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.IPLs.dlc_smuggler;
using TheLastPlanet.Client.NativeUI;
using TheLastPlanet.Client.SessionCache;
using TheLastPlanet.Shared;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client.MAINLOBBY
{
	public enum ModalitaServer
	{
		Lobby = 0,
		Roleplay,
		Minigiochi,
		Gare,
		Negozio
	}

	internal static class MainChooser
	{
		public static Dictionary<int, int> Bucket_n_Players = new() { [1] = 0, [2] = 0, [3] = 0 };

		private static BucketMarker RP_Marker = new(new Marker(MarkerType.VerticalCylinder, new Vector3(-1266.863f, -3013.068f, -49.0f), new Vector3(10f, 10f, 1f), Colors.RoyalBlue), "", "mp_mission_name_freemode_199");
		private static BucketMarker Mini_Marker = new(new Marker(MarkerType.VerticalCylinder, new Vector3(-1280.206f, -3021.234f, -49.0f), new Vector3(10f, 10f, 1f), Colors.ForestGreen), "", "mp_mission_name_freemode_1999");
		private static BucketMarker Gare_Marker = new(new Marker(MarkerType.VerticalCylinder, new Vector3(-1267.147f, -3032.353f, -49.0f), new Vector3(10f, 10f, 1f), Colors.MediumPurple), "", "mp_mission_name_freemode_19999");
		private static BucketMarker Nego_Marker = new(new Marker(MarkerType.VerticalCylinder, new Vector3(-1251.566f, -3032.304f, -49.0f), new Vector3(10f, 10f, 1f), Colors.Orange), "", "mp_mission_name_freemode_199999");
		private static ParticleEffectsAssetNetworked SpawnParticle = new("scr_powerplay");
		private static Scaleform Warning = new("POPUP_WARNING");

		public static void Init()
		{
			Client.Instance.AddTick(Entra);
		}

		public static void Stop()
		{
			Client.Instance.RemoveTick(Entra);
			Client.Instance.RemoveTick(DrawMarkers);
			Client.Instance.RemoveTick(DrawScaleform);
		}

		private static Vector3 _posRp = Vector3.Zero;
		private static Vector3 _posMini = Vector3.Zero;
		private static Vector3 _posGare = Vector3.Zero;
		private static Vector3 _posNego = Vector3.Zero;

		private static async Task DrawMarkers()
		{
			await Cache.Loaded();

			if (_posRp == Vector3.Zero)
			{
				_posRp = await RP_Marker.Marker.Position.GetVector3WithGroundZ();
				RP_Marker.Marker.Position = _posRp;
			}

			if (_posMini == Vector3.Zero)
			{
				_posMini = await Mini_Marker.Marker.Position.GetVector3WithGroundZ();
				Mini_Marker.Marker.Position = _posMini;
			}

			if (_posGare == Vector3.Zero)
			{
				_posGare = await Gare_Marker.Marker.Position.GetVector3WithGroundZ();
				Gare_Marker.Marker.Position = _posGare;
			}

			if (_posNego == Vector3.Zero)
			{
				_posNego = await Nego_Marker.Marker.Position.GetVector3WithGroundZ();
				Nego_Marker.Marker.Position = _posNego;
			}

			RP_Marker.Scaleform.CallFunction("SET_MISSION_INFO", "Immergiti nella simulazione!", "~b~Server RolePlay~w~", "", "", "", "", $"{Bucket_n_Players[1]}/256", "", "", "");
			Mini_Marker.Scaleform.CallFunction("SET_MISSION_INFO", "Minigiochi a squadre o singoli!", "~g~Minigiochi~w~", "", "", "", "", $"{Bucket_n_Players[2]}/64", "", "", "");
			Gare_Marker.Scaleform.CallFunction("SET_MISSION_INFO", "Gareggia contro tutti!", "~p~Gare~w~", "", "", "", "", $"{Bucket_n_Players[3]}/64", "", "", "");
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
					if (Bucket_n_Players[1] == 256)
					{
						HUD.ShowNotification("Il server RolePlay è pieno al momento, riprova più tardi!", NotificationColor.Red, true);

						return;
					}

					await CambiaBucket("~b~Server RolePlay~w~", 1);
					await RolePlay.Initializer.Init();
					Stop();
				}
			}

			if (Mini_Marker.Marker.IsInMarker)
			{
				HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per entrare nel mondo dei ~g~Minigiochi~w~");

				if (Input.IsControlJustPressed(Control.Context))
				{
					if (Bucket_n_Players[2] == 64)
					{
						HUD.ShowNotification("Il mondo dei Minigiochi è pieno al momento, riprova più tardi!", NotificationColor.Red, true);

						return;
					}

					await CambiaBucket("~g~Server Minigiochi~w~", 2);
					Screen.Fading.FadeIn(1000);
				}
			}

			if (Gare_Marker.Marker.IsInMarker)
			{
				HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per entrare nel mondo delle ~p~Gare~w~");

				if (Input.IsControlJustPressed(Control.Context))
				{
					if (Bucket_n_Players[3] == 64)
					{
						HUD.ShowNotification("Il mondo delle Gare è pieno al momento, riprova più tardi!", NotificationColor.Red, true);

						return;
					}

					await CambiaBucket("~p~Server Gare~w~", 3);
					Races.Creator.RaceCreator.CreatorPreparation();
					await Task.FromResult(0);
				}
			}

			if (Nego_Marker.Marker.IsInMarker)
			{
				HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per entrare nel ~o~Negozio~w~");

				if (Input.IsControlJustPressed(Control.Context))
				{
					await CambiaBucket("~o~Negozio~w~", 4);
					Screen.Fading.FadeIn(1000);
				}
			}
		}

		private static async Task DrawScaleform()
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
			SmugglerHangar.LoadDefault();
			await Cache.InitPlayer();
			while (!NetworkIsPlayerActive(Cache.MyPlayer.Player.Handle)) await BaseScript.Delay(0);
			BaseScript.TriggerServerEvent("lprp:coda: playerConnected");
			Client.Instance.NuiManager.SendMessage(new { resname = GetCurrentResourceName() });
			await Cache.MyPlayer.Player.ChangeModel(new Model(PedHash.FreemodeMale01));
			Cache.MyPlayer.UpdatePedId();
			Cache.MyPlayer.Ped.IsPositionFrozen = true;
			Cache.MyPlayer.Player.IgnoredByPolice = true;
			Cache.MyPlayer.Player.DispatchsCops = false;
			NetworkSetTalkerProximity(-1000f);
			Screen.Hud.IsRadarVisible = false;
			CharSelect();
		}

		public static async void CharSelect()
		{
			SpawnParticle.Request();
			while (!SpawnParticle.IsLoaded) await BaseScript.Delay(0);
			Cache.MyPlayer.Player.CanControlCharacter = true;
			if (Cache.MyPlayer.Ped.IsVisible) NetworkFadeOutEntity(Cache.MyPlayer.Ped.Handle, true, false);
			RequestCollisionAtCoord(-1266.726f, -2986.766f, -48f);
			Cache.MyPlayer.Ped.Position = new Vector3(-1266.726f, -2986.766f, -49.2f);
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
			Cache.MyPlayer.User.StatiPlayer.Bucket = 0;
			Bucket_n_Players = await Client.Instance.Eventi.Get<Dictionary<int, int>>("lprp:richiediContoBuckets");
			await BaseScript.Delay(100);
			Cache.MyPlayer.Player.State.Set("Pausa", new { Attivo = false }, true);
			Cache.MyPlayer.Ped.IsPositionFrozen = false;
			SmugglerHangar.LoadDefault();
			ShutdownLoadingScreen();
			ShutdownLoadingScreenNui();
			Screen.Fading.FadeIn(1000);
			NetworkFadeInEntity(Cache.MyPlayer.Ped.Handle, true);
			SpawnParticle.StartNonLoopedOnEntityNetworked("scr_powerplay_beast_appear", Cache.MyPlayer.Ped);
			Client.Instance.AddTick(DrawMarkers);
			Client.Instance.AddTick(DrawScaleform);
		}

		#endregion

		private static async Task CambiaBucket(string nome, int id)
		{
			Cache.MyPlayer.Player.CanControlCharacter = false;
			Screen.Fading.FadeOut(500);
			await BaseScript.Delay(500);
			Warning.CallFunction("SHOW_POPUP_WARNING", 1000f, nome, "Ingresso nella sezione in corso...", "", true, 0, "The Last Planet ver. 5.78.995");
			Screen.Fading.FadeIn(1);
			await BaseScript.Delay(3000);
			bool dentro = await Client.Instance.Eventi.Get<bool>("lprp:checkSeGiaDentro", id);

			if (dentro)
			{
				Warning.CallFunction("SHOW_POPUP_WARNING", 3000f, nome, "Errore nel caricamento... Ritorno alla lobby!", "", true, 0, "The Last Planet ver. 5.78.995");
				await BaseScript.Delay(3000);
				PlayerSpawned();
				await BaseScript.Delay(1000);
				Warning.CallFunction("HIDE_POPUP_WARNING", 2000f);

				return;
			}

			Warning.CallFunction("SHOW_POPUP_WARNING", 3000f, nome, "Caricamento completato!", "", true, 0, "The Last Planet ver. 5.78.995");
			Cache.MyPlayer.User.StatiPlayer.Bucket = id;
			Bucket_n_Players = await Client.Instance.Eventi.Get<Dictionary<int, int>>("lprp:richiediContoBuckets");
			await BaseScript.Delay(3000);
			Screen.Fading.FadeOut(1);
			Warning.CallFunction("HIDE_POPUP_WARNING", 2000f);
			Cache.MyPlayer.Player.CanControlCharacter = true;
			Stop();
		}
	}
}