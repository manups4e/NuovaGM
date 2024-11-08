﻿using Newtonsoft.Json;
using Settings.Shared.Config.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using TheLastPlanet.Shared.PlayerChar;
using TheLastPlanet.Shared.Vehicles;

namespace TheLastPlanet.Server.Core.PlayerChar
{
    public class User : BasePlayerShared
    {
        [JsonIgnore] internal string source;
        [JsonIgnore] internal DateTime LastSaved;
        [JsonIgnore] internal BasePlayerShared basePlayer;

        public User() { }
        public User(Player player, BasePlayerShared result)
        {
            basePlayer = result;
            Name = player.Name;
            source = player.Handle;
            ID = result.ID;
            PlayerID = result.PlayerID;
            group = result.group;
            group_level = result.group_level;
            playTime = result.playTime;
            char_data = result.char_data;
            LastSaved = DateTime.Now;
            Player = player;
            Identifiers = new()
            {
                Steam = player.GetLicense(Identifier.Steam),
                License = player.GetLicense(Identifier.License),
                Discord = player.GetLicense(Identifier.Discord),
                Fivem = player.GetLicense(Identifier.Fivem),
                Ip = player.GetLicense(Identifier.Ip),
            };
        }

        public User(Player player, dynamic result)
        {
            source = player.Handle;
            ID = result.UserID;
            group = result.group;
            group_level = (UserGroup)result.group_level;
            playTime = result.playTime;
            Characters = (result.char_data as string).FromJson<List<Char_data>>();
            LastSaved = DateTime.Now;
        }

        public User(dynamic result)
        {
            //source = player.Handle;
            group = result.group;
            group_level = (UserGroup)result.group_level;
            playTime = result.playTime;
            //p = player;
            Characters = (result.char_data as string).FromJson<List<Char_data>>();
            LastSaved = DateTime.Now;
        }

        [JsonIgnore] internal string FullName => CurrentChar.Info.Firstname + " " + CurrentChar.Info.Lastname;

        [JsonIgnore] internal string DOB => CurrentChar.Info.DateOfBirth;


        [JsonIgnore]
        internal bool DeathStatus
        {
            get => CurrentChar.Is_dead;
            set
            {
                CurrentChar.Is_dead = value;
                //Status.RolePlayStates.Svenuto = true;
            }
        }


        [JsonIgnore]
        internal int Money
        {
            get => CurrentChar.Finance.Money;
            set
            {
                int var = value - CurrentChar.Finance.Money;
                CurrentChar.Finance.Money += var;
                if (var < 0)
                    if (CurrentChar.Finance.Money < 0)
                        CurrentChar.Finance.Money = 0;
                Player.TriggerEvent("lprp:changeMoney", var);
            }
        }


        [JsonIgnore]
        internal int Bank
        {
            get => CurrentChar.Finance.Bank;
            set
            {
                int var = value - CurrentChar.Finance.Bank;
                CurrentChar.Finance.Bank += var;
                if (var < 0) Player.TriggerEvent("lprp:rimuoviBank", var);
            }
        }


        [JsonIgnore]
        internal int DirtCash
        {
            get => CurrentChar.Finance.DirtyCash;
            set
            {
                int var = value - CurrentChar.Finance.DirtyCash;
                CurrentChar.Finance.DirtyCash += var;
                if (var < 0)
                    if (CurrentChar.Finance.DirtyCash < 0)
                        CurrentChar.Finance.DirtyCash = 0;
                Player.TriggerEvent("lprp:changeDirty", var);
            }
        }

        public void SetJob(string job, int grade)
        {
            CurrentChar.Job.Name = job;
            CurrentChar.Job.Grade = grade;
        }

        public void SetGang(string job, int grade)
        {
            CurrentChar.Gang.Name = job;
            CurrentChar.Gang.Grade = grade;
        }

