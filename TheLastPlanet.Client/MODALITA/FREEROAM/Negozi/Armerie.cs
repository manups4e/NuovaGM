using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using TheLastPlanet.Client.Handlers;

namespace TheLastPlanet.Client.MODALITA.FREEROAM.Scripts.Negozi
{
	static class Armerie
	{
		private static string anim = "random@shop_gunstore";
		private static string granadesProp = "lr2_prop_gc_grenades_02";
		private static string v_gun2_wall = "ch_prop_board_wpnwall_01a";
		private static string v_gun_wall = "ch_prop_board_wpnwall_02a";
		private static string lightProp = "v_lirg_gunlight";
		private static Prop Walls;
		private static Prop Weaps;
		private static bool ShopLoaded =false;
		private static int ShopId = -1;
		private static List<InputController> inputs = new();
		public static List<SharedWeapon> Weapons;
		private static List<Blip> blips = new();
		private static List<Position> negozi = new()
		{
			new(21.852f, -1106.65f, 29.61f),
			new(1693.4f, 3759.5f, 33.7f),
			new(252.3f, -50.0f, 68.9f),
			new(842.4f, -1033.4f, 27.1f),
			new(-330.2f, 6083.8f, 30.4f),
			new(-662.1f, -935.3f, 20.8f),
			new(-1306.2f, -394.0f, 35.6f),
			new(-1117.5f, 2698.6f, 17.5f),
			new(-3172.317f, 1087.959f, 20.83873f),
			new(2567.6f, 294.3f, 107.7f),
			new(810.2f, -2157.3f, 28.6f),
		};

		private static List<Vector3> Entrances = new()
		{
			new(17.6804f, -1114.288f, 28.797f),
			new(1697.979f, 3753.2f, 33.7053f),
			new(245.2711f, -45.8126f, 68.941f),
			new(844.1248f, -1025.571f, 27.1948f),
			new(-325.8904f, 6077.026f, 30.4548f),
			new(-664.2178f, -943.3646f, 20.8292f),
			new(-1313.948f, -390.9637f, 35.592f),
			new(-1111.238f, 2688.463f, 17.6131f),
			new(-3165.231f, 1082.855f, 19.8438f),
			new(2569.612f, 302.576f, 107.7349f),
			new(811.8699f, -2149.102f, 28.6363f)
		};
		private static Camera ArmCam;


		public static async void Init()
		{
			foreach (var v in negozi)
			{
				Blip bliparmi = World.CreateBlip(v.ToVector3);
				bliparmi.Sprite = BlipSprite.AmmuNation;
				SetBlipDisplay(bliparmi.Handle, 4);
				bliparmi.Scale = 1f;
				bliparmi.Color = BlipColor.Green;
				bliparmi.IsShortRange = true;
				bliparmi.Name = "AMMU-NATION";
				blips.Add(bliparmi);
				var inp = new InputController(Control.Context, v, "Premi ~INPUT_CONTEXT~ per accedere all'armeria", new((MarkerType)(-1), v, ScaleformUI.Colors.Transparent), ModalitaServer.FreeRoam, action: new Action<Ped, object[]>(MenuArmeria));
				InputHandler.AddInput(inp);
				inputs.Add(inp);
			}
			Weapons = await Client.Instance.Events.Get<List<SharedWeapon>>("tlg:getWeaponsConfig");
			Client.Instance.AddTick(ArmeriaTick);
		}

		public static void Stop()
		{
			blips.ForEach(v => v.Delete());
			blips.Clear();
			inputs.ForEach(x => InputHandler.RemoveInput(x));
			inputs.Clear();
			Client.Instance.RemoveTick(ArmeriaTick);
		}

