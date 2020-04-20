using CitizenFX.Core;
using Logger;
using NuovaGM.Client.gmPrincipale;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Client.MenuNativo;
using NuovaGM.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using static CitizenFX.Core.Native.API;

namespace NuovaGM.Client.Negozi
{
	static class Armerie
	{
		private static MenuPool pool = HUD.MenuPool;
		static List<ArmiLicenza> armi1 = new List<ArmiLicenza>()
		{
			new ArmiLicenza("WEAPON_PISTOL", 300),
			new ArmiLicenza("WEAPON_FLASHLIGHT", 60),
			new ArmiLicenza("WEAPON_MACHETE", 90),
			new ArmiLicenza("WEAPON_NIGHTSTICK", 150),
			new ArmiLicenza("WEAPON_BAT", 100),
			new ArmiLicenza("WEAPON_FIREEXTINGUISHER", 100),
			new ArmiLicenza("WEAPON_BALL", 50),
			new ArmiLicenza("WEAPON_SMOKEGRENADE", 100),
		};

		static List<ArmiLicenza> armi2 = new List<ArmiLicenza>()
		{
			new ArmiLicenza("WEAPON_MICROSMG", 1400),
			new ArmiLicenza("WEAPON_PUMPSHOTGUN", 3400),
			new ArmiLicenza("WEAPON_ASSAULTRIFLE", 10000),
			new ArmiLicenza("WEAPON_SPECIALCARBINE", 15000),
			new ArmiLicenza("WEAPON_SNIPERRIFLE", 22000),
		};

		static List<ArmiLicenza> armi3 = new List<ArmiLicenza>()
		{
			new ArmiLicenza("WEAPON_APPISTOL", 1100),
			new ArmiLicenza("WEAPON_CARBINERIFLE", 12000),
			new ArmiLicenza("WEAPON_HEAVYSNIPER", 30000),
			new ArmiLicenza("WEAPON_MINIGUN", 45000),
			new ArmiLicenza("WEAPON_RAILGUN", 50000),
			new ArmiLicenza("WEAPON_STICKYBOMB", 500)
		};

		static List<ArmiLicenza> componenti = new List<ArmiLicenza>()
		{
			new ArmiLicenza("COMPONENT_AT_SCOPE_MACRO", 1000),
			new ArmiLicenza("COMPONENT_AT_PI_FLSH", 500),
		};

		public static List<ArmiLicenza> tinte = new List<ArmiLicenza>()
		{
			new ArmiLicenza("WM_TINT0", 500),
			new ArmiLicenza("WM_TINT1", 500),
			new ArmiLicenza("WM_TINT2", 500),
			new ArmiLicenza("WM_TINT3", 500),
			new ArmiLicenza("WM_TINT4", 500),
			new ArmiLicenza("WM_TINT5", 500),
			new ArmiLicenza("WM_TINT6", 500),
			new ArmiLicenza("WM_TINT7", 500),
		};

		static UIMenuItem attTi = null;

