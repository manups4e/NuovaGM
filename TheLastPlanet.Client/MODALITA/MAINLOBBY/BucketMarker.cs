using CitizenFX.Core;
using CitizenFX.Core.Native;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Shared;

namespace TheLastPlanet.Client.MODALITA.MAINLOBBY
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
			Position p = Marker.Position - Cache.PlayerCache.MyPlayer.Ped.Position.ToPosition();
			var heading = API.GetHeadingFromVector_2d(p.X, p.Y);
			Scaleform.Render3D(Marker.Position.ToVector3, new(0, 0, -heading), Marker.Scale / 2);
		}
	}
}	
