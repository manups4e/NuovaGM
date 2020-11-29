using CitizenFX.Core;
using CitizenFX.Core.Native;
using TheLastPlanet.Client.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.IPLs.dlc_gunrunning
{
	public class GunrunningYacht
	{
		private static bool _enabled = false;
		public static List<string> ipl = new List<string>()
		{
			"gr_heist_yacht2",
			"gr_heist_yacht2_bar",
			"gr_heist_yacht2_bar_lod",
			"gr_heist_yacht2_bedrm",
			"gr_heist_yacht2_bedrm_lod",
			"gr_heist_yacht2_bridge",
			"gr_heist_yacht2_bridge_lod",
			"gr_heist_yacht2_enginrm",
			"gr_heist_yacht2_enginrm_lod",
			"gr_heist_yacht2_lod",
			"gr_heist_yacht2_lounge",
			"gr_heist_yacht2_lounge_lod",
			"gr_heist_yacht2_slod",
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

		public class YWater
		{
			int ModelHash = Funzioni.HashInt("apa_mp_apa_yacht_jacuzzi_ripple1");
			public async void Enable(bool state)
			{
				int handle = API.GetClosestObjectOfType(-1369.0f, 6736.0f, 5.40f, 5.0f, (uint)ModelHash, false, false, false);
				if (state)
				{
					if (handle == 0)
					{
						API.RequestModel((uint)ModelHash);
						while (!API.HasModelLoaded((uint)ModelHash)) await BaseScript.Delay(0);
						int water = API.CreateObjectNoOffset((uint)ModelHash, -1369.0f, 6736.0f, 5.40f, true, true, false);
						API.SetEntityAsMissionEntity(water, false, true);
					}
					else
					{
						if (handle != 0)
						{
							API.SetEntityAsMissionEntity(handle, false, false);
							API.DeleteEntity(ref handle);
						}
					}
				}
			}
		}

		public static YWater Water = new YWater();

		public static void LoadDefault()
		{
			Enabled = true;
		}

	}
}
