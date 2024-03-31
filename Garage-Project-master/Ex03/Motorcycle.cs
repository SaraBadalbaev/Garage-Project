using System.Collections.Generic;
using System;
using System.Text;

namespace Ex03
{
    public enum eLicenseType
    {
        A1 = 1,
        A2,
        AB,
        B2
    }

    public class Motorcycle : Vehicle
    {
        private const int k_AmountOfWheels = 2;
        private const int k_MaxTirePressure = 29;
        private const float k_MaxAmountOfEnergyElectric = 2.8f;
        private const float k_MaxAmountOfEnergyFuel = 5.8f;
        private const float k_MaxEngineVolumeInCC = 100f;
        private const eFuelType k_FuelType = eFuelType.Octan98;
        private eLicenseType m_LicenseType;
        private int m_EngineVolumeInCC;

        public Motorcycle(
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
                  i_EnergySource == eEnergySource.Fuel ? k_MaxAmountOfEnergyFuel : k_MaxAmountOfEnergyElectric)
        {
            setFuelType(k_FuelType);
        }

        public eLicenseType LicenseType
        {
            get{ return m_LicenseType; }

            set{ m_LicenseType = value; }
        }

        public int EngineVolumeInCC
        {
            get{ return m_EngineVolumeInCC; }

            set{ m_EngineVolumeInCC = value; }
        }

        public override string ToString()
        {
            return string.Format(
@"{0}
License type: {1}
Engine volume in CC: {2}", base.ToString(), m_LicenseType, m_EngineVolumeInCC);
        }

        public override bool IsDetailsValid(string i_Detail, int i_IndexDetail)
        {
            const int k_LicenseTypeIndex = 0;

            bool isDetailValid;
            if(i_IndexDetail == k_LicenseTypeIndex)
            {
                isDetailValid = Enum.TryParse<eLicenseType>(i_Detail, out m_LicenseType);
            }
            else
            {
                isDetailValid = int.TryParse(i_Detail, out m_EngineVolumeInCC);
            }

            if (!isDetailValid) 
            {
                throw new FormatException("Data is not valid");
            }

            if (i_IndexDetail == k_LicenseTypeIndex)
            {
                isDetailValid = Enum.IsDefined(typeof(eLicenseType), m_LicenseType);
                if (!isDetailValid)
                {
                    Array enumOption = Enum.GetValues(typeof(eLicenseType));
                    throw new ValueOutOfRangeException((int)enumOption.GetValue(0), (int)enumOption.GetValue(enumOption.Length - 1));
                }
            }
            else
            {
                isDetailValid = m_EngineVolumeInCC > 0 && m_EngineVolumeInCC <= k_MaxEngineVolumeInCC;
                if(!isDetailValid)
                {
                    throw new ValueOutOfRangeException(0, k_MaxEngineVolumeInCC);
                }
            }

            return isDetailValid;
        }

        public override List<string> GetParams()
        {
            List<string> detalisDictionary = new List<string>();
            StringBuilder licenseTypeMsg = new StringBuilder();

            licenseTypeMsg.AppendLine("license type");
            foreach (var enumValue in Enum.GetValues(typeof(eLicenseType)))
            {
                licenseTypeMsg.AppendLine(string.Format("{0}-{1}", (int)enumValue, enumValue));
            }

            detalisDictionary.Add(licenseTypeMsg.ToString());
            detalisDictionary.Add("engine volume in CC");

            return detalisDictionary;
        }
    }
}
