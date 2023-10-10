using Settings.Shared.Config.Generic;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheLastPlanet.Server.Core;
using TheLastPlanet.Server.Core.PlayerChar;

namespace TheLastPlanet.Server.Vehicles
{
    static class FuelServer
    {
        public static void Init()
        {
            Server.Instance.AddEventHandler("lprp:fuel:payForFuel", new Action<Player, int, float, float>(PayForFuel));
            Server.Instance.AddEventHandler("lprp:fuel:buytanker", new Action<Player, string>(BuyTanker));
            Server.Instance.AddEventHandler("lprp:fuel:buyfuelfortanker", new Action<Player, int, int>(BuyFuelForTanker));
            Server.Instance.AddEventHandler("lprp:getDecor", new Action<Player>(RespondDecor));
            EventDispatcher.Mount("tlg:roleplay:getStations", new Func<PlayerClient, Task<List<GasStation>>>(GetStations));
        }

        private static async Task<List<GasStation>> GetStations([FromSource] PlayerClient client)
        {
            return ConfigShared.SharedConfig.Main.Vehicles.gasstations;
        }

        public static void RespondDecor([FromSource] Player p)
        {
            BaseScript.TriggerClientEvent(p, "lprp:fuel:setFuelDecor", "NuovaGMRPFUEL20192020!!yeah!?#@", 65.0f);
        }

        public static async void PayForFuel([FromSource] Player p, int stationindex, float addedfuel, float fuelval)
        {
            User player = p.GetCurrentChar();
            int sidx = stationindex;
            float fuelCost;
            dynamic result = await Server.Instance.Query($"SELECT `fuelprice` FROM `businesses` WHERE `stationindex` = @idx", new { idx = sidx });
            await BaseScript.Delay(0);
            int money = player.Money;
            int bank = player.Bank;
            fuelCost = result[0].fuelprice;
            int price = (int)Math.Ceiling(addedfuel * fuelCost);
            if (money >= price)
            {
                player.Money -= price;
                Server.Logger.Info("The character " + player.FullName + " of the player " + GetPlayerName(player.source) + " paid " + price + "$ to get fuel.");
                BaseScript.TriggerEvent("lprp:serverlog", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + " -- The character " + player.FullName + " of the player " + GetPlayerName(player.source) + " paid " + price + "$ to get fuel.");
                p.TriggerEvent("lprp:fuel:addfueltovehicle", true, "Hai pagato ~b~$ " + price + "~w~ per fare carburante.", fuelval);
                BaseScript.TriggerEvent("lprp:businesses:addmoneytostation", sidx, price);
                BaseScript.TriggerEvent("lprp:businesses:removefuelfromstation", stationindex, addedfuel);
            }
            else
            {
                if (bank >= price)
                {
                    player.Bank -= price;
                    Server.Logger.Info("The character " + player.FullName + " of the player " + GetPlayerName(player.source) + " paid " + price + "$ to get fuel.");
                    BaseScript.TriggerEvent("lprp:serverlog", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + " -- The character " + player.FullName + " of the player " + GetPlayerName(player.source) + " paid " + price + "$ to get fuel.");
                    BaseScript.TriggerClientEvent(p, "lprp:fuel:addfueltovehicle", true, "Hai pagato ~b~$ " + price + "~w~ per fare carburante.", fuelval);
                    BaseScript.TriggerEvent("lprp:businesses:addmoneytostation", sidx, price);
                    BaseScript.TriggerEvent("lprp:businesses:removefuelfromstation", stationindex, addedfuel);
                }
                else
                    BaseScript.TriggerClientEvent(p, "lprp:fuel:addfueltovehicle", false, "You don't have enough money in your wallet or bank to get fuel.");
            }
        }

        public static void BuyTanker([FromSource] Player p, string Json)
        {
            User user = p.GetCurrentChar();
            Tanker t = Json.FromJson<Tanker>();
            int amount = (int)Math.Ceiling(t.ppu * t.fuelForTanker);
            if (user.Money >= amount)
            {
                user.Money -= amount;
                Server.Logger.Info($"The character {user.FullName} [{GetPlayerName(p.Handle)}] paid {amount}$ for a fuel tank.");
                BaseScript.TriggerEvent("lprp:serverlog", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + $" -- The character {user.FullName} [{GetPlayerName(p.Handle)}] paid {amount}$ for a fuel tank.");
                BaseScript.TriggerClientEvent(p, "lprp:fuel:buytanker", true, t.ToJson());
            }
            else
            {
                BaseScript.TriggerClientEvent(p, "lprp:fuel:buytanker", false, "You can't afford to buy the gas can.");
            }
        }

        public static void BuyFuelForTanker([FromSource] Player p, int cost, int fuel)
        {
            User user = p.GetCurrentChar();
            if (user.Money >= cost)
            {
                user.Money -= cost;
                Server.Logger.Info($"The character {user.FullName} [{GetPlayerName(p.Handle)}] paid {cost}$ to fill the entire tank with fuel.");
                BaseScript.TriggerEvent("lprp:serverlog", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + $"--The character {user.FullName} [{GetPlayerName(p.Handle)}] paid {cost}$ to fill the entire tank with fuel.");
                BaseScript.TriggerClientEvent(p, "lprp:fuel:buyfuelfortanker", true, fuel.ToString());
            }
            else
            {
                BaseScript.TriggerClientEvent(p, "lprp:fuel:buyfuelfortanker", false, "You don't have enough money for this purchase.");
            }
        }

    }

    public class Tanker
    {
        public Vector3 pos;
        public float heading;
        public Vector3 t;
        public float theading;
        public float ppu;
        public int fuelForTanker;
        public Tanker(Vector3 p, float h, Vector3 t, float th, float ppu, int fuel)
        {
            pos = p;
            heading = h;
            this.t = t;
            theading = th;
            this.ppu = ppu;
            fuelForTanker = fuel;
        }
    }
}