		public static async Task ArmeriaTick()
		{
			if (!ShopLoaded)
			{
				foreach (var pos in Entrances)
				{
					if (PlayerCache.MyPlayer.Posizione.IsInRangeOf(pos, 200))
					{
						ShopLoaded = true;
						ShopId = Entrances.IndexOf(pos) + 28;
						CreateModelHide(pos.X, pos.Y, pos.Z, 20f, Funzioni.HashUint("v_ilev_gc_weapons"), true);
						CreateModelHide(pos.X, pos.Y, pos.Z, 20f, Funzioni.HashUint("v_ilev_gc_handguns"), true);
						CreateModelHide(pos.X, pos.Y, pos.Z, 20f, Funzioni.HashUint("p_parachute_s"), true);
						CreateModelHide(pos.X, pos.Y, pos.Z, 20f, Funzioni.HashUint("v_7_wallhooks"), true);
						CreateModelHide(pos.X, pos.Y, pos.Z, 20f, Funzioni.HashUint("v_22_wallhooks"), true);
						CreateModelHide(pos.X, pos.Y, pos.Z, 20f, Funzioni.HashUint("v_ilev_gc_grenades"), true);
						CreateModelHide(pos.X, pos.Y, pos.Z, 2f, Funzioni.HashUint("prop_box_ammo07b"), true);
						CreateModelHide(pos.X, pos.Y, pos.Z, 2f, Funzioni.HashUint("v_ret_gc_calc"), true);
						CreateModelHide(pos.X, pos.Y, pos.Z, 2f, Funzioni.HashUint("v_ret_gc_mags"), true);
						CreateModelHide(pos.X, pos.Y, pos.Z, 20f, Funzioni.HashUint("v_7_wallguns"), true);
						CreateModelHide(pos.X, pos.Y, pos.Z, 20f, Funzioni.HashUint("v_22_wallguns"), true);

						RequestModel(Funzioni.HashUint(granadesProp));
						while (!HasModelLoaded(Funzioni.HashUint(granadesProp))) await BaseScript.Delay(0);
						RequestModel(Funzioni.HashUint(v_gun2_wall));
						while(!HasModelLoaded(Funzioni.HashUint(v_gun2_wall))) await BaseScript.Delay(0);
						RequestModel(Funzioni.HashUint(v_gun_wall));
						while(!HasModelLoaded(Funzioni.HashUint(v_gun_wall))) await BaseScript.Delay(0);

						if (ShopId == 28 || ShopId == 38)
						{
							Vector3 Var33 = new(21.852f, -1106.65f, 29.61f);
							Vector3 Var36 = new(0f, 0f, -20f);
							 func_212(28, ShopId, ref Var33);
							 func_208(28, ShopId, ref Var36);

							Vector3 Var39 = new(22.2036f, -1106.676f, 30.2385f);
							Vector3 Var42 = new(0f, 0f, -20f);
							 func_212(28, ShopId, ref Var39);
							 func_208(28, ShopId, ref Var42);

							Weaps = new(CreateObjectNoOffset(Funzioni.HashUint(granadesProp), Var33.X, Var33.Y, Var33.Z,false, true,false));
							Weaps.Rotation = Var36;

							Walls = new(CreateObjectNoOffset(Funzioni.HashUint(v_gun_wall), Var39.X, Var39.Y, Var39.Z,false, true,false));
							Walls.Rotation = Var42;
						}
						else
                        {
							Vector3 Var33 = new(-1305.318f, -394.031f, 36.51f);
							Vector3 Var36 = new(0f, 0f, -105f);
							 func_212(34, ShopId, ref Var33);
							 func_208(34, ShopId, ref Var36);

							Vector3 Var39 = new(-1305.308f, -394.3083f, 37.1373f);
							Vector3 Var42 = new(0f, 0f, -104.22f);
							 func_212(34, ShopId, ref Var39);
							 func_208(34, ShopId, ref Var42);

							Weaps = new(CreateObjectNoOffset(Funzioni.HashUint(granadesProp), Var33.X, Var33.Y, Var33.Z,false, true,false));
							Weaps.Rotation = Var36;

							Walls = new(CreateObjectNoOffset(Funzioni.HashUint(v_gun2_wall), Var39.X, Var39.Y, Var39.Z,false, true,false));
							Walls.Rotation = Var42;
						}
						SetModelAsNoLongerNeeded(Funzioni.HashUint(granadesProp));
						SetModelAsNoLongerNeeded(Funzioni.HashUint(v_gun2_wall));
						SetModelAsNoLongerNeeded(Funzioni.HashUint(v_gun_wall));

						for (var i = 0; i < 22; i++)
						{
							for (int j = 0; j < 5; j++)
							{
								var info = GetWeaponValues(i, j);
								if(info != null)
                                {
									var hash = Funzioni.HashUint(info.Model);
									int weapon;
									if (info.Type == 0)
                                    {
										RequestModel(hash);
										while (!HasModelLoaded(hash)) await BaseScript.Delay(0);
										weapon= CreateObjectNoOffset(hash, info.Coords.X, info.Coords.Y, info.Coords.Z, false, false, false);
                                    }
                                    else
                                    {
										RequestWeaponAsset(hash, 31, 0);
										while (!HasWeaponAssetLoaded(hash)) await BaseScript.Delay(0);
										weapon = Function.Call<int>(Hash.CREATE_WEAPON_OBJECT, hash, 0, info.Coords.X, info.Coords.Y, info.Coords.Z, true, 1f, 0, 0, 1);
										SetWeaponObjectTintIndex(weapon, 0);
									}
									SetEntityVisible(weapon, true, false);
									SetEntityCoordsNoOffset(weapon, info.Coords.X, info.Coords.Y, info.Coords.Z, false, false, true);
									SetEntityAlpha(weapon, 255, 1);
									SetEntityRotation(weapon, info.Rotation.X, info.Rotation.Y, info.Rotation.Z, 2, true);
									SetEntityCanBeDamaged(weapon, false);
									Client.Logger.Debug($"Weapon:{info.Model},pos: {GetEntityCoords(weapon, true)}");
								}
							}
						}
					}
				}
			}
			else
			{
				var pos = Entrances[ShopId - 28];
				if (!PlayerCache.MyPlayer.Posizione.IsInRangeOf(pos, 200))
				{
					if (ShopLoaded)
					{
						ShopLoaded =false;
						Function.Call(Hash.REMOVE_MODEL_HIDE, pos.X, pos.Y, pos.Z, 20.0f, Funzioni.HashUint("v_ilev_gc_weapons"),false);
						Function.Call(Hash.REMOVE_MODEL_HIDE, pos.X, pos.Y, pos.Z, 20.0f, Funzioni.HashUint("v_ilev_gc_handguns"),false);
						Function.Call(Hash.REMOVE_MODEL_HIDE, pos.X, pos.Y, pos.Z, 20.0f, Funzioni.HashUint("p_parachute_s"),false);
						Function.Call(Hash.REMOVE_MODEL_HIDE, pos.X, pos.Y, pos.Z, 20.0f, Funzioni.HashUint("v_7_wallhooks"),false);
						Function.Call(Hash.REMOVE_MODEL_HIDE, pos.X, pos.Y, pos.Z, 20.0f, Funzioni.HashUint("v_22_wallhooks"),false);
						Function.Call(Hash.REMOVE_MODEL_HIDE, pos.X, pos.Y, pos.Z, 20.0f, Funzioni.HashUint("v_ilev_gc_grenades"),false);
						Function.Call(Hash.REMOVE_MODEL_HIDE, pos.X, pos.Y, pos.Z, 2.0f, Funzioni.HashUint("prop_box_ammo07b"),false);
						Function.Call(Hash.REMOVE_MODEL_HIDE, pos.X, pos.Y, pos.Z, 2.0f, Funzioni.HashUint("v_ret_gc_calc"),false);
						Function.Call(Hash.REMOVE_MODEL_HIDE, pos.X, pos.Y, pos.Z, 2.0f, Funzioni.HashUint("v_ret_gc_mags"),false);
						Function.Call(Hash.REMOVE_MODEL_HIDE, pos.X, pos.Y, pos.Z, 20.0f, Funzioni.HashUint("v_7_wallguns"),false);
						Function.Call(Hash.REMOVE_MODEL_HIDE, pos.X, pos.Y, pos.Z, 20.0f, Funzioni.HashUint("v_22_wallguns"),false);
						if (Walls is not null && Walls.Exists()) Walls.Delete();
						Walls = null;
						if (Weaps is not null && Weaps.Exists()) Weaps.Delete();
						Weaps  = null;
					}
				}
			}
		}

		private static void MenuArmeria(Ped p, object[] _)
		{
			if (ArmCam is null)
				ArmCam = World.CreateCamera(new Vector3(-662.5455f, -934.1703f, 22.72922f), new Vector3(-89.34778f, 0, 0), 52f);
			RenderScriptCams(true, true, 1000,false, true);
			PlayerCache.MyPlayer.Player.CanControlCharacter =false;
			Armeria();
		}