		public static async void NuovaArmeria()
		{
			UIMenu Armeria = new UIMenu(" ", "La drogheria delle armi", new System.Drawing.Point(0, 0), Main.Textures["AmmuNation"].Key, Main.Textures["AmmuNation"].Value);
			HUD.MenuPool.Add(Armeria);
			UIMenu ArmiLic1 = pool.AddSubMenu(Armeria, "Armi Licenza Base", "Scegli l'arma che preferisci");
			UIMenu ArmiLic2 = pool.AddSubMenu(Armeria, "Armi Licenza Intermedia", "Scegli l'arma che preferisci");
			UIMenu ArmiLic3 = pool.AddSubMenu(Armeria, "Armi Licenza Avanzata", "Scegli l'arma che preferisci");
			UIMenu component = pool.AddSubMenu(Armeria, "Componenti", "Scegli il componente giusto!\nNB: Caricatori, silenziatori e mirini avanzati non sono in vendita!");
			UIMenu Tinte = pool.AddSubMenu(Armeria, "Colori", "Scegli il colore per le tue armi!");
			Armeria.OnMenuOpen += (menuBase) =>
			{
				if (!Game.Player.GetPlayerData().hasLicense("Armi1"))
				{
					ArmiLic1.ParentItem.Enabled = false;
					ArmiLic1.ParentItem.SetRightBadge(UIMenuItem.BadgeStyle.Lock);
				}
				if (!Game.Player.GetPlayerData().hasLicense("Armi2"))
				{
					ArmiLic2.ParentItem.Enabled = false;
					ArmiLic2.ParentItem.SetRightBadge(UIMenuItem.BadgeStyle.Lock);
				}
				if (!Game.Player.GetPlayerData().hasLicense("Armi3"))
				{
					ArmiLic3.ParentItem.Enabled = false;
					ArmiLic3.ParentItem.SetRightBadge(UIMenuItem.BadgeStyle.Lock);
				}
				if (Game.Player.GetPlayerData().CurrentChar.weapons.Count == 0)
				{
					component.ParentItem.Enabled = false;
					component.ParentItem.SetRightBadge(UIMenuItem.BadgeStyle.Lock);
					Tinte.ParentItem.Enabled = false;
					Tinte.ParentItem.SetRightBadge(UIMenuItem.BadgeStyle.Lock);
				}
				#region ArmiBase
				ArmiLic1.OnMenuOpen += async (menu) =>
				{
					ArmiLic1.Clear();
					for (int i = 0; i < armi1.Count; i++)
					{
						UIMenuItem arma = new UIMenuItem(GetLabelText(Funzioni.GetWeaponLabel(Funzioni.HashUint(armi1[i].name))));
						if ((Game.Player.GetPlayerData().Money >= armi1[i].price) || (Game.Player.GetPlayerData().Bank >= armi1[i].price))
							arma.SetRightLabel("~g~" + armi1[i].price + "$");
						else
							arma.SetRightLabel("~r~" + armi1[i].price + "$");

						if (Game.PlayerPed.Weapons.HasWeapon((WeaponHash)Funzioni.HashUint(armi1[i].name)))
							arma.SetRightBadge(UIMenuItem.BadgeStyle.Gun);
						ArmiLic1.AddItem(arma);
					}
					ArmiLic1.OnItemSelect += (_menu, _item, _index) =>
					{
						if (Game.PlayerPed.Weapons.HasWeapon((WeaponHash)Funzioni.HashUint(armi1[_index].name)))
						{
							if (Game.Player.GetPlayerData().Money >= 150)
							{
								AddAmmoToPed(Game.PlayerPed.Handle, Funzioni.HashUint(armi1[_index].name), 250);
								HUD.ShowNotification("Poichè già possiedi quest'arma, ti sono state ricaricate le munizioni al prezzo di 150$");
								BaseScript.TriggerServerEvent("lprp:removemoney", 150);
							}
							else
							{
								if (Game.Player.GetPlayerData().Bank >= 150)
								{
									AddAmmoToPed(Game.PlayerPed.Handle, Funzioni.HashUint(armi1[_index].name), 250);
									HUD.ShowNotification("Poichè già possiedi quest'arma, ti sono state ricaricate le munizioni al prezzo di 150$");
									BaseScript.TriggerServerEvent("lprp:removebank", 150);
								}
								else
									HUD.ShowNotification("Non possiedi i soldi necessari ad acquistare le munizioni per quest'arma!");
							}
						}
						else
						{
							if (Game.Player.GetPlayerData().Money >= armi1[_index].price)
							{
								BaseScript.TriggerServerEvent("lprp:addWeapon", armi1[_index].name, 250);
								BaseScript.TriggerServerEvent("lprp:removemoney", armi1[_index].price);
								HUD.ShowNotification("Hai acquistato un/a ~y~" + GetLabelText(Funzioni.GetWeaponLabel(Funzioni.HashUint(armi1[_index].name))));
								_item.SetRightBadge(UIMenuItem.BadgeStyle.Gun);
							}
							else
							{
								if (Game.Player.GetPlayerData().Money >= armi1[_index].price)
								{
									BaseScript.TriggerServerEvent("lprp:addWeapon", armi1[_index].name, 250);
									BaseScript.TriggerServerEvent("lprp:removebank", armi1[_index].price);
									HUD.ShowNotification("Hai acquistato un/a ~y~" + GetLabelText(Funzioni.GetWeaponLabel(Funzioni.HashUint(armi1[_index].name))));
									_item.SetRightBadge(UIMenuItem.BadgeStyle.Gun);
								}
								else
									HUD.ShowNotification("Non possiedi i soldi necessari ad acquistare quest'arma!");
							}
						}
					};
				};
				#endregion
				#region ArmiMedie
				ArmiLic2.OnMenuOpen += async (menu) =>
				{
					ArmiLic2.Clear();
					for (int i = 0; i < armi2.Count; i++)
					{
						UIMenuItem arma = new UIMenuItem(GetLabelText(Funzioni.GetWeaponLabel(Funzioni.HashUint(armi2[i].name))));
						if ((Game.Player.GetPlayerData().Money >= armi2[i].price) || (Game.Player.GetPlayerData().Bank >= armi2[i].price))
							arma.SetRightLabel("~g~" + armi2[i].price + "$");
						else
							arma.SetRightLabel("~r~" + armi2[i].price + "$");

						if (Game.PlayerPed.Weapons.HasWeapon((WeaponHash)Funzioni.HashUint(armi2[i].name)))
							arma.SetRightBadge(UIMenuItem.BadgeStyle.Gun);
						ArmiLic2.AddItem(arma);
					}
					ArmiLic2.OnItemSelect += (_menu, _item, _index) =>
					{
						if (Game.PlayerPed.Weapons.HasWeapon((WeaponHash)Funzioni.HashUint(armi2[_index].name)))
						{
							if (Game.Player.GetPlayerData().Money >= 150)
							{
								AddAmmoToPed(Game.PlayerPed.Handle, Funzioni.HashUint(armi2[_index].name), 250);
								HUD.ShowNotification("Poichè già possiedi quest'arma, ti sono state ricaricate le munizioni al prezzo di 150$");
								BaseScript.TriggerServerEvent("lprp:removemoney", 150);
							}
							else
							{
								if (Game.Player.GetPlayerData().Bank >= 150)
								{
									AddAmmoToPed(Game.PlayerPed.Handle, Funzioni.HashUint(armi2[_index].name), 250);
									HUD.ShowNotification("Poichè già possiedi quest'arma, ti sono state ricaricate le munizioni al prezzo di 150$");
									BaseScript.TriggerServerEvent("lprp:removebank", 150);
								}
								else
									HUD.ShowNotification("Non possiedi i soldi necessari ad acquistare le munizioni per quest'arma!");
							}
						}
						else
						{
							if (Game.Player.GetPlayerData().Money >= armi2[_index].price)
							{
								BaseScript.TriggerServerEvent("lprp:addWeapon", armi2[_index].name, 250);
								BaseScript.TriggerServerEvent("lprp:removemoney", armi2[_index].price);
								HUD.ShowNotification("Hai acquistato un/a ~y~" + GetLabelText(Funzioni.GetWeaponLabel(Funzioni.HashUint(armi2[_index].name))));
								_item.SetRightBadge(UIMenuItem.BadgeStyle.Gun);
							}
							else
							{
								if (Game.Player.GetPlayerData().Money >= armi2[_index].price)
								{
									BaseScript.TriggerServerEvent("lprp:addWeapon", armi2[_index].name, 250);
									BaseScript.TriggerServerEvent("lprp:removebank", armi2[_index].price);
									HUD.ShowNotification("Hai acquistato un/a ~y~" + GetLabelText(Funzioni.GetWeaponLabel(Funzioni.HashUint(armi2[_index].name))));
									_item.SetRightBadge(UIMenuItem.BadgeStyle.Gun);
								}
								else
									HUD.ShowNotification("Non possiedi i soldi necessari ad acquistare quest'arma!");
							}
						}
					};
				};
				#endregion
				#region ArmiAvanzate
				ArmiLic3.OnMenuOpen += async (menu) =>
				{
					ArmiLic3.Clear();
					for (int i = 0; i < armi3.Count; i++)
					{
						UIMenuItem arma = new UIMenuItem(GetLabelText(Funzioni.GetWeaponLabel(Funzioni.HashUint(armi3[i].name))));
						if ((Game.Player.GetPlayerData().Money >= armi3[i].price) || (Game.Player.GetPlayerData().Bank >= armi3[i].price))
							arma.SetRightLabel("~g~" + armi3[i].price + "$");
						else
							arma.SetRightLabel("~r~" + armi3[i].price + "$");

						if (Game.PlayerPed.Weapons.HasWeapon((WeaponHash)Funzioni.HashUint(armi3[i].name)))
							arma.SetRightBadge(UIMenuItem.BadgeStyle.Gun);
						ArmiLic3.AddItem(arma);
					}
					ArmiLic3.OnItemSelect += (_menu, _item, _index) =>
					{
						if (Game.PlayerPed.Weapons.HasWeapon((WeaponHash)Funzioni.HashUint(armi3[_index].name)))
						{
							if (Game.Player.GetPlayerData().Money >= 150)
							{
								AddAmmoToPed(Game.PlayerPed.Handle, Funzioni.HashUint(armi3[_index].name), 250);
								HUD.ShowNotification("Poichè già possiedi quest'arma, ti sono state ricaricate le munizioni al prezzo di 150$");
								BaseScript.TriggerServerEvent("lprp:removemoney", 150);
							}
							else
							{
								if (Game.Player.GetPlayerData().Bank >= 150)
								{
									AddAmmoToPed(Game.PlayerPed.Handle, Funzioni.HashUint(armi3[_index].name), 250);
									HUD.ShowNotification("Poichè già possiedi quest'arma, ti sono state ricaricate le munizioni al prezzo di 150$");
									BaseScript.TriggerServerEvent("lprp:removebank", 150);
								}
								else
									HUD.ShowNotification("Non possiedi i soldi necessari ad acquistare le munizioni per quest'arma!");
							}
						}
						else
						{
							if (Game.Player.GetPlayerData().Money >= armi3[_index].price)
							{
								BaseScript.TriggerServerEvent("lprp:addWeapon", armi3[_index].name, 250);
								BaseScript.TriggerServerEvent("lprp:removemoney", armi3[_index].price);
								HUD.ShowNotification("Hai acquistato un/a ~y~" + GetLabelText(Funzioni.GetWeaponLabel(Funzioni.HashUint(armi3[_index].name))));
								_item.SetRightBadge(UIMenuItem.BadgeStyle.Gun);
							}
							else
							{
								if (Game.Player.GetPlayerData().Money >= armi3[_index].price)
								{
									BaseScript.TriggerServerEvent("lprp:addWeapon", armi3[_index].name, 250);
									BaseScript.TriggerServerEvent("lprp:removebank", armi3[_index].price);
									HUD.ShowNotification("Hai acquistato un/a ~y~" + GetLabelText(Funzioni.GetWeaponLabel(Funzioni.HashUint(armi3[_index].name))));
									_item.SetRightBadge(UIMenuItem.BadgeStyle.Gun);
								}
								else
									HUD.ShowNotification("Non possiedi i soldi necessari ad acquistare quest'arma!");
							}
						}
					};
				};
				#endregion
				#region Componenti
				component.OnMenuOpen += (menu) =>
				{
					component.Clear();
					foreach (Weapons armi in Game.Player.GetPlayerData().CurrentChar.weapons)
					{
						if (SharedScript.hasComponents(armi.name))
						{
							UIMenu Arma = pool.AddSubMenu(component, GetLabelText(Funzioni.GetWeaponLabel(Funzioni.HashUint(armi.name))), "Vedi qui i componenti acquistabili per la tua arma");
							foreach (ArmiLicenza co in componenti)
							{
								if (SharedScript.hasWeaponComponent(armi.name, co.name))
								{
									UIMenuItem compon = new UIMenuItem(GetLabelText(Funzioni.GetWeaponLabel(Funzioni.HashUint(co.name))));
									Arma.AddItem(compon);
									if (Game.Player.GetPlayerData().Money >= co.price || Game.Player.GetPlayerData().Bank >= co.price)
										compon.SetRightLabel("~g~" + co.price + "$");
									else
										compon.SetRightLabel("~r~" + co.price + "$");
									for (int k = 0; k < armi.components.Count; k++)
										if (armi.components[k].name == co.name)
											compon.SetRightBadge(UIMenuItem.BadgeStyle.Ammo);

								}
							}
							Arma.OnItemSelect += async (_menu, _item, _index) =>
							{
								if (_item.RightBadge == UIMenuItem.BadgeStyle.Ammo)
									HUD.ShowNotification("Hai già acquistato questo componente!!", true);
								else
								{
									ArmiLicenza arm = componenti.FirstOrDefault(x => GetLabelText(Funzioni.GetWeaponLabel(Funzioni.HashUint(x.name))) == _item.Text);

									Log.Printa(LogType.Debug, "Prezzo = " + arm.price);
									Log.Printa(LogType.Debug, "name = " + arm.name);
									if (Game.Player.GetPlayerData().Money >= arm.price)
									{
										BaseScript.TriggerServerEvent("lprp:addWeaponComponent", armi.name, arm.name);
										BaseScript.TriggerServerEvent("lprp:removemoney", arm.price);
										HUD.ShowNotification("Hai acquistato un/a ~y~" + GetLabelText(Funzioni.GetWeaponLabel(Funzioni.HashUint(arm.name))));
										_item.SetRightBadge(UIMenuItem.BadgeStyle.Ammo);
									}
									else
									{
										if (Game.Player.GetPlayerData().Bank >= arm.price)
										{
											BaseScript.TriggerServerEvent("lprp:addWeaponComponent", armi.name, arm.name);
											BaseScript.TriggerServerEvent("lprp:removebank", arm.price);
											HUD.ShowNotification("Hai acquistato un/a ~y~" + GetLabelText(Funzioni.GetWeaponLabel(Funzioni.HashUint(arm.name))));
											_item.SetRightBadge(UIMenuItem.BadgeStyle.Ammo);
										}
										else
										{
											HUD.ShowNotification("Non possiedi i soldi necessari ad acquistare questo componente!", true);
										}
									}
								}
							};
						}
					}
				};
				#endregion
				#region Tinte
				Tinte.OnMenuOpen += (menu) =>
				{
					Tinte.Clear();
					foreach (Weapons armi in Game.Player.GetPlayerData().CurrentChar.weapons)
					{
						bool Hastints = SharedScript.hasTints(armi.name);
						if (Hastints) 
						{
							UIMenu Tnt = pool.AddSubMenu(Tinte, GetLabelText(Funzioni.GetWeaponLabel(Funzioni.HashUint(armi.name))), "Vedi qui i colori acquistabili per la tua arma");
							foreach (var tin in tinte)
							{
								UIMenuItem tintina = new UIMenuItem(GetLabelText(Funzioni.GetWeaponLabel(Funzioni.HashUint(tin.name))));
								Tnt.AddItem(tintina);
								if ((Game.Player.GetPlayerData().Money >= tin.price) || (Game.Player.GetPlayerData().Bank >= tin.price))
									tintina.SetRightLabel("~g~" + tin.price + "$");
								else
									tintina.SetRightLabel("~r~" + tin.price + "$");

								if (Game.Player.GetPlayerData().hasWeaponTint(armi.name, Convert.ToInt32(tin.name.Substring(7))))
									tintina.SetRightBadge(UIMenuItem.BadgeStyle.Ammo);

							}
							Tnt.OnItemSelect += async (_menu, _item, _index) =>
							{
								if (_item.RightBadge == UIMenuItem.BadgeStyle.Ammo)
									HUD.ShowNotification("Hai già acquistato questo colore!!", true);
								else
								{
									if (Game.Player.GetPlayerData().Money >= tinte[_index].price)
									{
										BaseScript.TriggerServerEvent("lprp:removemoney", tinte[_index].price);
										BaseScript.TriggerServerEvent("lprp:addWeaponTint", armi.name, _index);
										HUD.ShowNotification("Hai acquistato un/a ~y~" + GetLabelText(Funzioni.GetWeaponLabel(Funzioni.HashUint(tinte[_index].name))));
										_menu.MenuItems.ForEach(x => x.SetRightBadge(UIMenuItem.BadgeStyle.None));
										//attTi.SetRightBadge(UIMenuItem.BadgeStyle.None);
										//attTi = _item;
										_item.SetRightBadge(UIMenuItem.BadgeStyle.Ammo);
										armi.tint = _index;
									}
									else
									{
										if (Game.Player.GetPlayerData().Bank >= tinte[_index].price)
										{
											BaseScript.TriggerServerEvent("lprp:removebank", tinte[_index].price);
											BaseScript.TriggerServerEvent("lprp:addWeaponTint", armi.name, _index);
											HUD.ShowNotification("Hai acquistato un/a ~y~" + GetLabelText(Funzioni.GetWeaponLabel(Funzioni.HashUint(tinte[_index].name))));
											_menu.MenuItems.ForEach(x => x.SetRightBadge(UIMenuItem.BadgeStyle.None));
											//attTi.SetRightBadge(UIMenuItem.BadgeStyle.None);
											//attTi = _item;
											_item.SetRightBadge(UIMenuItem.BadgeStyle.Ammo);
											armi.tint = _index;
										}
										else
											HUD.ShowNotification("Non possiedi i soldi necessari ad acquistare questo colore!", true);
									}
								}
							};
						}
					}
				};
				#endregion
			};
			Armeria.Visible = true;
		}


