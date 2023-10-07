using CitizenFX.Core;
using System;
using System.Collections.Generic;
using TheLastPlanet.Server.Core;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.Vehicles;

namespace TheLastPlanet.Server.Lavori.Whitelistati
{
    static class CarDealerServer
    {
        public static void Init()
        {
            Server.Instance.AddEventHandler("lprp:cardealer:attivaCatalogoAlcuni", new Action<Player, List<int>>(CatalogoAlcuni));
            Server.Instance.AddEventHandler("lprp:cardealer:cambiaVehCatalogo", new Action<Player, List<int>, string>(CambiaVeh));
            Server.Instance.AddEventHandler("lprp:cardealer:vendiVehAMe", new Action<Player, string>(VendiAMe));
        }

        private static async void VendiAMe([FromSource] Player p, string JsonVeh)
        {
            OwnedVehicle veh = JsonVeh.FromJson<OwnedVehicle>(settings: JsonHelper.IgnoreJsonIgnoreAttributes);
            await Server.Instance.Execute("INSERT INTO owned_vehicles VALUES (@disc, @name, @charname, @plate, @vehN, @data, @garage, @state)", new
            {
                disc = p.GetLicense(Identifier.Discord),
                name = p.Name,
                charname = p.GetCurrentChar().FullName,
                plate = veh.Plate,
                vehN = veh.VehData.Props.Name,
                data = veh.VehData.ToJson(settings: JsonHelper.IgnoreJsonIgnoreAttributes),
                garage = veh.Garage.ToJson(settings: JsonHelper.IgnoreJsonIgnoreAttributes),
                state = veh.State,
            });
            p.GetCurrentChar().CurrentChar.Vehicles.Add(veh);
            //p.TriggerEvent("lprp:sendUserInfo", p.GetCurrentChar().Characters.ToJson(includeEverything: true), p.GetCurrentChar().char_current, p.GetCurrentChar().group);
        }

        private static void CatalogoAlcuni([FromSource] Player p, List<int> players)
        {
            p.TriggerEvent("lprp:cardealer:catalogoAlcuni", true, players);
            players.ForEach(x => Funzioni.GetPlayerFromId(x).TriggerEvent("lprp:cardealer:catalogoAlcuni", false, players));
        }

        private static void CambiaVeh([FromSource] Player p, List<int> players, string vehName)
        {
            p.TriggerEvent("lprp:cardealer:catalogoAlcuni", true, vehName);
            players.ForEach(x => Funzioni.GetPlayerFromId(x).TriggerEvent("lprp:cardealer:catalogoAlcuni", false, vehName));
        }
    }
}
