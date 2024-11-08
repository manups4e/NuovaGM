﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using TheLastPlanet.Shared.Vehicles;

namespace TheLastPlanet.Client.AdminAC
{
    internal static class DevManager
    {
        private static readonly Color DefaultCrosshair = Color.FromArgb(120, 120, 120);
        private static readonly Color ActiveCrosshair = Color.FromArgb(255, 255, 255);

        private static readonly PointF DefaultPos = new(0.6f, 0.5f);

        public static void Init()
        {
            EventDispatcher.Mount("lprp:sviluppatoreOn", new Action<bool>(Sviluppatore));
        }

        public static void Sviluppatore(bool toggle)
        {
            if (toggle)
            {
                Client.Instance.AddTick(OnTickSviluppo);
            }
            else
            {
                Client.Instance.RemoveTick(OnTickSviluppo);
            }
        }

        public static async Task OnTickSviluppo()
        {
            Ped pl = Cache.PlayerCache.MyPlayer.Ped;
            RaycastResult Crossair = WorldProbe.CrossairRaycastResult;
            //TODO: in worldprobe un metodo fisso da cui prendere i valori in modo syncrono
            HUD.DrawText(0.4f, 0.925f, $"~o~Position~w~: {(Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle ? pl.CurrentVehicle.Position : pl.Position)} H:{(Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle ? pl.CurrentVehicle.Heading : pl.Heading)}");
            HUD.DrawText(0.4f, 0.95f, $"Rotation: {(Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle ? pl.CurrentVehicle.Rotation : pl.Rotation)}");
            HUD.DrawText(0.4f, 0.90f, $"Interior Id = {GetInteriorFromGameplayCam()}");
            HUD.DrawText(0.7f, 0.90f, $"~b~GamePlayCam Position~w~ = {GameplayCamera.Position}");
            HUD.DrawText(0.7f, 0.925f, $"~r~GamePlayCam Point to~w~ = {Crossair.HitPosition}");

            if (Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle)
            {
                Vehicle veicolo = new(GetVehiclePedIsIn(PlayerPedId(), false));
                VehProp props = await veicolo.GetVehicleProperties();
                Vector3 entityPos = veicolo.Position;
                PointF pos = Functions.WorldToScreen(entityPos);
                if (pos.X <= 0f || pos.Y <= 0f || pos.X >= 1f || pos.Y >= 1f) pos = DefaultPos;
                float dist = Vector3.Distance(pl.Position, entityPos);
                float offsetX = MathUtil.Clamp((1f - dist / 100f) * 0.1f, 0f, 0.1f);
                pos.X += offsetX;
                Dictionary<string, string> data = new()
                {
                    ["Vehicle Model"] = GetDisplayNameFromVehicleModel((uint)props.Model),
                    ["Hash Model"] = $"{(uint)props.Model}",
                    ["Hash Model (Hex)"] = $"0x{(uint)props.Model:X}",
                    ["Plate"] = props.Plate,
                    [""] = string.Empty,
                    ["Engine Health"] = $"{Math.Round(props.EngineHealth, 2)} / 1,000.0",
                    ["Body Health"] = $"{Math.Round(props.BodyHealth, 2)} / 1,000.0",
                    ["Actual speed"] = $"{Math.Round(veicolo.Speed * 3.6f, 0)} KM/H",
                    ["RPM / m"] = $"{Math.Round(veicolo.CurrentRPM * 1000, 2)}",
                    ["Current Gear"] = $"{veicolo.CurrentGear}",
                    ["Acceleration"] = $"{Math.Round(veicolo.Acceleration, 3)}",
                    ["Max Break"] = $"{Math.Round(veicolo.MaxBraking, 3)}",
                    ["Max Traction"] = $"{Math.Round(veicolo.MaxTraction, 3)}"
                };
                DrawRect(pos.X + 0.12f, pos.Y, 0.24f, data.Count * 0.024f + 0.048f, 0, 0, 0, 120);
                float offsetY = data.Count * 0.012f;
                pos.Y -= offsetY;
                pos.X += 0.13f;

                // Draw data
                foreach (KeyValuePair<string, string> entry in data)
                {
                    if (!string.IsNullOrEmpty(entry.Value)) HUD.DrawText(pos.X, pos.Y, $"{entry.Key}: {entry.Value}");
                    pos.Y += 0.024f;
                }
            }
            else
            {
                if (pl.IsAiming)
                {
                    Entity ent = Cache.PlayerCache.MyPlayer.Player.GetTargetedEntity();
                    if (ent.Exists()) HUD.DrawText3D(ent.GetOffsetPosition(new Vector3(0, 0, 1)).ToPosition(), Colors.DarkSeaGreen, "Hash = " + ent.Model.Hash);
                }
                else
                {
                    foreach (Prop p in World.GetAllProps())
                        if (p.IsInRangeOf(Cache.PlayerCache.MyPlayer.Position.ToVector3, 20f) && HasEntityClearLosToEntity(p.Handle, Cache.PlayerCache.MyPlayer.Ped.Handle, 17))
                            HUD.DrawText3D(p.Position.ToPosition(), Colors.Aquamarine, Enum.GetName(typeof(ObjectHash), (uint)p.Model.Hash));
                    foreach (Ped p in World.GetAllPeds())
                        if (p.IsInRangeOf(Cache.PlayerCache.MyPlayer.Position.ToVector3, 20f) && p != Cache.PlayerCache.MyPlayer.Ped && HasEntityClearLosToEntity(p.Handle, Cache.PlayerCache.MyPlayer.Ped.Handle, 17))
                            HUD.DrawText3D(p.Position.ToPosition(), Colors.Orange, Enum.GetName(typeof(PedHash), (uint)p.Model.Hash));
                    foreach (Vehicle p in World.GetAllVehicles())
                        if (p.IsInRangeOf(Cache.PlayerCache.MyPlayer.Position.ToVector3, 20f) && HasEntityClearLosToEntity(p.Handle, Cache.PlayerCache.MyPlayer.Ped.Handle, 17))
                            HUD.DrawText3D(p.Position.ToPosition(), Colors.Green, Enum.GetName(typeof(VehicleHash), (uint)p.Model.Hash));
                }
            }

            await Task.FromResult(0);
        }
    }
}