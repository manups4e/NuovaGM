using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuovaGM.Client.IPLs.dlc_heists
{
	public class HeistYacht
	{
		private static bool _enabled = false;
		public static List<string> ipl = new List<string>()
		{
			"hei_yacht_heist",
			"hei_yacht_heist_bar",
			"hei_yacht_heist_bar_lod",
			"hei_yacht_heist_bedrm",
			"hei_yacht_heist_bedrm_lod",
			"hei_yacht_heist_bridge",
			"hei_yacht_heist_bridge_lod",
			"hei_yacht_heist_enginrm",
			"hei_yacht_heist_enginrm_lod",
			"hei_yacht_heist_lod",
			"hei_yacht_heist_lounge",
			"hei_yacht_heist_lounge_lod",
			"hei_yacht_heist_slod"
		};

		public class YacthWater
		{
			int modelHash = API.GetHashKey("apa_mp_apa_yacht_jacuzzi_ripple1");

			public async void Enable(bool state)
			{
				int handle = API.GetClosestObjectOfType(-2023.773f, -1038.0f, 5.40f, 5.0f, (uint)modelHash, false, false, false);
				if (state)
				{
					if (handle == 0)
					{
						API.RequestModel((uint)modelHash);
						while (!API.HasModelLoaded((uint)modelHash)) await BaseScript.Delay(0);
						Prop water = new Prop(API.CreateObjectNoOffset((uint)modelHash, -2023.773f, -1038.0f, 5.40f, false, true, false));
						API.SetEntityAsMissionEntity(water.Handle, false, false);
					}
				}
				else
				{
					if (handle != 0)
						API.SetEntityAsMissionEntity(handle, false, false);
					API.DeleteEntity(ref handle);
				}
			}
		}

		public static YacthWater Water = new YacthWater();

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
