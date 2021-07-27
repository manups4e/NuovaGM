using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheLastPlanet.Server.Core.PlayerChar;
using TheLastPlanet.Server.FreeRoam.Scripts.EventiFreemode;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.Core.Buckets;
using TheLastPlanet.Shared.Internal.Events;
using TheLastPlanet.Shared.PlayerChar;
using TheLastPlanet.Shared.Snowflakes;

namespace TheLastPlanet.Server.Core.Buckets
{

    // TODO: Verrà poi rimosso quando avrò un container per ogni modalità
    public class BucketsContainer
    {
        public ModalitaServer Modalita { get; set; }
        public Bucket Bucket { get; set; }
        public List<Bucket> Buckets { get; set; }

        public BucketsContainer(ModalitaServer modalitaServer, Bucket bucket)
        {
            Modalita = modalitaServer;
            Bucket = bucket;
        }
        public BucketsContainer(ModalitaServer modalitaServer, List<Bucket> buckets)
        {
            Modalita = modalitaServer;
            Buckets = buckets;
        }

        public int GetTotalPlayers()
        {
            int total = 0;
            if (Buckets != null)
                foreach (Bucket b in Buckets)
                    total += b.TotalPlayers;
            if (Bucket != null)
                total += Bucket.TotalPlayers;
            return total;
        }

    }


    public class FreeRoamBucketContainer
    {
        public ModalitaServer Modalita { get; set; }
        public Bucket Bucket { get; set; }

        public FreeRoamBucketContainer(ModalitaServer modalitaServer, Bucket bucket)
        {
            Modalita = modalitaServer;
            Bucket = bucket;
            Server.Instance.Events.Mount("lprp:Select_FreeRoamChar", new Func<ClientId, int, Task<FreeRoamChar>>(LoadFreeRoamChar));
        }

        public int GetTotalPlayers()
        {
            return Bucket.TotalPlayers;
        }

        public void AddPlayer(ClientId client)
        {
            Bucket.AddPlayer(client);
            var highscores = new List<PlayerScore>();
            foreach (var worldEvent in WorldEventsManager.WorldEvents)
                highscores.Add(new PlayerScore { EventId = worldEvent.Id, BestAttempt = 0, CurrentAttempt = 0, EventXpMultiplier = worldEvent.EventXpMultiplier });
            client.User.PlayerScores = highscores;
            BucketsHandler.UpdateBucketsCount();
        }


        public async void RemovePlayer(ClientId client, string reason = "")
        {
            Bucket.RemovePlayer(client, reason);
            await Bucket.Players.ForEachAsync(async (x) =>
            {
                x.User.showNotification($"Il player {client.Player.Name} è uscito.");
                await Task.FromResult(0);
            });
            Server.Logger.Info($"Il Player {client.Player.Name} [{client.Identifiers.Discord}] è uscito dal pianeta FreeRoam.");
            BucketsHandler.UpdateBucketsCount();
        }

        public void UpdateCurrentAttempt(ClientId client, int eventId, float currentAttempt)
        {
            try
            {
                var player = Bucket.Players.FirstOrDefault(x => x.Id == client.Id);
                if (player != null)
                {
                    var data = player.User.PlayerScores.FirstOrDefault(x => x.EventId == eventId);
                    if (data != null)
                    {
                        data.CurrentAttempt = currentAttempt;
                        if (currentAttempt > data.BestAttempt)
                            data.BestAttempt = currentAttempt;
                    }
                    else
                        Server.Logger.Warning($"Data for Event {eventId} does not exist for Player {client.Player.Name}");
                }
            }
            catch (Exception e)
            {
                Server.Logger.Error(e.ToString());
            }
        }

        public void ResetAllCurrentAttempts(int eventId)
        {
            try
            {
                foreach (var player in Bucket.Players)
                    player.User.PlayerScores.First(x => x.EventId == eventId).CurrentAttempt = 0;
            }
            catch (Exception e)
            {
                Server.Logger.Error(e.ToString());
            }
        }

