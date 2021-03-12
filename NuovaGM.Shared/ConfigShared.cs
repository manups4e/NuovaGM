using Impostazioni.Shared.Configurazione.Generici;

namespace TheLastPlanet.Shared
{
	public class ConfigShared
	{
		public static SharedConfig SharedConfig = new SharedConfig();
	}

	public class SharedConfig
	{
		public MainShared Main = new MainShared();
	}

	public class MainShared
	{
		public SharedConfigVeicoli Veicoli = new SharedConfigVeicoli();
		public SharedMeteo Meteo = new SharedMeteo();
		public SharedGenerici Generici = new SharedGenerici();
	}
}