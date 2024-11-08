﻿namespace TheLastPlanet.Shared
{

    public class LogInInfo
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public int Money { get; set; }
        public int Bank { get; set; }
    }


    public class SkinAndDress
    {
        public Skin Skin { get; set; }
        public Dressing Dressing { get; set; }
        public Position Position { get; set; }
    }
}
