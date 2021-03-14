-- E' SIMILE A QUELLO DI ESX... MA CON DELLE MIGLIORIE SECONDO ME
Config.Client.Lavori.Polizia.Config = {
    AbilitaBlipVolanti = true, -- se vero, i pulotti possono vedere i veicoli (AUTO, ELICOTTERI, BARCHE) dei colleghi.. ma non i player
    AbilitaTimerManette = false, -- se true, i player ammanettati vengono smanettati dopo un certo tempo.
    TimerManette = 10 * 60000, -- 10 minuti
    MaxInServizio = -1, -- se maggiore di -1 indica il massimo di poliziotti in servizio raggiungibili.. oltre non puoi cambiarti.
    PuoPlaccare = true, -- se true il poliziotto può placcare gli altri
    AbilitaUsoDeiCani = true, -- li vuoi i cani della polizia vero? ;)

    Cani = {
        {Nome = "Rottweiler", Model= "a_c_rottweiler"},
        {Nome = "Husky", Model= "a_c_husky"},
        {Nome = "Pastore Tedesco", Model= "a_c_shepherd"},
        {Nome = "Retriver", Model= "a_c_retriever"},
    },

    -- una volta presa l'arma.. basta usare il menu M per configurare le Modifiche
    ModificheAutorizzate = {
        "COMPONENT_AT_SCOPE_MACRO", -- piccolo mirino (per le armi che lo supportano)
        "COMPONENT_AT_PI_FLSH", -- torcia, per le armi che lo supportano
    },

    -- per la lista di modifiche ti direi guarda https://wiki.rage.mp/index.php?title=Weapons_Components. cmq AT_PI_FLSH = torcia, COMPONENT_AT_SCOPE_MACRO è il mirino zoom piccolo
    Stazioni = { --[[ se aggiungi una stazione.. considera che sono in lista.. quindi non hanno nome così puoi metterne quante vuoi anche 10]]
        { -- prima stazione

            -- NON METTERE X,Y,Z!! SOLO I NUMERI!!
            Blip = {
                Coords  = vector3(425.1, -979.5, 30.7),
                Sprite  = 60,
                Display = 4,
                Scale   = 1.0,
                Color  = 29
            },
    
            Spogliatoio = {
                vector3(452.6, -992.8, 29.69)
            },
    
            Armerie = {
                vector3(451.7, -980.1, 29.69)
            },

            -- -1 per tutti i gradi.. sennò il numero del grado seguito dalla virgola..
            --  SE CI SONO MAIUSCOLE.. METTILE.. "Nome" va bene.. "nome" ti darà errore.. C# è viziato!
            VeicoliAutorizzati = {
                { Nome = "Macchina della Polizia 1", Model = "police",   GradiAutorizzati = {-1} },
                { Nome = "Macchina della Polizia 2", Model = "police2",  GradiAutorizzati = {-1} },
                { Nome = "Macchina della Polizia 3", Model = "police3",  GradiAutorizzati = {-1} },
                { Nome = "Macchina della Polizia 4", Model = "police4",  GradiAutorizzati = {-1} },
                { Nome = "Moto della Polizia",       Model = "policeb",  GradiAutorizzati = {-1} },
                { Nome = "Furgone Polizia 1",        Model = "policet",  GradiAutorizzati = {-1} },
                { Nome = "SUV Sceriffo 1",           Model = "sheriff2", GradiAutorizzati = {-1} },
                { Nome = "SUV Ranger",               Model = "pranger",  GradiAutorizzati = {-1} },
                { Nome = "FIB 1",                    Model = "fbi",      GradiAutorizzati = {-1} },
                { Nome = "FIB 2",                    Model = "fbi2",     GradiAutorizzati = {-1} },
                { Nome = "Macchina Sceriffo 1",      Model = "sheriff",  GradiAutorizzati = {-1} },
                { Nome = "SUV Sceriffo 1",           Model = "sheriff2", GradiAutorizzati = {-1} },
            },

            ElicotteriAutorizzati = {
                { Nome = "Maverik Polizia", Model = "polmav",   GradiAutorizzati = {-1} },
                { Nome = "Annihilator", Model = "annihilator",   GradiAutorizzati = {-1} },
                { Nome = "Buzzard ~g~non armato~w~", Model = "buzzard2",   GradiAutorizzati = {-1} },
                { Nome = "Buzzard ~r~armato~w~", Model = "buzzard",   GradiAutorizzati = {-1} },
                { Nome = "Valchiria", Model = "Valkyrie",   GradiAutorizzati = {-1} }
            },

            ArmiAutorizzate = {
                { Nome = "Tazer", Model = "WEAPON_STUNGUN", GradiAutorizzati = {-1} },
                { Nome = "Pistola", Model = "WEAPON_PISTOL", GradiAutorizzati = {-1} }
            },
    
            Veicoli = {
                {
--                    SpawnerMenu = {454.6, -1017.4, 28.4},
                    SpawnerMenu = vector3(470.396, -984.783, 30.69),
                    SpawnPoints = {
                         { Coords = vector3(438.4, -1018.3, 27.7), Heading = 90.0, Radius = 6.0},
                         { Coords = vector3(441.0, -1024.2, 28.3), Heading = 90.0, Radius = 6.0 },
                         { Coords = vector3(453.5, -1022.2, 28.0), Heading = 90.0, Radius = 6.0 },
                         { Coords = vector3(450.9, -1016.5, 28.1), Heading = 90.0, Radius = 6.0 }
                    },
                    Deleters = {
                        vector3(452.567, -996.697, 25.763),
                        vector3(447.449, -996.954, 25.763)
                    }
                },
            },
    
            Elicotteri = {
                {
                    SpawnerMenu = vector3(461.1, -981.5, 43.6),
                    SpawnPoints = {
                        { Coords = vector3(449.5, -981.2, 43.6), Heading = 92.6, Radius = 10.0 }
                    },
                    Deleters = {
                        vector3(449.5, -981.2, 43.6)
                    }
                }
            },
    
            BossActions = {
                vector3(448.4, -973.2, 29.69)
            }
    
        },
    },
}