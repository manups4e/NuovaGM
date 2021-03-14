Config.Client.Lavori.Generici.Pescatore = {
    TempoPescaDinamico = false, -- se true, la pesca è più realistica.. può volerci anche 1 minuto o 2 di attesa
    TempoFisso = 10, -- secondi se il tempo di pesca dinamico è false

    PrezzoVenditaPesce = 10, -- guadagno al pesce (moltiplicato per peso e grandezza ;) )
    PrezzoVenditaAltro = 20, -- come sopra ma per altre cose.. frutti di mare.. gamberi..
    
    LuoghiVendita = {
        vector3(-3251.2, 991.5, 11.49),
        vector3(3804.0, 4443.3, 3.0),
        vector3(2517.6, 4218.0, 38.8),
        vector3(-1.827, 6488.367, 31.506),
        vector3(-1552.16, -966.732, 13.017),
        vector3(-1017.513, -1350.519, 5.473)
    },

    AffittoBarca = {
        {
            Menu = vector3(3857.235, 4459.287, 1.84),
            SpawnBarca = vector4(3856.264, 4455.604, 1.405, 268.4614), -- x, y, z, h
            CancellaBarca = vector3(0,0,0),
            Barche = {
--              "TUG", -- non sono sicuro di voler dare il tug (barcone immenso)
                "DINGHY"
            }
        },
    },

--[[
    Pesci = {
        Mare = {
            facile = {
                "orata",
                "sogliola",
                "branzino"
            },
            medio = {
                "branzino",
                "orata",
                "sgombro",
                "sogliola",
            },
            avanzato = {
                "branzino",
                "orata",
                "sgombro",
                "sogliola",
                "tonno",
                "pescespada"
            }
        }
        AcquaDolce = {
            facile = {},
            medio = {},
            avanzato = {}
        }
    }

]]
    Pesci = {
        facile = {
            "orata",
            "sogliola",
            "branzino"
        },
        medio = {
            "branzino",
            "orata",
            "sgombro",
            "sogliola",
        },
        avanzato = {
            "branzino",
            "orata",
            "sgombro",
            "sogliola",
            "tonno",
            "pescespada",
        }
    }
}