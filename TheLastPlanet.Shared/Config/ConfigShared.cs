using Impostazioni.Shared.Configurazione.Generici;
using FxEvents.Shared.Attributes;

namespace TheLastPlanet.Shared
{
    public class ConfigShared
    {
        public static SharedConfig SharedConfig = new();
    }

    public class SharedConfig
    {
        public MainShared Main = new();
    }

    
    public class MainShared
    {
        public SharedConfigVeicoli Veicoli = new();
        public SharedMeteo Meteo = new();
        public SharedGenerici Generici = new();
    }
}