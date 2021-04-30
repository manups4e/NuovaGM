using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using Logger;
using TheLastPlanet.Client.Core.PlayerChar;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.MenuNativo;
using TheLastPlanet.Shared;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client.Core.BucketChooser
{
	internal static class MainChooser
	{
		private static Marker RP_Marker = new(MarkerType.VerticalCylinder, new Vector3(-1266.863f, -3013.068f, -49.0f), new Vector3(10f, 10f, 1f), Colors.RoyalBlue);
		private static Marker Mini_Marker = new(MarkerType.VerticalCylinder, new Vector3(-1280.206f, -3021.234f, -49.0f), new Vector3(10f, 10f, 1f), Colors.ForestGreen);
		private static Marker Gare_Marker = new(MarkerType.VerticalCylinder, new Vector3(-1267.147f, -3032.353f, -49.0f), new Vector3(10f, 10f, 1f), Colors.MediumPurple);
		private static Marker Nego_Marker = new(MarkerType.VerticalCylinder, new Vector3(-1251.566f, -3032.304f, -49.0f), new Vector3(10f, 10f, 1f), Colors.Orange);
		private static Scaleform rp = new("mp_mission_name_freemode_199");
		private static Scaleform mini = new("mp_mission_name_freemode_1999");
		private static Scaleform race = new("mp_mission_name_freemode_19999");
		private static Scaleform store = new("mp_mission_name_freemode_199999");

		public static void Init()
		{
			Client.Instance.AddTick(DrawMarkers);
		}

		private static Vector3 posRP = Vector3.Zero;
		private static Vector3 posMini = Vector3.Zero;
		private static Vector3 posGare = Vector3.Zero;
		private static Vector3 posNego = Vector3.Zero;

		private static async Task DrawMarkers()
		{
			if (!Main.spawned) return;

			if (posRP == Vector3.Zero)
			{
				posRP = await RP_Marker.Position.GetVector3WithGroundZ();
				RP_Marker.Position = posRP;
			}

			if (posMini == Vector3.Zero)
			{
				posMini = await Mini_Marker.Position.GetVector3WithGroundZ();
				Mini_Marker.Position = posMini;
			}

			if (posGare == Vector3.Zero)
			{
				posGare = await Gare_Marker.Position.GetVector3WithGroundZ();
				Gare_Marker.Position = posGare;
			}

			if (posNego == Vector3.Zero)
			{
				posNego = await Nego_Marker.Position.GetVector3WithGroundZ();
				Nego_Marker.Position = posNego;
			}

			RP_Marker.Draw();
			Mini_Marker.Draw();
			Gare_Marker.Draw();
			Nego_Marker.Draw();
			rp.CallFunction("SET_MISSION_INFO", "Immergiti nella simulazione", "~b~Server Roleplay~w~", "", "", "", true, "0/256", "", "", "");
			mini.CallFunction("SET_MISSION_INFO", "Gioca contro gli altri o a squadre", "~g~Minigiochi~w~", "", "", "", true, "0/64", "", "", "");
			race.CallFunction("SET_MISSION_INFO", "Gareggia contro tutti", "~p~Gare~w~", "", "", "", true, "0/64", "", "", "");
			store.CallFunction("SET_MISSION_INFO", "**Non si riflette sul server RolePlay**", "~o~Negozio~w~", "", "", "", true, "", "", "", "");
			rp.Render3D(RP_Marker.Position, GameplayCamera.Rotation, RP_Marker.Scale / 2);
			mini.Render3D(Mini_Marker.Position, GameplayCamera.Rotation, Mini_Marker.Scale / 2);
			race.Render3D(Gare_Marker.Position, GameplayCamera.Rotation, Gare_Marker.Scale / 2);
			store.Render3D(Nego_Marker.Position, GameplayCamera.Rotation, Nego_Marker.Scale / 2);
		}
	}
}