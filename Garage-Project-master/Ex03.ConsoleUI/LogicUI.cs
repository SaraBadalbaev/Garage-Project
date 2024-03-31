using System;

namespace Ex03.ConsoleUI
{
    public class LogicUI
    {
        public static bool IsStringEmpty(string i_InputVehicleType)
        {
            bool isStringEmpty = i_InputVehicleType.Length == 0;

            if (isStringEmpty)
            {
                throw new ArgumentException("An empty string is not valid");
            }

            return isStringEmpty;
        }

        public static bool ValidateMenu(string i_UserInput, out int i_NumInput)
        {
            const int k_MaxMenuOption = 7;
            bool validInput = int.TryParse(i_UserInput, out i_NumInput);

            if (!validInput)
            {
                throw new FormatException("The format is not valid");
            }

            validInput = i_NumInput > 0 && i_NumInput <= k_MaxMenuOption;
            if (!validInput)
            {
                throw new ValueOutOfRangeException(1, k_MaxMenuOption);
            }

            return validInput;
        }
    }
}
