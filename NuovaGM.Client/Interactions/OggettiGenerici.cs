
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Shared;

namespace NuovaGM.Client.Interactions
{
	static class OggettiGenerici
	{
		public static void Init()
		{

			ConfigShared.SharedConfig.Main.Generici.ItemList["hamburger"].Usa += async (item, index) =>
			{
				RequestAnimDict("mp_player_inteat@burger");
				while (!HasAnimDictLoaded("mp_player_inteat@burger")) await BaseScript.Delay(0);

				TaskSequence mangia = new TaskSequence();
				mangia.AddTask.PlayAnimation("mp_player_inteat@burger", "mp_player_int_eat_burger_enter");
				mangia.AddTask.PlayAnimation("mp_player_inteat@burger", "mp_player_int_eat_burger");
				mangia.AddTask.PlayAnimation("mp_player_inteat@burger", "mp_player_int_eat_burger_fp");
				mangia.AddTask.PlayAnimation("mp_player_inteat@burger", "mp_player_int_eat_exit_burger");
				mangia.Close();

				HUD.MenuPool.CloseAllMenus();
				Prop burg = new Prop(CreateObject((int)item.prop, 0, 0, 0, true, true, true));
				burg.AttachTo(Game.PlayerPed.Bones[(Bone)18905], new Vector3(0.12f, 0.028f, 0.001f), new Vector3(10.0f, 175.0f, 0.0f));
				while (burg == null && !burg.Exists() && !burg.IsAttachedTo(Game.PlayerPed)) await BaseScript.Delay(0);

				Game.PlayerPed.Task.PerformSequence(mangia);
				while (Game.PlayerPed.TaskSequenceProgress != 0) await BaseScript.Delay(0);
				while (Game.PlayerPed.TaskSequenceProgress != -1) await BaseScript.Delay(0);

				burg.Detach();
				burg.Delete();

				RemoveAnimDict("mp_player_inteat@burger");
			};

			ConfigShared.SharedConfig.Main.Generici.ItemList["acqua"].Usa += async (item, index) =>
			{
				RequestAnimDict("mp_player_intdrink");
				while (!HasAnimDictLoaded("mp_player_intdrink")) await BaseScript.Delay(0);

				HUD.MenuPool.CloseAllMenus();
				Prop water = new Prop(CreateObject((int)item.prop, 0, 0, 0, true, true, true));
				water.AttachTo(Game.PlayerPed.Bones[(Bone)18905], new Vector3(0.12f, 0.028f, 0.001f), new Vector3(10.0f, 175.0f, 0.0f));
//				AttachEntityToEntity(water.Handle, Game.PlayerPed.Handle, GetPedBoneIndex(PlayerPedId(), 18905), 0.12f, 0.028f, 0.001f, 10.0f, 175.0f, 0.0f, true, true, false, true, 1, true);

				while (water == null && !water.Exists()) await BaseScript.Delay(0);

				TaskSequence bevi = new TaskSequence();
				bevi.AddTask.PlayAnimation("mp_player_intdrink", "intro_bottle");
				bevi.AddTask.PlayAnimation("mp_player_intdrink", "loop_bottle", 2f, 3000, AnimationFlags.Loop);
				bevi.AddTask.PlayAnimation("mp_player_intdrink", "outro_bottle");
				bevi.Close();

				Game.PlayerPed.Task.PerformSequence(bevi);

				while (Game.PlayerPed.TaskSequenceProgress != 0) await BaseScript.Delay(0);
				while (Game.PlayerPed.TaskSequenceProgress != -1) await BaseScript.Delay(0);

				water.Detach();
				water.Delete();

				RemoveAnimDict("mp_player_intdrink");
			};
		}
	}
}
