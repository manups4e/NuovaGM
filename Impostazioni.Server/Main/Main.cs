using System.Collections.Generic;
using Newtonsoft.Json;
using TheLastPlanet.Shared.Internal.Events.Attributes;

namespace Impostazioni.Server.Configurazione.Main
{
    public class ConfPrincipale
    {
        public string NomeServer{get; set;}
        public string? WebHookLog { get; set; }
        public string? WebHookAnticheat{get; set;}
        public string notWhitelisted{get; set;}
        public bool EnableAntiSpam{get; set;}
        public int PlayersToStartRocade{get; set;}
        public int PingMax{get; set;}
        public int SalvataggioTutti{get; set;}
        public int RentPricePompeDiBenzina{get; set;}
        public int RentPriceNegozi{get; set;}
        public string RuoloWhitelistato{get; set;}
        public string DiscordToken{get; set;}
        public long GuildId{get; set;}
        public Dictionary<string, string> BadWords { get; set; }
    }
}