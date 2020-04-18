using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using static CitizenFX.Core.Native.API;
using NuovaGM.Client.gmPrincipale;
using NuovaGM.Client.gmPrincipale.Personaggio;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Client.MenuNativo;
using NuovaGM.Client.Veicoli;

using Newtonsoft.Json;

namespace NuovaGM.Client.Veicoli
{
	static class Bagagliaio
	{
		private static bool trunkOpen = false;
		private static float trunkdist = 1.375f;

		public static void Init()
		{
			Client.Instance.AddEventHandler("lprp:bagaliaio:chiudi", new Action(ChiudiBagagliaio));
		}

		private static async void ChiudiBagagliaio()
		{
			trunkOpen = false;
			if (Game.PlayerPed.LastVehicle != null)
				Game.PlayerPed.LastVehicle.Doors[VehicleDoorIndex.Trunk].Close();
		}

		public static async Task ControlloBagagliaio()
		{
			if (!Game.PlayerPed.IsInVehicle())
			{
				Tuple<Vehicle, float> closestVeh = Funzioni.GetClosestVehicle(Game.PlayerPed.Position);
				Vehicle veh = closestVeh.Item1;
				float distance = closestVeh.Item2;
				if(distance < trunkdist)
				{
					EntityBone bone = veh.Bones["boot"];
					Vector3 bonepos = bone.Position;
					string plate = veh.Mods.LicensePlate;
					// da rimuovere
					World.DrawMarker(MarkerType.ChevronUpx1, bonepos, new Vector3(0), new Vector3(0), new Vector3(0.5f, 0.5f, 1f), Colors.Cyan, false, false, true);

					if (!trunkOpen && !Game.PlayerPed.IsInVehicle() && !HUD.MenuPool.IsAnyMenuOpen())
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per gestire il bagagliaio");
						if (Input.IsControlJustPressed(Control.Context))
						{
							trunkOpen = true;
							BaseScript.TriggerServerEvent("lprp:bagagliaio:getTrunksContents", veh.Mods.LicensePlate);
						}
					}
				}
			}

		}

	}
}