		private static async void Armeria()
		{
			Prop armaObj = null;
			UIMenu armeria = new("", "", new PointF(20, 20), new KeyValuePair<string, string>("ShopUI_Title_GunClub", "ShopUI_Title_GunClub"));
			HUD.MenuPool.Add(armeria);
			var coords = PlayerCache.MyPlayer.Posizione;
			var pp = new Prop(GetClosestObjectOfType(coords.X, coords.Y, coords.Z, 10, 1948561556,false, true, true));
			var left = pp.Position + (pp.RightVector * 0.6f) + (pp.UpVector * 0.2f);
			armeria.InstructionalButtons.Add(new(InputGroup.INPUTGROUP_FRONTEND_DPAD_LR, "Scegli arma"));

			for (int i = 0; i < Weapons.Count; i++)
			{
				UIMenuItem weap = new(Game.GetGXTEntry(Weapons[i].Label.Name), Weapons[i].Description != null ? Weapons[i].Description.Hash : 0);
				weap.SetRightBadge(BadgeIcon.NONE);
				armeria.AddItem(weap);
			}

			armeria.OnIndexChange += async (a, b) =>
			{
				if (armaObj != null && armaObj.Exists())
					armaObj.Delete();
				RequestWeaponAsset(Weapons[b].UintHash, 31, 0);
				while (!HasWeaponAssetLoaded(Weapons[b].UintHash)) await BaseScript.Delay(0);
				var pos = new Vector3(-662.4155f, -934.2104f, pp.Position.Z);
				armaObj = new Prop(CreateWeaponObject(Weapons[b].UintHash, 50, pos.X, pos.Y, left.Z, true, 1.0f, 0));
				armaObj.PositionNoOffset = pos;
				RemoveWeaponAsset(Weapons[b].UintHash);
				armaObj.Rotation = new Vector3(90);
				armaObj.IsPositionFrozen = true;
			};

			HUD.MenuPool.OnMenuStateChanged += (a, b, c) =>
			{
				if (c == MenuState.Closed)
				{
					if (armaObj != null && armaObj.Exists())
						armaObj.Delete();
					PlayerCache.MyPlayer.Player.CanControlCharacter = true;
					RenderScriptCams(false, true, 2000,false, true);
					ArmCam = null;
				}
			};
			armeria.Visible = true;
		}
		private static WeaponInfo GetWeaponValues(int iParam3, int iParam4)
		{
			Vector3 Var17;
			Vector3 Var20;
			int iVar33;
			WeaponInfo info = null;
			if (ShopId == 28 || ShopId == 38)
			{
				Var17 = new(23f, -1108.65f, 29.55f);
				Var20 = new(23.07f, -1108.5f, 29.55f);
				iVar33 = 28;
				switch (iParam3)
				{
					case 0:
						switch (iParam4)
						{
							case 0:
								info = new WeaponInfo(0, "p_parachute_s_shop", 19.7012f, -1103.268f, 31.3153f, -10.75f, 0f, -200f, 0f, 0.755f, 0f, 0f, 0f, 0f);
								break;
						}
						break;
					case 1:
						switch (iParam4)
						{
							case 0:
								info = new WeaponInfo(1, "weapon_microsmg", 19.6809f, -1103.098f, 30.753f, 0f, 0f, -20f, 0f, -0.755f, 0f, 0f, 0f, 0f);
								break;
							case 1:
								info = new WeaponInfo(1, "weapon_smg", 19.5978f, -1103.076f, 30.361f, 0f, 0f, -20f, 0f, -0.755f, 0f, 0f, 0f, 0f);
								break;
						}
						break;
					case 2:
						switch (iParam4)
						{
							case 0:
								info = new WeaponInfo(1, "weapon_assaultsmg", 20.7097f, -1103.477f, 31.489f, 0f, 0f, -20f, 0f, -0.755f, 0f, 0f, 0f, 0f);
								break;
							case 1:
								info = new WeaponInfo(1, "weapon_pumpshotgun", 20.5371f, -1103.415f, 31.131f, 0f, 0f, -20f, 0f, -0.755f, 0f, 0f, 0f, 0f);
								break;
							case 2:
								info = new WeaponInfo(1, "weapon_assaultshotgun", 20.5226f, -1103.468f, 30.781f, 0f, 0f, -20f, 0f, -0.755f, 0f, 0f, 0f, 0f);
								break;
						}
						break;
					case 3:
						switch (iParam4)
						{
							case 0:
								info = new WeaponInfo(1, "weapon_assaultrifle", 21.62219f, -1103.803f, 31.471f, 0f, 0f, -20f, 0f, -0.755f, 0f, 0f, 0f, 0f);
								break;
							case 1:
								info = new WeaponInfo(1, "weapon_carbinerifle", 21.6639f, -1103.817f, 31.156f, 0f, 0f, -20f, 0f, -0.755f, 0f, 0f, 0f, 0f);
								break;
							case 2:
								info = new WeaponInfo(1, "weapon_advancedrifle", 21.6752f, -1103.849f, 30.745f, 0f, 0f, -20f, 0f, -0.755f, 0f, 0f, 0f, 0f);
								break;
						}
						break;
					case 4:
						switch (iParam4)
						{
							case 0:
								info = new WeaponInfo(1, "weapon_sniperrifle", 22.61f, -1104.16f, 30.78f, 0f, 0f, -20f, 0f, -0.755f, 0f, 0f, 0f, 0f);
								break;
							case 1:
								info = new WeaponInfo(1, "weapon_heavysniper", 22.56f, -1104.14f, 30.35f, 0f, 0f, -20f, 0f, -0.755f, 0f, 0f, 0f, 0f);
								break;
						}
						break;
					case 5:
						switch (iParam4)
						{
							case 0:
								info = new WeaponInfo(1, "weapon_mg", 20.4875f, -1103.447f, 30.36f, 0f, 0f, -20f, 0f, -0.755f, 0f, 0f, 0f, 0f);
								break;

							case 1:
								info = new WeaponInfo(1, "weapon_combatmg", 21.5992f, -1103.867f, 30.36f, 0f, 0f, -20f, 0f, -0.755f, 0f, 0f, 0f, 0f);
								break;
						}
						break;
					case 6:
						switch (iParam4)
						{
							case 0:
								info = new WeaponInfo(1, "weapon_rpg", 24.37f, -1104.84f, 31.44f, 0f, 0f, -20f, 0f, -0.755f, 0f, 0f, 0f, 0f);
								break;

							case 1:
								info = new WeaponInfo(1, "weapon_grenadelauncher", 24.0825f, -1104.755f, 31.1125f, 0f, 0f, -20f, 0f, -0.755f, 0f, 0f, 0f, 0f);
								break;

							case 2:
								info = new WeaponInfo(1, "weapon_minigun", 23.9675f, -1104.77f, 30.757f, 0f, 0f, -20f, 0f, -0.755f, 0f, 0f, 0f, 0f);
								break;

							case 3:
								info = new WeaponInfo(1, "weapon_raycarbine", 25.2725f, -1105.213f, 31.427f, 0f, 0f, -20f, -0.5f, -0.755f, 0f, 0f, 0f, 0f);
								break;

							case 4:
								info = new WeaponInfo(1, "weapon_rayminigun", 25.22f, -1105.228f, 31.057f, 0f, 0f, -20f, -0.5f, -0.755f, 0f, 0f, 0f, 0f);
								break;
						}
						break;
					case 7:
						switch (iParam4)
						{
							case 0:
								info = new WeaponInfo(0, "prop_w_me_knife_01", 19.647f, -1105.051f, 29.54f, -89.9802f, 65f, 0f, 0f, 0f, 0.475f, 75f, -20f, 0f);
								break;

							case 1:
								info = new WeaponInfo(1, "weapon_nightstick", 19.881f, -1105.446f, 29.54f, -89.98f, 60f, 0f, 0f, 0f, 0.475f, 75f, -20f, 0f);
								break;

							case 2:
								info = new WeaponInfo(1, "weapon_hammer", 20.411f, -1105.616f, 29.54f, -89.98f, 60f, 0f, 0f, 0f, 0.475f, 75f, -20f, 0f);
								break;

							case 3:
								info = new WeaponInfo(0, "prop_w_me_bottle", 19.597f, -1105.301f, 29.56f, -89.966f, 110f, 0f, 0f, 0f, 0.475f, 75f, -60f, 0f);
								break;

							case 4:
								info = new WeaponInfo(1, "weapon_knuckle", 20.147f, -1105.131f, 29.56f, -2.5f, 95f, 168f, 0f, 0f, 0.475f, 265f, 210f, 30f);
								break;
						}
						break;
					case 8:
						switch (iParam4)
						{
							case 0:
								info = new WeaponInfo(1, "weapon_stungun", 21.925f, -1105.835f, 29.5454f, -90f, 0f, 6f, 0f, 0f, 0.475f, 90f, 0f, -26f);
								break;

							case 1:
								info = new WeaponInfo(1, "weapon_appistol", 22.1775f, -1106.162f, 29.5387f, -90f, 0f, 6f, 0f, 0f, 0.475f, 90f, 0f, -26f);
								break;

							case 2:
								info = new WeaponInfo(1, "weapon_combatpistol", 22.284f, -1105.931f, 29.5407f, -90f, 0f, 6f, 0f, 0f, 0.475f, 90f, 0f, -26f);
								break;

							case 3:
								info = new WeaponInfo(1, "weapon_pistol", 22.5315f, -1106.292f, 29.5416f, -90f, 0f, 6f, 0f, 0f, 0.475f, 90f, 0f, -26f);
								break;

							case 4:
								info = new WeaponInfo(1, "weapon_pistol50", 22.914f, -1106.419f, 29.5416f, -90f, 0f, 6f, 0f, 0f, 0.475f, 90f, 0f, -26f);
								break;
						}
						break;
					case 9:
						switch (iParam4)
						{
							case 0:
								info = new WeaponInfo(1, "weapon_smokegrenade", 23.2676f, -1107.351f, 29.6454f, 0f, 0f, 245f, 0f, 0f, 0.475f, 0f, 0f, 0f);
								break;

							case 1:
								info = new WeaponInfo(1, "weapon_grenade", 23.0382f, -1108.005f, 29.5878f, 0f, 0f, 245f, 0f, 0f, 0.475f, 0f, 0f, 0f);
								break;

							case 2:
								info = new WeaponInfo(1, "weapon_stickybomb", Var17.X, Var17.Y, Var17.Z, 0f, 0f, 25f, 0f, 0f, 0.475f, 90f, 0f, -135f);
								break;

							case 3:
								info = new WeaponInfo(1, "weapon_proxmine", Var20.X, Var20.Y, Var20.Z, 0f, 0f, 25f, 0f, 0f, 0.475f, 90f, 0f, -135f);
								break;

							case 4:
								info = new WeaponInfo(1, "weapon_pipebomb", 23.15f, -1108.4f, 29.546f, 90f, -154f, 0f, 0f, 0f, 0.475f, 90f, -26f, -60f);
								break;
						}
						break;
					case 10:
						switch (iParam4)
						{
							case 0:
								info = new WeaponInfo(1, "weapon_petrolcan", 22.9739f, -1109.181f, 29.6053f, 90f, 0f, 115f, -0.278f, -0.073f, 0.693f, -90f, 0f, 0f);
								break;

							case 1:
								info = new WeaponInfo(1, "weapon_doubleaction", 22.8f, -1108.48f, 29.545f, -90f, 0f, -110f, 0f, 0f, 0.475f, 90f, 0f, 0f);
								break;
						}
						break;
					case 16:
						switch (iParam4)
						{
							case 0:
								info = new WeaponInfo(1, "weapon_unarmed", 16.2547f, -1110.088f, 30.7311f, 0f, 0f, 70f, 0f, -0.565f, 0f, 0f, 0f, 0f);
								break;

							case 1:
								info = new WeaponInfo(1, "weapon_unarmed", 16.4693f, -1109.498f, 30.7311f, 0f, 0f, 70f, 0f, -0.565f, 0f, 0f, 0f, 0f);
								break;

							case 2:
								info = new WeaponInfo(1, "weapon_unarmed", 16.6786f, -1108.923f, 30.7311f, 0f, 0f, 70f, 0f, -0.565f, 0f, 0f, 0f, 0f);
								break;

							case 3:
								info = new WeaponInfo(1, "weapon_unarmed", 16.3512f, -1109.822f, 29.9462f, 0f, 0f, 70f, 0f, -0.565f, 0f, 0f, 0f, 0f);
								break;

							case 4:
								info = new WeaponInfo(1, "weapon_unarmed", 16.5802f, -1109.193f, 29.9462f, 0f, 0f, 70f, 0f, -0.565f, 0f, 0f, 0f, 0f);
								break;
						}
						break;
					case 17:
						switch (iParam4)
						{
							case 0:
								info = new WeaponInfo(1, "weapon_machete", 20.767f, -1105.751f, 29.54f, -89.98f, 60f, 0f, 0f, 0f, 0.475f, 75f, -20f, 0f);
								break;

							case 1:
								info = new WeaponInfo(0, "prop_w_me_dagger", 19.687f, -1105.151f, 29.54f, -89.98f, 60f, 0f, 0f, 0f, 0.475f, 75f, -20f, 0f);
								break;

							case 2:
								info = new WeaponInfo(1, "weapon_switchblade", 19.762f, -1105.274f, 29.54f, -89.98f, 60f, 0f, 0f, 0f, 0.475f, 75f, -20f, 0f);
								break;

							case 3:
								info = new WeaponInfo(1, "weapon_flashlight", 20.141f, -1105.416f, 29.54f, -89.98f, 60f, 0f, 0f, 0f, 0.475f, 75f, -20f, 0f);
								break;

							case 4:
								info = new WeaponInfo(0, "prop_w_me_hatchet", 20.201f, -1105.496f, 29.54f, -89.98f, 60f, 0f, 0f, 0f, 0.475f, 75f, -20f, 0f);
								break;
						}
						break;
					case 18:
						switch (iParam4)
						{
							case 0:
								info = new WeaponInfo(1, "weapon_snspistol", 21.6115f, -1105.967f, 29.5416f, -90f, 0f, 6f, 0f, 0f, 0.475f, 90f, 0f, -26f);
								break;

							case 1:
								info = new WeaponInfo(1, "weapon_heavypistol", 21.859f, -1106.075f, 29.562f, -90f, 0f, 6f, 0f, 0f, 0.475f, 90f, 0f, -26f);
								break;

							case 2:
								info = new WeaponInfo(1, "weapon_vintagepistol", 22.959f, -1106.185f, 29.5416f, -90f, 0f, 6f, 0f, 0f, 0.475f, 90f, 0f, -26f);
								break;

							case 3:
								info = new WeaponInfo(1, "weapon_flaregun", 23.15f, -1107.6f, 29.545f, -90f, -0.5f, -110f, 0f, 0f, 0.475f, 90f, 0f, 0f);
								break;
						}
						break;
					case 20:
						switch (iParam4)
						{
							case 0:
								info = new WeaponInfo(1, "weapon_revolver", 21.6059f, -1105.711f, 29.5416f, -90f, 0f, 6f, 0f, 0f, 0.475f, 90f, 0f, -26f);
								break;

							case 1:
								info = new WeaponInfo(1, "weapon_machinepistol", 21.9285f, -1105.841f, 29.562f, -90f, 0f, 6f, 0f, 0f, 0.475f, 90f, 0f, -26f);
								break;

							case 2:
								info = new WeaponInfo(1, "weapon_compactlauncher", 22.92f, -1108.18f, 29.545f, -90f, 0f, -110f, 0f, 0f, 0.475f, 90f, 0f, 0f);
								break;

							case 3:
								info = new WeaponInfo(1, "weapon_hominglauncher", 24.4025f, -1104.853f, 30.3595f, 0f, 0f, -20f, 0.35f, -0.755f, 0f, 0f, 0f, 0f);
								break;

							case 4:
								info = new WeaponInfo(1, "weapon_ceramicpistol", 22.654f, -1106.072f, 29.5416f, -90f, 0f, 6f, 0f, 0f, 0.475f, 90f, 0f, -26f);
								break;
						}
						break;
					case 21:
						switch (iParam4)
						{
							case 0:
								info = new WeaponInfo(1, "weapon_poolcue", 20.434f, -1105.161f, 29.54f, 0f, 90f, -19.5f, 0f, 0f, 0.43f, 0f, -45f, 0f);
								break;

							case 1:
								info = new WeaponInfo(1, "weapon_battleaxe", 20.994f, -1105.771f, 29.54f, -89.98f, 60f, 0f, 0f, 0f, 0.43f, 45f, 0f, 0f);
								break;

							case 2:
								info = new WeaponInfo(1, "weapon_wrench", 20.634f, -1105.671f, 29.54f, -89.98f, 60f, 0f, 0f, 0f, 0.475f, 75f, -20f, 0f);
								break;

							case 3:
								info = new WeaponInfo(1, "weapon_marksmanrifle", 25.3925f, -1105.182f, 30.732f, 0f, 0f, -20f, -0.5f, -0.755f, 0f, 0f, 0f, 0f);
								break;

							case 4:
								info = new WeaponInfo(1, "weapon_heavyshotgun", 25.4075f, -1105.167f, 30.367f, 0f, 0f, -20f, -0.5f, -0.755f, 0f, 0f, 0f, 0f);
								break;
						}
						break;
				}
			}
			else
			{
				Var17 = new(1695.31f, 3760.17f, 34.46f);
				Var20 = new(1695.22f, 3760.29f, 34.457f);
				iVar33 = 29;
				switch (iParam3)
				{
					case 0:
						switch (iParam4)
						{
							case 0:
								info = new WeaponInfo(0, "p_parachute_s_shop", 1689.082f, 3759.161f, 36.2236f, 0f, 0f, -132f, 0f, 0.755f, 0f, 0f, 0f, 0f);
								break;
						}
						break;
					case 1:
						switch (iParam4)
						{
							case 0:
								info = new WeaponInfo(1, "weapon_microsmg", 1688.883f, 3759.17f, 35.67f, 0f, 0f, 47.3919f, 0f, -0.755f, 0f, 0f, 0f, 0f);
								break;
							case 1:
								info = new WeaponInfo(1, "weapon_smg", 1688.82f, 3759.1f, 35.27f, 0f, 0f, 47.3919f, 0f, -0.755f, 0f, 0f, 0f, 0f);
								break;
						}
						break;
					case 2:
						switch (iParam4)
						{
							case 0:
								info = new WeaponInfo(1, "weapon_assaultsmg", 1689.693f, 3760.031f, 36.398f, 0f, 0f, 47.3919f, 0f, -0.755f, 0f, 0f, 0f, 0f);
								break;
							case 1:
								info = new WeaponInfo(1, "weapon_pumpshotgun", 1689.51f, 3759.83f, 36.04f, 0f, 0f, 47.3919f, 0f, -0.755f, 0f, 0f, 0f, 0f);
								break;
							case 2:
								info = new WeaponInfo(1, "weapon_assaultshotgun", 1689.612f, 3759.863f, 35.69f, 0f, 0f, 47.3919f, 0f, -0.755f, 0f, 0f, 0f, 0f);
								break;
						}
						break;
					case 3:
						switch (iParam4)
						{
							case 0:
								info = new WeaponInfo(1, "weapon_assaultrifle", 1690.327f, 3760.753f, 36.38f, 0f, 0f, 47.3919f, 0f, -0.755f, 0f, 0f, 0f, 0f);
								break;
							case 1:
								info = new WeaponInfo(1, "weapon_carbinerifle", 1690.367f, 3760.793f, 36.03f, 0f, 0f, 47.3919f, 0f, -0.755f, 0f, 0f, 0f, 0f);
								break;
							case 2:
								info = new WeaponInfo(1, "weapon_advancedrifle", 1690.379f, 3760.773f, 35.65f, 0f, 0f, 47.3919f, 0f, -0.755f, 0f, 0f, 0f, 0f);
								break;
						}
						break;
					case 4:
						switch (iParam4)
						{
							case 0:
								info = new WeaponInfo(1, "weapon_sniperrifle", 1691.02f, 3761.51f, 35.685f, 0f, 0f, 47.3919f, 0f, -0.755f, 0f, 0f, 0f, 0f);
								break;
							case 1:
								info = new WeaponInfo(1, "weapon_heavysniper", 1690.99f, 3761.47f, 35.28f, 0f, 0f, 47.3919f, 0f, -0.755f, 0f, 0f, 0f, 0f);
								break;
						}
						break;
					case 5:
						switch (iParam4)
						{
							case 0:
								info = new WeaponInfo(1, "weapon_mg", 1689.604f, 3759.869f, 35.27f, 0f, 0f, 47.3919f, 0f, -0.755f, 0f, 0f, 0f, 0f);
								break;

							case 1:
								info = new WeaponInfo(1, "weapon_combatmg", 1690.387f, 3760.716f, 35.28f, 0f, 0f, 47.3919f, 0f, -0.755f, 0f, 0f, 0f, 0f);
								break;
						}
						break;
					case 6:
						switch (iParam4)
						{
							case 0:
								info = new WeaponInfo(1, "weapon_rpg", 1692.32f, 3762.84f, 36.38f, 0f, 0f, 47.3919f, 0f, -0.755f, 0f, 0f, 0f, 0f);
								break;

							case 1:
								info = new WeaponInfo(1, "weapon_grenadelauncher", 1692.104f, 3762.608f, 36.03f, 0f, 0f, 47.3919f, 0f, -0.755f, 0f, 0f, 0f, 0f);
								break;

							case 2:
								info = new WeaponInfo(1, "weapon_minigun", 1692.062f, 3762.524f, 35.675f, 0f, 0f, 47.3919f, 0f, -0.755f, 0f, 0f, 0f, 0f);
								break;

							case 3:
								info = new WeaponInfo(1, "weapon_raycarbine", 1692.995f, 3763.491f, 36.35f, 0f, 0f, 47.3919f, -0.5f, -0.755f, 0f, 0f, 0f, 0f);
								break;

							case 4:
								info = new WeaponInfo(1, "weapon_rayminigun", 1692.971f, 3763.463f, 35.99f, 0f, 0f, 47.3919f, -0.5f, -0.755f, 0f, 0f, 0f, 0f);
								break;
						}
						break;
					case 7:
						switch (iParam4)
						{
							case 0:
								info = new WeaponInfo(0, "prop_w_me_knife_01", 1690.703f, 3758.437f, 34.449f, -90f, 0f, 0f, 0f, 0f, 0.475f, 35f, 0f, 0f);
								break;

							case 1:
								info = new WeaponInfo(1, "weapon_nightstick", 1691.133f, 3758.507f, 34.4425f, -89.9657f, -2.39193f, 0f, 0f, 0f, 0.475f, 45f, 0f, 0f);
								break;

							case 2:
								info = new WeaponInfo(1, "weapon_hammer", 1691.5f, 3758.95f, 34.449f, -89.9657f, -2.39193f, 0f, 0f, 0f, 0.475f, 45f, 0f, 0f);
								break;

							case 3:
								info = new WeaponInfo(0, "prop_w_me_bottle", 1690.903f, 3758.287f, 34.466f, -89.9657f, 42.9f, 0f, 0f, 0f, 0.475f, 45f, 0f, 0f);
								break;

							case 4:
								info = new WeaponInfo(1, "weapon_knuckle", 1690.943f, 3758.847f, 34.415f, -182f, 96.0003f, 41f, 0f, 0f, 0.475f, 90f, 0f, 0f);
								break;
						}
						break;
					case 8:
						switch (iParam4)
						{
							case 0:
								info = new WeaponInfo(1, "weapon_stungun", 1692.277f, 3760.185f, 34.4537f, -90f, -73f, 0f, 0f, 0f, 0.475f, 90f, 73f, 45f);
								break;

							case 1:
								info = new WeaponInfo(1, "weapon_appistol", 1692.722f, 3760.345f, 34.447f, -90f, -73f, 0f, 0f, 0f, 0.475f, 90f, 73f, 45f);
								break;

							case 2:
								info = new WeaponInfo(1, "weapon_combatpistol", 1692.542f, 3760.539f, 34.449f, -90f, -73f, 0f, 0f, 0f, 0.475f, 90f, 73f, 45f);
								break;

							case 3:
								info = new WeaponInfo(1, "weapon_pistol", 1692.989f, 3760.618f, 34.4499f, -90f, -72.5f, 0f, 0f, 0f, 0.475f, 90f, 72.5f, 45f);
								break;

							case 4:
								info = new WeaponInfo(1, "weapon_pistol50", (1693.159f + 0.07f), (3760.838f + 0.09f), 34.4499f, -90f, -72.5f, 0f, 0f, 0f, 0.475f, 90f, 72.5f, 45f);
								break;
						}
						break;
					case 9:
						switch (iParam4)
						{
							case 0:
								info = new WeaponInfo(1, "weapon_smokegrenade", 1694.224f, 3760.886f, 34.5537f, 0f, 0f, 310f, 0f, 0f, 0.475f, 0f, 0f, 0f);
								break;

							case 1:
								info = new WeaponInfo(1, "weapon_grenade", 1694.739f, 3760.423f, 34.4961f, 0f, 0f, 310f, 0f, 0f, 0.475f, 0f, 0f, 0f);
								break;

							case 2:
								info = new WeaponInfo(1, "weapon_stickybomb", Var17.X, Var17.Y, Var17.Z, 0f, 0f, 90f, 0f, 0f, 0.475f, 90f, 0f, -135f);
								break;

							case 3:
								info = new WeaponInfo(1, "weapon_proxmine", Var20.X, Var20.Y, Var20.Z, 0f, 0f, 90f, 0f, 0f, 0.475f, 90f, 0f, -135f);
								break;

							case 4:
								info = new WeaponInfo(1, "weapon_pipebomb", 1695.15f, 3760.39f, 34.455f, 90f, -90f, 0f, 0f, 0f, 0.475f, 90f, -90f, 0f);
								break;
						}
						break;
					case 10:
						switch (iParam4)
						{
							case 0:
								info = new WeaponInfo(1, "weapon_petrolcan", 1695.804f, 3759.918f, 34.5136f, -90f, 0f, 2.5f, -0.278f, -0.073f, 0.693f, 90f, 0f, -2.5f);
								break;

							case 1:
								info = new WeaponInfo(1, "weapon_doubleaction", 1695.07f, 3760.02f, 34.454f, -90f, -47f, -88f, 0f, 0f, 0.475f, 90f, 47f, 45f);
								break;
						}
						break;
					case 16:
						switch (iParam4)
						{
							case 0:
								info = new WeaponInfo(1, "weapon_unarmed", 1694.045f, 3753.344f, 35.6458f, 0f, 0f, 137.392f, 0f, -0.565f, 0f, 0f, 0f, 0f);
								break;

							case 1:
								info = new WeaponInfo(1, "weapon_unarmed", 1693.583f, 3753.768f, 35.6458f, 0f, 0f, 137.392f, 0f, -0.565f, 0f, 0f, 0f, 0f);
								break;

							case 2:
								info = new WeaponInfo(1, "weapon_unarmed", 1693.133f, 3754.183f, 35.6458f, 0f, 0f, 137.392f, 0f, -0.565f, 0f, 0f, 0f, 0f);
								break;

							case 3:
								info = new WeaponInfo(1, "weapon_unarmed", 1693.838f, 3753.535f, 34.8608f, 0f, 0f, 137.392f, 0f, -0.565f, 0f, 0f, 0f, 0f);
								break;

							case 4:
								info = new WeaponInfo(1, "weapon_unarmed", 1693.345f, 3753.988f, 34.8608f, 0f, 0f, 137.392f, 0f, -0.565f, 0f, 0f, 0f, 0f);
								break;
						}
						break;
					case 17:
						switch (iParam4)
						{
							case 0:
								info = new WeaponInfo(1, "weapon_machete", 1691.765f, 3759.2f, 34.449f, -89.9657f, -2.39193f, 0f, 0f, 0f, 0.475f, 45f, 0f, 0f);
								break;

							case 1:
								info = new WeaponInfo(0, "prop_w_me_dagger", 1690.813f, 3758.442f, 34.449f, -89.9657f, -2.39193f, 0f, 0f, 0f, 0.475f, 45f, 0f, 0f);
								break;

							case 2:
								info = new WeaponInfo(1, "weapon_switchblade", 1690.95f, 3758.485f, 34.449f, -89.9657f, -2.39193f, 0f, 0f, 0f, 0.475f, 45f, 0f, 0f);
								break;

							case 3:
								info = new WeaponInfo(1, "weapon_flashlight", 1691.223f, 3758.727f, 34.449f, -89.9657f, -2.39193f, 0f, 0f, 0f, 0.475f, 45f, 0f, 0f);
								break;

							case 4:
								info = new WeaponInfo(0, "prop_w_me_hatchet", 1691.323f, 3758.79f, 34.449f, -89.9657f, -2.39193f, 0f, 0f, 0f, 0.475f, 45f, 0f, 0f);
								break;
						}
						break;
					case 18:
						switch (iParam4)
						{
							case 0:
								info = new WeaponInfo(1, "weapon_snspistol", 1692.289f, 3759.878f, 34.4499f, -90f, -72f, 0f, 0f, 0f, 0.475f, 90f, 72f, 45f);
								break;

							case 1:
								info = new WeaponInfo(1, "weapon_heavypistol", 1692.489f, 3760.065f, 34.447f, -90f, -73f, 0f, 0f, 0f, 0.475f, 90f, 73f, 45f);
								break;

							case 2:
								info = new WeaponInfo(1, "weapon_vintagepistol", 1693.049f, 3761.088f, 34.4537f, -90f, -72.5f, 0f, 0f, 0f, 0.475f, 90f, 72.5f, 45f);
								break;

							case 3:
								info = new WeaponInfo(1, "weapon_flaregun", 1694.41f, 3760.68f, 34.4537f, -90f, -47f, -88f, 0f, 0f, 0.475f, 90f, 47f, 45f);
								break;
						}
						break;
					case 20:
						switch (iParam4)
						{
							case 0:
								info = new WeaponInfo(1, "weapon_revolver", 1692.054f, 3759.98f, 34.4499f, -90f, -72f, 0f, 0f, 0f, 0.475f, 90f, 72f, 45f);
								break;

							case 1:
								info = new WeaponInfo(1, "weapon_machinepistol", 1692.312f, 3760.233f, 34.447f, -90f, -73f, 0f, 0f, 0f, 0.475f, 90f, 73f, 45f);
								break;

							case 2:
								info = new WeaponInfo(1, "weapon_compactlauncher", 1694.83f, 3760.25f, 34.454f, -90f, -47f, -88f, 0f, 0f, 0.475f, 90f, 47f, 45f);
								break;

							case 3:
								info = new WeaponInfo(1, "weapon_hominglauncher", 1692.281f, 3762.843f, 35.28f, 0f, 0f, 47.3919f, 0.35f, -0.755f, 0f, 0f, 0f, 0f);
								break;

							case 4:
								info = new WeaponInfo(1, "weapon_ceramicpistol", 1692.794f, 3760.81f, 34.4499f, -90f, -72f, 0f, 0f, 0f, 0.475f, 90f, 72f, 45f);
								break;
						}
						break;
					case 21:
						switch (iParam4)
						{
							case 0:
								info = new WeaponInfo(1, "weapon_poolcue", 1691.15f, 3759.175f, 34.449f, -89.9657f, 42.9f, 0f, 0f, 0f, 0.475f, 45f, 0f, 0f);
								break;

							case 1:
								info = new WeaponInfo(1, "weapon_battleaxe", 1691.895f, 3759.38f, 34.449f, -89.9657f, -2.39193f, 0f, 0f, 0f, 0.475f, 45f, 0f, 0f);
								break;

							case 2:
								info = new WeaponInfo(1, "weapon_wrench", 1691.645f, 3759.11f, 34.449f, -89.9657f, -2.39193f, 0f, 0f, 0f, 0.475f, 45f, 0f, 0f);
								break;

							case 3:
								info = new WeaponInfo(1, "weapon_marksmanrifle", 1693.021f, 3763.663f, 35.65f, 0f, 0f, 47.3919f, -0.5f, -0.755f, 0f, 0f, 0f, 0f);
								break;

							case 4:
								info = new WeaponInfo(1, "weapon_heavyshotgun", 1693.031f, 3763.693f, 35.29f, 0f, 0f, 47.3919f, -0.5f, -0.755f, 0f, 0f, 0f, 0f);
								break;
						}
						break;
				}
			}
			if (info != null)
			{
				func_212(iVar33, ShopId, ref info.Coords);
				func_208(iVar33, ShopId, ref info.Rotation);
			}
			return info;
		}

