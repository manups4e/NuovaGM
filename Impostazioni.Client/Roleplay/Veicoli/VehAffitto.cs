using System.Collections.Generic;
using TheLastPlanet.Shared.Veicoli;

namespace Impostazioni.Client.Configurazione.Veicoli
{
    public class VeicoliAffitto
    {
        public List<Veicoloaff> biciclette { get; set; }
        public List<Veicoloaff> macchineGeneric { get; set; }
        public List<Veicoloaff> macchineMedium { get; set; }
        public List<Veicoloaff> macchineSuper { get; set; }
        public List<Veicoloaff> motoGeneric { get; set; }
        public List<Veicoloaff> motoMedium { get; set; }
        public List<Veicoloaff> motoSuper { get; set; }
    }
}