		public static async void ArmeriaMenu()
		{
			UIMenu Armeria = new UIMenu(" ", "La drogheria delle armi", new System.Drawing.Point(0, 0), Main.Textures["AmmuNation"].Key, Main.Textures["AmmuNation"].Value);
			UIMenu ArmiLic1 = pool.AddSubMenu(Armeria, "Armi Licenza Base", "Scegli l'arma che preferisci");
			UIMenu ArmiLic2 = pool.AddSubMenu(Armeria, "Armi Licenza Intermedia", "Scegli l'arma che preferisci");
			UIMenu ArmiLic3 = pool.AddSubMenu(Armeria, "Armi Licenza Avanzata", "Scegli l'arma che preferisci");
			pool.Add(Armeria);
			ArmiLic1.OnMenuOpen += (menu) =>
			{
				ArmiLic1.Clear();
				for (int i = 0; i < armi1.Count; i++)
				{
					UIMenuItem arma = new UIMenuItem(GetLabelText(Funzioni.GetWeaponLabel(Funzioni.HashUint(armi1[i].name))));
					if ((Game.Player.GetPlayerData().Money >= armi1[i].price) || (Game.Player.GetPlayerData().Bank >= armi1[i].price))
						arma.SetRightLabel("~g~" + armi1[i].price + "$");
					else
						arma.SetRightLabel("~r~" + armi1[i].price + "$");

					if (Game.PlayerPed.Weapons.HasWeapon((WeaponHash)Funzioni.HashUint(armi1[i].name)))
						arma.SetRightBadge(UIMenuItem.BadgeStyle.Gun);
					ArmiLic1.AddItem(arma);

					ArmiLic1.OnItemSelect += (_menu, _item, _index) =>
					{
						if (Game.PlayerPed.Weapons.HasWeapon((WeaponHash)Funzioni.HashUint(armi1[_index].name)))
						{
							if (Game.Player.GetPlayerData().Money >= 150)
							{
								AddAmmoToPed(Game.PlayerPed.Handle, Funzioni.HashUint(armi1[_index].name), 250);
								HUD.ShowNotification("Poichè già possiedi quest'arma, ti sono state ricaricate le munizioni al prezzo di 150$");
								BaseScript.TriggerServerEvent("lprp:removemoney", 150);
							}
							else
							{
								if (Game.Player.GetPlayerData().Bank >= 150)
								{
									AddAmmoToPed(Game.PlayerPed.Handle, Funzioni.HashUint(armi1[_index].name), 250);
									HUD.ShowNotification("Poichè già possiedi quest'arma, ti sono state ricaricate le munizioni al prezzo di 150$");
									BaseScript.TriggerServerEvent("lprp:removebank", 150);
								}
								else
								{
									HUD.ShowNotification("Non possiedi i soldi necessari ad acquistare le munizioni per quest'arma!");
								}
							}
						}
						else
						{
							if (Game.Player.GetPlayerData().Money >= armi1[_index].price)
							{
								BaseScript.TriggerServerEvent("lprp:addWeapon", armi1[_index].name, 250);
								BaseScript.TriggerServerEvent("lprp:removemoney", armi1[_index].price);
								HUD.ShowNotification("Hai acquistato un/a ~y~" + GetLabelText(Funzioni.GetWeaponLabel(Funzioni.HashUint(armi1[_index].name))));
								_item.SetRightBadge(UIMenuItem.BadgeStyle.Gun);
							}
							else
							{
								if (Game.Player.GetPlayerData().Money >= armi1[_index].price)
								{
									BaseScript.TriggerServerEvent("lprp:addWeapon", armi1[_index].name, 250);
									BaseScript.TriggerServerEvent("lprp:removebank", armi1[_index].price);
									HUD.ShowNotification("Hai acquistato un/a ~y~" + GetLabelText(Funzioni.GetWeaponLabel(Funzioni.HashUint(armi1[i].name))));
									_item.SetRightBadge(UIMenuItem.BadgeStyle.Gun);
								}
								else
								{
									HUD.ShowNotification("Non possiedi i soldi necessari ad acquistare quest'arma!");
								}
							}
						}
					};
				}
			};
			ArmiLic2.OnMenuOpen += (menu) =>
			{
				ArmiLic2.Clear();
				for (int i = 0; i < armi2.Count; i++)
				{
					UIMenuItem arma = new UIMenuItem(GetLabelText(Funzioni.GetWeaponLabel(Funzioni.HashUint(armi2[i].name))));
					if ((Game.Player.GetPlayerData().Money >= armi2[i].price) || (Game.Player.GetPlayerData().Bank >= armi2[i].price))
					{
						arma.SetRightLabel("~g~" + armi2[i].price + "$");
					}
					else
					{
						arma.SetRightLabel("~r~" + armi2[i].price + "$");
					}

					if (Game.PlayerPed.Weapons.HasWeapon((WeaponHash)Funzioni.HashUint(armi2[i].name)))
					{
						arma.SetRightBadge(UIMenuItem.BadgeStyle.Gun);
					}

					ArmiLic2.AddItem(arma);
					ArmiLic2.OnItemSelect += (_menu, _item, _index) =>
					{
						if (Game.PlayerPed.Weapons.HasWeapon((WeaponHash)Funzioni.HashUint(armi2[_index].name)))
						{
							if (Game.Player.GetPlayerData().Money >= 150)
							{
								AddAmmoToPed(Game.PlayerPed.Handle, Funzioni.HashUint(armi2[_index].name), 250);
								HUD.ShowNotification("Poichè già possiedi quest'arma, ti sono state ricaricate le munizioni al prezzo di 150$");
								BaseScript.TriggerServerEvent("lprp:removemoney", 150);
							}
							else
							{
								if (Game.Player.GetPlayerData().Bank >= 150)
								{
									AddAmmoToPed(Game.PlayerPed.Handle, Funzioni.HashUint(armi2[_index].name), 250);
									HUD.ShowNotification("Poichè già possiedi quest'arma, ti sono state ricaricate le munizioni al prezzo di 150$");
									BaseScript.TriggerServerEvent("lprp:removebank", 150);
								}
								else
								{
									HUD.ShowNotification("Non possiedi i soldi necessari ad acquistare le munizioni per quest'arma!");
								}
							}
						}
						else
						{
							if (Game.Player.GetPlayerData().Money >= armi2[_index].price)
							{
								BaseScript.TriggerServerEvent("lprp:addWeapon", armi2[_index].name, 250);
								BaseScript.TriggerServerEvent("lprp:removemoney", armi2[_index].price);
								HUD.ShowNotification("Hai acquistato un/a ~y~" + GetLabelText(Funzioni.GetWeaponLabel(Funzioni.HashUint(armi2[i].name))));
								_item.SetRightBadge(UIMenuItem.BadgeStyle.Gun);
							}
							else
							{
								if (Game.Player.GetPlayerData().Money >= armi2[_index].price)
								{
									BaseScript.TriggerServerEvent("lprp:addWeapon", armi2[_index].name, 250);
									BaseScript.TriggerServerEvent("lprp:removebank", armi2[_index].price);
									HUD.ShowNotification("Hai acquistato un/a ~y~" + GetLabelText(Funzioni.GetWeaponLabel(Funzioni.HashUint(armi2[i].name))));
									_item.SetRightBadge(UIMenuItem.BadgeStyle.Gun);
								}
								else
								{
									HUD.ShowNotification("Non possiedi i soldi necessari ad acquistare quest'arma!");
								}
							}
						}
					};
				}
			};
			ArmiLic3.OnMenuOpen += (menu) =>
			{
				ArmiLic3.Clear();
				for (int i = 0; i < armi3.Count; i++)
				{
					UIMenuItem arma = new UIMenuItem(GetLabelText(Funzioni.GetWeaponLabel(Funzioni.HashUint(armi3[i].name))));
					if ((Game.Player.GetPlayerData().Money >= armi3[i].price) || (Game.Player.GetPlayerData().Bank >= armi3[i].price))
					{
						arma.SetRightLabel("~g~" + armi3[i].price + "$");
					}
					else
					{
						arma.SetRightLabel("~r~" + armi3[i].price + "$");
					}

					if (Game.PlayerPed.Weapons.HasWeapon((WeaponHash)Funzioni.HashUint(armi3[i].name)))
					{
						arma.SetRightBadge(UIMenuItem.BadgeStyle.Gun);
					}

					ArmiLic3.AddItem(arma);
					ArmiLic3.OnItemSelect += async (_menu, _item, _index) =>
					{
						if (Game.PlayerPed.Weapons.HasWeapon((WeaponHash)Funzioni.HashUint(armi3[_index].name)))
						{
							if (Game.Player.GetPlayerData().Money >= 150)
							{
								AddAmmoToPed(Game.PlayerPed.Handle, Funzioni.HashUint(armi3[_index].name), 250);
								HUD.ShowNotification("Poichè già possiedi quest'arma, ti sono state ricaricate le munizioni al prezzo di 150$");
								BaseScript.TriggerServerEvent("lprp:removemoney", 150);
							}
							else
							{
								if (Game.Player.GetPlayerData().Bank >= 150)
								{
									AddAmmoToPed(Game.PlayerPed.Handle, Funzioni.HashUint(armi3[_index].name), 250);
									HUD.ShowNotification("Poichè già possiedi quest'arma, ti sono state ricaricate le munizioni al prezzo di 150$");
									BaseScript.TriggerServerEvent("lprp:removebank", 150);
								}
								else
								{
									HUD.ShowNotification("Non possiedi i soldi necessari ad acquistare le munizioni per quest'arma!");
								}
							}
						}
						else
						{
							if (Game.Player.GetPlayerData().Money >= armi3[_index].price)
							{
								BaseScript.TriggerServerEvent("lprp:addWeapon", armi3[_index].name, 250);
								BaseScript.TriggerServerEvent("lprp:removemoney", armi3[_index].price);
								HUD.ShowNotification("Hai acquistato un/a ~y~" + GetLabelText(Funzioni.GetWeaponLabel(Funzioni.HashUint(armi3[_index].name))));
								_item.SetRightBadge(UIMenuItem.BadgeStyle.Gun);
							}
							else
							{
								if (Game.Player.GetPlayerData().Money >= armi3[_index].price)
								{
									BaseScript.TriggerServerEvent("lprp:addWeapon", armi3[_index].name, 250);
									BaseScript.TriggerServerEvent("lprp:removebank", armi3[_index].price);
									HUD.ShowNotification("Hai acquistato un/a ~y~" + GetLabelText(Funzioni.GetWeaponLabel(Funzioni.HashUint(armi3[_index].name))));
									_item.SetRightBadge(UIMenuItem.BadgeStyle.Gun);
								}
								else
								{
									HUD.ShowNotification("Non possiedi i soldi necessari ad acquistare quest'arma!");
								}
							}
						}
					};
				}
			};
			UIMenu component = pool.AddSubMenu(Armeria, "Componenti", "Scegli il componente giusto!\nNB: Caricatori, silenziatori e mirini avanzati non li vendiamo!");
			component.OnMenuOpen += (menu) =>
			{
				component.Clear();
				UIMenu Arma = new UIMenu(" ", "");
				if (Game.Player.GetPlayerData().CurrentChar.weapons.Count > 0)
				{
					foreach (Weapons armi in Game.Player.GetPlayerData().CurrentChar.weapons)
					{
						for (int j = 0; j < SharedScript.Armi.Count; j++)
						{
							if ((SharedScript.Armi[j].name == armi.name) && (SharedScript.Armi[j].components.Count > 0))
							{
								Arma = pool.AddSubMenu(component, GetLabelText(Funzioni.GetWeaponLabel(Funzioni.HashUint(armi.name))), "Vedi qui i componenti acquistabili per la tua arma");
							}
						}

						foreach (ArmiLicenza co in componenti)
						{
							for (int j = 0; j < SharedScript.Armi.Count; j++)
							{
								if (SharedScript.Armi[j].name == armi.name)
								{
									foreach (Components v in SharedScript.Armi[j].components)
									{
										if (v.name == co.name)
										{
											UIMenuItem compon = new UIMenuItem(GetLabelText(Funzioni.GetWeaponLabel(Funzioni.HashUint(v.name))));
											Arma.AddItem(compon);
											if ((Game.Player.GetPlayerData().Money >= co.price) || (Game.Player.GetPlayerData().Bank >= co.price))
												compon.SetRightLabel("~g~" + co.price + "$");
											else
												compon.SetRightLabel("~r~" + co.price + "$");

											for (int k = 0; k < armi.components.Count; k++)
											{
												if (armi.components[k].name == v.name)
													compon.SetRightBadge(UIMenuItem.BadgeStyle.Ammo);
											}
											Arma.OnItemSelect += async (_menu, _item, _index) =>
											{
												for (int l = 0; l < armi.components.Count; l++)
												{
													if (armi.components[l].name == co.name)
														HUD.ShowNotification("Hai già acquistato questo componente!!", true);
													else
													{
														if (Game.Player.GetPlayerData().Money >= co.price)
														{
															BaseScript.TriggerServerEvent("lprp:addWeaponComponent", armi.name, co.name);
															BaseScript.TriggerServerEvent("lprp:removemoney", co.price);
															HUD.ShowNotification("Hai acquistato un/a ~y~" + GetLabelText(Funzioni.GetWeaponLabel(Funzioni.HashUint(co.name))));
															_item.SetRightBadge(UIMenuItem.BadgeStyle.Ammo);
														}
														else
														{
															if (Game.Player.GetPlayerData().Bank >= co.price)
															{
																BaseScript.TriggerServerEvent("lprp:addWeaponComponent", armi.name, co.name);
																BaseScript.TriggerServerEvent("lprp:removebank", co.price);
																HUD.ShowNotification("Hai acquistato un/a ~y~" + GetLabelText(Funzioni.GetWeaponLabel(Funzioni.HashUint(co.name))));
																_item.SetRightBadge(UIMenuItem.BadgeStyle.Ammo);
															}
															else
															{
																HUD.ShowNotification("Non possiedi i soldi necessari ad acquistare questo componente!", true);
															}
														}
													}
												}
											};
										}
									}
								}
							}
						}
					}
				}
				else
				{
					UIMenuItem noweaponbutton = new UIMenuItem("Non hai armi nell'inventario delle armi.", "Bravo! Abbasso le armi!");
					component.AddItem(noweaponbutton);
				}
			};
			UIMenu Tinte = pool.AddSubMenu(Armeria, "Colori", "Scegli il colore per le tue armi!");
			Tinte.OnMenuOpen += (menu) =>
			{
				Tinte.Clear();
				UIMenu Tnt = new UIMenu(" ", "");
				if (Game.Player.GetPlayerData().CurrentChar.weapons.Count > 0)
				{
					foreach (Weapons armi in Game.Player.GetPlayerData().CurrentChar.weapons)
					{
						for (int j = 0; j < SharedScript.Armi.Count; j++)
						{
							if (SharedScript.Armi[j].name == armi.name && GetWeaponDamageType(Funzioni.HashUint(armi.name)) == 3)
							{
								Tnt = pool.AddSubMenu(Tinte, GetLabelText(Funzioni.GetWeaponLabel(Funzioni.HashUint(armi.name))), "Vedi qui i colori acquistabili per la tua arma");
							}
						}

						foreach (ArmiLicenza tin in tinte)
						{
							for (int j = 0; j < SharedScript.Armi.Count; j++)
							{
								if (SharedScript.Armi[j].name == armi.name)
								{
									for (int l = 0; l < SharedScript.Armi[j].tints.Count; l++)
									{
										if (SharedScript.Armi[j].tints[l].name == tin.name)
										{
											UIMenuItem tintina = new UIMenuItem(GetLabelText(Funzioni.GetWeaponLabel(Funzioni.HashUint(tin.name))));
											Tnt.AddItem(tintina);
											if ((Game.Player.GetPlayerData().Money >= tin.price) || (Game.Player.GetPlayerData().Bank >= tin.price))
											{
												tintina.SetRightLabel("~g~" + tin.price + "$");
											}
											else
											{
												tintina.SetRightLabel("~r~" + tin.price + "$");
											}

											if (armi.tint == SharedScript.Armi[j].tints[l].value)
											{
												tintina.SetRightBadge(UIMenuItem.BadgeStyle.Ammo);
												attTi = tintina;
												//attTi = Tnt.GetUIMenuItems()[l];
												//attTi.RightIcon = UIMenuItem.Icon.AMMO;
											}
										}
									}
								}
							}
						}
						Tnt.OnItemSelect += async (_menu, _item, _index) =>
						{
							try
							{
								if (armi.tint == _index)
								{
									HUD.ShowNotification("Hai già acquistato questo colore!!", true);
									return;
								}
								else
								{
									if (Game.Player.GetPlayerData().Money >= tinte[_index].price)
									{
										BaseScript.TriggerServerEvent("lprp:removemoney", tinte[_index].price);
										BaseScript.TriggerServerEvent("lprp:addWeaponTint", armi.name, _index);
										HUD.ShowNotification("Hai acquistato un/a ~y~" + GetLabelText(Funzioni.GetWeaponLabel(Funzioni.HashUint(tinte[_index].name))));
										attTi.SetRightBadge(UIMenuItem.BadgeStyle.None);
										attTi = _item;
										attTi.SetRightBadge(UIMenuItem.BadgeStyle.Ammo);
										armi.tint = _index;
									}
									else
									{
										if (Game.Player.GetPlayerData().Bank >= tinte[_index].price)
										{
											BaseScript.TriggerServerEvent("lprp:removebank", tinte[_index].price);
											BaseScript.TriggerServerEvent("lprp:addWeaponTint", armi.name, _index);
											HUD.ShowNotification("Hai acquistato un/a ~y~" + GetLabelText(Funzioni.GetWeaponLabel(Funzioni.HashUint(tinte[_index].name))));
											attTi.SetRightBadge(UIMenuItem.BadgeStyle.None);
											attTi = _item;
											attTi.SetRightBadge(UIMenuItem.BadgeStyle.Ammo);
											armi.tint = _index;
										}
										else
										{
											HUD.ShowNotification("Non possiedi i soldi necessari ad acquistare questo colore!", true);
										}
									}
								}
							}
							catch (Exception e)
							{
								Log.Printa(LogType.Error, "Da segnalare allo scripter!!\nMessaggio NegoziClient.cs:607 = " + e.Message);
							}
						};
					}
				}
				else
				{
					UIMenuItem notintbutton = new UIMenuItem("Non hai armi nell'inventario delle armi.", "Bravo! Abbasso le armi!");
					Tinte.AddItem(notintbutton);
				}
			};
			Armeria.Visible = true;
		}
	}

	public class ArmiLicenza
	{
		public string name;
		public int price;
		public ArmiLicenza(string name, int price)
		{
			this.name = name;
			this.price = price;
		}
	}
}
