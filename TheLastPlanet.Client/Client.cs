global using CitizenFX.Core;
global using CitizenFX.Core.Native;
global using CitizenFX.Core.UI;
global using FxEvents;
global using FxEvents.Shared;
global using ScaleformUI;
global using ScaleformUI.Elements;
global using ScaleformUI.Menu;
global using ScaleformUI.PauseMenu;
global using ScaleformUI.Scaleforms;
global using TheLastPlanet.Client.Cache;
global using TheLastPlanet.Client.Core.PlayerChar;
global using TheLastPlanet.Client.Core.Utility;
global using TheLastPlanet.Client.Core.Utility.HUD;
global using TheLastPlanet.Shared;
global using static CitizenFX.Core.Native.API;
using FxEvents.Shared.Snowflakes;
using Logger;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheLastPlanet.Client.AdminAC;
using TheLastPlanet.Client.Core.Ingresso;
using TheLastPlanet.Client.Handlers;

namespace TheLastPlanet.Client
{
    public class Client : BaseScript
    {
        public static Log Logger;
        public static Client Instance { get; protected set; }
        public ExportDictionary GetExports => Exports;
        public CitizenFX.Core.PlayerList GetPlayers => Players;
        public static Configuration Settings = new Configuration();
        public List<PlayerClient> Clients { get; set; }
        public NuiManager NuiManager { get; set; }
        public StateBagsHandler StateBagsHandler { get; set; }
        public StateBag ServerState => GlobalState;
        public Client()
        {
            // we could add the events in a lua config file.. randomize the string at every restart.. 
            EventDispatcher.Initalize("qIFBYn6qv7ZxbGLT7uzpFHa1wPCpmIHbDTWGJ8fy", "QNrAF12UC1qOvnhL6JEShdEdNiCyASUbbNpvyZPG", "Pi5V5nvCki0BcwppyczIfgy3ZZCJPqaYAeQsLZOs");
            Logger = new();
            SnowflakeGenerator.Create(new Random().NextShort(1, 199));
            Instance = this;
            Clients = new();
            NuiManager = new();
            HUD.Init();
            TestClass.Init(); // TO BE REMOVED
            ClientManager.Init();
            DevManager.Init();
            InputHandler.Init();
            PlayerList.FivemPlayerlist.Init();
            InternalGameEvents.Init();
            StateBagsHandler = new StateBagsHandler();
            PlayerStatesHandler.Init();
            VehicleChecker.Init();
            ServerJoining.Init();
            Minimap.Init();
            SetMapZoomDataLevel(0, 2.73f, 0.9f, 0.08f, 0.0f, 0.0f);
            SetMapZoomDataLevel(1, 2.8f, 0.9f, 0.08f, 0.0f, 0.0f);
            SetMapZoomDataLevel(2, 8.0f, 0.9f, 0.08f, 0.0f, 0.0f);
            SetMapZoomDataLevel(3, 11.3f, 0.9f, 0.08f, 0.0f, 0.0f);
            SetMapZoomDataLevel(4, 16f, 0.9f, 0.08f, 0.0f, 0.0f);
            SetMapZoomDataLevel(5, 55f, 0f, 0.1f, 2.0f, 1.0f);
            SetMapZoomDataLevel(6, 450f, 0f, 0f, 0.1f, 0.1f);
            SetMapZoomDataLevel(7, 4.5f, 0f, 0f, 0f, 0f);
            SetMapZoomDataLevel(8, 11f, 0f, 0f, 2.0f, 3.0f);
        }

        public void AddEventHandler(string eventName, Delegate action)
        {
            EventHandlers[eventName] += action;
        }

        public void RemoveEventHandler(string eventName, Delegate action)
        {
            EventHandlers[eventName] -= action;
        }

        public void AddTick(Func<Task> onTick) { Tick += onTick; }

        public void RemoveTick(Func<Task> onTick) { Tick -= onTick; }

        public void RegisterExport(string name, Delegate action)
        {
            GetExports.Add(name, action);
        }

        public void AddCommand(string commandName, InputArgument handler) { API.RegisterCommand(commandName, handler, false); }
    }
}