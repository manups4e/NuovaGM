using CitizenFX.Core;
using Impostazioni.Shared.Configurazione.Generici;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheLastPlanet.Server.Core;
using TheLastPlanet.Server.Core.Buckets;
using TheLastPlanet.Server.Core.PlayerChar;
using TheLastPlanet.Server.Interactions;
using TheLastPlanet.Shared;


namespace TheLastPlanet.Server.RolePlay.Core
{
    public static class EventiRolePlay
    {
        public static void Init()
        {
            EventDispatcher.Mount("tlg:roleplay:finishCharServer", new Action<PlayerClient, string>(FinishChar));
            EventDispatcher.Mount("tlg:roleplay:onPlayerSpawn", new Action<PlayerClient>(Spawnato));
            EventDispatcher.Mount("lprp:setDeathStatus", new Action<PlayerClient, bool>(deathStatus));
            EventDispatcher.Mount("lprp:payFine", new Action<PlayerClient, int>(PayFine));
            EventDispatcher.Mount("lprp:givemoney", new Action<PlayerClient, int>(GiveMoney));
            EventDispatcher.Mount("lprp:removemoney", new Action<PlayerClient, int>(RemoveMoney));
            EventDispatcher.Mount("lprp:givebank", new Action<PlayerClient, int>(GiveBank));
            EventDispatcher.Mount("lprp:removebank", new Action<PlayerClient, int>(RemoveBank));
            EventDispatcher.Mount("lprp:removedirty", new Action<PlayerClient, int>(RemoveDirty));
            EventDispatcher.Mount("lprp:givedirty", new Action<PlayerClient, int>(GiveDirty));
            EventDispatcher.Mount("lprp:addIntenvoryItem",
                new Action<PlayerClient, string, int, float>(AddInventory));
            EventDispatcher.Mount("lprp:removeIntenvoryItem",
                new Action<PlayerClient, string, int>(RemoveInventory));
            EventDispatcher.Mount("lprp:addWeapon", new Action<PlayerClient, string, int>(AddWeapon));
            EventDispatcher.Mount("lprp:removeWeapon", new Action<PlayerClient, string>(RemoveWeapon));
            EventDispatcher.Mount("lprp:addWeaponComponent",
                new Action<PlayerClient, string, string>(AddWeaponComp));
            EventDispatcher.Mount("lprp:removeWeaponComponent",
                new Action<PlayerClient, string, string>(RemoveWeaponComp));
            EventDispatcher.Mount("lprp:addWeaponTint", new Action<PlayerClient, string, int>(AddWeaponTint));
            EventDispatcher.Mount("lprp:removeItemsDeath", new Action<PlayerClient>(removeItemsDeath));
            EventDispatcher.Mount("lprp:salvaPlayer", new Action<PlayerClient>(SalvaPlayer));
            EventDispatcher.Mount("lprp:givemoneytochar", new Action<string, int, int>(GiveMoneyToChar));
            EventDispatcher.Mount("lprp:removemoneytochar", new Action<string, int, int>(RemoveMoneyToChar));
            EventDispatcher.Mount("lprp:givebanktochar", new Action<string, int, int>(GiveBankToChar));
            EventDispatcher.Mount("lprp:removebanktochar", new Action<string, int, int>(RemoveBankToChar));
            EventDispatcher.Mount("lprp:givedirtytochar", new Action<string, int, int>(GiveDirtyToChar));
            EventDispatcher.Mount("lprp:removedirtytochar", new Action<string, int, int>(RemoveDirtyToChar));
            EventDispatcher.Mount("lprp:givedirtytochar", new Action<string, int, int>(GiveDirtyToChar));
            EventDispatcher.Mount("lprp:addIntenvoryItemtochar",
                new Action<string, int, string, int, float>(AddInventoryToChar));
            EventDispatcher.Mount("lprp:removeIntenvoryItemtochar",
                new Action<string, int, string, int>(RemoveInventoryToChar));
            EventDispatcher.Mount("lprp:addWeapontochar", new Action<string, int, string, int>(AddWeaponToChar));
            EventDispatcher.Mount("lprp:removeWeapontochar",
                new Action<string, int, string>(RemoveWeaponToChar));
            EventDispatcher.Mount("lprp:addWeaponComponenttochar",
                new Action<string, int, string, string>(AddWeaponCompToChar));
            EventDispatcher.Mount("lprp:removeWeaponComponenttochar",
                new Action<string, int, string, string>(RemoveWeaponCompToChar));
            EventDispatcher.Mount("lprp:addWeaponTinttochar",
                new Action<string, int, string, int>(AddWeaponTintToChar));
            EventDispatcher.Mount("lprp:giveLicense", new Action<PlayerClient, string>(GiveLicense));
            EventDispatcher.Mount("lprp:giveLicenseToChar",
                new Action<PlayerClient, int, string>(GiveLicenseToChar));
            EventDispatcher.Mount("lprp:removeLicense", new Action<PlayerClient, string>(RemoveLicense));
            EventDispatcher.Mount("lprp:removeLicenseToChar",
                new Action<PlayerClient, int, string>(RemoveLicenseToChar));
            EventDispatcher.Mount("lprp:updateWeaponAmmo", new Action<PlayerClient, string, int>(AggiornaAmmo));
            EventDispatcher.Mount("lprp:giveInventoryItemToPlayer",
                new Action<PlayerClient, int, string, int>(GiveItemToOtherPlayer));
            EventDispatcher.Mount("lprp:giveWeaponToPlayer",
                new Action<PlayerClient, int, string, int>(GiveWeaponToOtherPlayer));
            EventDispatcher.Mount("lprp:callDBPlayers", new Func<PlayerClient, Task<Dictionary<string, User>>>(async (a) =>
                (await MySQL.QueryListAsync<User>("select * from users")).ToDictionary(p => a.Player.Handle)));
        }

