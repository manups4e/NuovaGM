using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.MODALITA.MAINLOBBY;

namespace TheLastPlanet.Client.Core.Ingresso
{
    internal static class ServerJoining
    {
        private static bool _firstTick = true;
        internal static readonly ParticleEffectsAssetNetworked SpawnParticle = new("scr_powerplay");

        public static async void Init()
        {
            EventDispatcher.Mount("tlg:SetBucketsPlayers", new Action<Dictionary<ModalitaServer, int>>(UpdateCountPlayers));
#if DEBUG
            Client.Instance.AddTick(Entra);
#endif
            InternalGameEvents.PlayerJoined += InternalGameEvents_PlayerJoined;
        }

        private static void InternalGameEvents_PlayerJoined()
        {
            PlayerSpawned();
        }

        private static async Task Entra()
        {
            await BaseScript.Delay(100);
            if (NetworkIsSessionStarted() && _firstTick)
            {
                _firstTick = false;
                PlayerSpawned();
                Client.Instance.RemoveTick(Entra);
            }

            await Task.FromResult(0);
        }

        private static void UpdateCountPlayers(Dictionary<ModalitaServer, int> count)
        {
            MainChooser.Bucket_n_Players = count;
        }

        public static async void PlayerSpawned()
        {
            _firstTick = false;
            Screen.Fading.FadeOut(800);
            while (!Screen.Fading.IsFadedOut) await BaseScript.Delay(1000);
            IPLs.IPLInstance.SmugglerHangar.LoadDefault();
            await PlayerCache.InitPlayer();
            while (!NetworkIsPlayerActive(PlayerCache.MyPlayer.Player.Handle)) await BaseScript.Delay(0);
            BaseScript.TriggerServerEvent("lprp:coda:playerConnected");
            Client.Instance.NuiManager.SendMessage(new { resname = GetCurrentResourceName() });
            if (PlayerCache.MyPlayer.Ped.Model.Hash != (int)PedHash.FreemodeMale01)
                await PlayerCache.MyPlayer.Player.ChangeModel(new Model(PedHash.FreemodeMale01));
            NetworkSetTalkerProximity(-1000f);

            // eventi di utility.. da sistemare (togliere gli eventi non di utility e spostarli)
            Eventi.Init();
            // TODO: gestire questa parte separatamente per i vari pianeti
            PlayerCache.MyPlayer.Ped.IsPositionFrozen = true;
            PlayerCache.MyPlayer.Player.IgnoredByPolice = true;
            PlayerCache.MyPlayer.Player.DispatchsCops = false;
            Screen.Hud.IsRadarVisible = false;
            // TODO: gestire questa parte separatamente per i vari pianeti
            CharSelect();
        }

        public static async void CharSelect()
        {
            SpawnParticle.Request();
            while (!SpawnParticle.IsLoaded) await BaseScript.Delay(0);
            PlayerCache.MyPlayer.Player.CanControlCharacter = true;
            if (PlayerCache.MyPlayer.Ped.IsVisible) NetworkFadeOutEntity(PlayerCache.MyPlayer.Ped.Handle, true, false);
            RequestCollisionAtCoord(-1266.726f, -2986.766f, -48f);
            PlayerCache.MyPlayer.Ped.Position = new Vector3(-1266.726f, -2986.766f, -49.2f);
            PlayerCache.MyPlayer.Ped.Heading = 176.1187f;
            Ped p = PlayerCache.MyPlayer.Ped;
            p.Style.SetDefaultClothes();
            await PlayerCache.Loaded();
            PlayerCache.MyPlayer.Status.PlayerStates.Modalita = ModalitaServer.Lobby;
            PlayerCache.MyPlayer.Ped.IsPositionFrozen = false;
            ShutdownLoadingScreen();
            ShutdownLoadingScreenNui();
            ClampGameplayCamPitch(0, 0);
            ClampGameplayCamYaw(0, 0);
            Screen.Fading.FadeIn(1000);
            //MainChooser.Bucket_n_Players = await EventDispatcher.Get<Dictionary<ModalitaServer, int>>("tlg:richiediContoBuckets");
            SpawnParticle.StartNonLoopedOnEntityNetworked("scr_powerplay_beast_appear", PlayerCache.MyPlayer.Ped);
            Function.Call(Hash.NETWORK_FADE_IN_ENTITY, PlayerCache.MyPlayer.Ped.Handle, true, 1);
            MainChooser.Init();
            PlayerCache.MyPlayer.Status.PlayerStates.ModalitaPassiva = true;
            SpawnParticle.MarkAsNoLongerNeeded();
        }

        public static async void ReturnToLobby()
        {
            PlayerCache.MyPlayer.Player.CanControlCharacter = false;
            Screen.Fading.FadeOut(500);
            while (Screen.Fading.IsFadingOut) await BaseScript.Delay(0);
            SpawnParticle.Request();
            while (!SpawnParticle.IsLoaded) await BaseScript.Delay(0);
            if (PlayerCache.MyPlayer.Ped.IsVisible) NetworkFadeOutEntity(PlayerCache.MyPlayer.Ped.Handle, true, false);
            RequestCollisionAtCoord(-1266.726f, -2986.766f, -48f);
            PlayerCache.MyPlayer.Ped.Position = new Vector3(-1266.726f, -2986.766f, -49.2f);
            PlayerCache.MyPlayer.Ped.Heading = 176.1187f;

            // TODO: sostituire con caricamento personaggio freeroam.
            PlayerCache.MyPlayer.Ped.Style.SetDefaultClothes();

            PlayerCache.MyPlayer.Status.PlayerStates.Modalita = ModalitaServer.Lobby;
            PlayerCache.MyPlayer.Ped.IsPositionFrozen = false;
            Screen.Fading.FadeIn(1000);
            SpawnParticle.StartNonLoopedOnEntityNetworked("scr_powerplay_beast_appear", PlayerCache.MyPlayer.Ped);
            Function.Call(Hash.NETWORK_FADE_IN_ENTITY, PlayerCache.MyPlayer.Ped.Handle, true, 1);
            PlayerCache.MyPlayer.Player.CanControlCharacter = true;
            MainChooser.Init();
            PlayerCache.MyPlayer.Status.PlayerStates.ModalitaPassiva = true;
            SpawnParticle.MarkAsNoLongerNeeded();
        }
    }
}