		private static bool HasPedMK2Variant(WeaponHash variant)
		{
			return variant switch
			{
				WeaponHash.Pistol => PlayerCache.MyPlayer.Ped.Weapons.HasWeapon(WeaponHash.PistolMk2),
				WeaponHash.SMG => PlayerCache.MyPlayer.Ped.Weapons.HasWeapon(WeaponHash.SMGMk2),
				WeaponHash.HeavySniper => PlayerCache.MyPlayer.Ped.Weapons.HasWeapon(WeaponHash.HeavySniperMk2),
				WeaponHash.CombatMG => PlayerCache.MyPlayer.Ped.Weapons.HasWeapon(WeaponHash.CombatMGMk2),
				WeaponHash.AssaultRifle => PlayerCache.MyPlayer.Ped.Weapons.HasWeapon(WeaponHash.AssaultRifleMk2),
				WeaponHash.CarbineRifle => PlayerCache.MyPlayer.Ped.Weapons.HasWeapon(WeaponHash.CarbineRifleMk2),
				WeaponHash.PumpShotgun => PlayerCache.MyPlayer.Ped.Weapons.HasWeapon(WeaponHash.PumpShotgunMk2),
				WeaponHash.SpecialCarbine => PlayerCache.MyPlayer.Ped.Weapons.HasWeapon(WeaponHash.SpecialCarbineMk2),
				WeaponHash.SNSPistol => PlayerCache.MyPlayer.Ped.Weapons.HasWeapon(WeaponHash.SNSPistolMk2),
				WeaponHash.MarksmanRifle => PlayerCache.MyPlayer.Ped.Weapons.HasWeapon(WeaponHash.MarksmanRifleMk2),
				WeaponHash.Revolver => PlayerCache.MyPlayer.Ped.Weapons.HasWeapon(WeaponHash.RevolverMk2),
				WeaponHash.BullpupRifle => PlayerCache.MyPlayer.Ped.Weapons.HasWeapon(WeaponHash.BullpupRifleMk2),
				_ =>false,
			};
		}

