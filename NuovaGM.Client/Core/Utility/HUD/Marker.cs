using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using TheLastPlanet.Client.MenuNativo;

namespace TheLastPlanet.Client.Core.Utility.HUD
{
	public class Marker
	{
		public MarkerType MarkerType = MarkerType.VerticalCylinder;
		public Vector3 Position = Vector3.Zero;
		public Vector3 Direction = Vector3.Zero;
		public Vector3 Rotation = Vector3.Zero;
		public Vector3 Scale = new Vector3(1.5f);
		public Color Color = Colors.WhiteSmoke;
		public bool BobUpDown = false;
		public bool Rotate = false;
		public bool FaceCamera = false;

		public Marker(MarkerType type, Vector3 position, Color color, bool bobUpDown = false, bool rotate = false, bool faceCamera = false)
		{
			MarkerType = type;
			Position = position;
			Color = color;
			BobUpDown = bobUpDown;
			Rotate = rotate;
			FaceCamera = faceCamera;
		}

		public Marker(MarkerType type, Vector3 position, Vector3 scale, Color color, bool bobUpDown = false, bool rotate = false, bool faceCamera = false)
		{
			MarkerType = type;
			Position = position;
			Scale = scale;
			Color = color;
			BobUpDown = bobUpDown;
			Rotate = rotate;
			FaceCamera = faceCamera;
		}

		public async void Draw() { World.DrawMarker(MarkerType, Position, Direction, Rotation, Scale, Color, BobUpDown, FaceCamera, Rotate); }
	}
}