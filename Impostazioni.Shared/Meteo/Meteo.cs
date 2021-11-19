using System;
using System.Collections.Generic;

namespace Impostazioni.Shared.Configurazione.Generici
{
    public class SharedMeteo
    {
        public bool ss_enable_wind_sync = true;
        public float ss_wind_speed_max = 10f;
        public float ss_night_time_speed_mult = 1f;
        public float ss_day_time_speed_mult = 1f;
        public bool ss_enable_dynamic_weather = false;
        //public int ss_default_weather = new Random().Next(0, 9);
        public int ss_default_weather = 13;
        public int ss_weather_timer = 10;
        public bool ss_reduce_rain_chance = true;
        public int ss_rain_timeout = 45;

        public List<float> ss_wind_speed_Mult = new List<float>()
        {
            0.2f,
            0.3f,
            0.1f,
            0.1f,
            0.1f,
            0.7f,
            0.5f,
            1.0f,
            0.7f,
            0.5f,
            0.6f,
            0.8f,
            0.4f,
            0.4f,
            0.8f
        };

        public List<List<int>> ss_weather_Transition = new List<List<int>>()
        {
            new() {1, 4},
            new() {3, 0, 8, 4, 5},
            new() {0},
            new() {1, 4, 8, 5},
            new() {1, 8, 5, 3, 0},
            new() {1, 3, 4, 8, 7},
            new() {1, 3, 4, 8, 7},
            new() {5},
            new() {1, 3, 5, 4},
            new() {0},
            new() {10, 12},
            new() {11},
            new() {10, 12},
            new() {13},
            new() {14},
        };
    }
}