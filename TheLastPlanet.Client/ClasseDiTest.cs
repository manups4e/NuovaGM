using CitizenFX.Core;
using CitizenFX.Core.Native;
using ScaleformUI;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.IPLs;
using TheLastPlanet.Shared;

namespace TheLastPlanet.Client
{
	internal static class ClasseDiTest
	{
		public static void Init()
		{
			Client.Instance.AddTick(TestTick);
			IPLInstance.CayoPericoIsland.Enabled = false;
		}

		static bool pp = false;
		static Marker dummyMarker = new(MarkerType.VerticalCylinder, WorldProbe.CrossairRaycastResult.HitPosition.ToPosition(), Colors.Blue);
		private static async Task TestTick()
        {
			RaycastResult res = WorldProbe.CrossairRaycastResult;
			Vector3 direction = res.HitPosition;
			dummyMarker.Color = Colors.Red;
			dummyMarker.Draw();
			dummyMarker.Position = direction.ToPosition();
			float z = 0;
			API.GetGroundZFor_3dCoord(direction.X, direction.Y, direction.Z, ref z, false);
			if (z != 0 && res.DitHit)
				dummyMarker.Position = new(dummyMarker.Position.X, dummyMarker.Position.Y, z);

			//Client.Logger.Debug(IplManager.Global.ToJson());
			if (Input.IsControlJustPressed(Control.Detonate, PadCheck.Keyboard, ControlModifier.Shift) && !HUD.MenuPool.IsAnyMenuOpen)
			{
				//AttivaMenu();
			}
		}

		public static void Stop()
		{
			Client.Instance.RemoveTick(TestTick);
		}

		private static void AttivaMenu()
		{
			JobSelectionData data = new();
			data.SetTitle("TEST");
			data.SetVotes(0, 3, "TEST");
			data.Cards = new List<JobSelectionCard>();
			for (int i=0; i<6; i++)
            {
				var card = new JobSelectionCard("Test", "test", "", "", 12, 15, JobSelectionCardIcon.CAPTURE_THE_FLAG, HudColor.HUD_COLOUR_FREEMODE, 2, new List<JobSelectionCardDetail>()
				 {
					 new JobSelectionCardDetail(JobSelectionCardDetailType.WITH_ICON, "Test Left", "Test Right"),
					 new JobSelectionCardDetail(JobSelectionCardDetailType.WITH_ICON, "Test Left", "Test Right"),
					 new JobSelectionCardDetail(JobSelectionCardDetailType.WITH_ICON, "Test Left", "Test Right")
				 });

				data.AddCard(card);
            }
			data.Buttons = new List<JobSelectionButton>()
			{
				new JobSelectionButton("Test1", null),
				new JobSelectionButton("Test2", null),
				new JobSelectionButton("Test3", null),
			};
			NativeUIScaleform.JobMissionSelection.JobData = data;
			NativeUIScaleform.JobMissionSelection.Enabled = true;
		}
	}
}