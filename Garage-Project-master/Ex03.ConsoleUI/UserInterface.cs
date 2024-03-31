using System.Collections.Generic;
using System.Text;
using System;
using System.Linq;

namespace Ex03.ConsoleUI
{
    public class UserInterface
    {
        private GarageManager m_GarageManager = new GarageManager();

        private bool isContinue()
        {
            bool isValid;
            bool result;

            Console.WriteLine("Do you want to continue? Press 'True' or 'False':");
            do
            {
                string userInput = Console.ReadLine().ToLower();
                isValid = bool.TryParse(userInput, out result);

                if (!isValid)
                {
                    Console.WriteLine("Your input is not valid. Please enter 'True' or 'False':");
                }
            } while (!isValid);

            return result;
        }

        public void Start()
        {
            string menuMsg, userInput;
            int userInputInt;

            menuMsg = string.Format(
@"Please select one of the following options:   
    1.Add a new vehicle to the garage             
    2.View the list of vehicles in the garage     
    3.Modify vehicle condition                    
    4.Inflate wheels to the Maximum air pressure           
    5.Refuel vehicle                             
    6.Recharge energy source                        
    7.Display vehicle information");
            bool isContinueInTheGarage = true;

            while (isContinueInTheGarage)
            {
                Console.Clear();
                Console.WriteLine(menuMsg);
                userInput = Console.ReadLine();

                try
                {
                    LogicUI.ValidateMenu(userInput, out userInputInt);

                    switch (userInputInt)
                    {
                        case 1:
                            addNewCustomerToGarage();
                            break;

                        case 2:
                            showAllVehiclesInGarageByFilter();
                            break;

                        case 3:
                            changeCarStatus();
                            break;

                        case 4:
                            inflateWheelsToMax();
                            break;

                        case 5:
                            refuelVechile();
                            break;

                        case 6:
                            loadEngery();
                            break;

                        case 7:
                            printVehicleDetalis();
                            break;
                        
                        default:
                            Console.WriteLine("Invalid input"); 
                            break;
                    }
                }
                catch (FormatException formatException)
                {
                    Console.WriteLine(formatException.Message);
                }
                catch (ValueOutOfRangeException valueOutOfRangeException)
                {
                    Console.WriteLine(valueOutOfRangeException.Message);
                }

                if(!isContinue())
                {
                    isContinueInTheGarage = false;
                }
            }
        }

        private void addNewCustomerToGarage()
        {
            Vehicle vehicle = createAllVehicles();
            if (vehicle != null) 
            {
                CustomerData newCustomerData = new CustomerData();
                newCustomerData.OwnerInfo = createOwner();
                newCustomerData.Vehicle = vehicle;
                m_GarageManager.CustomersData.Add(vehicle.LicenseNumber, newCustomerData);
                Console.WriteLine(createSuccessfulMsg());
            }
        }

        private List<string> createVehicleIfNotExist()
        {
            List<string> myList = Vehicle.GetBasicParams();
            List<string> vehicleParamsList = new List<string>();

            foreach(string detail in myList)
            {
                string userInput = readStringParams(detail);
                if (detail == "license number" && m_GarageManager.IsVehicleExist(userInput))
                {
                    Console.WriteLine("This vehicle already exists in the garage");
                    m_GarageManager.ChangeCarStatus(userInput);
                    break;
                }

                vehicleParamsList.Add(userInput);
            }

            return vehicleParamsList;
        }

        private Vehicle createAllVehicles()
        {
            List<string> vehicleParamsList = createVehicleIfNotExist();
            Vehicle vehicle = null;

            if (vehicleParamsList.Count > 0)
            {
                eVehicleType type = readVehicleType();
                vehicle = Factory.CreateVehicle(type, vehicleParamsList[0],
                                            vehicleParamsList[1], vehicleParamsList[2]);
                List<string> floatListParams = vehicle.GetFloatParams();

                vehicle.UpdateAllWheelsPressure(readFloatParams(vehicle, floatListParams[0]));
                vehicle.EnergySource.CurrentEnergy = readFloatParams(vehicle, floatListParams[1]);
                vehicle.CulcPerecentageOfEnergyLeft();
                List<string> details = vehicle.GetParams();
                List<string> userInputDetails = new List<string>();
                List<bool> isParamsValid = new List<bool> { false, false };
                do
                {
                    try
                    {
                        for (int item = 0; item < details.Count; item++)
                        {
                            if (isParamsValid[item] == false) 
                            {
                                Console.WriteLine("Please enter " + details[item]);
                                string userInput = Console.ReadLine();
                                if(vehicle.IsDetailsValid(userInput, item))
                                {
                                    userInputDetails.Add(userInput);
                                    isParamsValid[item] = true;
                                }
                            }
                        }
                    }
                    catch (FormatException formatException)
                    {
                        Console.WriteLine(formatException.Message);
                    }
                    catch (ValueOutOfRangeException valueOutOfRangeException)
                    {
                        Console.WriteLine(valueOutOfRangeException.Message);
                    }
                }
                while (!isParamsValid.All(x => x));
            }

            return vehicle;
        }

        private float readFloatParams(Vehicle i_Vehicle, string i_Param)
        {
            string userInputString;
            float userInputFloat = -1;
            bool isValueValid = false;

            do
            {
                try
                {
                    Console.WriteLine("Please enter " + i_Param);
                    userInputString = Console.ReadLine();
                    if(i_Param.Contains("air pressure"))
                    {
                        Wheel wheel = i_Vehicle.WheelsCollection[0];

                        isValueValid = wheel.ValidateAirPressure(userInputString, out userInputFloat);

                    }
                    else if (i_Param.Contains("energy"))
                    {
                        isValueValid = i_Vehicle.EnergySource.IsValidAmountOfEnergy(userInputString,out userInputFloat);
                    }
                }
                catch (FormatException formatException)
                {
                    Console.WriteLine(formatException.Message);
                }
                catch (ValueOutOfRangeException valueOutOfRangeException)
                {
                    Console.WriteLine(valueOutOfRangeException.Message);
                }
            }
            while (!isValueValid);

            return userInputFloat;
        }

        private OwnerInformation createOwner()
        {
            List<string> ownerParams = OwnerInformation.GetStringParams();

            return new OwnerInformation(readStringParams(ownerParams[0]), readPhoneNumber());
        }

        private void changeCarStatus()
        {
            eVehicleStatusInGarage newStatus;
            string licenseNumber = null;

            try
            {
                m_GarageManager.IsGarageEmpty();
                licenseNumber = getExistedLicenseNumber();
                newStatus = readInputStatus();
                m_GarageManager.ChangeCarStatus(licenseNumber, newStatus);
                Console.WriteLine(createSuccessfulMsg());
            }
            catch (ArgumentException argumentException)
            {
                Console.WriteLine(argumentException.Message);
            }
        }

        private string getExistedLicenseNumber()
        {
            string licenseNumber = null;
            bool isValidLicenseNumber = false;

            do
            {
                try
                {
                    licenseNumber = readStringParams("license number");
                    if (m_GarageManager.IsVehicleExist(licenseNumber))
                    {
                        isValidLicenseNumber = true;
                    }
                    else
                    {
                        Console.WriteLine("Thr vehicle does not exist");
                    }
                }
                catch (ArgumentException argumentException)
                {
                    Console.WriteLine(argumentException.Message);
                }
            }
            while (!isValidLicenseNumber);

            return licenseNumber;
        }

        private void showAllVehiclesInGarageByFilter()
        {
            bool serchFilter, isValid;
            List<string> filterVehicle = new List<string>();

            try
            {
                m_GarageManager.IsGarageEmpty();
                do
                {
                    try
                    {
                        eVehicleStatusInGarage filterBy = readVehicleStatus(out serchFilter);
                        isValid = !serchFilter;
                        filterVehicle = m_GarageManager.FilterVehicleByStatus(filterBy, serchFilter);
                        filterVehicle.ForEach(Console.WriteLine);

                    }
                    catch (ArgumentException argumentExecption)
                    {
                        Console.WriteLine(argumentExecption.Message);
                    }
                }
                while (filterVehicle.Count == 0);
            }
            catch (ArgumentException argumentException)
            {
                Console.WriteLine(argumentException.Message);
            }

        }

        private void inflateWheelsToMax()
        {
            Vehicle vehicle;

            try
            {
                m_GarageManager.IsGarageEmpty();
                vehicle = getVehicle();
                vehicle.InflateWheelsAirPressureToMax();
                Console.WriteLine(createSuccessfulMsg());
            }
            catch (ArgumentException argumentException)
            {
                Console.WriteLine(argumentException.Message);
            }
        }

        private Vehicle getVehicle()
        {
            Vehicle vehicle;
            string licenseNumber;

            Console.WriteLine("Enter vehicle's license number: ");
            licenseNumber = Console.ReadLine();
            if (!m_GarageManager.IsVehicleExist(licenseNumber))
            {
                throw new ArgumentException("The vehicle does not exist in the garage");
            }

            vehicle = m_GarageManager.GetVehicleByLicenseNumber(licenseNumber);

            return vehicle;
        }

        private eVehicleStatusInGarage readInputStatus()
        {
            string userInput;
            eVehicleStatusInGarage VehicleStatus;
            bool valid;
            string msgStatus = OwnerInformation.GetVehicleStatus();

            do
            {
                Console.WriteLine(msgStatus);
                userInput = Console.ReadLine();
                valid = validateStatus(userInput, out VehicleStatus);
            }
            while (!valid);

            return VehicleStatus;
        }

        private bool validateStatus(string i_UserInput, out eVehicleStatusInGarage io_VehicleStatus)
        {
            bool valid = false;
            io_VehicleStatus = eVehicleStatusInGarage.InProgress;

            try
            {
                valid = OwnerInformation.IsVehicleStatusValid(i_UserInput, out io_VehicleStatus);
            }
            catch (FormatException formatException)
            {
                Console.WriteLine(formatException.Message);
            }
            catch (ValueOutOfRangeException valueOutOfRangeException)
            {
                Console.WriteLine(valueOutOfRangeException.Message);
            }

            return valid;
        }

        private eVehicleStatusInGarage readVehicleStatus(out bool i_IsFilterChosen)
        {
            const string k_AllVehicle = "0";
            StringBuilder msg;
            string userInput;
            eVehicleStatusInGarage VehicleStatus = eVehicleStatusInGarage.InProgress;
            bool valid;

            msg = createStatusMsg();
            do
            {
                Console.WriteLine(msg);
                userInput = Console.ReadLine();
                i_IsFilterChosen = !userInput.Equals(k_AllVehicle);
                valid = !i_IsFilterChosen;

                if (i_IsFilterChosen)
                {
                    valid = validateStatus(userInput, out VehicleStatus);
                }
            }
            while (!valid);

            return VehicleStatus;
        }

        private static StringBuilder createStatusMsg()
        {
            StringBuilder msgStatus = new StringBuilder();

            msgStatus.AppendLine("select vehicle status:");
            msgStatus.AppendLine("0-All Vehicle");
            foreach (var enumValue in Enum.GetValues(typeof(eVehicleStatusInGarage)))
            {
                msgStatus.AppendLine(string.Format("{0}-{1}",(int)enumValue, enumValue));
            }

            return msgStatus;
        }

        private eVehicleType readVehicleType()
        {
            string userInput;
            StringBuilder msgType;
            bool validType;
            eVehicleType vehicleType;

            validType = false;
            vehicleType = eVehicleType.Car;
            msgType = Factory.GetVehicleFactoryTypes();
            do
            {
                Console.WriteLine("Enter your vehicle type");
                Console.Write(msgType);
                userInput = Console.ReadLine();

                try
                {
                    validType = Factory.ValidateType(userInput, out vehicleType);
                }
                catch (FormatException formatException)
                {
                    Console.WriteLine(formatException.Message);
                }
                catch (ValueOutOfRangeException valueOutOfRangeException)
                {
                    Console.WriteLine(valueOutOfRangeException.Message);
                }
            }
            while (!validType);

            return vehicleType;
        }

        private string readStringParams(string i_Msg)
        {
            string userInput = null;
            bool isStringEmpty = true;

            do
            {
                try
                {
                    Console.WriteLine("Enter the " + i_Msg);
                    userInput = Console.ReadLine();
                    isStringEmpty = LogicUI.IsStringEmpty(userInput);
                }
                catch (ArgumentException argumentException)
                {
                    Console.WriteLine(argumentException.Message);
                }
            }
            while (isStringEmpty);

            return userInput;
        }

        private float readEnergy(Vehicle i_Vehicle, string i_Msg)
        {
            string userInput;
            float amountToLoad;
            bool validEnergy;

            amountToLoad = 0;
            validEnergy = false;
            do
            {
                Console.WriteLine(i_Msg);
                userInput = Console.ReadLine();
                try
                {
                    validEnergy = i_Vehicle.EnergySource.IsValidAmountOfEnergy(userInput, out amountToLoad);
                }
                catch (FormatException formatException)
                {
                    Console.WriteLine(formatException.Message);
                }
                catch (ValueOutOfRangeException valueOutOfRangeException)
                {
                    Console.WriteLine(valueOutOfRangeException.Message);
                }
            }
            while (!validEnergy);

            return amountToLoad;
        }

        private string readPhoneNumber()
        {
            string userInput;
            bool isNumberValid;

            userInput = null;
            isNumberValid = false;
            do
            {
                try
                {
                    Console.WriteLine("Please enter a 9-10 digit phone number, first digit must be 0");
                    userInput = Console.ReadLine();
                    isNumberValid = OwnerInformation.IsValidPhoneNumber(userInput);
                }
                catch (FormatException formatException)
                {
                    Console.WriteLine(formatException.Message);
                }
                catch (ArgumentException argumentException)
                {
                    Console.WriteLine(argumentException.Message);
                }
            }
            while (!isNumberValid);

            return userInput;
        }

        private string createSuccessfulMsg()
        {
            return "Successfully done";
        }

        private float getAmountToLoad(Vehicle i_Vehicle)
        {
            string msg;
            float amountToLoad;

            msg = string.Format(@"Please enter the amount of energy to load, ranging from 0-{0}",
                i_Vehicle.EnergySource.MaxEnergy - i_Vehicle.EnergySource.CurrentEnergy);
            amountToLoad = readEnergy(i_Vehicle, msg);

            return amountToLoad;
        }

        private Vehicle getVehicleToLoad(out float io_Amount)
        {
            Vehicle vehicle;
            string licensNum;

            licensNum = getExistLicensNum();
            vehicle = m_GarageManager.GetVehicleByLicenseNumber(licensNum);
            if(vehicle.EnergySource is Battery)
            {
                throw new ArgumentException("This vehicle does not use fuel");
            }
            io_Amount = 0;

            try
            {
                vehicle.EnergySource.IsCurrAmountIsMax();
                io_Amount = getAmountToLoad(vehicle);
                Fuel fuelVehicle = vehicle.EnergySource as Fuel;
                readMachFuel(fuelVehicle);
                
            }
            catch (ArgumentException argumentException)
            {
                Console.WriteLine(argumentException.Message);
            }

            return vehicle;
        }

        private Vehicle getElectricVehicleToLoad(out float io_Amount)
        {
            Vehicle vehicle;
            string licensNum;

            licensNum = getExistLicensNum();
            vehicle = m_GarageManager.GetVehicleByLicenseNumber(licensNum);
            if(vehicle.EnergySource is Fuel)
            {
                throw new ArgumentException("This is not an electric vehicle");
            }
            io_Amount = 0;
            try
            {
                vehicle.EnergySource.IsCurrAmountIsMax();
                io_Amount = getAmountToLoad(vehicle);
            }
            catch (ArgumentException argumentException)
            {
                Console.WriteLine(argumentException.Message);
            }

            return vehicle;
        }

        private void printVehicleDetalis()
        {
            string licensNum;
            CustomerData customer;

            try
            {
                m_GarageManager.IsGarageEmpty();
                licensNum = getExistLicensNum();
                customer = m_GarageManager.GetCustomerDataByLicensNumber(licensNum);
                Console.WriteLine(customer.ToString());
            }
            catch (ArgumentException argumentException)
            {
                Console.WriteLine(argumentException.Message);
            }
        }

       private string getExistLicensNum()
        {
            string i_LicensNum;

            do
            {
                i_LicensNum = readStringParams("license number");
                if (!m_GarageManager.IsVehicleExist(i_LicensNum))
                {
                    Console.WriteLine("This vehicle does not exist in the garage");
                }

            } while (!m_GarageManager.IsVehicleExist(i_LicensNum));

            return i_LicensNum;
        }


        private void refuelVechile()
        {
            float amount;

            try
            {
                m_GarageManager.IsGarageEmpty();
                Vehicle vehicle = getVehicleToLoad(out amount);
                vehicle.EnergySource.LoadEnergySource(amount);
                vehicle.CulcPerecentageOfEnergyLeft();
                Console.WriteLine(createSuccessfulMsg());
            }
            catch (ArgumentException argumentException)
            {
                Console.WriteLine(argumentException.Message);
            }
        }

        private void loadEngery()
        {
            float amount;

            try
            {
                m_GarageManager.IsGarageEmpty();
                Vehicle vehicle = getElectricVehicleToLoad(out amount);
                vehicle.EnergySource.LoadEnergySource(amount);
                vehicle.CulcPerecentageOfEnergyLeft();
                Console.WriteLine(createSuccessfulMsg());
            }
            catch (ArgumentException argumentException)
            {
                Console.WriteLine(argumentException.Message);
            }
        }

        eFuelType readMachFuel(Fuel i_FuelVehicle)
        {
            bool isMatch;
            eFuelType fuelType;

            isMatch = false;
            do
            {
                fuelType = readFuelType();
                try
                {
                    isMatch = i_FuelVehicle.IsFuelMatchVehicle(fuelType);
                }
                catch (ArgumentException argumentExecption)
                {
                    Console.WriteLine(argumentExecption.Message);
                }
            }
            while (!isMatch);

            return fuelType;
        }

        private eFuelType readFuelType()
        {
            string userInput;
            eFuelType fuelType;
            StringBuilder msgType;
            bool validType;

            validType = false;
            fuelType = eFuelType.Soler;
            msgType = createFuelMsg();
            do
            {
                Console.Write(msgType);
                userInput = Console.ReadLine();
                try
                {
                    validType = Fuel.ValidteType(userInput, out fuelType);
                }
                catch (FormatException formatException)
                {
                    Console.WriteLine(formatException.Message);
                }
                catch (ValueOutOfRangeException valueOutOfRangeException)
                {
                    Console.WriteLine(valueOutOfRangeException.Message);
                }
            }
            while (!validType);

            return fuelType;
        }

        private StringBuilder createFuelMsg()
        {
            StringBuilder msgType = new StringBuilder();

            msgType.AppendLine("Please enter Fuel type:");
            foreach (var enumValue in Enum.GetValues(typeof(eFuelType)))
            {
                msgType.AppendLine(string.Format("{0}-{1}",(int)enumValue, enumValue));
            }

            return msgType;
        }
    }
}
