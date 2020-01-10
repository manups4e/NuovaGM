using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuovaGM.Shared.Weather
{
	public class SharedMeteo
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
		public Dictionary<int, List<int>> ss_weather_Transition = new Dictionary<int, List<int>>();
		public Dictionary<int, float> ss_wind_speed_Mult = new Dictionary<int, float>();
	}
}
