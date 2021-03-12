using System.Collections.Generic;
using TheLastPlanet.Shared.Veicoli;

namespace Impostazioni.Client.Configurazione.Veicoli
{
    public class VeicoliAffitto
    {
        public List<Veicoloaff> biciclette = new()
        {
            new Veicoloaff("Whippet Racemoto", 1750, "", "tribike"),
            new Veicoloaff("Cruiser", 950, "", "cruiser"),
            new Veicoloaff("BMX", 800, "", "BMX"),
            new Veicoloaff("Scorcher", 1000, "", "SCORCHER"),
        };
        public List<Veicoloaff> macchineGeneric = new()
        {
            new Veicoloaff("Blista", 4000, "", "blista"),
        };
        public List<Veicoloaff> macchineMedium = new()
        {
            new Veicoloaff("Neon", 40000, "", "neon"),
        };
        public List<Veicoloaff> macchineSuper = new()
        {
            new Veicoloaff("Zentorno", 400000, "", "zentorno"),
            new Veicoloaff("Tezeract", 400000, "", "tezeract"),
        };
        public List<Veicoloaff> motoGeneric = new()
        {
            new Veicoloaff("Faggio", 4000, "", "faggio2"),
            new Veicoloaff("Faggio Sport", 6500, "", "faggio"),
            new Veicoloaff("Faggio Mod", 8000, "", "Faggio3"),
            new Veicoloaff("Ratbike", 15000, "", "Ratbike"),
            new Veicoloaff("Sanchez", 20000, "", "sanchez"),
            new Veicoloaff("Vader", 29000, "", "vader"),
            new Veicoloaff("Carbon RS", 32000, "", "carbonrs"),
            new Veicoloaff("Akuma", 43000, "", "akuma"),
            new Veicoloaff("Hexer", 43000, "", "hexer"),
            new Veicoloaff("PCJ-600", 47000, "", "pcj"),
        };
        public List<Veicoloaff> motoMedium = new()
        {
            new Veicoloaff("Ruffian", 50000, "", "ruffian"),
            new Veicoloaff("Nemesis", 51000, "", "nemesis"),
            new Veicoloaff("Bagger", 55000, "", "bagger"),
            new Veicoloaff("Daemon", 56000, "", "daemon"),
            new Veicoloaff("Sovereign", 60000, "", "sovereign"),
            new Veicoloaff("Thrust", 60000, "", "thrust"),
            new Veicoloaff("Double T", 63000, "", "double"),
            new Veicoloaff("Daemon DLC", 65000, "", "Daemon2"),
            new Veicoloaff("Lectro", 70000, "", "lectro"),
            new Veicoloaff("Enduro", 75000, "", "enduro"),
            new Veicoloaff("Wolfsbane", 75000, "", "wolfsbane"),
            new Veicoloaff("Esskey", 78500, "", "Esskey"),
            new Veicoloaff("FCR1000", 87200, "", "FCR"),
            new Veicoloaff("Avarus", 88000, "", "Avarus"),
            new Veicoloaff("Cliffhanger", 90000, "", "cliffhanger"),
            new Veicoloaff("Vortex", 92000, "", "Vortex"),
            new Veicoloaff("Diablous", 95000, "", "Diablous"),
            new Veicoloaff("Nightblade", 95200, "", "Nightblade"),
            new Veicoloaff("FCR1000 Custom", 99500, "", "FCR2"),
        };
        public List<Veicoloaff> motoSuper = new()
        {
            new Veicoloaff("Chimera", 100000, "", "Chimera"),
            new Veicoloaff("Diablous Custom", 115000, "", "Diablous2"),
            new Veicoloaff("HAKUCHOU Drag", 650000, "", "Hakuchou2"),
            new Veicoloaff("Shotaro", 1200000, "", "Shotaro"),
            new Veicoloaff("Zombie Bobber", 103000, "", "Zombiea"),
            new Veicoloaff("Zombie Chopper", 110000, "", "Zombieb"),
            new Veicoloaff("Bati 801", 102000, "", "bati"),
            new Veicoloaff("Bati 801RR", 125000, "", "bati2"),
            new Veicoloaff("Gargoyle", 120000, "", "gargoyle"),
            new Veicoloaff("Hakuchou", 175000, "", "hakuchou"),
            new Veicoloaff("Innovation", 100000, "", "innovation"),
            new Veicoloaff("Sanctus", 280000, "", "sanctus"),
            new Veicoloaff("Vindicator", 210000, "", "vindicator"),
        };
    }
}