﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Veicoli;


namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Lavori.Whitelistati.Polizia
{
    public static class MenuPolizia
    {
        #region MenuSpogliatoio

        public static async void CloakRoomMenu()
        {
            AbitiLav PilotaMaschio = new() { Abiti = new ComponentDrawables(-1, 0, -1, 96, 41, -1, 24, 40, 15, 0, 0, 54), TextureVestiti = new ComponentDrawables(-1, 0, -1, 0, 0, -1, 0, 0, 0, 0, 0, 0), Accessori = new PropIndices(47, -1, -1, -1, -1, -1, -1, -1, -1), TexturesAccessori = new PropIndices(0, -1, -1, -1, -1, -1, -1, -1, -1) };
            AbitiLav PilotaFemmina = new() { Abiti = new ComponentDrawables(-1, 0, -1, 25, 0, -1, 24, 0, 13, 0, 0, 1), TextureVestiti = new ComponentDrawables(-1, 0, -1, 0, 1, -1, 0, 0, 0, 0, 0, 4), Accessori = new PropIndices(-1, -1, -1, -1, -1, -1, -1, -1, -1), TexturesAccessori = new PropIndices(-1, -1, -1, -1, -1, -1, -1, -1, -1) };
            UIMenu Spogliatoio = new UIMenu("Spogliatoio Polizia", "Cambiati ed entra/esci dal servizio", PointF.Empty, "thelastgalaxy", "bannerbackground", false, true);

            UIMenuItem Uniforme = new UIMenuItem("");
            UIMenuItem Giubbotto = new UIMenuItem("");
            UIMenuItem Pilota = new UIMenuItem("");

            switch (Cache.PlayerCache.MyPlayer.Status.RolePlayStates.OnDuty)
            {
                case false when !PoliziaMainClient.InServizioDaPilota:
                    Uniforme = new UIMenuItem("Indossa l'uniforme", "Se ti cambi entri automaticamente in servizio!");
                    Pilota = new UIMenuItem("Indossa la tuta da Pilota", "Oggi lo piloti tu l'elicottero della liberta!");

                    break;
                case false when PoliziaMainClient.InServizioDaPilota:
                    Uniforme = new UIMenuItem("Indossa l'uniforme", "Se ti cambi entri automaticamente in servizio!");
                    Pilota = new UIMenuItem("Rimuovi la tuta da Pilota", "Se ti cambi esci automaticamente dal servizio e le armi prese alla polizia verranno restituite!");

                    break;
                case true when !PoliziaMainClient.InServizioDaPilota:
                    Uniforme = new UIMenuItem("Rimuovi l'uniforme", "Se ti cambi esci automaticamente dal servizio e le armi prese alla polizia verranno restituite!");
                    Pilota = new UIMenuItem("Indossa la tuta da Pilota", "Oggi lo piloti tu l'elicottero della liberta!");

                    break;
                default:
                    {
                        if (Cache.PlayerCache.MyPlayer.Status.RolePlayStates.OnDuty || PoliziaMainClient.InServizioDaPilota)
                        {
                            Giubbotto = Cache.PlayerCache.MyPlayer.Ped.Armor < 1 ? new UIMenuItem("Indossa il Giubbotto Anti-Proiettile", "Potrebbe salvarti la vita") : new UIMenuItem("Rimuovi il Giubbotto Anti-Proiettile", "Speriamo sia stato utile");
                            Spogliatoio.AddItem(Giubbotto);
                        }

                        break;
                    }
            }

            Spogliatoio.AddItem(Uniforme);
            Spogliatoio.AddItem(Pilota);
            Spogliatoio.OnItemSelect += async (menu, item, index) =>
            {
                Screen.Fading.FadeOut(800);
                await BaseScript.Delay(1000);
                MenuHandler.CloseAndClearHistory();
                NetworkFadeOutEntity(PlayerPedId(), true, false);

                if (item == Uniforme)
                {
                    if (!Cache.PlayerCache.MyPlayer.Status.RolePlayStates.OnDuty)
                    {
                        foreach (KeyValuePair<string, JobGrade> Grado in Client.Impostazioni.RolePlay.Jobs.Police.Grades.Where(Grado => Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Name == "Polizia").Where(Grado => Grado.Value.Id == Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Grade))
                            switch (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.Sex)
                            {
                                case "Maschio":
                                    CambiaVestito(Grado.Value.Vestiti.Maschio);

                                    break;
                                case "Femmina":
                                    CambiaVestito(Grado.Value.Vestiti.Femmina);

                                    break;
                            }

                        Cache.PlayerCache.MyPlayer.Status.RolePlayStates.OnDuty = true;
                    }
                    else
                    {
                        Funzioni.UpdateDress(PlayerPedId(), Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing);
                        Cache.PlayerCache.MyPlayer.Status.RolePlayStates.OnDuty = false;
                    }
                }
                else if (item == Pilota)
                {
                    switch (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.Sex)
                    {
                        case "Maschio":
                            CambiaVestito(new AbitiLav() { Abiti = new ComponentDrawables(-1, 0, -1, 96, 41, -1, 24, 40, 15, 0, 0, 54), TextureVestiti = new ComponentDrawables(-1, 0, -1, 0, 0, -1, 0, 0, 0, 0, 0, 0), Accessori = new PropIndices(47, -1, -1, -1, -1, -1, -1, -1, -1), TexturesAccessori = new PropIndices(-1, -1, -1, -1, -1, -1, -1, -1, -1) });

                            break;
                        case "Femmina":
                            CambiaVestito(new AbitiLav() { Abiti = new ComponentDrawables(-1, 0, -1, 111, 42, -1, 24, 24, 3, 0, 0, 47), TextureVestiti = new ComponentDrawables(-1, 0, -1, 0, 0, -1, 0, 0, 0, 0, 0, 0), Accessori = new PropIndices(46, 0, 0, 0, 0, 0, 0, 0, 0), TexturesAccessori = new PropIndices(0, -1, -1, -1, -1, -1, -1, -1, -1) });

                            break;
                    }
                }
                else if (item == Giubbotto)
                {
                    if (Cache.PlayerCache.MyPlayer.Ped.Armor < 1)
                    {
                        SetPedComponentVariation(PlayerPedId(), 9, 4, 1, 2);
                        Cache.PlayerCache.MyPlayer.Ped.Armor = 30;
                    }
                    else
                    {
                        SetPedComponentVariation(PlayerPedId(), 9, 0, 1, 2);
                        Cache.PlayerCache.MyPlayer.Ped.Armor = 0;
                    }
                }

                NetworkFadeInEntity(PlayerPedId(), true);
                await BaseScript.Delay(500);
                Screen.Fading.FadeIn(800);
                await Task.FromResult(0);
            };
            Spogliatoio.Visible = true;
        }

        public static async void CambiaVestito(AbitiLav dress)
        {
            int id = PlayerPedId();
            SetPedComponentVariation(id, (int)DrawableIndexes.Face, dress.Abiti.Face, dress.TextureVestiti.Face, 2);
            SetPedComponentVariation(id, (int)DrawableIndexes.Mask, dress.Abiti.Mask, dress.TextureVestiti.Mask, 2);
            SetPedComponentVariation(id, (int)DrawableIndexes.Torso, dress.Abiti.Torso, dress.TextureVestiti.Torso, 2);
            SetPedComponentVariation(id, (int)DrawableIndexes.Pants, dress.Abiti.Pants, dress.TextureVestiti.Pants, 2);
            SetPedComponentVariation(id, (int)DrawableIndexes.Bag_Parachute, dress.Abiti.Bag_Parachute, dress.TextureVestiti.Bag_Parachute, 2);
            SetPedComponentVariation(id, (int)DrawableIndexes.Shoes, dress.Abiti.Shoes, dress.TextureVestiti.Shoes, 2);
            SetPedComponentVariation(id, (int)DrawableIndexes.Accessories, dress.Abiti.Accessories, dress.TextureVestiti.Accessories, 2);
            SetPedComponentVariation(id, (int)DrawableIndexes.Undershirt, dress.Abiti.Undershirt, dress.TextureVestiti.Undershirt, 2);
            SetPedComponentVariation(id, (int)DrawableIndexes.Kevlar, dress.Abiti.Kevlar, dress.TextureVestiti.Kevlar, 2);
            SetPedComponentVariation(id, (int)DrawableIndexes.Badge, dress.Abiti.Badge, dress.TextureVestiti.Badge, 2);
            SetPedComponentVariation(id, (int)DrawableIndexes.Torso_2, dress.Abiti.Torso_2, dress.TextureVestiti.Torso_2, 2);
            SetPedPropIndex(id, (int)PropIndexes.Hats_Masks, dress.Accessori.Hats_masks, dress.TexturesAccessori.Hats_masks, true);
            SetPedPropIndex(id, (int)PropIndexes.Ears, dress.Accessori.Ears, dress.TexturesAccessori.Ears, true);
            SetPedPropIndex(id, (int)PropIndexes.Glasses, dress.Accessori.Glasses, dress.TexturesAccessori.Glasses, true);
            SetPedPropIndex(id, (int)PropIndexes.Unk_3, dress.Accessori.Unk_3, dress.TexturesAccessori.Unk_3, true);
            SetPedPropIndex(id, (int)PropIndexes.Unk_4, dress.Accessori.Unk_4, dress.TexturesAccessori.Unk_4, true);
            SetPedPropIndex(id, (int)PropIndexes.Unk_5, dress.Accessori.Unk_5, dress.TexturesAccessori.Unk_5, true);
            SetPedPropIndex(id, (int)PropIndexes.Watches, dress.Accessori.Watches, dress.TexturesAccessori.Watches, true);
            SetPedPropIndex(id, (int)PropIndexes.Bracelets, dress.Accessori.Bracelets, dress.TexturesAccessori.Bracelets, true);
            SetPedPropIndex(id, (int)PropIndexes.Unk_8, dress.Accessori.Unk_8, dress.TexturesAccessori.Unk_8, true);
        }

        #endregion

        #region MenuF6
        private static UIMenuItem InterazioneCivileItem;
        private static UIMenuItem InterazioneVeicoloItem;
        private static UIMenuItem ControlliPersonaRemotoItem;
        private static UIMenuItem ControlliVeicoloRemotoItem;
        private static UIMenuItem OggettiItem;

        private static UIMenu MenuPoliziaPrincipale = new UIMenu("", "");
        private static UIMenu InterazioneCivile = new UIMenu("", "");
        private static UIMenu InterazioneVeicolo = new UIMenu("", "");
        private static UIMenu ControlliPersonaRemoto = new UIMenu("", "");
        private static UIMenu ControlliVeicoloRemoto = new UIMenu("", "");
        private static UIMenu Oggetti = new UIMenu("", "");
        private static uint StreetA = 0;
        private static uint StreetB = 0;
        private static UIMenu CercaPers = new UIMenu("", "");
        private static UIMenuItem Pos = new UIMenuItem("");

        public static async void MainMenu()
        {
            string nome = "";
            string cognome = "";
            string numero = "";
            MenuPoliziaPrincipale = new UIMenu("Menu Polizia", "IO SONO LA LEGGE!", new Point(50, 200), "thelastgalaxy", "bannerbackground", false, true);
            InterazioneCivileItem = new("Interazioni col Cittadino", "Mi faccia vedere i dati!");
            InterazioneCivile = new("Interazioni col Cittadino", "");
            InterazioneVeicoloItem = new("Interazioni col Veicolo", "Mi faccia controllare il veicolo!");
            InterazioneVeicolo = new("Interazioni col Veicolo", "");
            ControlliPersonaRemotoItem = new("S.O.C.P.", "Sistema Online di Controllo Persone!");
            ControlliPersonaRemoto = new("S.O.C.P.", "");
            ControlliVeicoloRemotoItem = new("S.O.C.T.", "Sistema Online di Controllo Veicoli!");
            ControlliVeicoloRemoto = new("S.O.C.T.", "");
            OggettiItem = new("Posa a terra un oggetto", "Abbiamo anche le bande chiodate!");
            Oggetti = new("Posa a terra un oggetto", "");

            InterazioneCivileItem.BindItemToMenu(InterazioneCivile);
            InterazioneVeicoloItem.BindItemToMenu(InterazioneVeicolo);
            ControlliPersonaRemotoItem.BindItemToMenu(ControlliPersonaRemoto);
            ControlliVeicoloRemotoItem.BindItemToMenu(ControlliVeicoloRemoto);
            OggettiItem.BindItemToMenu(Oggetti);

            MenuPoliziaPrincipale.AddItem(InterazioneCivileItem);
            MenuPoliziaPrincipale.AddItem(InterazioneVeicoloItem);
            MenuPoliziaPrincipale.AddItem(ControlliPersonaRemotoItem);
            MenuPoliziaPrincipale.AddItem(ControlliVeicoloRemotoItem);
            MenuPoliziaPrincipale.AddItem(OggettiItem);


            #region CIVILE

            UIMenuItem DatiPlayerItem = new("Carta d'identità", "Controllino");
            UIMenu DatiPlayer = new("Carta d'identità", "Controllino");
            UIMenuItem PerquisizioneItem = new("Perquisisci", "Controllino");
            UIMenu Perquisizione = new("Perquisisci", "Controllino");
            UIMenuItem ammanetta = new UIMenuItem("Ammanetta / Smanetta"); // CON ANIMAZIONE -- testare
            UIMenuItem accompagna = new UIMenuItem("Accompagna");          // SE RIESCO FACENDO ANCHE CAMMINARE IL PED DELLA PERSONA -- testare
            UIMenuItem FedinaPenaleItem = new("Controllo Fedina");
            UIMenu FedinaPenale = new("Controllo Fedina", "");
            UIMenuItem mettiVeicolo = new UIMenuItem("Fai sedere nel veicolo");
            UIMenuItem togliVeicolo = new UIMenuItem("Fai uscire dal veicolo");              // FAI USCIRE FAI ENTRARE CON ANIMAZIONE -- testare
            UIMenuItem multaItem = new("Fai una Multa");                    // CREARE SISTEMA MULTE e aggiungere multa personalizzata 
            UIMenu multa = new("Fai una Multa", "");
            UIMenuItem fattureItem = new("Controllo pagamenti in sospeso"); // SALVATAGGIO FATTURE IN DB
            UIMenu fatture = new("Controllo pagamenti in sospeso", "");
            UIMenuItem incarcera = new UIMenuItem("Incarcera");                              //DEVI ESSERE VICINO ALLA CELLA!
            UIMenuItem LicenzeItem = new("Controlla Licenze");              // controllo licenze e patenti
            UIMenu Licenze = new("Controlla Licenze", "");

            DatiPlayerItem.BindItemToMenu(DatiPlayer);
            PerquisizioneItem.BindItemToMenu(Perquisizione);
            FedinaPenaleItem.BindItemToMenu(FedinaPenale);
            multaItem.BindItemToMenu(multa);
            fattureItem.BindItemToMenu(fatture);
            LicenzeItem.BindItemToMenu(Licenze);

            InterazioneCivile.AddItem(DatiPlayerItem);
            InterazioneCivile.AddItem(PerquisizioneItem);
            InterazioneCivile.AddItem(FedinaPenaleItem);
            InterazioneCivile.AddItem(multaItem);
            InterazioneCivile.AddItem(fattureItem);
            InterazioneCivile.AddItem(LicenzeItem);

            InterazioneCivile.AddItem(ammanetta);
            InterazioneCivile.AddItem(accompagna);
            InterazioneCivile.AddItem(mettiVeicolo);
            InterazioneCivile.AddItem(togliVeicolo);
            InterazioneCivile.AddItem(incarcera);

            DatiPlayer.OnMenuOpen += (_newMenu, b) =>
            {
                if (Client.Instance.GetPlayers.ToList().Count > 1)
                {
                    Tuple<Player, float> Player_Distance = Funzioni.GetClosestPlayer();
                    Ped ClosestPed = Player_Distance.Item1.Character;
                    int playerServerId = GetPlayerServerId(Player_Distance.Item1.Handle);
                    User player = Funzioni.GetPlayerCharFromServerId(playerServerId);
                    float distance = Player_Distance.Item2;

                    if (distance < 3f && ClosestPed != null)
                    {
                        UIMenuItem nomeCognome = new("Nome e Cognome");
                        nomeCognome.SetRightLabel(player.FullName);
                        UIMenuItem dDN = new("Data di Nascita");
                        dDN.SetRightLabel(player.DoB);
                        UIMenuItem sesso = new("Sesso");
                        sesso.SetRightLabel(Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.Sex);
                        UIMenuItem altezza = new("Altezza");
                        altezza.SetRightLabel(Cache.PlayerCache.MyPlayer.User.CurrentChar.Info.Height + "cm");
                        UIMenuItem job = new("Occupazione Attuale");
                        job.SetRightLabel(Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Name);
                        UIMenuItem telefono = new("N° di Telefono");
                        telefono.SetRightLabel("" + Cache.PlayerCache.MyPlayer.User.CurrentChar.Info.PhoneNumber);
                        UIMenuItem assicurazione = new("N° di Assicurazione");
                        assicurazione.SetRightLabel("" + Cache.PlayerCache.MyPlayer.User.CurrentChar.Info.Insurance);
                        UIMenuItem nomePlayer = new("Nome Player", "~r~ATTENZIONE!!~w~ - Da usare solo in caso di necessità~n~Un uso sbagliato verrà considerato metagame!");
                        nomePlayer.SetRightLabel(Player_Distance.Item1.Name);
                        DatiPlayer.AddItem(nomeCognome);
                        DatiPlayer.AddItem(dDN);
                        DatiPlayer.AddItem(sesso);
                        DatiPlayer.AddItem(altezza);
                        DatiPlayer.AddItem(job);
                        DatiPlayer.AddItem(telefono);
                        DatiPlayer.AddItem(assicurazione);
                        DatiPlayer.AddItem(nomePlayer);
                    }
                    else
                    {
                        UIMenuItem noPlayers = new("Non ci sono Player nelle vicinanze!");
                        DatiPlayer.AddItem(noPlayers);
                    }
                }
                else
                {
                    UIMenuItem noPlayers = new("Non ci sono altri Player nel server!");
                    DatiPlayer.AddItem(noPlayers);
                }
            };

            Perquisizione.OnMenuOpen += (_newMenu, b) =>
            {
                if (_newMenu.MenuItems.Count > 0) _newMenu.Clear();

                if (Client.Instance.GetPlayers.ToList().Count() > 1)
                {
                    Tuple<Player, float> Player_Distance = Funzioni.GetClosestPlayer();
                    Ped ClosestPed = Player_Distance.Item1.Character;
                    int GetPlayerserverId = GetPlayerServerId(Player_Distance.Item1.Handle);
                    User player = Funzioni.GetPlayerCharFromServerId(GetPlayerserverId);
                    float distance = Player_Distance.Item2;

                    if (distance < 3f)
                    {
                        List<Inventory> inv = Cache.PlayerCache.MyPlayer.User.Inventory;

                        if (inv.Count > 0)
                        {
                            foreach (Inventory it in inv)
                                if (it.Amount > 0)
                                {
                                    UIMenuItem oggetto = new(it.Item);
                                    oggetto.SetRightLabel($"Quantità: {it.Amount}");
                                    _newMenu.AddItem(oggetto);
                                    oggetto.Activated += async (_menu, item) =>
                                    {
                                        BaseScript.TriggerServerEvent("lprp:polizia:confisca", it.Item, it.Amount);
                                    };
                                }
                        }
                        else
                        {
                            Perquisizione.AddItem(new UIMenuItem("Questa persona non ha oggetti con se!"));
                        }
                    }
                    else
                    {
                        Perquisizione.AddItem(new UIMenuItem("Non ci sono Player nelle vicinanze!"));
                    }
                }
                else
                {
                    Perquisizione.AddItem(new UIMenuItem("Non ci sono Players oltre te"));
                }
            };
            CercaPers.OnMenuOpen += async (_newMenu, b) =>
            {
                Dictionary<string, User> players = await Funzioni.GetAllPlayersAndTheirData();
                _newMenu.Clear();

                foreach (KeyValuePair<string, User> pers in players.OrderBy(x => x.Key))
                {
                    foreach (Char_data data in pers.Value.Characters)
                        if (!string.IsNullOrEmpty(nome) && data.Info.Firstname.Contains(nome) || !string.IsNullOrEmpty(cognome) && data.Info.Lastname.Contains(cognome) || numero != "" && numero != null && data.Info.PhoneNumber.ToString().Contains(numero))
                        {
                            int source = 0;
                            foreach (Player p in Client.Instance.GetPlayers.ToList().Where(p => p.Name == pers.Key)) source = p.ServerId;
                            UIMenuItem PersonaggioItem = new UIMenuItem(data.Info.Firstname + " " + data.Info.Lastname + " [" + pers.Key + "]");
                            UIMenu Personaggio = new(data.Info.Firstname + " " + data.Info.Lastname + " [" + pers.Key + "]", "");
                            PersonaggioItem.BindItemToMenu(Personaggio);
                            CercaPers.AddItem(PersonaggioItem);
                            UIMenuItem NomeCognome = new("Nome:", "Il suo Nome");
                            NomeCognome.SetRightLabel(data.Info.Firstname + " " + data.Info.Lastname);
                            Personaggio.AddItem(NomeCognome);
                            UIMenuItem DDN = new("Data di Nascita:", "La sua data di nascita");
                            DDN.SetRightLabel(data.Info.DateOfBirth);
                            Personaggio.AddItem(DDN);
                            //UIMenuItem Altez = new UIMenuItem("Altezza:");
                            UIMenuItem job = new("Lavoro: ", "Il suo lavoro");
                            job.SetRightLabel(data.Job.Name);
                            Personaggio.AddItem(job);
                            //UIMenuItem gang = new UIMenuItem("Gang: ", "Le affiliazioni");
                            UIMenuItem bank = new("Banca: ", "I soldi in banca");
                            bank.SetRightLabel("$" + data.Finance.Bank);
                            Personaggio.AddItem(bank);
                            Pos = new UIMenuItem("Ultima Posizione conosciuta");
                            if (source != 0)
                                GetStreetNameAtCoord(Client.Instance.GetPlayers[source].Character.Position.X, Client.Instance.GetPlayers[source].Character.Position.Y, Client.Instance.GetPlayers[source].Character.Position.Z, ref StreetA, ref StreetB);
                            else
                                GetStreetNameAtCoord(data.Position.X, data.Position.Y, data.Position.Z, ref StreetA, ref StreetB);
                            Pos.Description = GetStreetNameFromHashKey(StreetA);
                            if (StreetB != 0) Pos.Description = Pos.Description + ", angolo " + GetStreetNameFromHashKey(StreetB);
                            Personaggio.AddItem(Pos);
                            UIMenuItem fedinaPenaleItem = new UIMenuItem("Fedina Penale");
                            UIMenuItem FattureItem = new UIMenuItem("Multe / Insoluti");
                            UIMenu fedinaPenale = new("Fedina Penale", "");
                            UIMenu Fatture = new("Multe / Insoluti", "");
                            fedinaPenaleItem.BindItemToMenu(fedinaPenale);
                            FattureItem.BindItemToMenu(Fatture);
                            Personaggio.AddItem(fedinaPenaleItem);
                            Personaggio.AddItem(FattureItem);

                        }
                }
            };
            MenuPoliziaPrincipale.OnMenuOpen += (a, b) => Client.Instance.AddTick(ControlloMenu);
            MenuPoliziaPrincipale.OnMenuClose += (a) => Client.Instance.RemoveTick(ControlloMenu);

            InterazioneCivile.OnItemSelect += async (menu, item, index) =>
            {
                if (Client.Instance.Clients.ToList().Count() > 1)
                {
                    Tuple<Player, float> Player_Distance = Funzioni.GetClosestPlayer();
                    Ped ClosestPed = Player_Distance.Item1.Character;
                    int playerServerId = Player_Distance.Item1.ServerId;
                    PlayerClient player = Funzioni.GetPlayerClientFromServerId(playerServerId);
                    float distance = Player_Distance.Item2;

                    if (distance < 3f && ClosestPed != null)
                        switch (item)
                        {
                            case UIMenuItem i when i == ammanetta:
                                BaseScript.TriggerServerEvent("lprp:polizia:ammanetta_smanetta", playerServerId);

                                break;
                            case UIMenuItem i when i == accompagna:
                                if (player.Status.RolePlayStates.Cuffed) // rifare client-->server-->client
                                    BaseScript.TriggerServerEvent("lprp:polizia:accompagna", playerServerId, Cache.PlayerCache.MyPlayer.Ped.NetworkId);
                                else
                                    HUD.ShowNotification("Non è ammanettato!!");

                                break;
                            case UIMenuItem i when i == mettiVeicolo:
                                if (player.Status.RolePlayStates.Cuffed) // rifare client-->server-->client
                                    BaseScript.TriggerServerEvent("lprp:polizia:mettiVeicolo", playerServerId);
                                else
                                    HUD.ShowNotification("Non è ammanettato!!");

                                break;
                            case UIMenuItem i when i == togliVeicolo: // rifare client-->server-->client
                                if (player.Status.RolePlayStates.Cuffed)
                                    BaseScript.TriggerServerEvent("lprp:polizia:esciVeicolo", playerServerId);
                                else
                                    HUD.ShowNotification("Non è ammanettato!!");

                                break;
                            case UIMenuItem i when i == incarcera: break;
                        }
                    else
                        HUD.ShowNotification("Nessuno trovato vicino a te..~n~Avvicinati!", ColoreNotifica.Red, true);
                }
                else
                {
                    HUD.ShowNotification("Non ci sono altri giocatori!", ColoreNotifica.Red, true);
                }
            };

            #endregion

            #region VEICOLO

            UIMenuItem ControlloItem = new("Controlla stato veicolo");
            UIMenu Controllo = new("Controlla stato veicolo", "");
            ControlloItem.BindItemToMenu(Controllo);
            InterazioneVeicolo.AddItem(ControlloItem);
            UIMenuItem PickLock = new UIMenuItem("Apri veicolo chiuso", "Qualcuno lo ha chiuso?");
            UIMenuItem requisizione = new UIMenuItem("Requisisci Veicolo");
            InterazioneVeicolo.AddItem(PickLock);
            InterazioneVeicolo.AddItem(requisizione);
            InterazioneVeicolo.OnItemSelect += async (menu, item, index) =>
            {
                switch (item)
                {
                    case UIMenuItem n when n == PickLock:
                        // check veicolo di un player locked
                        RequestAnimDict("anim@amb@clubhouse@tutorial@bkr_tut_ig3@");
                        while (!HasAnimDictLoaded("anim@amb@clubhouse@tutorial@bkr_tut_ig3@")) await BaseScript.Delay(0);
                        Cache.PlayerCache.MyPlayer.Ped.Task.PlayAnimation("anim@amb@clubhouse@tutorial@bkr_tut_ig3@", "machinic_loop_mechandplayer", 8f, -1, AnimationFlags.Loop);
                        await BaseScript.Delay(5000);
                        // veicolo aperto qui da ora
                        Cache.PlayerCache.MyPlayer.Ped.Task.ClearAll();

                        break;
                    case UIMenuItem n when n == requisizione:
                        TaskStartScenarioInPlace(PlayerPedId(), "CODE_HUMAN_MEDIC_TIME_OF_DEATH", 0, true);
                        await BaseScript.Delay(5000);
                        // veicolo eliminato e riportato in deposito...
                        // oppure marchiato come non necessario (MarkAsNoLongerNeeded) e poi despawnato dopo un po'
                        // oppure si chiama azienda di rimozione auto
                        Cache.PlayerCache.MyPlayer.Ped.Task.ClearAll();

                        break;
                }
            };

            #endregion

            #region Ricerca

            UIMenuItem cercaNome = new UIMenuItem("Cerca per Nome");
            UIMenuItem cercaCognome = new UIMenuItem("Cerca per Cognome");
            UIMenuItem cercaNTelefono = new UIMenuItem("Cerca per N° di telefono");
            // apre un menu a parte (per dinamicità)
            // controllo tutti i player in ServerList (mandandoli al client ogni minuto / o piu tempo) controllando ogni personaggio di ogni player
            // e segnalo tutti quelli con quel nome / cognome. se selezionato mostro i suoi dati.
            ControlliPersonaRemoto.AddItem(cercaNome);
            ControlliPersonaRemoto.AddItem(cercaCognome);
            ControlliPersonaRemoto.AddItem(cercaNTelefono);

            UIMenuItem CercaPersItem = new("Effettua ricerca");
            CercaPers = new("Effettua ricerca", "");
            CercaPersItem.BindItemToMenu(Controllo);
            ControlliPersonaRemoto.AddItem(CercaPersItem);

            UIMenuItem cercaModello = new UIMenuItem("Cerca per Modello");
            UIMenuItem cercaTarga = new UIMenuItem("Cerca per Targa");
            // COME SOPRA
            ControlliVeicoloRemoto.AddItem(cercaModello);
            ControlliVeicoloRemoto.AddItem(cercaTarga);
            ControlliPersonaRemoto.OnItemSelect += async (menu, item, index) =>
            {
                if (item == cercaNome)
                {
                    nome = await HUD.GetUserInput("Inserisci il nome da cercare", "", 30);
                    if (!nome.Any(char.IsDigit))
                        item.SetRightLabel(nome);
                    else
                        HUD.ShowNotification("Non puoi ricercare un nome che contiene cifre!", ColoreNotifica.Red, true);
                }
                else if (item == cercaCognome)
                {
                    cognome = await HUD.GetUserInput("Inserisci il cognome da cercare", "", 30);
                    if (!cognome.Any(char.IsDigit))
                        item.SetRightLabel(cognome);
                    else
                        HUD.ShowNotification("Non puoi ricercare un cognome che contiene cifre!", ColoreNotifica.Red, true);
                }
                else if (item == cercaNTelefono)
                {
                    numero = await HUD.GetUserInput("Inserisci il numero da cercare", "", 30);
                    if (numero.All(char.IsDigit))
                        item.SetRightLabel(numero);
                    else
                        HUD.ShowNotification("Non puoi ricercare un numero che contiene lettere!", ColoreNotifica.Red, true);
                }
            };

            #endregion

            #region OGGETTI

            #endregion

            MenuPoliziaPrincipale.Visible = true;
        }

        #endregion

        #region MenuSpawnVeicoli

        ///////////////////////// ELICOTTERI
        private static Vehicle PreviewHeli = new Vehicle(0);
        private static Camera HeliCam = new Camera(0);

        public static async void HeliMenu(StazioniDiPolizia Stazione, SpawnerSpawn Punto)
        {
            LoadInterior(GetInteriorAtCoords(-1267.0f, -3013.135f, -48.5f));
            RequestCollisionAtCoord(-1267.0f, -3013.135f, -48.5f);
            RequestAdditionalCollisionAtCoord(-1267.0f, -3013.135f, -48.5f);
            HeliCam = new Camera(CreateCam("DEFAULT_SCRIPTED_CAMERA", true)) { Position = new Vector3(-1268.174f, -2999.561f, -44.215f), IsActive = true };
            await BaseScript.Delay(1000);
            UIMenu MenuElicotteri = new UIMenu("Elicotteri Polizia", "Pattuglia le strade con stile!", PointF.Empty, "thelastgalaxy", "bannerbackground", false, true);

            foreach (Autorizzati t in Stazione.ElicotteriAutorizzati)
            {
                UIMenuItem veh = new UIMenuItem(t.Nome);
                MenuElicotteri.AddItem(veh);
            }

            MenuElicotteri.OnIndexChange += async (menu, index) =>
            {
                PreviewHeli = await Funzioni.SpawnLocalVehicle(Stazione.ElicotteriAutorizzati[index].Model, new Vector3(-1267.0f, -3013.135f, -48.490f), Punto.SpawnPoints[0].Heading);
                PreviewHeli.IsCollisionEnabled = false;
                PreviewHeli.IsPersistent = true;
                PreviewHeli.PlaceOnGround();
                PreviewHeli.IsPositionFrozen = true;
                if (PreviewHeli.Model.Hash == 353883353) SetVehicleLivery(PreviewHeli.Handle, 0); // per il polmav della pula.. funziona solo per i veicoli d'emergenza!
                PreviewHeli.LockStatus = VehicleLockStatus.Locked;
                SetHeliBladesFullSpeed(PreviewHeli.Handle);
                PreviewHeli.IsInvincible = true;
                PreviewHeli.IsEngineRunning = true;
                PreviewHeli.IsDriveable = false;
                if (HeliCam.IsActive && PreviewHeli.Exists()) HeliCam.PointAt(PreviewHeli);
            };
            MenuElicotteri.OnItemSelect += async (menu, item, index) =>
            {
                Screen.Fading.FadeOut(800);
                await BaseScript.Delay(1000);
                HeliCam.IsActive = false;
                RenderScriptCams(false, false, 0, false, false);

                foreach (SpawnPoints t in Punto.SpawnPoints)
                    if (!Funzioni.IsSpawnPointClear(t.Coords.ToVector3, 2f))
                    {
                        continue;
                    }
                    else if (Funzioni.IsSpawnPointClear(t.Coords.ToVector3, 2f))
                    {
                        PoliziaMainClient.ElicotteroAttuale = await Funzioni.SpawnVehicle(Stazione.ElicotteriAutorizzati[index].Model, t.Coords.ToVector3, t.Heading);

                        break;
                    }
                    else
                    {
                        PoliziaMainClient.ElicotteroAttuale = await Funzioni.SpawnVehicle(Stazione.ElicotteriAutorizzati[index].Model, Punto.SpawnPoints[0].Coords.ToVector3, Punto.SpawnPoints[0].Heading);

                        break;
                    }

                Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.SetVehicleFuelLevel(100f);
                Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.IsDriveable = true;
                Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Mods.LicensePlate = SharedMath.GetRandomInt(99) + "POL" + SharedMath.GetRandomInt(999);
                if (Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Model.Hash == 353883353) SetVehicleLivery(Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Handle, 0);
                Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.SetDecor("VehiclePoliceizia", SharedMath.GetRandomInt(100));
                VehiclePolice veh = new VehiclePolice(Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Mods.LicensePlate, Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Model.Hash, Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Handle);
                BaseScript.TriggerServerEvent("lprp:polizia:AggiungiVehPolizia", veh.ToJson());
                MenuHandler.CloseAndClearHistory();
                PreviewHeli.MarkAsNoLongerNeeded();
                PreviewHeli.Delete();
            };
            MenuElicotteri.OnMenuOpen += async (a, b) =>
            {
                PreviewHeli = await Funzioni.SpawnLocalVehicle(Stazione.ElicotteriAutorizzati[0].Model, new Vector3(-1267.0f, -3013.135f, -48.490f), 0);
                PreviewHeli.IsCollisionEnabled = false;
                PreviewHeli.IsPersistent = true;
                PreviewHeli.PlaceOnGround();
                PreviewHeli.IsPositionFrozen = true;
                if (PreviewHeli.Model.Hash == 353883353) SetVehicleLivery(PreviewHeli.Handle, 0); // 0 per pula, 1 per medici.. funziona solo per i veicoli d'emergenza!
                PreviewHeli.LockStatus = VehicleLockStatus.Locked;
                PreviewHeli.IsInvincible = true;
                PreviewHeli.IsEngineRunning = true;
                PreviewHeli.IsDriveable = false;
                Client.Instance.AddTick(Heading);
                HeliCam.PointAt(PreviewHeli);
                if (GetInteriorFromEntity(PreviewHeli.Handle) != 0) SetFocusEntity(PreviewHeli.Handle);
                while (!HasCollisionLoadedAroundEntity(PreviewHeli.Handle)) await BaseScript.Delay(1000);
                RenderScriptCams(true, false, 0, false, false);
                Screen.Fading.FadeIn(800);
            };
            MenuElicotteri.OnMenuClose += async (a) =>
            {
                Screen.Fading.FadeOut(800);
                await BaseScript.Delay(1000);
                Client.Instance.RemoveTick(Heading);
                HeliCam.IsActive = false;
                RenderScriptCams(false, false, 0, false, false);
                ClearFocus();
                await BaseScript.Delay(1000);
                Screen.Fading.FadeIn(800);
                PreviewHeli.MarkAsNoLongerNeeded();
                PreviewHeli.Delete();
            };
            MenuElicotteri.Visible = true;
        }

        #endregion

        #region MenuVeicoliGarageSperimentale

        private static List<Vehicle> veicoliParcheggio = new List<Vehicle>();
        private static StazioniDiPolizia StazioneAttuale = new StazioniDiPolizia();
        private static int LivelloGarage = 0;
        private static List<Vector4> parcheggi = new List<Vector4>()
        {
            new Vector4(224.500f, -998.695f, -99.6f, 225.0f),
            new Vector4(224.500f, -994.630f, -99.6f, 225.0f),
            new Vector4(224.500f, -990.255f, -99.6f, 225.0f),
            new Vector4(224.500f, -986.628f, -99.6f, 225.0f),
            new Vector4(224.500f, -982.496f, -99.6f, 225.0f),
            new Vector4(232.500f, -982.496f, -99.6f, 135.0f),
            new Vector4(232.500f, -986.628f, -99.6f, 135.0f),
            new Vector4(232.500f, -990.255f, -99.6f, 135.0f),
            new Vector4(232.500f, -994.630f, -99.6f, 135.0f),
            new Vector4(232.500f, -998.695f, -99.6f, 135.0f)
        };
        private static SpawnerSpawn PuntoAttuale = new SpawnerSpawn();
        private static bool InGarage = false;

        public static async void VehicleMenuNuovo(StazioniDiPolizia Stazione, SpawnerSpawn Punto)
        {
            Cache.PlayerCache.MyPlayer.Status.Instance.Istanzia("SceltaVeicoliPolizia");
            StazioneAttuale = Stazione;
            PuntoAttuale = Punto;
            Cache.PlayerCache.MyPlayer.Ped.Position = new Vector3(236.349f, -1005.013f, -100f);
            Cache.PlayerCache.MyPlayer.Ped.Heading = 85.162f;
            InGarage = true;

            if (Stazione.VeicoliAutorizzati.Count(o => o.GradiAutorizzati[0] == -1 || o.GradiAutorizzati.Contains(Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Grade)) <= 10)
                for (int i = 0; i < Stazione.VeicoliAutorizzati.Count(o => o.GradiAutorizzati[0] == -1 || o.GradiAutorizzati.Contains(Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Grade)); i++)
                {
                    veicoliParcheggio.Add(await Funzioni.SpawnLocalVehicle(Stazione.VeicoliAutorizzati[i].Model, new Vector3(parcheggi[i].X, parcheggi[i].Y, parcheggi[i].Z), parcheggi[i].W));
                    veicoliParcheggio[i].PlaceOnGround();
                    veicoliParcheggio[i].IsPersistent = true;
                    veicoliParcheggio[i].LockStatus = VehicleLockStatus.Unlocked;
                    veicoliParcheggio[i].IsInvincible = true;
                    veicoliParcheggio[i].IsCollisionEnabled = true;
                    veicoliParcheggio[i].IsEngineRunning = false;
                    veicoliParcheggio[i].IsDriveable = false;
                    veicoliParcheggio[i].IsSirenActive = true;
                    veicoliParcheggio[i].IsSirenSilent = true;
                    veicoliParcheggio[i].SetDecor("VehiclePoliceizia", SharedMath.GetRandomInt(100));
                }
            else
                await GarageConPiuVeicoli(Stazione.VeicoliAutorizzati, LivelloGarage);

            await BaseScript.Delay(1000);
            Screen.Fading.FadeIn(800);
            Client.Instance.AddTick(ControlloGarageNew);
        }

        private static async Task GarageConPiuVeicoli(List<Autorizzati> autorizzati, int livelloGarage)
        {
            foreach (Vehicle veh in veicoliParcheggio) veh.Delete();
            veicoliParcheggio.Clear();
            int totale = autorizzati.Count(o => o.GradiAutorizzati[0] == -1 || o.GradiAutorizzati.Contains(Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Grade));
            int LivelloGarageAttuali = totale - livelloGarage * 10 > livelloGarage * 10 ? 10 : totale - livelloGarage * 10;

            for (int i = 0; i < LivelloGarageAttuali; i++)
            {
                veicoliParcheggio.Add(await Funzioni.SpawnLocalVehicle(autorizzati[i + livelloGarage * 10].Model, new Vector3(parcheggi[i].X, parcheggi[i].Y, parcheggi[i].Z), parcheggi[i].W));
                veicoliParcheggio[i].PlaceOnGround();
                veicoliParcheggio[i].IsPersistent = true;
                veicoliParcheggio[i].LockStatus = VehicleLockStatus.Unlocked;
                veicoliParcheggio[i].IsInvincible = true;
                veicoliParcheggio[i].IsCollisionEnabled = true;
                veicoliParcheggio[i].IsEngineRunning = false;
                veicoliParcheggio[i].IsDriveable = false;
                veicoliParcheggio[i].IsSirenActive = true;
                veicoliParcheggio[i].IsSirenSilent = true;
                veicoliParcheggio[i].SetDecor("VehiclePoliceizia", SharedMath.GetRandomInt(100));
            }
        }

        #endregion

        #region MenuArmeria

        #endregion

        #region ControlliETask

        private static async Task ControlloGarageNew()
        {
            Ped p = Cache.PlayerCache.MyPlayer.Ped;

            if (Cache.PlayerCache.MyPlayer.Status.Instance.Instanced)
                if (InGarage)
                {
                    if (p.IsInRangeOf(new Vector3(240.317f, -1004.901f, -99f), 3f))
                    {
                        HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per cambiare piano");
                        if (Input.IsControlJustPressed(Control.Context)) MenuPiano();
                    }

                    if (Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle)
                        if (p.CurrentVehicle.HasDecor("VehiclePoliceizia"))
                        {
                            HUD.ShowHelp("Per selezionare questo veicolo~n~~y~Accendi il motore~w~ e ~y~accelera~w~.");

                            if (Input.IsControlJustPressed(Control.VehicleAccelerate) && p.CurrentVehicle.IsEngineRunning)
                            {
                                Screen.Fading.FadeOut(800);
                                await BaseScript.Delay(1000);
                                int model = p.CurrentVehicle.Model.Hash;
                                foreach (Vehicle vehicle in veicoliParcheggio) vehicle.Delete();
                                veicoliParcheggio.Clear();

                                for (int i = 0; i < PuntoAttuale.SpawnPoints.Count; i++)
                                    if (!Funzioni.IsSpawnPointClear(PuntoAttuale.SpawnPoints[i].Coords.ToVector3, 2f))
                                    {
                                        continue;
                                    }
                                    else if (Funzioni.IsSpawnPointClear(PuntoAttuale.SpawnPoints[i].Coords.ToVector3, 2f))
                                    {
                                        PoliziaMainClient.VeicoloAttuale = await Funzioni.SpawnVehicle(model, PuntoAttuale.SpawnPoints[i].Coords.ToVector3, PuntoAttuale.SpawnPoints[i].Heading);

                                        break;
                                    }
                                    else
                                    {
                                        PoliziaMainClient.VeicoloAttuale = await Funzioni.SpawnVehicle(model, PuntoAttuale.SpawnPoints[0].Coords.ToVector3, PuntoAttuale.SpawnPoints[0].Heading);

                                        break;
                                    }

                                p.CurrentVehicle.SetVehicleFuelLevel(100f);
                                p.CurrentVehicle.IsEngineRunning = true;
                                p.CurrentVehicle.IsDriveable = true;
                                p.CurrentVehicle.Mods.LicensePlate = SharedMath.GetRandomInt(99) + "POL" + SharedMath.GetRandomInt(999);
                                p.CurrentVehicle.SetDecor("VehiclePoliceizia", SharedMath.GetRandomInt(100));
                                VehiclePolice veh = new VehiclePolice(p.CurrentVehicle.Mods.LicensePlate, p.CurrentVehicle.Model.Hash, p.CurrentVehicle.Handle);
                                BaseScript.TriggerServerEvent("lprp:polizia:AggiungiVehPolizia", veh.ToJson());
                                InGarage = false;
                                StazioneAttuale = null;
                                PuntoAttuale = null;
                                veicoliParcheggio.Clear();
                                Cache.PlayerCache.MyPlayer.Status.Instance.RimuoviIstanza();
                                await BaseScript.Delay(1000);
                                Screen.Fading.FadeIn(800);
                                Client.Instance.RemoveTick(ControlloGarageNew);
                            }
                        }
                }
        }

        private static async void MenuPiano()
        {
            UIMenu Ascensore = new UIMenu("Seleziona Piano", "Sali o scendi?", PointF.Empty, "thelastgalaxy", "bannerbackground", false, true);
            UIMenuItem esci = new UIMenuItem("Esci dal Garage");
            Ascensore.AddItem(esci);
            int conto = StazioneAttuale.VeicoliAutorizzati.Count(o => o.GradiAutorizzati[0] == -1 || o.GradiAutorizzati.Contains(Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Grade));
            int piani = 1;
            for (int i = 1; i < conto + 1; i++)
                if (i % 10 == 0)
                    piani++;

            for (int i = 0; i < piani; i++)
            {
                UIMenuItem piano = new UIMenuItem($"{i + 1}° piano");
                Ascensore.AddItem(piano);
                if (i == LivelloGarage) piano.SetRightBadge(BadgeIcon.CAR);
            }

            Ascensore.OnItemSelect += async (menu, item, index) =>
            {
                if (item.RightBadge == BadgeIcon.CAR)
                {
                    HUD.ShowNotification("Questo è il garage attuale!!", true);
                }
                else
                {
                    MenuHandler.CloseAndClearHistory();
                    Screen.Fading.FadeOut(800);
                    await BaseScript.Delay(1000);

                    if (item == esci)
                    {
                        Cache.PlayerCache.MyPlayer.Ped.Position = StazioneAttuale.Veicoli[StazioneAttuale.Veicoli.IndexOf(PuntoAttuale)].SpawnerMenu.ToVector3;
                        InGarage = false;
                        StazioneAttuale = null;
                        PuntoAttuale = null;
                        Cache.PlayerCache.MyPlayer.Status.Instance.RimuoviIstanza();
                        veicoliParcheggio.Clear();
                        Client.Instance.RemoveTick(ControlloGarageNew);
                    }
                    else
                    {
                        LivelloGarage = index - 1;
                        await GarageConPiuVeicoli(StazioneAttuale.VeicoliAutorizzati, LivelloGarage);
                    }

                    await BaseScript.Delay(1000);
                    Screen.Fading.FadeIn(800);
                }
            };
            Ascensore.Visible = true;
        }

        private static async Task ControlloMenu()
        {
            Ped p = Cache.PlayerCache.MyPlayer.Ped;

            if (Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle)
            {
                if (p.CurrentVehicle.Driver == Cache.PlayerCache.MyPlayer.Ped && p.CurrentVehicle.Speed < 2 || p.CurrentVehicle.GetPedOnSeat(VehicleSeat.Passenger) == Cache.PlayerCache.MyPlayer.Ped)
                {
                    if (InterazioneCivileItem.Enabled)
                    {
                        InterazioneCivileItem.Enabled = false;
                        InterazioneCivileItem.SetRightBadge(BadgeIcon.LOCK);
                        InterazioneCivileItem.Description = InterazioneCivileItem.Description + " - ~r~NON~w~ disponibile dentro un veicolo";
                    }

                    if (InterazioneVeicoloItem.Enabled)
                    {
                        InterazioneVeicoloItem.Enabled = false;
                        InterazioneVeicoloItem.SetRightBadge(BadgeIcon.LOCK);
                        InterazioneVeicoloItem.Description = InterazioneVeicoloItem.Description + " - ~r~NON~w~ disponibile dentro un veicolo";
                    }

                    if (OggettiItem.Enabled)
                    {
                        OggettiItem.Enabled = false;
                        OggettiItem.SetRightBadge(BadgeIcon.LOCK);
                        OggettiItem.Description = OggettiItem.Description + " - ~r~NON~w~ disponibile dentro un veicolo";
                    }

                    if (!ControlliPersonaRemotoItem.Enabled)
                    {
                        ControlliPersonaRemotoItem.Enabled = true;
                        ControlliPersonaRemotoItem.SetRightBadge(BadgeIcon.NONE);
                        ControlliPersonaRemotoItem.Description = "Sistema Online di Controllo Persone!";
                    }

                    if (!ControlliVeicoloRemotoItem.Enabled)
                    {
                        ControlliVeicoloRemotoItem.Enabled = true;
                        ControlliVeicoloRemotoItem.SetRightBadge(BadgeIcon.NONE);
                        ControlliVeicoloRemotoItem.Description = "Sistema Online di Controllo Veicoli!";
                    }
                }
            }
            else
            {
                if (!InterazioneCivileItem.Enabled)
                {
                    InterazioneCivileItem.Enabled = true;
                    InterazioneCivileItem.SetRightBadge(BadgeIcon.NONE);
                    InterazioneCivileItem.Description = "Mi faccia vedere i dati!";
                }

                if (!InterazioneVeicoloItem.Enabled)
                {
                    InterazioneVeicoloItem.Enabled = true;
                    InterazioneVeicoloItem.SetRightBadge(BadgeIcon.NONE);
                    InterazioneVeicoloItem.Description = "Mi faccia controllare il veicolo!";
                }

                if (!OggettiItem.Enabled)
                {
                    OggettiItem.Enabled = true;
                    OggettiItem.SetRightBadge(BadgeIcon.NONE);
                    OggettiItem.Description = "Abbiamo anche le bande chiodate!";
                }

                if (ControlliPersonaRemotoItem.Enabled)
                {
                    ControlliPersonaRemotoItem.Enabled = false;
                    ControlliPersonaRemotoItem.SetRightBadge(BadgeIcon.LOCK);
                    ControlliPersonaRemotoItem.Description = ControlliPersonaRemotoItem.Description + " - ~r~NON~w~ disponibile fuori da un veicolo della polizia o lontano da un computer!";
                }

                if (ControlliVeicoloRemotoItem.Enabled)
                {
                    ControlliVeicoloRemotoItem.Enabled = false;
                    ControlliVeicoloRemotoItem.SetRightBadge(BadgeIcon.LOCK);
                    ControlliVeicoloRemotoItem.Description = ControlliVeicoloRemotoItem.Description + " - ~r~NON~w~ disponibile fuori da un veicolo della polizia o lontano da un computer!";
                }
            }

            if (Client.Instance.GetPlayers.ToList().Count() > 1)
            {
                Tuple<Player, float> Player_Distance = Funzioni.GetClosestPlayer();
                float distance = Player_Distance.Item2;

                if (distance < 3)
                {
                    if (!InterazioneCivileItem.Enabled)
                    {
                        InterazioneCivileItem.Enabled = true;
                        InterazioneCivileItem.SetRightBadge(BadgeIcon.NONE);
                        InterazioneCivileItem.Description = "Mi faccia vedere i dati!";
                    }
                }
                else
                {
                    if (InterazioneCivileItem.Enabled)
                    {
                        InterazioneCivileItem.Enabled = false;
                        InterazioneCivileItem.SetRightBadge(BadgeIcon.LOCK);
                        InterazioneCivileItem.Description += "- ~r~NON~w~ ci sono player vicini";
                        if (InterazioneCivile.Visible) InterazioneCivile.Visible = false;
                    }
                }
            }
            else
            {
                if (InterazioneCivileItem.Enabled)
                {
                    InterazioneCivileItem.Enabled = false;
                    InterazioneCivileItem.SetRightBadge(BadgeIcon.LOCK);
                    InterazioneCivileItem.Description += "- ~r~NON~w~ ci sono player vicini";
                    if (InterazioneCivile.Visible) InterazioneCivile.Visible = false;
                }
            }

            MenuPoliziaPrincipale.UpdateDescription();
            await BaseScript.Delay(250);
        }

        private static async Task Heading()
        {
            if (PreviewHeli.Exists())
            {
                RequestCollisionAtCoord(-1267.0f, -3013.135f, -48.5f);
                PreviewHeli.Heading += 1;
                if (!PreviewHeli.IsEngineRunning) SetHeliBladesFullSpeed(PreviewHeli.Handle);
            }

            await Task.FromResult(0);
        }

        #endregion
    }
}