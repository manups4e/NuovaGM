using CitizenFX.Core;
using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core.Native;
using static CitizenFX.Core.Native.API;
using Logger;
using NuovaGM.Server.Discord;

namespace NuovaGM.Server.gmPrincipale
{
	enum SessionState
	{
		Coda,
		Grazia,
		Caricamento,
		Attivo,
	}

	enum Reserved
	{
		Public
	}

	public static class NuovaCoda
	{
		internal static string resourceName = GetCurrentResourceName();
		internal static string resourcePath = $"resources/{GetResourcePath(resourceName).Substring(GetResourcePath(resourceName).LastIndexOf("//") + 2)}";
		private static ConcurrentQueue<string> queue = new ConcurrentQueue<string>();
		private static ConcurrentQueue<string> newQueue = new ConcurrentQueue<string>();
		private static ConcurrentQueue<string> pQueue = new ConcurrentQueue<string>();
		private static ConcurrentQueue<string> newPQueue = new ConcurrentQueue<string>();
		private static ConcurrentDictionary<string, string> messages = new ConcurrentDictionary<string, string>();
		private static ConcurrentDictionary<string, SessionState> session = new ConcurrentDictionary<string, SessionState>();
		private static ConcurrentDictionary<string, int> index = new ConcurrentDictionary<string, int>();
		private static ConcurrentDictionary<string, DateTime> timer = new ConcurrentDictionary<string, DateTime>();
		private static ConcurrentDictionary<string, Player> sentLoading = new ConcurrentDictionary<string, Player>();
		internal static ConcurrentDictionary<string, int> priority = new ConcurrentDictionary<string, int>();
		internal static ConcurrentDictionary<string, Reserved> reserved = new ConcurrentDictionary<string, Reserved>();
		internal static ConcurrentDictionary<string, Reserved> slotTaken = new ConcurrentDictionary<string, Reserved>();
		private static string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 ~`!@#$%^&*()_-+={[}]|:;,.?/\\";

        private static bool allowSymbols = true;
        private static bool queueCycleComplete = false;
        private static int maxSession = 0;
        private static int publicTypeSlots = 0;
        private static double queueGraceTime = 2;
        private static double graceTime = 3;
        private static double loadTime = 4;
        private static int inQueue = 0;
        private static int inPriorityQueue = 0;
        private static string hostName = string.Empty;
        private static int lastCount = 0;
        private static bool whitelistonly = false;
        private static bool serverQueueReady = false;
        private static bool stateChangeMessages = false;
        internal static bool bannedReady = false;
        internal static bool reservedReady = false;
        internal static bool priorityReady = false;
        private static bool puoentrare = false;
        internal static string notWhitelisted = "";
        private static string TestCard = $@"{{""$schema"": ""http://adaptivecards.io/schemas/adaptive-card.json"",""type"": ""AdaptiveCard"",""version"": ""1.0"",""body"": [{{""type"": ""ColumnSet"",""columns"": [{{""type"": ""Column"",""width"": 2,""items"": [{{""type"": ""TextBlock"",""text"": ""Non sei whitelistato nel server"",""weight"": ""Bolder"",""size"": ""Medium""}},{{""type"": ""TextBlock"",""text"": ""Non hai i permessi necessari ad accedere al server."",""isSubtle"": true,""wrap"": true}},{{""type"": ""TextBlock"",""text"": ""Siamo in fase Alpha Testing, vuoi partecipare al testing e segnalare i bugs per aiutare lo sviluppo? Inserisci i tuoi dati qui sotto ed entra nel nostro Discord! (https://discord.gg/n4ep9Fq)"",""isSubtle"": true,""wrap"": true,""size"": ""Small""}},{{""type"": ""TextBlock"",""text"": ""Il tuo nome"",""wrap"": true}},{{""type"": ""Input.Text"",""id"": ""myName"",""placeholder"": ""Scrivi qui Nome o NickName""}},{{""type"": ""TextBlock"",""text"": ""Motivazione"",""wrap"": true}},{{""type"": ""Input.Text"",""id"": ""myMotivazione"",""placeholder"": ""Scrivi qui la motivazione"",""style"": ""Text""}},{{""type"": ""TextBlock"",""text"": ""Nome Discord""}}]}},{{""type"": ""Column"",""width"": 1,""items"": [{{""type"": ""Image"",""url"": ""https://miro.medium.com/max/1000/1*OQQLQscmbtr-xxxw5GKZ3w.jpeg"",""size"": ""auto""}}]}}]}},{{""type"": ""Input.Text"",""placeholder"": ""Scrivi qui NomeDiscord#0000"",""id"": ""MyDiscordId""}}],""actions"": [{{""type"": ""Action.Submit"",""title"": ""Invia""}}]}}";
        public static void Init()
        {
            LoadConfigs();
//            Server.Instance.AddEventHandler("onResourceStop", new Action<string>(OnResourceStop));
            Server.Instance.AddEventHandler("playerConnecting", new Action<Player, string, CallbackDelegate, ExpandoObject>(PlayerConnecting));
            Server.Instance.AddEventHandler("playerDropped", new Action<Player, string>(PlayerDropped));
            Server.Instance.AddEventHandler("lprp:coda: playerConnected", new Action<Player>(PlayerActivated));
            Server.Instance.AddCommand("sessione", new Action<int, List<object>, string>(QueueSession), true);
            Server.Instance.AddCommand("cambiamax", new Action<int, List<object>, string>(QueueChangeMax), true);
            Server.Instance.AddCommand("ricaricaconfig", new Action<int, List<object>, string>(ReloadConfig), true);
            Server.Instance.AddCommand("ckick", new Action<int, List<object>, string>(Kick), true);
            Server.Instance.AddCommand("steamhexfromprofile", new Action<int, List<object>, string>(DiscordProfileToHex), true);
            Server.Instance.AddCommand("exitgame", new Action<int, List<object>, string>(ExitSession), false);
            Server.Instance.AddCommand("count", new Action<int, List<object>, string>(QueueCheck), false);
            StopHardcap();
            Task.Run(QueueCycle);
//            Server.Instance.AddTick(QueueCycle);
            serverQueueReady = true;
        }

        private static void LoadConfigs()
        {
            if (hostName == string.Empty) { hostName = GetConvar("sv_hostname", string.Empty); }
            maxSession = GetConvarInt("sv_maxclients", 32);
            publicTypeSlots = maxSession;
            messages = Server.Impostazioni.Coda.messages;
            loadTime = Server.Impostazioni.Coda.loadTime;
            graceTime = Server.Impostazioni.Coda.graceTime;
            queueGraceTime = Server.Impostazioni.Coda.queueGraceTime;
            whitelistonly = Server.Impostazioni.Coda.whitelistonly;
            allowSymbols = Server.Impostazioni.Coda.allowSymbols;
            stateChangeMessages = Server.Impostazioni.Coda.stateChangeMessages;
        }

        private static void QueueCheck(int source, List<object> args, string raw)
        {
            Log.Printa(LogType.Info, $"Attualmente in Coda: {queue.Count} - Coda con priorita': {pQueue.Count}");
            session.Where(k => k.Value == SessionState.Coda).ToList().ForEach(j =>
            {
                Log.Printa(LogType.Info, $"{j.Key} in coda. Timer: {timer.TryGetValue(j.Key, out DateTime oldTimer)} Priorita: {priority.TryGetValue(j.Key, out int oldPriority)}");
            });
        }

        private static void ReloadConfig(int source, List<object> args, string raw)
        {
            LoadConfigs();
        }

        private static bool IsEverythingReady()
        {
            if (serverQueueReady && priorityReady)
            { return true; }
            return false;
        }

        private static void ExitSession(int source, List<object> args, string raw)
        {
            try
            {
                if (source == 0)
                    Log.Printa(LogType.Error, $"Questo non è un comando da console");
                else
                {
                    Player player = Server.Instance.GetPlayers.FirstOrDefault(k => k.Handle == source.ToString());
                    RemoveFrom(player.Identifiers["license"], true, true, true, true, true, true);
                    player.Drop("Exited");
                }
            }
            catch (Exception)
            {
                Log.Printa(LogType.Error, $"[{resourceName}] - ExitSession()");
            }
        }

        private static void Kick(int source, List<object> args, string raw)
        {
            try
            {
                if (args.Count != 1)
                {
                    Log.Printa(LogType.Error, $"Questo comando richiede 1 argomento. <Discord> O <License>");
                    return;
                }
                string identifier = args[0].ToString();
                Player player = Server.Instance.GetPlayers.FirstOrDefault(k => k.Identifiers["license"] == identifier || k.Identifiers["discord"] == identifier);
                if (player == null)
                {
                    Log.Printa(LogType.Warning, $"Nessun riscontro in sessione per l'ID {identifier}, usa il comando di sessione per vedere gli ID.");
                    return;
                }
                RemoveFrom(player.Identifiers["license"], true, true, true, true, true, true);
                player.Drop("Kicked");
                Log.Printa(LogType.Warning, $"{identifier} è stato kickato.");
            }
            catch (Exception)
            {
                Log.Printa(LogType.Error, $"[{resourceName}] - Kick()");
            }
        }

        private static void UpdateHostName()
        {
            try
            {
                if (hostName == string.Empty) { hostName = GetConvar("sv_hostname", string.Empty); }
                if (hostName == string.Empty) { return; }

                string concat = hostName;
                bool editHost = false;
                int count = inQueue + inPriorityQueue;
                if (Server.Impostazioni.Coda.contoCodaFineNomeServer)
                {
                    editHost = true;
                    if (count > 0) { concat = string.Format($"{concat} {messages["QueueCount"]}", count); }
                    else { concat = hostName; }
                }
                if (lastCount != count && editHost)
                {
                    SetConvar("sv_hostname", concat);
                }
                lastCount = count;
            }
            catch (Exception)
            {
                Log.Printa(LogType.Error, $"[{resourceName}] - UpdateHostName()");
            }
        }

        private static int QueueCount()
        {
            try
            {
                int place = 0;
                ConcurrentQueue<string> temp = new ConcurrentQueue<string>();
                while (!queue.IsEmpty)
                {
                    queue.TryDequeue(out string license);
                    if (IsTimeUp(license, queueGraceTime))
                    {
                        RemoveFrom(license, true, true, true, true, true, true);
                        if (stateChangeMessages)
                        {
                            var player = Server.Instance.GetPlayers.ToList().FirstOrDefault(x => license == x.Identifiers["license"]);
                            Log.Printa(LogType.Info, player != null ? $"[{resourceName}]: CANCELLATO -> RIMOSSO -> {player.Name}, Discord: {player.Identifiers["discord"]}" : $"[{resourceName}]: CANCELLATO -> RIMOSSO -> {license}");
                        }
                        continue;
                    }
                    if (priority.TryGetValue(license, out int priorityAdded))
                    {
                        newPQueue.Enqueue(license);
                        continue;
                    }
                    if (!Loading(license))
                    {
                        place += 1;
                        UpdatePlace(license, place);
                        temp.Enqueue(license);
                    }
                }
                while (!newQueue.IsEmpty)
                {
                    newQueue.TryDequeue(out string license);
                    if (!Loading(license))
                    {
                        place += 1;
                        UpdatePlace(license, place);
                        temp.Enqueue(license);
                    }
                }
                queue = temp;
                return queue.Count;
            }
            catch (Exception)
            {
                Log.Printa(LogType.Error, $"[{resourceName}] - QueueCount()"); return queue.Count;
            }
        }

        private static int PriorityQueueCount()
        {
            try
            {
                List<KeyValuePair<string, int>> order = new List<KeyValuePair<string, int>>();
                while (!pQueue.IsEmpty)
                {
                    pQueue.TryDequeue(out string license);
                    if (IsTimeUp(license, queueGraceTime))
                    {
                        RemoveFrom(license, true, true, true, true, true, true);
                        if (stateChangeMessages) 
                        {
                            var player = Server.Instance.GetPlayers.ToList().FirstOrDefault(x => license == x.Identifiers["license"]);
                            Log.Printa(LogType.Info, $"[{resourceName}]: CANCELLATO -> RIMOSSO -> {player.Name}, Discord: {player.Identifiers["discord"]}"); 
                        }
                        continue;
                    }
                    if (!priority.TryGetValue(license, out int priorityNum))
                    {
                        newQueue.Enqueue(license);
                        continue;
                    }
                    order.Insert(order.FindLastIndex(k => k.Value <= priorityNum) + 1, new KeyValuePair<string, int>(license, priorityNum));
                }
                while (!newPQueue.IsEmpty)
                {
                    newPQueue.TryDequeue(out string license);
                    priority.TryGetValue(license, out int priorityNum);
                    order.Insert(order.FindLastIndex(k => k.Value >= priorityNum) + 1, new KeyValuePair<string, int>(license, priorityNum));
                }
                int place = 0;
                order.ForEach(k =>
                {
                    if (!Loading(k.Key))
                    {
                        place += 1;
                        UpdatePlace(k.Key, place);
                        pQueue.Enqueue(k.Key);
                    }
                });
                return pQueue.Count;
            }
            catch (Exception e)
            {
                Log.Printa(LogType.Error,$"[{resourceName}] - PriorityQueueCount()\n {e}"); return pQueue.Count;
            }
        }

        private static bool Loading(string license)
        {
            try
            {
                if (session.Count(j => j.Value != SessionState.Coda) - slotTaken.Count(i => i.Value != Reserved.Public) < publicTypeSlots)
                { NewLoading(license, Reserved.Public); return true; }
                else { return false; }
            }
            catch (Exception e)
            {
                Log.Printa(LogType.Error,$"[{resourceName}] - Loading()\n {e}"); return false;
            }
        }

        private static void NewLoading(string license, Reserved slotType)
        {
            try
            {
                if (session.TryGetValue(license, out SessionState oldState))
                {
                    UpdateTimer(license);
                    RemoveFrom(license, false, true, false, false, false, false);
                    if (!slotTaken.TryAdd(license, slotType))
                    {
                        slotTaken.TryGetValue(license, out Reserved oldSlotType);
                        slotTaken.TryUpdate(license, slotType, oldSlotType);
                    }
                    session.TryUpdate(license, SessionState.Caricamento, oldState);
                    if (stateChangeMessages) 
                    {
                        var player = Server.Instance.GetPlayers.ToList().FirstOrDefault(x => license == x.Identifiers["license"]);
                        Log.Printa(LogType.Info, player!= null? $"[{resourceName}]: CODA -> CARICAMENTO -> ({Enum.GetName(typeof(Reserved), slotType)}) {player.Name}, Discord: {player.Identifiers["discord"]}" : $"[{resourceName}]: CODA -> CARICAMENTO -> ({Enum.GetName(typeof(Reserved), slotType)}) {license}"); 
                    }
                }
            }
            catch (Exception e)
            {
                Log.Printa(LogType.Error,$"[{resourceName}] - NewLoading()\n {e}");
            }
        }

        private static  bool IsTimeUp(string license, double time)
        {
            try
            {
                if (!timer.ContainsKey(license)) { return false; }
                return timer[license].AddMinutes(time) < DateTime.UtcNow;
            }
            catch (Exception e)
            {
                Log.Printa(LogType.Error,$"[{resourceName}] - IsTimeUp()\n {e}"); return false;
            }
        }

        private static  void UpdatePlace(string license, int place)
        {
            try
            {
                if (!index.TryAdd(license, place))
                {
                    index.TryGetValue(license, out int oldPlace);
                    index.TryUpdate(license, place, oldPlace);
                }
            }
            catch (Exception e)
            {
                Log.Printa(LogType.Error,$"[{resourceName}] - UpdatePlace()\n {e}");
            }
        }

        private static  void UpdateTimer(string license)
        {
            try
            {
                if (!timer.TryAdd(license, DateTime.UtcNow))
                {
                    timer.TryGetValue(license, out DateTime oldTime);
                    timer.TryUpdate(license, DateTime.UtcNow, oldTime);
                }
            }
            catch (Exception e)
            {
                Log.Printa(LogType.Error,$"[{resourceName}] - UpdateTimer()\n {e}");
            }
        }

        private static void UpdateStates()
        {
            try
            {
                session.Where(k => k.Value == SessionState.Caricamento || k.Value == SessionState.Grazia).ToList().ForEach(j =>
                {
                    string license = j.Key;
                    SessionState state = j.Value;
                    switch (state)
                    {
                        case SessionState.Caricamento:
                            if (!timer.TryGetValue(license, out DateTime oldLoadTime))
                            {
                                UpdateTimer(license);
                                break;
                            }
                            if (IsTimeUp(license, loadTime))
                            {
                                if (Server.Instance.GetPlayers.FirstOrDefault(i => i.Identifiers["license"] == license)?.EndPoint != null)
                                {
                                    Server.Instance.GetPlayers.FirstOrDefault(i => i.Identifiers["license"] == license).Drop($"{messages["Timeout"]}");
                                }
                                session.TryGetValue(license, out SessionState oldState);
                                session.TryUpdate(license, SessionState.Grazia, oldState);
                                UpdateTimer(license);
                                if (stateChangeMessages) 
                                {
                                    var player = Server.Instance.GetPlayers.ToList().FirstOrDefault(x => license == x.Identifiers["license"]);
                                    Log.Printa(LogType.Info, $"[{resourceName}]: CARICAMENTO -> GRAZIA -> {player.Name}, Discord: {player.Identifiers["discord"]}"); 
                                }
                            }
                            else
                            {
                                if (sentLoading.ContainsKey(license) && Server.Instance.GetPlayers.FirstOrDefault(i => i.Identifiers["license"] == license) != null)
                                {
                                    BaseScript.TriggerEvent("lprp:coda: newloading", sentLoading[license]);
                                    sentLoading.TryRemove(license, out Player oldPlayer);
                                }
                            }
                            break;
                        case SessionState.Grazia:
                            if (!timer.TryGetValue(license, out DateTime oldGraceTime))
                            {
                                UpdateTimer(license);
                                break;
                            }
                            if (IsTimeUp(license, graceTime))
                            {
                                if (Server.Instance.GetPlayers.FirstOrDefault(i => i.Identifiers["license"] == license)?.EndPoint != null)
                                {
                                    if (!session.TryAdd(license, SessionState.Attivo))
                                    {
                                        session.TryGetValue(license, out SessionState oldState);
                                        session.TryUpdate(license, SessionState.Attivo, oldState);
                                    }
                                }
                                else
                                {
                                    RemoveFrom(license, true, true, true, true, true, true);
                                    if (stateChangeMessages) 
                                    {
                                        var player = Server.Instance.GetPlayers.ToList().FirstOrDefault(x => license == x.Identifiers["license"]);
                                        Log.Printa(LogType.Info, player != null ? $"[{resourceName}]: GRAZIA -> RIMOSSO -> {player.Name}, Discord: {player.Identifiers["discord"]}" : $"[{resourceName}]: GRAZIA -> RIMOSSO -> {license}"); 
                                    }
                                }
                            }
                            break;
                    }
                });
            }
            catch (Exception e)
            {
                Log.Printa(LogType.Error,$"[{resourceName}] - UpdateStates()\n {e}");
            }
        }

        private static  void RemoveFrom(string license, bool doSession, bool doIndex, bool doTimer, bool doPriority, bool doReserved, bool doSlot)
        {
            try
            {
                if (doSession) { session.TryRemove(license, out SessionState oldState); }
                if (doIndex) { index.TryRemove(license, out int oldPosition); }
                if (doTimer) { timer.TryRemove(license, out DateTime oldTime); }
                if (doPriority) { priority.TryRemove(license, out int oldPriority); }
                if (doReserved) { reserved.TryRemove(license, out Reserved oldReserved); }
                if (doSlot) { slotTaken.TryRemove(license, out Reserved oldSlot); }
            }
            catch (Exception e)
            {
                Log.Printa(LogType.Error,$"[{resourceName}] - RemoveFrom()\n {e}");
            }
        }

        private static async void QueueSession(int source, List<object> args, string raw)
        {
            try
            {
                if (args.Count != 0)
                {
                    Log.Printa(LogType.Error, $"Questo comando non ha argomenti.");
                    return;
                }
                if (session.Count == 0)
                {
                    Log.Printa(LogType.Error, $"Nessun player in sessione");
                    return;
                }
                if (source == 0)
                {
                    Debug.WriteLine($"| LICENSE" + new string(' ', 33) + " | STATO IN CODA | DISCORD" + new string(' ', 11) + " | STEAM" + new string(' ', 10) + " | PRIORITA' | RISERVATO | SLOT USATO | HANDLE | NOME");
                    session.OrderByDescending(k => k.Value).ToList().ForEach(j =>
                    {
                        Player player = Server.Instance.GetPlayers.FirstOrDefault(i => i.Identifiers["license"] == j.Key);
                        if (player == null)
                            sentLoading.TryGetValue(j.Key, out player);

                        if (!priority.TryGetValue(j.Key, out int oldPriority)) { oldPriority = 0; }
                        if (!reserved.TryGetValue(j.Key, out Reserved oldReserved)) { oldReserved = Reserved.Public; }
                        if (!slotTaken.TryGetValue(j.Key, out Reserved oldSlot)) { oldSlot = Reserved.Public; }
                        Debug.WriteLine($"| {j.Key} | {j.Value}{new string(' ', 13 - j.Value.ToString().Length)} | {player?.Identifiers["discord"]} | {player?.Identifiers["STEAM"]} | {oldPriority}{new string(' ', 9 - oldPriority.ToString().Length)} | {oldReserved}{new string(' ', 9 - oldReserved.ToString().Length)} | {oldSlot}{new string(' ', 10 - oldSlot.ToString().Length)} | {player?.Handle}{new string(' ', 6 - player.Handle.Length)} | {player?.Name}");
                    });
                }
                else
                {
                    List<dynamic> sessionReturn = new List<dynamic>();
                    session.OrderByDescending(k => k.Value).ToList().ForEach(j =>
                    {
                        dynamic temp;
                        Player player = Server.Instance.GetPlayers.FirstOrDefault(i => i.Identifiers["license"] == j.Key);
                        if (player == null)
                        { sentLoading.TryGetValue(j.Key, out player); }
                        if (!priority.TryGetValue(j.Key, out int oldPriority)) { oldPriority = 0; }
                        if (!reserved.TryGetValue(j.Key, out Reserved oldReserved)) { oldReserved = Reserved.Public; }
                        if (!slotTaken.TryGetValue(j.Key, out Reserved oldSlot)) { oldSlot = Reserved.Public; }
                        temp = new { License = j.Key, State = j.Value, Discord = player?.Identifiers["discord"], Steam = player?.Identifiers["steam"], Priority = oldPriority, Handle = player?.Handle, Name = player?.Name };
                        sessionReturn.Add(temp);
                    });
                    Player requested = Server.Instance.GetPlayers.FirstOrDefault(k => k.Handle == source.ToString());
                    requested.TriggerEvent("lprp:coda: sessionResponse", JsonConvert.SerializeObject(sessionReturn));
                }
            }
            catch (Exception e)
            {
                Log.Printa(LogType.Error,$"[{resourceName}] - QueueSession()\n {e}");
            }
            await BaseScript.Delay(0);
        }

        private static  async void QueueChangeMax(int source, List<object> args, string raw)
        {
            try
            {
                if (args.Count != 1) { Log.Printa(LogType.Error, $"Il comando vuole 1 argomento. Esempio: changemax 32"); return; }
                int newMax = int.Parse(args[0].ToString());
                if (newMax <= 0 || newMax > 128) { Log.Printa(LogType.Warning, $"changemax DEVE essere tra 1 e 128"); return; }
                while (!queueCycleComplete)
                {
                    await BaseScript.Delay(0);
                }
                maxSession = newMax;
            }
            catch (Exception e)
            {
                Log.Printa(LogType.Error,$"[{resourceName}] - QueueChangeMax()\n {e}");
            }
        }

        private static async void StopHardcap()
        {
            await BaseScript.Delay(0);
            try
            {
                ExecuteCommand($"sets lprp:coda Enabled");
                int attempts = 0;
                while (attempts < 7)
                {
                    attempts += 1;
                    string state = GetResourceState("hardcap");
                    if (state == "missing") break;
                    if (state == "started")
                    {
                        StopResource("hardcap");
                        break;
                    }
                    await BaseScript.Delay(5000);
                }
            }
            catch (Exception e)
            {
                Log.Printa(LogType.Error,$"[{resourceName}] - StopHardcap()\n {e}");
            }
        }

        private static  void PlayerDropped([FromSource] Player source, string message)
        {
            try
            {
                string license = source.Identifiers["license"];
                string discord = source.Identifiers["discord"];
                if (license == null) return;
                if (!session.ContainsKey(license) || message == "Exited") return;
                bool hasState = session.TryGetValue(license, out SessionState oldState);
                if (hasState && oldState != SessionState.Coda)
                {
                    session.TryUpdate(license, SessionState.Grazia, oldState);
                    if (stateChangeMessages) { Log.Printa(LogType.Info, $"[{resourceName}]: {Enum.GetName(typeof(SessionState), oldState).ToUpper()} -> GRACE -> {source.Name}, Discord: {discord}"); }
                    UpdateTimer(license);
                }
            }
            catch (Exception e)
            {
                Log.Printa(LogType.Error,$"[{resourceName}] - PlayerDropped()\n {e}");
            }
        }

        private static  void PlayerActivated([FromSource] Player source)
        {
            try
            {
                string license = source.Identifiers["license"];
                if (!session.ContainsKey(license))
                {
                    session.TryAdd(license, SessionState.Attivo);
                    return;
                }
                session.TryGetValue(license, out SessionState oldState);
                session.TryUpdate(license, SessionState.Attivo, oldState);
                if (stateChangeMessages) { Log.Printa(LogType.Info, $"[{resourceName}]: {Enum.GetName(typeof(SessionState), oldState).ToUpper()} -> ACTIVE -> {source.Name}, Discord: {source.Identifiers["discord"]}"); }
            }
            catch (Exception e)
            {
                Log.Printa(LogType.Error,$"[{resourceName}] - PlayerActivated()\n {e}");
            }
        }

        private static  bool ValidName(string playerName)
        {
            char[] chars = playerName.ToCharArray();

            char lastCharacter = new char();
            foreach (char currentCharacter in chars)
            {
                if (!allowedChars.ToCharArray().Contains(currentCharacter)) { return false; }
                if (char.IsWhiteSpace(currentCharacter) && char.IsWhiteSpace(lastCharacter)) { return false; }
                lastCharacter = currentCharacter;
            }
            return true;
        }

        private static  async void PlayerConnecting([FromSource]Player source, string playerName, dynamic denyWithReason, dynamic deferrals)
        {
            try
            {
                deferrals.defer();
                await BaseScript.Delay(500);
                while (!IsEverythingReady()) { await BaseScript.Delay(0); }
                string license = source.Identifiers["license"];
                string discord = source.Identifiers["discord"];
                string steam = source.Identifiers["steam"];
                if (discord == null) { deferrals.done($"{messages["Discord"]}"); return; }
                if (license == null) { deferrals.done($"{messages["License"]}"); return; }
                //if (discord == null) { deferrals.done($"{messages["Discord"]}"); return; }
                string ControlloLicenza = "{\"$schema\":\"http://adaptivecards.io/schemas/adaptive-card.json\",\"type\": \"AdaptiveCard\",\"version\": \"1.0\",\"body\": [{\"type\": \"TextBlock\",\"text\": \"" + messages["Gathering"] + "\"}],\"backgroundImage\": {\"url\": \"https://s5.gifyu.com/images/ezgif.com-resize-1887cbdf86515eeeb.gif\",\"horizontalAlignment\": \"Center\"},\"minHeight\": \"360px\",\"verticalContentAlignment\": \"Bottom\"}";
                /*https://s5.gifyu.com/images/Blue_Sky_and_Clouds_Timelapse_0892__Videvo.gif */
                deferrals.presentCard(ControlloLicenza);
                await BaseScript.Delay(3000);

                puoentrare = await BotDiscordHandler.DoesPlayerHaveRole(discord, Server.Impostazioni.Coda.permessi);
                await BaseScript.Delay(1000);

                if (puoentrare)
                {
                    if (!allowSymbols && !ValidName(playerName)) { deferrals.done($"{messages["Symbols"]}"); return; }

                    if (sentLoading.ContainsKey(license))
                        sentLoading.TryRemove(license, out Player oldPlayer);
                    sentLoading.TryAdd(license, source);

                    if (Server_Priority.newwhitelist.Exists(k => k.License == license || k.Discord == discord))
                        Server_Priority.AutoWhitelist(new PriorityAccount(license, discord, Server_Priority.newwhitelist.FirstOrDefault(k => k.License == license || k.Discord == discord).Priority));
                    if (Server_Priority.accounts.Exists(k => k.License == license))
                    {
                        if (!priority.TryAdd(license, Server_Priority.accounts.FirstOrDefault(k => k.License == license).Priority))
                        {
                            priority.TryGetValue(license, out int oldPriority);
                            priority.TryUpdate(license, Server_Priority.accounts.FirstOrDefault(k => k.License == license).Priority, oldPriority);
                        }
                    }
                    else
                        RemoveFrom(license, false, false, false, true, false, false);

                    if (session.TryAdd(license, SessionState.Coda))
                    {
                        if (!priority.ContainsKey(license))
                        {
                            newQueue.Enqueue(license);
                            if (stateChangeMessages) { Log.Printa(LogType.Info, $"[{resourceName}]: NUOVO PLAYER -> IN CODA -> (Pubblico) {playerName}, Discord: {discord}"); }
                        }
                        else
                        {
                            newPQueue.Enqueue(license);
                            if (stateChangeMessages) { Log.Printa(LogType.Info,$"[{resourceName}]: NUOVO PLAYER -> IN CODA -> (Priorità) {playerName}, {discord}"); }
                        }
                        string inCoda = "{\"$schema\":\"http://adaptivecards.io/schemas/adaptive-card.json\",\"type\": \"AdaptiveCard\",\"version\": \"1.0\",\"body\": [{\"type\": \"TextBlock\",\"text\": \"Shield 2.0: Accesso consentito, attendi...\"}],\"backgroundImage\": {\"url\": \"https://s5.gifyu.com/images/ezgif.com-resize-1887cbdf86515eeeb.gif\",\"horizontalAlignment\": \"Center\"},\"minHeight\": \"360px\",\"verticalContentAlignment\": \"Bottom\"}";
                        deferrals.presentCard(inCoda);
                        await BaseScript.Delay(2000);
                    }

                    if (!session[license].Equals(SessionState.Coda))
                    {
                        UpdateTimer(license);
                        session.TryGetValue(license, out SessionState oldState);
                        session.TryUpdate(license, SessionState.Caricamento, oldState);
                        deferrals.done();
                        if (stateChangeMessages) { Log.Printa(LogType.Info,$"[{resourceName}]: {Enum.GetName(typeof(SessionState), oldState).ToUpper()} -> CARICAMENTO -> (Grace) {source.Name}, Discord: {discord}"); }
                        return;
                    }

                    bool inPriority = priority.ContainsKey(license);
                    int dots = 0;

                    while (session[license].Equals(SessionState.Coda))
                    {
                        if (index.ContainsKey(license) && index.TryGetValue(license, out int position))
                        {
                            int count = inPriority ? inPriorityQueue : inQueue;
                            string message = inPriority ? $"{messages["PriorityQueue"]}" : $"{messages["Queue"]}";
                            string coda = $"{message} {position} / {count}{new string('.', dots)}";
                            string InCoda = "{\"$schema\":\"http://adaptivecards.io/schemas/adaptive-card.json\",\"type\": \"AdaptiveCard\",\"version\": \"1.0\",\"body\": [{\"type\": \"TextBlock\",\"text\": \"" + coda + "\"}],\"backgroundImage\": {\"url\": \"https://s5.gifyu.com/images/ezgif.com-resize-1887cbdf86515eeeb.gif\",\"horizontalAlignment\": \"Center\"},\"minHeight\": \"360px\",\"verticalContentAlignment\": \"Bottom\"}";
                            deferrals.presentCard(InCoda);
                        }
                        dots = dots > 2 ? 0 : dots + 1;
                        if (source?.EndPoint == null)
                        {
                            UpdateTimer(license);
                            deferrals.done($"{messages["Canceled"]}");
                            if (stateChangeMessages) { Log.Printa(LogType.Info,$"[{resourceName}]: IN CODA -> CANCELLATO -> {source.Name}, Discord: {discord}"); }
                            return;
                        }
                        RemoveFrom(license, false, false, true, false, false, false);
                        await BaseScript.Delay(5000);
                    }
                    //ingresso finalmente
                    await BaseScript.Delay(500);
                    deferrals.done();
                }
                else
                {
                    RemoveFrom(license, false, false, false, false, true, true);
                    if (whitelistonly)
                    {
                        deferrals.presentCard(TestCard, new Action<dynamic>(async (var) => {
                            object dati = new { tipo = "RichiestaIngresso", RichiestaInterna = var, items = source.Identifiers.ToList(), nome = source.Name};
                            deferrals.done("Grazie di esserti candidato! I nostri admin prenderanno in considerazione la candidatura e se la riterranno valida ti contatteranno!\n" +
                                "Resta aggiornato sul mio server discord con invito https://discord.gg/n4ep9Fq!");
                            await BotDiscordHandler.InviaAlBot(dati);
                        }));
//                        deferrals.done($"{messages["Whitelist"]}");
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Printa(LogType.Error, e.ToString());
                string card = "{\"$schema\": \"http://adaptivecards.io/schemas/adaptive-card.json\",\"type\": \"AdaptiveCard\",\"version\": \"1.3\",\"body\": [{\"type\": \"ColumnSet\",\"columns\": [{\"type\": \"Column\",\"width\": 2,\"items\": [{\"type\": \"TextBlock\",\"text\": \"Errore sconosciuto\",\"weight\": \"Bolder\",\"size\": \"Medium\"},{\"type\": \"TextBlock\",\"text\": \"Siamo spiacenti, l'accesso al server o la comunicazione con il bot ha subito un errore imprevisto!\",\"isSubtle\": true,\"wrap\": true},{\"type\": \"TextBlock\",\"text\": \"Per farci perdonare, ecco qui l'immagine di un gattino su di un unicorno!\",\"isSubtle\": true,\"wrap\": true,\"size\": \"Small\"}]},{\"type\": \"Column\",\"width\": 1,\"items\": [{\"type\": \"Image\",\"url\": \"https://iyanceres.files.wordpress.com/2018/02/cat-unicorn.jpg\",\"size\": \"auto\"}]}]}]}";
                deferrals.presentCard(card);
               // deferrals.done($"{messages["Error"]}"); return;
            }
        }

        private static async Task QueueCycle()
        {
            try
            {
                while (true)
                {
                    inPriorityQueue = PriorityQueueCount();
                    await BaseScript.Delay(100);
                    inQueue = QueueCount();
                    await BaseScript.Delay(100);
                    UpdateHostName();
                    UpdateStates();
                    await BaseScript.Delay(1000);
                }
            }
            catch (Exception e)
            {
                Log.Printa(LogType.Error,$"[{resourceName}] - QueueCycle()\n {e}");
            }
        }

        private static  void DiscordProfileToHex(int source, List<object> args, string raw)
        {
            try
            {
                if (args.Count != 1) { Log.Printa(LogType.Error, $"Il comando richiede un argomento. <Discord Community Profile Number>"); }
                long SteamCommunityId = long.Parse(args[0].ToString());
                long quotient = Math.DivRem(SteamCommunityId, 16, out long remainder);
                Stack<long> hex = new Stack<long>();
                hex.Push(remainder);
                while (quotient != 0)
                {
                    quotient = Math.DivRem(quotient, 16, out remainder);
                    hex.Push(remainder);
                }
                string steamHex = string.Empty;
                while (hex.Count != 0)
                    steamHex = string.Concat(steamHex, hex.Pop().ToString("x"));
                Debug.WriteLine($"{steamHex}");
            }
            catch (Exception e)
            {
                Log.Printa(LogType.Error,$"[{resourceName}] - DiscordProfileToHex()\n {e}");
            }
        }
    }

    class PriorityAccount
    {
        public string License { get; set; }
        public string Discord { get; set; }
        public int Priority { get; set; }

        public PriorityAccount(string license, string discord, int priority)
        {
            License = license;
            Discord = discord;
            Priority = priority;
        }
    }

    class Server_Priority : BaseScript
    {
        static readonly string directory = $"{NuovaCoda.resourcePath}/JSON/Priority";
        static List<FileInfo> files = new List<FileInfo>();
        internal static List<PriorityAccount> accounts = new List<PriorityAccount>();
        internal static List<PriorityAccount> newwhitelist = new List<PriorityAccount>();

        public Server_Priority()
        {
            try
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                DirectoryInfo di = new DirectoryInfo(directory);
                files = di.GetFiles("*.json").ToList();
                files.ForEach(k =>
                {
                    accounts.Add(JsonConvert.DeserializeObject<PriorityAccount>(File.ReadAllText(k.FullName).ToString()));
                });
                accounts.ForEach(k =>
                {
                    if (k.Priority <= 0 || k.Priority > 100)
                    {
                        k.Priority = 100;
                        string path = $"{directory}/{k.License}-{k.Discord}.json";
                        File.WriteAllText(path, JsonConvert.SerializeObject(k));
                    }
                    NuovaCoda.priority.TryAdd(k.License, k.Priority);
                });
                if (File.Exists($"{NuovaCoda.resourcePath}/JSON/offlinepriority.json"))
                {
                    newwhitelist = JsonConvert.DeserializeObject<List<PriorityAccount>>(File.ReadAllText($"{NuovaCoda.resourcePath}/JSON/offlinepriority.json").ToString());
                }
                RegisterCommand("daipriorita", new Action<int, List<object>, string>(Add), true);
                RegisterCommand("rimuovipriorita", new Action<int, List<object>, string>(Remove), true);
                NuovaCoda.priorityReady = true;
            }
            catch (Exception)
            {
                Log.Printa(LogType.Error, $"[{NuovaCoda.resourceName}] - Server_Priority.Start()");
            }
        }

        internal void Add(int source, List<object> args, string raw)
        {
            try
            {
                if (args.Count != 2)
                {
                    Log.Printa(LogType.Warning, $"Questo comando richiede 2 argomenti. <Discord> O <License> & <Priorità> (1 <- 100)");
                    return;
                }
                string identifier = args[0].ToString();
                int priority = int.Parse(args[1].ToString());
                if (priority <= 0 || priority > 100) { priority = 100; }
                Player player = Server.Instance.GetPlayers.FirstOrDefault(k => k.Identifiers["license"] == identifier || k.Identifiers["discord"] == identifier);
                if (player != null)
                {
                    PriorityAccount account = new PriorityAccount(player.Identifiers["license"], player.Identifiers["discord"], priority);
                    accounts.Add(account);
                    if (!NuovaCoda.priority.TryAdd(account.License, priority))
                    {
                        NuovaCoda.priority.TryGetValue(account.License, out int oldPriority);
                        NuovaCoda.priority.TryUpdate(account.License, priority, oldPriority);
                    }
                    string path = $"{directory}/{account.License}-{account.Discord}.json";
                    File.WriteAllText(path, JsonConvert.SerializeObject(account));
                    Log.Printa(LogType.Info, $"{identifier} è stato settato in priorità {priority}.");
                }
                else
                {
                    Log.Printa(LogType.Warning, $"Nessun account trovato in sessione per {identifier}, aggiunto nella lista priorità offline");
                    newwhitelist.Add(new PriorityAccount(identifier, identifier, priority));
                    string path = $"{NuovaCoda.resourcePath}/JSON/offlinepriority.json";
                    File.WriteAllText(path, JsonConvert.SerializeObject(newwhitelist));
                }
            }
            catch (Exception)
            {
                Log.Printa(LogType.Error, $"[{NuovaCoda.resourceName}] - Server_Priority.Add()");
            }
            return;
        }

        internal static void Remove(int source, List<object> args, string raw)
        {
            try
            {
                if (args.Count != 1)
                {
                    Log.Printa(LogType.Warning, $"Questo comando richiede 1 argomento. <Discord> OR <License>");
                    return;
                }
                string identifier = args[0].ToString();
                newwhitelist.Where(k => k.License == identifier || k.Discord == identifier).ToList().ForEach(j =>
                {
                    newwhitelist.Remove(j);
                });
                string path = $"{NuovaCoda.resourcePath}/JSON/offlinepriority.json";
                File.WriteAllText(path, JsonConvert.SerializeObject(newwhitelist));
                accounts.Where(k => k.License == identifier || k.Discord == identifier).ToList().ForEach(j =>
                {
                    path = $"{directory}/{j.License}-{j.Discord}.json";
                    File.Delete(path);
                    accounts.Remove(j);
                });
                NuovaCoda.priority.TryRemove(identifier, out int oldPriority);
                Log.Printa(LogType.Info, $"{identifier} rimosso dalla lista priorità.");
            }
            catch (Exception)
            {
                Log.Printa(LogType.Error, $"[{NuovaCoda.resourceName}] - Server_Priority.Remove()");
            }
            return;
        }

        internal static void AutoWhitelist(PriorityAccount account)
        {
            try
            {
                accounts.Add(account);
                string path = $"{directory}/{account.License}-{account.Discord}.json";
                File.WriteAllText(path, JsonConvert.SerializeObject(account));
                newwhitelist.RemoveAll(k => k.License == account.License || k.Discord == account.Discord);
                path = $"{NuovaCoda.resourcePath}/JSON/offlinepriority.json";
                File.WriteAllText(path, JsonConvert.SerializeObject(newwhitelist));
                Log.Printa(LogType.Info, $"{account.License}-{account.Discord} prioritizzato automaticamente.");
            }
            catch (Exception)
            {
                Log.Printa(LogType.Error, $"[{NuovaCoda.resourceName}] - Server_Priority.AutoWhitelist()");
            }
        }
    }

    class ReservedAccount
    {
        public string License { get; set; }
        public string Discord { get; set; }
        public Reserved Reserve { get; set; }

        public ReservedAccount(string license, string discord, Reserved reserve)
        {
            License = license;
            Discord = discord;
            Reserve = reserve;
        }
    }
}