        public void UpdateBestAttempt(ClientId client, int eventId, int bestAttempt)
        {
            try
            {
                var player = Bucket.Players.FirstOrDefault(x => x.Id == client.Id);
                if (player != null)
                {
                    var score = player.User.PlayerScores.FirstOrDefault(x => x.EventId == eventId);
                    if (score != null && score.BestAttempt < bestAttempt)
                        score.BestAttempt = bestAttempt;
                }
            }
            catch (Exception e)
            {
                Server.Logger.Error(e.ToString());
            }
        }


        public void SendFinalTop3Players(int eventId, float eventMultiplier)
        {
            try
            {
                if (Bucket.Players.Count == 0) { return; }

                var tempDictionary = new Dictionary<string, float>();

                foreach (var player in Bucket.Players)
                {
                    var score = player.User.PlayerScores.Where(x => x.EventId == eventId).FirstOrDefault();
                    if (score != null)
                    {
                        var xpGain = (int)Math.Min(score.CurrentAttempt * eventMultiplier, Experience.RankRequirement[player.User.FreeRoamChar.Level + 1] - Experience.RankRequirement[player.User.FreeRoamChar.Level]);

                        if (xpGain != 0)
                            ExperienceManager.OnAddExperience(player, xpGain);

                        tempDictionary.Add(player.Player.Name, score.CurrentAttempt);
                    }
                }

                if (tempDictionary.Count == 0)
                {
                    tempDictionary.Add("-1", 0);
                    tempDictionary.Add("-2", 0);
                    tempDictionary.Add("-3", 0);
                }
                else if (tempDictionary.Count == 1)
                {
                    tempDictionary.Add("-1", 0);
                    tempDictionary.Add("-2", 0);
                }
                else if (tempDictionary.Count == 2)
                {
                    tempDictionary.Add("-1", 0);
                }

                var orderByDescending = tempDictionary.OrderByDescending(x => x.Value);

                var newerDict = orderByDescending.ToDictionary(x => x.Key, x => x.Value);

                Server.Instance.Events.Send(Bucket.Players, "worldEventsManage.Client:FinalTop3", eventId, newerDict.ToJson());
            }
            catch (Exception e)
            {
                Server.Logger.Error(e.ToString());
            }
        }

        public void SendTop3Players(int eventId)
        {
            try
            {
                if (Bucket.Players.Count == 0) { return; } // should never happen but :shrug:

                var tempDictionary = new Dictionary<string, float>();

                foreach (var player in Bucket.Players)
                {
                    var score = player.User.PlayerScores.Where(x => x.EventId == eventId).FirstOrDefault();
                    if (score != null)
                    {
                        if (!tempDictionary.ContainsKey(player.Player.Name))
                            tempDictionary.Add(player.Player.Name, score.CurrentAttempt);
                    }
                }

                if (tempDictionary.Count == 1)
                {
                    tempDictionary.Add("Giocatore 1", 0);
                    tempDictionary.Add("Giocatore 2", 0);
                }
                else if (tempDictionary.Count == 2)
                {
                    tempDictionary.Add("Player 1", 0);
                }

                var newDict = tempDictionary.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

                if (newDict.Count < 3) { return; }

                Server.Instance.Events.Send(Bucket.Players, "worldEventsManage.Client:GetTop3", newDict.ToJson());
            }
            catch (Exception e)
            {
                Server.Logger.Error(e.ToString());
            }
        }

        public void SendEventData(int eventId)
        {
            try
            {
                foreach (var p in Bucket.Players)
                {
                    if (p != null)
                    {
                        var eventData = p.User.PlayerScores.FirstOrDefault(x => x.EventId == eventId);
                        p.Player.TriggerEvent("worldeventsManager.Client:GetEventData", eventData.EventId, eventData.CurrentAttempt, eventData.BestAttempt);
                    }
                }
            }
            catch (Exception e)
            {
                Server.Logger.Error(e.ToString());
            }
        }

