using Impostazioni.Shared.Configurazione.Generici;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Core;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Core.Status;

namespace TheLastPlanet.Client.Core.Utility
{
    internal static class Eventi
    {
        private static int timer = 0;

        public static void Init()
        {
            EventDispatcher.Mount("lprp:teleportCoords", new Action<Position>(teleportCoords));
            //EventDispatcher.Mount("lprp:onPlayerDeath", new Action<dynamic>(onPlayerDeath));
            EventDispatcher.Mount("lprp:sendUserInfo", new Action<string, string>(sendUserInfo));
            EventDispatcher.Mount("lprp:ObjectDeleteGun", new Action<string>(DelGun));
            EventDispatcher.Mount("tlg:ShowNotification", new Action<string>(notification));
            EventDispatcher.Mount("lpop:ShowNotification", new Action<string>(notification));
            EventDispatcher.Mount("lprp:death", new Action(death));
            EventDispatcher.Mount("lprp:announce", new Action<string>(announce));
            EventDispatcher.Mount("lprp:reviveChar", new Action(Revive));
            EventDispatcher.Mount("lprp:spawnVehicle", new Action<string>(SpawnVehicle));
            EventDispatcher.Mount("lprp:deleteVehicle", new Action(DeleteVehicle));
            EventDispatcher.Mount("lprp:mostrasalvataggio", new Action(Salva));
            EventDispatcher.Mount("tlg:onPlayerEntrance", new Action<PlayerClient>(PlayerJoined));
            timer = GetGameTimer();
            AccessingEvents.OnFreeRoamSpawn += OnSpawn;
            AccessingEvents.OnRoleplaySpawn += OnSpawn;
        }

        private static void OnSpawn(PlayerClient client)
        {
            AggiornaPlayers();
        }

        public static void PlayerJoined(PlayerClient client)
        {
            if (client.Status == null)
                client.Status = new(client.Player);
            Client.Instance.Clients.Add(client);
        }

        public static async void AggiornaPlayers()
        {
            Client.Instance.Clients = await EventDispatcher.Get<List<PlayerClient>>("tlg:callPlayers", PlayerCache.MyPlayer.Posizione);
            foreach (var client in Client.Instance.Clients) client.Status = new(client.Player);
        }

        public static async void LoadModel()
        {
            uint hash = PlayerCache.MyPlayer.User.CurrentChar.Skin.model;
            RequestModel(hash);
            while (!HasModelLoaded(hash)) await BaseScript.Delay(1);
            SetPlayerModel(PlayerId(), hash);
            Funzioni.UpdateFace(PlayerCache.MyPlayer.Ped.Handle, PlayerCache.MyPlayer.User.CurrentChar.Skin);
            Funzioni.UpdateDress(PlayerCache.MyPlayer.Ped.Handle, PlayerCache.MyPlayer.User.CurrentChar.Dressing);
            // TODO: Cambiare con request
            RestoreWeapons();
        }

        public static async void teleportCoords(Position pos)
        {
            Screen.Fading.FadeOut(500);
            await BaseScript.Delay(1000);
            StartPlayerTeleport(PlayerId(), pos.X, pos.Y, pos.Z, 0, true, true, true);
            while (!HasPlayerTeleportFinished(PlayerId())) await BaseScript.Delay(0);
            await BaseScript.Delay(2000);
            Screen.Fading.FadeIn(500);
            //Funzioni.Teleport(pos);
        }

        public static void sendUserInfo(string _char_data, string _group)
        {
            PlayerCache.MyPlayer.User.char_data = _char_data;
            PlayerCache.MyPlayer.User.group = _group;
        }

        public static bool On = false;

        public static void DelGun(string toggle)
        {
            switch (toggle)
            {
                case "on" when !On:
                    HUD.HUD.ShowNotification("DelGun Attivata!", HUD.ColoreNotifica.GreenLight);
                    On = true;
                    break;
                case "on" when On:
                    HUD.HUD.ShowNotification("~y~DelGun già attivata!", HUD.ColoreNotifica.Yellow);
                    break;
                case "off" when On:
                    HUD.HUD.ShowNotification("~b~DelGun Disattivata!", HUD.ColoreNotifica.GreenLight);
                    On = false;
                    break;
                case "off" when !On:
                    HUD.HUD.ShowNotification("~y~DelGun già Disattivata!", HUD.ColoreNotifica.Yellow);
                    break;
            }
        }

