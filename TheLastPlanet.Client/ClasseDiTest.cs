using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using Logger;
using Newtonsoft.Json;
using TheLastPlanet.Client.Core.PlayerChar;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.MenuNativo;
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
			Client.Instance.AddTick(test);
		}

		private static void AttivaMenu()
		{
			UIMenu test = new("Test", "test", new System.Drawing.PointF(700, 300));
			HUD.MenuPool.Add(test);
			Ped ped = Cache.MyPlayer.Ped;
			UIMenu ActivePose = test.AddSubMenu("ActivePose");
			UIMenu ApplyImpulse = test.AddSubMenu("ApplyImpulse");
			UIMenu ApplyBulletImpulse = test.AddSubMenu("ApplyBulletImpulse");
			UIMenu BodyRelax = test.AddSubMenu("BodyRelax");
			UIMenu ConfigureBalance = test.AddSubMenu("ConfigureBalance");
			UIMenu ConfigureBalanceReset = test.AddSubMenu("ConfigureBalanceReset");
			UIMenu ConfigureSelfAvoidance = test.AddSubMenu("ConfigureSelfAvoidance");
			UIMenu ConfigureBullets = test.AddSubMenu("ConfigureBullets");
			UIMenu ConfigureLimits = test.AddSubMenu("ConfigureLimits");
			UIMenu ConfigureSoftLimit = test.AddSubMenu("ConfigureSoftLimit");
			UIMenu ConfigureShotInjuredArm = test.AddSubMenu("ConfigureShotInjuredArm");
			UIMenu ConfigureShotInjuredLeg = test.AddSubMenu("ConfigureShotInjuredLeg");
			UIMenu DefineAttachedObject = test.AddSubMenu("DefineAttachedObject");
			UIMenu ForceToBodyPart = test.AddSubMenu("ForceToBodyPart");
			UIMenu LeanInDirection = test.AddSubMenu("LeanInDirection");
			UIMenu LeanRandom = test.AddSubMenu("LeanRandom");
			UIMenu LeanToPosition = test.AddSubMenu("LeanToPosition");
			UIMenu LeanTowardsObject = test.AddSubMenu("LeanTowardsObject");
			UIMenu HipsLeanInDirection = test.AddSubMenu("HipsLeanInDirection");
			UIMenu HipsLeanRandom = test.AddSubMenu("HipsLeanRandom");
			UIMenu HipsLeanToPosition = test.AddSubMenu("HipsLeanToPosition");
			UIMenu HipsLeanTowardsObject = test.AddSubMenu("HipsLeanTowardsObject");
			UIMenu ForceLeanInDirection = test.AddSubMenu("ForceLeanInDirection");
			UIMenu ForceLeanRandom = test.AddSubMenu("ForceLeanRandom");
			UIMenu ForceLeanToPosition = test.AddSubMenu("ForceLeanToPosition");
			UIMenu ForceLeanTowardsObject = test.AddSubMenu("ForceLeanTowardsObject");
			UIMenu SetStiffness = test.AddSubMenu("SetStiffness");
			UIMenu SetMuscleStiffness = test.AddSubMenu("SetMuscleStiffness");
			UIMenu SetWeaponMode = test.AddSubMenu("SetWeaponMode");
			UIMenu RegisterWeapon = test.AddSubMenu("RegisterWeapon");
			UIMenu ShotRelax = test.AddSubMenu("ShotRelax");
			UIMenu FireWeapon = test.AddSubMenu("FireWeapon");
			UIMenu ConfigureConstraints = test.AddSubMenu("ConfigureConstraints");
			UIMenu StayUpright = test.AddSubMenu("StayUpright");
			UIMenu StopAllBehaviours = test.AddSubMenu("StopAllBehaviours");
			UIMenu SetCharacterStrength = test.AddSubMenu("SetCharacterStrength");
			UIMenu SetCharacterHealth = test.AddSubMenu("SetCharacterHealth");
			UIMenu SetFallingReaction = test.AddSubMenu("SetFallingReaction");
			UIMenu SetCharacterUnderwater = test.AddSubMenu("SetCharacterUnderwater");
			UIMenu SetCharacterCollisions = test.AddSubMenu("SetCharacterCollisions");
			UIMenu SetCharacterDamping = test.AddSubMenu("SetCharacterDamping");
			UIMenu SetFrictionScale = test.AddSubMenu("SetFrictionScale");
			UIMenu AnimPose = test.AddSubMenu("AnimPose");
			UIMenu ArmsWindmill = test.AddSubMenu("ArmsWindmill");
			UIMenu ArmsWindmillAdaptive = test.AddSubMenu("ArmsWindmillAdaptive");
			UIMenu BalancerCollisionsReaction = test.AddSubMenu("BalancerCollisionsReaction");
			UIMenu BodyBalance = test.AddSubMenu("BodyBalance");
			UIMenu BodyFoetal = test.AddSubMenu("BodyFoetal");
			UIMenu BodyRollUp = test.AddSubMenu("BodyRollUp");
			UIMenu BodyWrithe = test.AddSubMenu("BodyWrithe");
			UIMenu BraceForImpact = test.AddSubMenu("BraceForImpact");
			UIMenu Buoyancy = test.AddSubMenu("Buoyancy");
			UIMenu CatchFall = test.AddSubMenu("CatchFall");
			UIMenu Electrocute = test.AddSubMenu("Electrocute");
			UIMenu FallOverWall = test.AddSubMenu("FallOverWall");
			UIMenu Grab = test.AddSubMenu("Grab");
			UIMenu HeadLook = test.AddSubMenu("HeadLook");
			UIMenu HighFall = test.AddSubMenu("HighFall");
			UIMenu IncomingTransforms = test.AddSubMenu("IncomingTransforms");
			UIMenu InjuredOnGround = test.AddSubMenu("InjuredOnGround");
			UIMenu Carried = test.AddSubMenu("Carried");
			UIMenu Dangle = test.AddSubMenu("Dangle");
			UIMenu OnFire = test.AddSubMenu("OnFire");
			UIMenu PedalLegs = test.AddSubMenu("PedalLegs");
			UIMenu PointArm = test.AddSubMenu("PointArm");
			UIMenu PointGun = test.AddSubMenu("PointGun");
			UIMenu PointGunExtra = test.AddSubMenu("PointGunExtra");
			UIMenu RollDownStairs = test.AddSubMenu("RollDownStairs");
			UIMenu Shot = test.AddSubMenu("Shot");
			UIMenu ShotNewBullet = test.AddSubMenu("ShotNewBullet");
			UIMenu ShotSnap = test.AddSubMenu("ShotSnap");
			UIMenu ShotShockSpin = test.AddSubMenu("ShotShockSpin");
			UIMenu ShotFallToKnees = test.AddSubMenu("ShotFallToKnees");
			UIMenu ShotFromBehind = test.AddSubMenu("ShotFromBehind");
			UIMenu ShotInGuts = test.AddSubMenu("ShotInGuts");
			UIMenu ShotHeadLook = test.AddSubMenu("ShotHeadLook");
			UIMenu ShotConfigureArms = test.AddSubMenu("ShotConfigureArms");
			UIMenu SmartFall = test.AddSubMenu("SmartFall");
			UIMenu StaggerFall = test.AddSubMenu("StaggerFall");
			UIMenu Teeter = test.AddSubMenu("Teeter");
			UIMenu UpperBodyFlinch = test.AddSubMenu("UpperBodyFlinch");
			UIMenu Yanked = test.AddSubMenu("Yanked");
			test.OnMenuStateChanged += async (oldmenu, newmenu, state) =>
			{
				if(state == MenuState.ChangeForward)
				{

				}
			};
			test.Visible = true;
		}

		/*
		static TabView b = new TabView("New");
		static List<UIMenuItem> players = new List<UIMenuItem>()
				{
					new UIMenuItem(Cache.Player.Name)
				};
		static TabInteractiveListItem item2 = new TabInteractiveListItem("Item 2", GetPlayers);
		*/
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
			if (Input.IsControlJustPressed(Control.Detonate))
			{
				/*
				SetPedToRagdoll(PlayerPedId(), 4000, 5000, 1, true, true, false);
				CreateNmMessage(true, 0); // stopAllBehaviours - Stop all other behaviours, in case the Ped is already doing some Euphoria stuff.  
				GivePedNmMessage(PlayerPedId()); // Dispatch message to Ped.  
				CreateNmMessage(true, 1151); // staggerFall - Attempt to walk while falling.  
				GivePedNmMessage(PlayerPedId()); // Dispatch message to Ped.  
				CreateNmMessage(true, 372); // armsWindmill - Swing arms around.  
				GivePedNmMessage(PlayerPedId()); // Dispatch message to Ped.  
				SetPedConfigFlag(PlayerPedId(), 125, false);
				*/
				/*
				b.Tabs.Clear();
				TabItem item1 = new TabItem("Item 1");
				List<MissionInformation> missions = new List<MissionInformation>()
				{
					new MissionInformation("Mission Info", new List<Tuple<string, string>>()
					{
						new Tuple<string, string>("Mission title", "Mission subtitle")
					})
				};
				TabMissionSelectItem item3 = new TabMissionSelectItem("Mission control to Major Tom", missions);
				b.AddTab(item1);
				b.AddTab(item2);
				b.AddTab(item3);
				b.Visible = true;
				*/
			}
		}
	}
}