using System.Collections.Generic;
using System;
using System.Text;

namespace Ex03
{
    public class Car : Vehicle
    {
        private const int k_AmountOfWheels = 5;
        private const int k_MaxAirPressure = 30;
        private const float k_MaxAmountOfEnergyElectric = 4.8f;
        private const float k_MaxAmountOfEnergyFuel = 58f;
        private const eFuelType k_FuelType = eFuelType.Octan95;
        private eColor m_Color;
        private eAmountOfDoors m_AmountOfDoors;

        public Car(
            string i_LicenseNumber,
            string i_ModelName,
            string i_WheelManufacturName,
            eEnergySource i_EnergySource)
            : base(
                  i_LicenseNumber,
                  i_ModelName,
                  k_AmountOfWheels,
                  i_WheelManufacturName,
                  k_MaxAirPressure,
                  i_EnergySource,
                  i_EnergySource == eEnergySource.Fuel ? k_MaxAmountOfEnergyFuel : k_MaxAmountOfEnergyElectric)
        {
            setFuelType(k_FuelType);
        }

        public override string ToString()
        {
            return string.Format(
@"{0}
Color: {1}
Amount of doors: {2}",
base.ToString(), m_Color, m_AmountOfDoors);
        }

        public override bool IsDetailsValid(string i_Detail, int i_IndexDetail)
        {
            const int k_ColorIndex = 0;

            bool isDetailValid;
            if(i_IndexDetail == k_ColorIndex)
            {
                isDetailValid = Enum.TryParse<eColor>(i_Detail, out m_Color);
            }
            else
            {
                isDetailValid = Enum.TryParse<eAmountOfDoors>(i_Detail, out m_AmountOfDoors);
            }

            if (!isDetailValid)
            {
                throw new FormatException("Data is not valid");
            }

            if (i_IndexDetail == k_ColorIndex)
            {
                isDetailValid = Enum.IsDefined(typeof(eColor), m_Color);
                if(!isDetailValid)
                {
                    Array enumColorOptions = Enum.GetValues(typeof(eColor));
                    throw new ValueOutOfRangeException((int)enumColorOptions.GetValue(0), (int)enumColorOptions.GetValue(enumColorOptions.Length - 1));
                }
            }
            else
            {
                Array enumOptionDoors = Enum.GetValues(typeof(eAmountOfDoors));
                isDetailValid = Enum.IsDefined(typeof(eAmountOfDoors), m_AmountOfDoors);
                if (!isDetailValid)
                {
                    throw new ValueOutOfRangeException((int)enumOptionDoors.GetValue(0), (int)enumOptionDoors.GetValue(enumOptionDoors.Length - 1));
                }
            }

            return isDetailValid;
        }

        public override List<string> GetParams()
        {
            List<string> detalisDictionary = new List<string>();
            StringBuilder msgColor = new StringBuilder();
            StringBuilder msgDoor = new StringBuilder();

            msgColor.AppendLine("car color");
            foreach (var enumValue in Enum.GetValues(typeof(eColor)))
            {
                msgColor.AppendLine(string.Format("{0}-{1}", (int)enumValue, enumValue));
            }

            detalisDictionary.Add(msgColor.ToString());
            msgDoor.AppendLine("number of doors");
            foreach (var enumValue in Enum.GetValues(typeof(eAmountOfDoors)))
            {
                msgDoor.AppendLine(string.Format("{0}-{1}", (int)enumValue, enumValue));
            }

            detalisDictionary.Add(msgDoor.ToString());

            return detalisDictionary;
        }
    }
}
