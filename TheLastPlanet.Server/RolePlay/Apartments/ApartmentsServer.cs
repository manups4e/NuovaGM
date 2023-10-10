using System;
using TheLastPlanet.Server.Core;

namespace TheLastPlanet.Server.Apartments
{
    internal static class ApartmentsServer
    {
        public static void Init()
        {
            Server.Instance.AddEventHandler("lprp:citofonaAlPlayer", new Action<Player, int, string>(interphone));
            Server.Instance.AddEventHandler("lprp:citofono:puoEntrare", new Action<Player, int, string>(canEnter));
            Server.Instance.AddEventHandler("lprp:caricaAppartamenti", new Action<Player>(LoadProperties));
            Server.Instance.AddEventHandler("lprp:entraGarageConProprietario", new Action<Player, int, Vector3>(EnterWithOwner));
        }

        private static void interphone([FromSource] Player buzzer, int ownerId, string app)
        {
            Player citofonato = Functions.GetPlayerFromId(ownerId);
            citofonato.TriggerEvent("lprp:richiestaDiEntrare", Convert.ToInt32(citofonato.Handle), app);
        }

        private static void canEnter([FromSource] Player atHome, int buzzerServerId, string app)
        {
            Player p = Functions.GetPlayerFromId(buzzerServerId);
            p.TriggerEvent("lprp:citofono:puoiEntrare", Convert.ToInt32(atHome.Handle), app);
        }

        private static async void LoadProperties([FromSource] Player p)
        {
            dynamic result = await Server.Instance.Query("SELECT * FROM proprietà WHERE DiscordId = @id AND Personaggio = @pers", new { id = p.GetLicense(Identifier.Discord), pers = p.GetCurrentChar().FullName });
            if (result.Count > 0)
                foreach (dynamic ap in result)
                    p.GetCurrentChar().CurrentChar.Properties.Add(ap.Name);
            //p.TriggerEvent("lprp:sendUserInfo", p.GetCurrentChar().Characters.ToJson(includeEverything: true), p.GetCurrentChar().char_current, p.GetCurrentChar().group);
        }

        private static void EnterWithOwner([FromSource] Player p, int serverId, Vector3 pos)
        {
            Player sd = Functions.GetPlayerFromId(serverId);
            sd.TriggerEvent("lprp:entraGarageConProprietario", pos);
        }
    }
}