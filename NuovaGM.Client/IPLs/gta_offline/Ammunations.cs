using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuovaGM.Client.IPLs.gtav
{
	public class Ammunations
	{
		public class AmmunationsDetails
		{
			public List<string> hooks = new List<string> { "GunStoreHooks" }; // ganci per mostrare le armi
			public List<string> hooksClub = new List<string> { "GunClubWallHooks" }; // ganci per mostrare le armi
			public void Enable(List<string> details, bool state, bool refresh)
			{
				if (details == Ammunations.Details.hooks)
					IplManager.SetIplPropState(Ammunations.AmmunationsId, details, state, refresh);
				else if (details == Ammunations.Details.hooksClub)
					IplManager.SetIplPropState(Ammunations.GunClubsId, details, state, refresh);
			}
		}

		public static List<int> AmmunationsId = new List<int>()
		{
			140289,  // 249.8, -47.1, 70.0
			153857,	 // 844.0, -1031.5, 28.2
			168193,  // -664.0, -939.2, 21.8
			164609,	 // -1308.7, -391.5, 36.7
			176385,	 // -3170.0, 1085.0, 20.8
			175617,	 // -1116.0, 2694.1, 18.6
			200961,	 // 1695.2, 3756.0, 34.7
			180481,	 // -328.7, 6079.0, 31.5
			178689	 // 2569.8, 297.8, 108.7
		};

		public static List<int> GunClubsId = new List<int>()
		{
			137729,  // 19.1, -1110.0, 29.8
			248065   // 811.0, -2152.0, 29.6
		};

		public static AmmunationsDetails Details = new AmmunationsDetails();

		public static void LoadDefault()
		{
			Details.Enable(Details.hooks, true, true);
			Details.Enable(Details.hooksClub, true, true);
		}
	}
	
}
