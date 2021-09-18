using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheLastPlanet.Client.Cache;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Shared;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Interactions
{
    static class Pioggia
    {
        private static Random rand = new();
        private static int gameTimer;
        private static int rainDropCount;
        private static List<int> _rainDropTimers = new();
        private static List<int> _rainDropTexture = new();
        private static List<int> _rainDropPosX = new();
        private static List<int> _rainDropPosY = new();
        private static List<int> _rainDropScale = new();

        public static Random rnd = new();
        public static int[] index = new int[6];
        public static Vector3 playerPos;
        public static float playerSpeed;
        public static List<Raindrop> raindrops = new();

        public static void Init()
        {
            API.RequestStreamedTextureDict("pioggia", true);
            Client.Instance.AddTick(tickHandler);
        }

        private static async Task tickHandler()
        {
            playerSpeed = (float)(!PlayerCache.MyPlayer.Ped.IsInVehicle() ? Math.Sqrt(Math.Pow(PlayerCache.MyPlayer.Ped.Velocity.X, 2.0) + Math.Pow(PlayerCache.MyPlayer.Ped.Velocity.Y, 2.0) + Math.Pow(PlayerCache.MyPlayer.Ped.Velocity.Z, 2.0)) : PlayerCache.MyPlayer.Ped.CurrentVehicle.Speed);
            playerPos = PlayerCache.MyPlayer.Ped.Position;
            MoveRainDrops();
            RaycastResult raycast = World.Raycast(GameplayCamera.Position, new Vector3(0, 0, 1), 1000f, IntersectOptions.Everything);
            if (API.GetRainLevel() > 0.1f && API.GetInteriorFromGameplayCam() == 0 && !raycast.DitHit)
            {
                var rand = rnd.Next(1001);
                if (rand > 500f - (API.GetRainLevel() * 100 + playerSpeed) + 1)
                    raindrops.Add(new Raindrop());

                if (PlayerCache.MyPlayer.Ped.IsInVehicle())
                {
                    if (playerSpeed >= 10)
                        raindrops.Add(new Raindrop());
                    if (playerSpeed >= 15)
                        raindrops.Add(new Raindrop());
                    if (playerSpeed >= 20)
                        raindrops.Add(new Raindrop());
                    if (playerSpeed >= 25)
                        raindrops.Add(new Raindrop());
                    if (playerSpeed >= 30)
                        raindrops.Add(new Raindrop());
                    if (playerSpeed >= 35)
                        raindrops.Add(new Raindrop());
                    if (playerSpeed >= 40)
                        raindrops.Add(new Raindrop());
                    if (playerSpeed >= 45)
                        raindrops.Add(new Raindrop());
                    if (playerSpeed >= 50)
                        raindrops.Add(new Raindrop());
                }

            }
            if (raindrops.Count <= 0)
                return;
            DrawRaindrops();
            await Task.FromResult(0);
        }

        public static void MoveRainDrops()
        {
            List<Raindrop> rList = new();
            foreach (Raindrop raindrop in raindrops)
            {
                raindrop.opacity -= 1 + (int)playerSpeed / 15;
                if (raindrop.opacity <= 0 || raindrop.size < 0.0 || (raindrop.y > Screen.Resolution.Height || raindrop.y < 0) || (raindrop.x > Screen.Resolution.Width || raindrop.x < 0))
                    rList.Add(raindrop);
                if (raindrop.isFalling)
                {

                    raindrop.y += raindrop.size / 30.0f + rnd.NextFloat(5);
                    if (PlayerCache.MyPlayer.Ped.IsOnFoot)
                    {
                        if (raindrop.x < Screen.Resolution.Width / 2)
                            raindrop.x -= raindrop.size * playerSpeed / (raindrop.size / playerSpeed) / 10f;
                        else
                            raindrop.x += raindrop.size * playerSpeed / (raindrop.size / playerSpeed) / 10f;
                        if (raindrop.y < Screen.Resolution.Height / 2)
                            raindrop.y -= raindrop.size * playerSpeed / (raindrop.size / playerSpeed) / 10f;
                        else
                            raindrop.y += raindrop.size * playerSpeed / (raindrop.size / playerSpeed) / 10f;
                    }
                    if (PlayerCache.MyPlayer.Ped.IsInVehicle())
                    {
                        if (raindrop.x < Screen.Resolution.Width / 2)
                            raindrop.x -= raindrop.size * playerSpeed / (raindrop.size / playerSpeed) / 300f;
                        else
                            raindrop.x += raindrop.size * playerSpeed / (raindrop.size / playerSpeed) / 300f;
                        if (raindrop.y < Screen.Resolution.Height / 2)
                            raindrop.y -= raindrop.size * playerSpeed / (raindrop.size / playerSpeed) / 100f;
                        else
                            raindrop.y -= raindrop.size * playerSpeed / (raindrop.size / playerSpeed) / 100f;
                    }
                }
                else if (rnd.Next(1001) >= 1000 - playerSpeed)
                    raindrop.isFalling = true;

            }
            foreach (Raindrop r in rList)
                raindrops.Remove(r);
        }
        private static void DrawRaindrops()
        {
            if (API.GetFollowVehicleCamViewMode() == 4 || API.GetFollowPedCamViewMode() == 4) return;
            foreach (Raindrop raindrop in raindrops)
            {
                PointF point = new PointF(raindrop.x, raindrop.y);
                Color color = raindrop.opacity > 255 ? Color.FromArgb(255, 255, 255, 255) : Color.FromArgb(raindrop.opacity, 255, 255, 255);
                string str = "pioggia";
                int textureIndex = raindrop.textureNumber;
                SizeF size = new SizeF(raindrop.size, raindrop.size);
                NativeUI.Sprite.Draw(str, "raindrop_"+ textureIndex, point.X, point.Y, size.Width, size.Height, raindrop.heading, color);
            }
        }
    }

    public class Raindrop
    {
        public float x;
        public float y;
        public float size;
        public float heading;
        public int opacity;
        public int textureNumber;
        public int textureIndex;
        public bool isFalling;

        public Raindrop()
        {
            Random random = new(API.GetRandomIntInRange(100, 9999999));
            x = random.NextFloat(0f, Screen.Resolution.Width);
            y = random.NextFloat(0f, Screen.Resolution.Height);
            heading = random.NextFloat(360f);
            isFalling = false;
            opacity = 400 + random.Next(300);
            size = random.NextFloat(50, 80);
            int index = random.Next(0, 4);
//            int index = random.Next(1, 6);
            textureNumber = index;
            textureIndex = Pioggia.index[index];
            Pioggia.index[index] = Pioggia.index[index] >= 128 ? 0 : Pioggia.index[index] + 1;
        }
    }
}
