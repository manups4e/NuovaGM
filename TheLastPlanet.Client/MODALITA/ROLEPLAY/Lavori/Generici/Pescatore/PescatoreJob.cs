using Impostazioni.Client.Configurazione.Lavori.Generici;
using System;
using System.Collections.Generic;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Lavori.Generici.Pescatore
{
    class PescatoreJob : LavoroBase
    {
        public override Employment Lavoro { get; set; } = Employment.Pescatore;
        public override string Label { get; set; } = "Pescatore";
        public Pescatori PuntiPesca;
        private static string scenario = "WORLD_HUMAN_STAND_FISHING";
        private static string AnimDict = "amb@world_human_stand_fishing@base";
        private static bool LavoroAccettato = false;
        private static Vehicle lastVehicle;
        public static bool Pescando = false;
        public static bool CannaInMano = false;
        private static int TipoCanna = -1;
        private static Prop CannaDaPesca;
        private static bool mostrablip = false;
        private static List<string> PerVendereIlPesce = new List<string>()
        {
            "branzino",
            "sgombro",
            "sogliola",
            "orata",
            "tonno",
            "salmone",
            "merluzzo",
            "pescespada",
            "squalo",
            "fruttidimare",
            "carpa",
            "luccio",
            "persico",
            "pescegattocomune",
            "pescegattopunteggiato",
            "spigola",
            "trota",
            "ghiozzo",
            "lucioperca",
            "alborella",
            "carassio",
            "carassiodorato",
            "cheppia",
            "rovella",
            "spinarello",
            "storionecobice",
            "storionecomune",
            "storioneladano"
        };

        public override void OnJobRemoved()
        {
            throw new NotImplementedException();
        }
        public override void OnJobSet()
        {
            throw new NotImplementedException();
        }

        public override JobProfile[] Profiles { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override Dictionary<int, string> Roles { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
