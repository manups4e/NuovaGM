using CitizenFX.Core;
using TheLastPlanet.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.RolePlay.Sport
{
	static class Freccette
	{
		private static Vector3 inizia = new Vector3(987.8541f, -98.4173f, 73.8599f);
		private static Vector3 inizia2 = new Vector3(1992.293f, 3050.583f, 47.98973f);
		private static List<ObjectHash> frecce = new List<ObjectHash>
		{
			ObjectHash.prop_dart_1,
			ObjectHash.prop_dart_2,
		};

		private static List<ObjectHash> targets = new List<ObjectHash>
		{
			ObjectHash.prop_dart_bd_01,
			ObjectHash.prop_dart_bd_cab_01,
			ObjectHash.prop_target_bull
		};

		public static void Init()
		{

		}
	}
}
