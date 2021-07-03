using CitizenFX.Core;
using TheLastPlanet.Client.Core.Utility.HUD;

namespace TheLastPlanet.Client.MAINLOBBY
{
	public class BucketMarker
	{
		public Marker Marker;
		public string Name;
		public Scaleform Scaleform;

		public BucketMarker(Marker marker, string name, string scaleform)
		{
			Marker = marker;
			Name = name;
			Scaleform = new Scaleform(scaleform);
		}

		public void Draw()
		{
			Marker.Draw();

			if (!Scaleform.IsLoaded) return;
			Scaleform.Render3D(Marker.Position.ToVector3, GameplayCamera.Rotation, (Marker.Scale / 2).ToVector3);
		}
	}
}
