using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.IPLs.gtav
{
	public class UFO
	{
		public class UFOHippie
		{
			private bool _enabled = false;
			public string ipl = "ufo";
			public bool Enabled
			{
				get { return _enabled; }
				set
				{
					_enabled = value;
					IplManager.EnableIpl(ipl, _enabled);
				}
			}
		}
		public class UFOChiliad
		{
			private bool _enabled = false;
			public string ipl = "ufo_eye";
			public bool Enabled
			{
				get { return _enabled; }
				set
				{
					_enabled = value;
					IplManager.EnableIpl(ipl, _enabled);
				}
			}
		}
		public class UFOZancudo
		{
			private bool _enabled = false;
			public string ipl = "ufo_lod";
			public bool Enabled
			{
				get { return _enabled; }
				set
				{
					_enabled = value;
					IplManager.EnableIpl(ipl, _enabled);
				}
			}
		}

		public static UFOHippie Hippie = new UFOHippie();
		public static UFOChiliad Chiliad = new UFOChiliad();
		public static UFOZancudo Zancudo = new UFOZancudo();
	}
}