        public static void FinishChar([FromSource] PlayerClient client, string data)
        {
            try
            {
                Char_data Char = data.FromJson<Char_data>();
                client.User.Characters.Add(Char);
            }
            catch (Exception e)
            {
                Server.Logger.Error($"{e.Message}");
            }
        }

        public static void deathStatus([FromSource] PlayerClient source, bool value)
        {
            source.User.DeathStatus = value;
            source.SetState($"{source.Status.RolePlayStates._name}:FinDiVita", value);
        }

        public static void PayFine([FromSource] PlayerClient source, int amount)
        {
            User player = source.User;

            if (amount == 5000)
                player.Money -= 5000;
            else
            {
                DateTime now = DateTime.Now;
                Server.Logger.Warning($"Il player {source.Player.Name} ha usato un lua executor / CheatEngine per modificare il valore da pagare alla morte!");
                source.Player.Drop("LastPlanet Shield [Sospetto Modding]: entra in discord a chiarire perfavore!");
                // AGGIUNGERE AVVISO DISCORD! ;)
            }
        }

        public static void Spawnato([FromSource] PlayerClient source)
        {
            User user = source.User;
            Server.Logger.Info($"{user.FullName} ({source.Player.Name} è entrato in città");
            foreach (var client in from PlayerClient client in BucketsHandler.RolePlay.Bucket.Players where client.Handle != source.Handle select client)
                client.Player.TriggerEvent("tlg:ShowNotification", "~g~" + user.FullName + " (" + source.Player.Name + ")~w~ è entrato in città");
            source.Player.TriggerEvent("lprp:createMissingPickups", PickupsServer.Pickups.ToJson());
            source.SetState($"{source.Status.PlayerStates._name}:Spawned", true);
        }

        //TODO: DA CAMBIARE CON NUOVO METODO
        public static async void SalvaPlayer([FromSource] PlayerClient client)
        {
            await BaseScript.Delay(0);
            string name = client.Player.Name;

            User user = client.User;

            if (client.Status.PlayerStates.Spawned)
            {
                client.Player.TriggerEvent("lprp:mostrasalvataggio");
                BucketsHandler.RolePlay.SalvaPersonaggioRoleplay(client);
                Server.Logger.Info("Salvato personaggio: '" + user.FullName + "' appartenente a '" + name + "' tramite telefono");
            }
            await Task.FromResult(0);
        }

