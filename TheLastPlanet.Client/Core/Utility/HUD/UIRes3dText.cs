using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using TheLastPlanet.Client.MenuNativo;
using static CitizenFX.Core.Native.API;
using Font = CitizenFX.Core.UI.Font;

namespace TheLastPlanet.Client.Core.Utility.HUD
{
	public class UIRes3dText
	{
		public bool Enabled;
		public Vector3 Position = new Vector3();
		public string Text;
		public float Scale = 1f;
		public Color Color = Colors.WhiteSmoke;
		public Font Font = Font.ChaletLondon;
		public DropShadow DropShadow;

		public UIRes3dText(string caption, Vector3 position)
		{
			Text = caption;
			Position = position;
			Scale = 1.0f;
			Color = Colors.WhiteSmoke;
			Font = Font.ChaletLondon;
		}

		public UIRes3dText(string caption, Vector3 position, float scale)
		{
			Text = caption;
			Position = position;
			Scale = scale;
			Color = Colors.WhiteSmoke;
			Font = Font.ChaletLondon;
		}

		public UIRes3dText(string caption, Vector3 position, float scale, Color color)
		{
			Text = caption;
			Position = position;
			Scale = scale;
			Color = color;
			Font = Font.ChaletLondon;
		}

		public UIRes3dText(string caption, Vector3 position, float scale, Color color, Font font)
		{
			Text = caption;
			Position = position;
			Scale = scale;
			Color = color;
			Font = font;
		}

		public UIRes3dText(string caption, Vector3 position, float scale, Color color, Font font, DropShadow dropShadow)
		{
			Text = caption;
			Position = position;
			Scale = scale;
			Color = color;
			Font = font;
			DropShadow = dropShadow;
		}

		public async void Draw()
		{
			if (!Enabled) return;
			Vector3 cam = GameplayCamera.Position;
			float dist = Vector3.Distance(Position, cam);
			float _scale = 1 / dist * 20;
			float fov = 1 / GameplayCamera.FieldOfView * 100;
			float scale = _scale * fov;
			SetTextScale(0.1f * scale, 0.15f * scale);
			SetTextFont((int)Font);
			SetTextProportional(true);
			SetTextColour(Color.R, Color.G, Color.B, Color.A);
			if (DropShadow != null)
				SetTextDropshadow(DropShadow.distance, DropShadow.Color.R, DropShadow.Color.G, DropShadow.Color.B, DropShadow.Color.A);
			else
				SetTextDropshadow(0, 0, 0, 0, 255);
			SetTextEdge(2, 0, 0, 0, 150);
			SetTextDropShadow();
			SetTextOutline();
			SetTextCentre(true);
			SetDrawOrigin(Position.X, Position.Y, Position.Z, 0);
			BeginTextCommandDisplayText("STRING");
			AddTextComponentSubstringPlayerName(Text);
			EndTextCommandDisplayText(0, 0);
			ClearDrawOrigin();
		}
	}

	public class DropShadow
	{
		public int distance;
		public Color Color;
	}
}