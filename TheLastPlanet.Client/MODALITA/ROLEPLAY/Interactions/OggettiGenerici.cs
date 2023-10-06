using TheLastPlanet.Client.Core.Utility.HUD;


namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Interactions
{
    internal static class OggettiGenerici
    {
        public static void Init()
        {
            AccessingEvents.OnRoleplaySpawn += Spawnato;
            AccessingEvents.OnRoleplayLeave += onPlayerLeft;
        }

        public static void Spawnato(PlayerClient client)
        {
            Ped p = client.Ped;
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
                MenuHandler.CloseAndClearHistory();
                Prop burg = new Prop(CreateObject((int)item.prop, 0, 0, 0, true, true, true));
                burg.AttachTo(p.Bones[(Bone)18905], new Vector3(0.12f, 0.028f, 0.001f), new Vector3(10.0f, 175.0f, 0.0f));
                while (burg == null && !burg.Exists() && !burg.IsAttachedTo(p)) await BaseScript.Delay(0);
                p.Task.PerformSequence(mangia);
                while (p.TaskSequenceProgress != 0) await BaseScript.Delay(0);
                while (p.TaskSequenceProgress != -1) await BaseScript.Delay(0);
                burg.Detach();
                burg.Delete();
                RemoveAnimDict("mp_player_inteat@burger");
            };
            ConfigShared.SharedConfig.Main.Generici.ItemList["acqua"].Usa += async (item, index) =>
            {
                RequestAnimDict("mp_player_intdrink");
                while (!HasAnimDictLoaded("mp_player_intdrink")) await BaseScript.Delay(0);
                MenuHandler.CloseAndClearHistory();
                Prop water = new Prop(CreateObject((int)item.prop, 0, 0, 0, true, true, true));
                water.AttachTo(p.Bones[(Bone)18905], new Vector3(0.12f, 0.028f, 0.001f), new Vector3(10.0f, 175.0f, 0.0f));
                //				AttachEntityToEntity(water.Handle, p.Handle, GetPedBoneIndex(PlayerPedId(), 18905), 0.12f, 0.028f, 0.001f, 10.0f, 175.0f, 0.0f, true, true, false, true, 1, true);
                while (water == null && !water.Exists()) await BaseScript.Delay(0);
                TaskSequence bevi = new TaskSequence();
                bevi.AddTask.PlayAnimation("mp_player_intdrink", "intro_bottle");
                bevi.AddTask.PlayAnimation("mp_player_intdrink", "loop_bottle", 2f, 3000, AnimationFlags.Loop);
                bevi.AddTask.PlayAnimation("mp_player_intdrink", "outro_bottle");
                bevi.Close();
                p.Task.PerformSequence(bevi);
                while (p.TaskSequenceProgress != 0) await BaseScript.Delay(0);
                while (p.TaskSequenceProgress != -1) await BaseScript.Delay(0);
                water.Detach();
                water.Delete();
                RemoveAnimDict("mp_player_intdrink");
            };
        }
        public static void onPlayerLeft(PlayerClient client)
        {
            Ped p = client.Ped;
            ConfigShared.SharedConfig.Main.Generici.ItemList["hamburger"].Usa -= async (item, index) =>
            {
                RequestAnimDict("mp_player_inteat@burger");
                while (!HasAnimDictLoaded("mp_player_inteat@burger")) await BaseScript.Delay(0);
                TaskSequence mangia = new();
                mangia.AddTask.PlayAnimation("mp_player_inteat@burger", "mp_player_int_eat_burger_enter");
                mangia.AddTask.PlayAnimation("mp_player_inteat@burger", "mp_player_int_eat_burger");
                mangia.AddTask.PlayAnimation("mp_player_inteat@burger", "mp_player_int_eat_burger_fp");
                mangia.AddTask.PlayAnimation("mp_player_inteat@burger", "mp_player_int_eat_exit_burger");
                mangia.Close();
                MenuHandler.CloseAndClearHistory();
                Prop burg = new(CreateObject((int)item.prop, 0, 0, 0, true, true, true));
                burg.AttachTo(p.Bones[(Bone)18905], new Vector3(0.12f, 0.028f, 0.001f), new Vector3(10.0f, 175.0f, 0.0f));
                while (burg == null && !burg.Exists() && !burg.IsAttachedTo(p)) await BaseScript.Delay(0);
                p.Task.PerformSequence(mangia);
                while (p.TaskSequenceProgress != 0) await BaseScript.Delay(0);
                while (p.TaskSequenceProgress != -1) await BaseScript.Delay(0);
                burg.Detach();
                burg.Delete();
                RemoveAnimDict("mp_player_inteat@burger");
            };
            ConfigShared.SharedConfig.Main.Generici.ItemList["acqua"].Usa -= async (item, index) =>
            {
                RequestAnimDict("mp_player_intdrink");
                while (!HasAnimDictLoaded("mp_player_intdrink")) await BaseScript.Delay(0);
                MenuHandler.CloseAndClearHistory();
                Prop water = new(CreateObject((int)item.prop, 0, 0, 0, true, true, true));
                water.AttachTo(p.Bones[(Bone)18905], new Vector3(0.12f, 0.028f, 0.001f), new Vector3(10.0f, 175.0f, 0.0f));
                //				AttachEntityToEntity(water.Handle, p.Handle, GetPedBoneIndex(PlayerPedId(), 18905), 0.12f, 0.028f, 0.001f, 10.0f, 175.0f, 0.0f, true, true, false, true, 1, true);
                while (water == null && !water.Exists()) await BaseScript.Delay(0);
                TaskSequence bevi = new();
                bevi.AddTask.PlayAnimation("mp_player_intdrink", "intro_bottle");
                bevi.AddTask.PlayAnimation("mp_player_intdrink", "loop_bottle", 2f, 3000, AnimationFlags.Loop);
                bevi.AddTask.PlayAnimation("mp_player_intdrink", "outro_bottle");
                bevi.Close();
                p.Task.PerformSequence(bevi);
                while (p.TaskSequenceProgress != 0) await BaseScript.Delay(0);
                while (p.TaskSequenceProgress != -1) await BaseScript.Delay(0);
                water.Detach();
                water.Delete();
                RemoveAnimDict("mp_player_intdrink");
            };
        }

    }
}