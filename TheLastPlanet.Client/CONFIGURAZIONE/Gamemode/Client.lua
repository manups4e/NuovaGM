--[[ SE ALCUNI VALORI HANNO IL PUNTO (esempio 10.0) MANTIENI IL PUNTO ANCHE SE C'E' 0 DOPO SENNO' E' UN CASINO!!]]


Config.Client.Main = { -- QUESTA TABELLA E' PER IL LATO CLIENT!! CERCA ConfigPrincipaleSERVER PER QUELLA SERVER
    --[[ generici ]] --
    NomeServer = "The Last Planet",
    DiscordAppId = "625350962776637491", -- id dell'app discord da collegare a fivem
    DiscordRichPresenceAsset = "majoras2", -- nome dell'asset (immagine) tra quelle caricate sul rich presence dell'app discord
    PassengerDriveBy = true, -- Solo il passeggero può sparare? se false anche il guidatore
    Firstcoords = vector4(1.0, 1.0, 1.0, 1.0), -- coordinate dello spawn iniziale, x, y, z, h (heading cioè dov'è girato)

    AFKCheckTime = 600, -- secondi prima di essere kickato se afk (600 = 10 minuti)
    KickWarning = true, -- vuoi che il player sia avvisato? verrà avvisato sempre a un quarto di secondi prima di essere kickato..
                        -- quindi se sono 600 viene avvisato a (600/4) 150 secondi mancanti (circa 2 minuti e 30 secondi)
    
    -- beh.. qui fai spawnare certi personaggi.. tipo le donnine al vanilla.. o i negozianti d'armi.. basta seguire la scia
    stripClub = {
        {coords = vector3(102.423, -1290.594, 28.2587), animDict = "mini@strip_club@private_dance@part1", animName = "priv_dance_p1", model = "CSB_Stripper_02", heading = (math.random(50, 360)) * 1.0},
        {coords = vector3(104.256, -1294.67, 28.2587), animDict = "mini@strip_club@private_dance@part3", animName = "priv_dance_p3", model = "CSB_Stripper_01", heading = (math.random(50, 360)) * 1.0},
        {coords = vector3(112.480, -1287.032, 27.586), animDict = "mini@strip_club@private_dance@part2", animName = "priv_dance_p2", model = "CSB_Stripper_01", heading = (math.random(50, 360)) * 1.0},
        {coords = vector3(113.111, -1287.755, 27.586), animDict = "mini@strip_club@private_dance@part1", animName = "priv_dance_p1", model = "S_F_Y_Stripper_02", heading = (math.random(50, 360)) * 1.0},
        {coords = vector3(113.375, -1286.546, 27.586), animDict = "mini@strip_club@private_dance@part2", animName = "priv_dance_p2", model = "CSB_Stripper_02", heading = (math.random(50, 360)) * 1.0},
        {coords = vector3(129.442, -1283.407, 28.272), animDict = "missfbi3_party_d", animName = "stand_talk_loop_a_female", model = "S_F_Y_Bartender_01", heading = 122.471}
    },
    blackMarket = {
        {coords = vector3(-2166.786, 5197.684, 15.880), animDict = "", animName = "", model = "G_M_Y_SALVABOSS_01", scenario = "WORLD_HUMAN_SMOKING", heading = 122.471}
    },
    illegal_weapon_extra_shop = {
        {coords = vector3(181.4, 2792.8, 45.7), animDict = "", animName = "", model = "G_M_Y_SALVABOSS_01", scenario = "WORLD_HUMAN_SMOKING", heading = 302.471}
    },
    
    --[[ morte ]] --
    EarlyRespawn = true; -- se lo imposto su false non ci sarà la possibilità di respawnare prima che finisca il tempo
    EarlyRespawnFine = false, -- se hai abilitato la possibilità di respawnare prima dei 10 minuti.. vuoi che paghino?
    EarlyRespawnFineAmount = 5000, -- se EarlyRespawnFine è su true... quanto vuoi che pagano?
    ReviveReward = 700, -- premio per i medici se revivano qualcuno
    EarlySpawnTimer = 300, -- tempo in secondi prima della possibilità di respawnare a comando (5 minuti) se earlyRespawn = true
    BleedoutTimer = 300, -- tempo prima di morire una volta che finisce earlySpawnTimer (5 minuti)

    --[[ armi ]] --
    EscludiMirino = true, -- rimuovere il mirino?
    ScopedWeapons = { -- armi escluse dalla rimozione del mirino (basta cercare l'hash decimale su google)
        100416529,  -- WEAPON_SNIPERRIFLE
        205991906,  -- WEAPON_HEAVYSNIPER
        3342088282, -- WEAPON_MARKSMANRIFLE
        177293209,   -- WEAPON_HEAVYSNIPER MKII
        1785463520,  -- WEAPON_MARKSMANRIFLE_MK2
        911657153	-- tazer
    },

    recoils = {
        [453432689] = 0.3, --PISTOL
        [3219281620] = 0.3, --PISTOL MK2
        [1593441988] = 0.2, --COMBAT PISTOL
        [584646201] = 0.1, --AP PISTOL
        [2578377531] = 0.6, --PISTOL .50
        [324215364] = 0.2, --MICRO SMG
        [736523883] = 0.1, --SMG
        [2024373456] = 0.1, --SMG MK2
        [4024951519] = 0.1, --ASSAULT SMG
        [3220176749] = 0.2, --ASSAULT RIFLE
        [961495388] = 0.2, --ASSAULT RIFLE MK2
        [2210333304] = 0.1, --CARBINE RIFLE
        [4208062921] = 0.1, --CARBINE RIFLE MK2
        [2937143193] = 0.1, --ADVANCED RIFLE
        [2634544996] = 0.1, --MG
        [2144741730] = 0.1, --COMBAT MG
        [3686625920] = 0.1, --COMBAT MG MK2
        [487013001] = 0.4, --PUMP SHOTGUN
        [1432025498] = 0.4, --PUMP SHOTGUN MK2
        [2017895192] = 0.7, --SAWNOFF SHOTGUN
        [3800352039] = 0.4, --ASSAULT SHOTGUN
        [2640438543] = 0.2, --BULLPUP SHOTGUN
        [911657153] = 0.1, --STUN GUN
        [100416529] = 0.5, --SNIPER RIFLE
        [205991906] = 0.3, --HEAVY SNIPER
        [177293209] = 0.7, --HEAVY SNIPER MK2
        [856002082] = 1.2, --REMOTE SNIPER
        [2726580491] = 1.0, --GRENADE LAUNCHER
        [1305664598] = 1.0, --GRENADE LAUNCHER SMOKE
        [2982836145] = 0.0, --RPG
        [1752584910] = 0.0, --STINGER
        [1119849093] = 0.01, --MINIGUN
        [3218215474] = 0.2, --SNS PISTOL
        [2009644972] = 0.25, --SNS PISTOL MK2
        [1627465347] = 0.1, --GUSENBERG
        [3231910285] = 0.2, --SPECIAL CARBINE
        [-1768145561] = 0.25, --SPECIAL CARBINE MK2
        [3523564046] = 0.5, --HEAVY PISTOL
        [2132975508] = 0.2, --BULLPUP RIFLE
        [-2066285827] = 0.25, --BULLPUP RIFLE MK2
        [137902532] = 0.4, --VINTAGE PISTOL
        [-1746263880] = 0.4, --float ACTION REVOLVER
        [2828843422] = 0.7, --MUSKET
        [984333226] = 0.2, --HEAVY SHOTGUN
        [3342088282] = 0.3, --MARKSMAN RIFLE
        [1785463520] = 0.35, --MARKSMAN RIFLE MK2
        [1672152130] = 0, --HOMING LAUNCHER
        [1198879012] = 0.9, --FLARE GUN
        [171789620] = 0.2, --COMBAT PDW
        [3696079510] = 0.9, --MARKSMAN PISTOL
        [1834241177] = 2.4, --RAILGUN
        [3675956304] = 0.3, --MACHINE PISTOL
        [3249783761] = 0.6, --REVOLVER
        [-879347409] = 0.65, --REVOLVER MK2
        [4019527611] = 0.7, --float BARREL SHOTGUN
        [1649403952] = 0.3, --COMPACT RIFLE
        [317205821] = 0.2, --AUTO SHOTGUN
        [125959754] = 0.5, --COMPACT LAUNCHER
        [3173288789] = 0.1, --MINI SMG
    },

    pickupList = {
        "PICKUP_AMMO_BULLET_MP",
        "PICKUP_AMMO_FIREWORK",
        "PICKUP_AMMO_FLAREGUN",
        "PICKUP_AMMO_GRENADELAUNCHER",
        "PICKUP_AMMO_GRENADELAUNCHER_MP",
        "PICKUP_AMMO_HOMINGLAUNCHER",
        "PICKUP_AMMO_MG",
        "PICKUP_AMMO_MINIGUN",
        "PICKUP_AMMO_MISSILE_MP",
        "PICKUP_AMMO_PISTOL",
        "PICKUP_AMMO_RIFLE",
        "PICKUP_AMMO_RPG",
        "PICKUP_AMMO_SHOTGUN",
        "PICKUP_AMMO_SMG",
        "PICKUP_AMMO_SNIPER",
        "PICKUP_ARMOUR_STANDARD",
        "PICKUP_CAMERA",
        "PICKUP_CUSTOM_SCRIPT",
        "PICKUP_GANG_ATTACK_MONEY",
        "PICKUP_HEALTH_SNACK",
        "PICKUP_HEALTH_STANDARD",
        "PICKUP_MONEY_CASE",
        "PICKUP_MONEY_DEP_BAG",
        "PICKUP_MONEY_MED_BAG",
        "PICKUP_MONEY_PAPER_BAG",
        "PICKUP_MONEY_PURSE",
        "PICKUP_MONEY_SECURITY_CASE",
        "PICKUP_MONEY_VARIABLE",
        "PICKUP_MONEY_WALLET",
        "PICKUP_PARACHUTE",
        "PICKUP_PORTABLE_CRATE_FIXED_INCAR",
        "PICKUP_PORTABLE_CRATE_UNFIXED",
        "PICKUP_PORTABLE_CRATE_UNFIXED_INCAR",
        "PICKUP_PORTABLE_CRATE_UNFIXED_INCAR_SMALL",
        "PICKUP_PORTABLE_CRATE_UNFIXED_LOW_GLOW",
        "PICKUP_PORTABLE_DLC_VEHICLE_PACKAGE",
        "PICKUP_PORTABLE_PACKAGE",
        "PICKUP_SUBMARINE",
        "PICKUP_VEHICLE_ARMOUR_STANDARD",
        "PICKUP_VEHICLE_CUSTOM_SCRIPT",
        "PICKUP_VEHICLE_CUSTOM_SCRIPT_LOW_GLOW",
        "PICKUP_VEHICLE_HEALTH_STANDARD",
        "PICKUP_VEHICLE_HEALTH_STANDARD_LOW_GLOW",
        "PICKUP_VEHICLE_MONEY_VARIABLE",
        "PICKUP_VEHICLE_WEAPON_APPISTOL",
        "PICKUP_VEHICLE_WEAPON_ASSAULTSMG",
        "PICKUP_VEHICLE_WEAPON_COMBATPISTOL",
        "PICKUP_VEHICLE_WEAPON_GRENADE",
        "PICKUP_VEHICLE_WEAPON_MICROSMG",
        "PICKUP_VEHICLE_WEAPON_MOLOTOV",
        "PICKUP_VEHICLE_WEAPON_PISTOL",
        "PICKUP_VEHICLE_WEAPON_PISTOL50",
        "PICKUP_VEHICLE_WEAPON_SAWNOFF",
        "PICKUP_VEHICLE_WEAPON_SMG",
        "PICKUP_VEHICLE_WEAPON_SMOKEGRENADE",
        "PICKUP_VEHICLE_WEAPON_STICKYBOMB",
        "PICKUP_WEAPON_ADVANCEDRIFLE",
        "PICKUP_WEAPON_APPISTOL",
        "PICKUP_WEAPON_ASSAULTRIFLE",
        "PICKUP_WEAPON_ASSAULTSHOTGUN",
        "PICKUP_WEAPON_ASSAULTSMG",
        "PICKUP_WEAPON_AUTOSHOTGUN",
        "PICKUP_WEAPON_BAT",
        "PICKUP_WEAPON_BATTLEAXE",
        "PICKUP_WEAPON_BOTTLE",
        "PICKUP_WEAPON_BULLPUPRIFLE",
        "PICKUP_WEAPON_BULLPUPSHOTGUN",
        "PICKUP_WEAPON_CARBINERIFLE",
        "PICKUP_WEAPON_COMBATMG",
        "PICKUP_WEAPON_COMBATPDW",
        "PICKUP_WEAPON_COMBATPISTOL",
        "PICKUP_WEAPON_COMPACTLAUNCHER",
        "PICKUP_WEAPON_COMPACTRIFLE",
        "PICKUP_WEAPON_CROWBAR",
        "PICKUP_WEAPON_DAGGER",
        "PICKUP_WEAPON_DBSHOTGUN",
        "PICKUP_WEAPON_FIREWORK",
        "PICKUP_WEAPON_FLAREGUN",
        "PICKUP_WEAPON_FLASHLIGHT",
        "PICKUP_WEAPON_GRENADE",
        "PICKUP_WEAPON_GRENADELAUNCHER",
        "PICKUP_WEAPON_GUSENBERG",
        "PICKUP_WEAPON_GOLFCLUB",
        "PICKUP_WEAPON_HAMMER",
        "PICKUP_WEAPON_HATCHET",
        "PICKUP_WEAPON_HEAVYPISTOL",
        "PICKUP_WEAPON_HEAVYSHOTGUN",
        "PICKUP_WEAPON_HEAVYSNIPER",
        "PICKUP_WEAPON_HOMINGLAUNCHER",
        "PICKUP_WEAPON_KNIFE",
        "PICKUP_WEAPON_KNUCKLE",
        "PICKUP_WEAPON_MACHETE",
        "PICKUP_WEAPON_MACHINEPISTOL",
        "PICKUP_WEAPON_MARKSMANPISTOL",
        "PICKUP_WEAPON_MARKSMANRIFLE",
        "PICKUP_WEAPON_MG",
        "PICKUP_WEAPON_MICROSMG",
        "PICKUP_WEAPON_MINIGUN",
        "PICKUP_WEAPON_MINISMG",
        "PICKUP_WEAPON_MOLOTOV",
        "PICKUP_WEAPON_MUSKET",
        "PICKUP_WEAPON_NIGHTSTICK",
        "PICKUP_WEAPON_PETROLCAN",
        "PICKUP_WEAPON_PIPEBOMB",
        "PICKUP_WEAPON_PISTOL",
        "PICKUP_WEAPON_PISTOL50",
        "PICKUP_WEAPON_POOLCUE",
        "PICKUP_WEAPON_PROXMINE",
        "PICKUP_WEAPON_PUMPSHOTGUN",
        "PICKUP_WEAPON_RAILGUN",
        "PICKUP_WEAPON_REVOLVER",
        "PICKUP_WEAPON_RPG",
        "PICKUP_WEAPON_SAWNOFfsHOTGUN",
        "PICKUP_WEAPON_SMG",
        "PICKUP_WEAPON_SMOKEGRENADE",
        "PICKUP_WEAPON_SNIPERRIFLE",
        "PICKUP_WEAPON_SNSPISTOL",
        "PICKUP_WEAPON_SPECIALCARBINE",
        "PICKUP_WEAPON_STICKYBOMB",
        "PICKUP_WEAPON_STUNGUN",
        "PICKUP_WEAPON_SWITCHBLADE",
        "PICKUP_WEAPON_VINTAGEPISTOL",
        "PICKUP_WEAPON_WRENCH"
    },


    -- qui controlli gli npc in base al meteo
    baseTraffic = 0.9999999, -- traffico base a inizio script (considera la dinamicità del meteo + giorno / notte DEFAULT = 0.9999999)
    divMultiplier = 2.0, -- moltiplicatore del traffico (se vuoi giocaci pure.. tutto cambia in base a come lo imposti... DEFAULT = 2.0)
    -- finiti i negozi.. aggiungere negozi e armerie (armi incluse) :)
}