		static void func_208(int iParam0, int iParam1, ref Vector3 uParam2)
		{
			Vector3 uVar0 = Vector3.Zero;
			Vector3 Var3 = Vector3.Zero;
			Vector3 uVar6 = Vector3.Zero;
			Vector3 Var9 = Vector3.Zero;

			 func_209(iParam0, ref uVar0, ref Var3, -1);
			 func_209(iParam1, ref uVar6, ref Var9, -1);
			if (Var9.Z > 180f) Var9.Z -= 360f;
			if (Var9.Z < -180f) Var9.Z += 360f;
			if (Var3.Z > 180f) Var3.Z -= 360f;
			if (Var3.Z < -180f) Var3.Z += 360f;
			uParam2.Z += (Var9.Z - Var3.Z);
			if (uParam2.Z > 180f) uParam2.Z -= 360f;
			if (uParam2.Z < -180f) uParam2.Z += 360f;
		}
		static int func_209(int iParam0, ref Vector3 uParam1, ref Vector3 uParam2, int iParam3)
		{
			uParam1 = Vector3.Zero;
			uParam2 = Vector3.Zero;
			switch (iParam0)
			{
				case -1:
					break;

				case 28:
					uParam1 = new Vector3(23.6862f, -1106.461f, 29.9159f);
					uParam2 = new Vector3(0f, 0f, 160f);
					break;

				case 29:
					uParam1 = new Vector3(1693.572f, 3761.601f, 34.8242f);
					uParam2 = new Vector3(0f, 0f, -132.6081f);
					break;

				case 30:
					uParam1 = new Vector3(252.8583f, -51.6284f, 70.06f);
					uParam2 = new Vector3(0f, 0f, 69.9999f);
					break;

				case 31:
					uParam1 = new Vector3(841.0564f, -1034.762f, 28.3137f);
					uParam2 = new Vector3(0f, 0f, 0f);
					break;

				case 32:
					uParam1 = new Vector3(-330.2908f, 6085.548f, 31.5737f);
					uParam2 = new Vector3(0f, 0f, -135.0001f);
					break;

				case 33:
					uParam1 = new Vector3(-660.9294f, -934.1031f, 21.9481f);
					uParam2 = new Vector3(0f, 0f, 180f);
					break;

				case 34:
					uParam1 = new Vector3(-1304.976f, -395.8181f, 36.8147f);
					uParam2 = new Vector3(0f, 0f, 75.7783f);
					break;

				case 35:
					uParam1 = new Vector3(-1117.612f, 2700.264f, 18.673f);
					uParam2 = new Vector3(0f, 0f, -138.1729f);
					break;

				case 36:
					uParam1 = new Vector3(-3172.511f, 1089.412f, 20.9576f);
					uParam2 = new Vector3(0f, 0f, -113.4187f);
					break;

				case 37:
					uParam1 = new Vector3(2566.592f, 293.1332f, 108.8538f);
					uParam2 = new Vector3(0f, 0f, 0f);
					break;

				case 38:
					uParam1 = new Vector3(808.8609f, -2158.508f, 29.7379f);
					uParam2 = new Vector3(0f, 0f, 0f);
					break;
			}
			if ( func_210(uParam1, Vector3.Zero,false) && func_210(uParam2, Vector3.Zero,false))
			{
				return 0;
			}
			return 1;
		}

