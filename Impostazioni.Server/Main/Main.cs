using System.Collections.Generic;

namespace Settings.Server.Configurazione.Main
{
    public class MainConfig
    {
        public string ServerName { get; set; }
        public string? WebHookLog { get; set; }
        public string? WebHookAnticheat { get; set; }
        public string NotWhitelisted { get; set; }
        public bool EnableAntiSpam { get; set; }
        public int PlayersToStartRocade { get; set; }
        public int PingMax { get; set; }
        public int SaveAll { get; set; }
        public int RentPriceGasPumps { get; set; }
        public int RentPriceShops { get; set; }
        public string WhitelistedRoles { get; set; }
        public string DiscordToken { get; set; }
        public long GuildId { get; set; }
        public Dictionary<string, string> BadWords { get; set; }
    }
}