using Impostazioni.Shared.Configurazione.Generici;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TheLastPlanet.Shared.Internal.Events.Attributes;
using TheLastPlanet.Shared.Veicoli;
#if CLIENT
using TheLastPlanet.Client.Cache;
#endif

namespace TheLastPlanet.Shared
{
    public enum DrawableIndexes
    {
        Faccia = 0,
        Maschera = 1,
        Capelli = 2,
        Torso = 3,
        Pantaloni = 4,
        Borsa_Paracadute = 5,
        Scarpe = 6,
        Accessori = 7,
        Sottomaglia = 8,
        Kevlar = 9,
        Badge = 10,
        Torso_2 = 11,
    }

    public enum PropIndexes
    {
        Cappelli_Maschere = 0,
        Occhiali_Occhi = 1,
        Orecchie = 2,
        Unk_3 = 3,
        Unk_4 = 4,
        Unk_5 = 5,
        Orologi = 6,
        Bracciali = 7,
        Unk_8 = 8,
    }

    public enum BankTransactionType
    {
        Ritiro,
        Deposito,
        Ricezione,
        Invio
    }

    [Serialization]
    public partial class Char_data
    {
        public ulong CharID { get; set; }
        public bool is_dead { get; set; }
        public Info Info { get; set; }
        public Finance Finance { get; set; }
        public Position Posizione { get; set; }
        public Job Job { get; set; }
        public Gang Gang { get; set; }
        public Skin Skin { get; set; }
        public List<Weapons> Weapons = new();
        public List<Licenses> Licenze = new();
        public List<Inventory> Inventory = new();
        public List<string> Proprietà = new();
        public List<OwnedVehicle> Veicoli = new();
        public Dressing Dressing { get; set; }
        public Needs Needs { get; set; }
        public Statistiche Statistiche { get; set; }
        public Char_data() { }

        public Char_data(ulong id, Info info, Finance finance, Job job, Gang gang, Skin skin, Dressing dressing, List<Weapons> weapons, List<Inventory> inventory, Needs needs, Statistiche statistiche, bool is_dead)
        {
            this.CharID = id;
            this.Info = info;
            this.Finance = finance;
            this.Job = job;
            this.Gang = gang;
            this.Skin = skin;
            this.Dressing = dressing;
            this.Weapons = weapons;
            this.Inventory = inventory;
            this.Needs = needs;
            this.Statistiche = statistiche;
            this.is_dead = is_dead;
        }
    }

    public class Char_Metadata
    {
        public ulong CharId;
        public bool is_dead;
        public string info;/*{ set => Info = value.FromJson<Info>(); }*/
        public int Money;/*{ set => Finance.Money = value; }*/
        public int Bank;/*{ set => Finance.Bank = value; }*/
        public int DirtyCash;/*{ set => Finance.DirtyCash = value; }*/
        public string location;/*{ set => Posizione = value.FromJson<Position>(); }*/
        public string job;/*{ set => Job.Name = value; }*/
        public int job_grade;/*{ set => Job.Grade = value; }*/
        public string gang;/*{ set => Gang.Name = value; }*/
        public int gang_grade;/*{ set => Gang.Grade = value; }*/
        public string skin;/*{ set => Skin = value.FromJson<Skin>(); }*/
        public string inventory;/*{ set => Inventory = value.FromJson<List<Inventory>>(); }*/
        public string weapons;/*{ set => Weapons = value.FromJson<List<Weapons>>(); }*/
        public string dressing;/*{ set => Dressing = value.FromJson<Dressing>(); }*/
        public string needs;/*{ set => Needs = value.FromJson<Needs>(); }*/
        public string statistiche;/*{ set => Statistiche = value.FromJson<Statistiche>(); }*/
    }


    public class Info
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string dateOfBirth { get; set; }
        public int height { get; set; }
        public long phoneNumber { get; set; }
        public long insurance { get; set; }

        public Info() { }

