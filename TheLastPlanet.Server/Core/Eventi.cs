using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheLastPlanet.Server.Core.Buckets;
using TheLastPlanet.Server.Core.PlayerChar;
using TheLastPlanet.Shared;

using TheLastPlanet.Shared.PlayerChar;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Server.Core
{
    internal static class Eventi
    {
        private static int EarlyRespawnFineAmount = 5000;

        public static void Init()
        {
            EventDispatcher.Mount("lprp:dropPlayer", new Action<PlayerClient, string>(Drop));
            EventDispatcher.Mount("lprp:kickPlayerClient", new Action<string, string, int>(Kick));
            EventDispatcher.Mount("lprp:CheckPing", new Action<PlayerClient>(Ping));
            EventDispatcher.Mount("lprp:checkAFK", new Action<PlayerClient>(AFK));
            EventDispatcher.Mount("lprp:bannaPlayer", new Action<string, string, bool, long, int>(BannaPlayer));
            EventDispatcher.Mount("tlg:setStateBag", new Action<PlayerClient, string, string>(SetStateBag));
            EventDispatcher.Mount("tlg:GetUserFromServerId", new Func<int, Task<BasePlayerShared>>(GetUserFromHandle));
            EventDispatcher.Mount("tlg:getPlayers", new Func<Task<List<Player>>>(GetAllPlayers));
            EventDispatcher.Mount("tlg:getClients", new Func<PlayerClient, Task<List<PlayerClient>>>(GetAllClients));

            EventDispatcher.Mount("tlg:callPlayers", new Func<PlayerClient, Position, Task<List<PlayerClient>>>(
            async (a, b) =>
            {
                User user = a.User;
                switch (a.Status.PlayerStates.Modalita)
                {
                    case ModalitaServer.Roleplay:
                        if (user.CurrentChar is null) return null;
                        user.CurrentChar.Posizione = b;
                        TimeSpan time = (DateTime.Now - user.LastSaved);
                        if (time.Minutes > 10)
                        {
                            BaseScript.TriggerClientEvent(a.Player, "lprp:mostrasalvataggio");
                            BucketsHandler.RolePlay.SalvaPersonaggioRoleplay(a);
                            Server.Logger.Info("Salvato personaggio: '" + user.FullName + "' appartenente a '" +
                                               a.Player.Name + "' - " + user.Identifiers.Discord);
                        }
                        return BucketsHandler.RolePlay.Bucket.Players;
                    case ModalitaServer.FreeRoam:
                        if(a.Status.PlayerStates.Spawned)
                            user.FreeRoamChar.Posizione = b;
                        return BucketsHandler.FreeRoam.Bucket.Players;
                    case ModalitaServer.Lobby:
                        return BucketsHandler.Lobby.Bucket.Players;
                    case ModalitaServer.Minigiochi:
                        return null;
                    case ModalitaServer.Gare:
                        return null;
                    case ModalitaServer.Negozio:
                        return null;
                    default:
                        return Server.Instance.Clients;
                }
            }));
        }

        public static async Task<List<Player>> GetAllPlayers()
        {
            return Server.Instance.GetPlayers.OrderBy(x => Convert.ToInt32(x.Handle)).ToList();
        }
        public static async Task<List<PlayerClient>> GetAllClients(PlayerClient request)
        {


            switch (request.Status.PlayerStates.Modalita)
            {
                case ModalitaServer.Roleplay:
                    return BucketsHandler.RolePlay.Bucket.Players;
                case ModalitaServer.FreeRoam:
                    return BucketsHandler.FreeRoam.Bucket.Players;
                case ModalitaServer.Lobby:
                    return BucketsHandler.Lobby.Bucket.Players;
                case ModalitaServer.Minigiochi:
                    return null;
                case ModalitaServer.Gare:
                    return null;
                case ModalitaServer.Negozio:
                    return null;
                case ModalitaServer.UNKNOWN:
                    return Server.Instance.Clients;
                default:
                    return Server.Instance.Clients;
            }
        }

        public static async Task<BasePlayerShared> GetUserFromHandle(int handle) 
        {
            return Funzioni.GetUserFromPlayerId(handle).basePlayer;
        }
        public static void SetStateBag(PlayerClient client, string key, string value)
        {
            Server.Logger.Debug(key);
            byte[] val = value.StringToBytes();
            client.Player.SetState(key, val, true);
        }
        public static void Drop(PlayerClient client, string reason)
        {
            client.Player.Drop(reason);
        }

        public static void Ping(PlayerClient client)
        {
            if (client.Player.Ping >= Server.Impostazioni.Main.PingMax)
                client.Player.Drop("Ping troppo alto (Limite: " + Server.Impostazioni.Main.PingMax + ", tuo ping: " + client.Player.Ping + ")");
        }

        public static void AFK(PlayerClient client)
        {
            client.Player.Drop("Last Planet Shield 2.0:\nSei stato rilevato per troppo tempo in AFK");
        }

        private static async void BannaPlayer(string target, string motivazione, bool temporaneo, long tempodiban, int banner)
        {
            DateTime TempoBan = new DateTime(tempodiban);
            Player Target = Funzioni.GetPlayerFromId(target);
            List<string> Tokens = new List<string>();
            for (int i = 0; i < GetNumPlayerTokens(target); i++) Tokens.Add(GetPlayerToken(target, i));
            RequestResponse pippo = await Discord.BotDiscordHandler.InviaAlBotERicevi(new
            {
                tipo = "BannaPlayer",
                RichiestaInterna = new
                {
                    Banner = banner > 0 ? Funzioni.GetPlayerFromId(banner).Name : "Last Planet Shield 2.0",
                    Bannato = Target.Name,
                    IdMember = Target.Identifiers["discord"],
                    Motivazione = motivazione,
                    Temporaneo = temporaneo,
                    DataFine = tempodiban,
                    Tokens = Tokens.ToJson()
                }
            });

            if (pippo.content.FromJson<bool>())
            {
                Server.Logger.Warning($"{(banner > 0 ? $"Il player {Funzioni.GetPlayerFromId(banner).Name}" : "L'anticheat")} ha bannato {Target.Name}, data di fine {TempoBan.ToString("yyyy-MM-dd HH:mm:ss")}");
                BaseScript.TriggerEvent("lprp:serverLog", $"{(banner > 0 ? $"Il player {Funzioni.GetPlayerFromId(banner).Name}" : "L'anticheat")} ha bannato {Target.Name}, data di fine {TempoBan.ToString("yyyy-MM-dd HH:mm:ss")}");
                //Target.Drop($"SHIELD 2.0 Sei stato bannato dal server:\nMotivazione: {motivazione},\nBannato da: {(banner > 0 ? Funzioni.GetPlayerFromId(banner).Name : "Sistema anticheat")}"); // modificare con introduzione in stile anticheat
            }
            else
            {
                Server.Logger.Error("Ban fallito");
            }
        }

        private static void Kick(string target, string motivazione, int kicker)
        {
            Player Target = Funzioni.GetPlayerFromId(target);
            Player Kicker = Funzioni.GetPlayerFromId(kicker);
            Server.Logger.Warning($"Il player {Kicker.Name} ha kickato {Target.Name} fuori dal server, Motivazione: {motivazione}");
            BaseScript.TriggerEvent("lprp:serverLog", $"Il player {Kicker.Name} ha kickato {Target.Name} fuori dal server, Motivazione: {motivazione}");
            Target.Drop($"SHIELD 2.0 Sei stato allontanato dal server:\nMotivazione: {motivazione},\nKickato da: {Kicker.Name}");
        }
    }
}