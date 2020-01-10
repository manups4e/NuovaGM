using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuovaGM.Client.IPLs.gtav
{
	public class BahamaMamas // -1388.0013, -618.41967, 30.819599
	{
		private static bool _enabled = false;
		public static List<string> ipl = new List<string>() { "hei_sm_16_interior_v_bahama_milo_" };
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
