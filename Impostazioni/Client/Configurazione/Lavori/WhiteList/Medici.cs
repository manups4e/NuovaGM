using System.Collections.Generic;
using CitizenFX.Core;
using TheLastPlanet.Shared;

namespace Impostazioni.Client.Configurazione.Lavori.WhiteList
{
    public class ConfigMedici
    {
        public ConfigurazioneMedici Config = new()
        {
            Ospedali = new List<Ospedale>()
            {
                new Ospedale()
                {
                    Blip = new BlipLavoro()
                    {
                        Coords = new Position(307.7f, -1433.4f, 28.9f),
                        Sprite = 61,
                        Display = 4,
                        Scale = 1.2f,
                        Color = 2,
                        Nome = "Ospedale Centrale di Los Santos"
                    },
                    Spogliatoio = new List<Position>() {new Position(270.5f, -1363.0f, 23.5f)},
                    Farmacia = new List<Position>() {new(230.1f, -1366.1f, 38.5f)},
                    IngressoVisitatori = new List<Position>()
                    {
                        new(344.114f, -1397.703f, 31.509f),
                        new(342.956f, -1399.569f, 31.509f),
                        new(390.057f, -1433.125f, 28.432f),
                        new(391.464f, -1431.332f, 28.432f)
                    },
                    UscitaVisitatori = new List<Position>()
                    {
                        new(275.515f, -1361.274f, 23.538f),
                        new(254.465f, -1372.403f, 23.538f)
                    },
                    Veicoli = new List<SpawnerSpawn>()
                    {
                        new SpawnerSpawn()
                        {
                            SpawnerMenu = new Position(246.876f, -1371.816f, 24.538f),
                            SpawnPoints = new List<SpawnPoints>()
                            {
                                new() {Coords = new(297.2f, -1429.5f, 29.8f), Heading = 227.6f, Radius = 4f},
                                new() {Coords = new(294.0f, -1433.1f, 29.8f), Heading = 227.6f, Radius = 4f},
                                new() {Coords = new(309.4f, -1442.5f, 29.8f), Heading = 227.6f, Radius = 6f}
                            },
                            Deleters =
                            {
                                new(323.907f, -1473.516f, 29.799f),
                                new(326.377f, -1469.785f, 29.799f)
                            }
                        },
                    },
                    Elicotteri = new List<SpawnerSpawn>()
                    {
                        new SpawnerSpawn()
                        {
                            SpawnerMenu = new Position(317.5f, -1449.5f, 46.5f),
                            SpawnPoints =
                            {
                                new SpawnPoints()
                                    {Coords = new Position(313.5f, -1465.1f, 46.5f), Heading = 142.7f, Radius = 10f},
                                new SpawnPoints()
                                    {Coords = new Position(299.5f, -1453.2f, 46.5f), Heading = 142.7f, Radius = 10f}
                            },
                            Deleters =
                            {
                                new Position(313.5f, -1465.1f, 46.5f),
                                new Position(299.5f, -1453.2f, 46.5f)
                            }
                        }
                    },
                    VeicoliAutorizzati = new List<Autorizzati>()
                    {
                        new Autorizzati() {Nome = "Ambulanza", Model = "ambulance", GradiAutorizzati = {-1}},
                    },
                    ElicotteriAutorizzati = new List<Autorizzati>()
                    {
                        new() {Nome = "Maverik Medici", Model = "polmav", GradiAutorizzati = {-1}},
                    }
                }
            }
        };
        public Dictionary<string, JobGrade> Gradi = new()
        {
            ["Paramedico"] = new JobGrade()
            {
                Id = 0,
                Stipendio = 0,
                Vestiti = new()
                {
                    Maschio = new ()
                    {
                        Abiti = new ()
                        {
                            Faccia = -1, Maschera = 0, Capelli = -1, Torso = 90, Pantaloni = 96, Borsa_Paracadute = -1,
                            Scarpe = 51, Accessori = 126, Sottomaglia = 15, Kevlar = 0, Badge = 57, Torso_2 = 249
                        },
                        TextureVestiti = new ()
                        {
                            Faccia = -1, Maschera = 0, Capelli = -1, Torso = 0, Pantaloni = 0, Borsa_Paracadute = -1,
                            Scarpe = 0, Accessori = 0, Sottomaglia = 0, Kevlar = 0, Badge = 0, Torso_2 = 0
                        },
                        Accessori = new ()
                        {
                            Cappelli_Maschere = 122, Orecchie = -1, Occhiali_Occhi = -1, Unk_3 = -1, Unk_4 = -1,
                            Unk_5 = -1, Orologi = -1, Bracciali = -1, Unk_8 = -1
                        },
                        TexturesAccessori = new ()
                        {
                            Cappelli_Maschere = 0, Orecchie = -1, Occhiali_Occhi = -1, Unk_3 = -1, Unk_4 = -1,
                            Unk_5 = -1, Orologi = -1, Bracciali = -1, Unk_8 = -1
                        }
                    },
                    Femmina = new ()
                    {
                        Abiti = new ()
                        {
                            Faccia = -1, Maschera = 0, Capelli = -1, Torso = 109, Pantaloni = 99, Borsa_Paracadute = -1,
                            Scarpe = 52, Accessori = 97, Sottomaglia = 159, Kevlar = 0, Badge = 66, Torso_2 = 258
                        },
                        TextureVestiti = new ()
                        {
                            Faccia = -1, Maschera = 0, Capelli = -1, Torso = 0, Pantaloni = 0, Borsa_Paracadute = -1,
                            Scarpe = 0, Accessori = 0, Sottomaglia = 0, Kevlar = 0, Badge = 0, Torso_2 = 0
                        },
                        Accessori = new ()
                        {
                            Cappelli_Maschere = 121, Orecchie = -1, Occhiali_Occhi = -1, Unk_3 = -1, Unk_4 = -1,
                            Unk_5 = -1, Orologi = -1, Bracciali = -1, Unk_8 = -1
                        },
                        TexturesAccessori = new ()
                        {
                            Cappelli_Maschere = 0, Orecchie = -1, Occhiali_Occhi = -1, Unk_3 = -1, Unk_4 = -1,
                            Unk_5 = -1, Orologi = -1, Bracciali = -1, Unk_8 = -1
                        }
                    }
                }
            },
            ["Specializzando"] = new JobGrade()
            {
                Id = 1,
                Stipendio = 0,
                Vestiti = new()
                {
                    Maschio = new ()
                    {
                        Abiti = new ()
                        {
                            Faccia = -1, Maschera = 0, Capelli = -1, Torso = 90, Pantaloni = 96, Borsa_Paracadute = -1,
                            Scarpe = 51, Accessori = 126, Sottomaglia = 15, Kevlar = 0, Badge = 57, Torso_2 = 249
                        },
                        TextureVestiti = new ()
                        {
                            Faccia = -1, Maschera = 0, Capelli = -1, Torso = 0, Pantaloni = 0, Borsa_Paracadute = -1,
                            Scarpe = 0, Accessori = 0, Sottomaglia = 0, Kevlar = 0, Badge = 0, Torso_2 = 0
                        },
                        Accessori = new ()
                        {
                            Cappelli_Maschere = 122, Orecchie = -1, Occhiali_Occhi = -1, Unk_3 = -1, Unk_4 = -1,
                            Unk_5 = -1, Orologi = -1, Bracciali = -1, Unk_8 = -1
                        },
                        TexturesAccessori = new ()
                        {
                            Cappelli_Maschere = 0, Orecchie = -1, Occhiali_Occhi = -1, Unk_3 = -1, Unk_4 = -1,
                            Unk_5 = -1, Orologi = -1, Bracciali = -1, Unk_8 = -1
                        }
                    },
                    Femmina = new ()
                    {
                        Abiti = new ()
                        {
                            Faccia = -1, Maschera = 0, Capelli = -1, Torso = 105, Pantaloni = 99, Borsa_Paracadute = -1,
                            Scarpe = 52, Accessori = 96, Sottomaglia = 14, Kevlar = 0, Badge = 65, Torso_2 = 257
                        },
                        TextureVestiti = new ()
                        {
                            Faccia = -1, Maschera = 0, Capelli = -1, Torso = 0, Pantaloni = 1, Borsa_Paracadute = -1,
                            Scarpe = 0, Accessori = 0, Sottomaglia = 0, Kevlar = 0, Badge = 0, Torso_2 = 1
                        },
                        Accessori = new ()
                        {
                            Cappelli_Maschere = 121, Orecchie = -1, Occhiali_Occhi = -1, Unk_3 = -1, Unk_4 = -1,
                            Unk_5 = -1, Orologi = -1, Bracciali = -1, Unk_8 = -1
                        },
                        TexturesAccessori = new ()
                        {
                            Cappelli_Maschere = 0, Orecchie = -1, Occhiali_Occhi = -1, Unk_3 = -1, Unk_4 = -1,
                            Unk_5 = -1, Orologi = -1, Bracciali = -1, Unk_8 = -1
                        }
                    }
                }
            },
            ["Infermiere"] = new JobGrade()
            {
                Id = 2,
                Stipendio = 0,
                Vestiti = new()
                {
                    Maschio = new ()
                    {
                        Abiti = new ()
                        {
                            Faccia = -1, Maschera = 0, Capelli = -1, Torso = 85, Pantaloni = 96, Borsa_Paracadute = -1,
                            Scarpe = 51, Accessori = 127, Sottomaglia = 129, Kevlar = 0, Badge = 58, Torso_2 = 250
                        },
                        TextureVestiti = new ()
                        {
                            Faccia = -1, Maschera = 0, Capelli = -1, Torso = 0, Pantaloni = 0, Borsa_Paracadute = -1,
                            Scarpe = 0, Accessori = 0, Sottomaglia = 0, Kevlar = 0, Badge = 0, Torso_2 = 0
                        },
                        Accessori = new ()
                        {
                            Cappelli_Maschere = 122, Orecchie = -1, Occhiali_Occhi = -1, Unk_3 = -1, Unk_4 = -1,
                            Unk_5 = -1, Orologi = -1, Bracciali = -1, Unk_8 = -1
                        },
                        TexturesAccessori = new ()
                        {
                            Cappelli_Maschere = 0, Orecchie = -1, Occhiali_Occhi = -1, Unk_3 = -1, Unk_4 = -1,
                            Unk_5 = -1, Orologi = -1, Bracciali = -1, Unk_8 = -1
                        }
                    },
                    Femmina = new ()
                    {
                        Abiti = new ()
                        {
                            Faccia = -1, Maschera = 0, Capelli = -1, Torso = 109, Pantaloni = 99, Borsa_Paracadute = -1,
                            Scarpe = 52, Accessori = 97, Sottomaglia = 159, Kevlar = 0, Badge = 66, Torso_2 = 258
                        },
                        TextureVestiti = new ()
                        {
                            Faccia = -1, Maschera = 0, Capelli = -1, Torso = 0, Pantaloni = 1, Borsa_Paracadute = -1,
                            Scarpe = 0, Accessori = 0, Sottomaglia = 0, Kevlar = 0, Badge = 1, Torso_2 = 1
                        },
                        Accessori = new ()
                        {
                            Cappelli_Maschere = 121, Orecchie = -1, Occhiali_Occhi = -1, Unk_3 = -1, Unk_4 = -1,
                            Unk_5 = -1, Orologi = -1, Bracciali = -1, Unk_8 = -1
                        },
                        TexturesAccessori = new ()
                        {
                            Cappelli_Maschere = 0, Orecchie = -1, Occhiali_Occhi = -1, Unk_3 = -1, Unk_4 = -1,
                            Unk_5 = -1, Orologi = -1, Bracciali = -1, Unk_8 = -1
                        }
                    }
                }
            },
            ["Dottore"] = new JobGrade()
            {
                Id = 3,
                Stipendio = 0,
                Vestiti = new()
                {
                    Maschio = new ()
                    {
                        Abiti = new ()
                        {
                            Faccia = -1, Maschera = 0, Capelli = -1, Torso = 90, Pantaloni = 96, Borsa_Paracadute = -1,
                            Scarpe = 51, Accessori = 126, Sottomaglia = 15, Kevlar = 0, Badge = 57, Torso_2 = 249
                        },
                        TextureVestiti = new ()
                        {
                            Faccia = -1, Maschera = 0, Capelli = -1, Torso = 0, Pantaloni = 1, Borsa_Paracadute = -1,
                            Scarpe = 0, Accessori = 0, Sottomaglia = 0, Kevlar = 0, Badge = 0, Torso_2 = 1
                        },
                        Accessori = new ()
                        {
                            Cappelli_Maschere = 122, Orecchie = -1, Occhiali_Occhi = -1, Unk_3 = -1, Unk_4 = -1,
                            Unk_5 = -1, Orologi = -1, Bracciali = -1, Unk_8 = -1
                        },
                        TexturesAccessori = new ()
                        {
                            Cappelli_Maschere = 1, Orecchie = -1, Occhiali_Occhi = -1, Unk_3 = -1, Unk_4 = -1,
                            Unk_5 = -1, Orologi = -1, Bracciali = -1, Unk_8 = -1
                        }
                    },
                    Femmina = new ()
                    {
                        Abiti = new ()
                        {
                            Faccia = -1, Maschera = 95, Capelli = -1, Torso = 23, Pantaloni = 75, Borsa_Paracadute = -1,
                            Scarpe = 54, Accessori = 0, Sottomaglia = 67, Kevlar = 0, Badge = 0, Torso_2 = 76
                        },
                        TextureVestiti = new ()
                        {
                            Faccia = -1, Maschera = 7, Capelli = -1, Torso = 0, Pantaloni = 0, Borsa_Paracadute = -1,
                            Scarpe = 4, Accessori = 0, Sottomaglia = 1, Kevlar = 0, Badge = 0, Torso_2 = 0
                        },
                        Accessori = new ()
                        {
                            Cappelli_Maschere = -1, Orecchie = -1, Occhiali_Occhi = -1, Unk_3 = -1, Unk_4 = -1,
                            Unk_5 = -1, Orologi = -1, Bracciali = -1, Unk_8 = -1
                        },
                        TexturesAccessori = new ()
                        {
                            Cappelli_Maschere = -1, Orecchie = -1, Occhiali_Occhi = -1, Unk_3 = -1, Unk_4 = -1,
                            Unk_5 = -1, Orologi = -1, Bracciali = -1, Unk_8 = -1
                        }
                    }
                }
            },
            ["Primario"] = new JobGrade()
            {
                Id = 4,
                Stipendio = 0,
                Vestiti = new()
                {
                    Maschio = new ()
                    {
                        Abiti = new ()
                        {
                            Faccia = -1, Maschera = 0, Capelli = -1, Torso = 85, Pantaloni = 96, Borsa_Paracadute = -1,
                            Scarpe = 51, Accessori = 127, Sottomaglia = 129, Kevlar = 0, Badge = 58, Torso_2 = 250
                        },
                        TextureVestiti = new ()
                        {
                            Faccia = -1, Maschera = 0, Capelli = -1, Torso = 0, Pantaloni = 1, Borsa_Paracadute = -1,
                            Scarpe = 0, Accessori = 0, Sottomaglia = 0, Kevlar = 0, Badge = 1, Torso_2 = 1
                        },
                        Accessori = new ()
                        {
                            Cappelli_Maschere = 122, Orecchie = -1, Occhiali_Occhi = -1, Unk_3 = -1, Unk_4 = -1,
                            Unk_5 = -1, Orologi = -1, Bracciali = -1, Unk_8 = -1
                        },
                        TexturesAccessori = new ()
                        {
                            Cappelli_Maschere = 1, Orecchie = -1, Occhiali_Occhi = -1, Unk_3 = -1, Unk_4 = -1,
                            Unk_5 = -1, Orologi = -1, Bracciali = -1, Unk_8 = -1
                        }
                    },
                    Femmina = new ()
                    {
                        Abiti = new ()
                        {
                            Faccia = -1, Maschera = 95, Capelli = -1, Torso = 23, Pantaloni = 75, Borsa_Paracadute = -1,
                            Scarpe = 54, Accessori = 0, Sottomaglia = 67, Kevlar = 0, Badge = 0, Torso_2 = 76
                        },
                        TextureVestiti = new ()
                        {
                            Faccia = -1, Maschera = 7, Capelli = -1, Torso = 0, Pantaloni = 0, Borsa_Paracadute = -1,
                            Scarpe = 4, Accessori = 0, Sottomaglia = 1, Kevlar = 0, Badge = 0, Torso_2 = 0
                        },
                        Accessori = new ()
                        {
                            Cappelli_Maschere = -1, Orecchie = -1, Occhiali_Occhi = -1, Unk_3 = -1, Unk_4 = -1,
                            Unk_5 = -1, Orologi = -1, Bracciali = -1, Unk_8 = -1
                        },
                        TexturesAccessori = new ()
                        {
                            Cappelli_Maschere = -1, Orecchie = -1, Occhiali_Occhi = -1, Unk_3 = -1, Unk_4 = -1,
                            Unk_5 = -1, Orologi = -1, Bracciali = -1, Unk_8 = -1
                        }
                    }
                }
            }
        };
    }
}