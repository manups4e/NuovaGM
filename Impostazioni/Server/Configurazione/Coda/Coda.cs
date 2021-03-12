using System.Collections.Generic;

namespace Impostazioni.Server.Configurazione.Coda
{
    public class ConfigCoda
    {
        public Dictionary<string, string> messages = new Dictionary<string, string>()
        {
            ["Gathering"] = "Shield 2.0: raccolta info coda e controllo credenziali",
            ["License"] = "Licenza SocialClub obbligatoria",
            ["Discord"] = "ERRORE: DiscordID non trovato",
            ["Steam"] = "Steam obbligatorio",
            ["Banned"] = "Sei stato Bannato",
            ["Whitelist"] = "SHIELD 2.0:\nNon sei autorizzato a entrare su questo server!\nChiedi l'autorizzazione sul canale Discord di Manups4e (discord.gg/n4ep9Fq) nella sezione dedicata!.",
            ["Queue"] = "Sei in coda",
            ["PriorityQueue"] = "In coda con priorità",
            ["Canceled"] = "Espulso dalla coda",
            ["Error"] = "Errore contatta lo staff",
            ["Timeout"] = "Ecceduto tempo massimo di caricamento [timeout]",
            ["QueueCount"] = "[Coda: {0}]",
            ["Symbols"] = "I simboli non sono ammessi nel tuo nome steam"
        };
        public List<string> permessi = new List<string>()
        {
            "Admin",
            "Regina",
            "Tester",
            "Founder",
            "Fidanzata"
        };
        public bool whitelistonly = true;
        public int loadTime = 4;
        public int graceTime = 3;
        public int queueGraceTime = 2;
        public bool contoCodaFineNomeServer = true;
        public bool allowSymbols = true;
        public bool stateChangeMessages = true;
    }
    
}