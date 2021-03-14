Config.Client.Lavori.Medici.Config = {

    AbilitaBlipVolanti = true, -- vuoi che i medici vedano i colleghi quando sono nei veicoli?
    Ospedali = {
        {
            Blip = {
                Coords =  vector3(307.7, -1433.4, 28.9),
                Sprite = 61,
                Display = 4,
                Scale  = 1.2,
                Color  = 2,
                Nome = "Ospedale Centrale di Los Santos"
            },
    
            Spogliatoio = {
                vector3(270.5, -1363.0, 23.5)
            },
    
            Farmacia = {
                vector3(230.1, -1366.1, 38.5)
            },

            IngressoVisitatori = {
                vector3(344.114, -1397.703, 31.509),
                vector3(342.956, -1399.569, 31.509),
                vector3(390.057, -1433.125, 28.432),
                vector3(391.464, -1431.332, 28.432)
            },

            UscitaVisitatori = {
                vector3(275.515, -1361.274, 23.538),
                vector3(254.465, -1372.403, 23.538)
            },
    
            Veicoli = {
                {
                    SpawnerMenu = vector3(246.876, -1371.816, 24.538),
                    SpawnPoints = {
                        {coords = vector3(297.2, -1429.5, 29.8), heading = 227.6, radius = 4.0},
                        {coords = vector3(294.0, -1433.1, 29.8), heading = 227.6, radius = 4.0},
                        {coords = vector3(309.4, -1442.5, 29.8), heading = 227.6, radius = 6.0}
                    },
                    Deleters = {
                        vector3(323.907, -1473.516, 29.799),
                        vector3(326.377, -1469.785, 29.799)
                    }
                }
            },
    
            Elicotteri = {
                {
                    SpawnerMenu = vector3(317.5, -1449.5, 46.5),
                    SpawnPoints = {
                        {coords = vector3(313.5, -1465.1, 46.5), heading = 142.7, radius = 10.0},
                        {coords = vector3(299.5, -1453.2, 46.5), heading = 142.7, radius = 10.0}
                    },
                    Deleters = {
                        vector3(313.5, -1465.1, 46.5),
                        vector3(299.5, -1453.2, 46.5)
                    }
                }
            },

            VeicoliAutorizzati = {
                { Nome = "Ambulanza", Model = "ambulance",   GradiAutorizzati = {-1} },
            },

            ElicotteriAutorizzati = {
                { Nome = "Maverik Medici", Model = "polmav",   GradiAutorizzati = {-1} },
            },
        }
    }
}
