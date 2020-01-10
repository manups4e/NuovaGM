using System.Collections.Generic;

namespace NuovaGM.Shared
{
	public class SharedScript
	{

		public static Dictionary<string, List<Char_data>> Personaggi = new Dictionary<string, List<Char_data>>();

		public static Dictionary<string, Item> ItemList = new Dictionary<string, Item>()
		{
			["pietra"] = new Item("Una Bella Coppola Di Minchia", "Gekeje Gay", 1f, 100, 100, 100, new Use("Usa una Pietra", "Ci puoi uccidere una meretrice a lapidate!", true), new Give("Dai una pietra", "A chi vuoi tu!!", true), new Drop("Getta via una Pietra", "", true), new Sell("", "", false), new Buy("", "", false)),
		};

		Dictionary<uint, string> DeatReasons = new Dictionary<uint, string>()
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
			[3219281620] = "Pistol MK2",
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
			[3452007600] = "Caduto",
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

		public static List<Arma> Armi = new List<Arma>(){
			new Arma("WEAPON_KNIFE", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_NIGHTSTICK", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_HAMMER", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_BAT", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_GOLFCLUB", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_CROWBAR", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_RPG", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_STINGER", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_MINIGUN", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_GRENADE", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_STICKYBOMB", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_SMOKEGRENADE", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_BZGAS", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_MOLOTOV", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_FIREEXTINGUISHER", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_PETROLCAN", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_DIGISCANNER", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_BALL", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_BOTTLE", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_DAGGER", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_FIREWORK", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_MUSKET", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_STUNGUN", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_HOMINGLAUNCHER", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_PROXMINE", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_SNOWBALL", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_FLAREGUN", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_GARBAGEBAG", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_HANDCUFfs", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_MARKSMANPISTOL", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_HATCHET", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_RAILGUN", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_MACHETE", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_DBSHOTGUN", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_AUTOSHOTGUN", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_BATTLEAXE", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_COMPACTLAUNCHER", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_PIPEBOMB", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_POOLCUE", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_WRENCH", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_FLASHLIGHT", new List<Components>(), new List<Tinte>()),
			new Arma("GADGET_NIGHTVISION", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_FLARE", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_DOUBLEACTION", new List<Components>(), new List<Tinte>()),
			new Arma("WEAPON_PISTOL", new List<Components>() { new Components("COMPONENT_PISTOL_CLIP_01", true), new Components("COMPONENT_PISTOL_CLIP_02", false), new Components("COMPONENT_AT_PI_FLSH", false), new Components("COMPONENT_AT_PI_SUPP_02", false), new Components("COMPONENT_PISTOL_VARMOD_LUXE", false) }, new List<Tinte>() { new Tinte("WM_TINT0", 0), new Tinte("WM_TINT1", 1), new Tinte("WM_TINT2", 2), new Tinte("WM_TINT3", 3), new Tinte("WM_TINT4", 4), new Tinte("WM_TINT5", 5), new Tinte("WM_TINT6", 6), new Tinte("WM_TINT7", 7) }),
			new Arma("WEAPON_COMBATPISTOL", new List<Components>() { new Components("COMPONENT_COMBATPISTOL_CLIP_01", true), new Components("COMPONENT_COMBATPISTOL_CLIP_02", false), new Components("COMPONENT_AT_PI_FLSH", false), new Components("COMPONENT_AT_PI_SUPP", false), new Components("COMPONENT_COMBATPISTOL_VARMOD_LOWRIDER", false) }, new List<Tinte>() { new Tinte("WM_TINT0", 0), new Tinte("WM_TINT1", 1), new Tinte("WM_TINT2", 2), new Tinte("WM_TINT3", 3), new Tinte("WM_TINT4", 4), new Tinte("WM_TINT5", 5), new Tinte("WM_TINT6", 6), new Tinte("WM_TINT7", 7) }),
			new Arma("WEAPON_APPISTOL", new List<Components>() { new Components("COMPONENT_APPISTOL_CLIP_01", true), new Components("COMPONENT_APPISTOL_CLIP_02", false), new Components("COMPONENT_AT_PI_FLSH", false), new Components("COMPONENT_AT_PI_SUPP_02", false), new Components("COMPONENT_APPISTOL_VARMOD_LUXE", false) }, new List<Tinte>() { new Tinte("WM_TINT0", 0), new Tinte("WM_TINT1", 1), new Tinte("WM_TINT2", 2), new Tinte("WM_TINT3", 3), new Tinte("WM_TINT4", 4), new Tinte("WM_TINT5", 5), new Tinte("WM_TINT6", 6), new Tinte("WM_TINT7", 7) }),
			new Arma("WEAPON_PISTOL50", new List<Components>() { new Components("COMPONENT_PISTOL50_CLIP_01", true), new Components("COMPONENT_PISTOL50_CLIP_02", false), new Components("COMPONENT_AT_PI_FLSH", false), new Components("COMPONENT_AT_AR_SUPP_02", false), new Components("COMPONENT_PISTOL50_VARMOD_LUXE", false) }, new List<Tinte>() { new Tinte("WM_TINT0", 0), new Tinte("WM_TINT1", 1), new Tinte("WM_TINT2", 2), new Tinte("WM_TINT3", 3), new Tinte("WM_TINT4", 4), new Tinte("WM_TINT5", 5), new Tinte("WM_TINT6", 6), new Tinte("WM_TINT7", 7) }),
			new Arma("WEAPON_REVOLVER", new List<Components>(), new List<Tinte>() { new Tinte("WM_TINT0", 0), new Tinte("WM_TINT1", 1), new Tinte("WM_TINT2", 2), new Tinte("WM_TINT3", 3), new Tinte("WM_TINT4", 4), new Tinte("WM_TINT5", 5), new Tinte("WM_TINT6", 6), new Tinte("WM_TINT7", 7) }),
			new Arma("WEAPON_SNSPISTOL", new List<Components>() { new Components("COMPONENT_SNSPISTOL_CLIP_01", true), new Components("COMPONENT_SNSPISTOL_CLIP_02", false), new Components("COMPONENT_SNSPISTOL_VARMOD_LOWRIDER", false)}, new List<Tinte>() { new Tinte("WM_TINT0", 0), new Tinte("WM_TINT1", 1), new Tinte("WM_TINT2", 2), new Tinte("WM_TINT3", 3), new Tinte("WM_TINT4", 4), new Tinte("WM_TINT5", 5), new Tinte("WM_TINT6", 6), new Tinte("WM_TINT7", 7) }),
			new Arma("WEAPON_HEAVYPISTOL", new List<Components>() { new Components("COMPONENT_HEAVYPISTOL_CLIP_01", true), new Components("COMPONENT_HEAVYPISTOL_CLIP_02", false), new Components("COMPONENT_AT_PI_FLSH", false), new Components("COMPONENT_AT_PI_SUPP", false), new Components("COMPONENT_HEAVYPISTOL_VARMOD_LUXE", false) }, new List<Tinte>() { new Tinte("WM_TINT0", 0), new Tinte("WM_TINT1", 1), new Tinte("WM_TINT2", 2), new Tinte("WM_TINT3", 3), new Tinte("WM_TINT4", 4), new Tinte("WM_TINT5", 5), new Tinte("WM_TINT6", 6), new Tinte("WM_TINT7", 7) }),
			new Arma("WEAPON_VINTAGEPISTOL", new List<Components>() { new Components("COMPONENT_VINTAGEPISTOL_CLIP_01", true), new Components("COMPONENT_VINTAGEPISTOL_CLIP_02", false), new Components("COMPONENT_AT_PI_SUPP", false) }, new List<Tinte>() { new Tinte("WM_TINT0", 0), new Tinte("WM_TINT1", 1), new Tinte("WM_TINT2", 2), new Tinte("WM_TINT3", 3), new Tinte("WM_TINT4", 4), new Tinte("WM_TINT5", 5), new Tinte("WM_TINT6", 6), new Tinte("WM_TINT7", 7) }),
			new Arma("WEAPON_MICROSMG", new List<Components>() { new Components("COMPONENT_MICROSMG_CLIP_01", true), new Components("COMPONENT_MICROSMG_CLIP_02", false), new Components("COMPONENT_AT_PI_FLSH", false), new Components("COMPONENT_AT_SCOPE_MACRO", false), new Components("COMPONENT_AT_AR_SUPP_02", false), new Components("COMPONENT_MICROSMG_VARMOD_LUXE", false) }, new List<Tinte>() { new Tinte("WM_TINT0", 0), new Tinte("WM_TINT1", 1), new Tinte("WM_TINT2", 2), new Tinte("WM_TINT3", 3), new Tinte("WM_TINT4", 4), new Tinte("WM_TINT5", 5), new Tinte("WM_TINT6", 6), new Tinte("WM_TINT7", 7) }),
			new Arma("WEAPON_SMG", new List<Components>() { new Components("COMPONENT_SMG_CLIP_01", true), new Components("COMPONENT_SMG_CLIP_01", false), new Components("COMPONENT_SMG_CLIP_03", false), new Components("COMPONENT_AT_AR_FLSH", false), new Components("COMPONENT_AT_SCOPE_MACRO_02", false), new Components("COMPONENT_AT_PI_SUPP", false), new Components("COMPONENT_SMG_VARMOD_LUXE", false) }, new List<Tinte>() { new Tinte("WM_TINT0", 0), new Tinte("WM_TINT1", 1), new Tinte("WM_TINT2", 2), new Tinte("WM_TINT3", 3), new Tinte("WM_TINT4", 4), new Tinte("WM_TINT5", 5), new Tinte("WM_TINT6", 6), new Tinte("WM_TINT7", 7) }),
			new Arma("WEAPON_ASSAULTSMG", new List<Components>() { new Components("COMPONENT_ASSAULTSMG_CLIP_01", true), new Components("COMPONENT_ASSAULTSMG_CLIP_02", false), new Components("COMPONENT_AT_AR_FLSH", false), new Components("COMPONENT_AT_PI_SUPP_02", false), new Components("COMPONENT_AT_SCOPE_MACRO", false), new Components("COMPONENT_AT_AR_SUPP_02", false), new Components("COMPONENT_ASSAULTSMG_VARMOD_LOWRIDER", false) }, new List<Tinte>() { new Tinte("WM_TINT0", 0), new Tinte("WM_TINT1", 1), new Tinte("WM_TINT2", 2), new Tinte("WM_TINT3", 3), new Tinte("WM_TINT4", 4), new Tinte("WM_TINT5", 5), new Tinte("WM_TINT6", 6), new Tinte("WM_TINT7", 7) }),
			new Arma("WEAPON_MINISMG", new List<Components>() { new Components("COMPONENT_MINISMG_CLIP_01", true), new Components("COMPONENT_MINISMG_CLIP_02", false) }, new List<Tinte>() { new Tinte("WM_TINT0", 0), new Tinte("WM_TINT1", 1), new Tinte("WM_TINT2", 2), new Tinte("WM_TINT3", 3), new Tinte("WM_TINT4", 4), new Tinte("WM_TINT5", 5), new Tinte("WM_TINT6", 6), new Tinte("WM_TINT7", 7) }),
			new Arma("WEAPON_MACHINEPISTOL", new List<Components>() { new Components("COMPONENT_MACHINEPISTOL_CLIP_01", true), new Components("COMPONENT_MACHINEPISTOL_CLIP_02", false), new Components("COMPONENT_MACHINEPISTOL_CLIP_03", false), new Components("COMPONENT_AT_PI_SUPP", false) }, new List<Tinte>() { new Tinte("WM_TINT0", 0), new Tinte("WM_TINT1", 1), new Tinte("WM_TINT2", 2), new Tinte("WM_TINT3", 3), new Tinte("WM_TINT4", 4), new Tinte("WM_TINT5", 5), new Tinte("WM_TINT6", 6), new Tinte("WM_TINT7", 7) }),
			new Arma("WEAPON_COMBATPDW", new List<Components>() { new Components("COMPONENT_COMBATPDW_CLIP_01", true), new Components("COMPONENT_COMBATPDW_CLIP_02", false), new Components("COMPONENT_COMBATPDW_CLIP_03", false), new Components("COMPONENT_AT_AR_FLSH", false), new Components("COMPONENT_AT_AR_AFGRIP", false), new Components("COMPONENT_AT_SCOPE_SMALL", false) }, new List<Tinte>() { new Tinte("WM_TINT0", 0), new Tinte("WM_TINT1", 1), new Tinte("WM_TINT2", 2), new Tinte("WM_TINT3", 3), new Tinte("WM_TINT4", 4), new Tinte("WM_TINT5", 5), new Tinte("WM_TINT6", 6), new Tinte("WM_TINT7", 7) }),
			new Arma("WEAPON_PUMPSHOTGUN", new List<Components>() { new Components("COMPONENT_AT_AR_FLSH", false), new Components("COMPONENT_AT_SR_SUPP", false), new Components("COMPONENT_PUMPSHOTGUN_VARMOD_LOWRIDER", false) }, new List<Tinte>() { new Tinte("WM_TINT0", 0), new Tinte("WM_TINT1", 1), new Tinte("WM_TINT2", 2), new Tinte("WM_TINT3", 3), new Tinte("WM_TINT4", 4), new Tinte("WM_TINT5", 5), new Tinte("WM_TINT6", 6), new Tinte("WM_TINT7", 7) }),
			new Arma("WEAPON_SAWNOFfsHOTGUN", new List<Components>() { new Components("COMPONENT_SAWNOFfsHOTGUN_VARMOD_LUXE", false) }, new List<Tinte>() { new Tinte("WM_TINT0", 0), new Tinte("WM_TINT1", 1), new Tinte("WM_TINT2", 2), new Tinte("WM_TINT3", 3), new Tinte("WM_TINT4", 4), new Tinte("WM_TINT5", 5), new Tinte("WM_TINT6", 6), new Tinte("WM_TINT7", 7) }),
			new Arma("WEAPON_ASSAULTSHOTGUN", new List<Components>() { new Components("COMPONENT_ASSAULTSHOTGUN_CLIP_01", true), new Components("COMPONENT_ASSAULTSHOTGUN_CLIP_02", false), new Components("COMPONENT_AT_AR_FLSH", false), new Components("COMPONENT_AT_AR_SUPP", false), new Components("COMPONENT_AT_AR_AFGRIP", false) }, new List<Tinte>() { new Tinte("WM_TINT0", 0), new Tinte("WM_TINT1", 1), new Tinte("WM_TINT2", 2), new Tinte("WM_TINT3", 3), new Tinte("WM_TINT4", 4), new Tinte("WM_TINT5", 5), new Tinte("WM_TINT6", 6), new Tinte("WM_TINT7", 7) }),
			new Arma("WEAPON_BULLPUPSHOTGUN", new List<Components>() { new Components("COMPONENT_AT_AR_FLSH", false), new Components("COMPONENT_AT_AR_SUPP_02", false), new Components("COMPONENT_AT_AR_AFGRIP", false) }, new List<Tinte>() { new Tinte("WM_TINT0", 0), new Tinte("WM_TINT1", 1), new Tinte("WM_TINT2", 2), new Tinte("WM_TINT3", 3), new Tinte("WM_TINT4", 4), new Tinte("WM_TINT5", 5), new Tinte("WM_TINT6", 6), new Tinte("WM_TINT7", 7) }),
			new Arma("WEAPON_HEAVYSHOTGUN", new List<Components>() { new Components("COMPONENT_HEAVYSHOTGUN_CLIP_01", true), new Components("COMPONENT_HEAVYSHOTGUN_CLIP_02", false), new Components("COMPONENT_HEAVYSHOTGUN_CLIP_03", false), new Components("COMPONENT_AT_AR_FLSH", false), new Components("COMPONENT_AT_AR_SUPP_02", false), new Components("COMPONENT_AT_AR_AFGRIP", false) }, new List<Tinte>() { new Tinte("WM_TINT0", 0), new Tinte("WM_TINT1", 1), new Tinte("WM_TINT2", 2), new Tinte("WM_TINT3", 3), new Tinte("WM_TINT4", 4), new Tinte("WM_TINT5", 5), new Tinte("WM_TINT6", 6), new Tinte("WM_TINT7", 7) }),
			new Arma("WEAPON_ASSAULTRIFLE", new List<Components>() { new Components("COMPONENT_ASSAULTRIFLE_CLIP_01", true), new Components("COMPONENT_ASSAULTRIFLE_CLIP_02", false), new Components("COMPONENT_ASSAULTRIFLE_CLIP_03", false), new Components("COMPONENT_AT_AR_FLSH", false), new Components("COMPONENT_AT_SCOPE_MACRO", false), new Components("COMPONENT_AT_AR_SUPP_02", false), new Components("COMPONENT_AT_AR_AFGRIP", false), new Components("COMPONENT_ASSAULTRIFLE_VARMOD_LUXE", false) }, new List<Tinte>() { new Tinte("WM_TINT0", 0), new Tinte("WM_TINT1", 1), new Tinte("WM_TINT2", 2), new Tinte("WM_TINT3", 3), new Tinte("WM_TINT4", 4), new Tinte("WM_TINT5", 5), new Tinte("WM_TINT6", 6), new Tinte("WM_TINT7", 7) }),
			new Arma("WEAPON_CARBINERIFLE", new List<Components>() { new Components("COMPONENT_CARBINERIFLE_CLIP_01", true), new Components("COMPONENT_CARBINERIFLE_CLIP_02", false), new Components("COMPONENT_CARBINERIFLE_CLIP_03", false), new Components("COMPONENT_AT_AR_FLSH", false), new Components("COMPONENT_AT_SCOPE_MEDIUM", false), new Components("COMPONENT_AT_AR_SUPP", false), new Components("COMPONENT_AT_AR_AFGRIP", false), new Components("COMPONENT_CARBINERIFLE_VARMOD_LUXE", false) }, new List<Tinte>() { new Tinte("WM_TINT0", 0), new Tinte("WM_TINT1", 1), new Tinte("WM_TINT2", 2), new Tinte("WM_TINT3", 3), new Tinte("WM_TINT4", 4), new Tinte("WM_TINT5", 5), new Tinte("WM_TINT6", 6), new Tinte("WM_TINT7", 7) }),
			new Arma("WEAPON_ADVANCEDRIFLE", new List<Components>() { new Components("COMPONENT_ADVANCEDRIFLE_CLIP_01", true), new Components("COMPONENT_ADVANCEDRIFLE_CLIP_02", false), new Components("COMPONENT_AT_AR_FLSH", false), new Components("COMPONENT_AT_SCOPE_SMALL", false), new Components("COMPONENT_AT_AR_SUPP", false), new Components("COMPONENT_ADVANCEDRIFLE_VARMOD_LUXE", false) }, new List<Tinte>() { new Tinte("WM_TINT0", 0), new Tinte("WM_TINT1", 1), new Tinte("WM_TINT2", 2), new Tinte("WM_TINT3", 3), new Tinte("WM_TINT4", 4), new Tinte("WM_TINT5", 5), new Tinte("WM_TINT6", 6), new Tinte("WM_TINT7", 7) }),
			new Arma("WEAPON_SPECIALCARBINE", new List<Components>() { new Components("COMPONENT_SPECIALCARBINE_CLIP_01", true), new Components("COMPONENT_SPECIALCARBINE_CLIP_02", false), new Components("COMPONENT_SPECIALCARBINE_CLIP_03", false), new Components("COMPONENT_AT_AR_FLSH", false), new Components("COMPONENT_AT_SCOPE_MEDIUM", false), new Components("COMPONENT_AT_AR_SUPP_02", false), new Components("COMPONENT_AT_AR_AFGRIP", false), new Components("COMPONENT_SPECIALCARBINE_VARMOD_LOWRIDER", false) }, new List<Tinte>() { new Tinte("WM_TINT0", 0), new Tinte("WM_TINT1", 1), new Tinte("WM_TINT2", 2), new Tinte("WM_TINT3", 3), new Tinte("WM_TINT4", 4), new Tinte("WM_TINT5", 5), new Tinte("WM_TINT6", 6), new Tinte("WM_TINT7", 7) }),
			new Arma("WEAPON_BULLPUPRIFLE", new List<Components>() { new Components("COMPONENT_BULLPUPRIFLE_CLIP_01", true), new Components("COMPONENT_BULLPUPRIFLE_CLIP_02", false), new Components("COMPONENT_AT_AR_FLSH", false), new Components("COMPONENT_AT_SCOPE_SMALL", false), new Components("COMPONENT_AT_AR_SUPP", false), new Components("COMPONENT_AT_AR_AFGRIP", false), new Components("COMPONENT_BULLPUPRIFLE_VARMOD_LOW", false) }, new List<Tinte>() { new Tinte("WM_TINT0", 0), new Tinte("WM_TINT1", 1), new Tinte("WM_TINT2", 2), new Tinte("WM_TINT3", 3), new Tinte("WM_TINT4", 4), new Tinte("WM_TINT5", 5), new Tinte("WM_TINT6", 6), new Tinte("WM_TINT7", 7) }),
			new Arma("WEAPON_COMPACTRIFLE", new List<Components>() { new Components("COMPONENT_COMPACTRIFLE_CLIP_01", true), new Components("COMPONENT_COMPACTRIFLE_CLIP_02", false), new Components("COMPONENT_COMPACTRIFLE_CLIP_03", false) }, new List<Tinte>() { new Tinte("WM_TINT0", 0), new Tinte("WM_TINT1", 1), new Tinte("WM_TINT2", 2), new Tinte("WM_TINT3", 3), new Tinte("WM_TINT4", 4), new Tinte("WM_TINT5", 5), new Tinte("WM_TINT6", 6), new Tinte("WM_TINT7", 7) }),
			new Arma("WEAPON_MG", new List<Components>() { new Components("COMPONENT_MG_CLIP_01", true), new Components("COMPONENT_MG_CLIP_02", false), new Components("COMPONENT_AT_SCOPE_SMALL_02", false), new Components("COMPONENT_MG_VARMOD_LOWRIDER", false) }, new List<Tinte>() { new Tinte("WM_TINT0", 0), new Tinte("WM_TINT1", 1), new Tinte("WM_TINT2", 2), new Tinte("WM_TINT3", 3), new Tinte("WM_TINT4", 4), new Tinte("WM_TINT5", 5), new Tinte("WM_TINT6", 6), new Tinte("WM_TINT7", 7) }),
			new Arma("WEAPON_COMBATMG", new List<Components>() { new Components("COMPONENT_COMBATMG_CLIP_01", true), new Components("COMPONENT_COMBATMG_CLIP_02", false), new Components("COMPONENT_AT_SCOPE_MEDIUM", false), new Components("COMPONENT_AT_AR_AFGRIP", false), new Components("COMPONENT_COMBATMG_VARMOD_LOWRIDER", false) }, new List<Tinte>() { new Tinte("WM_TINT0", 0), new Tinte("WM_TINT1", 1), new Tinte("WM_TINT2", 2), new Tinte("WM_TINT3", 3), new Tinte("WM_TINT4", 4), new Tinte("WM_TINT5", 5), new Tinte("WM_TINT6", 6), new Tinte("WM_TINT7", 7) }),
			new Arma("WEAPON_GUSENBERG", new List<Components>() { new Components("COMPONENT_GUSENBERG_CLIP_01", true), new Components("COMPONENT_GUSENBERG_CLIP_02", false) }, new List<Tinte>() { new Tinte("WM_TINT0", 0), new Tinte("WM_TINT1", 1), new Tinte("WM_TINT2", 2), new Tinte("WM_TINT3", 3), new Tinte("WM_TINT4", 4), new Tinte("WM_TINT5", 5), new Tinte("WM_TINT6", 6), new Tinte("WM_TINT7", 7) }),
			new Arma("WEAPON_SNIPERRIFLE", new List<Components>() { new Components("COMPONENT_AT_SCOPE_LARGE", false), new Components("COMPONENT_AT_SCOPE_MAX", false), new Components("COMPONENT_AT_AR_SUPP_02", false), new Components("COMPONENT_SNIPERRIFLE_VARMOD_LUXE", false) }, new List<Tinte>() { new Tinte("WM_TINT0", 0), new Tinte("WM_TINT1", 1), new Tinte("WM_TINT2", 2), new Tinte("WM_TINT3", 3), new Tinte("WM_TINT4", 4), new Tinte("WM_TINT5", 5), new Tinte("WM_TINT6", 6), new Tinte("WM_TINT7", 7) }),
			new Arma("WEAPON_HEAVYSNIPER", new List<Components>() { new Components("COMPONENT_AT_SCOPE_LARGE", false), new Components("COMPONENT_AT_SCOPE_MAX", false) }, new List<Tinte>() { new Tinte("WM_TINT0", 0), new Tinte("WM_TINT1", 1), new Tinte("WM_TINT2", 2), new Tinte("WM_TINT3", 3), new Tinte("WM_TINT4", 4), new Tinte("WM_TINT5", 5), new Tinte("WM_TINT6", 6), new Tinte("WM_TINT7", 7) }),
			new Arma("WEAPON_MARKSMANRIFLE", new List<Components>() { new Components("COMPONENT_MARKSMANRIFLE_CLIP_01", true), new Components("COMPONENT_MARKSMANRIFLE_CLIP_02", false), new Components("COMPONENT_AT_AR_FLSH", false), new Components("COMPONENT_AT_SCOPE_LARGE_FIXED_ZOOM", false), new Components("COMPONENT_AT_AR_SUPP", false), new Components("COMPONENT_AT_AR_AFGRIP", false), new Components("COMPONENT_MARKSMANRIFLE_VARMOD_LUXE", false) }, new List<Tinte>() { new Tinte("WM_TINT0", 0), new Tinte("WM_TINT1", 1), new Tinte("WM_TINT2", 2), new Tinte("WM_TINT3", 3), new Tinte("WM_TINT4", 4), new Tinte("WM_TINT5", 5), new Tinte("WM_TINT6", 6), new Tinte("WM_TINT7", 7) }),
			new Arma("WEAPON_GRENADELAUNCHER", new List<Components>() { new Components("COMPONENT_AT_AR_FLSH", false), new Components("COMPONENT_AT_AR_AFGRIP", false), new Components("COMPONENT_AT_SCOPE_SMALL", false) }, new List<Tinte>() { new Tinte("WM_TINT0", 0), new Tinte("WM_TINT1", 1), new Tinte("WM_TINT2", 2), new Tinte("WM_TINT3", 3), new Tinte("WM_TINT4", 4), new Tinte("WM_TINT5", 5), new Tinte("WM_TINT6", 6), new Tinte("WM_TINT7", 7) }),
			new Arma("WEAPON_RPG", new List<Components>(), new List<Tinte>() { new Tinte("WM_TINT0", 0), new Tinte("WM_TINT1", 1), new Tinte("WM_TINT2", 2), new Tinte("WM_TINT3", 3), new Tinte("WM_TINT4", 4), new Tinte("WM_TINT5", 5), new Tinte("WM_TINT6", 6), new Tinte("WM_TINT7", 7) }),
			new Arma("WEAPON_STINGER", new List<Components>(), new List<Tinte>() { new Tinte("WM_TINT0", 0), new Tinte("WM_TINT1", 1), new Tinte("WM_TINT2", 2), new Tinte("WM_TINT3", 3), new Tinte("WM_TINT4", 4), new Tinte("WM_TINT5", 5), new Tinte("WM_TINT6", 6), new Tinte("WM_TINT7", 7) }),
			new Arma("WEAPON_MINIGUN", new List<Components>(), new List<Tinte>() { new Tinte("WM_TINT0", 0), new Tinte("WM_TINT1", 1), new Tinte("WM_TINT2", 2), new Tinte("WM_TINT3", 3), new Tinte("WM_TINT4", 4), new Tinte("WM_TINT5", 5), new Tinte("WM_TINT6", 6), new Tinte("WM_TINT7", 7) }),
			new Arma("WEAPON_KNUCKLE", new List<Components>() { new Components("COMPONENT_KNUCKLE_VARMOD_BASE", true), new Components("COMPONENT_KNUCKLE_VARMOD_PIMP", false), new Components("COMPONENT_KNUCKLE_VARMOD_BALLAS", false), new Components("COMPONENT_KNUCKLE_VARMOD_DOLLAR", false), new Components("COMPONENT_KNUCKLE_VARMOD_DIAMOND", false), new Components("COMPONENT_KNUCKLE_VARMOD_HATE", false), new Components("COMPONENT_KNUCKLE_VARMOD_LOVE", false), new Components("COMPONENT_KNUCKLE_VARMOD_PLAYER", false), new Components("COMPONENT_KNUCKLE_VARMOD_KING", false), new Components("COMPONENT_KNUCKLE_VARMOD_VAGOS", false) }, new List<Tinte>() { new Tinte("WM_TINT0", 0), new Tinte("WM_TINT1", 1), new Tinte("WM_TINT2", 2), new Tinte("WM_TINT3", 3), new Tinte("WM_TINT4", 4), new Tinte("WM_TINT5", 5), new Tinte("WM_TINT6", 6), new Tinte("WM_TINT7", 7) }),
		};


	}