        public Tuple<bool, Inventory> getInventoryItem(string item)
        {
            for (int i = 0; i < CurrentChar.Inventory.Count; i++)
                if (CurrentChar.Inventory[i].Item == item)
                    return new Tuple<bool, Inventory>(true, CurrentChar.Inventory[i]);

            return new Tuple<bool, Inventory>(false, null);
        }

        public List<Inventory> getCharInventory(uint charId)
        {
            for (int i = 0; i < Characters.Count; i++)
                if (Characters[i].CharID == charId)
                    return Characters[i].Inventory;

            return null;
        }

        public void addInventoryItem(string item, int amount, float weight)
        {
            bool vero = getInventoryItem(item).Item1;
            Inventory checkedItem = getInventoryItem(item).Item2;

            if (vero)
            {
                checkedItem.Amount += amount;

                if (checkedItem.Amount == ConfigShared.SharedConfig.Main.Generics.ItemList[item].max)
                {
                    checkedItem.Amount = ConfigShared.SharedConfig.Main.Generics.ItemList[item].max;
                    Player.TriggerEvent("tlg:ShowNotification", "YOU ALREADY HAVE THE MOST OF ~w~" + ConfigShared.SharedConfig.Main.Generics.ItemList[item].label + "~w~!");
                }
            }
            else
            {
                CurrentChar.Inventory.Add(new Inventory(item, amount, weight));
            }

            Player.TriggerEvent("tlg:ShowNotification", "Received " + amount + " " + ConfigShared.SharedConfig.Main.Generics.ItemList[item].label + "!");
        }

        public void removeInventoryItem(string item, int amount)
        {
            bool vero = getInventoryItem(item).Item1;
            Inventory checkedItem = getInventoryItem(item).Item2;

            if (vero)
            {
                checkedItem.Amount -= amount;
                if (checkedItem.Amount <= 0) CurrentChar.Inventory.Remove(checkedItem);
            }
            else
            {
                CurrentChar.Inventory.ToList().Remove(checkedItem);
            }

            Player.TriggerEvent("tlg:ShowNotification", amount + " " + ConfigShared.SharedConfig.Main.Generics.ItemList[item].label + " have been removed!");
        }

        public List<Weapons> getCharWeapons(uint charId)
        {
            for (int i = 0; i < Characters.Count; i++)
                if (Characters[i].CharID == charId)
                    return Characters[i].Weapons;

            return null;
        }

        public void addWeapon(string weaponName, int ammo)
        {
            if (!hasWeapon(weaponName))
            {
                CurrentChar.Weapons.Add(new Weapons(weaponName, ammo, new List<Components>(), 0));
                Player.TriggerSubsystemEvent("lprp:addWeapon", weaponName, ammo);
            }
        }

        public void updateWeaponAmmo(string weaponName, int ammo)
        {
            Tuple<int, Weapons> weapon = getWeapon(weaponName);
            if (weapon.Item2.Ammo > ammo) CurrentChar.Weapons[weapon.Item1].Ammo = ammo;
        }

        public void removeWeapon(string weaponName)
        {
            if (hasWeapon(weaponName))
            {
                CurrentChar.Weapons.Remove(getWeapon(weaponName).Item2);
                Player.TriggerSubsystemEvent("lprp:removeWeapon", weaponName);

            }
        }

        public void addWeaponComponent(string weaponName, string weaponComponent)
        {
            int num = getWeapon(weaponName).Item1;

            if (hasWeaponComponent(weaponName, weaponComponent))
            {
                Player.TriggerSubsystemEvent("lprp:possiediArma", weaponName, weaponComponent);
            }
            else
            {
                CurrentChar.Weapons[num].Components.Add(new Components(weaponComponent, true));
                Player.TriggerSubsystemEvent("lprp:addWeaponComponent", weaponName, weaponComponent);

            }
        }

