using Impostazioni.Shared.Configurazione.Generici;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.MODALITA.FREEROAM.CharCreation;

namespace TheLastPlanet.Client.MODALITA.FREEROAM.Spawner
{
    public static class FreeRoamLogin
    {
        internal static async void Inizializza()
        {
            var roamchar = await Client.Instance.Events.Get<FreeRoamChar>("tlg:Select_FreeRoamChar", Cache.PlayerCache.MyPlayer.User.ID);
            PlayerCache.MyPlayer.User.FreeRoamChar = roamchar;

            if (roamchar.CharID == 0 && roamchar.Skin is null)
            {
                RequestModel((uint)PedHash.FreemodeMale01);
                RequestModel((uint)PedHash.FreemodeFemale01);
                FreeRoamCreator.Init();
                var sex = Funzioni.GetRandomInt(0, 100) > 50 ? "Maschio" : "Femmina";
                await FreeRoamCreator.CharCreationMenu(sex);
                return;
            }

            await CaricaPlayer(roamchar);
        }

        public static async Task CaricaPlayer(FreeRoamChar roamchar)
        {
            PlayerCache.MyPlayer.Player.CanControlCharacter = false;
            PlayerCache.MyPlayer.Ped.IsPositionFrozen = true;

            if (PlayerCache.MyPlayer.Ped.IsVisible) NetworkFadeOutEntity(PlayerCache.MyPlayer.Ped.Handle, true, false);
            var apos = PlayerCache.MyPlayer.Ped.Position;
            var rpos = roamchar.Posizione;

            if (rpos is null)
                rpos = Position.Zero; // TODO: DA SOSTITUIRE

            int switchType = GetIdealPlayerSwitchType(apos.X, apos.Y, apos.Z, rpos.X, rpos.Y, rpos.Z);
            SwitchOutPlayer(PlayerCache.MyPlayer.Ped.Handle, 1 | 32 | 128 | 16384, switchType);
            await BaseScript.Delay(2000);
            if (Screen.Fading.IsFadedOut) Screen.Fading.FadeIn(1000);

            Screen.LoadingPrompt.Show("Caricamento", LoadingSpinnerType.Clockwise1);

            await LoadChar();

            if (Screen.LoadingPrompt.IsActive) Screen.LoadingPrompt.Hide();
            Screen.LoadingPrompt.Show("Sincronizzazione col server", LoadingSpinnerType.Clockwise1);
            NetworkClearClockTimeOverride();

            await BaseScript.Delay(7000);
            Client.Instance.Events.Send("SyncWeatherForMe", true);
            await BaseScript.Delay(2000);
            if (Screen.LoadingPrompt.IsActive) Screen.LoadingPrompt.Hide();

            if (PlayerCache.MyPlayer.User.FreeRoamChar.Posizione is null)
            {
                PlayerCache.MyPlayer.Ped.Position = new Vector3(0, 0, 0);
            }
            else
            {
                RequestCollisionAtCoord(PlayerCache.MyPlayer.User.FreeRoamChar.Posizione.X, PlayerCache.MyPlayer.User.FreeRoamChar.Posizione.Y, PlayerCache.MyPlayer.User.FreeRoamChar.Posizione.Z);
                PlayerCache.MyPlayer.Ped.Position = (await PlayerCache.MyPlayer.User.FreeRoamChar.Posizione.GetPositionWithGroundZ()).ToVector3;
            }

            // CARICAMENTO PROPRIETA'
            // CARICAMENTO VEICOLI

            SwitchInPlayer(PlayerCache.MyPlayer.Ped.Handle);
            while (IsPlayerSwitchInProgress()) await BaseScript.Delay(0);
            if (!PlayerCache.MyPlayer.Ped.IsVisible) NetworkFadeInEntity(PlayerCache.MyPlayer.Ped.Handle, true);
            PlayerCache.MyPlayer.Ped.IsPositionFrozen = false;
            PlayerCache.MyPlayer.Player.CanControlCharacter = true;
            Client.Instance.Events.Send("worldEventsManage.Server:AddParticipant");
            AccessingEvents.FreeRoamSpawn(PlayerCache.MyPlayer);
            PlayerCache.MyPlayer.Status.PlayerStates.Spawned = true;
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
