﻿using CitizenFX.Core;
using CitizenFX.Core.Native;
using static CitizenFX.Core.Native.API;
using NuovaGM.Client.gmPrincipale;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Client.MenuNativo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using static NuovaGM.Shared.Veicoli.Modifiche;

namespace NuovaGM.Client.Manager
{
	static class DevManager
	{
		private static bool attivo = false;
		private static readonly Color DefaultCrosshair = Color.FromArgb(120, 120, 120);
		private static readonly Color ActiveCrosshair = Color.FromArgb(255, 255, 255);

		private static readonly Vector2 DefaultPos = new Vector2(0.6f, 0.5f);

		public static void Init()
		{
			Client.GetInstance.RegisterEventHandler("lprp:sviluppatoreOn", new Action<bool>(Sviluppatore));
		}

		public static void Sviluppatore(bool toggle)
		{
			if (toggle)
			{
				if (Main.spawned)
					Client.GetInstance.RegisterTickHandler(OnTickSviluppo);
				else
					HUD.ShowNotification("Devi essere spawnato prima! Scegli un personaggio!!");
			}
			else
				Client.GetInstance.DeregisterTickHandler(OnTickSviluppo);
		}

		public static async Task OnTickSviluppo()
		{
			HUD.DrawText(0.4f, 0.95f, $"Posizione: X = {Math.Round(Game.PlayerPed.Position.X, 3)}, Y = {Math.Round(Game.PlayerPed.Position.Y, 3)}, Z = {Math.Round(Game.PlayerPed.Position.X, 3)}, Heading = {Math.Round(Game.PlayerPed.Heading, 3)}");
			if (Game.PlayerPed.IsInVehicle())
			{
				Vehicle veicolo = new Vehicle(GetVehiclePedIsIn(PlayerPedId(), false));
				VehProp props = Funzioni.GetVehicleProperties(veicolo);
				var entityPos = veicolo.Position;
				var pos = Funzioni.WorldToScreen(entityPos);
				if (pos.X <= 0f || pos.Y <= 0f || pos.X >= 1f || pos.Y >= 1f) pos = DefaultPos;
				var dist = (float)Math.Sqrt(Game.PlayerPed.Position.DistanceToSquared(entityPos));
				var offsetX = MathUtil.Clamp((1f - dist / 100f) * 0.1f, 0f, 0.1f);
				pos.X += offsetX;
				var data = new Dictionary<string, string>
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
				var offsetY = data.Count * 0.012f;
				pos.Y -= offsetY;
				pos.X += 0.13f;
				// Draw data
				foreach (var entry in data)
				{
					if (!string.IsNullOrEmpty(entry.Value))
						HUD.DrawText(pos.X, pos.Y, $"{entry.Key}: {entry.Value}");
					pos.Y += 0.024f;
				}
			}
			else
			{

			}
			await Task.FromResult(0);
		}
	}
}
