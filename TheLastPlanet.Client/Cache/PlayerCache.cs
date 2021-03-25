using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using Logger;
using TheLastPlanet.Client.Core.PlayerChar;
using TheLastPlanet.Shared;

namespace TheLastPlanet.Client.SessionCache
{
    public class PlayerCache
    {
        public Player Player { get; private set; }
        public Ped Ped { get; private set; }
        public User User { get; private set; }
        public Position Posizione => User.Posizione;
        public bool Ready => Player != null && Ped != null && User != null;

        public PlayerCache()
        {
            Player = Game.Player;
            Ped = new Ped(PlayerPedId());
        }

		public void SetCharacter(User data) => User = data;

		public void AddPlayer(string data)
        {
            try
            {
                User.char_data = data;
            }
            catch (Exception e)
            {
                Log.Printa(LogType.Error, e.ToString());
            }
        }

		public void UpdatePedId() => Ped = new Ped(PlayerPedId());

		public void UpdatePlayerId() => Player = Game.Player;


	}
}