        public int GetCurrentLevel(ClientId client)
        {
            try
            {
                var player = Bucket.Players.FirstOrDefault(x => x.Identifiers.License == client.Identifiers.License);

                if (player != null)
                {
                    return player.User.FreeRoamChar.Level;
                }
            }
            catch (Exception e)
            {
                Server.Logger.Error(e.ToString());
            }

            return -1;
        }

        public int GetCurrentExperiencePoints(ClientId client)
        {
            try
            {
                var player = Bucket.Players.FirstOrDefault(x => x.Identifiers.License == client.Identifiers.License);

                if (player != null)
                {
                    return player.User.FreeRoamChar.TotalXp;
                }
            }
            catch (Exception e)
            {
                Server.Logger.Error(e.ToString());
            }

            return 0;
        }

        public void AddExperience(ClientId client, int experiencePoints)
        {
            try
            {
                var player = Bucket.Players.FirstOrDefault(x => x.Identifiers.License == client.Identifiers.License);
                if (player != null)
                {
                    var nextLevelTotalXp = Experience.NextLevelExperiencePoints(player.User.FreeRoamChar.Level);

                    if (player.User.FreeRoamChar.TotalXp + experiencePoints >= nextLevelTotalXp)
                    {
                        int remainder = player.User.FreeRoamChar.TotalXp + experiencePoints - nextLevelTotalXp;

                        player.User.FreeRoamChar.Level++;

                        if (remainder > 0)
                            AddExperience(client, remainder);
                    }
                    else
                        player.User.FreeRoamChar.TotalXp += experiencePoints;
                }
            }
            catch (Exception e)
            {
                Server.Logger.Error(e.ToString());
            }
        }

        private async Task<FreeRoamChar> LoadFreeRoamChar(ClientId source, int id)
        {
            string query = "SELECT * FROM freeroampersonaggi WHERE UserID = @id";
            User user = Funzioni.GetClientFromPlayerId(source.Handle).User;
            FreeRoamChar_Metadata res = await MySQL.QuerySingleAsync<FreeRoamChar_Metadata>(query, new { id });
            if (res == null)
            {
                user.FreeRoamChar = new FreeRoamChar();
                Bucket?.AddPlayer(source);
                return user.FreeRoamChar;

            }
            user.FreeRoamChar = new()
            {
                Finance = new Finance(res.money, res.bank, 0),
                Posizione = res.location.FromJson<Position>(),
                Gang = new Gang(res.gang, res.gang_grade),
                Skin = res.skin.FromJson<Skin>(),
                Weapons = res.weapons.FromJson<List<Weapons>>(),
                Dressing = res.dressing.FromJson<Dressing>(),
                Statistiche = res.statistiche.FromJson<FreeRoamStats>(),
                Level = res.level,
                TotalXp = res.totalXp,
            };
            Bucket?.AddPlayer(source);
            return user.FreeRoamChar;

        }
    }

    public class RolePlayBucketsContainer
    {
        public ModalitaServer Modalita { get; set; }
        public Bucket Bucket { get; set; }

        public RolePlayBucketsContainer(ModalitaServer modalitaServer, Bucket bucket)
        {
            Modalita = modalitaServer;
            Bucket = bucket;
            Server.Instance.Events.Mount("lprp:RequestLoginInfo", new Func<ClientId, Task<List<LogInInfo>>>(LogInfo));
            Server.Instance.Events.Mount("lprp:anteprimaChar", new Func<ulong, Task<SkinAndDress>>(PreviewChar));
            Server.Instance.Events.Mount("lprp:Select_Char", new Func<ClientId, ulong, Task<Char_data>>(LoadChar));
        }

        public int GetTotalPlayers()
        {
            return Bucket.TotalPlayers;
        }

        public void AddPlayer(ClientId client)
        {
            Bucket.AddPlayer(client);
            BucketsHandler.UpdateBucketsCount();
        }

