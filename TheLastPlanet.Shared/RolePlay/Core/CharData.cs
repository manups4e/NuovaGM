using Newtonsoft.Json;
using Settings.Shared.Config.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using TheLastPlanet.Shared.Vehicles;
#if CLIENT
using TheLastPlanet.Client.Cache;
#endif

namespace TheLastPlanet.Shared
{
    public enum DrawableIndexes
    {
        Face = 0,
        Mask = 1,
        Hair = 2,
        Torso = 3,
        Pants = 4,
        Bag_Parachute = 5,
        Shoes = 6,
        Accessories = 7,
        Undershirt = 8,
        Kevlar = 9,
        Badge = 10,
        Torso_2 = 11,
    }

    public enum PropIndexes
    {
        Hats_Masks = 0,
        Glasses = 1,
        Ears = 2,
        Unk_3 = 3,
        Unk_4 = 4,
        Unk_5 = 5,
        Watches = 6,
        Bracelets = 7,
        Unk_8 = 8,
    }

    public enum BankTransactionType
    {
        Withdraw,
        Deposit,
        Receiving,
        Sending
    }


    public class Char_data
    {
        public ulong CharID { get; set; }
        public bool Is_dead { get; set; }
        public Info Info { get; set; }
        public Finance Finance { get; set; }
        public Position Position { get; set; }
        public Job Job { get; set; }
        public Gang Gang { get; set; }
        public Skin Skin { get; set; }
        public List<Weapons> Weapons = new();
        public List<Licenses> Licenses = new();
        public List<Inventory> Inventory = new();
        public List<string> Properties = new();
        public List<OwnedVehicle> Vehicles = new();
        public Dressing Dressing { get; set; }
        public Needs Needs { get; set; }
        public Statistiche Statistics { get; set; }
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
            this.Statistics = statistiche;
            this.Is_dead = is_dead;
        }
    }

    // old binary serialization class
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
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string DateOfBirth { get; set; }
        public int Height { get; set; }
        public long PhoneNumber { get; set; }
        public long Insurance { get; set; }

        public Info() { }

        public Info(string firstname, string lastname, string dateOfBirth, int height, long phoneNumber, long insurance)
        {
            this.Firstname = firstname;
            this.Lastname = lastname;
            this.DateOfBirth = dateOfBirth;
            this.Height = height;
            this.PhoneNumber = phoneNumber;
            this.Insurance = insurance;
        }
    }


    public class Job
    {
        public string? Name { get; set; } = "Unemployed";
        public int Paycheck { get; set; } = 0;
        public int Grade { get; set; } = 0;
        public bool OnDuty { get; set; } = false;
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
        public string Name { get; set; }
        public string StartDate { get; set; }
        public string ReleasedBy { get; set; } = "Admin"; // todo: handle who or what releases licenses!
        public string ExpirationDate { get; set; }
        public Licenses() { }
        public Licenses(string name, string possesso, string rilasciatoDa = "Admin")
        {
            this.Name = name;
            StartDate = possesso;
            ReleasedBy = rilasciatoDa;
        }
    }


    public class Finance
    {
        public int Money { get; set; } = 1000;
        public int Bank { get; set; } = 3000;
        public int DirtyCash { get; set; } = 0;
        public List<BankTransaction> Transactions { get; set; }

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
        public string Name { get; set; }
        public int Ammo { get; set; }
        public int Tint { get; set; }
        public List<Components> Components = new();
        public Weapons() { }
        public Weapons(string _name, int _ammo, dynamic data, int _tint)
        {
            this.Name = _name;
            this.Ammo = _ammo;
            this.Tint = _tint;
            if (data.Count > 0)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    Components.Add(new Components(data[i]["name"].Value, (bool)data[i]["active"].Value));
                }
            }
        }

        public Weapons(string _name, int _ammo, List<Components> data, int _tint)
        {
            this.Name = _name;
            this.Ammo = _ammo;
            this.Tint = _tint;
            Components = data.ToList();
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
        public int Face { get; set; }
        public int Mask { get; set; }
        public int Hair { get; set; }
        public int Torso { get; set; }
        public int Pants { get; set; }
        public int Bag_Parachute { get; set; }
        public int Shoes { get; set; }
        public int Accessories { get; set; }
        public int Undershirt { get; set; }
        public int Kevlar { get; set; }
        public int Badge { get; set; }
        public int Torso_2 { get; set; }
        public ComponentDrawables() { }
        public ComponentDrawables(int face, int mask, int hair, int torso, int pantaloni, int parachute_bag, int shoes, int accessory, int undershirt, int kevlar, int badge, int torso_2)
        {
            Face = face;
            Mask = mask;
            Hair = hair;
            Torso = torso;
            Pants = pantaloni;
            Bag_Parachute = parachute_bag;
            Shoes = shoes;
            Accessories = accessory;
            Undershirt = undershirt;
            Kevlar = kevlar;
            Badge = badge;
            Torso_2 = torso_2;
        }
    }


    public class PropIndices
    {
        public int Hats_masks { get; set; }
        public int Ears { get; set; }
        public int Glasses { get; set; }
        public int Unk_3 { get; set; }
        public int Unk_4 { get; set; }
        public int Unk_5 { get; set; }
        public int Watches { get; set; }
        public int Bracelets { get; set; }
        public int Unk_8 { get; set; }

        public PropIndices() { }
        public PropIndices(int cappelli_maschere, int orecchie, int occhiali_occhi, int unk_3, int unk_4, int unk_5, int orologi, int bracciali, int unk_8)
        {
            Hats_masks = cappelli_maschere;
            Ears = orecchie;
            Glasses = occhiali_occhi;
            Unk_3 = unk_3;
            Unk_4 = unk_4;
            Unk_5 = unk_5;
            Watches = orologi;
            Bracelets = bracciali;
            Unk_8 = unk_8;
        }
    }


    public class Needs
    {
        public float Hunger { get; set; }
        public float Thirst { get; set; }
        public float Tiredness { get; set; }
        public bool Sickness { get; set; } = false;
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
        public string Sex { get; set; } = "Maschio";
        public uint Model { get; set; } = 1885233650u;
        public float Resemblance { get; set; } = 0f;
        public float Skinmix { get; set; } = 0f;
        public Face Face { get; set; }
        public A2 Ageing { get; set; }
        public A2 Makeup { get; set; }
        public A2 Blemishes { get; set; }
        public A2 Complexion { get; set; }
        public A2 SkinDamage { get; set; }
        public A2 Freckles { get; set; }
        public A3 Lipstick { get; set; }
        public A3 Blusher { get; set; }
        public Facial FacialHair { get; set; }
        public Hair Hair { get; set; }
        public Eye Eye { get; set; }
        public Ears Ears { get; set; }

        public Skin() { }
        public Skin(string sex, uint model, float resemblance, float skinmix, Face face, A2 ageing, A2 makeup, A2 blemishes, A2 complexion, A2 skinDamage, A2 freckles, A3 lipstick, A3 blusher, Facial facialHair, Hair hair, Eye eye, Ears ears)
        {
            this.Sex = sex;
            this.Model = model;
            this.Resemblance = resemblance;
            this.Skinmix = skinmix;
            this.Face = face;
            this.Ageing = ageing;
            this.Makeup = makeup;
            this.Blemishes = blemishes;
            this.Complexion = complexion;
            this.SkinDamage = skinDamage;
            this.Freckles = freckles;
            this.Lipstick = lipstick;
            this.Blusher = blusher;
            this.FacialHair = facialHair;
            this.Hair = hair;
            this.Eye = eye;
            this.Ears = ears;
        }

    }


    public class Face
    {
        public int Mom { get; set; } = 0;
        public int Dad { get; set; } = 0;
        public float[] Traits { get; set; } = new float[20] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        public Face() { }
        public Face(int mom, int dad, float[] tratti)
        {
            this.Mom = mom;
            this.Dad = dad;
            this.Traits = tratti;
        }
    }


    public class A2
    {
        public int Style { get; set; }
        public float Opacity { get; set; }

        public A2() { }
        public A2(int style, float opacity)
        {
            this.Style = style;
            this.Opacity = opacity;
        }
    }


    public class Facial
    {
        public A3 Beard { get; set; } = new();
        public A3 Eyebrow { get; set; } = new();

        public Facial() { }
        public Facial(A3 beard, A3 eyebrow)
        {
            this.Beard = beard;
            this.Eyebrow = eyebrow;
        }
    }


    public class A3
    {
        public int Style { get; set; } = 0;
        public float Opacity { get; set; } = 0f;
        public int[] Color { get; set; } = new int[2] { 0, 0 };

        public A3() { }
        public A3(int style, float opacity, int[] color)
        {
            this.Style = style;
            this.Opacity = opacity;
            this.Color = color;
        }
    }


    public class Hair
    {
        public int Style { get; set; } = 0;
        public int[] Color { get; set; } = new int[2] { 0, 0 };
        public Hair() { }
        public Hair(int style, int[] color)
        {
            this.Style = style;
            this.Color = color;
        }
    }


    public class Eye
    {
        public int Style { get; set; }
        public Eye() { }
        public Eye(int style)
        {
            this.Style = style;
        }
    }


    public class Ears
    {
        public int Style { get; set; }
        public int Color { get; set; }
        public Ears() { }
        public Ears(int style, int color)
        {
            this.Style = style;
            this.Color = color;
        }
    }


    public class Phone_data
    {
        public int Id { get; set; } = 1;
        public int Theme { get; set; } = 1;
        public int Wallpaper { get; set; } = 4;
        public int Ringtone { get; set; } = 0;
        public int SleepMode { get; set; } = 0;
        public int Vibration { get; set; } = 1;
        public List<Message> Messages { get; set; } = new List<Message>();
        public List<Contatto> Contacts = new List<Contatto>()
        {
            new Contatto("Add contact", "CHAR_MULTIPLAYER", false, "", 0),
            new Contatto("Law Enforcements", "CHAR_CALL911", false, "Police", 0),
            new Contatto("Medic", "CHAR_CALL911", false, "Medics", 0),
            new Contatto("Mechanic", "CHAR_LS_CUSTOMS", false, "Mechanic", 0),
            new Contatto("Taxi", "CHAR_TAXI", false, "Taxi", 0),
            new Contatto("Car Dealer", "CHAR_CARSITE2", false, "CarDealer", 0),
            new Contatto("Real Estate Agent", "CHAR_PEGASUS_DELIVERY", false, "RealEstate", 0),
            new Contatto("Reporter", "CHAR_LIFEINVADER", false, "Reporter", 0),
        };

        public Phone_data() { }

        public Phone_data(dynamic result)
        {
            Theme = (int)result["Theme"].Value;
            Wallpaper = (int)result["WallPaper"].Value;
            if (result["Contacts"].HasValues)
                for (int i = 0; i < result["Contacts"].Count; i++)
                    Contacts.Add(new Contatto(result["Contacts"]["Name"].Value, result["Contacts"]["Icon"].Value, result["Contacts"]["TelephoneNumber"].Value, result["Contacts"]["IsPlayer"].Value, result["Contacts"]["PlayerIndex"].Value, result["Contacts"]["Player"].Value));
            if (result["Messages"].HasValues)
                for (int i = 0; i < result["Messages"].Count; i++)
                    Messages.Add(new Message(result["Messages"]["From"].Value, result["Messages"]["Title"].Value, result["Messages"]["Messaggio"].Value, (DateTime)result["Messages"]["Data"].Value));
        }
    }


    public class Message
    {
        public string From { get; set; }
        public string Title { get; set; }
        public string TxtMessage { get; set; }
        public DateTime Data { get; set; }

        public Message(string _from, string _title, string _message, DateTime _data)
        {
            this.From = _from;
            this.Title = _title;
            this.TxtMessage = _message;
            this.Data = _data;
        }
    }


    public class Contatto
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


    public class BankTransaction
    {
        public BankTransactionType Type { get; set; }
        public long Amount { get; set; }
        [JsonIgnore] public DateTime Date { get; set; }
        public string Information { get; set; }

        [JsonProperty("Date")] public string _Date => Date.ToString("MM/dd/yyyy HH:mm:ss");
    }
}