		static bool func_210(Vector3 Param0, Vector3 Param3, bool bParam6)
		{
			if (bParam6)
			{
				return (Param0.X == Param3.X && Param0.Y == Param3.Y);
			}
			return ((Param0.X == Param3.X && Param0.Y == Param3.Y) && Param0.Z == Param3.Z);
		}

		static void func_212(int iParam0, int iParam1, ref Vector3 uParam2)
		{
			Vector3 Var0 = Vector3.Zero;
			Vector3 Var3 = Vector3.Zero;
			Vector3 Var6 = Vector3.Zero;
			Vector3 Var9 = Vector3.Zero;
			Vector3 Var12 = Vector3.Zero;

			if ( func_210(uParam2, Vector3.Zero,false))
			{
				return;
			}
			 func_209(iParam0, ref Var0, ref Var3, -1);
			 func_209(iParam1, ref Var6, ref Var9, -1);
			Var12 = uParam2 - Var0;
			Var12 = func_15(Var12, -Var3.Z);
			Var12 = func_15(Var12, Var9.Z);
			uParam2 = GetObjectOffsetFromCoords(Var6.X, Var6.Y, Var6.Z, 0f, Var12.X, Var12.Y, Var12.Z);
		}

		static Vector3 func_15(Vector3 Param0,float fParam3)
		{
			float fVar3 = Sin(fParam3);
			float fVar4 = Cos(fParam3);
			return new((Param0.X *fVar4) - (Param0.Y *fVar3), (Param0.X *fVar3) + (Param0.Y *fVar4), Param0.Z);
		}

