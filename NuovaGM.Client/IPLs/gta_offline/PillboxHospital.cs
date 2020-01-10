using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuovaGM.Client.IPLs.gtav
{
	public class PillboxHospital
	{
		private static bool _enabled = false;
		public static List<string> ipl = new List<string>(){ "rc12b_default" };
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
