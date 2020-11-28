using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using static CitizenFX.Core.Native.API;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Shared;

namespace NuovaGM.Client.Interactions
{
	static class Macchinette
	{
		static string anim = "MINI@SPRUNK@FIRST_PERSON";
		static string AudioBank = "VENDING_MACHINE";
		static float MachineRange = 1.375f;
		static Prop VendingMachineClosest;
		static Prop Can = new Prop((int)ObjectHash.prop_ld_can_01b);
		static List<ObjectHash> VendingHashes = new List<ObjectHash>()
		{
			ObjectHash.prop_vend_coffe_01,
			ObjectHash.prop_vend_soda_01,
			ObjectHash.prop_vend_soda_02,
			ObjectHash.prop_vend_condom_01,
			ObjectHash.prop_vend_fags_01,
			ObjectHash.prop_vend_snak_01,
			ObjectHash.prop_vend_water_01
		};

		public static void Init()
		{
			RequestAmbientAudioBank(AudioBank, false);
		}

		public static async Task ControlloMachines()
		{
			VendingMachineClosest = World.GetAllProps().Select(o => new Prop(o.Handle)).Where(o => VendingHashes.Contains((ObjectHash)(uint)o.Model.Hash)).FirstOrDefault(o => Vector3.Distance(Game.Player.GetPlayerData().posizione.ToVector3(), o.Position) < MachineRange);
			await BaseScript.Delay(200);
		}