        public Info(string firstname, string lastname, string dateOfBirth, int height, long phoneNumber, long insurance)
        {
            this.firstname = firstname;
            this.lastname = lastname;
            this.dateOfBirth = dateOfBirth;
            this.height = height;
            this.phoneNumber = phoneNumber;
            this.insurance = insurance;
        }
    }


    public class Job
    {
        public string? Name { get; set; } = "Disoccupato";
        public int Stipendio { get; set; } = 0;
        public int Grade { get; set; } = 0;
        public bool InServizio { get; set; } = false;
        public DateTime HiredDate { get; set; }
        public bool IsBoss { get; set; } = false;

        public Job() { }
        public Job(string name, int grade)
        {
            this.Name = name;
            this.Grade = grade;
            HiredDate = DateTime.Now;
        }
    }

    public class Gang
    {
        public string? Name { get; set; } = "Incensurato";
        public int Grade { get; set; } = 0;
        public Gang() { }
        public Gang(string name, int grade)
        {
            this.Name = name;
            this.Grade = grade;
        }
    }


    public class Licenses
    {
        public string name { get; set; }
        public string dataDiPossesso { get; set; }
        public string rilasciataDa { get; set; } = "Admin"; // gestire chi l'ha rilasciata!
        public string scadenza { get; set; }
        public Licenses() { }
        public Licenses(string name, string possesso, string rilasciatoDa = "Admin")
        {
            this.name = name;
            dataDiPossesso = possesso;
            rilasciataDa = rilasciatoDa;
        }
    }


    public class Finance
    {
        public int Money { get; set; } = 1000;
        public int Bank { get; set; } = 3000;
        public int DirtyCash { get; set; } = 0;
        public List<BankTransaction> Transazioni { get; set; }

        public Finance() { }

        public Finance(int cash, int bank, int dirtyCash)
        {
            Money = cash;
            Bank = bank;
            DirtyCash = dirtyCash;
        }

    }


    public class Inventory
    {
        public string Item { get; set; }
        public int Amount { get; set; }
        public float Weight { get; set; }
        public Inventory() { }
        public Inventory(string _item, int _am, float _weight)
        {
            this.Item = _item;
            this.Amount = _am;
            Weight = _weight;
        }
    }


    public class Weapons
    {
        public string name { get; set; }
        public int ammo { get; set; }
        public int tint { get; set; }
        public List<Components> components = new List<Components>();
        public Weapons() { }
        public Weapons(string _name, int _ammo, dynamic data, int _tint)
        {
            this.name = _name;
            this.ammo = _ammo;
            this.tint = _tint;
            if (data.Count > 0)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    components.Add(new Components(data[i]["name"].Value, (bool)data[i]["active"].Value));
                }
            }
        }

        public Weapons(string _name, int _ammo, List<Components> data, int _tint)
        {
            this.name = _name;
            this.ammo = _ammo;
            this.tint = _tint;
            if (data.Count > 0)
                foreach (Components d in data)
                    components.Add(new Components(d.name, d.active));
        }
    }


    public class Dressing
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public ComponentDrawables ComponentDrawables { get; set; }
        public ComponentDrawables ComponentTextures { get; set; }
        public PropIndices PropIndices { get; set; }
        public PropIndices PropTextures { get; set; }

        public Dressing() { }
        public Dressing(string name, string description, ComponentDrawables ComponentDrawables, ComponentDrawables ComponentTextures, PropIndices PropIndices, PropIndices PropTextures)
        {
            this.Name = name;
            this.Description = description;
            this.ComponentDrawables = ComponentDrawables;
            this.ComponentTextures = ComponentTextures;
            this.PropIndices = PropIndices;
            this.PropTextures = PropTextures;
        }
    }


    public class ComponentDrawables
    {
        public int Faccia { get; set; }
        public int Maschera { get; set; }
        public int Capelli { get; set; }
        public int Torso { get; set; }
        public int Pantaloni { get; set; }
        public int Borsa_Paracadute { get; set; }
        public int Scarpe { get; set; }
        public int Accessori { get; set; }
        public int Sottomaglia { get; set; }
        public int Kevlar { get; set; }
        public int Badge { get; set; }
        public int Torso_2 { get; set; }
        public ComponentDrawables() { }
        public ComponentDrawables(int face, int mask, int hair, int torso, int pantaloni, int parachute_bag, int shoes, int accessory, int undershirt, int kevlar, int badge, int torso_2)
        {
            Faccia = face;
            Maschera = mask;
            Capelli = hair;
            Torso = torso;
            Pantaloni = pantaloni;
            Borsa_Paracadute = parachute_bag;
            Scarpe = shoes;
            Accessori = accessory;
            Sottomaglia = undershirt;
            Kevlar = kevlar;
            Badge = badge;
            Torso_2 = torso_2;
        }
    }


    public class PropIndices
    {
        public int Cappelli_Maschere { get; set; }
        public int Orecchie { get; set; }
        public int Occhiali_Occhi { get; set; }
        public int Unk_3 { get; set; }
        public int Unk_4 { get; set; }
        public int Unk_5 { get; set; }
        public int Orologi { get; set; }
        public int Bracciali { get; set; }
        public int Unk_8 { get; set; }

        public PropIndices() { }
        public PropIndices(int cappelli_maschere, int orecchie, int occhiali_occhi, int unk_3, int unk_4, int unk_5, int orologi, int bracciali, int unk_8)
        {
            Cappelli_Maschere = cappelli_maschere;
            Orecchie = orecchie;
            Occhiali_Occhi = occhiali_occhi;
            Unk_3 = unk_3;
            Unk_4 = unk_4;
            Unk_5 = unk_5;
            Orologi = orologi;
            Bracciali = bracciali;
            Unk_8 = unk_8;
        }
    }


    public class Needs
    {
        public float Fame { get; set; }
        public float Sete { get; set; }
        public float Stanchezza { get; set; }
        public bool Malattia { get; set; } = false;
    }


    public class Statistiche
    {
        public float STAMINA { get; set; }
        public float STRENGTH { get; set; }
        public float LUNG_CAPACITY { get; set; }
        public float SHOOTING_ABILITY { get; set; }
        public float WHEELIE_ABILITY { get; set; }
        public float FLYING_ABILITY { get; set; }
        public float DRUGS { get; set; }
        public float FISHING { get; set; }
        public float HUNTING { get; set; }
    }


    public class Skin
    {
        public string sex { get; set; } = "Maschio";
        public uint model { get; set; } = 1885233650u;
        public float resemblance { get; set; } = 0f;
        public float skinmix { get; set; } = 0f;
        public Face face { get; set; }
        public A2 ageing { get; set; }
        public A2 makeup { get; set; }
        public A2 blemishes { get; set; }
        public A2 complexion { get; set; }
        public A2 skinDamage { get; set; }
        public A2 freckles { get; set; }
        public A3 lipstick { get; set; }
        public A3 blusher { get; set; }
        public Facial facialHair { get; set; }
        public Hair hair { get; set; }
        public Eye eye { get; set; }
        public Ears ears { get; set; }

        public Skin() { }
        public Skin(string sex, uint model, float resemblance, float skinmix, Face face, A2 ageing, A2 makeup, A2 blemishes, A2 complexion, A2 skinDamage, A2 freckles, A3 lipstick, A3 blusher, Facial facialHair, Hair hair, Eye eye, Ears ears)
        {
            this.sex = sex;
            this.model = model;
            this.resemblance = resemblance;
            this.skinmix = skinmix;
            this.face = face;
            this.ageing = ageing;
            this.makeup = makeup;
            this.blemishes = blemishes;
            this.complexion = complexion;
            this.skinDamage = skinDamage;
            this.freckles = freckles;
            this.lipstick = lipstick;
            this.blusher = blusher;
            this.facialHair = facialHair;
            this.hair = hair;
            this.eye = eye;
            this.ears = ears;
        }

    }


    public class Face
    {
        public int mom { get; set; } = 0;
        public int dad { get; set; } = 0;
        public float[] tratti { get; set; } = new float[20] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        public Face() { }
        public Face(int mom, int dad, float[] tratti)
        {
            this.mom = mom;
            this.dad = dad;
            this.tratti = tratti;
        }
    }


    public class A2
    {
        public int style { get; set; }
        public float opacity { get; set; }

        public A2() { }
        public A2(int style, float opacity)
        {
            this.style = style;
            this.opacity = opacity;
        }
    }


    public class Facial
    {
        public A3 beard { get; set; } = new();
        public A3 eyebrow { get; set; } = new();

        public Facial() { }
        public Facial(A3 beard, A3 eyebrow)
        {
            this.beard = beard;
            this.eyebrow = eyebrow;
        }
    }


    public class A3
    {
        public int style { get; set; } = 0;
        public float opacity { get; set; } = 0f;
        public int[] color { get; set; } = new int[2] { 0, 0 };

        public A3() { }
        public A3(int style, float opacity, int[] color)
        {
            this.style = style;
            this.opacity = opacity;
            this.color = color;
        }
    }


    public class Hair
    {
        public int style { get; set; } = 0;
        public int[] color { get; set; } = new int[2] { 0, 0 };
        public Hair() { }
        public Hair(int style, int[] color)
        {
            this.style = style;
            this.color = color;
        }
    }


    public class Eye
    {
        public int style { get; set; }
        public Eye() { }
        public Eye(int style)
        {
            this.style = style;
        }
    }


    public class Ears
    {
        public int style { get; set; }
        public int color { get; set; }
        public Ears() { }
        public Ears(int style, int color)
        {
            this.style = style;
            this.color = color;
        }
    }

    [Serialization]
    public partial class Phone_data
    {
        public int id { get; set; } = 1;
        public int Theme { get; set; } = 1;
        public int Wallpaper { get; set; } = 4;
        public int Ringtone { get; set; } = 0;
        public int SleepMode { get; set; } = 0;
        public int Vibration { get; set; } = 1;
        public List<Message> messaggi { get; set; } = new List<Message>()
        {
            new Message("Francesco Pastrengoni", "Test", "Messaggio di prova", DateTime.Now),
            new Message("Francesco Pastrengoni", "Test", "Messaggio di prova", DateTime.Now)
        };
        public List<Contatto> contatti = new List<Contatto>()
        {
            new Contatto("Aggiungi Contatto", "CHAR_MULTIPLAYER", false, "", 0),
            new Contatto("Polizia", "CHAR_CALL911", false, "Polizia", 0),
            new Contatto("Medico", "CHAR_CALL911", false, "Medico", 0),
            new Contatto("Meccanico", "CHAR_LS_CUSTOMS", false, "Meccanico", 0),
            new Contatto("Taxi", "CHAR_TAXI", false, "Taxi", 0),
            new Contatto("Concessionario", "CHAR_CARSITE2", false, "Concessionario", 0),
            new Contatto("Agente Immobiliare", "CHAR_PEGASUS_DELIVERY", false, "Immobiliare", 0),
            new Contatto("Reporter", "CHAR_LIFEINVADER", false, "Reporter", 0),
        };

        public Phone_data() { }

        public Phone_data(dynamic result)
        {
            Theme = (int)result["Theme"].Value;
            Wallpaper = (int)result["WallPaper"].Value;
            if (result["contatti"].HasValues)
                for (int i = 0; i < result["contatti"].Count; i++)
                    contatti.Add(new Contatto(result["contatti"]["Name"].Value, result["contatti"]["Icon"].Value, result["contatti"]["TelephoneNumber"].Value, result["contatti"]["IsPlayer"].Value, result["contatti"]["PlayerIndex"].Value, result["contatti"]["Player"].Value));
            if (result["messaggi"].HasValues)
                for (int i = 0; i < result["messaggi"].Count; i++)
                    messaggi.Add(new Message(result["messaggi"]["From"].Value, result["messaggi"]["Title"].Value, result["messaggi"]["Messaggio"].Value, (DateTime)result["messaggi"]["Data"].Value));
        }
    }

    [Serialization]
    public partial class Message
    {
        public string From { get; set; }
        public string Title { get; set; }
        public string Messaggio { get; set; }
        public DateTime Data { get; set; }

        public Message(string _from, string _title, string _message, DateTime _data)
        {
            this.From = _from;
            this.Title = _title;
            this.Messaggio = _message;
            this.Data = _data;
        }
    }

    [Serialization]
    public partial class Contatto
    {
        public int Player { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string TelephoneNumber { get; set; }
        public bool IsPlayer { get; set; }
        public int PlayerIndex { get; set; }


        public Contatto(string name, string icon, bool isPlayer, string telephoneNumber, int playerIndex = 0, int player = 0)
        {
            this.Name = name;
            this.Icon = icon;
            this.IsPlayer = isPlayer;
            this.TelephoneNumber = telephoneNumber;
            this.PlayerIndex = playerIndex;
            this.Player = player;
        }
    }

    [Serialization]
    public partial class BankTransaction
    {
        public BankTransactionType Type { get; set; }
        public long Amount { get; set; }
        [Ignore] [JsonIgnore] public DateTime Date { get; set; }
        public string Information { get; set; }

        [JsonProperty("Date")] public string _Date => Date.ToString("MM/dd/yyyy HH:mm:ss");
    }
}
