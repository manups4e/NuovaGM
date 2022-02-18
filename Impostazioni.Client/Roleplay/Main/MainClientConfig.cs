using Impostazioni.Client.Configurazione.Negozi.Abiti;
using Impostazioni.Client.Configurazione.Negozi.Barbieri;
using Impostazioni.Client.Configurazione.Negozi.Generici;
using Impostazioni.Client.Configurazione.Veicoli;
using Impostazioni.Shared.Roleplay.Lavori.WhiteList;
using System.Collections.Generic;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Core;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Lavori;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Proprietà.Hotel;

namespace Impostazioni.Client.Configurazione.Main
{
    public class ConfPrincipaleRP
    {
        public string NomeServer { get; set; }
        public string DiscordAppId { get; set; }
        public string DiscordRichPresenceAsset { get; set; }
        public bool PassengerDriveBy { get; set; }
        public bool KickWarning { get; set; }
        public int AFKCheckTime { get; set; }
        public Vector4 Firstcoords { get; set; }
        public int ReviveReward { get; set; }
        public bool EarlyRespawn { get; set; }
        public bool EarlyRespawnFine { get; set; }
        public int EarlyRespawnFineAmount { get; set; }
        public int EarlySpawnTimer { get; set; }
        public int BleedoutTimer { get; set; }
        public bool EscludiMirino { get; set; }
        public List<WeaponHash> ScopedWeapons { get; set; }
        public Dictionary<long, float> recoils { get; set; }

        public List<string> pickupList { get; set; }

        public List<GenericPeds> stripClub { get; set; }
        public List<GenericPeds> blackMarket { get; set; }
        public List<GenericPeds> illegal_weapon_extra_shop { get; set; }
        public float baseTraffic { get; set; }
        public float divMultiplier { get; set; }
    }

    public class ClientConfigKVP
    {
        public bool ModCinema { get; set; }
        public float LetterBox { get; set; }
        public string Filtro { get; set; }
        public float FiltroStrenght { get; set; }
        public bool MiniMappaAttiva { get; set; }
        public int DimensioniMinimappa { get; set; }
        public bool MiniMappaInAuto { get; set; }
        public bool MostraContattiTelefonoInMappa { get; set; } // da valutare
        public bool ForzaPrimaPersona_Mira { get; set; }
        public bool ForzaPrimaPersona_InCopertura { get; set; }
        public bool ForzaPrimaPersona_InAuto { get; set; }

        public ClientConfigKVP()
        {
            ModCinema = false;
            LetterBox = 0f;
            Filtro = "None";
            FiltroStrenght = 0.5f;
            MiniMappaAttiva = true;
            DimensioniMinimappa = 0;
            MiniMappaInAuto = true;
            MostraContattiTelefonoInMappa = false;
            ForzaPrimaPersona_Mira = false;
            ForzaPrimaPersona_InCopertura = false;
            ForzaPrimaPersona_InAuto = false;
        }
    }
}