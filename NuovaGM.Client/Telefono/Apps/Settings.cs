using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using NuovaGM.Client.Telefono;
using NuovaGM.Client.Telefono.Models;
using NuovaGM.Shared;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Client.gmPrincipale.Utility;

namespace NuovaGM.Client.Telefono.Apps
{
	public class Settings : App
    {
        private int SelectedItem { get; set; } = 0;

		private SettingsSubMenu CurrentSubMenu = null;

		private static bool FirstTick = true;
		private static string numero;


		private static List<SettingsSubMenuItem> Themes = new List<SettingsSubMenuItem>
        {
            new SettingsSubMenuItem("~l~" + GetLabelText("CELL_820"), 1, 23),
            new SettingsSubMenuItem("~l~" + GetLabelText("CELL_821"), 2, 23),
            new SettingsSubMenuItem("~l~" + GetLabelText("CELL_822"), 3, 23),
            new SettingsSubMenuItem("~l~" + GetLabelText("CELL_823"), 4, 23),
            new SettingsSubMenuItem("~l~" + GetLabelText("CELL_824"), 5, 23),
            new SettingsSubMenuItem("~l~" + GetLabelText("CELL_825"), 6, 23),
            new SettingsSubMenuItem("~l~" + GetLabelText("CELL_826"), 7, 23),
        };

        private static List<SettingsSubMenuItem> Wallpapers = new List<SettingsSubMenuItem>
        {
            new SettingsSubMenuItem("~l~" + GetLabelText("CELL_844"), 4, 23),
            new SettingsSubMenuItem("~l~" + GetLabelText("CELL_845"), 5, 23),
            new SettingsSubMenuItem("~l~" + GetLabelText("CELL_846"), 6, 23),
            new SettingsSubMenuItem("~l~" + GetLabelText("CELL_847"), 7, 23),
            new SettingsSubMenuItem("~l~" + GetLabelText("CELL_848"), 8, 23),
            new SettingsSubMenuItem("~l~" + GetLabelText("CELL_849"), 9, 23),
            new SettingsSubMenuItem("~l~" + GetLabelText("CELL_850"), 10, 23),
            new SettingsSubMenuItem("~l~" + GetLabelText("CELL_851"), 11, 23),
            new SettingsSubMenuItem("~l~" + GetLabelText("CELL_852"), 12, 23),
            new SettingsSubMenuItem("~l~" + GetLabelText("CELL_853"), 13, 23),
            new SettingsSubMenuItem("~l~" + GetLabelText("CELL_854"), 14, 23),
            new SettingsSubMenuItem("~l~" + GetLabelText("CELL_855"), 15, 23),
            new SettingsSubMenuItem("~l~" + GetLabelText("CELL_856"), 16, 23),
            new SettingsSubMenuItem("~l~" + GetLabelText("CELL_857"), 17, 23)
        };

		private static List<SettingsSubMenuItem> Standby = new List<SettingsSubMenuItem>
		{
			new SettingsSubMenuItem("~l~" + GetLabelText("CELL_800"), 1, 25),
			new SettingsSubMenuItem("~l~" + GetLabelText("CELL_801"), 2, 26)
		};

		private static List<SettingsSubMenuItem> Suoneria = new List<SettingsSubMenuItem>
		{
			new SettingsSubMenuItem("~l~" + GetLabelText("CELL_810"), 1, 18),
			new SettingsSubMenuItem("~l~" + GetLabelText("CELL_811"), 2, 18),
			new SettingsSubMenuItem("~l~" + GetLabelText("CELL_812"), 3, 18),
			new SettingsSubMenuItem("~l~" + GetLabelText("CELL_813"), 4, 22),
		};

		private static List<SettingsSubMenuItem> Vibrate = new List<SettingsSubMenuItem>
		{
			new SettingsSubMenuItem("~l~" + GetLabelText("CELL_831"), 1, 21),
			new SettingsSubMenuItem("~l~" + GetLabelText("CELL_830"), 2, 20)
		};

		private static List<SettingsSubMenuItem> Personale = new List<SettingsSubMenuItem>
		{
			new SettingsSubMenuItem("~l~ Numero di Telefono", 0, 21),
			new SettingsSubMenuItem("~l~" + numero, 1, 21),


		};

		private List<SettingsSubMenu> SubMenus = new List<SettingsSubMenu>
        {
			new SettingsSubMenu("Dati Personali", 25, Personale),
			new SettingsSubMenu(GetLabelText("CELL_740"), 23, Wallpapers),
			new SettingsSubMenu(GetLabelText("CELL_700"), 25, Standby),
			new SettingsSubMenu(GetLabelText("CELL_710"), 18, Suoneria),
			new SettingsSubMenu(GetLabelText("CELL_720"), 23, Themes),
			new SettingsSubMenu(GetLabelText("CELL_730"), 20, Vibrate),
		};

		public Settings(Phone phone) : base(GetLabelText("CELL_16"), 24, phone)
        {
			numero = phone.getCurrentCharPhone().Numero;
        }

