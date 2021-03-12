using System.Collections.Generic;
using CitizenFX.Core;
using TheLastPlanet.Shared;

namespace Impostazioni.Client.Configurazione.Lavori.WhiteList
{
    public class ConfigPolizia
    {
		public ConfigurazionePolizia Config = new()
		{
			AbilitaBlipVolanti = true,
			AbilitaTimerManette = false,
			TimerManette = 600000,
			MaxInServizio = -1,
			PuoPlaccare = true,
			AbilitaUsoCani = true,

			Cani = new List<CaniPolizia>()
			{
				new() {Nome = "Rottweiler", Model = "a_c_rottweiler"},
				new() {Nome = "Husky", Model = "a_c_husky"},
				new() {Nome = "Pastore Tedesco", Model = "a_c_shepherd"},
				new() {Nome = "Retriver", Model = "a_c_retriever"},
			},

			ModificheAutorizzate = new List<string>()
				{"COMPONENT_AT_SCOPE_MACRO", "COMPONENT_AT_PI_FLSH"},

			Stazioni = new List<StazioniDiPolizia>()
			{
				new()
				{
					Blip = new BlipLavoro()
					{
						Coords = new Vector3(425.1f, -979.5f, 30.7f),
						Sprite = 60,
						Display = 4,
						Scale = 1f,
						Color = 29,
						Nome = "Stazione di Polizia"
					},

					Spogliatoio = new List<Vector3>() {new(452.6f, -992.8f, 29.69f)},
					Armerie = new List<Vector3>() {new(451.7f, -980.1f, 29.69f)},
					VeicoliAutorizzati = new List<Autorizzati>()
					{
						new() {Nome = "Macchina della Polizia 1", Model = "police", GradiAutorizzati = {-1}},
						new() {Nome = "Macchina della Polizia 2", Model = "police2", GradiAutorizzati = {-1}},
						new() {Nome = "Macchina della Polizia 3", Model = "police3", GradiAutorizzati = {-1}},
						new() {Nome = "Macchina della Polizia 4", Model = "police4", GradiAutorizzati = {-1}},
						new() {Nome = "Moto della Polizia", Model = "policeb", GradiAutorizzati = {-1}},
						new() {Nome = "Furgone Polizia 1", Model = "policet", GradiAutorizzati = {-1}},
						new() {Nome = "SUV Sceriffo 1", Model = "sheriff2", GradiAutorizzati = {-1}},
						new() {Nome = "SUV Ranger", Model = "pranger", GradiAutorizzati = {-1}},
						new() {Nome = "FIB 1", Model = "fbi", GradiAutorizzati = {-1}},
						new() {Nome = "FIB 2", Model = "fbi2", GradiAutorizzati = {-1}},
						new() {Nome = "Macchina Sceriffo 1", Model = "sheriff", GradiAutorizzati = {-1}},
						new() {Nome = "SUV Sceriffo 1", Model = "sheriff2", GradiAutorizzati = {-1}},
					},
					ElicotteriAutorizzati = new List<Autorizzati>()
					{
						new() {Nome = "Maverik Polizia", Model = "polmav", GradiAutorizzati = {-1}},
						new() {Nome = "Annihilator", Model = "annihilator", GradiAutorizzati = {-1}},
						new() {Nome = "Buzzard ~g~non armato~w~", Model = "buzzard2", GradiAutorizzati = {-1}},
						new() {Nome = "Buzzard ~r~armato~w~", Model = "buzzard", GradiAutorizzati = {-1}},
						new() {Nome = "Valchiria", Model = "Valkyrie", GradiAutorizzati = {-1}}
					},
					ArmiAutorizzate = new List<Autorizzati>()
					{
						new() {Nome = "Tazer", Model = "WEAPON_STUNGUN", GradiAutorizzati = {-1}},
						new() {Nome = "Pistola", Model = "WEAPON_PISTOL", GradiAutorizzati = {-1}}
					},

					Veicoli = new List<SpawnerSpawn>()
					{
						new()
						{
							SpawnerMenu = new Vector3(470.396f, -984.783f, 30.69f),
							SpawnPoints = new List<SpawnPoints>()
							{
								new() {Coords = new Vector3(438.4f, -1018.3f, 27.7f), Heading = 90f, Radius = 6f},
								new() {Coords = new Vector3(441.0f, -1024.2f, 28.3f), Heading = 90f, Radius = 6f},
								new() {Coords = new Vector3(453.5f, -1022.2f, 28.0f), Heading = 90f, Radius = 6f},
								new() {Coords = new Vector3(450.9f, -1016.5f, 28.1f), Heading = 90f, Radius = 6f}
							},
							Deleters = new List<Vector3>()
							{
								new Vector3(452.567f, -996.697f, 25.763f),
								new Vector3(447.449f, -996.954f, 25.763f)
							}
						}
					},
					Elicotteri = new List<SpawnerSpawn>()
					{
						new SpawnerSpawn()
						{
							SpawnerMenu = new Vector3(461.1f, -981.5f, 43.6f),
							SpawnPoints = new List<SpawnPoints>()
							{
								new() {Coords = new Vector3(449.5f, -981.2f, 43.6f), Heading = 92.6f, Radius = 10f}
							},
							Deleters = new List<Vector3>()
							{
								new Vector3(449.5f, -981.2f, 43.6f),
							}
						}
					},
					BossActions = new List<Vector3>()
					{
						new(448.4f, -973.2f, 29.69f)
					}
				}
			}
		};

