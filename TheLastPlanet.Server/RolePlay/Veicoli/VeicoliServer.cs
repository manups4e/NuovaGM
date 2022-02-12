using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheLastPlanet.Server.Core;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.Internal.Events;
using TheLastPlanet.Shared.Veicoli;

namespace TheLastPlanet.Server.Veicoli
{
    internal static class VeicoliServer
    {
        private static string lasthost = "";

        public static void Init()
        {
            Server.Instance.Events.Mount("tlg:roleplay:onPlayerSpawn", new Action<ClientId>(onPlayerSpawn));
            Server.Instance.AddEventHandler("lprp:lvc_TogIndicState_s", new Action<Player, int>(lvc_TogIndicState_s));
            Server.Instance.AddEventHandler("lprp:SilentSiren", new Action<Player, bool>(SilentSiren));
            Server.Instance.AddEventHandler("brakes:add_rear", new Action<int>(AddRear));
            Server.Instance.AddEventHandler("brakes:add_front", new Action<int>(AddFront));
            Server.Instance.AddEventHandler("brakes:rem_rear", new Action<int>(RemRear));
            Server.Instance.AddEventHandler("brakes:rem_front", new Action<int>(RemFront));
            Server.Instance.AddEventHandler("lprp:vehInGarage", new Action<Player, string, bool, string>(InGarage));
            Server.Instance.Events.Mount("lprp:caricaVeicoli", new Func<ClientId, ulong, Task<List<OwnedVehicle>>>(async (a, b) =>
            {
                const string query = "SELECT * FROM owned_vehicles WHERE UserID = @disc AND char_id = @pers";
                Player player = a.Player;
                dynamic vehs = await MySQL.QueryListAsync(query, new { disc = player.GetCurrentChar().ID, pers = b });
                List<OwnedVehicle> ownedVehicles = new();
                foreach (var v in vehs)
                    ownedVehicles.Add(new(v.targa, v.vehicle_data, v.garage, v.stato));
                foreach (var veh in ownedVehicles) player.GetCurrentChar().CurrentChar.Veicoli.Add(veh);

                return player.GetCurrentChar().CurrentChar.Veicoli;
            }));
        }

        public static async void onPlayerSpawn(ClientId client)
        {
            await BaseScript.Delay(0);

            if (client.Handle == 1)
            {
                Debug.WriteLine("train timeout activated.");
                await BaseScript.Delay(15000);
                activateTrain();
                //BaseScript.TriggerClientEvent("lprp:getHost", host);
            }
        }

        public static void SilentSiren([FromSource] Player player, bool toggle) { BaseScript.TriggerClientEvent("lprp:updateSirens", player.Handle, toggle); }

        public static void lvc_TogIndicState_s([FromSource] Player player, int newstate) { BaseScript.TriggerClientEvent("lprp:lvc_TogIndicState_c", player.Handle, newstate); }

        public static void activateTrain() { BaseScript.TriggerClientEvent("lprp:spawntrain"); }

        private static void AddRear(int veh) { BaseScript.TriggerClientEvent("cBrakes:add_rear", veh); }
        private static void AddFront(int veh) { BaseScript.TriggerClientEvent("cBrakes:add_front", veh); }
        private static void RemRear(int veh) { BaseScript.TriggerClientEvent("cBrakes:rem_rear", veh); }
        private static void RemFront(int veh) { BaseScript.TriggerClientEvent("cBrakes:rem_front", veh); }

        private static async void InGarage([FromSource] Player p, string plate, bool inGarage, string props)
        {
            p.GetCurrentChar().CurrentChar.Veicoli.FirstOrDefault(x => x.Targa == plate).Garage.InGarage = inGarage;

            if (inGarage)
            {
                p.GetCurrentChar().CurrentChar.Veicoli.FirstOrDefault(x => x.Targa == plate).DatiVeicolo.props = props.FromJson<VehProp>(settings: JsonHelper.IgnoreJsonIgnoreAttributes);
                await Server.Instance.Execute("Update owned_vehicles set Garage = @gar, vehicle_data = @dat WHERE targa = @t", new { gar = p.GetCurrentChar().CurrentChar.Veicoli.FirstOrDefault(x => x.Targa == plate).Garage.ToJson(), dat = p.GetCurrentChar().CurrentChar.Veicoli.FirstOrDefault(x => x.Targa == plate).DatiVeicolo.ToJson(settings: JsonHelper.IgnoreJsonIgnoreAttributes), t = plate });
            }
            else
            {
                await Server.Instance.Execute("Update owned_vehicles set Garage = @gar WHERE targa = @t", new { gar = p.GetCurrentChar().CurrentChar.Veicoli.FirstOrDefault(x => x.Targa == plate).Garage.ToJson(), t = plate });
            }

            //p.TriggerEvent("lprp:sendUserInfo", p.GetCurrentChar().Characters.ToJson(includeEverything: true), p.GetCurrentChar().char_current, p.GetCurrentChar().group);
        }
    }
}