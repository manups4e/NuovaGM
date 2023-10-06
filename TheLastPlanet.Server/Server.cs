global using CitizenFX.Core;
global using CitizenFX.Core.Native;
global using FxEvents;
global using FxEvents.Shared;
global using TheLastPlanet.Shared;
global using static CitizenFX.Core.Native.API;
using FxEvents.Shared.Snowflakes;
using Logger;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheLastPlanet.Server.Core;

namespace TheLastPlanet.Server
{
    public class Server : BaseScript
    {
        public static Log Logger { get; set; }
        public static Server Instance { get; protected set; }
        public ExportDictionary GetExports => Exports;
        public PlayerList GetPlayers => Players;
        public List<PlayerClient> Clients = new();
        public static Configurazione Impostazioni { get; set; }
        public static bool Debug { get; set; }
        public Request WebRequest { get; set; }
        public StateBag ServerState => GlobalState;
        public StateBagsHandler StateBagsHandler { get; set; }

        public Server()
        {
            EventDispatcher.Initalize("qIFBYn6qv7ZxbGLT7uzpFHa1wPCpmIHbDTWGJ8fy", "QNrAF12UC1qOvnhL6JEShdEdNiCyASUbbNpvyZPG", "Pi5V5nvCki0BcwppyczIfgy3ZZCJPqaYAeQsLZOs");
            Instance = this;
            Logger = new Log();
#if DEBUG
            SetConvarReplicated("DEBUG", "1");
            Debug = true;
#else
			SetConvarReplicated("DEBUG", "0");
#endif
            SnowflakeGenerator.Create(new Random().NextShort(100, 200));
            SetConvarServerInfo("sv_projectName", "^2THE ^0LAST ^1GALAXY.");
            SetConvarServerInfo("sv_projectDesc", "^5Un server per domarli, un server per trovarli, un server per ghermirli e nel videogioco incatenarli!");
            SetConvarServerInfo("locale", "it-IT");
            SetConvarServerInfo("tags", "RolePlay, GTAO style, MultiMode");
            SetGameType("RolePlay");
            SetMapName("The Last Planet");
            StartServer();

            EventDispatcher.Mount("testVector", new Action<Player, Tuple<Snowflake, Vector2>>(testVector));

            Tuple<Snowflake, Vector2> aa = new Tuple<Snowflake, Vector2>(Snowflake.Next(), new Vector2(15, 60));
            byte[] data = aa.ToBytes();
            Logger.Warning(data.BytesToString());

            Tuple<Snowflake, Vector2> bb = data.FromBytes<Tuple<Snowflake, Vector2>>();
            Logger.Warning(bb.ToJson());


        }

        private void testVector([FromSource] Player player, Tuple<Snowflake, Vector2> data)
        {
            GarageData garageData = new GarageData
            {
                Id = Guid.NewGuid(),
                Name = "Garage 1",
                VehLimit = 10,
                MinPermLevel = 2,
                EntranceSpawnsPed = new List<SpawnLocs>(),
                EnteranceSpawnsVeh = new List<SpawnLocs>(),
                ExitSpawnsPed = new List<SpawnLocs>(),
                ExitSpawnsVeh = new List<SpawnLocs>(),
                EntranceMarkersPed = new List<CoordsRadius>(),
                EntranceMarkersVeh = new List<CoordsRadius>(),
                ExitMarkersPed = new List<CoordsRadius>(),
                ExitMarkersVeh = new List<CoordsRadius>(),
                NoParkZones = new List<NoParkZones>()
            };

            // Add data to the lists
            garageData.EntranceSpawnsPed.Add(new SpawnLocs { Coords = new Vector3(1, 2, 3), Heading = 90 });
            garageData.EnteranceSpawnsVeh.Add(new SpawnLocs { Coords = new Vector3(4, 5, 6), Heading = 180 });
            garageData.ExitSpawnsPed.Add(new SpawnLocs { Coords = new Vector3(7, 8, 9), Heading = 270 });
            garageData.ExitSpawnsVeh.Add(new SpawnLocs { Coords = new Vector3(10, 11, 12), Heading = 0 });
            garageData.EntranceMarkersPed.Add(new CoordsRadius { Coords = new Vector3(13, 14, 15), RadiusToCheck = 5 });
            garageData.EntranceMarkersVeh.Add(new CoordsRadius { Coords = new Vector3(16, 17, 18), RadiusToCheck = 10 });
            garageData.ExitMarkersPed.Add(new CoordsRadius { Coords = new Vector3(19, 20, 21), RadiusToCheck = 15 });
            garageData.ExitMarkersVeh.Add(new CoordsRadius { Coords = new Vector3(22, 23, 24), RadiusToCheck = 20 });
            garageData.NoParkZones.Add(new NoParkZones { Start = new Vector3(25, 26, 27), End = new Vector3(28, 29, 30), Width = 3 });

            Logger.Warning(data.ToJson());
        }
        private async void StartServer()
        {
            StateBagsHandler = new StateBagsHandler();
            WebRequest = new();
            await ClassCollector.Init();
        }