        public static void notification(string text)
        {
            HUD.HUD.ShowNotification(text);
        }

        public static void advancedNotification(string title, string subject, string msg, string icon, HUD.TipoIcona iconType)
        {
            HUD.HUD.ShowAdvancedNotification(title, subject, msg, icon, iconType);
        }

        public static void death()
        {
            PlayerCache.MyPlayer.Ped.Kill();
        }

        public static async void announce(string msg)
        {
            Game.PlaySound("DELETE", "HUD_DEATHMATCH_SOUNDSET");
            ScaleformUI.Main.BigMessageInstance.ShowSimpleShard("~r~ANNUNCIO AI GIOCATORI", msg);
        }

        public static async void Revive()
        {
            Screen.Fading.FadeOut(800);
            while (Screen.Fading.IsFadingOut) await BaseScript.Delay(50);
            Main.RespawnPed(PlayerCache.MyPlayer.Posizione);
            if (PlayerCache.MyPlayer.Status.PlayerStates.Modalita == ModalitaServer.Roleplay)
            {
                StatsNeeds.Needs["Fame"].Val = 0.0f;
                StatsNeeds.Needs["Sete"].Val = 0.0f;
                StatsNeeds.Needs["Stanchezza"].Val = 0.0f;
                PlayerCache.MyPlayer.User.CurrentChar.Needs.Malattia = false;
                Needs nee = new() { Fame = StatsNeeds.Needs["Fame"].Val, Sete = StatsNeeds.Needs["Sete"].Val, Stanchezza = StatsNeeds.Needs["Stanchezza"].Val, Malattia = PlayerCache.MyPlayer.User.CurrentChar.Needs.Malattia };
                PlayerCache.MyPlayer.User.CurrentChar.Needs = nee;
                PlayerCache.MyPlayer.User.CurrentChar.is_dead = false;
                BaseScript.TriggerServerEvent("lprp:medici:rimuoviDaMorti");
                PlayerCache.MyPlayer.Status.RolePlayStates.FinDiVita = false;
                Death.endConteggio();
            }
            //BaseScript.TriggerServerEvent("lprp:updateCurChar", "needs", nee.ToJson());
            //BaseScript.TriggerServerEvent("lprp:setDeathStatus", false);
            Screen.Effects.Stop(ScreenEffect.DeathFailOut);
            Screen.Fading.FadeIn(800);
        }

        public static async void SpawnVehicle(string model)
        {
            Vector3 coords = PlayerCache.MyPlayer.Posizione.ToVector3;
            Vehicle Veh = await Funzioni.SpawnVehicle(model, coords, PlayerCache.MyPlayer.Ped.Heading);
            Veh.PreviouslyOwnedByPlayer = true;
            Veh.FadeEntity(false);
            await Veh.FadeEntityAsync(true);
        }

        public static void DeleteVehicle()
        {
            Entity vehicle = new Vehicle(Funzioni.GetVehicleInDirection());
            if (PlayerCache.MyPlayer.Ped.IsInVehicle()) vehicle = PlayerCache.MyPlayer.Ped.CurrentVehicle;
            if (vehicle.Exists()) DecorRemove(vehicle.Handle, Main.decorName);
            vehicle.Delete();
        }


        public static async void Salva()
        {
            Screen.LoadingPrompt.Show("Salvataggio Personaggio...", LoadingSpinnerType.SocialClubSaving);
            await BaseScript.Delay(5000);
            Screen.LoadingPrompt.Hide();
        }

        public static void RestoreWeapons()
        {
            Dictionary<int, bool> ammoTypes = new();

            if (PlayerCache.MyPlayer.User.CurrentChar.Weapons.Count > 0)
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
                    {
                        foreach (Components weaponComponent in weaps[i].components)
                        {
                            uint componentHash = Funzioni.HashUint(weaponComponent.name);
                            if (weaponComponent.active) GiveWeaponComponentToPed(PlayerPedId(), weaponHash, componentHash);
                        }
                    }

                    SetPedWeaponTintIndex(PlayerPedId(), weaponHash, tint);
                    if (ammoTypes.ContainsKey(ammoType)) continue;
                    AddAmmoToPed(PlayerPedId(), weaponHash, weaps[i].ammo);
                    ammoTypes[ammoType] = true;
                }
            }
            Main.LoadoutLoaded = true;
        }
    }
}