	public class Arma
	{
		public string name;
		public List<Components> components;
		public List<Tinte> tints;
		public Arma(string _name, List<Components> _comp, List<Tinte> _tints)
		{
			this.name = _name;
			this.components = _comp;
			this.tints = _tints;
		}
	}

	public class Item
	{
		public string label { get; protected set; }
		public string description { get; protected set; }
		public float peso { get; protected set; }
		public int buyPrice { get; protected set; }
		public int sellPrice { get; protected set; }
		public int max { get; protected set; }
		public Use use { get; protected set; }
		public Give give { get; protected set; }
		public Drop drop { get; protected set; }
		public Sell sell { get; protected set; }
		public Buy buy { get; protected set; }

		public Item(string _label, string _desc, float _peso, int _buypr, int _sellpr, int _max, Use _use, Give _give, Drop _drop, Sell _sell, Buy _buy)
		{
			this.label = _label;
			this.description = _desc;
			this.peso = _peso;
			this.buyPrice = _buypr;
			this.sellPrice = _sellpr;
			this.max = _max;
			this.use = _use;
			this.give = _give;
			this.drop = _drop;
			this.sell = _sell;
			this.buy = _buy;
		}
	}

	public class Use
	{
		public string label { get; protected set; }
		public string description { get; protected set; }
		public bool use { get; protected set; }

		public Use(string _label, string _description, bool _use)
		{
			this.label = _label;
			this.description = _description;
			this.use = _use;
		}
	}

	public class Give
	{
		public string label { get; protected set; }
		public string description { get; protected set; }
		public bool give { get; protected set; }
		public Give(string _label, string _description, bool _give)
		{
			this.label = _label;
			this.description = _description;
			this.give = _give;
		}
	}

	public class Drop
	{
		public string label { get; protected set; }
		public string description { get; protected set; }
		public bool drop { get; protected set; }

		public Drop(string _label, string _description, bool _drop)
		{
			this.label = _label;
			this.description = _description;
			this.drop = _drop;
		}
	}

	public class Sell
	{
		public string label { get; protected set; }
		public string description { get; protected set; }
		public bool sell { get; protected set; }

		public Sell(string _label, string _description, bool _sell)
		{
			this.label = _label;
			this.description = _description;
			this.sell = _sell;
		}
	}

	public class Buy
	{
		public string label { get; protected set; }
		public string description { get; protected set; }
		public bool buy { get; protected set; }

		public Buy(string _label, string _description, bool _buy)
		{
			this.label = _label;
			this.description = _description;
			this.buy = _buy;
		}
	}
}
