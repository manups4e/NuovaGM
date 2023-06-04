using System;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Negozi;


namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Core
{
    static class EventiRoleplay
    {
        public static async void Init()
        {
            AccessingEvents.OnRoleplaySpawn += Spawnato;
            AccessingEvents.OnRoleplayLeave += onPlayerLeft;
        }
        public static void Spawnato(PlayerClient client)
        {
            EventDispatcher.Mount("lprp:possiediArma", new Action<string, string>(PossiediArma));
            EventDispatcher.Mount("lprp:possiediTinta", new Action<string, int>(PossiediTinta));
            EventDispatcher.Mount("lprp:removeWeaponComponent", new Action<string, string>(RemoveWeaponComponent));
            EventDispatcher.Mount("lprp:removeWeapon", new Action<string>(RemoveWeapon));
            EventDispatcher.Mount("lprp:addWeapon", new Action<string, int>(AddWeapon));
            EventDispatcher.Mount("lprp:addWeaponComponent", new Action<string, string>(AddWeaponComponent));
            EventDispatcher.Mount("lprp:addWeaponTint", new Action<string, int>(AddWeaponTint));
            EventDispatcher.Mount("lprp:riceviOggettoAnimazione", new Action(AnimazioneRiceviOggetto));
        }
        public static void onPlayerLeft(PlayerClient client)
        {
            EventDispatcher.Unmount("lprp:possiediArma");
            EventDispatcher.Unmount("lprp:possiediTinta");
            EventDispatcher.Unmount("lprp:removeWeaponComponent");
            EventDispatcher.Unmount("lprp:removeWeapon");
            EventDispatcher.Unmount("lprp:addWeapon");
            EventDispatcher.Unmount("lprp:addWeaponComponent");
            EventDispatcher.Unmount("lprp:addWeaponTint");
            EventDispatcher.Unmount("lprp:riceviOggettoAnimazione");
        }

        private static void AnimazioneRiceviOggetto()
        {
            Cache.PlayerCache.MyPlayer.Ped.Task.PlayAnimation("mp_common", "givetake2_a");
        }

        public static void AddWeapon(string weaponName, int ammo)
        {
        }

        public static void RemoveWeapon(string weaponName)
        {
            WeaponHash weaponHash = (WeaponHash)GetHashKey(weaponName);
            RemoveWeaponFromPed(PlayerPedId(), (uint)weaponHash);
            SetPedAmmo(PlayerPedId(), (uint)weaponHash, 0);
            HUD.ShowNotification("Rimosso/a ~y~" + Funzioni.GetWeaponLabel((uint)weaponHash));
        }

        public static void PossiediArma(string weaponName, string componentName)
        {
            HUD.ShowNotification("Possiedi già la modifica: ~y~" + Funzioni.GetWeaponLabel(Funzioni.HashUint(componentName)) + "~w~ per ~b~" + Funzioni.GetWeaponLabel(Funzioni.HashUint(weaponName)) + "~w~.");
        }

        public static void PossiediTinta(string weaponName, int tinta)
        {
            HUD.ShowNotification("Possiedi già la modifica: ~y~" + Funzioni.GetWeaponLabel(Funzioni.HashUint(Armerie.tinte[tinta].name)) + "~w~ per ~b~" + Funzioni.GetWeaponLabel(Funzioni.HashUint(weaponName)) + "~w~.");
        }

        public static void AddWeaponComponent(string weaponName, string weaponComponent)
        {
            uint weaponHash = Funzioni.HashUint(weaponName);
            uint componentHash = Funzioni.HashUint(weaponComponent);

            if (!Cache.PlayerCache.MyPlayer.User.HasWeaponComponent(weaponName, weaponComponent))
            {
                GiveWeaponComponentToPed(PlayerPedId(), weaponHash, componentHash);
                HUD.ShowNotification("Hai ottenuto un ~b~" + Funzioni.GetWeaponLabel(componentHash));
            }
            else
            {
                HUD.ShowNotification("Quest'arma ha già un" + Funzioni.GetWeaponLabel(componentHash));
            }
        }

        public static void RemoveWeaponComponent(string weaponName, string weaponComponent)
        {
            uint weaponHash = Funzioni.HashUint(weaponName);
            uint componentHash = Funzioni.HashUint(weaponComponent);
            RemoveWeaponComponentFromPed(PlayerPedId(), weaponHash, componentHash);
            HUD.ShowNotification("Rimosso/a ~b~" + Funzioni.GetWeaponLabel(componentHash));
        }

        public static void AddWeaponTint(string weaponName, int tint)
        {
            uint weaponHash = Funzioni.HashUint(weaponName);
            SetPedWeaponTintIndex(PlayerPedId(), weaponHash, tint);
        }
    }
}
