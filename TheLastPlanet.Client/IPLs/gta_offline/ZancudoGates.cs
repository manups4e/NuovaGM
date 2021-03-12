using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.IPLs.gtav
{
	public class ZancudoGates
	{
		public class ZGates
		{
			public void Open()
			{
				IplManager.EnableIpl("CS3_07_MPGates", false);
			}
			public void Close()
			{
				IplManager.EnableIpl("CS3_07_MPGates", true);
			}
		}

		public static ZGates Gates = new ZGates();

		public static void LoadDefault()
		{
			Gates.Open();
		}
	}
}
