using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using Logger;
using TheLastPlanet.Client.Core.PlayerChar;
using TheLastPlanet.Client.Core.Utility.HUD;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client.Core.BucketChooser
{
	public class BucketMarker
	{
		public Marker Marker;
		public string Name;
		public Scaleform Scaleform;
		private string scale;

		public BucketMarker(Marker marker, string name, string scaleform)
		{
			Marker = marker;
			Name = name;
			Scaleform = new Scaleform(scaleform);
			scale = scaleform;
		}

		public async void Draw()
		{
			Client.Logger.Debug($"Scaleform => {scale} => isValid = {Scaleform.IsValid}, isLoaded = {Scaleform.IsLoaded}");
			while (!Scaleform.IsLoaded) await BaseScript.Delay(10);
			Scaleform.Render3D(Marker.Position, GameplayCamera.Rotation, Marker.Scale / 2);
			Marker.Draw();
		}
	}
}
