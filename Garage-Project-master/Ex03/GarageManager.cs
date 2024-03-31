using System.Collections.Generic;
using System;

namespace Ex03
{
    public class GarageManager
    {
        private readonly Dictionary<string, CustomerData> r_CustomersData = new Dictionary<string, CustomerData>();

        public Dictionary<string, CustomerData> CustomersData
        {
            get{ return r_CustomersData; }
        }

        public bool IsVehicleExist(string i_LicenseNumber)
        {
            return r_CustomersData.ContainsKey(i_LicenseNumber);
        }

        public void ChangeCarStatus(string i_LicenseNumber)
        {
            CustomersData[i_LicenseNumber].OwnerInfo.VehicleStatus = eVehicleStatusInGarage.InProgress;
        }

        public List<string> FilterVehicleByStatus(eVehicleStatusInGarage i_Status, bool i_IsFilterChosen)
        {
            List<string> licenseNumber = new List<string>();

            foreach (CustomerData customerData in r_CustomersData.Values)
            {
                if (!i_IsFilterChosen || customerData.OwnerInfo.VehicleStatus == i_Status)
                {
                    licenseNumber.Add(customerData.Vehicle.LicenseNumber);
                }
            }

            if (licenseNumber.Count == 0)
            {
                throw new ArgumentException("The vechile list is empty");
            }

            return licenseNumber;
        }

        public void ChangeCarStatus(string i_LicenseNumber, eVehicleStatusInGarage i_NewStatus)
        {
            r_CustomersData[i_LicenseNumber].OwnerInfo.VehicleStatus = i_NewStatus;
        }

        public Vehicle GetVehicleByLicenseNumber(string i_LicenseNumber)
        {
            return r_CustomersData[i_LicenseNumber].Vehicle;
        }

        public CustomerData GetCustomerDataByLicensNumber(string i_LicenseNumber)
        {
            return r_CustomersData[i_LicenseNumber];
        }

        public bool IsGarageEmpty()
        {
            bool isEmpty = r_CustomersData.Count == 0;

            if (isEmpty)
            {
                throw new ArgumentException("There are no vehicles in the garage");
            }

            return isEmpty;
        }
    }
}
