﻿using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheLastPlanet.Client.Core.Utility;

namespace TheLastPlanet.Client.IPLs.dlc_heists
{
	public class HeistYacht
	{
		private bool _enabled = false;
		public List<string> ipl = new List<string>()
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

		public YacthWater Water = new YacthWater(new Vector4(-2023.773f, -1038.0f, 5.40f, 5.0f));

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
	public class YacthWater
	{
		int modelHash = API.GetHashKey("apa_mp_apa_yacht_jacuzzi_ripple1");
		Vector4 Position = Vector4.Zero;
		public YacthWater(Vector4 pos)
        {
			Position = pos;
        }
		public async void Enable(bool state)
		{
			int handle = API.GetClosestObjectOfType(Position.X, Position.Y, Position.Z, Position.W, (uint)modelHash, false, false, false);
			if (state)
			{
				if (handle == 0)
				{
					API.RequestModel((uint)modelHash);
					while (!API.HasModelLoaded((uint)modelHash)) await BaseScript.Delay(0);
					handle = API.CreateObjectNoOffset((uint)modelHash, Position.X, Position.Y, Position.Z, false, true, false);
					API.SetEntityAsMissionEntity(handle, false, false);
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
}
