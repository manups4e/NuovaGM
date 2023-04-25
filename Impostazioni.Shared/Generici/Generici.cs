using System.Collections.Generic;
using TheLastPlanet.Shared;
using FxEvents.Shared.Attributes;

namespace Impostazioni.Shared.Configurazione.Generici
{
    public delegate void UsaOggetto(Item oggetto, int quantità);
    public delegate void DaiOggetto(Item oggetto, int quantità);
    public delegate void ButtaOggetto(Item oggetto, int quantità);
    public delegate void VendiOggetto(Item oggetto, int quantità);
    public delegate void CompraOggetto(Item oggetto, int quantità);

    
    public class SharedGenerici
    {
        #region ItemList
        public Dictionary<string, Item> ItemList = new Dictionary<string, Item>()
        {
            ["materassinoyoga"] = new Item()
            {
                label = "Materassino da yoga",
                description = "Namasté",
                peso = 1f,
                sellPrice = 0,
                max = 1,
                prop = (ObjectHash)(-1978741854),
                use = { label = "Piazza materassino", description = "", use = true },
                give = { label = "Dai materassino", description = "A chi vuoi tu!!", give = true },
                drop = { label = "Getta via materassino", description = "non ti serve piu?", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "Compra materassino", description = "", buy = true },
            },
            ["cannadapescabase"] = new Item()
            {
                label = "canna da pesca per principianti",
                description = "Ottima per iniziare",
                peso = 1f,
                sellPrice = 100,
                max = 100,
                prop = (ObjectHash)(-1910604593),
                use = { label = "Usa la canna da pesca", description = "Per pescare!", use = true },
                give = { label = "Dai una canna da pesca", description = "A chi vuoi tu!!", give = true },
                drop = { label = "Getta via la canna da pesca", description = "non ti serve piu?", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "Compra una canna da pesca base", description = "Per i principianti", buy = true }
            },
            ["cannadapescamedia"] = new()
            {
                label = "canna da pesca per intermedi",
                description = "Per pescatori mediamente bravi",
                peso = 1f,
                sellPrice = 100,
                max = 100,
                prop = (ObjectHash)(-1910604593),
                use = { label = "Usa la canna da pesca", description = "Per pescare!", use = true },
                give = { label = "Dai una canna da pesca", description = "A chi vuoi tu!!", give = true },
                drop = { label = "Getta via la canna da pesca", description = "non ti serve piu?", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "Compra una canna da pesca intermedia", description = "Per gli intermedi", buy = true }
            },
            ["cannadapescaavanzata"] = new()
            {
                label = "canna da pesca per avanzati",
                description = "Per pescatori avanzati",
                peso = 1f,
                sellPrice = 100,
                max = 100,
                prop = (ObjectHash)(-1910604593),
                use = { label = "Usa la canna da pesca", description = "Per pescare!", use = true },
                give = { label = "Dai una canna da pesca", description = "A chi vuoi tu!!", give = true },
                drop = { label = "Getta via la canna da pesca", description = "non ti serve piu?", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "Compra una canna da pesca avanzata", description = "Per i veri pro", buy = true }
            },


            ["branzino"] = new()
            {
                label = "Branzino",
                description = "Pesce di mare",
                peso = 1f,
                sellPrice = 32,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Dai un branzino", description = "A chi vuoi tu!!", give = true },
                drop = { label = "Getta via un branzino", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["sgombro"] = new()
            {
                label = "Sgombro",
                description = "Pesce di mare",
                peso = 1f,
                sellPrice = 40,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Dai uno sgombro", description = "A chi vuoi tu!!", give = true },
                drop = { label = "Getta via uno sgombro", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["sogliola"] = new()
            {
                label = "Sogliola",
                description = "Pesce di mare",
                peso = 1f,
                sellPrice = 25,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Dai una sogliola", description = "A chi vuoi tu!!", give = true },
                drop = { label = "Getta via una sogliola", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["orata"] = new()
            {
                label = "Orata",
                description = "Pesce di mare",
                peso = 1f,
                sellPrice = 100,
                max = 30,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Dai un'orata", description = "A chi vuoi tu!!", give = true },
                drop = { label = "Getta via un'orata", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["tonno"] = new()
            {
                label = "Tonno",
                description = "Pesce di mare",
                peso = 1f,
                sellPrice = 100,
                max = 50,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Dai un tonno", description = "A chi vuoi tu!!", give = true },
                drop = { label = "Getta via un tonno", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["salmone"] = new()
            {
                label = "Salmone",
                description = "Pesce di mare",
                peso = 1f,
                sellPrice = 26,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Dai un salmone", description = "A chi vuoi tu!!", give = true },
                drop = { label = "Getta via un salmone", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["merluzzo"] = new()
            {
                label = "Merluzzo",
                description = "Pesce di mare",
                peso = 1f,
                sellPrice = 45,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Dai un merluzzo", description = "A chi vuoi tu!!", give = true },
                drop = { label = "Getta via un merluzzo", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["pescespada"] = new()
            {
                label = "Pesce Spada",
                description = "Pesce di mare",
                peso = 1f,
                sellPrice = 56,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Dai un pesce spada", description = "A chi vuoi tu!!", give = true },
                drop = { label = "Getta via un pesce spada", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["squalo"] = new()
            {
                label = "Squalo",
                description = "Pesce di mare",
                peso = 1f,
                sellPrice = 120,
                max = 100,
                prop = (ObjectHash)(113504370),
                use = { label = "", description = "", use = false },
                give = { label = "Dai uno squalo", description = "A chi vuoi tu!!", give = true },
                drop = { label = "Getta via uno squalo", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },

            ["carpa"] = new()
            {
                label = "Carpa",
                description = "Pesce di lago/fiume",
                peso = 1f,
                sellPrice = 46,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Dai una carpa", description = "A chi vuoi tu!!", give = true },
                drop = { label = "Getta via una carpa", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["luccio"] = new()
            {
                label = "Luccio",
                description = "Pesce di lago/fiume",
                peso = 1f,
                sellPrice = 35,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Dai un luccio", description = "A chi vuoi tu!!", give = true },
                drop = { label = "Getta via un luccio", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["persico"] = new()
            {
                label = "Persico",
                description = "Pesce di lago/fiume",
                peso = 1f,
                sellPrice = 39,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Dai un persico", description = "A chi vuoi tu!!", give = true },
                drop = { label = "Getta via un persico", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["pescegattocomune"] = new()
            {
                label = "Pesce gatto comune",
                description = "Pesce di lago/fiume",
                peso = 1f,
                sellPrice = 40,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Dai un pesce gatto comune", description = "A chi vuoi tu!!", give = true },
                drop = { label = "Getta via un pesce gatto comune", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["pescegattopunteggiato"] = new()
            {
                label = "Pesce gatto punteggiato",
                description = "Pesce di lago/fiume",
                peso = 1f,
                sellPrice = 60,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Dai un pesce gatto punteggiato", description = "A chi vuoi tu!!", give = true },
                drop = { label = "Getta via un pesce gatto punteggiato", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["spigola"] = new()
            {
                label = "Spigola",
                description = "Pesce di lago/fiume",
                peso = 1f,
                sellPrice = 49,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Dai una spigola", description = "A chi vuoi tu!!", give = true },
                drop = { label = "Getta via una spigola", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["trota"] = new()
            {
                label = "Trota",
                description = "Pesce di lago/fiume",
                peso = 1f,
                sellPrice = 55,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Dai una trota", description = "A chi vuoi tu!!", give = true },
                drop = { label = "Getta via una trota", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["ghiozzo"] = new()
            {
                label = "Ghiozzo",
                description = "Pesce di lago/fiume",
                peso = 1f,
                sellPrice = 60,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Dai un ghiozzo", description = "A chi vuoi tu!!", give = true },
                drop = { label = "Getta via un ghiozzo", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["lucioperca"] = new()
            {
                label = "Lucioperca",
                description = "Pesce di lago/fiume",
                peso = 1f,
                sellPrice = 100,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Dai un lucioperca", description = "A chi vuoi tu!!", give = true },
                drop = { label = "Getta via un lucioperca", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["alborella"] = new()
            {
                label = "Alborella",
                description = "Pesce di lago/fiume",
                peso = 1f,
                sellPrice = 100,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Dai una alborella", description = "A chi vuoi tu!!", give = true },
                drop = { label = "Getta via una alborella", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["carassio"] = new()
            {
                label = "Carassio",
                description = "Pesce di lago/fiume",
                peso = 1f,
                sellPrice = 100,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Dai un carassio", description = "A chi vuoi tu!!", give = true },
                drop = { label = "Getta via un carassio", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["carassiodorato"] = new()
            {
                label = "Carassio dorato",
                description = "Pesce di lago/fiume",
                peso = 1f,
                sellPrice = 100,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Dai un carassio dorato", description = "A chi vuoi tu!!", give = true },
                drop = { label = "Getta via un carassio dorato", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["cheppia"] = new()
            {
                label = "Cheppia",
                description = "Pesce di lago/fiume",
                peso = 1f,
                sellPrice = 100,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Dai una cheppia", description = "A chi vuoi tu!!", give = true },
                drop = { label = "Getta via una cheppia", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["rovella"] = new()
            {
                label = "Rovella",
                description = "Pesce di lago/fiume",
                peso = 1f,
                sellPrice = 100,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Dai una rovella", description = "A chi vuoi tu!!", give = true },
                drop = { label = "Getta via una rovella", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["spinarello"] = new()
            {
                label = "Spinarello",
                description = "Pesce di lago/fiume",
                peso = 1f,
                sellPrice = 100,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Dai uno spinarello", description = "A chi vuoi tu!!", give = true },
                drop = { label = "Getta via uno spinarello", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["storionecobice"] = new()
            {
                label = "Storione cobice",
                description = "Pesce di lago/fiume",
                peso = 1f,
                sellPrice = 100,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Dai uno storione cobice", description = "A chi vuoi tu!!", give = true },
                drop = { label = "Getta via uno storione cobice", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["storionecomune"] = new()
            {
                label = "Storione comune",
                description = "Pesce di lago/fiume",
                peso = 1f,
                sellPrice = 100,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Dai uno storione comune", description = "A chi vuoi tu!!", give = true },
                drop = { label = "Getta via uno storione comune", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },
            ["storioneladano"] = new()
            {
                label = "Storione ladano",
                description = "Pesce di lago/fiume",
                peso = 1f,
                sellPrice = 100,
                max = 100,
                prop = (ObjectHash)(802685111),
                use = { label = "", description = "", use = false },
                give = { label = "Dai uno storione ladano", description = "A chi vuoi tu!!", give = true },
                drop = { label = "Getta via uno storione ladano", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },

            ["carnecervo"] = new()
            {
                label = "Carne di cervo",
                description = "Carne di selvaggina",
                peso = 1f,
                sellPrice = 100,
                max = 100,
                prop = (ObjectHash)(289396019),
                use = { label = "", description = "", use = false },
                give = { label = "Dai la carne di cervo", description = "A chi vuoi tu!!", give = true },
                drop = { label = "Getta via la carne di cervo", description = "", drop = true },
                sell = { label = "Vendi la carne di cervo", description = "", sell = true },
                buy = { label = "Compra la carne di cervo", description = "", buy = true }
            },
            ["carnecinghiale"] = new()
            {
                label = "Carne di cinghiale",
                description = "Carne di selvaggina",
                peso = 1f,
                sellPrice = 100,
                max = 100,
                prop = (ObjectHash)(289396019),
                use = { label = "", description = "", use = false },
                give = { label = "Dai la carne di cinghiale", description = "A chi vuoi tu!!", give = true },
                drop = { label = "Getta via la carne di cinghiale", description = "", drop = true },
                sell = { label = "Vendi la carne di cinghiale", description = "", sell = true },
                buy = { label = "Compra la carne di cinghiale", description = "", buy = true }
            },
            ["carneconiglio"] = new()
            {
                label = "Carne di coniglio",
                description = "Carne di selvaggina",
                peso = 1f,
                sellPrice = 100,
                max = 100,
                prop = (ObjectHash)(289396019),
                use = { label = "", description = "", use = false },
                give = { label = "Dai la carne di coniglio", description = "A chi vuoi tu!!", give = true },
                drop = { label = "Getta via la carne di coniglio", description = "", drop = true },
                sell = { label = "Vendi la carne di coniglio", description = "", sell = true },
                buy = { label = "Compra la carne di coniglio", description = "", buy = true }
            },
            ["carnecoyote"] = new()
            {
                label = "Carne di coyote",
                description = "Carne di selvaggina",
                peso = 1f,
                sellPrice = 100,
                max = 100,
                prop = (ObjectHash)(289396019),
                use = { label = "", description = "", use = false },
                give = { label = "Dai la carne di coyote", description = "A chi vuoi tu!!", give = true },
                drop = { label = "Getta via la carne di coyote", description = "", drop = true },
                sell = { label = "Vendi la carne di coyote", description = "", sell = true },
                buy = { label = "Compra la carne di coyote", description = "", buy = true }
            },
            ["carneaquila"] = new()
            {
                label = "Carne di aquila",
                description = "Carne di selvaggina",
                peso = 1f,
                sellPrice = 100,
                max = 100,
                prop = (ObjectHash)(289396019),
                use = { label = "", description = "", use = false },
                give = { label = "Dai la carne di aquila", description = "A chi vuoi tu!!", give = true },
                drop = { label = "Getta via la carne di aquila", description = "", drop = true },
                sell = { label = "Vendi la carne di aquila", description = "", sell = true },
                buy = { label = "Compra la carne di aquila", description = "", buy = true }
            },

            ["hamburger"] = new()
            {
                label = "Hamburger",
                description = "Il nettare degli dei",
                peso = 1f,
                sellPrice = 0,
                max = 2,
                prop = (ObjectHash)(-2054442544),
                use = { label = "Mangia un hamburger", description = "Che buono!", use = true },
                give = { label = "Dai l'hamburger", description = "A chi vuoi tu!!", give = true },
                drop = { label = "Getta via uno o più hamburger", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "Compra degli Hamburger", description = "", buy = true }
            },
            ["acqua"] = new()
            {
                label = "Bottiglia d'acqua",
                description = "Dissetante",
                peso = 1f,
                sellPrice = 0,
                max = 2,
                prop = (ObjectHash)(746336278),
                use = { label = "Bevi una bottiglia d'acqua", description = "Che sete!", use = true },
                give = { label = "Dai l'acqua", description = "A chi vuoi tu!!", give = true },
                drop = { label = "Getta via uno o più bottiglie", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "Compra l'acqua", description = "", buy = true }
            },

            ["ModKit"] = new()
            {
                label = "Kit per la Modifica",
                description = "Il tuning va prima installato!",
                peso = 1f,
                sellPrice = 0,
                max = 2,
                prop = (ObjectHash)(289396019),
                use = { label = "Installa il kit per le modifiche", description = "Abilita il veicolo alle modifiche", use = true },
                give = { label = "Dai kit per le modifiche", description = "A chi vuoi tu!!", give = true },
                drop = { label = "Getta via il kit per le modifiche", description = "", drop = true },
                sell = { label = "", description = "", sell = false },
                buy = { label = "", description = "", buy = false }
            },

        };
        #endregion

        #region DeathReasons
        public Dictionary<uint, string> DeathReasons = new Dictionary<uint, string>()
        {
            [2460120199] = "Daga di cavalleria antica",
            [2508868239] = "Mazza da Baseball",
            [4192643659] = "Bottiglia",
            [2227010557] = "Piede di porco",
            [2725352035] = "Pugno",
            [2343591895] = "Torcia",
            [1141786504] = "Mazza da Golf",
            [1317494643] = "Martello",
            [4191993645] = "Accetta",
            [3638508604] = "Tirapugni",
            [2578778090] = "Coltello",
            [3713923289] = "Machete",
            [3756226112] = "Coltello a Serramanico",
            [1737195953] = "Sfollagente",
            [419712736] = "Chiave giratubi",
            [3441901897] = "Ascia da battaglia",
            [2484171525] = "Stecca da biliardo",
            [940833800] = "Accetta di pietra",
            [453432689] = "Pistola",
            [3219281620] = "Pistola MK2",
            [1593441988] = "Pistola da Combattimento",
            [584646201] = "Pistola AP",
            [911657153] = "Tazer",
            [2578377531] = "Pistola .50",
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

        #region Armi
        public Dictionary<string, Arma> Armi = new Dictionary<string, Arma>()
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


    
    public class Arma
    {
        public string? name;
        public List<Components> components = new List<Components>();
        public List<Tinte> tints = new List<Tinte>();
        public Arma() { }
        public Arma(string _name, List<Components> _comp, List<Tinte> _tints)
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


        public event UsaOggetto Usa;
        public event DaiOggetto Dai;
        public event ButtaOggetto Butta;
        public event VendiOggetto Vendi;
        public event CompraOggetto Compra;

        public void UsaOggettoEvent(int quantity)
        {
            Usa.Invoke(this, quantity);
        }

        public void DaiOggettoEvent(int quantity)
        {
            Dai.Invoke(this, quantity);
        }

        public void ButtaOggettoEvent(int quantity)
        {
            Butta.Invoke(this, quantity);
        }

        public void VendiOggettoEvent(int quantity)
        {
            Vendi.Invoke(this, quantity);
        }

        public void CompraOggettoEvent(int quantity)
        {
            Compra.Invoke(this, quantity);
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

    
    public class Tinte
    {
        public string name { get; set; }
        public int value { get; set; }
        public Tinte() { }
        public Tinte(string _name, int _value)
        {
            this.name = _name;
            this.value = _value;
        }
    }
}