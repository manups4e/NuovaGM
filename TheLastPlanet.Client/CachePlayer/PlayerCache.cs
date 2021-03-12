using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using Logger;
using static CitizenFX.Core.Native.API;
using TheLastPlanet.Client.Core.PlayerChar;
using TheLastPlanet.Shared;

namespace TheLastPlanet.Client.Cache
{
    public class PlayerCache
    {
        public Player Player { get; private set; }
        public Ped Ped { get; private set; }
        public Character Character { get; private set; }

        public bool Ready => Player != null && Ped != null && Character != null;

        public PlayerCache()
        {
            Player = Game.Player;
            Ped = new Ped(PlayerPedId());
        }

        public void SetCharacter(Character data)
        {
            Character = data;
        }
        public void AddPlayer(string data)
        {
            try
            {
                Character.char_data = data;
            }
            catch (Exception e)
            {
                Log.Printa(LogType.Error, e.ToString());
            }
        }
        
        public void UpdatePedId()
        {
            Ped = new Ped(PlayerPedId());
        }
        
        public void UpdatePlayerId()
        {
            Player = Game.Player;
        }


    }
}