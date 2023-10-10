using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheLastPlanet.Server.Core;
using TheLastPlanet.Server.Core.PlayerChar;


namespace TheLastPlanet.Server.banking
{
    internal static class BankingServer
    {
        public static void Init()
        {
            EventDispatcher.Mount("lprp:banking:sendMoney", new Func<PlayerClient, string, int, Task<KeyValuePair<bool, string>>>(SendMoney));
            EventDispatcher.Mount("lprp:banking:atmwithdraw", new Func<PlayerClient, int, Task<KeyValuePair<bool, string>>>(Withdraw));
            EventDispatcher.Mount("lprp:banking:atmdeposit", new Func<PlayerClient, int, Task<KeyValuePair<bool, string>>>(Deposit));
        }

        private static async Task<KeyValuePair<bool, string>> SendMoney([FromSource] PlayerClient source, string name, int amount)
        {
            User user = source.User;
            if (user.Bank >= amount)
            {
                foreach (PlayerClient p in Server.Instance.Clients)
                {
                    if (name.ToLower() == p.User.FullName.ToLower())
                    {
                        user.Bank -= amount;
                        p.User.Bank += amount;
                        Server.Logger.Info($"Character '{user.FullName}' [{source.Player.Name}] sent ${amount} to'{p.User.FullName}' [{p.Player.Name}]");
                        return new KeyValuePair<bool, string>(true, user.Bank.ToString());
                    }
                }
                return new KeyValuePair<bool, string>(false, "User not found");
            }
            return new KeyValuePair<bool, string>(false, "Your founds don't cover the transaction!");
        }

        private static async Task<KeyValuePair<bool, string>> Withdraw([FromSource] PlayerClient source, int amount)
        {
            Player player = source.Player;
            if (amount > 0)
            {
                User user = player.GetCurrentChar();
                int bal = user.Bank;
                int newamt = bal - amount;

                if (bal >= amount)
                {
                    user.Money += amount;
                    Server.Logger.Info($"The character '{user.FullName}' [{player.Name}] has deposited {amount}$");
                    user.Bank -= amount;
                    return new KeyValuePair<bool, string>(true, newamt.ToString());
                }
                return new KeyValuePair<bool, string>(false, "You do not have enough funds in your checking account for this transaction.");
            }
            return new KeyValuePair<bool, string>(false, "You must enter a positive value.");
        }

        private static async Task<KeyValuePair<bool, string>> Deposit([FromSource] PlayerClient source, int amount)
        {
            Player player = source.Player;
            if (amount > 0)
            {
                User user = player.GetCurrentChar();
                int money = user.Money;
                int bankmoney = user.Bank;
                int newamt = bankmoney + amount;

                if (amount <= money)
                {
                    user.Money -= amount;
                    Server.Logger.Info($"The character '{user.FullName}' [{player.Name}] has deposited {amount}$");
                    user.Bank += amount;
                    return new KeyValuePair<bool, string>(true, newamt.ToString());
                }
                return new KeyValuePair<bool, string>(false, "You do not have enough money in your wallet for this transaction.");
            }
            return new KeyValuePair<bool, string>(false, "You must enter a positive value.");

        }
    }
}