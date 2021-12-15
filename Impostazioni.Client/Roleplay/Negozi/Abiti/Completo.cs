using System.Collections.Generic;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.Internal.Events.Attributes;

namespace Impostazioni.Client.Configurazione.Negozi.Abiti
{
    [Serialization]
    public partial class Completo : Dressing
    {
        public int Price { get; set; }

        public Completo() : base() { }
        public Completo(string name, string desc, int price, ComponentDrawables componentDrawables, ComponentDrawables componentTextures, PropIndices propIndices, PropIndices propTextures) : 
            base (name, desc, componentDrawables, componentTextures, propIndices, propTextures)
        {
            Price = price;
        }
    }

    [Serialization]
    public partial class Singolo
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Modello { get; set; }
        public int Price { get; set; }
        public List<int> Text { get; set; } = new List<int>();
    }

    [Serialization]
    public partial class Accessori
    {
        public List<Singolo> Borse { get; set; } = new List<Singolo>();
        public Testa Testa { get; set; } = new Testa();
        public List<Singolo> Orologi { get; set; } = new List<Singolo>();
        public List<Singolo> Bracciali { get; set; } = new List<Singolo>();
    }

    public class Testa
    {
        public List<Singolo> Orecchini { get; set; } = new List<Singolo>();
        public List<Singolo> Auricolari { get; set; } = new List<Singolo>();
        public List<Singolo> Cappellini { get; set; } = new List<Singolo>();
    }
}