Config.Shared.Main.Meteo = {
	-- Enable Dynamic Wind Speed Syncing between all GetPlayers? Options: true or false
	-- Note: Disabling this option will turn all wind effects in your server OFF (i.e. no wind movement at all).
	ss_enable_wind_sync = true,

	-- Wind Speed maximum. Default: Max = 2.00
	ss_wind_speed_max = 10.00,

	-- Lower then 1 = Longer, Higher then 1 = faster.  0.25 would be 4x slower then GTA time. 2.0 would be half as long as default GTA
	ss_night_time_speed_mult = 1.0,
	ss_day_time_speed_mult = 1.0,
	
	-- Enable Dynamic (changing) weather for the GetPlayers? Options: true or false
	ss_enable_dynamic_weather = true, --(true) per meteo dinamico

	-- Default weather type for when the resource starts
--[[	Unknown = -1,
		ExtraSunny = 0,
		Clear = 1,
		Clouds = 2,
		Smog = 3,
		Foggy = 4,
		Overcast = 5,
		Raining = 6,
		ThunderStorm = 7,
		Clearing = 8,
		Neutral = 9,
		Snowing = 10,
		Blizzard = 11,
		Snowlight = 12,
		Christmas = 13,
		Halloween = 14
--]]
	ss_default_weather = math.random(0, 9), -- dal 10 in poi sono attivati manualmente 
											-- cambiando questo valore o settandoli via comando
	-- Weather timer (in minutes) between dynamic weather changes (Default: 10minutes)
	ss_weather_timer = 1,

	ss_reduce_rain_chance = true,

	-- Weather timeout for rain (in minutes). This means it can only rain once every X minutes - Default: 60 minutes)
	ss_rain_timeout = 45,

	ss_wind_speed_Mult = {
		0.2,
		0.3,
		0.1,
		0.1,
		0.1,
		0.7,
		0.5,
		1.0,
		0.7,
		0.5,
		0.6,
		0.8,
		0.4,
		0.4,
		0.8
	},

	ss_weather_Transition = {
		{1,4},
		{3,0,8,4,5},
		{0},
		{1,4,8,5},
		{1,8,5,3,0},
		{1,3,4,8,7},
		{1,3,4,8,7},
		{5},		 -- Always rotate away from Thunder, as it's annoying
		{1,3,5,4},
		{0},
		{10,12},  -- Usually used for events - never changes and has to be manually set via /weather command
		{11},     -- Usually used for events - never changes and has to be manually set via /weather command
		{10,12},  -- Usually used for events - never changes and has to be manually set via /weather command
		{13},     -- Usually used for events - never changes and has to be manually set via /weather command
		{14}      -- Usually used for events - never changes and has to be manually set via /weather command
	}
}
