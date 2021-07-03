using System.Collections.Generic;
using CitizenFX.Core;
using TheLastPlanet.Client.RolePlay.Lavori;
using TheLastPlanet.Shared;

namespace Impostazioni.Client.Configurazione.Lavori.WhiteList
{
    public class ConfigVenditoriAuto
    {
        public ConfigurazioneVendAuto Config = new ConfigurazioneVendAuto()
        {
            BossActions = new Position(-32.0f, -1114.2f, 25.4f),
            MenuVendita = new Position(-33.7f, -1102.0f, 25.4f),
        };
        public Dictionary<string, JobGrade> Gradi = new Dictionary<string, JobGrade>()
        {
            ["Venditore"] = new JobGrade()
            {
                Id = 0,
                Stipendio = 0,
                Vestiti = new ()
                {
                    Maschio = new ()
                    {
                        Abiti = new(),
                        TextureVestiti = new(),
                        Accessori = new(),
                        TexturesAccessori = new()
                    },
                    Femmina = new ()
                    {
                        Abiti = new(),
                        TextureVestiti = new(),
                        Accessori = new(),
                        TexturesAccessori = new()
                    }
                }
            },
            ["Direttore"] = new JobGrade()
            {
                Id = 1,
                Stipendio = 0,
                Vestiti = new ()
                {
                    Maschio = new ()
                    {
                        Abiti = new(),
                        TextureVestiti = new(),
                        Accessori = new(),
                        TexturesAccessori = new()
                    },
                    Femmina = new ()
                    {
                        Abiti = new(),
                        TextureVestiti = new(),
                        Accessori = new(),
                        TexturesAccessori = new()
                    }
                }
            },
        };
        public Dictionary<string, List<VeicoloCatalogoVenditore>> Catalogo = new Dictionary<string, List<VeicoloCatalogoVenditore>>()
        {
            ["Compacts"] = new() {
                new VeicoloCatalogoVenditore(){name = "blista",	price = 1750, description = ""},
                new VeicoloCatalogoVenditore(){name = "issi2",		price = 1750, description = ""},
                new VeicoloCatalogoVenditore(){name = "panto",	price = 1750, description = ""},
                new VeicoloCatalogoVenditore(){name = "prairie",	price = 1750, description = ""},
            },
            ["Coupés"] = new  () {
                new VeicoloCatalogoVenditore(){name = "exemplar",	price = 1750, description = ""},
                new VeicoloCatalogoVenditore(){name = "f620",	price = 1750, description = ""},
                new VeicoloCatalogoVenditore(){name = "felon",	price = 1750, description = ""},
                new VeicoloCatalogoVenditore(){name = "jackal",	price = 1750, description = ""},
                new VeicoloCatalogoVenditore(){name = "oracle",	price = 1750, description = ""},
                new VeicoloCatalogoVenditore(){name = "sentinel",	price = 1750, description = ""},
            },
            ["Muscle"] = new  () {
                new VeicoloCatalogoVenditore(){name = "buccaneer",	price = 1750, description = ""},
                new VeicoloCatalogoVenditore(){name = "chino",	price = 1750, description = ""},
                new VeicoloCatalogoVenditore(){name = "dominator",	price = 1750, description = ""},
                new VeicoloCatalogoVenditore(){name = "gauntlet",	price = 1750, description = ""},
                new VeicoloCatalogoVenditore(){name = "hotknife",	price = 1750, description = ""},
            },
            ["Moto"] = new() {
                new VeicoloCatalogoVenditore(){name = "akuma",	price = 1750, description = ""},
                new VeicoloCatalogoVenditore(){name = "avarus",	price = 1750, description = ""},
                new VeicoloCatalogoVenditore(){name = "pcj",	price = 1750, description = ""},
                new VeicoloCatalogoVenditore(){name = "bagger",	price = 1750, description = ""},
                new VeicoloCatalogoVenditore(){name = "bati",	price = 1750, description = ""},
                new VeicoloCatalogoVenditore(){name = "hexer",	price = 1750, description = ""},
            },
            ["Off Road"] = new() {
                new VeicoloCatalogoVenditore(){name = "blista",	price = 1750, description = ""},
            },
            ["Sedans"] = new  () {
                new VeicoloCatalogoVenditore(){name = "blista",	price = 1750, description = ""},
            },
            ["Sports"] = new  () {
                new VeicoloCatalogoVenditore(){name = "blista",	price = 1750, description = ""},
            },
            ["Sports Classics"] = new () {
                new VeicoloCatalogoVenditore(){name = "blista",	price = 1750, description = ""},
            },
            ["Super"] = new   () {
                new VeicoloCatalogoVenditore(){name = "blista",	price = 1750, description = ""},
            },
            ["SUVs"] = new() {
                new VeicoloCatalogoVenditore(){name = "blista",	price = 1750, description = ""},
            },
            ["Vans"] = new() {
                new VeicoloCatalogoVenditore(){name = "blista",	price = 1750, description = ""},
            },
            ["Add-on"] = new  () {
                new VeicoloCatalogoVenditore(){name = "blista",	price = 1750, description = ""},
            },
        };
    }
}