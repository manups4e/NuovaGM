using System.Collections.Generic;
using CitizenFX.Core;
using TheLastPlanet.Shared;

namespace Impostazioni.Client.Configurazione.Lavori.WhiteList
{
    public class ConfigVenditoriCase
    {
        public ConfigurazioneVendCase Config = new ConfigurazioneVendCase()
        {
            BossActions = new Position(-32.0f, -1114.2f, 25.4f),
            Ingresso = new Position(-199.151f, -575.000f, 39.489f),
            Uscita = new Position(-141.226f, -614.166f, 167.820f),
            Dentro = new Position(-140.969f, -616.785f, 167.820f),
            Fuori = new Position(-202.238f, -578.193f, 39.500f),
            Actions = new Position(-124.786f, -641.486f, 167.820f)
        };

        public Dictionary<string, JobGrade> Gradi = new Dictionary<string, JobGrade>()
        {
            ["Venditore"] = new JobGrade()
            {
                Id = 0,
                Stipendio = 0,
                Vestiti = new()
                {
                    Maschio = new()
                    {
                        Abiti = new(),
                        TextureVestiti = new(),
                        Accessori = new(),
                        TexturesAccessori = new(),
                    },
                    Femmina = new()
                    {
                        Abiti = new(),
                        TextureVestiti = new(),
                        Accessori = new(),
                        TexturesAccessori = new(),
                    }
                }
            },
            ["Direttore"] = new JobGrade()
            {
                Id = 1,
                Stipendio = 0,
                Vestiti = new()
                {
                    Maschio = new()
                    {
                        Abiti = new(),
                        TextureVestiti = new(),
                        Accessori = new(),
                        TexturesAccessori = new(),
                    },
                    Femmina = new()
                    {
                        Abiti = new(),
                        TextureVestiti = new(),
                        Accessori = new(),
                        TexturesAccessori = new(),
                    }
                }
            },
        };
    }
}