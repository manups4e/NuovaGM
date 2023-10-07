using Settings.Shared.Config.Generic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TheLastPlanet.Shared.PlayerChar;

namespace TheLastPlanet.Client.Core.PlayerChar
{
    public class User : BasePlayerShared
    {
        public User(BasePlayerShared result)
        {
            ID = result.ID;
            PlayerID = result.PlayerID;
            group = result.group;
            group_level = result.group_level;
            playTime = result.playTime;
            Player = Game.Player;
            char_data = result.char_data;
            Identifiers = result.Identifiers;
        }

        public User() { }

        [JsonIgnore] internal string FullName => CurrentChar.Info.Firstname + " " + CurrentChar.Info.Lastname;

        [JsonIgnore] internal string DoB => CurrentChar.Info.DateOfBirth;

        [JsonIgnore] internal bool DeathStatus => PlayerCache.ModalitàAttuale == ServerMode.Roleplay ? CurrentChar.Is_dead : FreeRoamChar.is_dead;

        [JsonIgnore] internal int Money => PlayerCache.ModalitàAttuale == ServerMode.Roleplay ? CurrentChar.Finance.Money : FreeRoamChar.Finance.Money;

        [JsonIgnore] internal int Bank => PlayerCache.ModalitàAttuale == ServerMode.Roleplay ? CurrentChar.Finance.Bank : FreeRoamChar.Finance.Bank;

        [JsonIgnore] internal int DirtyCash => CurrentChar.Finance.DirtyCash;

        [JsonIgnore] internal List<Inventory> Inventory => GetCharInventory();

        public Tuple<bool, Inventory, Item> GetInventoryItem(string item)
        {
            foreach (Inventory t in CurrentChar.Inventory.Where(t => t.Item == item)) return new Tuple<bool, Inventory, Item>(true, t, ConfigShared.SharedConfig.Main.Generics.ItemList[item]);
            return new Tuple<bool, Inventory, Item>(false, null, null);
        }

        public List<Inventory> GetCharInventory()
        {
            return CurrentChar.Inventory;
        }

        public List<Weapons> GetCharWeapons()
        {
            return PlayerCache.ModalitàAttuale == ServerMode.Roleplay ? CurrentChar.Weapons : FreeRoamChar.Weapons;
        }

        public bool HasWeapon(string weaponName)
        {
            return PlayerCache.ModalitàAttuale == ServerMode.Roleplay ? CurrentChar.Weapons.Any(x => x.Name == weaponName) : FreeRoamChar.Weapons.Any(x => x.Name == weaponName);
        }

        public bool HasWeapon(WeaponHash weaponName)
        {
            return PlayerCache.ModalitàAttuale == ServerMode.Roleplay ? CurrentChar.Weapons.Any(x => Funzioni.HashInt(x.Name) == (int)weaponName) : FreeRoamChar.Weapons.Any(x => Funzioni.HashInt(x.Name) == (int)weaponName);
        }

        public Tuple<int, Weapons> GetWeapon(string weaponName)
        {
            if (PlayerCache.ModalitàAttuale == ServerMode.Roleplay)
            {
                for (int i = 0; i < CurrentChar.Weapons.Count; i++)
                    if (CurrentChar.Weapons[i].Name == weaponName)
                        return new Tuple<int, Weapons>(i, CurrentChar.Weapons[i]);
            }
            else
            {
                for (int i = 0; i < FreeRoamChar.Weapons.Count; i++)
                    if (FreeRoamChar.Weapons[i].Name == weaponName)
                        return new Tuple<int, Weapons>(i, FreeRoamChar.Weapons[i]);
            }

            return new Tuple<int, Weapons>(0, null);
        }

        public bool HasWeaponTint(string weaponName, int tint)
        {
            Weapons weapon = GetWeapon(weaponName).Item2;

            return weapon != null && weapon.Tint == tint;
        }

        public bool HasWeaponComponent(string weaponName, string weaponComponent)
        {
            Weapons weapon = GetWeapon(weaponName).Item2;

            return weapon != null && weapon.Components.Any(x => x.name == weaponComponent);
        }

        public bool HasLicense(string license)
        {
            return CurrentChar.Licenses.Any(x => x.Name == license);
        }
    }
}