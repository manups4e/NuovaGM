﻿using Settings.Shared.Config.Generic;

namespace TheLastPlanet.Shared
{
    public class ConfigShared
    {
        public static SharedConfig SharedConfig = new();
    }

    public class SharedConfig
    {
        public MainShared Main = new();
    }


    public class MainShared
    {
        public SharedConfigVehicles Vehicles = new();
        public SharedWeatherSettings Weather = new();
        public SharedGenerics Generics = new();
    }
}