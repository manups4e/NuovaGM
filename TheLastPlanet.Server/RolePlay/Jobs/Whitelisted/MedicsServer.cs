using System;
using System.Collections.Generic;
using System.Linq;



namespace TheLastPlanet.Server.Jobs.Whitelisted
{
    static class MedicsServer
    {
        private static readonly List<int> Morti = new List<int>();
        public static void Init()
        {
            EventDispatcher.Mount("tlg:roleplay:onPlayerSpawn", new Action<PlayerClient>(Spawned));
            Server.Instance.AddEventHandler("lprp:onPlayerDeath", new Action<Player>(playerDead));
            Server.Instance.AddEventHandler("lprp:medici:rimuoviDaMorti", new Action<Player>(PlayerAlive));
        }

        private static void playerDead([FromSource] Player player)
        {
            if (!Morti.Contains(Convert.ToInt32(player.Handle)))
            {
                Morti.Add(Convert.ToInt32(player.Handle));
                BaseScript.TriggerClientEvent("lprp:medici:aggiungiPlayerAiMorti", Convert.ToInt32(player.Handle));
            }
        }

        private static void PlayerAlive([FromSource] Player player)
        {
            if (Morti.ToList().Contains(Convert.ToInt32(player.Handle)))
            {
                Morti.Remove(Convert.ToInt32(player.Handle));
                BaseScript.TriggerClientEvent("lprp:medici:rimuoviPlayerAiMorti", Convert.ToInt32(player.Handle));
            }
        }

        private static void Spawned([FromSource] PlayerClient client)
        {
            for (int i = 0; i < Morti.Count; i++)
                client.Player.TriggerEvent("lprp:medici:aggiungiPlayerAiMorti", Morti[i]);
        }
    }
}
