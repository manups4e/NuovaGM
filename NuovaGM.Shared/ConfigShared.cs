using NuovaGM.Shared.Meteo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuovaGM.Shared
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
	}
	public class SharedConfigVeicoli
	{
		public List<GasStation> gasstations = new List<GasStation>();
	}
}