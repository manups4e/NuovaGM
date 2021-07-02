﻿using System.Collections.Generic;
using TheLastPlanet.Shared.Internal.Events.Attributes;

namespace TheLastPlanet.Shared.Meteo
{
	[Serialization]
	public partial class _SharedMeteo
	{
		public bool ss_enable_wind_sync;
		public float ss_wind_speed_max;
		public float ss_night_time_speed_mult;
		public float ss_day_time_speed_mult;
		public bool ss_enable_dynamic_weather;
		public int ss_default_weather;
		public int ss_weather_timer;
		public bool ss_reduce_rain_chance;
		public int ss_rain_timeout;
		public List<float> ss_wind_speed_Mult = new List<float>();
		public List<List<int>> ss_weather_Transition = new List<List<int>>();
	}
}