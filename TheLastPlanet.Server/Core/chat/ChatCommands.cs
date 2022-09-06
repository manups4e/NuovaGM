using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TheLastPlanet.Server.Core.Buckets;
using TheLastPlanet.Server.Core.PlayerChar;
using TheLastPlanet.Server.FREEROAM.Scripts.EventiFreemode;
using TheLastPlanet.Shared;

using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Server.Core
{
    internal static class ChatEvents
    {
        public static void Init()
        {
            Server.Instance.AddCommand("ooc", new Action<PlayerClient, List<string>, string>(Ooc), ModalitaServer.Roleplay, UserGroup.User, new ChatSuggestion("Scrivi in chat ~y~fuori dal personaggio~w~", new SuggestionParam[1] { new("Testo", "Il testo da inserire") }));
            Server.Instance.AddCommand("pol", new Action<PlayerClient, List<string>, string>(Pol), ModalitaServer.Roleplay, UserGroup.User, new ChatSuggestion("Scrivi in chat con i tuoi colleghi ~y~poliziotti~w~", new SuggestionParam[1] { new("Testo", "Il testo da inserire") }));
            Server.Instance.AddCommand("pil", new Action<PlayerClient, List<string>, string>(Pil), ModalitaServer.Roleplay, UserGroup.User, new ChatSuggestion("Scrivi in chat con i tuoi colleghi ~y~piloti~w~", new SuggestionParam[1] { new("Testo", "Il testo da inserire") }));
            Server.Instance.AddCommand("med", new Action<PlayerClient, List<string>, string>(Med), ModalitaServer.Roleplay, UserGroup.User, new ChatSuggestion("Scrivi in chat con i tuoi colleghi ~y~poliziotti~w~", new SuggestionParam[1] { new("Testo", "Il testo da inserire") }));
            Server.Instance.AddCommand("mec", new Action<PlayerClient, List<string>, string>(Mec), ModalitaServer.Roleplay, UserGroup.User, new ChatSuggestion("Scrivi in chat con i tuoi colleghi ~y~poliziotti~w~", new SuggestionParam[1] { new("Testo", "Il testo da inserire") }));
            Server.Instance.AddCommand("me", new Action<PlayerClient, List<string>, string>(Me), ModalitaServer.Roleplay, UserGroup.User, new ChatSuggestion("Descrivi i tuoi stati d'animo e personali", new SuggestionParam[1] { new("Testo", "Il testo da inserire") }));
            Server.Instance.AddCommand("do", new Action<PlayerClient, List<string>, string>(Do), ModalitaServer.Roleplay, UserGroup.User, new ChatSuggestion("Descrivi le tue azioni personali e interpersonali", new SuggestionParam[1] { new("Testo", "Il testo da inserire") }));
            Server.Instance.AddCommand("giveitem", new Action<PlayerClient, List<string>, string>(GiveItem), ModalitaServer.Roleplay, UserGroup.Moderatore, new ChatSuggestion("Dai un oggetto a un player", new SuggestionParam[3] { new("ID Player", "Il Server ID del player"), new("Oggetto", "L'oggetto da dare al player"), new("Quantità", "Quantità dell'oggetto da dare") }));
            Server.Instance.AddCommand("removeitem", new Action<PlayerClient, List<string>, string>(RemoveItem), ModalitaServer.Roleplay, UserGroup.Moderatore, new ChatSuggestion("Togli un oggetto a un player", new SuggestionParam[3] { new("ID Player", "Il Server ID del player"), new("Oggetto", "L'oggetto da togliere al player"), new("Quantità", "Quantità dell'oggetto da togliere") }));
            Server.Instance.AddCommand("giveweapon", new Action<PlayerClient, List<string>, string>(GiveWeapon), ModalitaServer.Roleplay, UserGroup.Moderatore, new ChatSuggestion("Dai un'arma a un player", new SuggestionParam[3] { new("ID Player", "Il Server ID del player"), new("Arma", "L'arma da dare al player [es. weapon_pistol]"), new("Quantità", "Quantità di munizioni da dare") }));
            Server.Instance.AddCommand("removeweapon", new Action<PlayerClient, List<string>, string>(RemoveWeapon), ModalitaServer.Roleplay, UserGroup.Moderatore, new ChatSuggestion("Togli un'arma a un player", new SuggestionParam[2] { new("ID Player", "Il Server ID del player"), new("Arma", "L'arma da togliere al player [es. weapon_pistol]") }));
            Server.Instance.AddCommand("givemoney", new Action<PlayerClient, List<string>, string>(GiveMoney), ModalitaServer.Roleplay, UserGroup.Moderatore, new ChatSuggestion("Dai soldi nel portafoglio ad un player", new SuggestionParam[2] { new("ID Player", "Il Server ID del player"), new("Quantità", "Quanti soldi vuoi dargli?") }));
            Server.Instance.AddCommand("givebank", new Action<PlayerClient, List<string>, string>(GiveBank), ModalitaServer.Roleplay, UserGroup.Moderatore, new ChatSuggestion("Dai soldi in banca ad un player", new SuggestionParam[2] { new("ID Player", "Il Server ID del player"), new("Quantità", "Quanti soldi vuoi dargli?") }));
            Server.Instance.AddCommand("givedirty", new Action<PlayerClient, List<string>, string>(GiveDirty), ModalitaServer.Roleplay, UserGroup.Moderatore, new ChatSuggestion("Dai soldi sporchi ad un player", new SuggestionParam[2] { new("ID Player", "Il Server ID del player"), new("Quantità", "Quanti soldi vuoi dargli?") }));
            Server.Instance.AddCommand("removemoney", new Action<PlayerClient, List<string>, string>(RemoveMoney), ModalitaServer.Roleplay, UserGroup.Moderatore, new ChatSuggestion("Rimuovi soldi nel portafoglio ad un player", new SuggestionParam[2] { new("ID Player", "Il Server ID del player"), new("Quantità", "Quanti soldi vuoi togliere?") }));
            Server.Instance.AddCommand("removebank", new Action<PlayerClient, List<string>, string>(RemoveBank), ModalitaServer.Roleplay, UserGroup.Moderatore, new ChatSuggestion("Rimuovi soldi in banca ad un player", new SuggestionParam[2] { new("ID Player", "Il Server ID del player"), new("Quantità", "Quanti soldi vuoi togliere?") }));
            Server.Instance.AddCommand("removedirty", new Action<PlayerClient, List<string>, string>(RemoveDirty), ModalitaServer.Roleplay, UserGroup.Moderatore, new ChatSuggestion("Rimuovi soldi sporchi ad un player", new SuggestionParam[2] { new("ID Player", "Il Server ID del player"), new("Quantità", "Quanti soldi vuoi togliere?") }));
            Server.Instance.AddCommand("setmoney", new Action<PlayerClient, List<string>, string>(SetFinances), ModalitaServer.Roleplay, UserGroup.Moderatore, new ChatSuggestion("Modifica definitivamente un account monetario del player", new SuggestionParam[3] { new("ID Player", "Il Server ID del player"), new("Account", "cash = soldi, bank = banca, dirty = sporchi"), new("Quantità", "Attenzione, se ho 10 e metto 1, la quantità diventa 1") }));
            Server.Instance.AddCommand("annuncio", new Action<PlayerClient, List<string>, string>(Annuncio), ModalitaServer.UNKNOWN, UserGroup.Moderatore, new ChatSuggestion("Annuncio a tutti i giocatori", new SuggestionParam[1] { new("Annuncio", "Messaggio da far leggere a tutti") }));
            Server.Instance.AddCommand("revive", new Action<PlayerClient, List<string>, string>(Revive), ModalitaServer.UNKNOWN, UserGroup.Moderatore, new ChatSuggestion("Rianima un giocatore", new SuggestionParam[1] { new("ID Player", "[Opzionale] Il Server ID del player, se non inserisci niente rianimi te stesso") }));
            Server.Instance.AddCommand("setgroup", new Action<PlayerClient, List<string>, string>(SetGroup), ModalitaServer.UNKNOWN, UserGroup.Admin, new ChatSuggestion("Cambia gruppo al player", new SuggestionParam[2] { new("ID Player", "Il Server ID del player"), new("Id Gruppo", "0 = User, 1 = Helper, 2 = Moderatore, 3 = Admin, 4 = Founder, 5 = Sviluppatore") }));
            Server.Instance.AddCommand("tp", new Action<PlayerClient, List<string>, string>(Teleport), ModalitaServer.UNKNOWN, UserGroup.Moderatore, new ChatSuggestion("Teletrasportati alle coordinate", new SuggestionParam[3] { new("X", ""), new("Y", ""), new("Z", "") }));
            Server.Instance.AddCommand("suicidati", new Action<PlayerClient, List<string>, string>(Muori), ModalitaServer.Roleplay, UserGroup.Moderatore, new ChatSuggestion("Uccide il tuo personaggio"));
            Server.Instance.AddCommand("car", new Action<PlayerClient, List<string>, string>(SpawnVehicle), ModalitaServer.UNKNOWN, UserGroup.Moderatore, new ChatSuggestion("Spawna un'auto e ti ci porta dentro", new SuggestionParam[1] { new("Modello", "Il modello del veicolo da spawnare") }));
            Server.Instance.AddCommand("dv", new Action<PlayerClient, List<string>, string>(Dv), ModalitaServer.UNKNOWN, UserGroup.Moderatore, new ChatSuggestion("Elimina il veicolo corrente o quello a cui guardi"));
            Server.Instance.AddCommand("salvatutti", new Action<PlayerClient, List<string>, string>(Salvatutti), ModalitaServer.UNKNOWN, UserGroup.Moderatore, new ChatSuggestion("Salva tutti i giocatori subito"));
            Server.Instance.AddCommand("sviluppatore", new Action<PlayerClient, List<string>, string>(Sviluppatore), ModalitaServer.UNKNOWN, UserGroup.Sviluppatore, new ChatSuggestion("Attiva le funzioni dello sviluppatore", new SuggestionParam[1] { new("Accensione", "On/Off") }));
            Server.Instance.AddCommand("setjob", new Action<PlayerClient, List<string>, string>(SetJob), ModalitaServer.UNKNOWN, UserGroup.Moderatore, new ChatSuggestion("Cambia lavoro ad un player", new SuggestionParam[3] { new("ID Player", "Il Server ID del player"), new("Lavoro", "Il lavoro da attivare"), new("Grado", "Il grado lavorativo") }));
            Server.Instance.AddCommand("setgang", new Action<PlayerClient, List<string>, string>(SetGang), ModalitaServer.UNKNOWN, UserGroup.Moderatore, new ChatSuggestion("Cambia gang ad un giocatore", new SuggestionParam[3] { new("ID Player", "Il Server ID del player"), new("Gang", "La gang da settare"), new("Grado", "Il grado della gang") }));
            Server.Instance.AddCommand("setmeteo", new Action<PlayerClient, List<string>, string>(Weather), ModalitaServer.UNKNOWN, UserGroup.Admin, new ChatSuggestion("Cambia il meteo in gioco", new SuggestionParam[1] { new("Meteo", "Inserisci il numero") }));
            Server.Instance.AddCommand("dailicenza", new Action<PlayerClient, List<string>, string>(DaiLicenza), ModalitaServer.UNKNOWN, UserGroup.Moderatore, new ChatSuggestion("Dai una licenza ad un player", new SuggestionParam[2] { new("ID Player", "Il Server ID del player"), new("Licenza", "La licenza da dare") }));
            Server.Instance.AddCommand("rimuovilicenza", new Action<PlayerClient, List<string>, string>(RimuoviLicenza), ModalitaServer.UNKNOWN, UserGroup.Moderatore, new ChatSuggestion("Togli una licenza ad un player", new SuggestionParam[2] { new("ID Player", "Il Server ID del player"), new("Licenza", "La licenza da togliere") }));
            Server.Instance.AddCommand("delchar", new Action<PlayerClient, List<string>, string>(delchar), ModalitaServer.FreeRoam, UserGroup.Moderatore);
            RegisterCommand("status", new Action<int, List<object>, string>((a, b, c) =>
            {
                if (a != 0) return;
                if (b.Count == 0)
                {
                    if (Server.Instance.GetPlayers.Count() > 0)
                    {
                        Server.Logger.Info($"Player totali: {Server.Instance.GetPlayers.Count()}.");
                        foreach (PlayerClient player in Server.Instance.Clients) Server.Logger.Info($"ID:{player.Handle}, {player.Player.Name}, {player.Ped.Position}, Discord:{player.Player.Identifiers["discord"]}, Ping:{player.Player.Ping}, Pianeta:{player.Status.PlayerStates.Modalita}");
                    }
                    else
                        Server.Logger.Warning("Non ci sono player in nel server");
                }
                else
                {
                    switch (b[0] as string)
                    {
                        case "0":
                            if (BucketsHandler.Lobby.GetTotalPlayers() > 0)
                            {
                                Server.Logger.Info($"Lobby -- Player totali: {Server.Instance.GetPlayers.Count()}");
                                foreach (PlayerClient client in BucketsHandler.Lobby.Bucket.Players) Server.Logger.Info($"ID:{client.Handle}, {client.Player.Name}, Discord:{client.Identifiers.Discord}, Ping:{client.Player.Ping}");
                            }
                            else
                                Server.Logger.Warning("Non ci sono player in questo pianeta");
                            break;
                        case "1":
                            if (BucketsHandler.RolePlay.GetTotalPlayers() > 0)
                            {
                                Server.Logger.Info($"Roleplay -- Player totali: {Server.Instance.GetPlayers.Count()}");
                                foreach (PlayerClient client in BucketsHandler.RolePlay.Bucket.Players) Server.Logger.Info($"ID:{client.Handle}, {client.Player.Name}, Discord:{client.Identifiers.Discord}, Ping:{client.Player.Ping}");
                            }
                            else
                                Server.Logger.Warning("Non ci sono player in questo pianeta");
                            break;
                        case "2":
                            if (BucketsHandler.Minigiochi.GetTotalPlayers() > 0)
                            {
                                Server.Logger.Info($"Minigiochi -- Player totali: {Server.Instance.GetPlayers.Count()}");
                                foreach (PlayerClient client in BucketsHandler.Minigiochi.Bucket.Players) Server.Logger.Info($"ID:{client.Handle}, {client.Player.Name}, Discord:{client.Identifiers.Discord}, Ping:{client.Player.Ping}");
                            }
                            else
                                Server.Logger.Warning("Non ci sono player in questo pianeta");
                            break;
                        case "3":
                            if (BucketsHandler.Gare.GetTotalPlayers() > 0)
                            {
                                Server.Logger.Info($"Gare -- Player totali: {Server.Instance.GetPlayers.Count()}");
                                foreach (PlayerClient client in BucketsHandler.Gare.Bucket.Players) Server.Logger.Info($"ID:{client.Handle}, {client.Player.Name}, Discord:{client.Identifiers.Discord}, Ping:{client.Player.Ping}");
                            }
                            else
                                Server.Logger.Warning("Non ci sono player in questo pianeta");
                            break;
                        case "4":
                            if (BucketsHandler.Negozio.GetTotalPlayers() > 0)
                            {
                                Server.Logger.Info($"Negozio -- Player totali: {Server.Instance.GetPlayers.Count()}");
                                foreach (PlayerClient client in BucketsHandler.Negozio.Bucket.Players) Server.Logger.Info($"ID:{client.Handle}, {client.Player.Name}, Discord:{client.Identifiers.Discord}, Ping:{client.Player.Ping}");
                            }
                            else
                                Server.Logger.Warning("Non ci sono player in questo pianeta");
                            break;
                        case "5":
                            if (BucketsHandler.FreeRoam.GetTotalPlayers() > 0)
                            {
                                Server.Logger.Info($"FreeRoam -- Player totali: {Server.Instance.GetPlayers.Count()}");
                                foreach (PlayerClient client in BucketsHandler.FreeRoam.Bucket.Players) Server.Logger.Info($"ID:{client.Handle}, {client.Player.Name}, Discord:{client.Identifiers.Discord}, Ping:{client.Player.Ping}");
                            }
                            else
                                Server.Logger.Warning("Non ci sono player in questo pianeta");
                            break;
                        default:
                            Server.Logger.Error("Pianeta sconosciuto.");
                            break;

                    }
                }
            }), true);

            //			Server.Instance.AddCommand("nome comando", new Action<PlayerClient, List<string>, string>(funzione comando), false, new ChatSuggestion("", new SuggestionParam[] { new SuggestionParam() }));
        }

        private static void delchar(PlayerClient sender, List<string> args, string rawCommand)
        {
            string bytes = GetResourceKvpString($"freeroam:player_{sender.User.Identifiers.Discord}:char_model");
            DeleteResourceKvpNoSync($"freeroam:player_{sender.User.Identifiers.Discord}:char_model");
            Server.Logger.Warning($"{bytes.StringToBytes().Length} bytes cancellati con successo");
        }

        // GESTIONE CHAT
        public static void Ooc(PlayerClient sender, List<string> args, string rawCommand)
        {
            try
            {
                if (args.Count <= 0) return;
                string noCom = rawCommand.Substring(5);
                string filtro = $"({Server.Impostazioni.Main.BadWords.Keys.Aggregate((i, j) => i + "|" + j)})";
                Regex filter = new(filtro, RegexOptions.IgnoreCase);
                MatchCollection matches = filter.Matches(noCom);
                noCom = matches.Cast<Match>().Aggregate(noCom, (current, m) => filter.Replace(current, Server.Impostazioni.Main.BadWords[m.Value], 1));
                BaseScript.TriggerClientEvent("chat:addMessage", new { color = new[] { 0, 255, 153 }, multiline = true, args = new[] { "[FUORI RP] | " + sender.Player.Name, noCom } });
            }
            catch (Exception e)
            {
                Server.Logger.Error(e.ToString());
            }
        }

        public static void Pol(PlayerClient sender, List<string> args, string rawCommand)
        {
            User user = Funzioni.GetUserFromPlayerId(sender.Handle);
            if (user.CurrentChar.Job.Name.ToLower() == "polizia")
                Server.Instance.Clients.Where(x => x.User.CurrentChar.Job.Name.ToLower() == "polizia").ToList().ForEach(x => x.Player.TriggerEvent("chat:addMessage", new { color = new[] { 244, 65, 125 }, multiline = true, args = new[] { "[POLIZIA] | " + user.FullName, rawCommand.Substring(5) } }));
            else
                user.showNotification("Non puoi usare questo comando!");
        }

        public static void Pil(PlayerClient sender, List<string> args, string rawCommand)
        {
            User user = Funzioni.GetUserFromPlayerId(sender.Handle);
            if (user.CurrentChar.Job.Name.ToLower() == "pilota")
                Server.Instance.Clients.Where(x => x.User.CurrentChar.Job.Name.ToLower() == "pilota").ToList().ForEach(x => x.Player.TriggerEvent("chat:addMessage", new { color = new[] { 244, 223, 66 }, multiline = true, args = new[] { "[PILOTI] | " + user.FullName, rawCommand.Substring(5) } }));
            else
                user.showNotification("Non puoi usare questo comando!");
        }

        public static void Med(PlayerClient sender, List<string> args, string rawCommand)
        {
            User user = Funzioni.GetUserFromPlayerId(sender.Handle);
            if (user.CurrentChar.Job.Name.ToLower() == "medico")
                Server.Instance.Clients.Where(x => x.User.CurrentChar.Job.Name.ToLower() == "medico").ToList().ForEach(x => x.Player.TriggerEvent("chat:addMessage", new { color = new[] { 88, 154, 202 }, multiline = true, args = new[] { "[MEDICI] | " + user.FullName, rawCommand.Substring(5) } }));
            else
                user.showNotification("Non puoi usare questo comando!");
        }

        public static void Mec(PlayerClient sender, List<string> args, string rawCommand)
        {
            User user = Funzioni.GetUserFromPlayerId(sender.Handle);
            if (user.CurrentChar.Job.Name.ToLower() == "meccanico")
                Server.Instance.Clients.Where(x => x.User.CurrentChar.Job.Name.ToLower() == "meccanico").ToList().ForEach(x => x.Player.TriggerEvent("chat:addMessage", new { color = new[] { 102, 102, 255 }, multiline = true, args = new[] { "[MECCANICI] | " + user.FullName, rawCommand.Substring(5) } }));
            else
                user.showNotification("Non puoi usare questo comando!");
        }

        public static void Me(PlayerClient sender, List<string> args, string rawCommand)
        {
            BucketsHandler.RolePlay.Bucket.TriggerClientEvent("lprp:triggerProximityDisplay", sender.Handle, "[ME]: ", rawCommand.Substring(4), 0, 255, 153);
        }

        public static void Do(PlayerClient sender, List<string> args, string rawCommand)
        {
            BucketsHandler.RolePlay.Bucket.TriggerClientEvent("lprp:triggerProximityDisplay", sender.Handle, "[DO]: ", rawCommand.Substring(4), 0, 255, 153);
        }

        // FINE CHAT
        // GESTIONE INVENTARIO
        public static void GiveItem(PlayerClient sender, List<string> args, string rawCommand)
        {
            try
            {
                var client = Funzioni.GetClientFromPlayerId(int.Parse(args[0]));
                if (client != null)
                {
                    User player = client.User;
                    string item = "" + args[1];
                    player.addInventoryItem(item, Convert.ToInt32(args[2]), ConfigShared.SharedConfig.Main.Generici.ItemList[item].peso);
                }
                else
                {
                    sender.Player.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO giveitem] = ", "Il player con ID \"" + args[0] + "\" non è online!" }, color = new[] { 255, 0, 0 } });
                }
            }
            catch
            {
                sender.Player.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO giveitem] = ", "Errore nei parametri!" }, color = new[] { 255, 0, 0 } });
            }
        }

        public static void RemoveItem(PlayerClient sender, List<string> args, string rawCommand)
        {
            try
            {
                var client = Funzioni.GetClientFromPlayerId(int.Parse(args[0]));
                if (client != null)
                {
                    User player = client.User;
                    string item = "" + args[1];
                    player.removeInventoryItem(item, Convert.ToInt32(args[2]));
                }
                else
                {
                    sender.Player.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO removeitem] = ", "Il player con ID" + args[0] + " non è online!" }, color = new[] { 255, 0, 0 } });
                }
            }
            catch
            {
                sender.Player.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO removeitem] = ", "Errore nei parametri!" }, color = new[] { 255, 0, 0 } });
            }
        }

        public static void GiveWeapon(PlayerClient sender, List<string> args, string rawCommand)
        {
            try
            {
                var client = Funzioni.GetClientFromPlayerId(int.Parse(args[0]));
                if (client != null)
                {
                    User player = client.User;
                    player.addWeapon(args[1].ToUpper(), Convert.ToInt32(args[2]));
                }
                else
                {
                    sender.Player.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO giveweapon] = ", "Il player con ID" + args[0] + " non è online!" }, color = new[] { 255, 0, 0 } });
                }
            }
            catch
            {
                sender.Player.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO giveweapon] = ", "Errore nei parametri!" }, color = new[] { 255, 0, 0 } });
            }
        }

        public static void RemoveWeapon(PlayerClient sender, List<string> args, string rawCommand)
        {
            try
            {
                var client = Funzioni.GetClientFromPlayerId(int.Parse(args[0]));
                if (client != null)
                {
                    User player = client.User;
                    player.removeWeapon(args[1].ToUpper());
                }
                else
                {
                    sender.Player.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO removeweapon] = ", "Il player con ID" + args[0] + " non è online!" }, color = new[] { 255, 0, 0 } });
                }
            }
            catch
            {
                sender.Player.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO removeweapon] = ", "Errore nei parametri!" }, color = new[] { 255, 0, 0 } });
            }
        }

        // FINE GESTIONE INVENTARIO
        // GESTIONE DELLE FINANZE
        public static void GiveMoney(PlayerClient sender, List<string> args, string rawCommand)
        {
            var client = Funzioni.GetClientFromPlayerId(int.Parse(args[0]));
            if (client != null)
                client.User.Money += Convert.ToInt32(args[1]);
            else
                sender.Player.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO givemoney] = ", "Il player con ID" + args[0] + " non è online!" }, color = new[] { 255, 0, 0 } });
        }

        public static void GiveBank(PlayerClient sender, List<string> args, string rawCommand)
        {
            var client = Funzioni.GetClientFromPlayerId(int.Parse(args[0]));
            if (client != null)
                client.User.Bank += Convert.ToInt32(args[1]);
            else
                sender.Player.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO givebank] = ", "Il player con ID" + args[0] + " non è online!" }, color = new[] { 255, 0, 0 } });
        }

        public static void GiveDirty(PlayerClient sender, List<string> args, string rawCommand)
        {
            var client = Funzioni.GetClientFromPlayerId(int.Parse(args[0]));
            if (client != null)
                client.User.DirtCash += Convert.ToInt32(args[1]);
            else
                sender.Player.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO givedirty] = ", "Il player con ID" + args[0] + " non è online!" }, color = new[] { 255, 0, 0 } });
        }

        public static void RemoveMoney(PlayerClient sender, List<string> args, string rawCommand)
        {
            var client = Funzioni.GetClientFromPlayerId(int.Parse(args[0]));
            if (client != null)
                client.User.Money -= Convert.ToInt32(args[1]);
            else
                sender.Player.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO givedirty] = ", "Il player con ID" + args[0] + " non è online!" }, color = new[] { 255, 0, 0 } });
        }

        public static void RemoveBank(PlayerClient sender, List<string> args, string rawCommand)
        {
            var client = Funzioni.GetClientFromPlayerId(int.Parse(args[0]));
            if (client != null)
                client.User.Bank -= Convert.ToInt32(args[1]);
            else
                sender.Player.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO givedirty] = ", "Il player con ID" + args[0] + " non è online!" }, color = new[] { 255, 0, 0 } });
        }

        public static void RemoveDirty(PlayerClient sender, List<string> args, string rawCommand)
        {
            var client = Funzioni.GetClientFromPlayerId(int.Parse(args[0]));
            if (client != null)
                client.User.DirtCash -= Convert.ToInt32(args[1]);
            else
                sender.Player.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO givedirty] = ", "Il player con ID" + args[0] + " non è online!" }, color = new[] { 255, 0, 0 } });
        }

        public static void SetFinances(PlayerClient sender, List<string> args, string rawCommand)
        {
            var client = Funzioni.GetClientFromPlayerId(int.Parse(args[0]));
            if (client != null)
            {
                var player = client.User;
                if (args[1] == "soldi")
                {
                    player.Money -= player.Money;
                    player.Money += Convert.ToInt32(args[2]);
                }
                else if (args[1] == "banca")
                {
                    player.Bank -= player.Bank;
                    player.Bank += Convert.ToInt32(args[2]);
                }
                else if (args[1] == "sporchi")
                {
                    player.DirtCash -= player.DirtCash;
                    player.DirtCash += Convert.ToInt32(args[2]);
                }
                else
                {
                    sender.Player.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO setmoney] = ", "L'account monetario '" + args[1] + "' non esiste!" }, color = new[] { 255, 0, 0 } });
                }
            }
            else
            {
                sender.Player.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO setmoney] = ", "Il player con ID" + args[0] + " non è online!" }, color = new[] { 255, 0, 0 } });
            }
        }

        // FINE GESTIONE FINANZE
        // ANNUNCIO Players
        public static void Annuncio(PlayerClient sender, List<string> args, string rawCommand)
        {
            BaseScript.TriggerClientEvent("lprp:announce", rawCommand.Replace("annuncio", string.Empty));
        }
        // FINE ANNUNCIO

        // REVIVE
        public static void Revive(PlayerClient sender, List<string> args, string rawCommand)
        {
            DateTime now = DateTime.Now;

            if (args != null && args.Count > 0)
            {
                if (GetPlayerName(args[0]) != ".")
                {
                    Player p = Server.Instance.GetPlayers[Convert.ToInt32(args[0])];
                    Server.Logger.Info("Comandi: " + sender.Player.Name + " ha usato il comando revive su " + GetPlayerName(args[0]));
                    p.TriggerSubsystemEvent("lprp:reviveChar");
                }
            }
            else
            {
                sender.TriggerSubsystemEvent("lprp:reviveChar");
            }
        }
        // FINE REVIVE

        // SETGROUP
        public static async void SetGroup(PlayerClient sender, List<string> args, string rawCommand)
        {
            await BaseScript.Delay(0);
            DateTime now = DateTime.Now;

            if (Convert.ToInt32(args[0]) > 0)
            {
                string group = "normal";
                int group_level = 0;
                Player ricevitore = Funzioni.GetPlayerFromId(args[0]);
                User user = Funzioni.GetUserFromPlayerId(ricevitore.Handle);

                if (ricevitore.Name.Length > 0)
                {
                    Server.Logger.Info("Comandi: " + sender.Player.Name + " ha usato il comando setgroup su " + ricevitore.Name);
                    BaseScript.TriggerEvent("lprp:serverlog", now.ToString("dd/MM/yyyy, HH:mm:ss") + " -- Comandi: " + sender.Player.Name + " ha usato il comando setgroup su " + ricevitore.Name);

                    if (args[1] == "normal")
                    {
                        group = "normal";
                        group_level = (int)UserGroup.User;
                    }
                    else if (args[1] == "helper")
                    {
                        group = "helper";
                        group_level = (int)UserGroup.Helper;
                    }
                    else if (args[1] == "mod")
                    {
                        group = "moderatore";
                        group_level = (int)UserGroup.Moderatore;
                    }
                    else if (args[1] == "admin")
                    {
                        group = "admin";
                        group_level = (int)UserGroup.Admin;
                    }
                    else if (args[1] == "founder")
                    {
                        group = "founder";
                        group_level = (int)UserGroup.Founder;
                    }
                    else if (args[1] == "dev")
                    {
                        group = "dev";
                        group_level = (int)UserGroup.Sviluppatore;
                    }

                    await Server.Instance.Execute("UPDATE `users` SET `group` = @gruppo,  `group_level` = @groupL WHERE `discord` = @disc", new { gruppo = group, groupL = group_level, disc = user.Identifiers.Discord });
                    user.group = group;
                    user.group_level = (UserGroup)group_level;
                    Server.Logger.Info($"Il player {ricevitore.Name} e' stato settato come gruppo {group}");
                    BaseScript.TriggerEvent("lprp:serverLog", now.ToString("dd/MM/yyyy, HH:mm:ss") + $" --  Il player {ricevitore.Name} e' stato settato come gruppo {group}");
                }
                else
                {
                    Server.Logger.Error("Il player con ID" + args[0] + " non è online!");
                }
            }
            else
            {
                Server.Logger.Error("errore nel comando setgroup..riprova");
            }
        }
        // FINE SETGROUP

        public static void Teleport(PlayerClient sender, List<string> args, string rawCommand)
        {
            if (float.TryParse(args[0].Replace("f", "").Replace(",", ""), out float x) && float.TryParse(args[1].Replace("f", "").Replace(",", ""), out float y) && float.TryParse(args[2].Replace("f", ""), out float z))
                sender.TriggerSubsystemEvent("lprp:teleportCoords", new Position(x, y, z));
            else
                sender.Player.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO tp] = ", "Errore coordinate non valide, riprova!" }, color = new[] { 255, 0, 0 } });
        }

        public static void Muori(PlayerClient sender, List<string> args, string rawCommand)
        {
            sender.TriggerSubsystemEvent("lprp:death");
        }

        public static void SpawnVehicle(PlayerClient sender, List<string> args, string rawCommand)
        {
            sender.TriggerSubsystemEvent("lprp:spawnVehicle", args[0]);
        }

        public static void Dv(PlayerClient sender, List<string> args, string rawCommand)
        {
            sender.TriggerSubsystemEvent("lprp:deleteVehicle");
        }

        public static void Delgun(PlayerClient sender, List<string> args, string rawCommand)
        {
            sender.TriggerSubsystemEvent("lprp:ObjectDeleteGun", args[0]);
        }

        private static async void Salvatutti(PlayerClient sender, List<string> args, string rawCommand)
        {
            try
            {
                var now = DateTime.Now;

                foreach (var player in Server.Instance.Clients)
                {
                    int freer = 0;
                    int rp = 0;
                    if (player.Status.PlayerStates.Spawned)
                    {
                        switch (player.Status.PlayerStates.Modalita)
                        {
                            case ModalitaServer.Roleplay:
                                player.TriggerSubsystemEvent("lprp:mostrasalvataggio");
                                BucketsHandler.RolePlay.SalvaPersonaggioRoleplay(player);
                                Server.Logger.Info($"Salvato personaggio: '{player.User.FullName}' appartenente a '{player.Player.Name}' - {player.User.Identifiers.Discord}");
                                await Task.FromResult(0);
                                rp++;
                                break;
                            case ModalitaServer.FreeRoam:
                                player.TriggerSubsystemEvent("tlg:freeroam:showLoading", 4, "Sincronizzazione", 5000);
                                EventiFreeRoam.SalvaPersonaggio(player);
                                Server.Logger.Info($"Salvato personaggio freeroam appartenente a '{player.Player.Name}' - {player.User.Identifiers.Discord}");
                                freer++;
                                break;
                        }
                    }
                    Server.Logger.Info($"Salvati in totale {freer} giocatori FreeRoam e {rp} giocatori RP");
                }
                //BaseScript.TriggerClientEvent("lprp:aggiornaPlayers", ServerSession.PlayerList.ToJson());
            }
            catch (Exception ex)
            {
                Server.Logger.Fatal("" + ex);
            }
        }

        public static void Sviluppatore(PlayerClient sender, List<string> args, string rawCommand)
        {
            if (args.Count == 0 || string.IsNullOrWhiteSpace(args[0]))
            {
                sender.Player.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO sviluppatore] = ", "Errore argomento non valido, riprova!" }, color = new[] { 255, 0, 0 } });
                return;
            }
            sender.TriggerSubsystemEvent("lprp:sviluppatoreOn", args[0].ToLower() == "on");
        }

        public static void SetJob(PlayerClient sender, List<string> args, string rawCommand)
        {
            PlayerClient p = Funzioni.GetClientFromPlayerId(args[0]);

            if (p != null)
            {
                if (p.Status.PlayerStates.Spawned)
                    p.User.SetJob(args[1], Convert.ToInt32(args[2]));
                else
                    sender.Player.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO setjob] = ", "Errore il player non ha selezionato un personaggio, riprova!" }, color = new[] { 255, 0, 0 } });
            }
            else
            {
                sender.Player.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO setjob] = ", "Errore id player non trovato, riprova!" }, color = new[] { 255, 0, 0 } });
            }
        }

        public static void SetGang(PlayerClient sender, List<string> args, string rawCommand)
        {
            PlayerClient p = Funzioni.GetClientFromPlayerId(args[0]);

            if (p != null)
            {
                if (p.Status.PlayerStates.Spawned)
                    p.User.SetGang(args[1], Convert.ToInt32(args[2]));
                else
                    sender.Player.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO setgang] = ", "Errore il player non ha selezionato un personaggio, riprova!" }, color = new[] { 255, 0, 0 } });
            }
            else
            {
                sender.Player.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO setgang] = ", "Errore id player non trovato, riprova!" }, color = new[] { 255, 0, 0 } });
            }
        }

        public static void Weather(PlayerClient sender, List<string> args, string rawCommand)
        {
            if (sender.Handle == 0)
            {
                if (args.Count > 1 || Convert.ToInt32(args[0]) > 14 || !(args[0] as string).All(o => char.IsDigit(o)))
                {
                    Server.Logger.Error("/weather <weathertype>\nCurrent Weather: " + TimeWeather.MeteoServer.Meteo.CurrentWeather + "\nErrore weather, argomenti disponibili: 0 = EXTRASUNNY, 1 =  CLEAR, 2 = CLOUDS, 3 = SMOG, 4 = FOGGY, 5 = OVERCAST, 6 = RAIN, 7 = THUNDERSTORM, 8 = CLEARING, 9 = NEUTRAL, 10 = SNOW, 11 =  BLIZZARD, 12 = SNOWLIGHT, 13 = XMAS, 14 = HALLOWEEN");

                    return;
                }
                else
                {
                    TimeWeather.MeteoServer.Meteo.CurrentWeather = Convert.ToInt32(args[0]);
                    Server.Logger.Debug(TimeWeather.MeteoServer.Meteo.CurrentWeather + "");
                    TimeWeather.MeteoServer.Meteo.WeatherTimer = ConfigShared.SharedConfig.Main.Meteo.ss_weather_timer * 60;
                    TimeWeather.MeteoServer.CambiaMeteo(false);
                }
            }
            else
            {
                if (args.Count < 1 || Convert.ToInt32(args[0]) > 14 || !(args[0] as string).All(o => char.IsDigit(o)))
                {
                    sender.Player.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO weather] = ", "Errore weather, argomenti disponibili: ~n~0 = EXTRASUNNY, 1 = CLEAR, 2 = CLOUDS, 3 = SMOG, 4 = FOGGY, 5 = OVERCAST 6 = RAIN, 7 = THUNDERSTORM, 8 = CLEARING, ~n~9 = NEUTRAL, 10 = SNOW, 11 = BLIZZARD, 12 = SNOWLIGHT, 13 = XMAS, 14 = HALLOWEEN!" }, color = new[] { 255, 0, 0 } });
                }
                else
                {
                    TimeWeather.MeteoServer.Meteo.CurrentWeather = Convert.ToInt32(args[0]);
                    TimeWeather.MeteoServer.Meteo.WeatherTimer = ConfigShared.SharedConfig.Main.Meteo.ss_weather_timer * 60;
                    TimeWeather.MeteoServer.CambiaMeteo(false);
                    string meteo = "";
                    int a = Convert.ToInt32(args[0]);

                    switch (a)
                    {
                        case 0:
                            meteo = "Super Soleggiato";

                            break;
                        case 1:
                            meteo = "Cielo Sgombro";

                            break;
                        case 2:
                            meteo = "Nuvoloso";

                            break;
                        case 3:
                            meteo = "Smog";

                            break;
                        case 4:
                            meteo = "Nebbioso";

                            break;
                        case 5:
                            meteo = "Nuvoloso";

                            break;
                        case 6:
                            meteo = "Piovoso";

                            break;
                        case 7:
                            meteo = "Tempestoso";

                            break;
                        case 8:
                            meteo = "Sereno";

                            break;
                        case 9:
                            meteo = "Neutrale";

                            break;
                        case 10:
                            meteo = "Nevoso";

                            break;
                        case 11:
                            meteo = "Bufera di neve";

                            break;
                        case 12:
                            meteo = "Nevoso con Nebbia";

                            break;
                        case 13:
                            meteo = "Natalizio";

                            break;
                        case 14:
                            meteo = "Halloween";

                            break;
                        default:
                            meteo = "Sconosciuto?";

                            break;
                    }

                    sender.TriggerSubsystemEvent("tlg:ShowNotification", "Meteo modificato in ~b~" + meteo + "~w~");
                }
            }
        }

        private static void DaiLicenza(PlayerClient sender, List<string> args, string rawCommand)
        {
            if (sender.Handle == 0)
            {
                Server.Logger.Error($"Comando permesso solo in game");
            }
            else
            {
                if (!string.IsNullOrEmpty(args[0] as string))
                {
                    if (!string.IsNullOrEmpty(args[1] as string))
                    {
                        User pers = Funzioni.GetUserFromPlayerId(args[0]);
                        pers.giveLicense(args[1], GetPlayerName("" + sender));
                    }
                    else
                    {
                        sender.TriggerSubsystemEvent("tlg:ShowNotification", "Nessuna licenza specificata!!");
                    }
                }
                else
                {
                    sender.TriggerSubsystemEvent("tlg:ShowNotification", "Nessun id specificato!!");
                }
            }
        }

        private static void RimuoviLicenza(PlayerClient sender, List<string> args, string rawCommand)
        {
            if (sender.Handle == 0)
            {
                Server.Logger.Error($"Comando permesso solo in game");
            }
            else
            {
                if (!string.IsNullOrEmpty(args[0] as string))
                {
                    if (!string.IsNullOrEmpty(args[1] as string))
                    {
                        User pers = Funzioni.GetUserFromPlayerId(args[0]);
                        pers.removeLicense(args[1]);
                    }
                    else
                    {
                        sender.TriggerSubsystemEvent("tlg:ShowNotification", "Nessuna licenza specificata!!");
                    }
                }
                else
                {
                    sender.TriggerSubsystemEvent("tlg:ShowNotification", "Nessun id specificato!!");
                }
            }
        }
    }
}