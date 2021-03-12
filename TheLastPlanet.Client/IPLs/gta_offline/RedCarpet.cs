using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.IPLs.gtav
{
	public class RedCarpet
	{
		private static bool _enabled = false;
		public static List<string> ipl = new List<string>() { "redCarpet" };
		public static bool Enabled
		{
			get { return _enabled; }
			set
			{
				_enabled = value;
				IplManager.EnableIpl(ipl, _enabled);
			}
		}
	}
}
