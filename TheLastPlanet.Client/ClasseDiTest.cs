using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using Logger;
using Newtonsoft.Json;
using TheLastPlanet.Client.Core.PlayerChar;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.NativeUI;
using TheLastPlanet.Client.SessionCache;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.PlayerChar;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client
{
	internal static class ClasseDiTest
	{
		public static async void Init()
		{
		}



		public static async void Stop()
		{
			//Client.Instance.AddTick(test);
			//ClearFocus();
		}

		private static void AttivaMenu()
		{
			UIMenu test = new("Test", "test", new System.Drawing.PointF(700, 300));
			HUD.MenuPool.Add(test);
			Ped ped = Cache.MyPlayer.Ped;
			Camera a1 = World.CreateCamera(new Vector3(-1503.000f, -1143.462f, 34.670f), Vector3.Zero, GameplayCamera.FieldOfView);
			Camera a2 = World.CreateCamera(ped.GetOffsetPosition(new Vector3(0, 2f, 2f)), Vector3.Zero, GameplayCamera.FieldOfView);
			UIMenu creaCam1 = test.AddSubMenu("CreaCam1", "");
			UIMenuItem avvia = new("avvia test 1");
			creaCam1.AddItem(avvia);
			avvia.Activated += async (a, b) =>
			{
				a1.InterpTo(a2, 3000, 1, 1);

				while (a1.IsInterpolating || a2.IsInterpolating)
				{
					await BaseScript.Delay(100);
					a1.Position.SetFocus();
				}

				await BaseScript.Delay(2000);
				World.RenderingCamera = null;
				ClearFocus();
			};
			test.OnMenuStateChanged += async (oldmenu, newmenu, state) =>
			{
				if (state == MenuState.ChangeForward)
					if (newmenu == creaCam1)
					{
						World.RenderingCamera = a1;
						a1.Position.SetFocus();
					}
			};
			test.Visible = true;
		}

		private static int timer = 0;

		public static async Task test()
		{
			/*
			SET_PED_TO_RAGDOLL
			CREATE_NM_MESSAGE
			GIVE_PED_NM_MESSAGE
			*/
			/*
			b.ProcessControls();
			b.Update();
			item2.ProcessControls();
			*/
			//if (Input.IsControlJustPressed(Control.Detonate)) AttivaMenu();
		}
	}
}