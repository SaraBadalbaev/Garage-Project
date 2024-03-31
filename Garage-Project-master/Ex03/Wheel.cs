using System;
using System.Collections.Generic;

namespace Ex03
{
    public class Wheel
    {
        private readonly string r_ManufacturName;
        private float m_CurrentAirPressure = 0;
        private readonly float r_MaxAirPressure;

        public Wheel(string i_ManufacturName, float i_MaxAirPressure)
        {
            r_ManufacturName = i_ManufacturName;
            r_MaxAirPressure = i_MaxAirPressure;
        }

        public Wheel ShalowClone()
        {
            return this.MemberwiseClone() as Wheel;
        }

        public string ManufacturName
        {
            get{ return r_ManufacturName; }
        }

        public float CurrentAirPressure
        {
            get{ return m_CurrentAirPressure; }

            set{ m_CurrentAirPressure = value; }
        }

        public float MaxAirPressure
        {
            get{ return r_MaxAirPressure; }
        }

        public void UpdateAirPressure(float i_AddAirPressure)
        {
            m_CurrentAirPressure += i_AddAirPressure;
        }

        public bool ValidateAirPressure(string i_UserInput, out float i_AirPressure)
        {
            bool validAirPressure;

            validAirPressure = float.TryParse(i_UserInput, out i_AirPressure);
            if (!validAirPressure)
            {
                throw new FormatException("Format is not valid");
            }

            validAirPressure = i_AirPressure >= 0 && m_CurrentAirPressure + i_AirPressure <= r_MaxAirPressure;
            if (!validAirPressure)
            {
                throw new ValueOutOfRangeException(0, r_MaxAirPressure - m_CurrentAirPressure);
            }

            return validAirPressure;
        }

        public override string ToString()
        {
            return string.Format(
@"Manufacturer name: {0}
Current air pressure: {1}
Maximum air pressure: {2}",r_ManufacturName,m_CurrentAirPressure,r_MaxAirPressure);
        }

        public static List<string> GetParams()
        {
            List<string> detalisList = new List<string> { "manufacturer name" };

            return detalisList;
        }
    }
}