        public void removeWeaponComponent(string weaponName, string weaponComponent)
        {
            int num = getWeapon(weaponName).Item1;
            Weapons weapon = getWeapon(weaponName).Item2;

            if (weapon != null)
                for (int i = 0; i < CurrentChar.Weapons[num].Components.Count; i++)
                    if (CurrentChar.Weapons[num].Components[i].name == weaponComponent)
                    {
                        CurrentChar.Weapons[num].Components.RemoveAt(i);
                        Player.TriggerSubsystemEvent("lprp:removeWeaponComponent", weaponName, weaponComponent);

                    }
        }

        public void addWeaponTint(string weaponName, int tint)
        {
            int num = getWeapon(weaponName).Item1;
            Weapons weapon = getWeapon(weaponName).Item2;

            if (weapon != null)
            {
                if (hasWeaponTint(weaponName, tint))
                {
                    Player.TriggerSubsystemEvent("lprp:possiediTinta", weaponName, tint);
                }
                else
                {
                    CurrentChar.Weapons[num].Tint = tint;
                    Player.TriggerSubsystemEvent("lprp:addWeaponTint", weaponName, tint);

                }
            }
        }

        public bool hasWeapon(string weaponName)
        {
            return CurrentChar.Weapons.Any(x => x.Name == weaponName);
        }

        public Tuple<int, Weapons> getWeapon(string weaponName)
        {
            Weapons weapon = CurrentChar.Weapons.FirstOrDefault(x => x.Name == weaponName);

            return weapon != null ? new Tuple<int, Weapons>(CurrentChar.Weapons.IndexOf(weapon), weapon) : new Tuple<int, Weapons>(0, null);
        }

        public bool hasWeaponTint(string weaponName, int tint)
        {
            Weapons weapon = getWeapon(weaponName).Item2;

            return weapon != null && weapon.Tint == tint;
        }

        public bool hasWeaponComponent(string weaponName, string weaponComponent)
        {
            Weapons weapon = getWeapon(weaponName).Item2;

            return weapon != null && weapon.Components.Any(x => x.name == weaponComponent);
        }

        [JsonIgnore] internal Vector3 getCoords => CurrentChar.Position.ToVector3;

        public void giveLicense(string license, string mittente)
        {
            Licenses licenza = new(license, DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss"), mittente);
            CurrentChar.Licenses.Add(licenza);
        }

        public void removeLicense(string license)
        {
            foreach (Licenses licen in CurrentChar.Licenses)
                if (licen.Name == license)
                    CurrentChar.Licenses.Remove(licen);
                else
                    Server.Logger.Warning($"Player {Player.Name} It does not have a named license '{license}'");
        }

        public List<OwnedVehicle> GetCharVehicles()
        {
            return CurrentChar.Vehicles;
        }

        public void AddExperience(int experiencePoints)
        {
            int nextLevelTotalXp = Experience.NextLevelExperiencePoints(FreeRoamChar.Level);

            if (FreeRoamChar.TotalXp + experiencePoints >= nextLevelTotalXp)
            {
                int remainder = FreeRoamChar.TotalXp + experiencePoints - nextLevelTotalXp;
                FreeRoamChar.Level++;
                if (remainder > 0)
                    AddExperience(remainder);
            }
            else
            {
                FreeRoamChar.TotalXp += experiencePoints;
            }
        }

        public void UpdateCurrentAttempt(int eventId, float currentAttempt)
        {
            try
            {
                PlayerScore data = PlayerScores.Where(x => x.EventId == eventId).FirstOrDefault();
                if (data != null)
                {
                    data.CurrentAttempt = currentAttempt;
                    if (currentAttempt > data.BestAttempt)
                        data.BestAttempt = currentAttempt;
                }
                else
                    Server.Logger.Warning($"Data for Event {eventId} does not exist for Player {Player.Name}");
            }
            catch (Exception e)
            {
                Server.Logger.Error(e.ToString());
            }
        }

        public void showNotification(string text)
        {
            Player.TriggerSubsystemEvent("tlg:ShowNotification", text);
        }
    }
}