        public static void removeItemsDeath([FromSource] PlayerClient source)
        {
            User player = source.User;
            int money = player.Money;
            int dirty = player.DirtCash;
            foreach (Inventory inv in player.CurrentChar.Inventory.ToList())
                player.removeInventoryItem(inv.Item, inv.Amount);
            foreach (Weapons inv in player.CurrentChar.Weapons.ToList()) player.removeWeapon(inv.name);
            player.Money -= money;
            player.DirtCash -= dirty;
        }

        public static void GiveMoney([FromSource] PlayerClient source, int amount)
        {
            User player = source.User;
            player.Money += amount;
        }

        public static void RemoveMoney([FromSource] PlayerClient source, int amount)
        {
            if (amount > 0)
                source.User.Money -= amount;
            else
                source.Player.Drop("Rilevata possibile modifica ai valori di gioco");
        }

        public static void GiveBank([FromSource] PlayerClient source, int amount)
        {
            source.User.Bank += amount;
        }

        public static void RemoveBank([FromSource] PlayerClient source, int amount)
        {
            source.User.Bank -= amount;
        }

        public static void GiveDirty([FromSource] PlayerClient source, int amount)
        {
            source.User.DirtCash += amount;
        }

        public static void RemoveDirty([FromSource] PlayerClient source, int amount)
        {
            source.User.DirtCash -= amount;
        }

        public static void AddInventory([FromSource] PlayerClient source, string item, int amount, float peso)
        {
            source.User.addInventoryItem(item, amount, peso > 0 ? peso : ConfigShared.SharedConfig.Main.Generici.ItemList[item].peso);
        }

        public static void RemoveInventory([FromSource] PlayerClient source, string item, int amount)
        {
            source.User.removeInventoryItem(item, amount);
        }

        public static void AddWeapon([FromSource] PlayerClient source, string weaponName, int ammo)
        {
            source.User.addWeapon(weaponName, ammo);
        }

        public static void RemoveWeapon([FromSource] PlayerClient source, string weaponName)
        {
            source.User.removeWeapon(weaponName);
        }

        public static void AddWeaponComp([FromSource] PlayerClient source, string weaponName, string weaponComponent)
        {
            source.User.addWeaponComponent(weaponName, weaponComponent);
        }

        public static void RemoveWeaponComp([FromSource] PlayerClient source, string weaponName, string weaponComponent)
        {
            source.User.removeWeaponComponent(weaponName, weaponComponent);
        }

        public static void AddWeaponTint([FromSource] PlayerClient source, string weaponName, int tint)
        {
            source.User.addWeaponTint(weaponName, tint);
        }

        public static void GiveMoneyToChar(string target, int charId, int amount)
        {
            if (amount < 1) return;
            User player = Funzioni.GetUserFromPlayerId(target);
            player.Money += amount;
        }

        public static void RemoveMoneyToChar(string target, int charId, int amount)
        {
            if (amount < 1) return;
            User player = Funzioni.GetUserFromPlayerId(target);
            player.Money -= amount;
        }

        public static void GiveBankToChar(string target, int charId, int amount)
        {
            User player = Funzioni.GetUserFromPlayerId(target);
            player.Bank += amount;
        }

        public static void RemoveBankToChar(string target, int charId, int amount)
        {
            User player = Funzioni.GetUserFromPlayerId(target);
            player.Bank -= amount;
        }

        public static void GiveDirtyToChar(string target, int charId, int amount)
        {
            User player = Funzioni.GetUserFromPlayerId(target);
            player.DirtCash += amount;
        }

        public static void RemoveDirtyToChar(string target, int charId, int amount)
        {
            User player = Funzioni.GetUserFromPlayerId(target);
            player.DirtCash -= amount;
        }

        public static void AddInventoryToChar(string target, int charId, string item, int amount, float peso)
        {
            Funzioni.GetUserFromPlayerId(target).addInventoryItem(item, amount,
                peso > 0 ? peso : ConfigShared.SharedConfig.Main.Generici.ItemList[item].peso);
        }

