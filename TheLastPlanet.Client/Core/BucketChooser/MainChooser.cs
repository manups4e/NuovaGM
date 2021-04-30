using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using Logger;
using TheLastPlanet.Client.Core.PlayerChar;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.IPLs.dlc_smuggler;
using TheLastPlanet.Client.MenuNativo;
using TheLastPlanet.Client.SessionCache;
using TheLastPlanet.Shared;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client.Core.BucketChooser
{
	internal static class MainChooser
	{
		private static BucketMarker RP_Marker = new(new Marker(MarkerType.VerticalCylinder, new Vector3(-1266.863f, -3013.068f, -49.0f), new Vector3(10f, 10f, 1f), Colors.RoyalBlue), "", "mp_mission_name_freemode_199");
		private static BucketMarker Mini_Marker = new(new Marker(MarkerType.VerticalCylinder, new Vector3(-1280.206f, -3021.234f, -49.0f), new Vector3(10f, 10f, 1f), Colors.ForestGreen), "", "mp_mission_name_freemode_1999");
		private static BucketMarker Gare_Marker = new(new Marker(MarkerType.VerticalCylinder, new Vector3(-1267.147f, -3032.353f, -49.0f), new Vector3(10f, 10f, 1f), Colors.MediumPurple), "", "mp_mission_name_freemode_19999");
		private static BucketMarker Nego_Marker = new(new Marker(MarkerType.VerticalCylinder, new Vector3(-1251.566f, -3032.304f, -49.0f), new Vector3(10f, 10f, 1f), Colors.Orange), "", "mp_mission_name_freemode_199999");
		private static ParticleEffectsAssetNetworked SpawnParticle = new("scr_powerplay");

		public static void Init()
		{
			Client.Instance.AddTick(Entra);
		}

		private static Vector3 posRP = Vector3.Zero;
		private static Vector3 posMini = Vector3.Zero;
		private static Vector3 posGare = Vector3.Zero;
		private static Vector3 posNego = Vector3.Zero;

		private static async Task DrawMarkers()
		{
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
				}
			}
		}

		#region INGRESSO SERVER

		private static async Task Entra()
		{
			if (NetworkIsSessionStarted())
			{
				PlayerSpawned();
				Client.Instance.RemoveTick(Entra);
				Client.Logger.Debug("ci sono");
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
			Client.Instance.AddTick(DrawMarkers);
			Screen.Fading.FadeIn(1000);
			NetworkFadeInEntity(Cache.MyPlayer.Ped.Handle, true);
			ShutdownLoadingScreen();
			ShutdownLoadingScreenNui();
			SpawnParticle.StartNonLoopedOnEntityNetworked("scr_powerplay_beast_appear", Cache.MyPlayer.Ped);
		}

		#endregion
	}
}