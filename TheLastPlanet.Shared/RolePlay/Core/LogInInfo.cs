﻿namespace TheLastPlanet.Shared
{

    public class LogInInfo
    {
        public string ID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string dateOfBirth { get; set; }
        public int Money { get; set; }
        public int Bank { get; set; }
        public float Test = 54.3214f;

    }


    public class SkinAndDress
    {
        public Skin Skin { get; set; }
        public Dressing Dressing { get; set; }
        public Position Position { get; set; }
    }
}
