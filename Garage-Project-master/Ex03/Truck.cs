using System.Collections.Generic;
using System;

namespace Ex03
{
    public class Truck : Vehicle
    {
        private const int k_AmountOfWheels = 12;
        private const int k_MaxTirePressure = 28;
        private const float k_MaxAmountOfEnergyFuel = 110f;
        private const float k_MaxCargoVolume = 150;
        private const eFuelType k_FuelType = eFuelType.Soler;
        private bool m_IsTransferDangerItems;
        private float m_CargoVolume;

        public Truck(
            string i_LicenseNumber,
            string i_ModelName,
            string i_WheelManufacturName,
            eEnergySource i_EnergySource)
            : base(
                  i_LicenseNumber,
                  i_ModelName,
                  k_AmountOfWheels,
                  i_WheelManufacturName,
                  k_MaxTirePressure,
                  i_EnergySource,
                  k_MaxAmountOfEnergyFuel)
        {
            setFuelType(k_FuelType);
        }

        public bool IsTransferDangerStuff
        {
            get{ return m_IsTransferDangerItems; }

            set{ m_IsTransferDangerItems = value; }
        }

        public float CargoVolume
        {
            get{ return m_CargoVolume; }

            set{ m_CargoVolume = value; }
        }

        public override string ToString()
        {
            return string.Format(
@"{0}
Is transfer danger items: {1}
Cargo volume: {2}", base.ToString(), m_IsTransferDangerItems, m_CargoVolume);
        }

        public override bool IsDetailsValid(string i_Detail, int i_DetailInput)
        {
            const int k_IsTransferDangerItems = 0;
            const int k_IsCargoVolumeIndex = 1; 

            bool isDetaliValid; 
            if(i_DetailInput == k_IsTransferDangerItems)
            {
                isDetaliValid = bool.TryParse(i_Detail, out m_IsTransferDangerItems);
            }
            else 
            {
                isDetaliValid = float.TryParse(i_Detail, out m_CargoVolume);
            }

            if (!isDetaliValid)
            {
                throw new FormatException("Data is not valid");
            }

            if (i_DetailInput == k_IsCargoVolumeIndex)
            {
                isDetaliValid = m_CargoVolume > 0 && m_CargoVolume <= k_MaxCargoVolume;
                if (!isDetaliValid)
                {
                    throw new ValueOutOfRangeException(0, k_MaxCargoVolume);
                }
            }

            return isDetaliValid;
        }

        public override List<string> GetParams()
        {
            List<string> detalisDictionary = new List<string>();

            detalisDictionary.Add("if transfer danger items (True or False)");
            detalisDictionary.Add("cargo volume");

            return detalisDictionary;
        }
    }
}
