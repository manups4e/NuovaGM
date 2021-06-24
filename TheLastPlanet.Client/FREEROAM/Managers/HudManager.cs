using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using TheLastPlanet.Client.SessionCache;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client.FreeRoam.Managers
{
    static class HudManager
    {
        public static bool MapEnabled = false;
        public static bool HudEnabled = false;
        public static bool ShowPlayerBlips = true;

        private static List<int> PlayerBlips = new List<int>();

        public static void Init()
        {
            Client.Instance.AddTick(OnHudTick);

            //Client.Instance.AddEventHandler("onClientResourceStart", new Action<string>(OnClientResourceStart)); trovare nuovo nome evento
            Client.Instance.Eventi.Mount("worldeventsManage.Client:GetLevelXp", new Action<int, int>(OnGetLevelXp));
        }

        private static async Task OnHudTick()
        {
            Screen.Hud.HideComponentThisFrame(HudComponent.AreaName);
            Screen.Hud.HideComponentThisFrame(HudComponent.StreetName);
            Screen.Hud.HideComponentThisFrame(HudComponent.VehicleName);
            /*
            if (Hud.WarningDisplayed)
            {
                Hud.ShowWarningMessage();
            }
            */
            DrawVehicleHud();

            if (ShowPlayerBlips)
            {
                DrawPlayerBlips();
            }

            await Task.FromResult(0);
        }

        private static void OnClientResourceStart(string resource)
        {
            if (GetCurrentResourceName() != resource) { return; }

            DisplayRadar(true);
        }

        public static void OnEnableMap(bool enable)
        {
            MapEnabled = enable;
            DisplayRadar(MapEnabled);
        }

        private static void OnGetLevelXp(int level, int xp)
        {
            Cache.MyPlayer.User.FreeRoamChar.Level = level;
            Cache.MyPlayer.User.FreeRoamChar.TotalXp = xp;
            Debug.WriteLine($"OnGetLevelXp | Level [{level}] | XP [{xp}]");
            BaseScript.TriggerEvent("worldeventsManage.Client:UpdatedLevel", level, false); // da aggiornare perché non esiste nel codice
        }

        private static void DrawVehicleHud()
        {
            if (Cache.MyPlayer.Ped.CurrentVehicle != null)
            {
                double vehicleSpeed = Math.Round(Cache.MyPlayer.Ped.CurrentVehicle.Speed * 3.6);
                SetTextFont(0);
                SetTextProportional(true);
                SetTextScale(0.0f, 0.35f);
                SetTextColour(255, 255, 255, 255);
                SetTextDropshadow(0, 0, 0, 0, 255);
                SetTextEdge(1, 0, 0, 0, 255);
                SetTextDropShadow();
                SetTextOutline();
                SetTextWrap(0f, 0.125f);
                SetTextRightJustify(true);
                SetTextEntry("STRING");
                AddTextComponentString(vehicleSpeed.ToString());
                DrawText(1f- 0.124f, 0.945f);

                SetTextFont(0);
                SetTextProportional(true);
                SetTextScale(0.0f, 0.35f);
                SetTextColour(255, 255, 255, 255);
                SetTextDropshadow(0, 0, 0, 0, 255);
                SetTextEdge(1, 0, 0, 0, 255);
                SetTextDropShadow();
                SetTextOutline();
                SetTextEntry("STRING");
                AddTextComponentString("km/h");
                DrawText(0.1275f, 0.945f);

            }
        }
        private static void DrawPlayerBlips()
        {
            for (int i = 0; i < 64; i++)
            {
                if (!NetworkIsPlayerActive(i) || GetPlayerPed(i) == PlayerPedId()) continue;
                var player = GetPlayerPed(i);
                var blip = GetBlipFromEntity(player);
                if (!DoesBlipExist(blip))
                {
                    blip = AddBlipForEntity(player);
                    SetBlipSprite(blip, 1);
                    ShowHeadingIndicatorOnBlip(blip, true);
                }
                else
                {
                    var playerVeh = GetVehiclePedIsUsing(player);
                    var blipSprite = GetBlipSprite(blip);
                    if (playerVeh != 0)
                    {
                        var currentVehicle = new Vehicle(playerVeh);
                        switch (currentVehicle.ClassType)
                        {
                            case VehicleClass.Helicopters:
                                if (blipSprite != (int)BlipSprite.HelicopterAnimated)
                                {
                                    SetBlipSprite(blip, (int)BlipSprite.HelicopterAnimated);
                                    ShowHeadingIndicatorOnBlip(blip, false);
                                }

                                break;
                            case VehicleClass.Planes:
                                if (currentVehicle.Model == VehicleHash.Besra ||
                                    currentVehicle.Model == VehicleHash.Lazer ||
                                    currentVehicle.Model == VehicleHash.Hydra)
                                {
                                    if (blipSprite != 424)
                                    {
                                        SetBlipSprite(blip, 424);
                                        ShowHeadingIndicatorOnBlip(blip, false);
                                    }
                                }
                                else if (blipSprite != (int)BlipSprite.Plane)
                                {
                                    SetBlipSprite(blip, (int)BlipSprite.Plane);
                                    ShowHeadingIndicatorOnBlip(blip, false);
                                }

                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        if (blipSprite != (int)BlipSprite.Standard)
                        {
                            SetBlipSprite(blip, (int)BlipSprite.Standard);
                            ShowHeadingIndicatorOnBlip(blip, true);
                        }
                    }

                    SetBlipRotation(blip, (int)Math.Ceiling(GetEntityHeading(player)));
                    SetBlipNameToPlayerName(blip, i);
                    SetBlipScale(blip, 0.85f);
                    if (Game.IsPaused)
                    {
                        SetBlipAlpha(blip, 255);
                    }
                    else
                    {
                        var playerCoords = GetEntityCoords(player, true);
                        var myCoords = Cache.MyPlayer.Ped.Position;

                        var alpha = 0;
                        if (myCoords.DistanceToSquared(playerCoords) < 40000f)
                        {
                            alpha = 255;
                        }

                        SetBlipAlpha(blip, alpha);
                    }
                }
            }
        }
    }
}
