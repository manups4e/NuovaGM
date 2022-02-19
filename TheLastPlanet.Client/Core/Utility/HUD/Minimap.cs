﻿using System.Threading.Tasks;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Core;
using TheLastPlanet.Shared.Internal.Events;

namespace TheLastPlanet.Client.Core.Utility.HUD
{
    internal static class Minimap
    {
        //public static Scaleform minimap = new Scaleform("MINIMAP");
        public static async void Init()
        {
            AccessingEvents.OnRoleplaySpawn += Spawnato;
            AccessingEvents.OnRoleplayLeave += onPlayerLeft;
            AccessingEvents.OnFreeRoamSpawn += Spawnato;
            AccessingEvents.OnFreeRoamLeave += onPlayerLeft;
        }
        public static void Spawnato(ClientId client)
        {
            Client.Instance.AddTick(MinimapDrawing);
        }
        public static void onPlayerLeft(ClientId client)
        {
            Client.Instance.RemoveTick(MinimapDrawing);
        }

        public static async Task MinimapDrawing()
        {
            await PlayerCache.Loaded();

            Ped p = Cache.PlayerCache.MyPlayer.Ped;

            switch (Cache.PlayerCache.ModalitàAttuale)
            {
                case ModalitaServer.Lobby:
                    if (Screen.Hud.IsRadarVisible)
                        Screen.Hud.IsRadarVisible = false;
                    break;
                case ModalitaServer.FreeRoam:
                    if (PlayerCache.MyPlayer.User.Status.Istanza.Instance == "CreazionePersonaggio")
                    {
                        if (Screen.Hud.IsRadarVisible)
                            Screen.Hud.IsRadarVisible = false;
                        break;
                    }
                    if (!Screen.Hud.IsRadarVisible)
                        Screen.Hud.IsRadarVisible = true;
                    break;
                case ModalitaServer.Roleplay:
                    // SE NON STO NASCONDENDO L'HUD (cinematica)
                    if (!Main.ImpostazioniClient.ModCinema)
                    {
                        if (Main.ImpostazioniClient.MiniMappaAttiva)
                        {
                            if (!IsRadarEnabled()) Screen.Hud.IsRadarVisible = true;

                            switch (Main.ImpostazioniClient.DimensioniMinimappa)
                            {
                                // se ho settato la minimappa piccina
                                case 0:
                                    {
                                        if (IsBigmapActive())              // se attualmente la minimappa è ingrandita
                                            SetBigmapActive(false, false); // riduciamola

                                        break;
                                    }
                                // altrimenti
                                case 1:
                                    {
                                        if (!IsBigmapActive())            // se è piccina
                                            SetBigmapActive(true, false); // ingrandiscila

                                        break;
                                    }
                            }

                            switch (Cache.PlayerCache.MyPlayer.User.Status.PlayerStates.InVeicolo)
                            {
                                //se non sono su un veicolo e non ho il menu di pausa attivo.
                                case false when !IsPauseMenuActive():
                                    DisableRadarThisFrame(); // lascia la minimappa attiva, ma nasconda la mappa se non sono in un veicolo

                                    break;
                                case true when Main.ImpostazioniClient.MiniMappaInAuto:
                                    {
                                        Vehicle veh = p.CurrentVehicle;

                                        if (veh == null) return;
                                        if (veh.Model.IsBicycle || IsThisModelAJetski((uint)veh.Model.Hash) || veh.Model.IsQuadbike || !veh.IsEngineRunning) DisableRadarThisFrame();

                                        break;
                                    }
                                case true:
                                    DisableRadarThisFrame();

                                    break;
                            }
                        }
                        else
                        {
                            if (IsRadarEnabled()) Screen.Hud.IsRadarVisible = false;
                        }
                    }
                    else
                    {
                        if (IsRadarEnabled()) Screen.Hud.IsRadarVisible = false;
                    }
                    break;
            }
            await Task.FromResult(0);
        }
    }
}