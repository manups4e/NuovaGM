using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Shared.TypeExtensions;

namespace TheLastPlanet.Client
{
    internal static class ClasseDiTest
    {
        static ClientList list = new ClientList();
        public static async void Init()
        {
            Client.Instance.AddTick(TestTick);
        }

        static bool pp = false;
        static Marker dummyMarker = new(MarkerType.VerticalCylinder, WorldProbe.CrossairRaycastResult.HitPosition.ToPosition(), Colors.Blue);
        private static async Task TestTick()
        {
            //Client.Logger.Debug(IplManager.Global.ToJson());
            if (Input.IsControlJustPressed(Control.Detonate, PadCheck.Keyboard, ControlModifier.Shift) && !HUD.MenuPool.IsAnyMenuOpen)
            {
                //await PlayerCache.InitPlayer();

                //PlayerCache.MyPlayer.Ped.Weapons.Give(WeaponHash.Pistol, 100, true, true);
                //TestMenu();
                //AttivaMenu();
                /*
                list.RequestPlayerList();
                await list.WaitRequested();
                foreach (var pl in list) 
                {
                    Client.Logger.Debug($"giocatori: {pl.Player.Name}, {pl.Handle}");
                    Client.Logger.Debug(pl.ToJson());
                }
                */
            }
            if (Input.IsControlJustPressed(Control.Context, PadCheck.Keyboard, ControlModifier.Shift))
            {
                menu.MenuItems[0].Enabled = !menu.MenuItems[0].Enabled;
            }
        }

        public static void Stop()
        {
            Client.Instance.RemoveTick(TestTick);
        }

        static UIMenu menu = new("Menu Test", "Test");

        private static void TestMenu()
        {
            var menu = new UIMenu("🐌", "test", new PointF(50, 50), "thelastgalaxy", "bannerbackground", false, true);
            UIMenuItem item = new("😐", "~BLIP_INFO_ICON~ Potrai in ogni mento riaprire questo menu di pausa premendo i tasti ~INPUT_SPRINT~ + ~INPUT_DROP_WEAPON~ oppure con il comando /help.");
            menu.AddItem(item);
            HUD.MenuPool.Add(menu);
            menu.Visible = true;
        }
    }
}