		public Dictionary<string, JobGrade> Gradi = new Dictionary<string, JobGrade>()
		{
			["Recluta"] = new()
			{
				Id = 0,
				Stipendio = 0,
				Vestiti = new AbitiLavoro()
				{
					Maschio = new AbitiLav()
					{
						Abiti = new ComponentDrawables()
						{
							Faccia = -1, Maschera = 0, Capelli = -1, Torso = 0, Pantaloni = 35, Borsa_Paracadute = -1,
							Scarpe = 25, Accessori = 0, Sottomaglia = 58, Kevlar = 0, Badge = 0, Torso_2 = 55
						},
						TextureVestiti = new ()
						{
							Faccia = -1, Maschera = 0, Capelli = -1, Torso = 0, Pantaloni = 0, Borsa_Paracadute = -1,
							Scarpe = 0, Accessori = 0, Sottomaglia = 0, Kevlar = 0, Badge = 0, Torso_2 = 0
						},
						Accessori = new ()
						{
							Cappelli_Maschere = -1, Orecchie = 9, Occhiali_Occhi = -1, Unk_3 = -1, Unk_4 = -1,
							Unk_5 = -1, Orologi = -1, Bracciali = -1, Unk_8 = -1
						},
						TexturesAccessori = new ()
						{
							Cappelli_Maschere = -1, Orecchie = 0, Occhiali_Occhi = -1, Unk_3 = -1, Unk_4 = -1,
							Unk_5 = -1, Orologi = -1, Bracciali = -1, Unk_8 = -1
						}
					},
					Femmina = new ()
					{
						Abiti = new ()
						{
							Faccia = -1, Maschera = 0, Capelli = -1, Torso = 14, Pantaloni = 3, Borsa_Paracadute = -1,
							Scarpe = 3, Accessori = 0, Sottomaglia = 2, Kevlar = 0, Badge = 0, Torso_2 = 49
						},
						TextureVestiti = new ()
						{
							Faccia = -1, Maschera = 0, Capelli = -1, Torso = 0, Pantaloni = 15, Borsa_Paracadute = -1,
							Scarpe = 1, Accessori = 0, Sottomaglia = 0, Kevlar = 0, Badge = 0, Torso_2 = 0
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
			["Agente"] = new JobGrade()
			{
				Id = 1,
				Stipendio = 0,
				Vestiti = new()
				{
					Maschio = new ()
					{
						Abiti = new ()
						{
							Faccia = -1, Maschera = 0, Capelli = -1, Torso = 0, Pantaloni = 35, Borsa_Paracadute = 0,
							Scarpe = 25, Accessori = 0, Sottomaglia = 58, Kevlar = 0, Badge = 0, Torso_2 = 55
						},
						TextureVestiti = new ()
						{
							Faccia = -1, Maschera = 0, Capelli = -1, Torso = 0, Pantaloni = 0, Borsa_Paracadute = 0,
							Scarpe = 0, Accessori = 0, Sottomaglia = 0, Kevlar = 0, Badge = 0, Torso_2 = 0
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
					},
					Femmina = new ()
					{
						Abiti = new ()
						{
							Faccia = -1, Maschera = 0, Capelli = -1, Torso = 4, Pantaloni = 3, Borsa_Paracadute = 0,
							Scarpe = 1, Accessori = 0, Sottomaglia = 3, Kevlar = 0, Badge = 0, Torso_2 = 5
						},
						TextureVestiti = new ()
						{
							Faccia = -1, Maschera = 0, Capelli = -1, Torso = 0, Pantaloni = 0, Borsa_Paracadute = 3,
							Scarpe = 0, Accessori = 0, Sottomaglia = 0, Kevlar = 0, Badge = 0, Torso_2 = 0
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
			["Sergente"] = new JobGrade()
			{
				Id = 2,
				Stipendio = 0,
				Vestiti = new()
				{
					Maschio = new ()
					{
						Abiti = new ()
						{
							Faccia = -1, Maschera = 0, Capelli = -1, Torso = 0, Pantaloni = 35, Borsa_Paracadute = -1,
							Scarpe = 25, Accessori = 0, Sottomaglia = 58, Kevlar = 0, Badge = 0, Torso_2 = 55
						},
						TextureVestiti = new ()
						{
							Faccia = -1, Maschera = 0, Capelli = -1, Torso = 0, Pantaloni = 0, Borsa_Paracadute = -1,
							Scarpe = 0, Accessori = 0, Sottomaglia = 0, Kevlar = 0, Badge = 0, Torso_2 = 0
						},
						Accessori = new ()
						{
							Cappelli_Maschere = 46, Orecchie = 9, Occhiali_Occhi = -1, Unk_3 = -1, Unk_4 = -1,
							Unk_5 = -1, Orologi = -1, Bracciali = -1, Unk_8 = -1
						},
						TexturesAccessori = new ()
						{
							Cappelli_Maschere = 0, Orecchie = 0, Occhiali_Occhi = -1, Unk_3 = -1, Unk_4 = -1,
							Unk_5 = -1, Orologi = -1, Bracciali = -1, Unk_8 = -1
						}
					},
					Femmina = new ()
					{
						Abiti = new ()
						{
							Faccia = -1, Maschera = 0, Capelli = -1, Torso = 23, Pantaloni = 38, Borsa_Paracadute = -1,
							Scarpe = 49, Accessori = 0, Sottomaglia = 14, Kevlar = 0, Badge = 0, Torso_2 = 59
						},
						TextureVestiti = new ()
						{
							Faccia = -1, Maschera = 0, Capelli = -1, Torso = 0, Pantaloni = 0, Borsa_Paracadute = -1,
							Scarpe = 0, Accessori = 0, Sottomaglia = 0, Kevlar = 0, Badge = 0, Torso_2 = 0
						},
						Accessori = new ()
						{
							Cappelli_Maschere = 17, Orecchie = 26, Occhiali_Occhi = -1, Unk_3 = -1, Unk_4 = -1,
							Unk_5 = -1, Orologi = -1, Bracciali = -1, Unk_8 = -1
						},
						TexturesAccessori = new ()
						{
							Cappelli_Maschere = 3, Orecchie = 1, Occhiali_Occhi = -1, Unk_3 = -1, Unk_4 = -1,
							Unk_5 = -1, Orologi = -1, Bracciali = -1, Unk_8 = -1
						}
					}
				}
			},
			["Tenente"] = new JobGrade()
			{
				Id = 3,
				Stipendio = 0,
				Vestiti = new()
				{
					Maschio = new ()
					{
						Abiti = new ()
						{
							Faccia = -1, Maschera = 0, Capelli = -1, Torso = 0, Pantaloni = 35, Borsa_Paracadute = -1,
							Scarpe = 25, Accessori = 0, Sottomaglia = 58, Kevlar = 0, Badge = 0, Torso_2 = 55
						},
						TextureVestiti = new ()
						{
							Faccia = -1, Maschera = 0, Capelli = -1, Torso = 0, Pantaloni = 0, Borsa_Paracadute = -1,
							Scarpe = 0, Accessori = 0, Sottomaglia = 0, Kevlar = 0, Badge = 0, Torso_2 = 0
						},
						Accessori = new ()
						{
							Cappelli_Maschere = 46, Orecchie = 9, Occhiali_Occhi = -1, Unk_3 = -1, Unk_4 = -1,
							Unk_5 = -1, Orologi = -1, Bracciali = -1, Unk_8 = -1
						},
						TexturesAccessori = new ()
						{
							Cappelli_Maschere = 0, Orecchie = 0, Occhiali_Occhi = -1, Unk_3 = -1, Unk_4 = -1,
							Unk_5 = -1, Orologi = -1, Bracciali = -1, Unk_8 = -1
						}
					},
					Femmina = new ()
					{
						Abiti = new ()
						{
							Faccia = -1, Maschera = 0, Capelli = -1, Torso = 23, Pantaloni = 38, Borsa_Paracadute = -1,
							Scarpe = 49, Accessori = 0, Sottomaglia = 14, Kevlar = 0, Badge = 0, Torso_2 = 59
						},
						TextureVestiti = new ()
						{
							Faccia = -1, Maschera = 0, Capelli = -1, Torso = 0, Pantaloni = 0, Borsa_Paracadute = -1,
							Scarpe = 0, Accessori = 0, Sottomaglia = 0, Kevlar = 0, Badge = 0, Torso_2 = 0
						},
						Accessori = new ()
						{
							Cappelli_Maschere = 17, Orecchie = 26, Occhiali_Occhi = -1, Unk_3 = -1, Unk_4 = -1,
							Unk_5 = -1, Orologi = -1, Bracciali = -1, Unk_8 = -1
						},
						TexturesAccessori = new ()
						{
							Cappelli_Maschere = 3, Orecchie = 1, Occhiali_Occhi = -1, Unk_3 = -1, Unk_4 = -1,
							Unk_5 = -1, Orologi = -1, Bracciali = -1, Unk_8 = -1
						}
					}
				}
			},
			["Capo della Polizia"] = new JobGrade()
			{
				Id = 4,
				Stipendio = 0,
				Vestiti = new()
				{
					Maschio = new ()
					{
						Abiti = new ()
						{
							Faccia = -1, Maschera = 0, Capelli = -1, Torso = 0, Pantaloni = 35, Borsa_Paracadute = -1,
							Scarpe = 25, Accessori = 0, Sottomaglia = 58, Kevlar = 0, Badge = 0, Torso_2 = 55
						},
						TextureVestiti = new ()
						{
							Faccia = -1, Maschera = 0, Capelli = -1, Torso = 0, Pantaloni = 0, Borsa_Paracadute = -1,
							Scarpe = 0, Accessori = 0, Sottomaglia = 0, Kevlar = 0, Badge = 0, Torso_2 = 0
						},
						Accessori = new ()
						{
							Cappelli_Maschere = 46, Orecchie = 9, Occhiali_Occhi = -1, Unk_3 = -1, Unk_4 = -1,
							Unk_5 = -1, Orologi = -1, Bracciali = -1, Unk_8 = -1
						},
						TexturesAccessori = new ()
						{
							Cappelli_Maschere = 0, Orecchie = 0, Occhiali_Occhi = -1, Unk_3 = -1, Unk_4 = -1,
							Unk_5 = -1, Orologi = -1, Bracciali = -1, Unk_8 = -1
						}
					},
					Femmina = new ()
					{
						Abiti = new ()
						{
							Faccia = -1, Maschera = 0, Capelli = -1, Torso = 23, Pantaloni = 38, Borsa_Paracadute = -1,
							Scarpe = 49, Accessori = 0, Sottomaglia = 14, Kevlar = 0, Badge = 0, Torso_2 = 59
						},
						TextureVestiti = new ()
						{
							Faccia = -1, Maschera = 0, Capelli = -1, Torso = 0, Pantaloni = 0, Borsa_Paracadute = -1,
							Scarpe = 0, Accessori = 0, Sottomaglia = 0, Kevlar = 0, Badge = 0, Torso_2 = 0
						},
						Accessori = new ()
						{
							Cappelli_Maschere = 17, Orecchie = 26, Occhiali_Occhi = -1, Unk_3 = -1, Unk_4 = -1,
							Unk_5 = -1, Orologi = -1, Bracciali = -1, Unk_8 = -1
						},
						TexturesAccessori = new ()
						{
							Cappelli_Maschere = 3, Orecchie = 1, Occhiali_Occhi = -1, Unk_3 = -1, Unk_4 = -1,
							Unk_5 = -1, Orologi = -1, Bracciali = -1, Unk_8 = -1
						}
					}
				}
			}
		};
    }
}