        public async void RemovePlayer(ClientId client, string reason = "")
        {
            Bucket.RemovePlayer(client, reason);
            if (client.User.status.Spawned)
            {
                await client.User.SalvaPersonaggioRoleplay();
                Server.Logger.Info($"Salvato personaggio: {client.User.FullName} [{client.Player.Name}] all'uscita dal pianeta RolePlay -- Discord:{client.Identifiers.Discord}");
            }
            else
                Server.Logger.Info($"Il Player {client.Player.Name} [{client.Identifiers.Discord}] è uscito dal pianeta RolePlay senza selezionare un personaggio.");
            BucketsHandler.UpdateBucketsCount();

        }

        #region EVENTS

        private static async Task<List<LogInInfo>> LogInfo(ClientId client)
        {
            Server.Logger.Debug(client.User.ToJson());
            string query = "SELECT CharID, info, money, bank FROM personaggi WHERE UserID = @id";
            var info = await MySQL.QueryListAsync(query, new
            {
                client.User.ID
            });
            List<LogInInfo> result = new();
            foreach (var ii in info)
            {
                var p = (ii.info as string).FromJson<Info>();
                result.Add(new LogInInfo()
                {
                    ID = ((long)ii.CharID).ToString(),
                    firstName = p.firstname,
                    lastName = p.lastname,
                    dateOfBirth = p.dateOfBirth,
                    Money = ii.money,
                    Bank = ii.bank,
                });
            }

            return result;
        }

        private static async Task<SkinAndDress> PreviewChar(ulong id)
        {

            string querySkin = "SELECT skin FROM personaggi WHERE CharID = @id";
            string queryDress = "SELECT dressing FROM personaggi WHERE CharID = @id";
            string queryLoc = "SELECT location FROM personaggi WHERE CharID = @id";

            string skin = await MySQL.QuerySingleAsync<string>(querySkin, new { id });
            string dress = await MySQL.QuerySingleAsync<string>(queryDress, new { id });
            string loc = await MySQL.QuerySingleAsync<string>(queryLoc, new { id });

			SkinAndDress result = new()
            {
                Skin = skin.FromJson<Skin>(),
                Position = loc.FromJson<Position>(),
                Dressing = dress.FromJson<Dressing>()
            };
            return result;
        }

        private static async Task<Char_data> LoadChar(ClientId source, ulong id)
        {
            string query = "SELECT * FROM personaggi WHERE CharID = @id";
            User user = Funzioni.GetClientFromPlayerId(source.Handle).User;
            Char_Metadata res = await MySQL.QuerySingleAsync<Char_Metadata>(query, new { id });
            user.CurrentChar = new Char_data()
            {
                Info = res.info.FromJson<Info>(),
                Finance = new Finance(res.money, res.bank, res.dirtyCash),
                Posizione = res.location.FromJson<Position>(),
                Job = new Job(res.job, res.job_grade),
                Gang = new Gang(res.gang, res.gang_grade),
                Skin = res.skin.FromJson<Skin>(),
                Inventory = res.inventory.FromJson<List<Inventory>>(),
                Weapons = res.weapons.FromJson<List<Weapons>>(),
                Dressing = res.dressing.FromJson<Dressing>(),
                Needs = res.needs.FromJson<Needs>(),
                Statistiche = res.statistiche.FromJson<Statistiche>()
            };
            return user.CurrentChar;
        }

        #endregion

    }

    public class LobbyBucketsContainer
	{
        public ModalitaServer Modalita { get; set; }
        public Bucket Bucket { get; set; }

        public LobbyBucketsContainer(ModalitaServer modalitaServer, Bucket bucket)
        {
            Modalita = modalitaServer;
            Bucket = bucket;
        }

        public int GetTotalPlayers()
        {
            if (Bucket != null)
                return Bucket.TotalPlayers;
            return 0;
        }

        public void AddPlayer(ClientId client)
        {
            Bucket.AddPlayer(client);
            BucketsHandler.UpdateBucketsCount();
        }

        public async void RemovePlayer(ClientId client, string reason = "")
        {
            Bucket.RemovePlayer(client, reason);
            BucketsHandler.UpdateBucketsCount();
        }
    }
}
