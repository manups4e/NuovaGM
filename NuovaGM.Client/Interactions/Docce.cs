using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using static CitizenFX.Core.Native.API;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;

namespace NuovaGM.Client.Interactions
{
	static class Docce
	{
		private static string sLocal_436;
		private static string sLocal_437;
		private static string sLocal_438;
		private static string sLocal_439;
		private static string sLocal_440;
		private static string sLocal_441;
		private static string sLocal_442;
		private static string sLocal_443;
		private static string sLocal_444;
		private static string sLocal_445;
		private static float Global_2499242_f_20 = 0;
		private static float Global_2499242_f_21 = 0;
		private static float Global_2499242_f_22 = 0;
		private static float Global_2499242_f_23 = 0;
		private static float Global_2499242_f_15 = 0;
		private static float Global_2499242_f_16 = 0;
		private static float Global_2499242_f_17 = 0;
		private static float Global_2499242_f_18 = 0;

		public static void Init()
		{
			Client.GetInstance.RegisterEventHandler("lprp:onPlayerSpawned", new Action(Spawnato));
			
		}

		private async static void Spawnato()
		{
			if (Eventi.Player.CurrentChar.skin.sex == "Maschio")
			{
				sLocal_436 = "mp_safehouseshower@male@";
				sLocal_437 = "male_shower_undress_&_turn_on_water";
				sLocal_438 = "male_shower_enter_into_idle";
				sLocal_439 = "male_shower_idle_a";
				sLocal_440 = "male_shower_idle_b";
				sLocal_441 = "male_shower_idle_c";
				sLocal_442 = "male_shower_idle_d";
				sLocal_443 = "Male_Shower_Exit_To_Idle";
				sLocal_444 = "male_shower_undress_&_turn_on_water_door";
				sLocal_445 = "Male_Shower_Exit_To_Idle_Door";
				Global_2499242_f_20 = 0.5f;
				Global_2499242_f_21 = 0.55f;
				Global_2499242_f_22 = 0.833f;
				Global_2499242_f_23 = 0.25f;
				Global_2499242_f_15 = 0.26f;
				Global_2499242_f_16 = 0.9f;
				Global_2499242_f_17 = 0.3f;
				Global_2499242_f_18 = 0.79f;
			}
			else
			{
				sLocal_436 = "mp_safehouseshower@female@";
				sLocal_437 = "shower_undress_&_turn_on_water";
				sLocal_438 = "shower_enter_into_idle";
				sLocal_439 = "shower_idle_a";
				sLocal_440 = "shower_idle_b";
				sLocal_441 = "shower_idle_b";
				sLocal_442 = "shower_idle_a";
				sLocal_443 = "shower_Exit_To_Idle";
				sLocal_444 = "shower_undress_&_turn_on_water_door";
				sLocal_445 = "shower_Exit_To_Idle_Door";
				Global_2499242_f_20 = 0.5f;
				Global_2499242_f_21 = 0.5f;
				Global_2499242_f_22 = 0.384f;
				Global_2499242_f_23 = 0.166f;
				Global_2499242_f_15 = 0.26f;
				Global_2499242_f_16 = 0.9f;
				Global_2499242_f_17 = 0.3f;
				Global_2499242_f_18 = 0.75f;
			}
			RequestAnimDict(sLocal_436);
			while (!HasAnimDictLoaded(sLocal_436)) await BaseScript.Delay(0);
		}

		private static async void FaiLaDoccia()
		{
			ClearAreaOfProjectiles(Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y, Game.PlayerPed.Position.Z, 3f, false);
		}
	}
}
