using System.Collections.Generic;
using TheLastPlanet.Shared;

namespace Impostazioni.Client.Configurazione.Negozi.Abiti
{
    public class Completo : Dressing
    {
        public int Price;

        public Completo(){}
        public Completo(string name, string desc, int price, ComponentDrawables componentDrawables,
            ComponentDrawables componentTextures, PropIndices propIndices, PropIndices propTextures)
        {
            Name = name;
            Description = desc;
            Price = price;
            ComponentDrawables = componentDrawables;
            ComponentTextures = componentTextures;
            PropIndices = propIndices;
            PropTextures = propTextures;
        }
    }
    
    public class Singolo
    {
        public string Title;
        public string Description;
        public int Modello;
        public int Price;
        public List<int> Text = new List<int>();
    }

    public class Accessori
    {
        public List<Singolo> Borse = new List<Singolo>();
        public Testa Testa = new Testa();
        public List<Singolo> Orologi = new List<Singolo>();
        public List<Singolo> Bracciali = new List<Singolo>();
    }

    public class Testa
    {
        public List<Singolo> Orecchini = new List<Singolo>();
        public List<Singolo> Auricolari = new List<Singolo>();
        public List<Singolo> Cappellini = new List<Singolo>();
    }
}