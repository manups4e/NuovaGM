using Impostazioni.Shared.Configurazione.Generici;
using TheLastPlanet.Shared.Internal.Events.Attributes;

namespace TheLastPlanet.Shared
{
	[Serialization]
	public partial class ConfigShared
	{
		public static SharedConfig SharedConfig = new SharedConfig();
	}

	[Serialization]
	public partial class SharedConfig
	{
		public MainShared Main = new MainShared();
	}

	[Serialization]
	public partial class MainShared
	{
		public SharedConfigVeicoli Veicoli = new SharedConfigVeicoli();
		public SharedMeteo Meteo = new SharedMeteo();
		public SharedGenerici Generici = new SharedGenerici();
	}
}