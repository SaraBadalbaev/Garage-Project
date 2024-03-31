using System.Collections.Generic;

namespace Ex03
{
    public abstract class Vehicle
    {
        private readonly string r_ModelName;
        private readonly string r_LicenseNumber;
        private readonly List<Wheel> r_WheelsCollection;
        protected EnergySource m_EnergySource;
        private float m_RemainingEnergyPrecentage;

        public Vehicle(string i_LicenseNumber, string i_ModelName, int i_AmountOfWheels,
            string i_WheelManufacturName, float i_MaxTirePressure, eEnergySource i_EnergySource,
            float i_MaxAmountOfEnergy)
        {
            Wheel wheel = new Wheel(i_WheelManufacturName, i_MaxTirePressure);
            r_LicenseNumber = i_LicenseNumber;
            r_ModelName = i_ModelName;
            r_WheelsCollection = createWheels(wheel, i_AmountOfWheels);
            setEnergySource(i_EnergySource, i_MaxAmountOfEnergy);
        }

        public string LicenseNumber
        {
            get{ return r_LicenseNumber; }
        }

        public List<Wheel> WheelsCollection
        {
            get{ return r_WheelsCollection; }
        }

        public EnergySource EnergySource
        {
            get{ return m_EnergySource; }
        }

        private void setEnergySource(eEnergySource i_EnergySource, float i_MaxAmountOfEnergy)
        {
            if (i_EnergySource == eEnergySource.Fuel)
            {
                m_EnergySource = new Fuel(i_MaxAmountOfEnergy);
            }
            else
            {
                m_EnergySource = new Battery(i_MaxAmountOfEnergy);
            }
        }

        private List<Wheel> createWheels(Wheel i_Wheel, int i_AmountOfWheels)
        {
            List<Wheel> wheelsList = new List<Wheel>(i_AmountOfWheels);

            for (int wheelIndex = 0; wheelIndex < i_AmountOfWheels; wheelIndex++)
            {
                wheelsList.Add(i_Wheel.ShalowClone());
            }

            return wheelsList;
        }

        protected void setFuelType(eFuelType i_FuelType)
        {
            if (m_EnergySource is Fuel)
            {
                ((Fuel)m_EnergySource).FuelType = i_FuelType;
            }
        }

        public abstract bool IsDetailsValid(string i_Detail, int i_DetailIndex);

        public void InflateWheelsAirPressureToMax() 
        {
            float maxAirPressure;

            maxAirPressure = r_WheelsCollection[0].MaxAirPressure;
            foreach (Wheel wheel in r_WheelsCollection)
            {
                wheel.CurrentAirPressure = maxAirPressure;
            }
        }

        public void UpdateAllWheelsPressure(float i_AddAirPressure)
        {
            foreach (Wheel wheel in r_WheelsCollection)
            {
                wheel.UpdateAirPressure(i_AddAirPressure);
            }
        }

        public override string ToString()
        {
            return string.Format(
@"Model name: {0}
License number: {1}
Remaining energy precentage: {2}%
Energy source: {3}
Wheels collection: {4}", r_ModelName, r_LicenseNumber, m_RemainingEnergyPrecentage,
                m_EnergySource.ToString(), r_WheelsCollection[0].ToString());
        }

        public void CulcPerecentageOfEnergyLeft()
        {
            m_RemainingEnergyPrecentage = (m_EnergySource.CurrentEnergy / m_EnergySource.MaxEnergy) *100;
        }

        public static List<string> GetBasicParams()
        {
            List<string> detalisList = new List<string> { "license number", "model name" };

            detalisList.AddRange(Wheel.GetParams());

            return detalisList;
        }

        public abstract List<string> GetParams();

        public List<string> GetFloatParams()
        {
            List<string> detalisList = new List<string>();
            detalisList.Add("wheels air pressure up to " + (r_WheelsCollection[0].MaxAirPressure - r_WheelsCollection[0].CurrentAirPressure));
            detalisList.Add("how much energy up to " + (m_EnergySource.MaxEnergy - m_EnergySource.CurrentEnergy));
            return detalisList;
        }
    }
}
