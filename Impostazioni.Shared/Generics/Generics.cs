using System.Collections.Generic;

namespace Settings.Shared.Config.Generic
{
    public delegate void UseObject(Item oggetto, int quantità);
    public delegate void GiveObject(Item oggetto, int quantità);
    public delegate void ThrowObject(Item oggetto, int quantità);
    public delegate void SellObject(Item oggetto, int quantità);
    public delegate void BuyObject(Item oggetto, int quantità);


    public class SharedGenerics
    {
        #region ItemList
        public Dictionary<string, Item> ItemList = new Dictionary<string, Item>()
        {
            ["yogamatress"] = new Item()
            {
                label = "Yoga matress",
                description = "Namasté",
                peso = 1f,
                sellPrice = 0,
                max = 1,
                prop = (ObjectHash)(-1978741854),
                use = { label = "Place matress", description = "", use = true },
                give = { label = "Give matress", description = "", give = true },
                drop = { label = "Throw away matress", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "Buy matress", description = "", buy = true },
            },
            ["fishingrodbasic"] = new Item()
            {
                label = "Fishing rod for dummies",
                description = "Good for starters",
                peso = 1f,
                sellPrice = 100,
                max = 100,
                prop = (ObjectHash)(-1910604593),
                use = { label = "Use fishing rod", description = "", use = true },
                give = { label = "Give fishing rod", description = "", give = true },
                drop = { label = "Throw away fishing rod", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "Buy fishing rod", description = "", buy = true }
            },
            ["fishingrodinter"] = new()
            {
                label = "Fishing rod for intermediates",
                description = "",
                peso = 1f,
                sellPrice = 100,
                max = 100,
                prop = (ObjectHash)(-1910604593),
                use = { label = "Use fishing rod", description = "", use = true },
                give = { label = "Give fishing rod", description = "", give = true },
                drop = { label = "Throw away fishing rod", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "Buy fishing rod", description = "", buy = true }
            },
            ["fishingrodexpert"] = new()
            {
                label = "Fishing rod for experts",
                description = "",
                peso = 1f,
                sellPrice = 100,
                max = 100,
                prop = (ObjectHash)(-1910604593),
                use = { label = "Use fishing rod", description = "", use = true },
                give = { label = "Give fishing rod", description = "", give = true },
                drop = { label = "Throw away fishing rod", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "Buy fishing rod", description = "", buy = true }
            },


            ["europeanbass"] = new()
            {
                label = "European bass",
                description = "Sea fish",
                peso = 1f,
                sellPrice = 32,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Give european bass", description = "", give = true },
                drop = { label = "Throw away european bass", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["mackerel"] = new()
            {
                label = "Mackerel",
                description = "Sea fish",
                peso = 1f,
                sellPrice = 40,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Give mackerel", description = "", give = true },
                drop = { label = "Throw away mackerel", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["sole"] = new()
            {
                label = "sole",
                description = "Sea fish",
                peso = 1f,
                sellPrice = 25,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Give sole", description = "", give = true },
                drop = { label = "Throw away sole", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["seabream"] = new()
            {
                label = "Sea bream",
                description = "Sea fish",
                peso = 1f,
                sellPrice = 100,
                max = 30,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Give sea bream", description = "", give = true },
                drop = { label = "Throw away sea bream", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["tuna"] = new()
            {
                label = "Tuna",
                description = "Sea fish",
                peso = 1f,
                sellPrice = 100,
                max = 50,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Give tuna", description = "", give = true },
                drop = { label = "Throw away tuna", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["salmon"] = new()
            {
                label = "Salmone",
                description = "Sea fish",
                peso = 1f,
                sellPrice = 26,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Give salmon", description = "", give = true },
                drop = { label = "Throw away salmon", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["codfish"] = new()
            {
                label = "Codfish",
                description = "Sea fish",
                peso = 1f,
                sellPrice = 45,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Give codfish", description = "", give = true },
                drop = { label = "Throw away codfish", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["swordfish"] = new()
            {
                label = "Swordfish",
                description = "Sea fish",
                peso = 1f,
                sellPrice = 56,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Give swordfish", description = "", give = true },
                drop = { label = "Throw away swordfish", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["shark"] = new()
            {
                label = "Shark",
                description = "Sea fish",
                peso = 1f,
                sellPrice = 120,
                max = 100,
                prop = (ObjectHash)(113504370),
                use = { label = "", description = "", use = false },
                give = { label = "Give shark", description = "", give = true },
                drop = { label = "Throw away shark", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },

            ["carp"] = new()
            {
                label = "Carp",
                description = "Lake/river fish",
                peso = 1f,
                sellPrice = 46,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Give carp", description = "", give = true },
                drop = { label = "Throw away carp", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["pike"] = new()
            {
                label = "Pike",
                description = "Lake/river fish",
                peso = 1f,
                sellPrice = 35,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Give pike", description = "", give = true },
                drop = { label = "Throw away pike", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["perch"] = new()
            {
                label = "Perch",
                description = "Lake/river fish",
                peso = 1f,
                sellPrice = 39,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Give perch", description = "", give = true },
                drop = { label = "Throw away perch", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["catfish"] = new()
            {
                label = "Catfish",
                description = "Lake/river fish",
                peso = 1f,
                sellPrice = 40,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Give catfish", description = "", give = true },
                drop = { label = "Throw away catfish", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["speckledcatfish"] = new()
            {
                label = "Speckled catfish",
                description = "Lake/river fish",
                peso = 1f,
                sellPrice = 60,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Give speckled catfish", description = "", give = true },
                drop = { label = "Throw away speckled catfish", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["seabass"] = new()
            {
                label = "Sea ​​bass",
                description = "Lake/river fish",
                peso = 1f,
                sellPrice = 49,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Give sea ​​bass", description = "", give = true },
                drop = { label = "Throw away sea ​​bass", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["trout"] = new()
            {
                label = "Trout",
                description = "Lake/river fish",
                peso = 1f,
                sellPrice = 55,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Give trout", description = "", give = true },
                drop = { label = "Throw away trout", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["goby"] = new()
            {
                label = "Goby",
                description = "Lake/river fish",
                peso = 1f,
                sellPrice = 60,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Give goby", description = "", give = true },
                drop = { label = "Throw away goby", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["zander"] = new()
            {
                label = "Zander",
                description = "Lake/river fish",
                peso = 1f,
                sellPrice = 100,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Give zander", description = "", give = true },
                drop = { label = "Throw away zander", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["bleak"] = new()
            {
                label = "Bleak",
                description = "Lake/river fish",
                peso = 1f,
                sellPrice = 100,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Give bleak", description = "", give = true },
                drop = { label = "Throw away bleak", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["cruciancarp"] = new()
            {
                label = "Crucian carp",
                description = "Lake/river fish",
                peso = 1f,
                sellPrice = 100,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Give crucian carp", description = "", give = true },
                drop = { label = "Throw away crucian carp", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["goldencruciancarp"] = new()
            {
                label = "Golden crucian carp",
                description = "Lake/river fish",
                peso = 1f,
                sellPrice = 100,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Give golden crucian carp", description = "", give = true },
                drop = { label = "Throw away golden crucian carp", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["twaiteshad"] = new()
            {
                label = "Twaite shad",
                description = "Lake/river fish",
                peso = 1f,
                sellPrice = 100,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Give twaite shad", description = "", give = true },
                drop = { label = "Throw away twaite shad", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["rove"] = new()
            {
                label = "Rove",
                description = "Lake/river fish",
                peso = 1f,
                sellPrice = 100,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Give rove", description = "", give = true },
                drop = { label = "Throw away rove", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["stickleback"] = new()
            {
                label = "Stickleback",
                description = "Lake/river fish",
                peso = 1f,
                sellPrice = 100,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Give stickleback", description = "", give = true },
                drop = { label = "Throw away stickleback", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["cobiansturgeon"] = new()
            {
                label = "Cobian sturgeon",
                description = "Lake/river fish",
                peso = 1f,
                sellPrice = 100,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Give cobian sturgeon", description = "", give = true },
                drop = { label = "Throw away cobian sturgeon", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["sturgeon"] = new()
            {
                label = "Sturgeon",
                description = "Lake/river fish",
                peso = 1f,
                sellPrice = 100,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Give sturgeon", description = "", give = true },
                drop = { label = "Throw away sturgeon", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["sturgeonlabdanum"] = new()
            {
                label = "Sturgeon labdanum",
                description = "Lake/river fish",
                peso = 1f,
                sellPrice = 100,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Give sturgeon labdanum", description = "", give = true },
                drop = { label = "Throw away sturgeon labdanum", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },

            ["meatdeer"] = new()
            {
                label = "Deer meat",
                description = "",
                peso = 1f,
                sellPrice = 100,
                max = 100,
                prop = (ObjectHash)(289396019),
                use = { label = "", description = "", use = false },
                give = { label = "Give deer meat", description = "", give = true },
                drop = { label = "Throw away deer meat", description = "", drop = true },
                sell = { label = "Vendi deer meat", description = "", sell = true },
                buy = { label = "Buy deer meat", description = "", buy = true }
            },
            ["meatboar"] = new()
            {
                label = "Boar meat",
                description = "",
                peso = 1f,
                sellPrice = 100,
                max = 100,
                prop = (ObjectHash)(289396019),
                use = { label = "", description = "", use = false },
                give = { label = "Give boar meat", description = "", give = true },
                drop = { label = "Throw away boar meat", description = "", drop = true },
                sell = { label = "Vendi boar meat", description = "", sell = true },
                buy = { label = "Buy boar meat", description = "", buy = true }
            },
            ["meatrabbit"] = new()
            {
                label = "Rabbit meat",
                description = "",
                peso = 1f,
                sellPrice = 100,
                max = 100,
                prop = (ObjectHash)(289396019),
                use = { label = "", description = "", use = false },
                give = { label = "Give rabbit meat", description = "", give = true },
                drop = { label = "Throw away rabbit meat", description = "", drop = true },
                sell = { label = "Vendi rabbit meat", description = "", sell = true },
                buy = { label = "Buy rabbit meat", description = "", buy = true }
            },
            ["meatcoyote"] = new()
            {
                label = "Coyote meat",
                description = "",
                peso = 1f,
                sellPrice = 100,
                max = 100,
                prop = (ObjectHash)(289396019),
                use = { label = "", description = "", use = false },
                give = { label = "Give coyote meat", description = "", give = true },
                drop = { label = "Throw away coyote meat", description = "", drop = true },
                sell = { label = "Vendi coyote meat", description = "", sell = true },
                buy = { label = "Buy coyote meat", description = "", buy = true }
            },
            ["meateagle"] = new()
            {
                label = "Eagle meat",
                description = "",
                peso = 1f,
                sellPrice = 100,
                max = 100,
                prop = (ObjectHash)(289396019),
                use = { label = "", description = "", use = false },
                give = { label = "Give eagle meat", description = "", give = true },
                drop = { label = "Throw away eagle meat", description = "", drop = true },
                sell = { label = "Vendi eagle meat", description = "", sell = true },
                buy = { label = "Buy eagle meat", description = "", buy = true }
            },

            ["hamburger"] = new()
            {
                label = "Hamburger",
                description = "",
                peso = 1f,
                sellPrice = 0,
                max = 2,
                prop = (ObjectHash)(-2054442544),
                use = { label = "Mangia hamburger", description = "Che buono!", use = true },
                give = { label = "Give hamburger", description = "", give = true },
                drop = { label = "Throw away hamburger", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "Buy hamburger", description = "", buy = true }
            },
            ["water"] = new()
            {
                label = "Water bottle",
                description = "Thirst quenching",
                peso = 1f,
                sellPrice = 0,
                max = 2,
                prop = (ObjectHash)(746336278),
                use = { label = "Drink water bottle", description = "Che sete!", use = true },
                give = { label = "Give water bottle", description = "", give = true },
                drop = { label = "Throw away water bottle", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "Buy water bottle", description = "", buy = true }
            },

            ["ModKit"] = new()
            {
                label = "Kit for customizing",
                description = "",
                peso = 1f,
                sellPrice = 0,
                max = 2,
                prop = (ObjectHash)(289396019),
                use = { label = "Install ModKit", description = "Enable vehicle mods", use = true },
                give = { label = "Give ModKit", description = "", give = true },
                drop = { label = "Throw away ModKit", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },

        };
        #endregion

        #region DeathReasons
        public Dictionary<uint, string> DeathReasons = new Dictionary<uint, string>()
        {
            [2460120199] = "Ancient cavalry dagger",
            [2508868239] = "Baseball bat",
            [4192643659] = "Bottle",
            [2227010557] = "Crowbar",
            [2725352035] = "Fist",
            [2343591895] = "Torch",
            [1141786504] = "Golf club",
            [1317494643] = "Hammer",
            [4191993645] = "Axe",
            [3638508604] = "Brass knuckles",
            [2578778090] = "Knife",
            [3713923289] = "Machete",
            [3756226112] = "Claw hammer",
            [1737195953] = "Dustpan",
            [419712736] = "Wrench",
            [3441901897] = "Battle axe",
            [2484171525] = "Pool cue",
            [940833800] = "Stone axe",
            [453432689] = "Pistol",
            [3219281620] = "Pistol MK2",
            [1593441988] = "Combat pistol",
            [584646201] = "AP pistol",
            [911657153] = "Taser",
            [2578377531] = "Pistol .50",
            [3218215474] = "SNS Pistol",
            [2285322324] = "SNS Pistol MK2",
            [3523564046] = "Heavy Pistol",
            [137902532] = "Vintage Pistol",
            [1198879012] = "Flare Gun",
            [3696079510] = "Marksman Pistol",
            [3249783761] = "Heavy Revolver",
            [3415619887] = "Heavy Revolver MK2",
            [2548703416] = "Double Action",
            [2939590305] = "Up-n-Atomizer",
            [324215364] = "Micro SMG",
            [736523883] = "SMG",
            [2024373456] = "SMG MK2",
            [4024951519] = "Assault SMG",
            [171789620] = "Combat PDW",
            [3675956304] = "Machine Pistol",
            [3173288789] = "Mini SMG",
            [1198256469] = "Unholy Hellbringer",
            [487013001] = "Fucile a Pompa",
            [1432025498] = "Pump Shotgun MK2",
            [2017895192] = "Sawed-Off Shotgun",
            [3800352039] = "Assault Shotgun",
            [2640438543] = "Bullpup Shotgun",
            [2828843422] = "Musket",
            [984333226] = "Heavy Shotgun",
            [4019527611] = "Double Barrel Shotgun",
            [317205821] = "Sweeper Shotgun",
            [3220176749] = "Assault Rifle",
            [961495388] = "Assault Rifle MK2",
            [2210333304] = "Carbine Rifle",
            [4208062921] = "Carbine Rifle MK2",
            [2937143193] = "Advanced Rifle",
            [3231910285] = "Special Carbine",
            [2526821735] = "Special Carbine MK2",
            [2132975508] = "Bullpup Rifle",
            [2228681469] = "Bullpup Rifle MK2",
            [1649403952] = "Compact Rifle",
            [2634544996] = "MG",
            [2144741730] = "MG da combattimento",
            [3686625920] = "Combat MG MK2",
            [1627465347] = "Gusenberg Sweeper",
            [100416529] = "Fucile da Cecchino",
            [205991906] = "Fucile da Cecchino pesante",
            [177293209] = "Heavy Sniper MK2",
            [3342088282] = "Marksman Rifle",
            [1785463520] = "Marksman Rifle MK2",
            [2982836145] = "RPG",
            [2726580491] = "LanciaGranate",
            [1305664598] = "LangiaGranate fumogene",
            [1119849093] = "Minigun",
            [2138347493] = "Fuochi d'artificio",
            [1834241177] = "Railgun",
            [1672152130] = "Homing Launcher",
            [125959754] = "Lanciagranate compatto",
            [3056410471] = "Minigun",
            [2481070269] = "Granata",
            [2694266206] = "BZ Gas",
            [4256991824] = "Granata fumogena",
            [1233104067] = "Flare",
            [615608432] = "Molotov",
            [741814745] = "Bomba Adesiva",
            [2874559379] = "Mina di prossimità",
            [126349499] = "Palla di neve",
            [3125143736] = "Tubo Bomba",
            [600439132] = "Baseball",
            [883325847] = "Tanica di Benzina",
            [101631238] = "Estintore",
            [4222310262] = "Paracadute",
            [2461879995] = "Grata Elettrificata",
            [3425972830] = "Hit by Water Cannon",
            [133987706] = "Speronato da una macchina",
            [2741846334] = "Investito da una macchina",
            [3452007600] = "Caduto/Generico/Suicidio",
            [4194021054] = "Animale",
            [324506233] = "Razzo Aereo",
            [2339582971] = "Sanguinato",
            [2294779575] = "Briefcase",
            [28811031] = "Briefcase 02",
            [148160082] = "Giaguaro",
            [1223143800] = "Filo spinato",
            [4284007675] = "Affogato",
            [1936677264] = "Affogato in veicolo",
            [539292904] = "Esplosione",
            [910830060] = "Esaustione",
            [3750660587] = "Fuoco",
            [341774354] = "Crash Elicottero",
            [3204302209] = "Vehicle Rocket",
            [2282558706] = "Vehicle Akula Barrage",
            [431576697] = "Vehicle Akula Minigun",
            [2092838988] = "Vehicle Akula Missile",
            [476907586] = "Vehicle Akula Turret Dual",
            [3048454573] = "Vehicle Akula Turret Single",
            [328167896] = "Vehicle APC Cannon",
            [190244068] = "Vehicle APC MG",
            [1151689097] = "Vehicle APC Missile",
            [3293463361] = "Vehicle Ardent MG",
            [2556895291] = "Vehicle Avenger Cannon",
            [2756453005] = "Vehicle Barrage Rear GL",
            [1200179045] = "Vehicle Barrage Rear MG",
            [525623141] = "Vehicle Barrage Rear Minigun",
            [4148791700] = "Vehicle Barrage Top MG",
            [1000258817] = "Vehicle Barrage Top Minigun",
            [3628350041] = "Vehicle Bombushka Cannon",
            [741027160] = "Vehicle Bombushka Dual MG",
            [3959029566] = "Vehicle Cannon Blazer",
            [1817275304] = "Vehicle Caracara MG",
            [1338760315] = "Vehicle Caracara Minigun",
            [2722615358] = "Vehicle Cherno Missile",
            [3936892403] = "Vehicle Comet MG",
            [2600428406] = "Vehicle Deluxo MG",
            [3036244276] = "Vehicle Deluxo Missile",
            [1595421922] = "Vehicle Dogfighter MG",
            [3393648765] = "Vehicle Dogfighter Missile",
            [2700898573] = "Vehicle Dune Grenade Launcher",
            [3507816399] = "Vehicle Dune MG",
            [1416047217] = "Vehicle Dune Minigun",
            [1566990507] = "Vehicle Enemy Laser",
            [1987049393] = "Vehicle Hacker Missile",
            [2011877270] = "Vehicle Hacker Missile Homing",
            [1331922171] = "Vehicle Halftrack Dual MG",
            [1226518132] = "Vehicle Halftrack Quad MG",
            [855547631] = "Vehicle Havok Minigun",
            [785467445] = "Vehicle Hunter Barrage",
            [704686874] = "Vehicle Hunter Cannon",
            [1119518887] = "Vehicle Hunter MG",
            [153396725] = "Vehicle Hunter Missile",
            [2861067768] = "Vehicle Insurgent Minigun",
            [507170720] = "Vehicle Khanjali Cannon",
            [2206953837] = "Vehicle Khanjali Cannon Heavy",
            [394659298] = "Vehicle Khanjali GL",
            [711953949] = "Vehicle Khanjali MG",
            [3754621092] = "Vehicle Menacer MG",
            [3303022956] = "Vehicle Microlight MG",
            [3846072740] = "Vehicle Mobileops Cannon",
            [3857952303] = "Vehicle Mogul Dual Nose",
            [3123149825] = "Vehicle Mogul Dual Turret",
            [4128808778] = "Vehicle Mogul Nose",
            [3808236382] = "Vehicle Mogul Turret",
            [2220197671] = "Vehicle Mule4 MG",
            [1198717003] = "Vehicle Mule4 Missile",
            [3708963429] = "Vehicle Mule4 Turret GL",
            [2786772340] = "Vehicle Nightshark MG",
            [1097917585] = "Vehicle Nose Turret Valkyrie",
            [3643944669] = "Vehicle Oppressor MG",
            [2344076862] = "Vehicle Oppressor Missile",
            [3595383913] = "Vehicle Oppressor2 Cannon",
            [3796180438] = "Vehicle Oppressor2 MG",
            [1966766321] = "Vehicle Oppressor2 Missile",
            [3473446624] = "Vehicle Plane Rocket",
            [1186503822] = "Vehicle Player Buzzard",
            [3800181289] = "Vehicle Player Lazer",
            [1638077257] = "Vehicle Player Savage",
            [2456521956] = "Vehicle Pounder2 Barrage",
            [2467888918] = "Vehicle Pounder2 GL",
            [2263283790] = "Vehicle Pounder2 Mini",
            [162065050] = "Vehicle Pounder2 Missile",
            [3530961278] = "Vehicle Radar",
            [3177079402] = "Vehicle Revolter MG",
            [3878337474] = "Vehicle Rogue Cannon",
            [158495693] = "Vehicle Rogue MG",
            [1820910717] = "Vehicle Rogue Missile",
            [50118905] = "Vehicle Ruiner Bullet",
            [84788907] = "Vehicle Ruiner Rocket",
            [3946965070] = "Vehicle Savestra MG",
            [231629074] = "Vehicle Scramjet MG",
            [3169388763] = "Vehicle Scramjet Missile",
            [1371067624] = "Vehicle Seabreeze MG",
            [3450622333] = "Vehicle Searchlight",
            [4171469727] = "Vehicle Space Rocket",
            [3355244860] = "Vehicle Speedo4 MG",
            [3595964737] = "Vehicle Speedo4 Turret MG",
            [2667462330] = "Vehicle Speedo4 Turret Mini",
            [968648323] = "Vehicle Strikeforce Barrage",
            [955522731] = "Vehicle Strikeforce Cannon",
            [519052682] = "Vehicle Strikeforce Missile",
            [1176362416] = "Vehicle Subcar MG",
            [3565779982] = "Vehicle Subcar Missile",
            [3884172218] = "Vehicle Subcar Torpedo",
            [1744687076] = "Vehicle Tampa Dual Minigun",
            [3670375085] = "Vehicle Tampa Fixed Minigun",
            [2656583842] = "Vehicle Tampa Missile",
            [1015268368] = "Vehicle Tampa Mortar",
            [1945616459] = "Vehicle Tank",
            [3683206664] = "Vehicle Technical Minigun",
            [1697521053] = "Vehicle Thruster MG",
            [1177935125] = "Vehicle Thruster Missile",
            [2156678476] = "Vehicle Trailer Dualaa",
            [341154295] = "Vehicle Trailer Missile",
            [1192341548] = "Vehicle Trailer Quad MG",
            [2966510603] = "Vehicle Tula Dual MG",
            [1217122433] = "Vehicle Tula MG",
            [376489128] = "Vehicle Tula Minigun",
            [1100844565] = "Vehicle Tula Nose MG",
            [3041872152] = "Vehicle Turret Boxville",
            [1155224728] = "Vehicle Turret Insurgent",
            [729375873] = "Vehicle Turret Limo",
            [2144528907] = "Vehicle Turret Technical",
            [2756787765] = "Vehicle Turret Valkyrie",
            [4094131943] = "Vehicle Vigilante MG",
            [1347266149] = "Vehicle Vigilante Missile",
            [2275421702] = "Vehicle Viseris MG",
            [1150790720] = "Vehicle Volatol Dual MG",
            [1741783703] = "Vehicle Water Cannon"
        };
        #endregion

        #region Weapons
        public Dictionary<string, Weapon> Weapons = new Dictionary<string, Weapon>()
        {
            ["WEAPON_KNIFE"] = new(),
            ["WEAPON_NIGHTSTICK"] = new(),
            ["WEAPON_HAMMER"] = new(),
            ["WEAPON_BAT"] = new(),
            ["WEAPON_GOLFCLUB"] = new(),
            ["WEAPON_CROWBAR"] = new(),
            ["WEAPON_RPG"] = new(),
            ["WEAPON_STINGER"] = new(),
            ["WEAPON_MINIGUN"] = new(),
            ["WEAPON_GRENADE"] = new(),
            ["WEAPON_STICKYBOMB"] = new(),
            ["WEAPON_SMOKEGRENADE"] = new(),
            ["WEAPON_BZGAS"] = new(),
            ["WEAPON_MOLOTOV"] = new(),
            ["WEAPON_FIREEXTINGUISHER"] = new(),
            ["WEAPON_PETROLCAN"] = new(),
            ["WEAPON_DIGISCANNER"] = new(),
            ["WEAPON_BALL"] = new(),
            ["WEAPON_BOTTLE"] = new(),
            ["WEAPON_DAGGER"] = new(),
            ["WEAPON_FIREWORK"] = new(),
            ["WEAPON_MUSKET"] = new(),
            ["WEAPON_STUNGUN"] = new(),
            ["WEAPON_HOMINGLAUNCHER"] = new(),
            ["WEAPON_PROXMINE"] = new(),
            ["WEAPON_SNOWBALL"] = new(),
            ["WEAPON_FLAREGUN"] = new(),
            ["WEAPON_GARBAGEBAG"] = new(),
            ["WEAPON_HANDCUFfs"] = new(),
            ["WEAPON_MARKSMANPISTOL"] = new(),
            ["WEAPON_HATCHET"] = new(),
            ["WEAPON_RAILGUN"] = new(),
            ["WEAPON_MACHETE"] = new(),
            ["WEAPON_DBSHOTGUN"] = new(),
            ["WEAPON_AUTOSHOTGUN"] = new(),
            ["WEAPON_BATTLEAXE"] = new(),
            ["WEAPON_COMPACTLAUNCHER"] = new(),
            ["WEAPON_PIPEBOMB"] = new(),
            ["WEAPON_POOLCUE"] = new(),
            ["WEAPON_WRENCH"] = new(),
            ["WEAPON_FLASHLIGHT"] = new(),
            ["GADGET_NIGHTVISION"] = new(),
            ["WEAPON_FLARE"] = new(),
            ["WEAPON_DOUBLEACTION"] = new(),
            ["WEAPON_PISTOL"] = new()
            {
                components =
                {
                    new() {name = "COMPONENT_PISTOL_CLIP_01", active = true},
                    new() {name = "COMPONENT_PISTOL_CLIP_02", active = false},
                    new() {name = "COMPONENT_AT_PI_FLSH", active = false},
                    new() {name = "COMPONENT_AT_PI_SUPP_02", active = false},
                    new() {name = "COMPONENT_PISTOL_VARMOD_LUXE", active = false}
                },
                tints =
                {
                    new() {name = "WM_TINT0", value = 0}, new() {name = "WM_TINT1", value = 1},
                    new() {name = "WM_TINT2", value = 2}, new() {name = "WM_TINT3", value = 3},
                    new() {name = "WM_TINT4", value = 4}, new() {name = "WM_TINT5", value = 5},
                    new() {name = "WM_TINT6", value = 6}, new() {name = "WM_TINT7", value = 7}
                }
            },
            ["WEAPON_COMBATPISTOL"] = new()
            {
                components =
                {
                    new() {name = "COMPONENT_COMBATPISTOL_CLIP_01", active = true},
                    new() {name = "COMPONENT_COMBATPISTOL_CLIP_02", active = false},
                    new() {name = "COMPONENT_AT_PI_FLSH", active = false},
                    new() {name = "COMPONENT_AT_PI_SUPP", active = false},
                    new() {name = "COMPONENT_COMBATPISTOL_VARMOD_LOWRIDER", active = false}
                },
                tints =
                {
                    new() {name = "WM_TINT0", value = 0}, new() {name = "WM_TINT1", value = 1},
                    new() {name = "WM_TINT2", value = 2}, new() {name = "WM_TINT3", value = 3},
                    new() {name = "WM_TINT4", value = 4}, new() {name = "WM_TINT5", value = 5},
                    new() {name = "WM_TINT6", value = 6}, new() {name = "WM_TINT7", value = 7}
                }
            },
            ["WEAPON_APPISTOL"] = new()
            {
                components =
                {
                    new() {name = "COMPONENT_APPISTOL_CLIP_01", active = true},
                    new() {name = "COMPONENT_APPISTOL_CLIP_02", active = false},
                    new() {name = "COMPONENT_AT_PI_FLSH", active = false},
                    new() {name = "COMPONENT_AT_PI_SUPP_02", active = false},
                    new() {name = "COMPONENT_APPISTOL_VARMOD_LUXE", active = false}
                },
                tints =
                {
                    new() {name = "WM_TINT0", value = 0}, new() {name = "WM_TINT1", value = 1},
                    new() {name = "WM_TINT2", value = 2}, new() {name = "WM_TINT3", value = 3},
                    new() {name = "WM_TINT4", value = 4}, new() {name = "WM_TINT5", value = 5},
                    new() {name = "WM_TINT6", value = 6}, new() {name = "WM_TINT7", value = 7}
                }
            },
            ["WEAPON_PISTOL50"] = new()
            {
                components =
                {
                    new() {name = "COMPONENT_PISTOL50_CLIP_01", active = true},
                    new() {name = "COMPONENT_PISTOL50_CLIP_02", active = false},
                    new() {name = "COMPONENT_AT_PI_FLSH", active = false},
                    new() {name = "COMPONENT_AT_AR_SUPP_02", active = false},
                    new() {name = "COMPONENT_PISTOL50_VARMOD_LUXE", active = false}
                },
                tints =
                {
                    new() {name = "WM_TINT0", value = 0}, new() {name = "WM_TINT1", value = 1},
                    new() {name = "WM_TINT2", value = 2}, new() {name = "WM_TINT3", value = 3},
                    new() {name = "WM_TINT4", value = 4}, new() {name = "WM_TINT5", value = 5},
                    new() {name = "WM_TINT6", value = 6}, new() {name = "WM_TINT7", value = 7}
                }
            },
            ["WEAPON_REVOLVER"] = new()
            {
                tints =
                {
                    new() {name = "WM_TINT0", value = 0}, new() {name = "WM_TINT1", value = 1},
                    new() {name = "WM_TINT2", value = 2}, new() {name = "WM_TINT3", value = 3},
                    new() {name = "WM_TINT4", value = 4}, new() {name = "WM_TINT5", value = 5},
                    new() {name = "WM_TINT6", value = 6}, new() {name = "WM_TINT7", value = 7}
                }
            },
            ["WEAPON_SNSPISTOL"] = new()
            {
                components =
                {
                    new() {name = "COMPONENT_SNSPISTOL_CLIP_01", active = true},
                    new() {name = "COMPONENT_SNSPISTOL_CLIP_02", active = false},
                    new() {name = "COMPONENT_SNSPISTOL_VARMOD_LOWRIDER", active = false}
                },
                tints =
                {
                    new() {name = "WM_TINT0", value = 0}, new() {name = "WM_TINT1", value = 1},
                    new() {name = "WM_TINT2", value = 2}, new() {name = "WM_TINT3", value = 3},
                    new() {name = "WM_TINT4", value = 4}, new() {name = "WM_TINT5", value = 5},
                    new() {name = "WM_TINT6", value = 6}, new() {name = "WM_TINT7", value = 7}
                }
            },
            ["WEAPON_HEAVYPISTOL"] = new()
            {
                components =
                {
                    new() {name = "COMPONENT_HEAVYPISTOL_CLIP_01", active = true},
                    new() {name = "COMPONENT_HEAVYPISTOL_CLIP_02", active = false},
                    new() {name = "COMPONENT_AT_PI_FLSH", active = false},
                    new() {name = "COMPONENT_AT_PI_SUPP", active = false},
                    new() {name = "COMPONENT_HEAVYPISTOL_VARMOD_LUXE", active = false}
                },
                tints =
                {
                    new() {name = "WM_TINT0", value = 0}, new() {name = "WM_TINT1", value = 1},
                    new() {name = "WM_TINT2", value = 2}, new() {name = "WM_TINT3", value = 3},
                    new() {name = "WM_TINT4", value = 4}, new() {name = "WM_TINT5", value = 5},
                    new() {name = "WM_TINT6", value = 6}, new() {name = "WM_TINT7", value = 7}
                }
            },
            ["WEAPON_VINTAGEPISTOL"] = new()
            {
                components =
                {
                    new() {name = "COMPONENT_VINTAGEPISTOL_CLIP_01", active = true},
                    new() {name = "COMPONENT_VINTAGEPISTOL_CLIP_02", active = false},
                    new() {name = "COMPONENT_AT_PI_SUPP", active = false}
                },
                tints =
                {
                    new() {name = "WM_TINT0", value = 0}, new() {name = "WM_TINT1", value = 1},
                    new() {name = "WM_TINT2", value = 2}, new() {name = "WM_TINT3", value = 3},
                    new() {name = "WM_TINT4", value = 4}, new() {name = "WM_TINT5", value = 5},
                    new() {name = "WM_TINT6", value = 6}, new() {name = "WM_TINT7", value = 7}
                }
            },
            ["WEAPON_MICROSMG"] = new()
            {
                components =
                {
                    new() {name = "COMPONENT_MICROSMG_CLIP_01", active = true},
                    new() {name = "COMPONENT_MICROSMG_CLIP_02", active = false},
                    new() {name = "COMPONENT_AT_PI_FLSH", active = false},
                    new() {name = "COMPONENT_AT_SCOPE_MACRO", active = false},
                    new() {name = "COMPONENT_AT_AR_SUPP_02", active = false},
                    new() {name = "COMPONENT_MICROSMG_VARMOD_LUXE", active = false}
                },
                tints =
                {
                    new() {name = "WM_TINT0", value = 0}, new() {name = "WM_TINT1", value = 1},
                    new() {name = "WM_TINT2", value = 2}, new() {name = "WM_TINT3", value = 3},
                    new() {name = "WM_TINT4", value = 4}, new() {name = "WM_TINT5", value = 5},
                    new() {name = "WM_TINT6", value = 6}, new() {name = "WM_TINT7", value = 7}
                }
            },
            ["WEAPON_SMG"] = new()
            {
                components =
                {
                    new() {name = "COMPONENT_SMG_CLIP_01", active = true},
                    new() {name = "COMPONENT_SMG_CLIP_01", active = false},
                    new() {name = "COMPONENT_SMG_CLIP_03", active = false},
                    new() {name = "COMPONENT_AT_AR_FLSH", active = false},
                    new() {name = "COMPONENT_AT_SCOPE_MACRO_02", active = false},
                    new() {name = "COMPONENT_AT_PI_SUPP", active = false},
                    new() {name = "COMPONENT_SMG_VARMOD_LUXE", active = false}
                },
                tints =
                {
                    new() {name = "WM_TINT0", value = 0}, new() {name = "WM_TINT1", value = 1},
                    new() {name = "WM_TINT2", value = 2}, new() {name = "WM_TINT3", value = 3},
                    new() {name = "WM_TINT4", value = 4}, new() {name = "WM_TINT5", value = 5},
                    new() {name = "WM_TINT6", value = 6}, new() {name = "WM_TINT7", value = 7}
                }
            },
            ["WEAPON_ASSAULTSMG"] = new()
            {
                components =
                {
                    new() {name = "COMPONENT_ASSAULTSMG_CLIP_01", active = true},
                    new() {name = "COMPONENT_ASSAULTSMG_CLIP_02", active = false},
                    new() {name = "COMPONENT_AT_AR_FLSH", active = false},
                    new() {name = "COMPONENT_AT_PI_SUPP_02", active = false},
                    new() {name = "COMPONENT_AT_SCOPE_MACRO", active = false},
                    new() {name = "COMPONENT_AT_AR_SUPP_02", active = false},
                    new() {name = "COMPONENT_ASSAULTSMG_VARMOD_LOWRIDER", active = false}
                },
                tints =
                {
                    new() {name = "WM_TINT0", value = 0}, new() {name = "WM_TINT1", value = 1},
                    new() {name = "WM_TINT2", value = 2}, new() {name = "WM_TINT3", value = 3},
                    new() {name = "WM_TINT4", value = 4}, new() {name = "WM_TINT5", value = 5},
                    new() {name = "WM_TINT6", value = 6}, new() {name = "WM_TINT7", value = 7}
                }
            },
            ["WEAPON_MINISMG"] = new()
            {
                components =
                {
                    new() {name = "COMPONENT_MINISMG_CLIP_01", active = true},
                    new() {name = "COMPONENT_MINISMG_CLIP_02", active = false}
                },
                tints =
                {
                    new() {name = "WM_TINT0", value = 0}, new() {name = "WM_TINT1", value = 1},
                    new() {name = "WM_TINT2", value = 2}, new() {name = "WM_TINT3", value = 3},
                    new() {name = "WM_TINT4", value = 4}, new() {name = "WM_TINT5", value = 5},
                    new() {name = "WM_TINT6", value = 6}, new() {name = "WM_TINT7", value = 7}
                }
            },
            ["WEAPON_MACHINEPISTOL"] = new()
            {
                components =
                {
                    new() {name = "COMPONENT_MACHINEPISTOL_CLIP_01", active = true},
                    new() {name = "COMPONENT_MACHINEPISTOL_CLIP_02", active = false},
                    new() {name = "COMPONENT_MACHINEPISTOL_CLIP_03", active = false},
                    new() {name = "COMPONENT_AT_PI_SUPP", active = false}
                },
                tints =
                {
                    new() {name = "WM_TINT0", value = 0}, new() {name = "WM_TINT1", value = 1},
                    new() {name = "WM_TINT2", value = 2}, new() {name = "WM_TINT3", value = 3},
                    new() {name = "WM_TINT4", value = 4}, new() {name = "WM_TINT5", value = 5},
                    new() {name = "WM_TINT6", value = 6}, new() {name = "WM_TINT7", value = 7}
                }
            },
            ["WEAPON_COMBATPDW"] = new()
            {
                components =
                {
                    new() {name = "COMPONENT_COMBATPDW_CLIP_01", active = true},
                    new() {name = "COMPONENT_COMBATPDW_CLIP_02", active = false},
                    new() {name = "COMPONENT_COMBATPDW_CLIP_03", active = false},
                    new() {name = "COMPONENT_AT_AR_FLSH", active = false},
                    new() {name = "COMPONENT_AT_AR_AFGRIP", active = false},
                    new() {name = "COMPONENT_AT_SCOPE_SMALL", active = false}
                },
                tints =
                {
                    new() {name = "WM_TINT0", value = 0}, new() {name = "WM_TINT1", value = 1},
                    new() {name = "WM_TINT2", value = 2}, new() {name = "WM_TINT3", value = 3},
                    new() {name = "WM_TINT4", value = 4}, new() {name = "WM_TINT5", value = 5},
                    new() {name = "WM_TINT6", value = 6}, new() {name = "WM_TINT7", value = 7}
                }
            },
            ["WEAPON_PUMPSHOTGUN"] = new()
            {
                components =
                {
                    new() {name = "COMPONENT_AT_AR_FLSH", active = false},
                    new() {name = "COMPONENT_AT_SR_SUPP", active = false},
                    new() {name = "COMPONENT_PUMPSHOTGUN_VARMOD_LOWRIDER", active = false}
                },
                tints =
                {
                    new() {name = "WM_TINT0", value = 0}, new() {name = "WM_TINT1", value = 1},
                    new() {name = "WM_TINT2", value = 2}, new() {name = "WM_TINT3", value = 3},
                    new() {name = "WM_TINT4", value = 4}, new() {name = "WM_TINT5", value = 5},
                    new() {name = "WM_TINT6", value = 6}, new() {name = "WM_TINT7", value = 7}
                }
            },
            ["WEAPON_SAWNOFfsHOTGUN"] = new()
            {
                components = { new() { name = "COMPONENT_SAWNOFfsHOTGUN_VARMOD_LUXE", active = false } },
                tints =
                {
                    new() {name = "WM_TINT0", value = 0}, new() {name = "WM_TINT1", value = 1},
                    new() {name = "WM_TINT2", value = 2}, new() {name = "WM_TINT3", value = 3},
                    new() {name = "WM_TINT4", value = 4}, new() {name = "WM_TINT5", value = 5},
                    new() {name = "WM_TINT6", value = 6}, new() {name = "WM_TINT7", value = 7}
                }
            },
            ["WEAPON_ASSAULTSHOTGUN"] = new()
            {
                components =
                {
                    new() {name = "COMPONENT_ASSAULTSHOTGUN_CLIP_01", active = true},
                    new() {name = "COMPONENT_ASSAULTSHOTGUN_CLIP_02", active = false},
                    new() {name = "COMPONENT_AT_AR_FLSH", active = false},
                    new() {name = "COMPONENT_AT_AR_SUPP", active = false},
                    new() {name = "COMPONENT_AT_AR_AFGRIP", active = false}
                },
                tints =
                {
                    new() {name = "WM_TINT0", value = 0}, new() {name = "WM_TINT1", value = 1},
                    new() {name = "WM_TINT2", value = 2}, new() {name = "WM_TINT3", value = 3},
                    new() {name = "WM_TINT4", value = 4}, new() {name = "WM_TINT5", value = 5},
                    new() {name = "WM_TINT6", value = 6}, new() {name = "WM_TINT7", value = 7}
                }
            },
            ["WEAPON_BULLPUPSHOTGUN"] = new()
            {
                components =
                {
                    new() {name = "COMPONENT_AT_AR_FLSH", active = false},
                    new() {name = "COMPONENT_AT_AR_SUPP_02", active = false},
                    new() {name = "COMPONENT_AT_AR_AFGRIP", active = false}
                },
                tints =
                {
                    new() {name = "WM_TINT0", value = 0}, new() {name = "WM_TINT1", value = 1},
                    new() {name = "WM_TINT2", value = 2}, new() {name = "WM_TINT3", value = 3},
                    new() {name = "WM_TINT4", value = 4}, new() {name = "WM_TINT5", value = 5},
                    new() {name = "WM_TINT6", value = 6}, new() {name = "WM_TINT7", value = 7}
                }
            },
            ["WEAPON_HEAVYSHOTGUN"] = new()
            {
                components =
                {
                    new() {name = "COMPONENT_HEAVYSHOTGUN_CLIP_01", active = true},
                    new() {name = "COMPONENT_HEAVYSHOTGUN_CLIP_02", active = false},
                    new() {name = "COMPONENT_HEAVYSHOTGUN_CLIP_03", active = false},
                    new() {name = "COMPONENT_AT_AR_FLSH", active = false},
                    new() {name = "COMPONENT_AT_AR_SUPP_02", active = false},
                    new() {name = "COMPONENT_AT_AR_AFGRIP", active = false}
                },
                tints =
                {
                    new() {name = "WM_TINT0", value = 0}, new() {name = "WM_TINT1", value = 1},
                    new() {name = "WM_TINT2", value = 2}, new() {name = "WM_TINT3", value = 3},
                    new() {name = "WM_TINT4", value = 4}, new() {name = "WM_TINT5", value = 5},
                    new() {name = "WM_TINT6", value = 6}, new() {name = "WM_TINT7", value = 7}
                }
            },
            ["WEAPON_ASSAULTRIFLE"] = new()
            {
                components =
                {
                    new() {name = "COMPONENT_ASSAULTRIFLE_CLIP_01", active = true},
                    new() {name = "COMPONENT_ASSAULTRIFLE_CLIP_02", active = false},
                    new() {name = "COMPONENT_ASSAULTRIFLE_CLIP_03", active = false},
                    new() {name = "COMPONENT_AT_AR_FLSH", active = false},
                    new() {name = "COMPONENT_AT_SCOPE_MACRO", active = false},
                    new() {name = "COMPONENT_AT_AR_SUPP_02", active = false},
                    new() {name = "COMPONENT_AT_AR_AFGRIP", active = false},
                    new() {name = "COMPONENT_ASSAULTRIFLE_VARMOD_LUXE", active = false}
                },
                tints =
                {
                    new() {name = "WM_TINT0", value = 0}, new() {name = "WM_TINT1", value = 1},
                    new() {name = "WM_TINT2", value = 2}, new() {name = "WM_TINT3", value = 3},
                    new() {name = "WM_TINT4", value = 4}, new() {name = "WM_TINT5", value = 5},
                    new() {name = "WM_TINT6", value = 6}, new() {name = "WM_TINT7", value = 7}
                }
            },
            ["WEAPON_CARBINERIFLE"] = new()
            {
                components =
                {
                    new() {name = "COMPONENT_CARBINERIFLE_CLIP_01", active = true},
                    new() {name = "COMPONENT_CARBINERIFLE_CLIP_02", active = false},
                    new() {name = "COMPONENT_CARBINERIFLE_CLIP_03", active = false},
                    new() {name = "COMPONENT_AT_AR_FLSH", active = false},
                    new() {name = "COMPONENT_AT_SCOPE_MEDIUM", active = false},
                    new() {name = "COMPONENT_AT_AR_SUPP", active = false},
                    new() {name = "COMPONENT_AT_AR_AFGRIP", active = false},
                    new() {name = "COMPONENT_CARBINERIFLE_VARMOD_LUXE", active = false}
                },
                tints =
                {
                    new() {name = "WM_TINT0", value = 0}, new() {name = "WM_TINT1", value = 1},
                    new() {name = "WM_TINT2", value = 2}, new() {name = "WM_TINT3", value = 3},
                    new() {name = "WM_TINT4", value = 4}, new() {name = "WM_TINT5", value = 5},
                    new() {name = "WM_TINT6", value = 6}, new() {name = "WM_TINT7", value = 7}
                }
            },
            ["WEAPON_ADVANCEDRIFLE"] = new()
            {
                components =
                {
                    new() {name = "COMPONENT_ADVANCEDRIFLE_CLIP_01", active = true},
                    new() {name = "COMPONENT_ADVANCEDRIFLE_CLIP_02", active = false},
                    new() {name = "COMPONENT_AT_AR_FLSH", active = false},
                    new() {name = "COMPONENT_AT_SCOPE_SMALL", active = false},
                    new() {name = "COMPONENT_AT_AR_SUPP", active = false},
                    new() {name = "COMPONENT_ADVANCEDRIFLE_VARMOD_LUXE", active = false}
                },
                tints =
                {
                    new() {name = "WM_TINT0", value = 0}, new() {name = "WM_TINT1", value = 1},
                    new() {name = "WM_TINT2", value = 2}, new() {name = "WM_TINT3", value = 3},
                    new() {name = "WM_TINT4", value = 4}, new() {name = "WM_TINT5", value = 5},
                    new() {name = "WM_TINT6", value = 6}, new() {name = "WM_TINT7", value = 7}
                }
            },
            ["WEAPON_SPECIALCARBINE"] = new()
            {
                components =
                {
                    new() {name = "COMPONENT_SPECIALCARBINE_CLIP_01", active = true},
                    new() {name = "COMPONENT_SPECIALCARBINE_CLIP_02", active = false},
                    new() {name = "COMPONENT_SPECIALCARBINE_CLIP_03", active = false},
                    new() {name = "COMPONENT_AT_AR_FLSH", active = false},
                    new() {name = "COMPONENT_AT_SCOPE_MEDIUM", active = false},
                    new() {name = "COMPONENT_AT_AR_SUPP_02", active = false},
                    new() {name = "COMPONENT_AT_AR_AFGRIP", active = false},
                    new() {name = "COMPONENT_SPECIALCARBINE_VARMOD_LOWRIDER", active = false}
                },
                tints =
                {
                    new() {name = "WM_TINT0", value = 0}, new() {name = "WM_TINT1", value = 1},
                    new() {name = "WM_TINT2", value = 2}, new() {name = "WM_TINT3", value = 3},
                    new() {name = "WM_TINT4", value = 4}, new() {name = "WM_TINT5", value = 5},
                    new() {name = "WM_TINT6", value = 6}, new() {name = "WM_TINT7", value = 7}
                }
            },
            ["WEAPON_BULLPUPRIFLE"] = new()
            {
                components =
                {
                    new() {name = "COMPONENT_BULLPUPRIFLE_CLIP_01", active = true},
                    new() {name = "COMPONENT_BULLPUPRIFLE_CLIP_02", active = false},
                    new() {name = "COMPONENT_AT_AR_FLSH", active = false},
                    new() {name = "COMPONENT_AT_SCOPE_SMALL", active = false},
                    new() {name = "COMPONENT_AT_AR_SUPP", active = false},
                    new() {name = "COMPONENT_AT_AR_AFGRIP", active = false},
                    new() {name = "COMPONENT_BULLPUPRIFLE_VARMOD_LOW", active = false}
                },
                tints =
                {
                    new() {name = "WM_TINT0", value = 0}, new() {name = "WM_TINT1", value = 1},
                    new() {name = "WM_TINT2", value = 2}, new() {name = "WM_TINT3", value = 3},
                    new() {name = "WM_TINT4", value = 4}, new() {name = "WM_TINT5", value = 5},
                    new() {name = "WM_TINT6", value = 6}, new() {name = "WM_TINT7", value = 7}
                }
            },
            ["WEAPON_COMPACTRIFLE"] = new()
            {
                components =
                {
                    new() {name = "COMPONENT_COMPACTRIFLE_CLIP_01", active = true},
                    new() {name = "COMPONENT_COMPACTRIFLE_CLIP_02", active = false},
                    new() {name = "COMPONENT_COMPACTRIFLE_CLIP_03", active = false}
                },
                tints =
                {
                    new() {name = "WM_TINT0", value = 0}, new() {name = "WM_TINT1", value = 1},
                    new() {name = "WM_TINT2", value = 2}, new() {name = "WM_TINT3", value = 3},
                    new() {name = "WM_TINT4", value = 4}, new() {name = "WM_TINT5", value = 5},
                    new() {name = "WM_TINT6", value = 6}, new() {name = "WM_TINT7", value = 7}
                }
            },
            ["WEAPON_MG"] = new()
            {
                components =
                {
                    new() {name = "COMPONENT_MG_CLIP_01", active = true},
                    new() {name = "COMPONENT_MG_CLIP_02", active = false},
                    new() {name = "COMPONENT_AT_SCOPE_SMALL_02", active = false},
                    new() {name = "COMPONENT_MG_VARMOD_LOWRIDER", active = false}
                },
                tints =
                {
                    new() {name = "WM_TINT0", value = 0}, new() {name = "WM_TINT1", value = 1},
                    new() {name = "WM_TINT2", value = 2}, new() {name = "WM_TINT3", value = 3},
                    new() {name = "WM_TINT4", value = 4}, new() {name = "WM_TINT5", value = 5},
                    new() {name = "WM_TINT6", value = 6}, new() {name = "WM_TINT7", value = 7}
                }
            },
            ["WEAPON_COMBATMG"] = new()
            {
                components =
                {
                    new() {name = "COMPONENT_COMBATMG_CLIP_01", active = true},
                    new() {name = "COMPONENT_COMBATMG_CLIP_02", active = false},
                    new() {name = "COMPONENT_AT_SCOPE_MEDIUM", active = false},
                    new() {name = "COMPONENT_AT_AR_AFGRIP", active = false},
                    new() {name = "COMPONENT_COMBATMG_VARMOD_LOWRIDER", active = false}
                },
                tints =
                {
                    new() {name = "WM_TINT0", value = 0}, new() {name = "WM_TINT1", value = 1},
                    new() {name = "WM_TINT2", value = 2}, new() {name = "WM_TINT3", value = 3},
                    new() {name = "WM_TINT4", value = 4}, new() {name = "WM_TINT5", value = 5},
                    new() {name = "WM_TINT6", value = 6}, new() {name = "WM_TINT7", value = 7}
                }
            },
            ["WEAPON_GUSENBERG"] = new()
            {
                components =
                {
                    new() {name = "COMPONENT_GUSENBERG_CLIP_01", active = true},
                    new() {name = "COMPONENT_GUSENBERG_CLIP_02", active = false}
                },
                tints =
                {
                    new() {name = "WM_TINT0", value = 0}, new() {name = "WM_TINT1", value = 1},
                    new() {name = "WM_TINT2", value = 2}, new() {name = "WM_TINT3", value = 3},
                    new() {name = "WM_TINT4", value = 4}, new() {name = "WM_TINT5", value = 5},
                    new() {name = "WM_TINT6", value = 6}, new() {name = "WM_TINT7", value = 7}
                }
            },
            ["WEAPON_SNIPERRIFLE"] = new()
            {
                components =
                {
                    new() {name = "COMPONENT_AT_SCOPE_LARGE", active = false},
                    new() {name = "COMPONENT_AT_SCOPE_MAX", active = false},
                    new() {name = "COMPONENT_AT_AR_SUPP_02", active = false},
                    new() {name = "COMPONENT_SNIPERRIFLE_VARMOD_LUXE", active = false}
                },
                tints =
                {
                    new() {name = "WM_TINT0", value = 0}, new() {name = "WM_TINT1", value = 1},
                    new() {name = "WM_TINT2", value = 2}, new() {name = "WM_TINT3", value = 3},
                    new() {name = "WM_TINT4", value = 4}, new() {name = "WM_TINT5", value = 5},
                    new() {name = "WM_TINT6", value = 6}, new() {name = "WM_TINT7", value = 7}
                }
            },
            ["WEAPON_HEAVYSNIPER"] = new()
            {
                components =
                {
                    new() {name = "COMPONENT_AT_SCOPE_LARGE", active = false},
                    new() {name = "COMPONENT_AT_SCOPE_MAX", active = false}
                },
                tints =
                {
                    new() {name = "WM_TINT0", value = 0}, new() {name = "WM_TINT1", value = 1},
                    new() {name = "WM_TINT2", value = 2}, new() {name = "WM_TINT3", value = 3},
                    new() {name = "WM_TINT4", value = 4}, new() {name = "WM_TINT5", value = 5},
                    new() {name = "WM_TINT6", value = 6}, new() {name = "WM_TINT7", value = 7}
                }
            },
            ["WEAPON_MARKSMANRIFLE"] = new()
            {
                components =
                {
                    new() {name = "COMPONENT_MARKSMANRIFLE_CLIP_01", active = true},
                    new() {name = "COMPONENT_MARKSMANRIFLE_CLIP_02", active = false},
                    new() {name = "COMPONENT_AT_AR_FLSH", active = false},
                    new() {name = "COMPONENT_AT_SCOPE_LARGE_FIXED_ZOOM", active = false},
                    new() {name = "COMPONENT_AT_AR_SUPP", active = false},
                    new() {name = "COMPONENT_AT_AR_AFGRIP", active = false},
                    new() {name = "COMPONENT_MARKSMANRIFLE_VARMOD_LUXE", active = false}
                },
                tints =
                {
                    new() {name = "WM_TINT0", value = 0}, new() {name = "WM_TINT1", value = 1},
                    new() {name = "WM_TINT2", value = 2}, new() {name = "WM_TINT3", value = 3},
                    new() {name = "WM_TINT4", value = 4}, new() {name = "WM_TINT5", value = 5},
                    new() {name = "WM_TINT6", value = 6}, new() {name = "WM_TINT7", value = 7}
                }
            },
            ["WEAPON_GRENADELAUNCHER"] = new()
            {
                components =
                {
                    new() {name = "COMPONENT_AT_AR_FLSH", active = false},
                    new() {name = "COMPONENT_AT_AR_AFGRIP", active = false},
                    new() {name = "COMPONENT_AT_SCOPE_SMALL", active = false}
                },
                tints =
                {
                    new() {name = "WM_TINT0", value = 0}, new() {name = "WM_TINT1", value = 1},
                    new() {name = "WM_TINT2", value = 2}, new() {name = "WM_TINT3", value = 3},
                    new() {name = "WM_TINT4", value = 4}, new() {name = "WM_TINT5", value = 5},
                    new() {name = "WM_TINT6", value = 6}, new() {name = "WM_TINT7", value = 7}
                }
            },
            ["WEAPON_RPG"] = new()
            {
                tints =
                {
                    new() {name = "WM_TINT0", value = 0}, new() {name = "WM_TINT1", value = 1},
                    new() {name = "WM_TINT2", value = 2}, new() {name = "WM_TINT3", value = 3},
                    new() {name = "WM_TINT4", value = 4}, new() {name = "WM_TINT5", value = 5},
                    new() {name = "WM_TINT6", value = 6}, new() {name = "WM_TINT7", value = 7}
                }
            },
            ["WEAPON_STINGER"] = new()
            {
                tints =
                {
                    new() {name = "WM_TINT0", value = 0}, new() {name = "WM_TINT1", value = 1},
                    new() {name = "WM_TINT2", value = 2}, new() {name = "WM_TINT3", value = 3},
                    new() {name = "WM_TINT4", value = 4}, new() {name = "WM_TINT5", value = 5},
                    new() {name = "WM_TINT6", value = 6}, new() {name = "WM_TINT7", value = 7}
                }
            },
            ["WEAPON_MINIGUN"] = new()
            {
                tints =
                {
                    new() {name = "WM_TINT0", value = 0}, new() {name = "WM_TINT1", value = 1},
                    new() {name = "WM_TINT2", value = 2}, new() {name = "WM_TINT3", value = 3},
                    new() {name = "WM_TINT4", value = 4}, new() {name = "WM_TINT5", value = 5},
                    new() {name = "WM_TINT6", value = 6}, new() {name = "WM_TINT7", value = 7}
                }
            },
            ["WEAPON_KNUCKLE"] = new()
            {
                components =
                {
                    new() {name = "COMPONENT_KNUCKLE_VARMOD_BASE", active = true},
                    new() {name = "COMPONENT_KNUCKLE_VARMOD_PIMP", active = false},
                    new() {name = "COMPONENT_KNUCKLE_VARMOD_BALLAS", active = false},
                    new() {name = "COMPONENT_KNUCKLE_VARMOD_DOLLAR", active = false},
                    new() {name = "COMPONENT_KNUCKLE_VARMOD_DIAMOND", active = false},
                    new() {name = "COMPONENT_KNUCKLE_VARMOD_HATE", active = false},
                    new() {name = "COMPONENT_KNUCKLE_VARMOD_LOVE", active = false},
                    new() {name = "COMPONENT_KNUCKLE_VARMOD_PLAYER", active = false},
                    new() {name = "COMPONENT_KNUCKLE_VARMOD_KING", active = false},
                    new() {name = "COMPONENT_KNUCKLE_VARMOD_VAGOS", active = false}
                },
                tints =
                {
                    new() {name = "WM_TINT0", value = 0}, new() {name = "WM_TINT1", value = 1},
                    new() {name = "WM_TINT2", value = 2}, new() {name = "WM_TINT3", value = 3},
                    new() {name = "WM_TINT4", value = 4}, new() {name = "WM_TINT5", value = 5},
                    new() {name = "WM_TINT6", value = 6}, new() {name = "WM_TINT7", value = 7}
                }
            },
        };
        #endregion
    }



    public class Weapon
    {
        public string? name;
        public List<Components> components = new List<Components>();
        public List<Tints> tints = new List<Tints>();
        public Weapon() { }
        public Weapon(string _name, List<Components> _comp, List<Tints> _tints)
        {
            name = _name;
            components = _comp;
            tints = _tints;
        }
    }


    public class Item
    {
        public string label;
        public string description;
        public float peso;
        public int sellPrice;
        public int max;
        public ObjectHash prop;
        public Use use = new Use();
        public Give give = new Give();
        public Drop drop = new Drop();
        public Sell sell = new Sell();
        public Buy buy = new Buy();


        public event UseObject Use;
        public event GiveObject Give;
        public event ThrowObject Throw;
        public event SellObject Sell;
        public event BuyObject Buy;

        public void UseObjectEvent(int quantity)
        {
            Use.Invoke(this, quantity);
        }

        public void GiveObjectEvent(int quantity)
        {
            Give.Invoke(this, quantity);
        }

        public void ThrowObjectEvent(int quantity)
        {
            Throw.Invoke(this, quantity);
        }

        public void SellObjectEvent(int quantity)
        {
            Sell.Invoke(this, quantity);
        }

        public void BuyObjectEvent(int quantity)
        {
            Buy.Invoke(this, quantity);
        }
        public Item() { }
        public Item(string _label, string _desc, float _peso, int _sellpr, int _max, ObjectHash _prop, Use _use, Give _give, Drop _drop, Sell _sell, Buy _buy)
        {
            label = _label;
            description = _desc;
            peso = _peso;
            sellPrice = _sellpr;
            max = _max;
            prop = _prop;
            use = _use;
            give = _give;
            drop = _drop;
            sell = _sell;
            buy = _buy;
        }
    }


    public class Use
    {
        public string label;
        public string description;
        public bool use;

        public Use() { }
        public Use(string _label, string _description, bool _use)
        {
            this.label = _label;
            this.description = _description;
            this.use = _use;
        }
    }


    public class Give
    {
        public string label;
        public string description;
        public bool give;
        public Give() { }
        public Give(string _label, string _description, bool _give)
        {
            this.label = _label;
            this.description = _description;
            this.give = _give;
        }
    }


    public class Drop
    {
        public string label;
        public string description;
        public bool drop;

        public Drop() { }
        public Drop(string _label, string _description, bool _drop)
        {
            this.label = _label;
            this.description = _description;
            this.drop = _drop;
        }
    }


    public class Sell
    {
        public string label;
        public string description;
        public bool sell;

        public Sell() { }
        public Sell(string _label, string _description, bool _sell)
        {
            this.label = _label;
            this.description = _description;
            this.sell = _sell;
        }
    }


    public class Buy
    {
        public string label;
        public string description;
        public bool buy;

        public Buy() { }
        public Buy(string _label, string _description, bool _buy)
        {
            this.label = _label;
            this.description = _description;
            this.buy = _buy;
        }
    }


    public class Components
    {
        public string name { get; set; }
        public bool active { get; set; }
        public Components() { }
        public Components(string _name)
        {
            name = _name;
            active = false;
        }
        public Components(string _name, bool _ac)
        {
            this.name = _name;
            this.active = _ac;
        }
    }


    public class Tints
    {
        public string name { get; set; }
        public int value { get; set; }
        public Tints() { }
        public Tints(string _name, int _value)
        {
            this.name = _name;
            this.value = _value;
        }
    }
}