        public override async Task Tick()
        {
            if (FirstTick)
            {
                FirstTick = false;
                await BaseScript.Delay(100);
                return;
            }
            Phone.Scaleform.CallFunction("SET_DATA_SLOT_EMPTY", 13);

            var appName = GetLabelText("CELL_16");
            if (CurrentSubMenu != null)
            {
                foreach (var item in CurrentSubMenu.Items)
                {
                    Phone.Scaleform.CallFunction("SET_DATA_SLOT", 13, CurrentSubMenu.Items.IndexOf(item), item.Icon, item.Name);
                }
                if (SelectedItem < CurrentSubMenu.Items.Count)
                {
                    if (!String.IsNullOrEmpty(CurrentSubMenu.Name))
                    {
                        appName = CurrentSubMenu.Name;
					}
				}
            }
            else
            {
                foreach (var subMenu in SubMenus)
                {
                    Phone.Scaleform.CallFunction("SET_DATA_SLOT", 13, SubMenus.IndexOf(subMenu), subMenu.Icon, "~l~" + subMenu.Name);
                }                
            }

            Phone.Scaleform.CallFunction("SET_HEADER", "~w~" + appName);

            Phone.Scaleform.CallFunction("DISPLAY_VIEW", 13, SelectedItem);

            var navigated = true;
			if (Game.IsControlJustPressed(0, Control.PhoneUp))
			{
				MoveFinger(1);
				if (SelectedItem > 0)
				{
					SelectedItem -= 1;
				}
				else
				{
					if (CurrentSubMenu != null)
					{
						SelectedItem = CurrentSubMenu.Items.Count - 1;
					}
					else
					{
						SelectedItem = SubMenus.Count - 1;
					}
				}
			}
			else if (Game.IsControlJustPressed(0, Control.PhoneDown))
			{
				MoveFinger(2);
				if (CurrentSubMenu == null)
				{
					if (SelectedItem < SubMenus.Count - 1)
					{
						SelectedItem += 1;
					}
					else
					{
						SelectedItem = 0;
					}
				}
				else
				{
					if (SelectedItem < CurrentSubMenu.Items.Count - 1)
					{
						SelectedItem += 1;
					}
					else
					{
						SelectedItem = 0;
					}
				}
			}
			else if (Game.IsControlJustPressed(0, Control.FrontendAccept))
			{
				MoveFinger(5);
				if (CurrentSubMenu == null)
				{
					CurrentSubMenu = SubMenus[SelectedItem];
					SelectedItem = 0;
				}
				else
				{
					if (CurrentSubMenu.Name == GetLabelText("CELL_720"))
					{
						SetTheme(CurrentSubMenu.Items[SelectedItem].Id);
					}
					else if (CurrentSubMenu.Name == GetLabelText("CELL_740"))
					{
						SetWallpaper(CurrentSubMenu.Items[SelectedItem].Id);
					}
					else if (CurrentSubMenu.Name == GetLabelText("CELL_700"))
					{
						SetSleep(CurrentSubMenu.Items[SelectedItem].Id);
					}
					else if (CurrentSubMenu.Name == GetLabelText("CELL_710"))
					{
						SetRingtone(CurrentSubMenu.Items[SelectedItem].Id);
					}
					else if (CurrentSubMenu.Name == GetLabelText("CELL_730"))
					{
						SetVibration(CurrentSubMenu.Items[SelectedItem].Id);
					}
				}
			}
			else if (Game.IsControlJustPressed(0, Control.FrontendCancel))
			{
				MoveFinger(5);
				if (CurrentSubMenu != null)
				{
					CurrentSubMenu = null;
					SelectedItem = 0;
				}
				else
				{
					navigated = false;
					BaseScript.TriggerEvent("lprp:phone_start", "Main");
				}
			}
			else
			{
				navigated = false;
			}

            if (navigated)
            {
                Game.PlaySound("Menu_Navigate", "Phone_SoundSet_Default");
            }
			await Task.FromResult(0);
        }

        public override void Initialize(Phone phone)
        {
            Phone = phone;
            CurrentSubMenu = null;
            FirstTick = true;
            SelectedItem = 0;
        }

        public override void Kill()
        {

        }

        public void SetTheme(int themeId)
        {
            Phone.getCurrentCharPhone().Theme = themeId;
			BaseScript.TriggerServerEvent("lprp:phone:updatePhones", "Theme", themeId);
		}

		public void SetWallpaper(int wallpaperId)
        {
            Phone.getCurrentCharPhone().Wallpaper = wallpaperId;
			BaseScript.TriggerServerEvent("lprp:phone:updatePhones", "wall", wallpaperId);
        }

		public void SetRingtone(int ringtoneId)
		{
			Phone.getCurrentCharPhone().Ringtone = ringtoneId;
			BaseScript.TriggerServerEvent("lprp:phone:updatePhones", "ring", ringtoneId);
		}
		public void SetVibration(int Vibe)
		{
			if (Vibe == 2)
				SetPadShake(0, 300, 200);
			Phone.getCurrentCharPhone().Vibration = Vibe;
			BaseScript.TriggerServerEvent("lprp:phone:updatePhones", "vibe", Vibe);
		}

		public void SetSleep(int toggle)
		{
			if (toggle == 2)
				HUD.ShowHelp("Attenzione! Se attivi la modalità StandBy non riceverai chiamate da nessuno!~n~Potrai ricevere di nuovo chiamate quando riattiverai la modalità normale");
			Phone.getCurrentCharPhone().SleepMode = toggle;
			BaseScript.TriggerServerEvent("lprp:phone:updatePhones", "sleep", toggle);
		}

	}

	public class SettingsSubMenu
    {
        public string Name { get; private set; }
        public int Icon { get; private set; }
        public List<SettingsSubMenuItem> Items { get; set; }

        public SettingsSubMenu(string label, int icon, List<SettingsSubMenuItem> items = null)
        {
            Name = label;
            Icon = icon;
            Items = items;
        }
    }

    public class SettingsSubMenuItem
    {
        public string Name { get; set; }
        public int Id { get; set; }
		public int Icon { get; set; }

		public SettingsSubMenuItem(string name, int id, int icon)
        {
            Name = name;
            Id = id;
			Icon = icon;
        }
    }
}
