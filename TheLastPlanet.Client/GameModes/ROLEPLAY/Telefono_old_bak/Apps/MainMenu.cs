using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheLastPlanet.Client.Telefono.Models;

namespace TheLastPlanet.Client.Telefono.Apps
{
    public class MainMenu : App
    {
        public int SelectedItem { get; set; }
        public List<App> AllApps { get; set; }

        public MainMenu(Phone phone, List<App> allApps) : base("Main", 0, phone, false)
        {
            AllApps = allApps;

            Client.Logger.Debug($"Apps totali {AllApps.Count}");
        }

        public override async Task Tick()
        {
            try
            {
                if (Phone.Scaleform == null || AllApps == null) return;

                for (int i = 0; i < 9; i++)
                {
                    int thirdParam = 3;
                    if (i < AllApps.Count)
                        if (AllApps[i].Icon != 0)
                            thirdParam = AllApps[i].Icon;
                    Phone.Scaleform.CallFunction("SET_DATA_SLOT", 1, i, thirdParam);
                }

                Phone.Scaleform.CallFunction("DISPLAY_VIEW", 1, SelectedItem);
                Phone.Scaleform.CallFunction("CELLPHONE_APP", SelectedItem, "Hi", true);

                string appName = "";
                if (SelectedItem < AllApps.Count)
                    if (!String.IsNullOrEmpty(AllApps[SelectedItem].Name))
                        appName = AllApps[SelectedItem].Name;

                Phone.Scaleform.CallFunction("SET_HEADER", appName);

                bool navigated = true;

                if (Input.IsControlJustPressed(Control.PhoneUp))
                {
                    SelectedItem -= 3;
                    CellCamMoveFinger(1);
                    if (SelectedItem < 0)
                        SelectedItem = 8 + SelectedItem;
                }
                else if (Input.IsControlJustPressed(Control.PhoneDown))
                {
                    SelectedItem += 3;
                    CellCamMoveFinger(2);
                    if (SelectedItem > 8)
                        SelectedItem = SelectedItem - 8;
                }
                else if (Input.IsControlJustPressed(Control.PhoneRight))
                {
                    SelectedItem += 1;
                    CellCamMoveFinger(3);
                    if (SelectedItem > 8)
                        SelectedItem = 0;
                }
                else if (Input.IsControlJustPressed(Control.PhoneLeft))
                {
                    SelectedItem -= 1;
                    CellCamMoveFinger(3);
                    if (SelectedItem < 0)
                        SelectedItem = 8;
                }
                else if (Input.IsControlJustPressed(Control.PhoneSelect))
                {
                    CellCamMoveFinger(5);
                    BaseScript.TriggerEvent("lprp:phone_start", AllApps[SelectedItem].Name);
                }
                else
                    navigated = false;

                if (navigated)
                    Game.PlaySound("Menu_Navigate", "Phone_SoundSet_Default");
            }
            catch (Exception e)
            {
                Client.Logger.Error($"{e.Message} : Exception thrown on Apps.Main.Tick()");
            }
        }

        public override void Initialize(Phone phone)
        {
            Phone = phone;
            SelectedItem = 0;
            SetMobilePhoneRotation(-90.0f, 0.0f, 0.0f, 0);
            SetPhoneLean(false);
        }

        public override void Kill()
        {
        }
    }
}
