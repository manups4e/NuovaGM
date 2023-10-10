using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using TheLastPlanet.Server.Core;
using TheLastPlanet.Server.Proprietà;


namespace TheLastPlanet.Server.Jobs.Whitelisted
{
    static class HouseDealerServer
    {
        public static void Init()
        {
            Server.Instance.AddEventHandler("housedealer:vendi", new Action<Player, bool, int, string, int>(Sell));
            Server.Instance.AddEventHandler("lprp:agenteimmobiliare:salvaAppartamento", new Action<Player, string, string, string>(SaveApartment));
            EventDispatcher.Mount("tlg:roleplay:onPlayerSpawn", new Action<PlayerClient>(Spawned));
        }

        private static async void Spawned([FromSource] PlayerClient client)
        {
            dynamic aparts = await Server.Instance.Query("select * from immobili_creati");
            if (aparts.Count > 0)
            {
                Dictionary<string, string> apart = new Dictionary<string, string>();
                Dictionary<string, string> garag = new Dictionary<string, string>();
                foreach (dynamic p in aparts)
                {
                    if (p.tipo == "home")
                        apart.Add(p.abbreviazione, p.datiImmobile);
                    else if (p.tipo == "garage")
                        garag.Add(p.abbreviazione, p.datiImmobile);
                }
                client.Player.TriggerEvent("lprp:housedealer:caricaImmobiliDaDB", apart.ToJson(), garag.ToJson());
            }
        }

        private static async void Sell([FromSource] Player p, bool sold, int target, string jsonProperty, int price)
        {
            Player buyer = Functions.GetPlayerFromId(target);
            KeyValuePair<string, JContainer> house = jsonProperty.FromJson<KeyValuePair<string, JContainer>>();
            SoldProperty prop = new SoldProperty(buyer.GetLicense(Identifier.Discord), buyer.GetCurrentChar().FullName, "Apartment", !sold, price, house.Key, house.Value["Label"].Value<string>(), "{}", "{}", "{}", "{}", DateTime.Now.AddDays(7), DateTime.Now);
            await Server.Instance.Execute("INSERT INTO proprietà Values(@disc, @pers, @tipo, @aff, @pr, @name, @label, @gar, @gua, @inv, @arm, @boll, @acq)", new
            {
                disc = prop.DiscordId,
                pers = prop.Character,
                tipo = prop.Type,
                aff = prop.Rent,
                pr = prop.Price,
                name = prop.Name,
                label = prop.Label,
                gar = prop.Garage,
                gua = prop.Wardrobe,
                inv = prop.Inventory,
                arm = prop.Armory,
                boll = prop.Bills,
                acq = prop.Purchase
            });
            p.GetCurrentChar().showNotification($"You {(sold ? "sold" : "rented")} ~y~{prop.Label}~w~ to ~o~{buyer.GetCurrentChar().FullName}~w~.");
            buyer.GetCurrentChar().showNotification($"You{(sold ? "bought" : "rented")} a new apartment: ~y~{prop.Label}~w~.");
        }

        private static async void SaveApartment([FromSource] Player p, string tipo, string jsonData, string abbreviazione)
        {
            await Server.Instance.Execute("INSERT INTO immobili_creati VALUES (@nome, @data, @abbr, @dati, @tipo)", new
            {
                nome = p.Name,
                data = DateTime.Now,
                abbr = abbreviazione,
                dati = jsonData,
                tipo
            });
            p.GetCurrentChar().showNotification($"property ~y~{abbreviazione} saved successfully!");
        }
    }
}
