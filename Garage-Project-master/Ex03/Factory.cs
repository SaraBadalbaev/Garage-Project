using System;
using System.Text;

namespace Ex03
{
    public enum eVehicleType
    {
        Car = 1,
        ElectricCar,
        Motorcycle,
        ElectricMotorcycle,
        Truck,
    }

    public static class Factory
    {
        public static Vehicle CreateVehicle(eVehicleType i_Type, string i_LicenseNumber, 
                                            string i_ModelName, string i_ManufacturName)
        {
            Vehicle result = null;

            switch (i_Type)
            {
                case eVehicleType.Motorcycle:
                    result = new Motorcycle(i_LicenseNumber, i_ModelName, i_ManufacturName, eEnergySource.Fuel);
                    break;

                case eVehicleType.ElectricMotorcycle:
                    result = new Motorcycle(i_LicenseNumber, i_ModelName, i_ManufacturName, eEnergySource.Battery);
                    break;

                case eVehicleType.Car:
                    result = new Car(i_LicenseNumber, i_ModelName, i_ManufacturName, eEnergySource.Fuel);
                    break;

                case eVehicleType.ElectricCar:
                    result = new Car(i_LicenseNumber, i_ModelName, i_ManufacturName, eEnergySource.Battery);
                    break;

                case eVehicleType.Truck:
                    result = new Truck(i_LicenseNumber, i_ModelName, i_ManufacturName, eEnergySource.Fuel);
                    break;
            }

            return result;
        }

        public static bool ValidateType(string i_UserInput, out eVehicleType i_VehicleStatus)
        {
            Array enumOption;
            bool validStatus = Enum.TryParse(i_UserInput, out i_VehicleStatus);

            enumOption = Enum.GetValues(typeof(eVehicleType));
            if (!validStatus)
            {
                throw new FormatException("Vehicle type is not exist");
            }

            validStatus = Enum.IsDefined(typeof(eVehicleType), i_VehicleStatus);
            if (!validStatus)
            {
                throw new ValueOutOfRangeException((int)enumOption.GetValue(0), (int)enumOption.GetValue(enumOption.Length - 1));
            }

            return validStatus;
        }

        public static StringBuilder GetVehicleFactoryTypes()
        {
            string msg;
            StringBuilder msgType = new StringBuilder();

            foreach (var enumValue in Enum.GetValues(typeof(eVehicleType)))
            {
                msg =string.Format(
@"{0}-{1}", (int)enumValue, enumValue);
                msgType.AppendLine(msg);
            }

            return msgType;
        }
    }
}
