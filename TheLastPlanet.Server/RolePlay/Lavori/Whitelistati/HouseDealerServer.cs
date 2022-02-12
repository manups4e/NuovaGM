using CitizenFX.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using TheLastPlanet.Server.Core;
using TheLastPlanet.Server.Proprietà;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.Internal.Events;

namespace TheLastPlanet.Server.Lavori.Whitelistati
{
    static class HouseDealerServer
    {
        public static void Init()
        {
            Server.Instance.AddEventHandler("housedealer:vendi", new Action<Player, bool, int, string, int>(Vendi));
            Server.Instance.AddEventHandler("lprp:agenteimmobiliare:salvaAppartamento", new Action<Player, string, string, string>(SalvaAppartamento));
            Server.Instance.Events.Mount("tlg:roleplay:onPlayerSpawn", new Action<ClientId>(Spawnato));
        }

        private static async void Spawnato(ClientId client)
        {
            dynamic aparts = await Server.Instance.Query("select * from immobili_creati");
            if (aparts.Count > 0)
            {
                Dictionary<string, string> apart = new Dictionary<string, string>();
                Dictionary<string, string> garag = new Dictionary<string, string>();
                foreach (dynamic p in aparts)
                {
                    if (p.tipo == "casa")
                        apart.Add(p.abbreviazione, p.datiImmobile);
                    else if (p.tipo == "garage")
                        garag.Add(p.abbreviazione, p.datiImmobile);
                }
                client.Player.TriggerEvent("lprp:housedealer:caricaImmobiliDaDB", apart.ToJson(), garag.ToJson());
            }
        }

        private static async void Vendi([FromSource] Player p, bool venduto, int target, string jsonProperty, int prezzo)
        {
            Player acquirente = Funzioni.GetPlayerFromId(target);
            KeyValuePair<string, JContainer> casa = jsonProperty.FromJson<KeyValuePair<string, JContainer>>();
            SoldProperty prop = new SoldProperty(acquirente.GetLicense(Identifier.Discord), acquirente.GetCurrentChar().FullName, "Appartamento", !venduto, prezzo, casa.Key, casa.Value["Label"].Value<string>(), "{}", "{}", "{}", "{}", DateTime.Now.AddDays(7), DateTime.Now);
            await Server.Instance.Execute("INSERT INTO proprietà Values(@disc, @pers, @tipo, @aff, @pr, @name, @label, @gar, @gua, @inv, @arm, @boll, @acq)", new
            {
                disc = prop.DiscordId,
                pers = prop.Personaggio,
                tipo = prop.Tipo,
                aff = prop.InAffitto,
                pr = prop.Prezzo,
                name = prop.Name,
                label = prop.Label,
                gar = prop.Garage,
                gua = prop.Guardaroba,
                inv = prop.Inventario,
                arm = prop.Armeria,
                boll = prop.Bollette,
                acq = prop.Acquisto
            });
            p.GetCurrentChar().showNotification($"Hai {(venduto ? "venduto" : "affittato")} ~y~{prop.Label}~w~ a ~o~{acquirente.GetCurrentChar().FullName}~w~.");
            acquirente.GetCurrentChar().showNotification($"Hai {(venduto ? "comprato" : "affittato")} un nuovo appartamento: ~y~{prop.Label}~w~.");
        }

        private static async void SalvaAppartamento([FromSource] Player p, string tipo, string jsonData, string abbreviazione)
        {
            await Server.Instance.Execute("INSERT INTO immobili_creati VALUES (@nome, @data, @abbr, @dati, @tipo)", new
            {
                nome = p.Name,
                data = DateTime.Now,
                abbr = abbreviazione,
                dati = jsonData,
                tipo
            });
            p.GetCurrentChar().showNotification($"Immobile ~y~{abbreviazione} salvato con successo!");
        }
    }
}
