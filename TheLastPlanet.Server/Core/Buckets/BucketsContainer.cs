using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheLastPlanet.Server.Core.PlayerChar;
using TheLastPlanet.Server.FreeRoam.Scripts.EventiFreemode;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.Core.Buckets;
using TheLastPlanet.Shared.Internal.Events;

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
            Server.Instance.Events.Mount("tlg:Select_FreeRoamChar", new Func<ClientId, int, Task<FreeRoamChar>>(LoadFreeRoamChar));
            Server.Instance.Events.Mount("tlg:Save_FreeRoamChar", new Action<ClientId>(SavePlayerData));
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
            SetPlayerCullingRadius(client.Handle.ToString(), 5000f);
        }


        public async void RemovePlayer(ClientId client, string reason = "")
        {
            Bucket.RemovePlayer(client, reason);
            SavePlayerData(client);
            if (Bucket.Players.Count > 0)
            {
                await Bucket.Players.ForEachAsync(async (x) =>
                {
                    x.User.showNotification($"Il player {client.Player.Name} è uscito.");
                    await Task.FromResult(0);
                });
            }
            Server.Logger.Info($"Il Player {client.Player.Name} [{client.Identifiers.Discord}] è uscito dal pianeta FreeRoam.");
            SetPlayerCullingRadius(client.Handle.ToString(), 0f);
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
                    if (score != null && score.CurrentAttempt > 0)
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
                    if (score != null && score.CurrentAttempt > 0)
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
            //API.DeleteResourceKvpNoSync($"freeroam:player_{source.User.Identifiers.Discord}:char_model");
            if (source.User.ID != id) return null;
            string sbytes = API.GetResourceKvpString($"freeroam:player_{source.User.Identifiers.Discord}:char_model");
            if (string.IsNullOrWhiteSpace(sbytes) || sbytes == "00")
            {
                source.User.FreeRoamChar = new();
            }
            else
            {
                byte[] bytes = sbytes.StringToBytes();
                source.User.FreeRoamChar = bytes.FromBytes<FreeRoamChar>();
            }
            await BaseScript.Delay(0);
            return source.User.FreeRoamChar;
        }

        private void SavePlayerData(ClientId client)
        {
            client.User.FreeRoamChar.Posizione = client.Ped.Position.ToPosition();
            API.SetResourceKvpNoSync($"freeroam:player_{client.User.Identifiers.Discord}:char_model", BitConverter.ToString(client.User.FreeRoamChar.ToBytes()));
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
            SetPlayerCullingRadius(client.Handle.ToString(), 5000f);
        }

        public void RemovePlayer(ClientId client, string reason = "")
        {
            Bucket.RemovePlayer(client, reason);
            if (client.Status.PlayerStates.Spawned)
            {
                SalvaPersonaggioRoleplay(client);
                Server.Logger.Info($"Salvato personaggio: {client.User.FullName} [{client.Player.Name}] all'uscita dal pianeta RolePlay -- Discord:{client.Identifiers.Discord}");
            }
            else
                Server.Logger.Info($"Il Player {client.Player.Name} [{client.Identifiers.Discord}] è uscito dal pianeta RolePlay senza selezionare un personaggio.");
            SetPlayerCullingRadius(client.Handle.ToString(), 0f);
        }

        #region EVENTS

        private static async Task<List<LogInInfo>> LogInfo(ClientId client)
        {
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
            User user = Funzioni.GetClientFromPlayerId(source.Handle).User;

            await BaseScript.Delay(0);

            var data = GetResourceKvpString($"roleplay:player_{source.User.Identifiers.Discord}:char_model_{id}");
            var bytes = data.StringToBytes();
            user.CurrentChar = bytes.FromBytes<Char_data>();

            /*
            string query = "SELECT * FROM personaggi WHERE CharID = @id";

            Char_Metadata res = await MySQL.QuerySingleAsync<Char_Metadata>(query, new { id });

            user.CurrentChar = new Char_data(
                id,
                res.info.FromJson<Info>(),
                new Finance(res.Money, res.Bank, res.DirtyCash),
                new Job(res.job, res.job_grade),
                new Gang(res.gang, res.gang_grade),
                res.skin.FromJson<Skin>(),
                res.dressing.FromJson<Dressing>(),
                res.weapons.FromJson<List<Weapons>>(),
                res.inventory.FromJson<List<Inventory>>(),
                res.needs.FromJson<Needs>(),
                res.statistiche.FromJson<Statistiche>(),
                res.is_dead
            ){ Posizione = res.location.FromJson<Position>() };
            */
            return user.CurrentChar;
        }

        public async void SalvaPersonaggioRoleplay(ClientId client)
        {
            try
            {
                // per sicurezza teniamo anche il database aggiornato...
                await MySQL.ExecuteAsync("call SalvaPersonaggio(@gr, @level, @time, @current, @mon, @bank, @dirty, @weap, @invent, @location, @job, @gang, @skin, @dress, @needs, @stats, @dead, @id)", new
                {
                    gr = client.User.group,
                    level = client.User.group_level,
                    time = client.User.playTime,
                    current = client.User.CurrentChar.CharID,
                    mon = client.User.Money,
                    bank = client.User.Bank,
                    dirty = client.User.DirtCash,
                    weap = client.User.CurrentChar.Weapons.ToJson(),
                    invent = client.User.CurrentChar.Inventory.ToJson(),
                    location = client.User.CurrentChar.Posizione.ToJson(),
                    job = client.User.CurrentChar.Job.ToJson(),
                    gang = client.User.CurrentChar.Gang.ToJson(),
                    skin = client.User.CurrentChar.Skin.ToJson(),
                    dress = client.User.CurrentChar.Dressing.ToJson(),
                    needs = client.User.CurrentChar.Needs.ToJson(),
                    stats = client.User.CurrentChar.Statistiche.ToJson(),
                    dead = client.User.CurrentChar.is_dead,
                    id = client.User.ID,
                });

                await BaseScript.Delay(0);
                API.SetResourceKvpNoSync($"roleplay:player_{client.User.Identifiers.Discord}:char_model_{client.User.CurrentChar.CharID}", BitConverter.ToString(client.User.CurrentChar.ToBytes()));
                client.User.LastSaved = DateTime.Now;
            }
            catch (Exception e)
            {
                Server.Logger.Error(e.ToString());
            }
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
        }

        public void RemovePlayer(ClientId client, string reason = "")
        {
            Bucket.RemovePlayer(client, reason);
        }
    }

    public class RaceBucketContainer
    {
        public ModalitaServer Modalita { get; set; }
        public Bucket Bucket { get; set; }

        public RaceBucketContainer(ModalitaServer modalitaServer, Bucket bucket)
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
        }

        public void RemovePlayer(ClientId client, string reason = "")
        {
            Bucket.RemovePlayer(client, reason);
        }

    }
}