        /// <summary>
        /// registra un evento (TriggerEvent)
        /// </summary>
        /// <param name="name">Nome evento</param>
        /// <param name="action">Azione legata all'evento</param>
        public void AddEventHandler(string eventName, Delegate action)
        {
            EventHandlers[eventName] += action;
        }

        /// <summary>
        /// registra un evento (TriggerEvent)
        /// </summary>
        /// <param name="name">Nome evento</param>
        /// <param name="action">Azione legata all'evento</param>
        public void RemoveEventHandler(string eventName, Delegate action)
        {
            EventHandlers[eventName] -= action;
        }

        /// <summary>
        /// Chiama il db ed esegue una Query con risultato dynamic
        /// </summary>
        /// <param name="query">Testo della query</param>
        /// <param name="parameters">Parametri da passare</param>
        /// <returns>dynamic List if more than one or a dynamic object if only one</returns>
        public async Task<dynamic> Query(string query, object parameters = null)
        {
            return await MySQL.QueryListAsync(query, parameters);
        }

        /// <summary>
        /// Esegue una query sul db modificandone il contenuto
        /// </summary>
        /// <param name="query">Testo della query</param>
        /// <param name="parameters">Parametri da passare</param>
        /// <returns></returns>
        public async Task Execute(string query, object parameters = null)
        {
            await MySQL.ExecuteAsync(query, parameters);
        }

        /// <summary>
        /// Registra una funzione OnTick
        /// </summary>
        /// <param name="action"></param>
        public void AddTick(Func<Task> onTick)
        {
            Tick += onTick;
        }

        /// <summary>
        /// Rimuove la funzione OnTick
        /// </summary>
        /// <param name="action"></param>
        public void RemoveTick(Func<Task> onTick)
        {
            Tick -= onTick;
        }

        /// <summary>
        /// registra un export, Registered GetExports still have to be defined in the __resource.lua file
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        public void RegisterExport(string name, Delegate action)
        {
            GetExports.Add(name, action);
        }

        /// <summary>
        /// registra un comando di chat
        /// </summary>
        /// <param name="commandName">Nome comando</param>
        /// <param name="handler">Una nuova Action<int source, List<dynamic> args, string rawCommand</param>
        /// <param name="restricted">tutti o solo chi può?</param>
        //public void AddCommand(string commandName, InputArgument handler, bool restricted) => API.RegisterCommand(commandName, handler, restricted);
        public void AddCommand(string commandName, Delegate handler, ModalitaServer modalita, UserGroup restricted = UserGroup.User, ChatSuggestion suggestion = null)
        {
            //API.RegisterCommand(commandName, handler, restricted);
            ChatServer.Commands.Add(new ChatCommand(commandName, restricted, modalita, handler));

            if (suggestion != null)
            {
                suggestion.name = "/" + commandName;
                ChatServer.Suggestions.Add(suggestion);
            }
        }
    }
}