using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using TheLastPlanet.Client.NativeUI;
using TheLastPlanet.Shared;
using static TheLastPlanet.Client.Cache.PlayerCache;

namespace TheLastPlanet.Client.Core.Utility.HUD
{
	public class Marker
	{
		public MarkerType MarkerType { get; set; }
		public Position Position { get; set; }
		public Vector3 Direction { get; set; } = Vector3.Zero;
		public Vector3 Scale  { get; set; } = new(1.5f);
		public Color Color { get; set; }
		public bool BobUpDown { get; set; }
		public bool Rotate { get; set; }
		public bool FaceCamera { get; set; }
		public bool IsInMarker  { get; set; }

		public Marker(MarkerType type, Position position, Color color, bool bobUpDown = false, bool rotate = false, bool faceCamera = false)
		{
			MarkerType = type;
			Position = position;
			Color = color;
			BobUpDown = false;
			BobUpDown = bobUpDown;
			Rotate = rotate;
			FaceCamera = false;
			FaceCamera = faceCamera;
		}

		public Marker(MarkerType type, Position position, Vector3 scale, Color color, bool bobUpDown = false, bool rotate = false, bool faceCamera = false)
		{
			MarkerType = type;
			Position = position;
			Scale = scale;
			Color = color;
			BobUpDown = false;
			BobUpDown = bobUpDown;
			Rotate = rotate;
			FaceCamera = false;
			FaceCamera = faceCamera;
		}

		public void Draw()
		{
			World.DrawMarker(MarkerType, Position.ToVector3, Direction, Position.ToRotationVector, Scale, Color, BobUpDown, FaceCamera, Rotate);
			IsInMarker = Position.Distance(MyPlayer.Posizione) <= (Scale / 2).X || Position.Distance(MyPlayer.Posizione) <= (Scale / 2).Y || Position.Distance(MyPlayer.Posizione) <= (Scale / 2).Z;
		}
	}
}