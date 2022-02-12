using Impostazioni.Shared.Configurazione.Generici;
using TheLastPlanet.Shared.Internal.Events.Attributes;

namespace TheLastPlanet.Shared
{
    [Serialization]
    public partial class ConfigShared
    {
        public static SharedConfig SharedConfig = new();
    }

    [Serialization]
    public partial class SharedConfig
    {
        public MainShared Main = new();
    }

    [Serialization]
    public partial class MainShared
    {
        public SharedConfigVeicoli Veicoli = new();
        public SharedMeteo Meteo = new();
        public SharedGenerici Generici = new();
    }
}