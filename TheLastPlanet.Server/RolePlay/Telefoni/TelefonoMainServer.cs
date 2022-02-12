using CitizenFX.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using TheLastPlanet.Server.Core;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.Internal.Events;

namespace TheLastPlanet.Server.Telefoni
{
    static class TelefonoMainServer
    {
        public static ConcurrentDictionary<string, Phone> Phones = new ConcurrentDictionary<string, Phone>();

        public static void Init()
        {
            //Server.Instance.AddEventHandler("lprp:setupUser", new Action<Player>(SetupPhone));
            Server.Instance.Events.Mount("tlg:roleplay:onPlayerSpawn", new Action<ClientId>(SetupPhone));
        }

        private static async void SetupPhone(ClientId client)
        {
            await BaseScript.Delay(0);
            try
            {
                dynamic result = await Server.Instance.Query("SELECT * FROM telefoni WHERE discord = @disc", new { disc = License.GetLicense(client.Player, Identifier.Discord) });
                await BaseScript.Delay(0);
                string valore = JsonConvert.SerializeObject(result);
                if (valore != "[]" && valore != "{}" && valore != null)
                {
                    Phones[client.Handle + ""] = new Phone(client.Player, result[0]);
                    string datiphone = (Phones[client.Handle + ""]).ToJson();
                    client.Player.TriggerEvent("lprp:setupPhoneClientUser", datiphone);
                }
                else
                {
                    await Server.Instance.Execute("INSERT INTO telefoni (discord, playerName, phone_data) VALUES (@disc, @name, @data)", new
                    {
                        disc = License.GetLicense(client, Identifier.Discord),
                        name = client.Player.Name,
                        data = "{}"
                    });
                    await BaseScript.Delay(0);
                    dynamic Newresult = await Server.Instance.Query("SELECT * FROM telefoni WHERE discord = @disc", new { disc = License.GetLicense(client.Player, Identifier.Discord) });
                    await BaseScript.Delay(0);
                    Phones[client.Player.Handle] = new Phone(client.Player, Newresult[0]);
                    string datiphone = (Phones[client.Player.Handle]).ToJson();
                    client.Player.TriggerEvent("lprp:setupPhoneClientUser", datiphone);
                }
            }
            catch (Exception e)
            {
                Server.Logger.Error(e.ToString());
                Server.Logger.Error(e.StackTrace);
            }
        }
    }
}
