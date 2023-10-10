using System;
using System.Collections.Generic;
using System.Linq;
using TheLastPlanet.Server.Core.Buckets;
using TheLastPlanet.Server.Core.PlayerChar;

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
                PlayerClient p = Functions.GetClientFromPlayerId(id);
                ServerMode currentMode = p.Status.PlayerStates.Mode;
                if ((int)p.User.group_level < 0) return;

                if (message.StartsWith("/"))
                {
                    string fullCommand = message.Replace("/", "");
                    string[] command = fullCommand.Split(' ');
                    string cmd = command[0];
                    ChatCommand comm = null;
                    if (Commands.Any(x => x.CommandName.ToLower() == cmd.ToLower())) comm = Commands.FirstOrDefault(x => x.CommandName.ToLower() == cmd.ToLower());

                    if (comm is not null)
                    {
                        if (p.User.group_level >= comm.Restriction)
                        {
                            if (currentMode == comm.Modalita || comm.Modalita == ServerMode.UNKNOWN)
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
                        case ServerMode.Roleplay:
                            BucketsHandler.RolePlay.Bucket.TriggerClientEvent("lprp:triggerProximityDisplay", p.Handle, p.User.FullName + ":", message);
                            break;
                        case ServerMode.Lobby:
                            foreach (PlayerClient player in BucketsHandler.Lobby.Bucket.Players)
                            {
                                p.Player.TriggerEvent("chat:addMessage", new
                                {
                                    color = new[] { 255, 255, 255 },
                                    multiline = true,
                                    args = new[] { name, message }
                                });
                            }
                            break;
                        case ServerMode.FreeRoam:
                            foreach (PlayerClient player in BucketsHandler.FreeRoam.Bucket.Players)
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

        public static void chatCommandEntered(Player sender, string fullCommand, string[] command, string cmd, ChatCommand comm, ServerMode mode)
        {
            User user = Functions.GetUserFromPlayerId(sender.Handle);

            if (comm != null)
            {
                if (user.group_level >= comm.Restriction)
                {
                    string txt;
                    if (command.Length > 1)
                        txt = $"Command: /{cmd} invoked by{sender.Name} with text: {fullCommand.Substring(cmd.Length + 1)}, in {mode} mode";
                    else
                        txt = $"Command: /{cmd} invoked by{sender.Name}, in {mode} mode";
                    Server.Logger.Info(txt);
                }
                else
                {
                    user.showNotification("You do not have permission to use this command!");
                    Server.Logger.Warning($"{sender.Name} tried to use the {cmd} command, in {mode}");
                }
            }
            else
            {
                user.showNotification("You entered an invalid command!");
                Server.Logger.Warning($"{sender.Name} entered an invalid command: {cmd}");
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