using CitizenFX.Core;
using CitizenFX.Core.Native;
using static CitizenFX.Core.Native.API;
using TheLastPlanet.Client.Core;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.MenuNativo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.Veicoli;

namespace TheLastPlanet.Client.Manager
{
	internal static class DevManager
	{
		private static bool attivo = false;
		private static readonly Color DefaultCrosshair = Color.FromArgb(120, 120, 120);
		private static readonly Color ActiveCrosshair = Color.FromArgb(255, 255, 255);

		private static readonly Vector2 DefaultPos = new Vector2(0.6f, 0.5f);

		public static void Init() { Client.Instance.AddEventHandler("lprp:sviluppatoreOn", new Action<bool>(Sviluppatore)); }

		public static void Sviluppatore(bool toggle)
		{
			if (toggle)
			{
				if (Main.spawned)
					Client.Instance.AddTick(OnTickSviluppo);
				else
					HUD.ShowNotification("Devi essere spawnato prima! Scegli un personaggio!!");
			}
			else
			{
				Client.Instance.RemoveTick(OnTickSviluppo);
			}
		}

		public static async Task OnTickSviluppo()
		{
			Ped pl = Cache.PlayerPed;
			AsyncRaycastResult ray = await WorldProbe.GamePlayCamCrosshairRaycast();
			HUD.DrawText(0.4f, 0.925f, $"~o~Posizione~w~: {(Cache.Char.StatiPlayer.InVeicolo ? pl.CurrentVehicle.Position : pl.Position)} H:{(Cache.Char.StatiPlayer.InVeicolo ? pl.CurrentVehicle.Heading : pl.Heading)}");
			HUD.DrawText(0.4f, 0.95f, $"Rotazione: {(Cache.Char.StatiPlayer.InVeicolo ? pl.CurrentVehicle.Rotation : pl.Rotation)}");
			HUD.DrawText(0.4f, 0.90f, $"Interior Id = {GetInteriorFromGameplayCam()}");
			HUD.DrawText(0.7f, 0.90f, $"~b~GamePlayCam Posizione~w~ = {GameplayCamera.Position}");
			HUD.DrawText(0.7f, 0.925f, $"~r~GamePlayCam punta a~w~ = {ray.HitPosition}");

			if (pl.IsAiming)
			{
				Entity ent = Cache.Player.GetTargetedEntity();
				if (ent.Exists()) HUD.DrawText3D(ent.GetOffsetPosition(new Vector3(0, 0, 1)), Colors.DarkSeaGreen, "Hash = " + ent.Model.Hash);
			}

			if (Cache.Char.StatiPlayer.InVeicolo)
			{
				Vehicle veicolo = new Vehicle(GetVehiclePedIsIn(PlayerPedId(), false));
				VehProp props = await veicolo.GetVehicleProperties();
				Vector3 entityPos = veicolo.Position;
				Vector2 pos = Funzioni.WorldToScreen(entityPos);
				if (pos.X <= 0f || pos.Y <= 0f || pos.X >= 1f || pos.Y >= 1f) pos = DefaultPos;
				float dist = Vector3.Distance(pl.Position, entityPos);
				float offsetX = MathUtil.Clamp((1f - dist / 100f) * 0.1f, 0f, 0.1f);
				pos.X += offsetX;
				Dictionary<string, string> data = new Dictionary<string, string>
				{
					["Modello Veicolo"] = GetDisplayNameFromVehicleModel((uint)props.Model),
					["Hash Modello"] = $"{(uint)props.Model}",
					["Hash Modello (Hex)"] = $"0x{(uint)props.Model:X}",
					["Targa Veicolo"] = props.Plate,
					[""] = "",
					["Salute Motore"] = $"{Math.Round(props.EngineHealth, 2)} / 1,000.0",
					["Salute Carrozzeria"] = $"{Math.Round(props.BodyHealth, 2)} / 1,000.0",
					["Velocità Attuale"] = $"{Math.Round(veicolo.Speed * 3.6f, 0)} KM/H",
					["RPM / m"] = $"{Math.Round(veicolo.CurrentRPM * 1000, 2)}",
					["Marcia Corrente"] = $"{veicolo.CurrentGear}",
					["Accelerazione"] = $"{Math.Round(veicolo.Acceleration, 3)}",
					["Frenata Massima"] = $"{Math.Round(veicolo.MaxBraking, 3)}",
					["Trazione Massima"] = $"{Math.Round(veicolo.MaxTraction, 3)}"
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
				foreach (Prop p in World.GetAllProps())
					if (p.IsInRangeOf(Cache.Char.posizione.ToVector3(), 20f))
						HUD.DrawText3D(p.Position, Colors.Aquamarine, Enum.GetName(typeof(ObjectHash), (uint)p.Model.Hash));
				foreach (Ped p in World.GetAllPeds())
					if (p.IsInRangeOf(Cache.Char.posizione.ToVector3(), 20f) && p != Cache.PlayerPed)
						HUD.DrawText3D(p.Position, Colors.Orange, Enum.GetName(typeof(PedHash), (uint)p.Model.Hash));
				foreach (Vehicle p in World.GetAllVehicles())
					if (p.IsInRangeOf(Cache.Char.posizione.ToVector3(), 20f))
						HUD.DrawText3D(p.Position, Colors.Green, Enum.GetName(typeof(VehicleHash), (uint)p.Model.Hash));
			}

			await Task.FromResult(0);
		}
	}
}