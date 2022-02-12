using System.Collections.Generic;
using System.Linq;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Lavori
{
    public enum Employment
    {
        Unemployed,
        Polizia,
        Medico,
        VenditoreAuto,
        VenditoreCase,
        Taxi,
        Pescatore,
        Cacciatore,
        RimozioneForzata
    }

    public class EmploymentRoles
    {
        public enum Polizia
        {
            Capo = 4,
            Tenente = 3,
            Sergente = 2,
            Agente = 1,
            Recluta = 0
        }

        public enum Medico
        {
            Primario = 4,
            Dottore = 3,
            Infermiere = 2,
            Specializzando = 1,
            Paramedico = 0
        }

        public enum Taxi
        {
            CEO = 3,
            COO = 2,
            Impiegato = 1,
            Stagista = 0
        }
        public enum VenditoreCase
        {
            Direttore = 1,
            Venditore = 0
        }
        public enum VenditoreAuto
        {
            Direttore = 1,
            Venditore = 0
        }
    }

    public abstract class LavoroBase
    {
        public abstract Employment Lavoro { get; set; }
        public abstract string Label { get; set; }
        public abstract void OnJobSet();
        public abstract void OnJobRemoved();
        public abstract Dictionary<int, string> Roles { get; set; }
        public abstract JobProfile[] Profiles { get; set; }

        public virtual void Init()
        {
            // Ignored
        }


        public T GetProfile<T>() where T : JobProfile
        {
            return (T)Profiles.FirstOrDefault(self => self.GetType() == typeof(T));
        }

    }
}
