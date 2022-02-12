using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using TheLastPlanet.Server.Core.Buckets;
using TheLastPlanet.Server.Core.PlayerChar;
using TheLastPlanet.Shared;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Server.Core
{
    internal static class ChatServer
    {
        public static List<ChatCommand> Commands = new List<ChatCommand>();
        public static List<ChatSuggestion> Suggestions = new List<ChatSuggestion>();

        public static void Init()
        {
            Server.Instance.AddEventHandler("chatMessage", new Action<int, string, string>(chatMessage));
            //Server.Instance.AddEventHandler("consoleCommand", new Action<string, string>(ConsoleCommand));
            Server.Instance.AddEventHandler("lprp:chat:commands", new Action<Player>(SendComms));
        }

        public static void chatMessage(int id, string name, string message)
        {
            try
            {
                var p = Funzioni.GetClientFromPlayerId(id);
                User user = p.User;
                var currentMode = user.Status.PlayerStates.Modalita;
                if ((int)user.group_level < 0) return;

                if (message.StartsWith("/"))
                {
                    string fullCommand = message.Replace("/", "");
                    string[] command = fullCommand.Split(' ');
                    string cmd = command[0];
                    ChatCommand comm = null;
                    if (Commands.Any(x => x.CommandName.ToLower() == cmd.ToLower())) comm = Commands.FirstOrDefault(x => x.CommandName.ToLower() == cmd.ToLower());

                    if (comm is not null)
                    {
                        if (user.group_level >= comm.Restriction)
                        {
                            if (currentMode == comm.Modalita || comm.Modalita == ModalitaServer.UNKNOWN)
                            {
                                comm.Source = p.Player;
                                comm.rawCommand = message;
                                if (command.Length > 1)
                                    comm.Args = command.Skip(1).ToList();
                                else
                                    comm.Args = new List<string>();
                                comm.Action.DynamicInvoke(p, comm.Args, comm.rawCommand);
                            }
                        }
                    }
                    chatCommandEntered(p.Player, fullCommand, command, cmd, comm, currentMode);
                }
                else
                {
                    switch (currentMode)
                    {
                        case ModalitaServer.Roleplay:
                            BucketsHandler.RolePlay.Bucket.TriggerClientEvent("lprp:triggerProximityDisplay", p.Handle, user.FullName + ":", message);
                            break;
                        case ModalitaServer.Lobby:
                            foreach (var player in BucketsHandler.Lobby.Bucket.Players)
                            {
                                p.Player.TriggerEvent("chat:addMessage", new
                                {
                                    color = new[] { 255, 255, 255 },
                                    multiline = true,
                                    args = new[] { name, message }
                                });
                            }
                            break;
                        case ModalitaServer.FreeRoam:
                            foreach (var player in BucketsHandler.FreeRoam.Bucket.Players)
                            {
                                p.Player.TriggerEvent("chat:addMessage", new
                                {
                                    color = new[] { 255, 255, 255 },
                                    multiline = true,
                                    args = new[] { name, message }
                                });
                            }
                            break;
                    }
                    //BaseScript.TriggerClientEvent("lprp:triggerProximityDisplay", Convert.ToInt32(p.Handle), /*user.FullName + ":",*/ message);
                }
                CancelEvent();
            }
            catch (Exception e)
            {
                Server.Logger.Error(e.ToString());
            }
        }

        //private static void ConsoleCommand(string name, string command) { }

        public static void chatCommandEntered(Player sender, string fullCommand, string[] command, string cmd, ChatCommand comm, ModalitaServer mode)
        {
            User user = Funzioni.GetUserFromPlayerId(sender.Handle);

            if (comm != null)
            {
                if (user.group_level >= comm.Restriction)
                {
                    string txt;
                    if (command.Length > 1)
                        txt = $"Comando: /{cmd} invocato da {sender.Name} con testo: {fullCommand.Substring(cmd.Length + 1)}, in modalità {mode}";
                    else
                        txt = $"Comando: /{cmd} invocato da {sender.Name}, in modalità {mode}";
                    Server.Logger.Info(txt);
                }
                else
                {
                    user.showNotification("Non hai i permessi per usare questo comando!");
                    Server.Logger.Warning($"{sender.Name} ha provato a usare il comando {cmd}, in modalità {mode}");
                }
            }
            else
            {
                user.showNotification("Hai inserito un comando non valido!");
                Server.Logger.Warning($"{sender.Name} ha inserito un comando non valido: {cmd}");
            }
        }

        private static void SendComms([FromSource] Player p)
        {
            List<object> suggestions = new List<object>();

            foreach (ChatSuggestion sug in Suggestions)
            {
                List<object> paramss = new List<object>();
                foreach (SuggestionParam par in sug.@params) paramss.Add(new { par.name, par.help });
                suggestions.Add(new { sug.name, sug.help, @params = paramss });
            }

            p.TriggerEvent("chat:addSuggestions", suggestions);
        }
    }
}