		static void PlaySound(int iParam0)
		{
			switch (iParam0)
			{
				case int a when a == Funzioni.HashInt("weapon_pistol"):
				case int b when b == Funzioni.HashInt("weapon_appistol"):
				case int c when c == Funzioni.HashInt("weapon_combatpistol"):
				case int d when d == Funzioni.HashInt("weapon_stungun"):
				case int e when e == Funzioni.HashInt("weapon_snspistol"):
				case int f when f == Funzioni.HashInt("weapon_heavypistol"):
					PlaySoundFrontend(-1, "WEAPON_SELECT_HANDGUN", "HUD_AMMO_SHOP_SOUNDSET", true);
					break;

				case int a when a == Funzioni.HashInt("weapon_pumpshotgun"):
				case int b when b == Funzioni.HashInt("weapon_sawnoffshotgun"):
				case int c when c == Funzioni.HashInt("weapon_assaultshotgun"):
				case int d when d == Funzioni.HashInt("weapon_bullpupshotgun"):
					PlaySoundFrontend(-1, "WEAPON_SELECT_SHOTGUN", "HUD_AMMO_SHOP_SOUNDSET", true);
					break;

				case int a when a == Funzioni.HashInt("weapon_advancedrifle"):
				case int b when b == Funzioni.HashInt("weapon_carbinerifle"):
				case int d when d == Funzioni.HashInt("weapon_specialcarbine"):
				case int e when e == Funzioni.HashInt("weapon_assaultrifle"):
				case int f when f == Funzioni.HashInt("weapon_mg"):
				case int g when g == Funzioni.HashInt("weapon_combatmg"):
				case int h when h == Funzioni.HashInt("weapon_bullpuprifle"):
					PlaySoundFrontend(-1, "WEAPON_SELECT_RIFLE", "HUD_AMMO_SHOP_SOUNDSET", true);
					break;

				case int a when a == Funzioni.HashInt("weapon_grenadelauncher"):
					PlaySoundFrontend(-1, "WEAPON_SELECT_GRENADE_LAUNCHER", "HUD_AMMO_SHOP_SOUNDSET", true);
					break;

				case int a when a == Funzioni.HashInt("weapon_rpg"):
					PlaySoundFrontend(-1, "WEAPON_SELECT_RPG_LAUNCHER", "HUD_AMMO_SHOP_SOUNDSET", true);
					break;

				case int a when a == Funzioni.HashInt("weapon_knife"):
					PlaySoundFrontend(-1, "WEAPON_SELECT_KNIFE", "HUD_AMMO_SHOP_SOUNDSET", true);
					break;

				case int a when a == Funzioni.HashInt("weapon_nightstick"):
					PlaySoundFrontend(-1, "WEAPON_SELECT_BATON", "HUD_AMMO_SHOP_SOUNDSET", true);
					break;

				case int a when a == Funzioni.HashInt("gadget_parachute"):
					PlaySoundFrontend(-1, "WEAPON_SELECT_PARACHUTE", "HUD_AMMO_SHOP_SOUNDSET", true);
					break;

				case int a when a == Funzioni.HashInt("weapon_petrolcan"):
					PlaySoundFrontend(-1, "WEAPON_SELECT_FUEL_CAN", "HUD_AMMO_SHOP_SOUNDSET", true);
					break;

				case int a when a == Funzioni.HashInt("weapon_unarmed"):
					PlaySoundFrontend(-1, "WEAPON_SELECT_ARMOR", "HUD_AMMO_SHOP_SOUNDSET", true);
					break;

				default:
					PlaySoundFrontend(-1, "WEAPON_SELECT_OTHER", "HUD_AMMO_SHOP_SOUNDSET", true);
					break;
			}
		}
		/*
			TASK::OPEN_SEQUENCE_TASK(&iLocal_147);
			TASK::TASK_PLAY_ANIM(0, sLocal_154, "_neutral_to_idle", 8f, -8f, -1, 262144, 0f,false,false,false);
			TASK::TASK_PLAY_ANIM(0, sLocal_154, "_idle", 8f, -8f, -1, 262144, 0f,false,false,false);
			TASK::TASK_PLAY_ANIM(0, sLocal_154, "_idle_a", 8f, -8f, -1, 262144, 0f,false,false,false);
			TASK::TASK_PLAY_ANIM(0, sLocal_154, "_idle_b", 8f, -8f, -1, 262144, 0f,false,false,false);
			TASK::TASK_PLAY_ANIM(0, sLocal_154, "_idle", 8f, -8f, -1, 262144, 0f,false,false,false);
			TASK::TASK_PLAY_ANIM(0, sLocal_154, "_idle_a", 8f, -8f, -1, 262144, 0f,false,false,false);
			TASK::TASK_PLAY_ANIM(0, sLocal_154, "_idle", 8f, -8f, -1, 262145, 0f,false,false,false);
			TASK::CLOSE_SEQUENCE_TASK(iLocal_147);
			TASK::OPEN_SEQUENCE_TASK(&iLocal_148);
			TASK::TASK_PLAY_ANIM(0, sLocal_154, "_greeting", 8f, -8f, -1, 786432, 0f,false,false,false);
			TASK::TASK_PLAY_ANIM(0, sLocal_154, "_idle", 8f, -2f, -1, 786433, 0f,false,false,false);
			TASK::CLOSE_SEQUENCE_TASK(iLocal_148);
			TASK::OPEN_SEQUENCE_TASK(&iLocal_149);
			TASK::TASK_PLAY_ANIM(0, sLocal_154, "_positive_goodbye", 8f, -8f, -1, 786432, 0f,false,false,false);
			TASK::TASK_PLAY_ANIM(0, sLocal_154, "_idle", 8f, -2f, -1, 786433, 0f,false,false,false);
			TASK::CLOSE_SEQUENCE_TASK(iLocal_149);
			TASK::OPEN_SEQUENCE_TASK(&iLocal_150);
			TASK::TASK_PLAY_ANIM(0, sLocal_154, "_impatient_a", 8f, -8f, -1, 786432, 0f,false,false,false);
			TASK::TASK_PLAY_ANIM(0, sLocal_154, "_idle", 8f, -2f, -1, 786433, 0f,false,false,false);
			TASK::CLOSE_SEQUENCE_TASK(iLocal_150);
			TASK::OPEN_SEQUENCE_TASK(&iLocal_151);
			TASK::TASK_PLAY_ANIM(0, sLocal_154, "_impatient_b", 8f, -8f, -1, 786432, 0f,false,false,false);
			TASK::TASK_PLAY_ANIM(0, sLocal_154, "_idle", 8f, -2f, -1, 786433, 0f,false,false,false);
			TASK::CLOSE_SEQUENCE_TASK(iLocal_151);
			TASK::OPEN_SEQUENCE_TASK(&iLocal_152);
			TASK::TASK_PLAY_ANIM(0, sLocal_154, "_idle_a", 4f, -8f, -1, 786432, 0f,false,false,false);
			TASK::TASK_PLAY_ANIM(0, sLocal_154, "_idle", 8f, -2f, -1, 786433, 0f,false,false,false);
			TASK::CLOSE_SEQUENCE_TASK(iLocal_152);
			TASK::OPEN_SEQUENCE_TASK(&iLocal_153);
			TASK::TASK_PLAY_ANIM(0, sLocal_154, "_positive_a", 8f, -8f, -1, 786432, 0f,false,false,false);
			TASK::TASK_PLAY_ANIM(0, sLocal_154, "_idle", 8f, -2f, -1, 786433, 0f,false,false,false);
			TASK::CLOSE_SEQUENCE_TASK(iLocal_153);
		*/
	}

	public class WeaponInfo
	{
		public int Type;
		public string Model;
		public Vector3 Coords;
		public Vector3 Rotation;
		public Vector3 OffsetCoords;
		public Vector3 OffsetRotation;
		public WeaponInfo(int type, string model, Vector3 coords, Vector3 rot, Vector3 offc, Vector3 offr)
		{
			Type = type;
			Model = model;
			Coords = coords;
			Rotation = rot;
			OffsetCoords = offc;
			OffsetRotation = offr;
		}
		public WeaponInfo(int type, string model, float coords, float coords_1, float coords_2, float rot, float rot_1, float rot_2, float offc, float offc_1, float offc_2, float offr, float offr_1, float offr_2)
		{
			Type = type;
			Model = model;
			Coords = new(coords, coords_1, coords_2);
			Rotation = new(rot, rot_1, rot_2);
			OffsetCoords = new(offc, offc_1, offc_2);
			OffsetRotation = new(offr, offr_1, offr_2);
		}
	}
}