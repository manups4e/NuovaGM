speedometers = {
    {
        skinName = "default",
		ytdName = "default",
		lightsLoc = { 0.01, 0.092, 0.018, 0.02 },
		blinkerLoc = { 0.105, 0.034, 0.022, 0.03 },
		fuelLoc = { 0.105, 0.09, 0.012, 0.025 },
		oilLoc = { 0.1, 0.062, 0.02, 0.025 },
		engineLoc = { 0.13, 0.092, 0.02, 0.025 },
		seatbeltLoc = { 0.14, 0.062, 0.025, 0.03 },
		SpeedoBGLoc = { 0.0, 0.06, 0.12, 0.185 },
		SpeedoNeedleLoc = { 0.0, 0.062, 0.076, 0.15 },
		TachoBGloc = { 0.12, 0.06, 0.12, 0.185 },
		TachoNeedleLoc = { 0.12, 0.062, 0.076, 0.15 },
		FuelBGLoc = { 0.06, -0.02, 0.04, 0.04 },
		FuelGaugeLoc = { 0.06, 0.0, 0.04, 0.08 },
		RotMult = 2.036936,
		RotStep = 2.32833,
		centerCoords = { 0.8, 0.8 },
		rpmScale = 270,
		rpmScaleDecrease = 30,
    },
    {
        -- names
        skinName = "id5",
        ytdName = "id5",
        -- texture dictionary informations:
        -- night textures are supposed to look like this:
        -- "needle", "tachometer", skinData.ytdName, "fuelgauge"
        -- daytime textures this:
        -- "needle_day", "tachometer_day", "speedometer_day", "fuelgauge_day"
        -- these names are hardcoded
        -- where the speedo gets centered, values below are OFFSETS from this.
        centerCoords = {0.8,0.8},
        -- icon locations
        lightsLoc = {0.015,0.12,0.018,0.02},
        blinkerLoc = {0.04,0.12,0.022,0.03},
        fuelLoc = {-0.005,0.12,0.012,0.025},
        oilLoc = {0.100,0.12,0.020,0.025},
        engineLoc = {0.130,0.12,0.020,0.025},
        -- gauge locations
        SpeedoBGLoc = {0.053, 0.020, 0.25,0.23},
        SpeedoNeedleLoc = {0.000,5,0.076,0.15},
        TachoBGloc = {0.110,0.004,0.125,0.17},
        TachoNeedleLoc = {0.110,0.030,0.09,0.17},
        FuelBGLoc = {-0.035, -0.030,0.050, 0.040},
        FuelGaugeLoc = {0.060,0.000,0.030,0.080},
        -- you can also add your own values and use them in the code below, the sky is the limit!
        GearLoc = {0.010,-0.033,0.025,0.055}, -- gear location
        Speed1Loc = {-0.024,0.042,0.025,0.06}, -- 3rd digit
        Speed2Loc = {-0.004,0.042,0.025,0.06}, -- 2nd digit
        Speed3Loc = {0.020,0.042,0.025,0.06}, -- 1st digit
        UnitLoc = {0.029,0.088,0.025,0.025},
        TurboBGLoc = {0.053, -0.130, 0.075,0.105},
        TurboGaugeLoc = {0.0533, -0.125, 0.045,0.075},
        RotMult = 2.036936,
        RotStep = 2.32833,
        -- rpm scale, defines how "far" the rpm gauge goes before hitting redline
        rpmScale = 250,
    },
    {
        skinName = 'id6',
        ytdName = 'id6',
        centerCoords = {0.8,0.8},
        lightsLoc = {0.015,0.12,0.018,0.02},
        blinkerLoc = {0.04,0.12,0.022,0.03},
        fuelLoc = {-0.005,0.12,0.012,0.025},
        oilLoc = {0.100,0.12,0.020,0.025},
        engineLoc = {0.130,0.12,0.020,0.025},
        SpeedoBGLoc = {0.053, 0.020, 0.25,0.23},
        SpeedoNeedleLoc = {0.000,5,0.076,0.15},
        TachoBGloc = {0.110,0.004,0.125,0.17},
        TachoNeedleLoc = {0.110,0.030,0.09,0.17},
        FuelBGLoc = {-0.035, -0.030,0.050, 0.040},
        FuelGaugeLoc = {0.060,0.000,0.030,0.080},
        GearLoc = {0.010,-0.033,0.025,0.055},  -- gear location
        Speed1Loc = {-0.024,0.042,0.025,0.06}, -- 3rd digit
        Speed2Loc = {-0.004,0.042,0.025,0.06}, -- 2nd digit
        Speed3Loc = {0.020,0.042,0.025,0.06},  -- 1st digit
        UnitLoc = {0.029,0.088,0.025,0.025},
        TurboBGLoc = {0.053, -0.130, 0.075,0.090},
        TurboGaugeLoc = {0.0533, -0.125, 0.045,0.060},
        RotMult = 2.036936,
        RotStep = 2.32833,
        rpmScale = 250
    },
    {
        skinName = 'id7',
        ytdName = 'id7',
        centerCoords = {0.8,0.8},
        lightsLoc = {0.015,0.12,0.018,0.02},
        blinkerLoc = {0.04,0.12,0.022,0.03},
        fuelLoc = {-0.005,0.12,0.012,0.025},
        oilLoc = {0.100,0.12,0.020,0.025},
        engineLoc = {0.130,0.12,0.020,0.025},
        SpeedoBGLoc = {0.115, 0.012, 0.17,0.28},
        SpeedoNeedleLoc = {0.000,5,0.076,0.15},
        TachoBGloc = {0.108,0.009,0.135,0.235},
        TachoNeedleLoc = {0.108,0.009,0.135,0.215},
        FuelBGLoc = {0.085, 0.020,0.030, 0.020},
        FuelGaugeLoc = {0.060,0.000,0.030,0.080},
        GearLoc = {0.115,0.043,0.025,0.055},   -- gear location
        Speed1Loc = {0.090,-0.020,0.022,0.05}, -- 3rd digit
        Speed2Loc = {0.106,-0.020,0.022,0.05}, -- 2nd digit
        Speed3Loc = {0.126,-0.020,0.022,0.05}, -- 1st digit
        UnitLoc = {0.145,-0.000,0.020,0.020},
        RevLight = {0.1054,-0.005,0.138,0.230},
        RotMult = 2.036936,
        RotStep = 2.32833,
        rpmScale = 250,
        rpmScaleDecrease = 60
    },
}