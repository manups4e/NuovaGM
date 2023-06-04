﻿using Impostazioni.Shared.Configurazione.Generici;
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

        [JsonIgnore] internal string FullName => CurrentChar.Info.firstname + " " + CurrentChar.Info.lastname;

        [JsonIgnore] internal string DoB => CurrentChar.Info.dateOfBirth;

        [JsonIgnore] internal bool DeathStatus => PlayerCache.ModalitàAttuale == ModalitaServer.Roleplay ? CurrentChar.is_dead : FreeRoamChar.is_dead;

        [JsonIgnore] internal int Money => PlayerCache.ModalitàAttuale == ModalitaServer.Roleplay ? CurrentChar.Finance.Money : FreeRoamChar.Finance.Money;

        [JsonIgnore] internal int Bank => PlayerCache.ModalitàAttuale == ModalitaServer.Roleplay ? CurrentChar.Finance.Bank : FreeRoamChar.Finance.Bank;

        [JsonIgnore] internal int DirtyCash => CurrentChar.Finance.DirtyCash;

        [JsonIgnore] internal List<Inventory> Inventory => GetCharInventory();

        public Tuple<bool, Inventory, Item> GetInventoryItem(string item)
        {
            foreach (Inventory t in CurrentChar.Inventory.Where(t => t.Item == item)) return new Tuple<bool, Inventory, Item>(true, t, ConfigShared.SharedConfig.Main.Generici.ItemList[item]);
            return new Tuple<bool, Inventory, Item>(false, null, null);
        }

        public List<Inventory> GetCharInventory()
        {
            return CurrentChar.Inventory;
        }

        public List<Weapons> GetCharWeapons()
        {
            return PlayerCache.ModalitàAttuale == ModalitaServer.Roleplay ? CurrentChar.Weapons : FreeRoamChar.Weapons;
        }

        public bool HasWeapon(string weaponName)
        {
            return PlayerCache.ModalitàAttuale == ModalitaServer.Roleplay ? CurrentChar.Weapons.Any(x => x.name == weaponName) : FreeRoamChar.Weapons.Any(x => x.name == weaponName);
        }

        public bool HasWeapon(WeaponHash weaponName)
        {
            return PlayerCache.ModalitàAttuale == ModalitaServer.Roleplay ? CurrentChar.Weapons.Any(x => Funzioni.HashInt(x.name) == (int)weaponName) : FreeRoamChar.Weapons.Any(x => Funzioni.HashInt(x.name) == (int)weaponName);
        }

        public Tuple<int, Weapons> GetWeapon(string weaponName)
        {
            if (PlayerCache.ModalitàAttuale == ModalitaServer.Roleplay)
            {
                for (int i = 0; i < CurrentChar.Weapons.Count; i++)
                    if (CurrentChar.Weapons[i].name == weaponName)
                        return new Tuple<int, Weapons>(i, CurrentChar.Weapons[i]);
            }
            else
            {
                for (int i = 0; i < FreeRoamChar.Weapons.Count; i++)
                    if (FreeRoamChar.Weapons[i].name == weaponName)
                        return new Tuple<int, Weapons>(i, FreeRoamChar.Weapons[i]);
            }

            return new Tuple<int, Weapons>(0, null);
        }

        public bool HasWeaponTint(string weaponName, int tint)
        {
            Weapons weapon = GetWeapon(weaponName).Item2;

            return weapon != null && weapon.tint == tint;
        }

        public bool HasWeaponComponent(string weaponName, string weaponComponent)
        {
            Weapons weapon = GetWeapon(weaponName).Item2;

            return weapon != null && weapon.components.Any(x => x.name == weaponComponent);
        }

        public bool HasLicense(string license)
        {
            return CurrentChar.Licenze.Any(x => x.name == license);
        }
    }
}