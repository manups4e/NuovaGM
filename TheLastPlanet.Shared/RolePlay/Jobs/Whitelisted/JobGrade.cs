using System.Collections.Generic;
using FxEvents.Shared.Attributes;

namespace TheLastPlanet.Shared
{
    
    public class PoliceSettings
    {
        public bool AbilitaBlipVolanti { get; set; }
        public bool AbilitaTimerManette { get; set; }
        public int TimerManette { get; set; }
        public int MaxInServizio { get; set; }
        public bool PuoPlaccare { get; set; }
        public bool AbilitaUsoCani { get; set; }
        public List<CaniPolizia> Cani { get; set; }
        public List<string> ModificheAutorizzate { get; set; }
        public List<StazioniDiPolizia> Stazioni { get; set; }
    }

    
    public class MedicsSettings
    {
        public bool AbilitaBlipVolanti { get; set; }
        public List<Ospedale> Ospedali { get; set; }
    }

    
    public class CarDealerSettings
    {
        public Position BossActions { get; set; }
        public Position MenuVendita { get; set; }
    }
    
    public class RealEstateSettings
    {
        public Position BossActions { get; set; }
        public Position Ingresso { get; set; }
        public Position Uscita { get; set; }
        public Position Dentro { get; set; }
        public Position Fuori { get; set; }
        public Position Actions { get; set; }
    }

    
    public class _Pescatori
    {
        public bool TempoPescaDinamico { get; set; }
        public int TempoFisso { get; set; }
        public int PrezzoVenditaPesce { get; set; }
        public int PrezzoVenditaAltro { get; set; }
        public List<Position> LuoghiVendita { get; set; }
        //public float[] AffittoBarca = new float[3];
        public Position SpawnBarca { get; set; }
        public List<string> Barche { get; set; }
        public _PesciPescati Pesci { get; set; }
    }

    
    public class _PesciPescati
    {
        public List<string> facile { get; set; }
        public List<string> medio { get; set; }
        public List<string> avanzato { get; set; }
    }

    
    public class _Cacciatori
    {
        public Position inizioCaccia { get; set; }
        public Position zonaDiCaccia { get; set; }
        public float limiteArea { get; set; }
    }
    
    public class _Tassisti
    {
        public Position PosAccettazione { get; set; }
        public Position PosDepositoVeicolo { get; set; }
        public Position PosRitiroVeicolo { get; set; }
        public Position PosSpawnVeicolo { get; set; }
        public float PrezzoModifier { get; set; }
        public float pickupRange { get; set; }
        public List<Position> jobCoords { get; set; }
    }

    
    public class _Towing
    {
        public Position InizioLavoro { get; set; }
        public List<Position> PuntiDespawn { get; set; }
        public List<string> VeicoliDaRimorchiare { get; set; }
        public List<Position> SpawnVeicoli { get; set; }
    }

    
    public class WhiteListed
    {
        public Dictionary<string, JobGrade> Polizia { get; set; }
        public Dictionary<string, JobGrade> Medico { get; set; }
        public Dictionary<string, JobGrade> Meccanico { get; set; }

    }

    
    public class JobGrade
    {
        public int Id { get; set; }
        public int Stipendio { get; set; }
        public AbitiLavoro Vestiti { get; set; }
    }
    
    public class AbitiLavoro
    {
        public AbitiLav Maschio { get; set; }
        public AbitiLav Femmina { get; set; }
    }
    
    public class AbitiLav
    {
        public ComponentDrawables Abiti { get; set; }
        public ComponentDrawables TextureVestiti { get; set; }
        public PropIndices Accessori { get; set; }
        public PropIndices TexturesAccessori { get; set; }
    }

    
    public class CaniPolizia
    {
        public string Nome { get; set; }
        public string Model { get; set; }
    }

    
    public class Autorizzati
    {
        public string Nome { get; set; }
        public string Model { get; set; }
        public List<int> GradiAutorizzati { get; set; }
    }


    
    public class StazioniDiPolizia
    {
        public BlipLavoro Blip { get; set; }
        public List<Position> Spogliatoio { get; set; }
        public List<Position> Armerie { get; set; }
        public List<Autorizzati> VeicoliAutorizzati { get; set; }
        public List<Autorizzati> ElicotteriAutorizzati { get; set; }
        public List<Autorizzati> ArmiAutorizzate { get; set; }
        public List<SpawnerSpawn> Veicoli { get; set; }
        public List<SpawnerSpawn> Elicotteri { get; set; }
        public List<Position> BossActions { get; set; }
    }

    
    public class Ospedale
    {
        public BlipLavoro Blip { get; set; }
        public List<Position> Spogliatoio { get; set; }
        public List<Position> Farmacia { get; set; }
        public List<Position> IngressoVisitatori { get; set; }
        public List<Position> UscitaVisitatori { get; set; }
        public List<Autorizzati> VeicoliAutorizzati { get; set; }
        public List<Autorizzati> ElicotteriAutorizzati { get; set; }
        public List<SpawnerSpawn> Veicoli { get; set; }
        public List<SpawnerSpawn> Elicotteri { get; set; }
        public List<Position> AzioniCapo { get; set; }
    }

    
    public class BlipLavoro
    {
        public Position Coords { get; set; }
        public int Sprite { get; set; }
        public int Display { get; set; }
        public float Scale { get; set; }
        public int Color { get; set; }
        public string Nome { get; set; }
    }
    
    public class SpawnerSpawn
    {
        public Position SpawnerMenu { get; set; }
        public List<SpawnPoints> SpawnPoints { get; set; }
        public List<Position> Deleters { get; set; }
    }

    
    public class SpawnPoints
    {
        public Position Coords { get; set; }
        public float Heading { get; set; }
        public float Radius { get; set; }
    }
}