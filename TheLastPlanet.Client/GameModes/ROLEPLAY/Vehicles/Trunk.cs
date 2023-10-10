using System;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.GameMode.ROLEPLAY.Vehicles
{
    internal static class Trunk
    {
        private static bool trunkOpen = false;
        private static float trunkdist = 1.375f;

        public static void Init() { Client.Instance.AddEventHandler("lprp:bagaliaio:chiudi", new Action(CloseTrunk)); }

        private static async void CloseTrunk()
        {
            trunkOpen = false;
            if (Cache.PlayerCache.MyPlayer.Ped.LastVehicle != null) Cache.PlayerCache.MyPlayer.Ped.LastVehicle.Doors[VehicleDoorIndex.Trunk].Close();
        }

        public static async Task TrunkCheck()
        {
            Ped playerPed = Cache.PlayerCache.MyPlayer.Ped;

            if (!Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle)
            {
                Tuple<Vehicle, float> closestVeh = playerPed.GetClosestVehicleWithDistance();
                Vehicle veh = closestVeh.Item1;
                float distance = closestVeh.Item2;

                if (distance < trunkdist)
                {
                    EntityBone bone = veh.Bones["boot"];
                    Vector3 bonepos = bone.Position;
                    string plate = veh.Mods.LicensePlate;
                    // da rimuovere
                    World.DrawMarker(MarkerType.ChevronUpx1, bonepos, new Vector3(0), new Vector3(0), new Vector3(0.5f, 0.5f, 1f), Colors.Cyan, false, false, true);

                    if (!trunkOpen && !Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle && !MenuHandler.IsAnyMenuOpen)
                    {
                        HUD.ShowHelp("Press ~INPUT_CONTEXT~ to manage the trunk");

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