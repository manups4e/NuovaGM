using CitizenFX.Core.Native;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;

namespace TheLastPlanet.Client
{
    internal static class ClasseDiTest
    {
        public static void Init()
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
                //TestMenu();
                AttivaMenu();
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

        private static void AttivaMenu()
        {
            var txd = API.CreateRuntimeTxd("test");
            var _paneldui = API.CreateDui("https://i.imgur.com/mH0Y65C.gif", 288, 160);
            API.CreateRuntimeTextureFromDuiHandle(txd, "panelbackground", API.GetDuiHandle(_paneldui));
            JobSelectionData data = new();
            data.SetTitle("TEST");
            data.SetVotes(0, 3, "TEST");
            data.Cards = new List<JobSelectionCard>();
            for (int i = 0; i < 6; i++)
            {
                var card = new JobSelectionCard("Test", "test", "test", "panelbackground", 12, 15, JobSelectionCardIcon.CAPTURE_THE_FLAG, HudColor.HUD_COLOUR_FREEMODE, 2, new List<JobSelectionCardDetail>()
                {
                     new JobSelectionCardDetail(JobSelectionCardDetailType.WITH_ICON, "Test Left", "Test Right"),
                     new JobSelectionCardDetail(JobSelectionCardDetailType.WITH_ICON, "Test Left", "Test Right"),
                     new JobSelectionCardDetail(JobSelectionCardDetailType.WITH_ICON, "Test Left", "Test Right")
                });

                data.AddCard(card);
            }
            data.Buttons = new List<JobSelectionButton>()
            {
                new JobSelectionButton("Test1", null),
                new JobSelectionButton("Test2", null),
                new JobSelectionButton("Test3", null),
            };
            ScaleformUI.ScaleformUI.JobMissionSelection.JobData = data;
            ScaleformUI.ScaleformUI.JobMissionSelection.Enabled = true;
        }
    }
}