        public static void RemoveInventoryToChar(string target, int charId, string item, int amount)
        {
            Funzioni.GetUserFromPlayerId(target).removeInventoryItem(item, amount);
        }

        public static void AddWeaponToChar(string target, int charId, string weaponName, int ammo)
        {
            Funzioni.GetUserFromPlayerId(target).addWeapon(weaponName, ammo);
        }

        public static void RemoveWeaponToChar(string target, int charId, string weaponName)
        {
            Funzioni.GetUserFromPlayerId(target).removeWeapon(weaponName);
        }

        public static void AddWeaponCompToChar(string target, int charId, string weaponName, string weaponComponent)
        {
            Funzioni.GetUserFromPlayerId(target).addWeaponComponent(weaponName, weaponComponent);
        }

        public static void RemoveWeaponCompToChar(string target, int charId, string weaponName, string weaponComponent)
        {
            Funzioni.GetUserFromPlayerId(target).removeWeaponComponent(weaponName, weaponComponent);
        }

        public static void AddWeaponTintToChar(string target, int charId, string weaponName, int tint)
        {
            Funzioni.GetUserFromPlayerId(target).addWeaponTint(weaponName, tint);
        }

        private static void GiveLicense([FromSource] PlayerClient source, string license)
        {
            User player = source.User;
        }

        private static void GiveLicenseToChar([FromSource] PlayerClient source, int target, string license)
        {
            User player = source.User;
        }

        private static void RemoveLicense([FromSource] PlayerClient source, string license)
        {
            User player = source.User;
        }

        private static void RemoveLicenseToChar([FromSource] PlayerClient source, int target, string license)
        {
            User player = source.User;
        }

        private static void AggiornaAmmo([FromSource] PlayerClient source, string weaponName, int ammo)
        {
            source.User.updateWeaponAmmo(weaponName, ammo);
        }

        private static void GiveItemToOtherPlayer([FromSource] PlayerClient source, int target, string itemName, int amount)
        {
            User player = source.User;
            PlayerClient targetClient = Funzioni.GetClientFromPlayerId(target);
            User targetPlayer = targetClient.User;
            player.removeInventoryItem(itemName, amount);
            player.showNotification($"Hai dato {amount} di {ConfigShared.SharedConfig.Main.Generici.ItemList[itemName].label} a {targetPlayer.FullName}");
            targetPlayer.addInventoryItem(itemName, amount, ConfigShared.SharedConfig.Main.Generici.ItemList[itemName].peso);
            targetClient.TriggerSubsystemEvent("lprp:riceviOggettoAnimazione");
            targetPlayer.showNotification($"Hai ricevuto {amount} di {ConfigShared.SharedConfig.Main.Generici.ItemList[itemName].label} da {player.FullName}");
        }

        private static void GiveWeaponToOtherPlayer([FromSource] PlayerClient source, int target, string weaponName, int ammo)
        {
            User player = source.User;
            PlayerClient targetClient = Funzioni.GetClientFromPlayerId(target);
            User targetPlayer = targetClient.User;
            Tuple<int, Weapons> weapon = player.getWeapon(weaponName);
            Weapons arma = weapon.Item2;

            if (targetPlayer.hasWeapon(weaponName))
            {
                player.showNotification($"{player.FullName} ha già quest'arma!");
            }
            else
            {
                player.removeWeapon(weaponName);
                player.showNotification($"Hai dato la tua arma a {targetPlayer.FullName}");
                targetPlayer.addWeapon(weaponName, ammo);
                foreach (Components comp in arma.components) targetPlayer.addWeaponComponent(weaponName, comp.name);
                if (arma.tint != 0) targetPlayer.addWeaponTint(weaponName, arma.tint);
                targetClient.TriggerSubsystemEvent("lprp:riceviOggettoAnimazione");
                targetPlayer.showNotification($"Hai ricevuto un'arma con {ammo} munizioni da {player.FullName}");
            }
        }
    }
}