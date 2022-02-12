using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using TheLastPlanet.Shared.Internal.Events;


namespace TheLastPlanet.Server.Lavori.Whitelistati
{
    static class MediciServer
    {
        private static readonly List<int> Morti = new List<int>();
        public static void Init()
        {
            Server.Instance.Events.Mount("tlg:roleplay:onPlayerSpawn", new Action<ClientId>(Spawnato));
            Server.Instance.AddEventHandler("lprp:onPlayerDeath", new Action<Player>(PlayerMorto));
            Server.Instance.AddEventHandler("lprp:medici:rimuoviDaMorti", new Action<Player>(PlayerVivo));
        }

        private static void PlayerMorto([FromSource] Player player)
        {
            if (!Morti.Contains(Convert.ToInt32(player.Handle)))
            {
                Morti.Add(Convert.ToInt32(player.Handle));
                BaseScript.TriggerClientEvent("lprp:medici:aggiungiPlayerAiMorti", Convert.ToInt32(player.Handle));
            }
        }

        private static void PlayerVivo([FromSource] Player player)
        {
            if (Morti.ToList().Contains(Convert.ToInt32(player.Handle)))
            {
                Morti.Remove(Convert.ToInt32(player.Handle));
                BaseScript.TriggerClientEvent("lprp:medici:rimuoviPlayerAiMorti", Convert.ToInt32(player.Handle));
            }
        }

        private static void Spawnato(ClientId client)
        {
            for (int i = 0; i < Morti.Count; i++)
                client.Player.TriggerEvent("lprp:medici:aggiungiPlayerAiMorti", Morti[i]);
        }
    }
}
