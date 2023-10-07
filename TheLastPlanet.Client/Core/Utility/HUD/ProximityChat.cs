using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.Core.Utility.HUD
{
    internal static class ProximityChat
    {
        public static void Init()
        {
            Client.Instance.AddEventHandler("lprp:triggerProximityDisplay", new Action<int, string, string>(TriggerProximtyDisplay));
            EventDispatcher.Mount("lprp:triggerProximityDisplay", new Action<int, string, string>(TriggerProximtyDisplay));
        }

        private static Dictionary<int, List<ProxMess>> Messaggi = new Dictionary<int, List<ProxMess>>();

        public static void TriggerProximtyDisplay(int player, string title, string text)
        {
            Player target = new Player(API.GetPlayerFromServerId(player));
            if (Messaggi.ContainsKey(player))
                Messaggi[player].Add(new ProxMess(title + " " + text, Colors.WhiteSmoke, target.Character.Bones[Bone.SKEL_Head].Position));
            else
                Messaggi.Add(player, new List<ProxMess>() { new ProxMess(title + " " + text, Colors.WhiteSmoke, target.Character.Bones[Bone.SKEL_Head].Position) });
        }

        public static async Task Prossimità()
        {
            bool canDraw;
            Ped myPed = new Ped(API.PlayerPedId());

            if (Messaggi.Count > 0)
                foreach (KeyValuePair<int, List<ProxMess>> p in Messaggi)
                {
                    Player player = new Player(API.GetPlayerFromServerId(p.Key));
                    Ped ped = player.Character;

                    if (!myPed.IsInRangeOf(ped.Position, 19f)) continue;
                    if (p.Value.Count < 1) continue;
                    canDraw = ProximityVehCheck(myPed, ped);

                    foreach (ProxMess m in p.Value.ToList())
                    {
                        if (canDraw) m.Draw(p.Value.Count - p.Value.IndexOf(m), ped);

                        if (Game.GameTime - m.Timer < 1000) continue;
                        m.Tempo = m.Tempo.Subtract(TimeSpan.FromSeconds(1));
                        m.Timer = Game.GameTime;

                        if (m.Tempo != TimeSpan.Zero) continue;
                        p.Value.Remove(m);
                    }

                    //else Messaggi.Remove(p.Key);
                }

            await Task.FromResult(0);
        }

        private static bool ProximityVehCheck(Ped io, Ped lui)
        {
            bool ioInVeh = Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle;
            bool luiInVeh = lui.IsInVehicle();

            switch (ioInVeh)
            {
                case false when !luiInVeh:
                    return true;
                case true when !luiInVeh:
                    return !io.CurrentVehicle.Windows.AreAllWindowsIntact || io.CurrentVehicle.Doors.GetAll().Any(x => x.IsOpen);
                case false when luiInVeh:
                    return !lui.CurrentVehicle.Windows.AreAllWindowsIntact || lui.CurrentVehicle.Doors.GetAll().Any(x => x.IsOpen);
            }

            if (io.CurrentVehicle == lui.CurrentVehicle) return true;

            return !lui.CurrentVehicle.Windows.AreAllWindowsIntact && !io.CurrentVehicle.Windows.AreAllWindowsIntact || io.CurrentVehicle.Doors.GetAll().Any(x => x.IsOpen) || lui.CurrentVehicle.Doors.GetAll().Any(x => x.IsOpen);
        }
    }

    public class ProxMess
    {
        public string Message;
        public Color Color;
        public Vector3 Position;
        public TimeSpan Tempo = new TimeSpan(0, 0, 5); // cambiare con 5 secondi
        public int Timer = 0;

        public ProxMess(string mess, Color color, Vector3 pos)
        {
            Message = mess;
            Color = color;
            Position = pos;
        }

        /// <summary>
        /// Chiamata ad ogni frame!
        /// </summary>
        public void Draw(int index, Ped p)
        {
            if (Timer == 0) Timer = Game.GameTime;
            Color textColor = Colors.WhiteSmoke;

            switch (index)
            {
                case 1:
                    textColor = Colors.Green;

                    break;
                case 2:
                    textColor = Colors.Cyan;

                    break;
                case 3:
                    textColor = Colors.PurpleLight;

                    break;
                case 4:
                    textColor = Colors.RedLight;

                    break;
                case 5:
                    textColor = Colors.Orange;

                    break;
                case 6:
                    textColor = Colors.Yellow;

                    break;
            }

            Color = textColor;
            Position = p.Bones[Bone.SKEL_Head].Position + new Vector3(0, 0, 0.4f + index * 0.24f);
            HUD.DrawText3D(Position.ToPosition(), Color, Message, CitizenFX.Core.UI.Font.ChaletLondon);
        }
    }
}