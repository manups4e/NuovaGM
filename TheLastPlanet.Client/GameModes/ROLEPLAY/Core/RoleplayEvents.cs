using System;
using TheLastPlanet.Client.GameMode.ROLEPLAY.Shops;


namespace TheLastPlanet.Client.GameMode.ROLEPLAY.Core
{
    static class RoleplayEvents
    {
        public static async void Init()
        {
            AccessingEvents.OnRoleplaySpawn += Spawned;
            AccessingEvents.OnRoleplayLeave += onPlayerLeft;
        }
        public static void Spawned(PlayerClient client)
        {
            EventDispatcher.Mount("lprp:possiediArma", new Action<string, string>(getWeapon));
            EventDispatcher.Mount("lprp:possiediTinta", new Action<string, int>(getTint));
            EventDispatcher.Mount("lprp:removeWeaponComponent", new Action<string, string>(RemoveWeaponComponent));
            EventDispatcher.Mount("lprp:removeWeapon", new Action<string>(RemoveWeapon));
            EventDispatcher.Mount("lprp:addWeapon", new Action<string, int>(AddWeapon));
            EventDispatcher.Mount("lprp:addWeaponComponent", new Action<string, string>(AddWeaponComponent));
            EventDispatcher.Mount("lprp:addWeaponTint", new Action<string, int>(AddWeaponTint));
            EventDispatcher.Mount("lprp:riceviOggettoAnimazione", new Action(AnimationReceiveObject));
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

        private static void AnimationReceiveObject()
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
            HUD.ShowNotification("Removed/a ~y~" + Functions.GetWeaponLabel((uint)weaponHash));
        }

        public static void getWeapon(string weaponName, string componentName)
        {
            HUD.ShowNotification("Modification already obtained: ~y~" + Functions.GetWeaponLabel(Functions.HashUint(componentName)) + "~w~ for ~b~" + Functions.GetWeaponLabel(Functions.HashUint(weaponName)) + "~w~.");
        }

        public static void getTint(string weaponName, int tinta)
        {
            HUD.ShowNotification("Modification already obtained: ~y~" + Functions.GetWeaponLabel(Functions.HashUint(WeaponShops.tints[tinta].name)) + "~w~ for ~b~" + Functions.GetWeaponLabel(Functions.HashUint(weaponName)) + "~w~.");
        }

        public static void AddWeaponComponent(string weaponName, string weaponComponent)
        {
            uint weaponHash = Functions.HashUint(weaponName);
            uint componentHash = Functions.HashUint(weaponComponent);

            if (!Cache.PlayerCache.MyPlayer.User.HasWeaponComponent(weaponName, weaponComponent))
            {
                GiveWeaponComponentToPed(PlayerPedId(), weaponHash, componentHash);
                HUD.ShowNotification("you got 1 ~b~" + Functions.GetWeaponLabel(componentHash));
            }
            else
            {
                HUD.ShowNotification("This weapon already has a " + Functions.GetWeaponLabel(componentHash));
            }
        }

        public static void RemoveWeaponComponent(string weaponName, string weaponComponent)
        {
            uint weaponHash = Functions.HashUint(weaponName);
            uint componentHash = Functions.HashUint(weaponComponent);
            RemoveWeaponComponentFromPed(PlayerPedId(), weaponHash, componentHash);
            HUD.ShowNotification("Removed/a ~b~" + Functions.GetWeaponLabel(componentHash));
        }

        public static void AddWeaponTint(string weaponName, int tint)
        {
            uint weaponHash = Functions.HashUint(weaponName);
            SetPedWeaponTintIndex(PlayerPedId(), weaponHash, tint);
        }
    }
}
