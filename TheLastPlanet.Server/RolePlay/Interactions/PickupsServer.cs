using Settings.Shared.Config.Generic;
using System;
using System.Collections.Generic;
using TheLastPlanet.Server.Core;
using TheLastPlanet.Server.Core.PlayerChar;

namespace TheLastPlanet.Server.Interactions
{
    internal class PickupsServer
    {
        public static List<PickupObject> Pickups = new List<PickupObject>();

        public static void Init()
        {
            Server.Instance.AddEventHandler("lprp:removeInventoryItemWithPickup", new Action<Player, string, int>(RemoveInventoryItemWithPickup));
            Server.Instance.AddEventHandler("lprp:removeWeaponWithPickup", new Action<Player, string>(RemoveWeaponWithPickup));
            Server.Instance.AddEventHandler("lprp:removeAccountWithPickup", new Action<Player, string, int>(RemoveAccountWithPickup));
            Server.Instance.AddEventHandler("lprp:onPickup", new Action<Player, int>(OnPickup));
        }

        public static void CreatePickup(Inventory oggetto, int count, string label, PlayerClient user)
        {
            PickupObject pickup = new PickupObject(Pickups.Count, oggetto.Item, count, ConfigShared.SharedConfig.Main.Generics.ItemList[oggetto.Item].prop, 0, label, user.Ped.Position.ToPosition());
            Pickups.Add(pickup);
            BaseScript.TriggerClientEvent("lprp:createPickup", pickup.ToJson(), user.Player.Handle);
        }

        public static void CreatePickup(Weapons oggetto, string label, PlayerClient user)
        {
            PickupObject arma = new PickupObject(Pickups.Count, oggetto.Name, oggetto.Ammo, 0, 0, label, user.Ped.Position.ToPosition(), "weapon", oggetto.Components, oggetto.Tint);
            Pickups.Add(arma);
            BaseScript.TriggerClientEvent("lprp:createPickup", arma.ToJson(), user.Player.Handle);
        }

        public static void CreatePickup(string name, int count, string label, PlayerClient user)
        {
            ObjectHash oggetto = 0;

            switch (count)
            {
                case int a when a < 101:
                    oggetto = ObjectHash.prop_cash_pile_01;

                    break;
                case int a when a > 100 && a < 501:
                    oggetto = ObjectHash.prop_anim_cash_pile_02;

                    break;
                case int a when a > 500 && a < 1001:
                    oggetto = ObjectHash.prop_cash_case_01;

                    break;
                case int a when a > 1000 && a < 3001:
                    oggetto = ObjectHash.prop_cash_case_02;

                    break;
                case int a when a > 3000:
                    oggetto = ObjectHash.prop_cash_dep_bag_01;

                    break;
            }

            PickupObject soldo = new PickupObject(Pickups.Count, name, count, oggetto, 0, label, user.Ped.Position.ToPosition(), "account");
            Pickups.Add(soldo);
            BaseScript.TriggerClientEvent("lprp:createPickup", soldo.ToJson(), user.Player.Handle);
        }

        private static void RemoveInventoryItemWithPickup([FromSource] Player player, string item, int count)
        {
            PlayerClient client = Functions.GetClientFromPlayerId(player.Handle);
            User user = client?.User ?? player.GetCurrentChar();
            Tuple<bool, Inventory> oggetto = user.getInventoryItem(item);

            if (oggetto.Item1)
            {
                if (oggetto.Item2.Amount > 0)
                {
                    user.removeInventoryItem(item, count);
                    string label = $"{oggetto.Item2.Item} [{count}]";
                    CreatePickup(oggetto.Item2, count, label, client);
                }
                else
                {
                    user.showNotification("No items in your inventory!");
                }
            }
        }

        private static void RemoveWeaponWithPickup([FromSource] Player player, string weapon)
        {
            PlayerClient client = Functions.GetClientFromPlayerId(player.Handle);
            User user = client?.User ?? player.GetCurrentChar();

            if (user.hasWeapon(weapon))
            {
                Tuple<int, Weapons> arma = user.getWeapon(weapon);
                user.removeWeapon(weapon);
                string label = Functions.GetWeaponLabel((uint)GetHashKey(weapon));
                CreatePickup(arma.Item2, label, client);
            }
        }

        private static void RemoveAccountWithPickup([FromSource] Player player, string name, int amount)
        {
            string label = "";
            PlayerClient client = Functions.GetClientFromPlayerId(player.Handle);
            User user = client?.User ?? player.GetCurrentChar();

            switch (name)
            {
                case "cash":
                    user.Money -= amount;
                    label = $"Cash [{amount}]";

                    break;
                case "dirty_money":
                    user.DirtCash -= amount;
                    label = $"Dirty cash [{amount}]";

                    break;
            }

            CreatePickup(name, amount, label, client);
        }

        private static void OnPickup([FromSource] Player source, int id)
        {
            User user = source.GetCurrentChar();
            PickupObject pickup = Pickups[id];
            bool success = false;

            switch (pickup.Type)
            {
                case "item":
                    //aggiungere controllo se può portarlo
                    user.addInventoryItem(pickup.Name, pickup.Amount, ConfigShared.SharedConfig.Main.Generics.ItemList[pickup.Name].weight);
                    success = true;

                    break;
                case "weapon":
                    if (user.hasWeapon(pickup.Name))
                    {
                        user.showNotification("You already have this weapon!");
                    }
                    else
                    {
                        success = true;
                        user.addWeapon(pickup.Name, pickup.Amount);
                        if (pickup.TintIndex != 0) user.addWeaponTint(pickup.Name, pickup.TintIndex);
                        foreach (Components comp in pickup.Components) user.addWeaponComponent(pickup.Name, comp.name);
                    }

                    break;
                case "account":
                    success = true;
                    if (pickup.Name == "cash")
                        user.Money += pickup.Amount;
                    else if (pickup.Name == "dirty_cash") user.DirtCash += pickup.Amount;

                    break;
            }

            if (success)
            {
                Pickups[id] = null;
                BaseScript.TriggerClientEvent("lprp:removePickup", id);
            }
        }
    }
}