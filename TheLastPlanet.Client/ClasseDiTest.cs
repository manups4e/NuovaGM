using FxEvents.Shared.Snowflakes;
using System;
using System.Drawing;
using System.Threading.Tasks;
using MarkerEx = TheLastPlanet.Client.Core.Utility.HUD.MarkerEx;

namespace TheLastPlanet.Client
{
    internal static class ClasseDiTest
    {
        static ClientList list = new ClientList();
        public static async void Init()
        {
            Client.Instance.AddTick(TestTick);
            EventDispatcher.Send("testVector", new Tuple<Snowflake, Vector2>(Snowflake.Next(), new Vector2(2f, 4f)));
        }


        static bool pp = false;
        static MarkerEx dummyMarker = new(MarkerType.VerticalCylinder, WorldProbe.CrossairRaycastResult.HitPosition.ToPosition(), SColor.Blue);
        private static async Task TestTick()
        {
            //Client.Logger.Debug(IplManager.Global.ToJson());
            if (Input.IsControlJustPressed(Control.Detonate, PadCheck.Keyboard, ControlModifier.Shift) && !MenuHandler.IsAnyMenuOpen)
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
            UIMenu menu = new UIMenu("🐌", "test", new PointF(50, 50), "thelastgalaxy", "bannerbackground", false, true);
            UIMenuItem item = new("😐", "~BLIP_INFO_ICON~ Potrai in ogni mento riaprire questo menu di pausa premendo i tasti ~INPUT_SPRINT~ + ~INPUT_DROP_WEAPON~ oppure con il comando /help.");
            menu.AddItem(item);
            menu.Visible = true;
        }
    }
}