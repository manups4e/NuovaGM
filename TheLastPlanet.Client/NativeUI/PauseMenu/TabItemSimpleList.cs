using CitizenFX.Core.UI;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Font = CitizenFX.Core.UI.Font;

namespace TheLastPlanet.Client.NativeUI.PauseMenu
{
    public class TabItemSimpleList : TabItem
    {
        public TabItemSimpleList(string title, Dictionary<string, string> dict) : base(title)
        {
            Dictionary = dict;
            DrawBg = false;
        }

        public Dictionary<string, string> Dictionary { get; set; }

        public override void Draw()
        {
            base.Draw();

            int alpha = (Focused || !CanBeFocused) ? 180 : 60;
            int blackAlpha = (Focused || !CanBeFocused) ? 200 : 90;
            int fullAlpha = (Focused || !CanBeFocused) ? 255 : 150;

            int rectSize = (int)(BottomRight.X - TopLeft.X);

            for (int i = 0; i < Dictionary.Count; i++)
            {
                new UIResRectangle(new PointF(TopLeft.X, TopLeft.Y + (40 * i)),
                    new SizeF(rectSize, 40), i % 2 == 0 ? Color.FromArgb(alpha, 0, 0, 0) : Color.FromArgb(blackAlpha, 0, 0, 0)).Draw();

                KeyValuePair<string, string> item = Dictionary.ElementAt(i);

                new UIResText(item.Key, new PointF(TopLeft.X + 6, TopLeft.Y + 5 + (40 * i)), 0.35f, Color.FromArgb(fullAlpha, Colors.White)).Draw();
                new UIResText(item.Value, new PointF(BottomRight.X - 6, TopLeft.Y + 5 + (40 * i)), 0.35f, Color.FromArgb(fullAlpha, Colors.White), Font.ChaletLondon, Alignment.Right).Draw();
            }
        }
    }
}