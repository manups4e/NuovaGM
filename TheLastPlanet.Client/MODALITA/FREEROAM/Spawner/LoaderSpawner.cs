﻿using CitizenFX.Core;
using CitizenFX.Core.Native;
using static CitizenFX.Core.Native.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.MODALITA.FREEROAM.Creator;
using TheLastPlanet.Shared;
using TheLastPlanet.Client.Cache;
using CitizenFX.Core.UI;
using TheLastPlanet.Client.MODALITA.FREEROAM.Managers;
using Impostazioni.Shared.Configurazione.Generici;

namespace TheLastPlanet.Client.MODALITA.FREEROAM.Spawner
{
    internal class LoaderSpawner
    {
        public static void Init()
        {
            Inizializza();
        }

        private static async void Inizializza()
        {
            //Cache.PlayerCache.MyPlayer.User.FreeRoamChar = await Client.Instance.Events.Get<FreeRoamChar>("lprp:Select_FreeRoamChar", Cache.PlayerCache.MyPlayer.User.ID);
            var roamchar = await Client.Instance.Events.Get<FreeRoamChar>("tlg:Select_FreeRoamChar", Cache.PlayerCache.MyPlayer.User.ID);
            PlayerCache.MyPlayer.User.FreeRoamChar = roamchar;

            if (roamchar.CharID == 0 && roamchar.Skin is null)
            {
                API.RequestModel((uint)PedHash.FreemodeMale01);
                API.RequestModel((uint)PedHash.FreemodeFemale01);
                FreeRoamCreator.Init();
                var sex = Funzioni.GetRandomInt(0, 100) > 50 ? "Maschio" : "Femmina";
                await FreeRoamCreator.CharCreationMenu(sex);
                return;
            }

            if (PlayerCache.MyPlayer.Ped.IsVisible) NetworkFadeOutEntity(PlayerCache.MyPlayer.Ped.Handle, true, false);
            var apos = PlayerCache.MyPlayer.Ped.Position;
            var rpos = roamchar.Posizione;

            if (rpos is null)
                rpos = Position.Zero; // TODO: DA SOSTITUIRE

            int switchType = GetIdealPlayerSwitchType(apos.X, apos.Y, apos.Z, rpos.X, rpos.Y, rpos.Z);
            SwitchOutPlayer(PlayerCache.MyPlayer.Ped.Handle, 1 | 32 | 128 | 16384, switchType);
            await BaseScript.Delay(2000);
            if(Screen.Fading.IsFadedOut) Screen.Fading.FadeIn(1000);

            Screen.LoadingPrompt.Show("Caricamento", LoadingSpinnerType.Clockwise1);

            await LoadChar();

            if (Screen.LoadingPrompt.IsActive) Screen.LoadingPrompt.Hide();
            Screen.LoadingPrompt.Show("Sincronizzazione col server", LoadingSpinnerType.Clockwise1);

            await BaseScript.Delay(1000);

            StartPlayerTeleport(PlayerId(), rpos.X, rpos.Y, rpos.Z, rpos.Heading, false, true, true);
            while (!HasPlayerTeleportFinished(PlayerId())) await BaseScript.Delay(0);

            Client.Instance.Events.Send("worldEventsManage.Server:AddParticipant");

            ExperienceManager.Init();
            WorldEventsManager.Init();
            ExperienceManager.Init();
            PlayerBlipsHandler.Init();
            //AGGIUNGERE GESTIONE METEO
            //AGGIUNGERE GESTIONE ORARIO
            //AGGIUNGERE GESTIONE STATISTICHE
            //AGGIUNGERE GESTIONE MORTE (SE POSSIBILE SERVERSIDE)
            BaseEventsFreeRoam.Init();
            //Death.Init();
            PlayerTags.Init();

            SwitchInPlayer(PlayerCache.MyPlayer.Ped.Handle);
            while (IsPlayerSwitchInProgress()) await BaseScript.Delay(0);
            if (!PlayerCache.MyPlayer.Ped.IsVisible) NetworkFadeInEntity(PlayerCache.MyPlayer.Ped.Handle, true);
        }

        public static async Task LoadChar()
        {
            uint model = PlayerCache.MyPlayer.User.FreeRoamChar.Skin.model;
            while (!await PlayerCache.MyPlayer.Player.ChangeModel(new Model((PedHash)model))) await BaseScript.Delay(0);

            Funzioni.UpdateFace(PlayerCache.MyPlayer.Ped.Handle, PlayerCache.MyPlayer.User.FreeRoamChar.Skin);
            Funzioni.UpdateDress(PlayerCache.MyPlayer.Ped.Handle, PlayerCache.MyPlayer.User.FreeRoamChar.Dressing);

            PlayerCache.MyPlayer.Ped.Weapons.RemoveAll();

            StatSetInt(Funzioni.HashUint("MP0_WALLET_BALANCE"), Cache.PlayerCache.MyPlayer.User.Money, true);
            StatSetInt(Funzioni.HashUint("BANK_BALANCE"), Cache.PlayerCache.MyPlayer.User.Bank, true);

        }

        public static void RestoreWeapons()
        {
            Dictionary<int, bool> ammoTypes = new();

            if (PlayerCache.MyPlayer.User.FreeRoamChar.Weapons.Count > 0)
            {
                PlayerCache.MyPlayer.Ped.Weapons.RemoveAll();
                List<Weapons> weaps = PlayerCache.MyPlayer.User.GetCharWeapons();

                for (int i = 0; i < weaps.Count; i++)
                {
                    string weaponName = weaps[i].name;
                    uint weaponHash = Funzioni.HashUint(weaponName);
                    int tint = weaps[i].tint;
                    PlayerCache.MyPlayer.Ped.Weapons.Give((WeaponHash)weaponHash, 0, false, false);
                    int ammoType = GetPedAmmoTypeFromWeapon(PlayerPedId(), weaponHash);

                    if (weaps[i].components.Count > 0)
                        foreach (Components weaponComponent in weaps[i].components)
                        {
                            uint componentHash = Funzioni.HashUint(weaponComponent.name);
                            if (weaponComponent.active) GiveWeaponComponentToPed(PlayerPedId(), weaponHash, componentHash);
                        }

                    SetPedWeaponTintIndex(PlayerPedId(), weaponHash, tint);

                    if (ammoTypes.ContainsKey(ammoType)) continue;
                    AddAmmoToPed(PlayerPedId(), weaponHash, weaps[i].ammo);
                    ammoTypes[ammoType] = true;
                }
            }
        }

    }
}