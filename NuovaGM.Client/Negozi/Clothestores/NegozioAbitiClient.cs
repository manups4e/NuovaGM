﻿using CitizenFX.Core;
using NuovaGM.Client.gmPrincipale.Personaggio;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using System;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace NuovaGM.Client.Negozi
{
	static class NegozioAbitiClient
	{
		static bool menu = false;
		public static Camera camm = new Camera(0);
		public static void Init()
		{
			Client.GetInstance.RegisterEventHandler("lprp:onPlayerSpawn", new Action(Spawnato));
		}

		public static async void Spawnato()
		{
			foreach (var v in ConfigClothes.Binco)
			{
				Blip blip = new Blip(AddBlipForCoord(v.Blip.X, v.Blip.Y, v.Blip.Z))
				{
					Sprite = (BlipSprite)v.BlipId,
					Color = (BlipColor)v.BlipColor,
					IsShortRange = true,
					Name = "Binco's"
				};
				SetBlipDisplay(blip.Handle, 4);
			}
			foreach (var v in ConfigClothes.Discount)
			{
				Blip blip = new Blip(AddBlipForCoord(v.Blip.X, v.Blip.Y, v.Blip.Z))
				{
					Sprite = (BlipSprite)v.BlipId,
					Color = (BlipColor)v.BlipColor,
					IsShortRange = true,
					Name = "Discount's"
				};
				SetBlipDisplay(blip.Handle, 4);
			}
			foreach (var v in ConfigClothes.Suburban)
			{
				Blip blip = new Blip(AddBlipForCoord(v.Blip.X, v.Blip.Y, v.Blip.Z))
				{
					Sprite = (BlipSprite)v.BlipId,
					Color = (BlipColor)v.BlipColor,
					IsShortRange = true,
					Name = "Suburban's"
				};
				SetBlipDisplay(blip.Handle, 4);
			}
			foreach (var v in ConfigClothes.Ponsombys)
			{
				Blip blip = new Blip(AddBlipForCoord(v.Blip.X, v.Blip.Y, v.Blip.Z))
				{
					Sprite = (BlipSprite)v.BlipId,
					Color = (BlipColor)v.BlipColor,
					IsShortRange = true,
					Name = "Ponsombys"
				};
				SetBlipDisplay(blip.Handle, 4);
			}
		}

		public static async void cam(int bone, Vector3 off, Vector3 c, bool toggle, Vector3 z)
		{
			if (!camm.Exists() || !camm.IsActive)
			{
				Vector3 xyz = Game.PlayerPed.Bones[(Bone)bone].Position;
				Vector3 offC = Game.PlayerPed.GetOffsetPosition(off);
				camm = new Camera(CreateCam("DEFAULT_SCRIPTED_CAMERA", true));
				camm.Position = new Vector3(c.X + offC.X, c.Y + offC.Y, xyz.Z + c.Z);
				if (toggle)
				{
					camm.PointAt(Game.PlayerPed.Bones[bone], new Vector3(0));
				}
				else
				{
					camm.PointAt(new Vector3(xyz.X - z.X, xyz.Y - z.Y, xyz.Z - z.Z));
				}

				camm.FieldOfView = 45.0f;
				SetCamParams(camm.Handle, offC.X + c.X, offC.Y + c.Y, xyz.Z + c.Z, 0.0f, 0.0f, 0.0f, 45.0f, 0, 1, 1, 2);
				camm.IsActive = true;
				RenderScriptCams(true, true, 1500, true, false);
			}
		}

		public static async Task OnTick()
		{
			if (!HUD.MenuPool.IsAnyMenuOpen())
			{
				foreach (var v in ConfigClothes.Binco)
				{
					if (World.GetDistance(Game.PlayerPed.Position, new Vector3(v.Vestiti.X, v.Vestiti.Y, v.Vestiti.Z)) < 0.8f)
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per guardare i vestiti");
						if (Input.IsControlJustPressed(Control.Context))
						{
							Game.PlayerPed.Task.AchieveHeading(v.Vestiti.W);
							while (Game.PlayerPed.Heading > v.Vestiti.W + 5f || Game.PlayerPed.Heading < v.Vestiti.W - 5f) await BaseScript.Delay(0);

							if (Eventi.Player.CurrentChar.skin.sex == "Maschio")
								MenuNegoziAbiti.MenuVest(ConfigClient.Conf.Negozi.Abiti.Maschio.BincoVest, "clothingshirt", "Binco");
							else
								MenuNegoziAbiti.MenuVest(ConfigClient.Conf.Negozi.Abiti.Femmina.BincoVest, "mp_clothing@female@shirt", "Binco");
							cam(24818, new Vector3(0.0f, 3.0f, 0.0f), new Vector3(0, 0, 0), true, new Vector3(0));
							menu = true;
						}
					}
					else if (World.GetDistance(Game.PlayerPed.Position, new Vector3(v.Scarpe.X, v.Scarpe.Y, v.Scarpe.Z)) < 0.8f)
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per guardare le scarpe");
						if (Input.IsControlJustPressed(Control.Context))
						{
							Game.PlayerPed.Task.AchieveHeading(v.Scarpe.W);
							while (Game.PlayerPed.Heading > v.Scarpe.W + 5f || Game.PlayerPed.Heading < v.Scarpe.W - 5f) await BaseScript.Delay(0);

							if (Eventi.Player.CurrentChar.skin.sex == "Maschio")
								MenuNegoziAbiti.MenuScarpe(ConfigClient.Conf.Negozi.Abiti.Maschio.BincoScarpe, "clothingshoes", "Binco");
							else
								MenuNegoziAbiti.MenuScarpe(ConfigClient.Conf.Negozi.Abiti.Femmina.BincoScarpe, "mp_clothing@female@Scarpe", "Binco");

							cam(14201, new Vector3(0.0f, 1.5f, 1.0f), new Vector3(0.3f, 0.0f, 0.5f), false, new Vector3(0));
							menu = true;
						}
					}
					/*					else if (World.GetDistance(Game.PlayerPed.Position, new Vector3(v.Maglie.X, v.Maglie.Y, v.Maglie.Z)) < 0.8f)
										{
											Funzioni.ShowHelp("Premi ~INPUT_CONTEXT~ per guardare le Maglie");
											if (Input.IsControlJustPressed(Control.Context))
											{
												Game.PlayerPed.Task.AchieveHeading(v.Maglie.W);
												if (Eventi.Player.CurrentChar.skin.sex == "Maschio")
													//ApriMenu(ConfigClient.Conf.Negozi.Abiti.Maschio);
													Debug.WriteLine("Maschio");
												else
													//ApriMenu(ConfigClient.Conf.Negozi.Abiti.Maschio);
													Debug.WriteLine("Femmina");
												//cam(24818, new Vector3(0), new Vector3(0), true, new Vector3(0));
												//menu = true;
											}
										}
					*/
					else if (World.GetDistance(Game.PlayerPed.Position, new Vector3(v.Pantaloni.X, v.Pantaloni.Y, v.Pantaloni.Z)) < 0.8f)
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per guardare i pantaloni");
						if (Input.IsControlJustPressed(Control.Context))
						{
							Game.PlayerPed.Task.AchieveHeading(v.Pantaloni.W);
							while (Game.PlayerPed.Heading > v.Pantaloni.W + 5f || Game.PlayerPed.Heading < v.Pantaloni.W - 5f)
							{
								await BaseScript.Delay(0);
							}

							if (Eventi.Player.CurrentChar.skin.sex == "Maschio")
							{
								MenuNegoziAbiti.MenuPant(ConfigClient.Conf.Negozi.Abiti.Maschio.BincoPant, "clothingtrousers", "Binco");
							}
							else
							{
								MenuNegoziAbiti.MenuPant(ConfigClient.Conf.Negozi.Abiti.Femmina.BincoPant, "mp_clothing@female@trousers", "Binco");
							}

							cam(51826, new Vector3(0.0f, 1.5f, 1.0f), new Vector3(0.6f, 0.0f, 0.2f), false, new Vector3(0, 0, 0.2f));
							menu = true;
						}
					}
					else if (World.GetDistance(Game.PlayerPed.Position, new Vector3(v.Occhiali.X, v.Occhiali.Y, v.Occhiali.Z)) < 0.8f)
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per guardare gli occhiali");
						if (Input.IsControlJustPressed(Control.Context))
						{
							Game.PlayerPed.Task.AchieveHeading(v.Occhiali.W);
							while (Game.PlayerPed.Heading > v.Occhiali.W + 5f || Game.PlayerPed.Heading < v.Occhiali.W - 5f)
							{
								await BaseScript.Delay(0);
							}

							if (Eventi.Player.CurrentChar.skin.sex == "Maschio")
							{
								MenuNegoziAbiti.MenuOcchiali(ConfigClient.Conf.Negozi.Abiti.Maschio.Occhiali, "clothingspecs", "Binco");
							}
							else
							{
								MenuNegoziAbiti.MenuOcchiali(ConfigClient.Conf.Negozi.Abiti.Femmina.Occhiali, "mp_clothing@female@glasses", "Binco");
							}

							cam(31086, new Vector3(0.0f, 1.45f, 0.0f), new Vector3(0), true, new Vector3(0));
							menu = true;
						}
					}
					else if (World.GetDistance(Game.PlayerPed.Position, new Vector3(v.Accessori.X, v.Accessori.Y, v.Accessori.Z)) < 0.8f)
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per guardare gli accessori");
						if (Input.IsControlJustPressed(Control.Context))
						{
							Game.PlayerPed.Task.AchieveHeading(v.Accessori.W);
							while (Game.PlayerPed.Heading > v.Accessori.W + 5f || Game.PlayerPed.Heading < v.Accessori.W - 5f)
							{
								await BaseScript.Delay(0);
							}

							if (Eventi.Player.CurrentChar.skin.sex == "Maschio")
							{
								if (Eventi.Player.CurrentChar.skin.sex == "Maschio")
								{
									MenuNegoziAbiti.MenuAccessori(ConfigClient.Conf.Negozi.Abiti.Maschio.Accessori, "clothingshirt", "Binco");
								}
								else
								{
									MenuNegoziAbiti.MenuAccessori(ConfigClient.Conf.Negozi.Abiti.Femmina.Accessori, "mp_clothing@female@shirt", "Binco");
								}
							}

							cam(24818, new Vector3(0.0f, 3.0f, 0.0f), new Vector3(0), true, new Vector3(0));
							menu = true;
						}
					}
				}
				foreach (var v in ConfigClothes.Discount)
				{
					if (World.GetDistance(Game.PlayerPed.Position, new Vector3(v.Vestiti.X, v.Vestiti.Y, v.Vestiti.Z)) < 0.8f)
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per guardare i vestiti");
						if (Input.IsControlJustPressed(Control.Context))
						{
							Game.PlayerPed.Task.AchieveHeading(v.Vestiti.W);
							while (Game.PlayerPed.Heading > v.Vestiti.W + 5f || Game.PlayerPed.Heading < v.Vestiti.W - 5f)
							{
								await BaseScript.Delay(0);
							}

							if (Eventi.Player.CurrentChar.skin.sex == "Maschio")
							{
								MenuNegoziAbiti.MenuVest(ConfigClient.Conf.Negozi.Abiti.Maschio.DiscVest, "clothingshirt", "Discount");
							}
							else
							{
								MenuNegoziAbiti.MenuVest(ConfigClient.Conf.Negozi.Abiti.Femmina.DiscVest, "mp_clothing@female@shirt", "Discount");
							}

							cam(24818, new Vector3(0.0f, 3.0f, 0.0f), new Vector3(0), true, new Vector3(0));
							menu = true;
						}
					}
					else if (World.GetDistance(Game.PlayerPed.Position, new Vector3(v.Scarpe.X, v.Scarpe.Y, v.Scarpe.Z)) < 0.8f)
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per guardare le scarpe");
						if (Input.IsControlJustPressed(Control.Context))
						{
							Game.PlayerPed.Task.AchieveHeading(v.Scarpe.W);
							while (Game.PlayerPed.Heading > v.Scarpe.W + 5f || Game.PlayerPed.Heading < v.Scarpe.W - 5f)
							{
								await BaseScript.Delay(0);
							}

							if (Eventi.Player.CurrentChar.skin.sex == "Maschio")
							{
								MenuNegoziAbiti.MenuScarpe(ConfigClient.Conf.Negozi.Abiti.Maschio.DiscScarpe, "clothingshoes", "Discount");
							}
							else
							{
								MenuNegoziAbiti.MenuScarpe(ConfigClient.Conf.Negozi.Abiti.Femmina.DiscScarpe, "mp_clothing@female@Scarpe", "Discount");
							}

							cam(14201, new Vector3(0.0f, 1.5f, 1.0f), new Vector3(0.3f, 0.0f, 0.5f), false, new Vector3(0));
							menu = true;
						}
					}
					/*					else if (World.GetDistance(Game.PlayerPed.Position, new Vector3(v.Maglie.X, v.Maglie.Y, v.Maglie.Z)) < 0.8f)
										{
											Funzioni.ShowHelp("Premi ~INPUT_CONTEXT~ per guardare le Maglie");
											if (Input.IsControlJustPressed(Control.Context))
											{
												Game.PlayerPed.Task.AchieveHeading(v.Maglie.W);
												if (Eventi.Player.CurrentChar.skin.sex == "Maschio")
													//ApriMenu(ConfigClient.Conf.Negozi.Abiti.Maschio);
													Debug.WriteLine("Maschio");
												else
													//ApriMenu(ConfigClient.Conf.Negozi.Abiti.Maschio);
													Debug.WriteLine("Femmina");
												//cam(24818, new Vector3(0), new Vector3(0), true, new Vector3(0));
												//menu = true;
											}
										}
					*/
					else if (World.GetDistance(Game.PlayerPed.Position, new Vector3(v.Pantaloni.X, v.Pantaloni.Y, v.Pantaloni.Z)) < 0.8f)
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per guardare i pantaloni");
						if (Input.IsControlJustPressed(Control.Context))
						{
							Game.PlayerPed.Task.AchieveHeading(v.Pantaloni.W);
							while (Game.PlayerPed.Heading > v.Pantaloni.W + 5f || Game.PlayerPed.Heading < v.Pantaloni.W - 5f)
							{
								await BaseScript.Delay(0);
							}

							if (Eventi.Player.CurrentChar.skin.sex == "Maschio")
							{
								MenuNegoziAbiti.MenuPant(ConfigClient.Conf.Negozi.Abiti.Maschio.DiscPant, "clothingtrousers", "Discount");
							}
							else
							{
								MenuNegoziAbiti.MenuPant(ConfigClient.Conf.Negozi.Abiti.Femmina.DiscPant, "mp_clothing@female@trousers", "Discount");
							}

							cam(51826, new Vector3(0.0f, 1.5f, 1.0f), new Vector3(0.6f, 0.0f, 0.2f), false, new Vector3(0, 0, 0.2f));
							menu = true;
						}
					}
					else if (World.GetDistance(Game.PlayerPed.Position, new Vector3(v.Occhiali.X, v.Occhiali.Y, v.Occhiali.Z)) < 0.8f)
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per guardare gli occhiali");
						if (Input.IsControlJustPressed(Control.Context))
						{
							Game.PlayerPed.Task.AchieveHeading(v.Occhiali.W);
							while (Game.PlayerPed.Heading > v.Occhiali.W + 5f || Game.PlayerPed.Heading < v.Occhiali.W - 5f)
							{
								await BaseScript.Delay(0);
							}

							if (Eventi.Player.CurrentChar.skin.sex == "Maschio")
							{
								MenuNegoziAbiti.MenuOcchiali(ConfigClient.Conf.Negozi.Abiti.Maschio.Occhiali, "clothingspecs", "Discount");
							}
							else
							{
								MenuNegoziAbiti.MenuOcchiali(ConfigClient.Conf.Negozi.Abiti.Femmina.Occhiali, "mp_clothing@female@glasses", "Discount");
							}

							cam(31086, new Vector3(0.0f, 1.45f, 0.0f), new Vector3(0), true, new Vector3(0));
							menu = true;
						}
					}
					else if (World.GetDistance(Game.PlayerPed.Position, new Vector3(v.Accessori.X, v.Accessori.Y, v.Accessori.Z)) < 0.8f)
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per guardare gli accessori");
						if (Input.IsControlJustPressed(Control.Context))
						{
							Game.PlayerPed.Task.AchieveHeading(v.Accessori.W);
							while (Game.PlayerPed.Heading > v.Accessori.W + 5f || Game.PlayerPed.Heading < v.Accessori.W - 5f)
							{
								await BaseScript.Delay(0);
							}

							if (Eventi.Player.CurrentChar.skin.sex == "Maschio")
							{
								MenuNegoziAbiti.MenuAccessori(ConfigClient.Conf.Negozi.Abiti.Maschio.Accessori, "clothingshirt", "Discount");
							}
							else
							{
								MenuNegoziAbiti.MenuAccessori(ConfigClient.Conf.Negozi.Abiti.Femmina.Accessori, "mp_clothing@female@shirt", "Discount");
							}

							cam(24818, new Vector3(0.0f, 3.0f, 0.0f), new Vector3(0), true, new Vector3(0));
							menu = true;
						}
					}
				}
				foreach (var v in ConfigClothes.Suburban)
				{
					if (World.GetDistance(Game.PlayerPed.Position, new Vector3(v.Vestiti.X, v.Vestiti.Y, v.Vestiti.Z)) < 0.8f)
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per guardare i vestiti");
						if (Input.IsControlJustPressed(Control.Context))
						{
							Game.PlayerPed.Task.AchieveHeading(v.Vestiti.W);
							while (Game.PlayerPed.Heading > v.Vestiti.W + 5f || Game.PlayerPed.Heading < v.Vestiti.W - 5f)
							{
								await BaseScript.Delay(0);
							}

							if (Eventi.Player.CurrentChar.skin.sex == "Maschio")
							{
								MenuNegoziAbiti.MenuVest(ConfigClient.Conf.Negozi.Abiti.Maschio.SubVest, "clothingshirt", "Suburban");
							}
							else
							{
								MenuNegoziAbiti.MenuVest(ConfigClient.Conf.Negozi.Abiti.Femmina.SubVest, "mp_clothing@female@shirt", "Suburban");
							}

							cam(24818, new Vector3(0.0f, 3.0f, 0.0f), new Vector3(0), true, new Vector3(0));
							menu = true;
						}
					}
					else if (World.GetDistance(Game.PlayerPed.Position, new Vector3(v.Scarpe.X, v.Scarpe.Y, v.Scarpe.Z)) < 0.8f)
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per guardare le scarpe");
						if (Input.IsControlJustPressed(Control.Context))
						{
							Game.PlayerPed.Task.AchieveHeading(v.Scarpe.W);
							while (Game.PlayerPed.Heading > v.Scarpe.W + 5f || Game.PlayerPed.Heading < v.Scarpe.W - 5f)
							{
								await BaseScript.Delay(0);
							}

							if (Eventi.Player.CurrentChar.skin.sex == "Maschio")
							{
								MenuNegoziAbiti.MenuScarpe(ConfigClient.Conf.Negozi.Abiti.Maschio.SubScarpe, "clothingshoes", "Suburban");
							}
							else
							{
								MenuNegoziAbiti.MenuScarpe(ConfigClient.Conf.Negozi.Abiti.Femmina.SubScarpe, "mp_clothing@female@Scarpe", "Suburban");
							}

							cam(14201, new Vector3(0.0f, 1.5f, 1.0f), new Vector3(0.6f, 0.0f, 0.5f), false, new Vector3(0));
							menu = true;
						}
					}
					/*					else if (World.GetDistance(Game.PlayerPed.Position, new Vector3(v.Maglie.X, v.Maglie.Y, v.Maglie.Z)) < 0.8f)
										{
											Funzioni.ShowHelp("Premi ~INPUT_CONTEXT~ per guardare le Maglie");
											if (Input.IsControlJustPressed(Control.Context))
											{
												Game.PlayerPed.Task.AchieveHeading(v.Maglie.W);
												if (Eventi.Player.CurrentChar.skin.sex == "Maschio")
													//ApriMenu(ConfigClient.Conf.Negozi.Abiti.Maschio);
													Debug.WriteLine("Maschio");
												else
													//ApriMenu(ConfigClient.Conf.Negozi.Abiti.Maschio);
													Debug.WriteLine("Femmina");
												//cam(24818, new Vector3(0), new Vector3(0), true, new Vector3(0));
												//menu = true;
											}
										}
					*/
					else if (World.GetDistance(Game.PlayerPed.Position, new Vector3(v.Pantaloni.X, v.Pantaloni.Y, v.Pantaloni.Z)) < 0.8f)
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per guardare i pantaloni");
						if (Input.IsControlJustPressed(Control.Context))
						{
							Game.PlayerPed.Task.AchieveHeading(v.Pantaloni.W);
							while (Game.PlayerPed.Heading > v.Pantaloni.W + 5f || Game.PlayerPed.Heading < v.Pantaloni.W - 5f)
							{
								await BaseScript.Delay(0);
							}

							if (Eventi.Player.CurrentChar.skin.sex == "Maschio")
							{
								MenuNegoziAbiti.MenuPant(ConfigClient.Conf.Negozi.Abiti.Maschio.SubPant, "clothingtrousers", "Suburban");
							}
							else
							{
								MenuNegoziAbiti.MenuPant(ConfigClient.Conf.Negozi.Abiti.Femmina.SubPant, "mp_clothing@female@trousers", "Suburban");
							}

							cam(51826, new Vector3(0.0f, 1.5f, 1.0f), new Vector3(0.6f, 0.0f, 0.2f), false, new Vector3(0, 0, 0.2f));
							menu = true;
						}
					}
					else if (World.GetDistance(Game.PlayerPed.Position, new Vector3(v.Occhiali.X, v.Occhiali.Y, v.Occhiali.Z)) < 0.8f)
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per guardare gli occhiali");
						if (Input.IsControlJustPressed(Control.Context))
						{
							Game.PlayerPed.Task.AchieveHeading(v.Occhiali.W);
							while (Game.PlayerPed.Heading > v.Occhiali.W + 5f || Game.PlayerPed.Heading < v.Occhiali.W - 5f)
							{
								await BaseScript.Delay(0);
							}

							if (Eventi.Player.CurrentChar.skin.sex == "Maschio")
							{
								MenuNegoziAbiti.MenuOcchiali(ConfigClient.Conf.Negozi.Abiti.Maschio.Occhiali, "clothingspecs", "Suburban");
							}
							else
							{
								MenuNegoziAbiti.MenuOcchiali(ConfigClient.Conf.Negozi.Abiti.Femmina.Occhiali, "mp_clothing@female@glasses", "Suburban");
							}

							cam(31086, new Vector3(0.0f, 1.45f, 0.0f), new Vector3(0), true, new Vector3(0));
							menu = true;
						}
					}
					else if (World.GetDistance(Game.PlayerPed.Position, new Vector3(v.Accessori.X, v.Accessori.Y, v.Accessori.Z)) < 0.8f)
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per guardare gli accessori");
						if (Input.IsControlJustPressed(Control.Context))
						{
							Game.PlayerPed.Task.AchieveHeading(v.Accessori.W);
							while (Game.PlayerPed.Heading > v.Accessori.W + 5f || Game.PlayerPed.Heading < v.Accessori.W - 5f)
							{
								await BaseScript.Delay(0);
							}

							if (Eventi.Player.CurrentChar.skin.sex == "Maschio")
							{
								MenuNegoziAbiti.MenuAccessori(ConfigClient.Conf.Negozi.Abiti.Maschio.Accessori, "clothingshirt", "Suburban");
							}
							else
							{
								MenuNegoziAbiti.MenuAccessori(ConfigClient.Conf.Negozi.Abiti.Femmina.Accessori, "mp_clothing@female@shirt", "Suburban");
							}

							cam(24818, new Vector3(0.0f, 3.0f, 0.0f), new Vector3(0), true, new Vector3(0));
							menu = true;
						}
					}
				}
				foreach (var v in ConfigClothes.Ponsombys)
				{
					if (World.GetDistance(Game.PlayerPed.Position, new Vector3(v.Vestiti.X, v.Vestiti.Y, v.Vestiti.Z)) < 0.8f)
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per guardare i vestiti");
						if (Input.IsControlJustPressed(Control.Context))
						{
							Game.PlayerPed.Task.AchieveHeading(v.Vestiti.W);
							while (Game.PlayerPed.Heading > v.Vestiti.W + 5f || Game.PlayerPed.Heading < v.Vestiti.W - 5f)
							{
								await BaseScript.Delay(0);
							}

							if (Eventi.Player.CurrentChar.skin.sex == "Maschio")
							{
								MenuNegoziAbiti.MenuVest(ConfigClient.Conf.Negozi.Abiti.Maschio.PonsVest, "clothingshirt", "Ponsombys");
							}
							else
							{
								MenuNegoziAbiti.MenuVest(ConfigClient.Conf.Negozi.Abiti.Femmina.PonsVest, "mp_clothing@female@shirt", "Ponsombys");
							}

							cam(24818, new Vector3(0.0f, 3.0f, 0.0f), new Vector3(0), true, new Vector3(0));
							menu = true;
						}
					}
					else if (World.GetDistance(Game.PlayerPed.Position, new Vector3(v.Scarpe.X, v.Scarpe.Y, v.Scarpe.Z)) < 0.8f)
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per guardare le scarpe");
						if (Input.IsControlJustPressed(Control.Context))
						{
							Game.PlayerPed.Task.AchieveHeading(v.Scarpe.W);
							while (Game.PlayerPed.Heading > v.Scarpe.W + 5f || Game.PlayerPed.Heading < v.Scarpe.W - 5f)
							{
								await BaseScript.Delay(0);
							}

							if (Eventi.Player.CurrentChar.skin.sex == "Maschio")
							{
								MenuNegoziAbiti.MenuScarpe(ConfigClient.Conf.Negozi.Abiti.Maschio.PonsScarpe, "clothingshoes", "Ponsombys");
							}
							else
							{
								MenuNegoziAbiti.MenuScarpe(ConfigClient.Conf.Negozi.Abiti.Femmina.PonsScarpe, "mp_clothing@female@Scarpe", "Ponsombys");
							}

							cam(14201, new Vector3(0.0f, 1.5f, 1.0f), new Vector3(0.6f, 0.0f, 0.5f), false, new Vector3(0));
							menu = true;
						}
					}
					/*					else if (World.GetDistance(Game.PlayerPed.Position, new Vector3(v.Maglie.X, v.Maglie.Y, v.Maglie.Z)) < 0.8f)
										{
											Funzioni.ShowHelp("Premi ~INPUT_CONTEXT~ per guardare le Maglie");
											if (Input.IsControlJustPressed(Control.Context))
											{
												Game.PlayerPed.Task.AchieveHeading(v.Maglie.W);
												if (Eventi.Player.CurrentChar.skin.sex == "Maschio")
													//ApriMenu(ConfigClient.Conf.Negozi.Abiti.Maschio);
													Debug.WriteLine("Maschio");
												else
													//ApriMenu(ConfigClient.Conf.Negozi.Abiti.Maschio);
													Debug.WriteLine("Femmina");
												//cam(24818, new Vector3(0), new Vector3(0), true, new Vector3(0));
												//menu = true;
											}
										}
					*/
					else if (World.GetDistance(Game.PlayerPed.Position, new Vector3(v.Pantaloni.X, v.Pantaloni.Y, v.Pantaloni.Z)) < 0.8f)
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per guardare i pantaloni");
						if (Input.IsControlJustPressed(Control.Context))
						{
							Game.PlayerPed.Task.AchieveHeading(v.Pantaloni.W);
							while (Game.PlayerPed.Heading > v.Pantaloni.W + 5f || Game.PlayerPed.Heading < v.Pantaloni.W - 5f)
							{
								await BaseScript.Delay(0);
							}

							if (Eventi.Player.CurrentChar.skin.sex == "Maschio")
							{
								MenuNegoziAbiti.MenuPant(ConfigClient.Conf.Negozi.Abiti.Maschio.PonsPant, "clothingtrousers", "Ponsombys");
							}
							else
							{
								MenuNegoziAbiti.MenuPant(ConfigClient.Conf.Negozi.Abiti.Femmina.PonsPant, "mp_clothing@female@trousers", "Ponsombys");
							}

							cam(51826, new Vector3(0.0f, 1.5f, 1.0f), new Vector3(0.6f, 0.0f, 0.2f), false, new Vector3(0, 0, 0.2f));
							menu = true;
						}
					}
					else if (World.GetDistance(Game.PlayerPed.Position, new Vector3(v.Accessori.X, v.Accessori.Y, v.Accessori.Z)) < 0.8f)
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per guardare gli accessori");
						if (Input.IsControlJustPressed(Control.Context))
						{
							Game.PlayerPed.Task.AchieveHeading(v.Accessori.W);
							while (Game.PlayerPed.Heading > v.Accessori.W + 5f || Game.PlayerPed.Heading < v.Accessori.W - 5f)
							{
								await BaseScript.Delay(0);
							}

							if (Eventi.Player.CurrentChar.skin.sex == "Maschio")
							{
								MenuNegoziAbiti.MenuAccessori(ConfigClient.Conf.Negozi.Abiti.Maschio.Accessori, "clothingshirt", "Ponsombys");
							}
							else
							{
								MenuNegoziAbiti.MenuAccessori(ConfigClient.Conf.Negozi.Abiti.Femmina.Accessori, "mp_clothing@female@shirt", "Ponsombys");
							}

							cam(24818, new Vector3(0.0f, 3.0f, 0.0f), new Vector3(0), true, new Vector3(0));
							menu = true;
						}
					}
				}
			}
			await Task.FromResult(0);
		}

		public static async Task Esci()
		{
			menu = false;
			ClearPedTasks(PlayerPedId());
			ClearPedSecondaryTask(PlayerPedId());
			SetBlockingOfNonTemporaryEvents(PlayerPedId(), false);
			ClearPedAlternateMovementAnim(PlayerPedId(), 0, 4.0f);
			if (!menu)
			{
				RenderScriptCams(false, true, 1500, true, false);
				camm.IsActive = false;
				DestroyAllCams(false);
				SetFrozenRenderingDisabled(true);
			}
			await Task.FromResult(0);
		}
	}
}
