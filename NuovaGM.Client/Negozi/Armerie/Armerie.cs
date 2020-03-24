using CitizenFX.Core;
using NuovaGM.Client.gmPrincipale;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Client.MenuNativo;
using NuovaGM.Shared;
using System;
using System.Collections.Generic;
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

		public static async void ArmeriaMenu()
		{
			UIMenu Armeria = new UIMenu(" ", "La drogheria delle armi", new System.Drawing.Point(0, 0), Main.Textures["AmmuNation"].Key, Main.Textures["AmmuNation"].Value);
			UIMenu ArmiLic1 = pool.AddSubMenu(Armeria, "Armi Licenza Base", "Scegli l'arma che preferisci");
			UIMenu ArmiLic2 = pool.AddSubMenu(Armeria, "Armi Licenza Intermedia", "Scegli l'arma che preferisci");
			UIMenu ArmiLic3 = pool.AddSubMenu(Armeria, "Armi Licenza Avanzata", "Scegli l'arma che preferisci");
			pool.Add(Armeria);
			//DA TENERE PER IL CONTROLLO DELLE LICENZE
			//UIMenuItem lic1 = new UIMenuItem("Armi Licenza Base", "Scegli l'arma che preferisci");
			//UIMenuItem lic2 = new UIMenuItem("Armi Licenza Intermedia", "Scegli l'arma che preferisci");
			//UIMenuItem lic3 = new UIMenuItem("Armi Licenza Avanzata", "Scegli l'arma che preferisci");
			ArmiLic1.OnMenuOpen += (menu) =>
			{
				ArmiLic1.Clear();
				for (int i = 0; i < armi1.Count; i++)
				{
					UIMenuItem arma = new UIMenuItem(GetLabelText(Funzioni.GetWeaponLabel((uint)GetHashKey(armi1[i].name))));
					if ((Eventi.Player.Money >= armi1[i].price) || (Eventi.Player.Bank >= armi1[i].price))
					{
						arma.SetRightLabel("~g~" + armi1[i].price + "$");
					}
					else
					{
						arma.SetRightLabel("~r~" + armi1[i].price + "$");
					}

					if (Game.PlayerPed.Weapons.HasWeapon((WeaponHash)(uint)GetHashKey(armi1[i].name)))
					{
						arma.SetRightBadge(UIMenuItem.BadgeStyle.Gun);
					}

					ArmiLic1.AddItem(arma);
					ArmiLic1.OnItemSelect += (_menu, _item, _index) =>
					{
						if (Game.PlayerPed.Weapons.HasWeapon((WeaponHash)(uint)GetHashKey(armi1[_index].name)))
						{
							if (Eventi.Player.Money >= 150)
							{
								AddAmmoToPed(Game.PlayerPed.Handle, (uint)GetHashKey(armi1[_index].name), 250);
								HUD.ShowNotification("Poichè già possiedi quest'arma, ti sono state ricaricate le munizioni al prezzo di 150$");
								BaseScript.TriggerServerEvent("lprp:removemoney", 150);
							}
							else
							{
								if (Eventi.Player.Bank >= 150)
								{
									AddAmmoToPed(Game.PlayerPed.Handle, (uint)GetHashKey(armi1[_index].name), 250);
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
							if (Eventi.Player.Money >= armi1[_index].price)
							{
								BaseScript.TriggerServerEvent("lprp:addWeapon", armi1[_index].name, 250);
								BaseScript.TriggerServerEvent("lprp:removemoney", armi1[_index].price);
								HUD.ShowNotification("Hai acquistato un/a ~y~" + GetLabelText(Funzioni.GetWeaponLabel((uint)GetHashKey(armi1[_index].name))));
								_item.SetRightBadge(UIMenuItem.BadgeStyle.Gun);
							}
							else
							{
								if (Eventi.Player.Money >= armi1[_index].price)
								{
									BaseScript.TriggerServerEvent("lprp:addWeapon", armi1[_index].name, 250);
									BaseScript.TriggerServerEvent("lprp:removebank", armi1[_index].price);
									HUD.ShowNotification("Hai acquistato un/a ~y~" + GetLabelText(Funzioni.GetWeaponLabel((uint)GetHashKey(armi1[i].name))));
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
					UIMenuItem arma = new UIMenuItem(GetLabelText(Funzioni.GetWeaponLabel((uint)GetHashKey(armi2[i].name))));
					if ((Eventi.Player.Money >= armi2[i].price) || (Eventi.Player.Bank >= armi2[i].price))
					{
						arma.SetRightLabel("~g~" + armi2[i].price + "$");
					}
					else
					{
						arma.SetRightLabel("~r~" + armi2[i].price + "$");
					}

					if (Game.PlayerPed.Weapons.HasWeapon((WeaponHash)(uint)GetHashKey(armi2[i].name)))
					{
						arma.SetRightBadge(UIMenuItem.BadgeStyle.Gun);
					}

					ArmiLic2.AddItem(arma);
					ArmiLic2.OnItemSelect += (_menu, _item, _index) =>
					{
						if (Game.PlayerPed.Weapons.HasWeapon((WeaponHash)(uint)GetHashKey(armi2[_index].name)))
						{
							if (Eventi.Player.Money >= 150)
							{
								AddAmmoToPed(Game.PlayerPed.Handle, (uint)GetHashKey(armi2[_index].name), 250);
								HUD.ShowNotification("Poichè già possiedi quest'arma, ti sono state ricaricate le munizioni al prezzo di 150$");
								BaseScript.TriggerServerEvent("lprp:removemoney", 150);
							}
							else
							{
								if (Eventi.Player.Bank >= 150)
								{
									AddAmmoToPed(Game.PlayerPed.Handle, (uint)GetHashKey(armi2[_index].name), 250);
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
							if (Eventi.Player.Money >= armi2[_index].price)
							{
								BaseScript.TriggerServerEvent("lprp:addWeapon", armi2[_index].name, 250);
								BaseScript.TriggerServerEvent("lprp:removemoney", armi2[_index].price);
								HUD.ShowNotification("Hai acquistato un/a ~y~" + GetLabelText(Funzioni.GetWeaponLabel((uint)GetHashKey(armi2[i].name))));
								_item.SetRightBadge(UIMenuItem.BadgeStyle.Gun);
							}
							else
							{
								if (Eventi.Player.Money >= armi2[_index].price)
								{
									BaseScript.TriggerServerEvent("lprp:addWeapon", armi2[_index].name, 250);
									BaseScript.TriggerServerEvent("lprp:removebank", armi2[_index].price);
									HUD.ShowNotification("Hai acquistato un/a ~y~" + GetLabelText(Funzioni.GetWeaponLabel((uint)GetHashKey(armi2[i].name))));
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
					UIMenuItem arma = new UIMenuItem(GetLabelText(Funzioni.GetWeaponLabel((uint)GetHashKey(armi3[i].name))));
					if ((Eventi.Player.Money >= armi3[i].price) || (Eventi.Player.Bank >= armi3[i].price))
					{
						arma.SetRightLabel("~g~" + armi3[i].price + "$");
					}
					else
					{
						arma.SetRightLabel("~r~" + armi3[i].price + "$");
					}

					if (Game.PlayerPed.Weapons.HasWeapon((WeaponHash)(uint)GetHashKey(armi3[i].name)))
					{
						arma.SetRightBadge(UIMenuItem.BadgeStyle.Gun);
					}

					ArmiLic3.AddItem(arma);
					ArmiLic3.OnItemSelect += async (_menu, _item, _index) =>
					{
						if (Game.PlayerPed.Weapons.HasWeapon((WeaponHash)(uint)GetHashKey(armi3[_index].name)))
						{
							if (Eventi.Player.Money >= 150)
							{
								AddAmmoToPed(Game.PlayerPed.Handle, (uint)GetHashKey(armi3[_index].name), 250);
								HUD.ShowNotification("Poichè già possiedi quest'arma, ti sono state ricaricate le munizioni al prezzo di 150$");
								BaseScript.TriggerServerEvent("lprp:removemoney", 150);
							}
							else
							{
								if (Eventi.Player.Bank >= 150)
								{
									AddAmmoToPed(Game.PlayerPed.Handle, (uint)GetHashKey(armi3[_index].name), 250);
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
							if (Eventi.Player.Money >= armi3[_index].price)
							{
								BaseScript.TriggerServerEvent("lprp:addWeapon", armi3[_index].name, 250);
								BaseScript.TriggerServerEvent("lprp:removemoney", armi3[_index].price);
								HUD.ShowNotification("Hai acquistato un/a ~y~" + GetLabelText(Funzioni.GetWeaponLabel((uint)GetHashKey(armi3[_index].name))));
								_item.SetRightBadge(UIMenuItem.BadgeStyle.Gun);
							}
							else
							{
								if (Eventi.Player.Money >= armi3[_index].price)
								{
									BaseScript.TriggerServerEvent("lprp:addWeapon", armi3[_index].name, 250);
									BaseScript.TriggerServerEvent("lprp:removebank", armi3[_index].price);
									HUD.ShowNotification("Hai acquistato un/a ~y~" + GetLabelText(Funzioni.GetWeaponLabel((uint)GetHashKey(armi3[_index].name))));
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
				if (Eventi.Player.CurrentChar.weapons.Count > 0)
				{
					foreach (Weapons armi in Eventi.Player.CurrentChar.weapons)
					{
						for (int j = 0; j < SharedScript.Armi.Count; j++)
						{
							if ((SharedScript.Armi[j].name == armi.name) && (SharedScript.Armi[j].components.Count > 0))
							{
								Arma = pool.AddSubMenu(component, GetLabelText(Funzioni.GetWeaponLabel((uint)GetHashKey(armi.name))), "Vedi qui i componenti acquistabili per la tua arma");
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
											UIMenuItem compon = new UIMenuItem(GetLabelText(Funzioni.GetWeaponLabel((uint)GetHashKey(v.name))));
											Arma.AddItem(compon);
											if ((Eventi.Player.Money >= co.price) || (Eventi.Player.Bank >= co.price))
											{
												compon.SetRightLabel("~g~" + co.price + "$");
											}
											else
											{
												compon.SetRightLabel("~r~" + co.price + "$");
											}

											for (int k = 0; k < armi.components.Count; k++)
											{
												if (armi.components[k].name == v.name)
												{
													compon.SetRightBadge(UIMenuItem.BadgeStyle.Ammo);
												}
											}
											Arma.OnItemSelect += async (_menu, _item, _index) =>
											{
												for (int l = 0; l < armi.components.Count; l++)
												{
													if (armi.components[l].name == co.name)
													{
														HUD.ShowNotification("Hai già acquistato questo componente!!", true);
													}
													else
													{
														if (Eventi.Player.Money >= co.price)
														{
															BaseScript.TriggerServerEvent("lprp:addWeaponComponent", armi.name, co.name);
															BaseScript.TriggerServerEvent("lprp:removemoney", co.price);
															HUD.ShowNotification("Hai acquistato un/a ~y~" + GetLabelText(Funzioni.GetWeaponLabel((uint)GetHashKey(co.name))));
															_item.SetRightBadge(UIMenuItem.BadgeStyle.Ammo);
														}
														else
														{
															if (Eventi.Player.Bank >= co.price)
															{
																BaseScript.TriggerServerEvent("lprp:addWeaponComponent", armi.name, co.name);
																BaseScript.TriggerServerEvent("lprp:removebank", co.price);
																HUD.ShowNotification("Hai acquistato un/a ~y~" + GetLabelText(Funzioni.GetWeaponLabel((uint)GetHashKey(co.name))));
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
				if (Eventi.Player.CurrentChar.weapons.Count > 0)
				{
					foreach (Weapons armi in Eventi.Player.CurrentChar.weapons)
					{
						for (int j = 0; j < SharedScript.Armi.Count; j++)
						{
							if (SharedScript.Armi[j].name == armi.name && GetWeaponDamageType((uint)GetHashKey(armi.name)) == 3)
							{
								Tnt = pool.AddSubMenu(Tinte, GetLabelText(Funzioni.GetWeaponLabel((uint)GetHashKey(armi.name))), "Vedi qui i colori acquistabili per la tua arma");
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
											UIMenuItem tintina = new UIMenuItem(GetLabelText(Funzioni.GetWeaponLabel((uint)GetHashKey(tin.name))));
											Tnt.AddItem(tintina);
											if ((Eventi.Player.Money >= tin.price) || (Eventi.Player.Bank >= tin.price))
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
									if (Eventi.Player.Money >= tinte[_index].price)
									{
										BaseScript.TriggerServerEvent("lprp:removemoney", tinte[_index].price);
										BaseScript.TriggerServerEvent("lprp:addWeaponTint", armi.name, _index);
										HUD.ShowNotification("Hai acquistato un/a ~y~" + GetLabelText(Funzioni.GetWeaponLabel((uint)GetHashKey(tinte[_index].name))));
										attTi.SetRightBadge(UIMenuItem.BadgeStyle.None);
										attTi = _item;
										attTi.SetRightBadge(UIMenuItem.BadgeStyle.Ammo);
										armi.tint = _index;
									}
									else
									{
										if (Eventi.Player.Bank >= tinte[_index].price)
										{
											BaseScript.TriggerServerEvent("lprp:removebank", tinte[_index].price);
											BaseScript.TriggerServerEvent("lprp:addWeaponTint", armi.name, _index);
											HUD.ShowNotification("Hai acquistato un/a ~y~" + GetLabelText(Funzioni.GetWeaponLabel((uint)GetHashKey(tinte[_index].name))));
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
								Client.Printa(LogType.Error, "Da segnalare allo scripter!!\nMessaggio NegoziClient.cs:607 = " + e.Message);
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
