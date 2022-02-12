using System;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Negozi;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Core
{
    static class EventiRoleplay
    {
        public static void Init()
        {
            Client.Instance.Events.Mount("lprp:possiediArma", new Action<string, string>(PossiediArma));
            Client.Instance.Events.Mount("lprp:possiediTinta", new Action<string, int>(PossiediTinta));
            Client.Instance.Events.Mount("lprp:removeWeaponComponent", new Action<string, string>(RemoveWeaponComponent));
            Client.Instance.Events.Mount("lprp:removeWeapon", new Action<string>(RemoveWeapon));
            Client.Instance.Events.Mount("lprp:addWeapon", new Action<string, int>(AddWeapon));
            Client.Instance.Events.Mount("lprp:addWeaponComponent", new Action<string, string>(AddWeaponComponent));
            Client.Instance.Events.Mount("lprp:addWeaponTint", new Action<string, int>(AddWeaponTint));
            Client.Instance.Events.Mount("lprp:riceviOggettoAnimazione", new Action(AnimazioneRiceviOggetto));
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
