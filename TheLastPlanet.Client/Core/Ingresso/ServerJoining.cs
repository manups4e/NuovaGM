using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.IPLs.dlc_smuggler;
using TheLastPlanet.Client.MODALITA.MAINLOBBY;
using TheLastPlanet.Shared;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client.Core.Ingresso
{
	internal static class ServerJoining
	{
		private static bool _firstTick = true;
		internal static readonly ParticleEffectsAssetNetworked SpawnParticle = new("scr_powerplay");

		public static void Init()
		{
			Client.Instance.AddTick(Entra);
			Client.Instance.Events.Mount("tlg:SetBucketsPlayers", new Action<Dictionary<ModalitaServer, int>>(UpdateCountPlayers));
		}

		private static async void UpdateCountPlayers(Dictionary<ModalitaServer, int> count)
		{
			await BaseScript.Delay(0);
			MainChooser.Bucket_n_Players = count;
			await BaseScript.Delay(0);
			Client.Logger.Debug($"MainChooser.Bucket_n_Players => {MainChooser.Bucket_n_Players.ToJson()}");
		}

		private static async Task Entra()
		{
			if (NetworkIsSessionStarted() && _firstTick)
			{
				_firstTick = false;
				PlayerSpawned();
				Client.Instance.RemoveTick(Entra);
			}

			await Task.FromResult(0);
		}

		public static async void PlayerSpawned()
		{
			Screen.Fading.FadeOut(800);
			while (!Screen.Fading.IsFadedOut) await BaseScript.Delay(1000);
			SmugglerHangar.LoadDefault();
			await Cache.PlayerCache.InitPlayer();
			Client.Logger.Debug("Player => " + Cache.PlayerCache.MyPlayer.Player.Name);
			while (!NetworkIsPlayerActive(Cache.PlayerCache.MyPlayer.Player.Handle)) await BaseScript.Delay(0);
			BaseScript.TriggerServerEvent("lprp:coda: playerConnected");
			Client.Instance.NuiManager.SendMessage(new { resname = GetCurrentResourceName() });
			if (Cache.PlayerCache.MyPlayer.Ped.Model.Hash != (int) PedHash.FreemodeMale01)
			{
				await Cache.PlayerCache.MyPlayer.Player.ChangeModel(new Model(PedHash.FreemodeMale01));
			}
			NetworkSetTalkerProximity(-1000f);

			// TODO: gestire questa parte separatamente per i vari pianeti
			Cache.PlayerCache.MyPlayer.Ped.IsPositionFrozen = true;
			Cache.PlayerCache.MyPlayer.Player.IgnoredByPolice = true;
			Cache.PlayerCache.MyPlayer.Player.DispatchsCops = false;
			Screen.Hud.IsRadarVisible = false;
			// TODO: gestire questa parte separatamente per i vari pianeti
			CharSelect();
		}

		public static async void CharSelect()
		{
			SpawnParticle.Request();
			while (!SpawnParticle.IsLoaded) await BaseScript.Delay(0);
			Cache.PlayerCache.MyPlayer.Player.CanControlCharacter = true;
			if (Cache.PlayerCache.MyPlayer.Ped.IsVisible) NetworkFadeOutEntity(Cache.PlayerCache.MyPlayer.Ped.Handle, true, false);
			RequestCollisionAtCoord(-1266.726f, -2986.766f, -48f);
			Cache.PlayerCache.MyPlayer.Ped.Position = new Vector3(-1266.726f, -2986.766f, -49.2f);
			Cache.PlayerCache.MyPlayer.Ped.Heading = 176.1187f;
			Ped p = Cache.PlayerCache.MyPlayer.Ped;
			p.Style.SetDefaultClothes();
			await Cache.PlayerCache.Loaded();
			Cache.PlayerCache.MyPlayer.User.Status.PlayerStates.Modalita = ModalitaServer.Lobby;
			Cache.PlayerCache.MyPlayer.Ped.IsPositionFrozen = false;
			ShutdownLoadingScreen();
			ShutdownLoadingScreenNui();
			Screen.Fading.FadeIn(1000);
			MainChooser.Bucket_n_Players = await Client.Instance.Events.Get<Dictionary<ModalitaServer, int>>("tlg:richiediContoBuckets");
			SpawnParticle.StartNonLoopedOnEntityNetworked("scr_powerplay_beast_appear", Cache.PlayerCache.MyPlayer.Ped);
			NetworkFadeInEntity(Cache.PlayerCache.MyPlayer.Ped.Handle, true);
			Client.Instance.AddTick(MainChooser.DrawMarkers);
			SpawnParticle.MarkAsNoLongerNeeded();
		}
	}
}