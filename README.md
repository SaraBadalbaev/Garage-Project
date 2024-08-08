# Garage Management System #

## Overview

The Garage Management System is a console-based application designed to manage vehicle information within a garage. The application supports multiple vehicle types such as motorcycles and trucks, and provides functionality to track vehicle status, manage vehicle details, and handle owner information.

## Features

1. **Vehicle Types** : Support for multiple vehicle types, including motorcycles and trucks.
2. **Vehicle Status Tracking**: Ability to track the status of vehicles in the garage (In Progress, Completed, Paid).
3. **Energy Management**: Manage vehicles based on their energy source (fuel or electric).
4. **Wheel Management**: Track and update the air pressure of vehicle wheels.
5. **Owner Information**: Store and validate owner details, including name, phone number, and vehicle status.
6. **Data Validation**: Comprehensive validation for input data, ensuring robustness against invalid or out-of-range values.

### Project Structure

The project is structured around several key classes and enumerations:

## Enumerations

* **eLicenseType**: Represents the different types of motorcycle licenses.

* **eVehicleStatusInGarage**: Enumerates the possible statuses of a vehicle in the garage (In Progress, Completed, Paid).

* **eFuelType**: Represents different types of fuel (e.g., Octan98, Soler).

* **eEnergySource**: Represents the type of energy source a vehicle uses (Fuel, Electric).

## Classes

* **Vehicle**: An abstract base class representing a generic vehicle. It includes properties like license number, model name, wheel collection, and energy source. This class also defines methods for handling wheels and energy levels.

* **Motorcycle**: Inherits from Vehicle. This class includes specific properties and methods related to motorcycles, such as license type and engine volume.

* **Truck**: Inherits from Vehicle. This class handles truck-specific features like cargo volume and whether the truck is transporting dangerous items.

* **Wheel**: Represents a wheel of a vehicle, managing properties such as manufacturer name, current air pressure, and maximum air pressure. The class provides methods for updating and validating air pressure.

* **OwnerInformation**: Manages the information related to the owner of a vehicle, including name, phone number, and vehicle status.
