using System;
using System.Collections.Generic;
using System.Text;

namespace Ex03
{
    public enum eVehicleStatusInGarage
    {
        InProgress = 1,
        Completed,
        Paid
    }

    public class OwnerInformation
    {
        private readonly string r_Name;
        private readonly string r_PhoneNumber;
        private eVehicleStatusInGarage m_VehicleStatus = eVehicleStatusInGarage.InProgress;

        public OwnerInformation(string i_Name, string i_PhoneNumber)
        {
            r_Name = i_Name;
            r_PhoneNumber = i_PhoneNumber;
        }

        public eVehicleStatusInGarage VehicleStatus
        {
            get{ return m_VehicleStatus; }

            set{ m_VehicleStatus = value; }
        }

        public override string ToString()
        {
            return string.Format(
@"Owner name: {0}
Owner phone number: {1}
Vehicle status: {2}", r_Name, r_PhoneNumber, m_VehicleStatus);
        }

        public static bool IsVehicleStatusValid(string i_UserInput, out eVehicleStatusInGarage io_VehicleStatus)
        {
            bool isStatusValid = Enum.TryParse(i_UserInput, true, out io_VehicleStatus);

            if (!isStatusValid)
            {
                throw new FormatException("The input vehicle status is not valid");
            }

            isStatusValid = Enum.IsDefined(typeof(eVehicleStatusInGarage), io_VehicleStatus);
            Array enumValues = Enum.GetValues(typeof(eVehicleStatusInGarage));
            int firstValue = (int)enumValues.GetValue(0);
            int lastValue = (int)enumValues.GetValue(enumValues.Length - 1);

            if (!isStatusValid)
            {
                throw new ValueOutOfRangeException(firstValue, lastValue);
            }

            return isStatusValid;
        }

        public static bool IsValidPhoneNumber(string i_PhoneNumber)
        {
            const int k_ValidMobilePhoneLength = 10;
            const int k_ValidLandlineLength = 9;
            bool isPhoneNumValid = (i_PhoneNumber.Length == k_ValidMobilePhoneLength || i_PhoneNumber.Length == k_ValidLandlineLength)
                                    && i_PhoneNumber[0] == '0';

            if (!isPhoneNumValid)
            {
                throw new ArgumentException("A valid phone number should contain 9 or 10 digits and should start with 0");
            }

            isPhoneNumValid = int.TryParse(i_PhoneNumber, out int intPhoneNum);
            if (!isPhoneNumValid)
            {
                throw new FormatException("A valid phone number should only contain digits from 0 to 9");
            }

            return isPhoneNumValid;
        }

        public static List<string> GetStringParams()
        {
            List<string> detalisList = new List<string> { "owner name", "phone number" };

            return detalisList;
        }

        public static string GetVehicleStatus()
        {
            StringBuilder msgStatus = new StringBuilder();

            msgStatus.AppendLine("select vehicle status:");
            foreach (var enumValue in Enum.GetValues(typeof(eVehicleStatusInGarage)))
            {
                msgStatus.AppendLine(string.Format("{0}-{1}", (int)enumValue, enumValue));
            }
            
            return msgStatus.ToString();    
        }
    }
}
