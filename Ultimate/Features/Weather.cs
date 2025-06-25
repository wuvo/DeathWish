using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ultimate.Features
{
    public enum WeatherType
    {
        None = 1,
        Rain = 2,
        Snow = 3,
        RainWind = 4,
        AutumnLeaves = 5,
        CherryBlossomPetals = 7,
        CherryBlossomPetalsWind = 8,
        BlowingCotten = 9,
        Atoms = 10
    }
    public static class Weather
    {
        public static DateTime NextChange = new DateTime();
        public static uint Intensity;
        public static uint Direction;
        public static uint Appearence;
        private static WeatherType _CurrentWeather;

        public static WeatherType CurrentWeather
        {
            get
            {
                return _CurrentWeather;
            }
            set
            {
                _CurrentWeather = value;

                foreach (Game.Character GC in Game.World.H_Chars.Values)
                    if (GC.Loc.Map == 1002)
                        GC.MyClient.AddSend(Packets.Weather((uint)value, Intensity, Appearence, Direction));
            }
        }
    }
}
