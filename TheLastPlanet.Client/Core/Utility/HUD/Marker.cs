using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using TheLastPlanet.Client.NativeUI;
using TheLastPlanet.Shared;
using static TheLastPlanet.Client.SessionCache.Cache;

namespace TheLastPlanet.Client.Core.Utility.HUD
{
	public class Marker
	{
		public MarkerType MarkerType = MarkerType.VerticalCylinder;
		public Position Position;
		public Position Direction = Position.Zero;
		public Position Rotation = Position.Zero;
		public Position Scale = new(1.5f);
		public Color Color;
		public bool BobUpDown;
		public bool Rotate;
		public bool FaceCamera;
		public bool IsInMarker = false;

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

		public Marker(MarkerType type, Position position, Position scale, Color color, bool bobUpDown = false, bool rotate = false, bool faceCamera = false)
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
			World.DrawMarker(MarkerType, Position.ToVector3, Direction.ToVector3, Rotation.ToVector3, Scale.ToVector3, Color, BobUpDown, FaceCamera, Rotate);
			IsInMarker = Position.Distance(MyPlayer.Posizione) <= Position.X / 2;
		}
	}
}