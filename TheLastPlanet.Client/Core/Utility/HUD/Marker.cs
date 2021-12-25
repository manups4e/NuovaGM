using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using ScaleformUI;
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
		public bool BobUpDown = false;
		public bool Rotate = false;
		public bool FaceCamera = false;
		public bool IsInMarker  = false;

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

		public void Draw(bool useZ = false)
		{
			if (Game.IsPaused) return;
			World.DrawMarker(MarkerType, Position.ToVector3, Direction, Position.ToRotationVector, Scale, Color, BobUpDown, FaceCamera, Rotate);
			if (useZ)
			{
				float distanceSquared = Position.ToVector3.DistanceToSquared(MyPlayer.Posizione.ToVector3);
				IsInMarker = (distanceSquared < Math.Pow(Scale.X / 2, 2) || distanceSquared < Math.Pow(Scale.Y / 2, 2)) || distanceSquared < Math.Pow(Scale.Z / 2, 2);
			}
			else
			{
				var pos = Position.ToVector3.DistanceToSquared2D(MyPlayer.Posizione.ToVector3);
				IsInMarker = pos <= Math.Pow(Scale.X / 2, 2) || pos <= Math.Pow(Scale.Y / 2, 2);
			}
		}
	}
}