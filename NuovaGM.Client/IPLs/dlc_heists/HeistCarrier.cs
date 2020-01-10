﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuovaGM.Client.IPLs.dlc_heists
{
	public class HeistCarrier
	{
		private static bool _enabled = false;
		public static List<string> ipl = new List<string>() 
		{       
			"hei_carrier",
			"hei_carrier_int1",
			"hei_carrier_int1_lod",
			"hei_carrier_int2",
			"hei_carrier_int2_lod",
			"hei_carrier_int3",
			"hei_carrier_int3_lod",
			"hei_carrier_int4",
			"hei_carrier_int4_lod",
			"hei_carrier_int5",
			"hei_carrier_int5_lod",
			"hei_carrier_int6",
			"hei_carrier_int6_lod",
			"hei_carrier_lod",
			"hei_carrier_slod"
		};
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