		public static async Task VendingMachines()
		{
			Ped p = Game.PlayerPed;
			if (VendingMachineClosest != null)
			{
				if (!p.IsDead && !HUD.MenuPool.IsAnyMenuOpen)
				{
					if (Game.Player.GetPlayerData().Money > 5)
					{
						if (VendingMachineClosest.Model.Hash == (int)ObjectHash.prop_vend_soda_01 || VendingMachineClosest.Model.Hash == (int)ObjectHash.prop_vend_soda_02)
						{
							if (VendingMachineClosest.Model.Hash == (int)ObjectHash.prop_vend_soda_01)
								HUD.ShowHelp("~r~eCola Deliciously Infectious!~w~~n~~INPUT_DETONATE~ per ~b~metterla via~w~.~n~~INPUT_CONTEXT~ per ~b~berla subito~w~.");
							else if (VendingMachineClosest.Model.Hash == (int)ObjectHash.prop_vend_soda_02)
								HUD.ShowHelp("~g~Sprunk The Essence of Life!~w~~n~~INPUT_DETONATE~ per ~b~metterla via~w~.~n~~INPUT_CONTEXT~ per ~b~berla subito~w~.");
							if (Input.IsControlJustPressed(Control.Context))
							{
								RequestAnimDict(anim);
								while (!HasAnimDictLoaded(anim)) await BaseScript.Delay(0);
								BaseScript.TriggerServerEvent("lprp:removemoney", 5);
								Vector3 offset = GetOffsetFromEntityInWorldCoords(VendingMachineClosest.Handle, 0f, -0.97f, 0.05f);
								p.Task.LookAt(VendingMachineClosest);
								TaskGoStraightToCoord(PlayerPedId(), offset.X, offset.Y, offset.Z, 1f, 20000, VendingMachineClosest.Heading, 0.1f);
								await BaseScript.Delay(1000);
								await p.Task.PlayAnimation(anim, "PLYR_BUY_DRINK_PT1", 2f, -4f, -1, (AnimationFlags)1048576, 0);
								await BaseScript.Delay(1000);
								RequestModel((uint)ObjectHash.prop_ld_can_01b);
								while (!HasModelLoaded((uint)ObjectHash.prop_ld_can_01b)) await BaseScript.Delay(0);
								if (VendingMachineClosest.Model.Hash == (int)ObjectHash.prop_vend_soda_01)
									Can = new Prop(CreateObjectNoOffset((uint)ObjectHash.prop_ecola_can, offset.X, offset.Y, offset.Z, true, false, false));
								else if (VendingMachineClosest.Model.Hash == (int)ObjectHash.prop_vend_soda_02)
									Can = new Prop(CreateObjectNoOffset((uint)ObjectHash.prop_ld_can_01b, offset.X, offset.Y, offset.Z, true, false, false));
								AttachEntityToEntity(Can.Handle, PlayerPedId(), GetPedBoneIndex(PlayerPedId(), 28422), 0f, 0f, 0f, 0f, 0f, 0f, true, true, false, false, 2, true);
								SetModelAsNoLongerNeeded((uint)ObjectHash.prop_ld_can_01b);

								while (GetEntityAnimCurrentTime(PlayerPedId(), anim, "PLYR_BUY_DRINK_PT1") < 0.95f) await BaseScript.Delay(0);
								await p.Task.PlayAnimation(anim, "PLYR_BUY_DRINK_PT2", 4f, -1000f, -1, (AnimationFlags)1048576, 0);
								N_0x2208438012482a1a(PlayerPedId(), false, false);

								while (GetEntityAnimCurrentTime(PlayerPedId(), anim, "PLYR_BUY_DRINK_PT2") < 0.95f) await BaseScript.Delay(0);
								await p.Task.PlayAnimation(anim, "PLYR_BUY_DRINK_PT3", 1000f, -4f, -1, (AnimationFlags)1048624, 0);

								while (GetEntityAnimCurrentTime(PlayerPedId(), anim, "PLYR_BUY_DRINK_PT3") < 0.35f) await BaseScript.Delay(0);
								Function.Call(Hash.HINT_AMBIENT_AUDIO_BANK, "VENDING_MACHINE", 0, -1);

								Can.Detach();
								Can.ApplyForce(new Vector3(6f, 10f, 2f), new Vector3(0), ForceType.MaxForceRot);
								Can.MarkAsNoLongerNeeded();
								RemoveAnimDict(anim);
							}
							else if (Input.IsControlJustPressed(Control.Detonate))
							{
								RequestAnimDict(anim);
								while (!HasAnimDictLoaded(anim)) await BaseScript.Delay(0);
								BaseScript.TriggerServerEvent("lprp:removemoney", 5);
								Vector3 offset = GetOffsetFromEntityInWorldCoords(VendingMachineClosest.Handle, 0f, -0.97f, 0.05f);
								p.Task.LookAt(VendingMachineClosest);
								TaskGoStraightToCoord(PlayerPedId(), offset.X, offset.Y, offset.Z, 1f, 20000, VendingMachineClosest.Heading, 0.1f);
								await BaseScript.Delay(1000);
								await p.Task.PlayAnimation(anim, "PLYR_BUY_DRINK_PT1", 2f, -4f, -1, (AnimationFlags)1048576, 0);
								await BaseScript.Delay(1000);
								RequestModel((uint)ObjectHash.prop_ld_can_01b);
								while (!HasModelLoaded((uint)ObjectHash.prop_ld_can_01b)) await BaseScript.Delay(0);
								if (VendingMachineClosest.Model.Hash == (int)ObjectHash.prop_vend_soda_01)
									Can = new Prop(CreateObjectNoOffset((uint)ObjectHash.prop_ecola_can, offset.X, offset.Y, offset.Z, false, false, false));
								else if (VendingMachineClosest.Model.Hash == (int)ObjectHash.prop_vend_soda_02)
									Can = new Prop(CreateObjectNoOffset((uint)ObjectHash.prop_ld_can_01b, offset.X, offset.Y, offset.Z, false, false, false));
								AttachEntityToEntity(Can.Handle, PlayerPedId(), GetPedBoneIndex(PlayerPedId(), 28422), 0f, 0f, 0f, 0f, 0f, 0f, true, true, false, false, 2, true);
								SetModelAsNoLongerNeeded((uint)ObjectHash.prop_ld_can_01b);
								while (GetEntityAnimCurrentTime(PlayerPedId(), anim, "PLYR_BUY_DRINK_PT1") < 0.65f) await BaseScript.Delay(0);
								//controllo oggetto quantità massima :)
								HUD.ShowNotification("Hai comprato una bibita dissetante", NotificationColor.Cyan);
								Can.Detach();
								Can.Delete();
								p.Task.ClearAll();
								RemoveAnimDict(anim);
							}
						}
						else if (VendingMachineClosest.Model.Hash == (int)ObjectHash.prop_vend_coffe_01)
						{
							HUD.ShowHelp("Comprare un ~y~Caffè~w~~n~~INPUT_DETONATE~ per ~b~metterlo via~w~.~n~~INPUT_CONTEXT~ per ~b~berlo subito~w~.");
							{

							}
						}
					}
					else
						HUD.ShowNotification("Non hai abbastanza contanti!!", NotificationColor.RedDifferent, true);
				}
			}
		}
	}
}
