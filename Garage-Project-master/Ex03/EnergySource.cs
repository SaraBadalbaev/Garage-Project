using System;

namespace Ex03
{
    public abstract class EnergySource
    {
        private float m_CurrentEnergy = 0;
        private float m_MaxEnergy;

        public EnergySource(float i_MaxEnergy)
        {
            m_MaxEnergy = i_MaxEnergy;
        }

        public float CurrentEnergy
        {
            get{ return m_CurrentEnergy; }

            set{ m_CurrentEnergy = value; }
        }

        public float MaxEnergy
        {
            get{ return m_MaxEnergy; }

            set{ m_MaxEnergy = value; }
        }

        public override string ToString()
        {
            return string.Format(
@"Current amount of energy: {0}
Maximum energy capacity: {1}", m_CurrentEnergy, m_MaxEnergy);
        }

        public void LoadEnergySource(float i_AmountToLoad)
        {
            m_CurrentEnergy += i_AmountToLoad;
        }

        public bool IsValidAmountOfEnergy(string i_UserInputAmount, out float i_AmountToLoad)
        {
            bool validAmount = float.TryParse(i_UserInputAmount, out i_AmountToLoad);

            if (!validAmount)
            {
                throw new FormatException("Format is invalid");
            }

            validAmount = i_AmountToLoad >= 0 && i_AmountToLoad + m_CurrentEnergy <= m_MaxEnergy;
            if (!validAmount)
            {
                throw new ValueOutOfRangeException(0, m_MaxEnergy - m_CurrentEnergy);
            }

            return validAmount;
        }

        public bool IsCurrAmountIsMax()
        {
            bool validAmount = m_CurrentEnergy != m_MaxEnergy;

            if (!validAmount)
            {
                throw new ArgumentException("The energy is at maximum capacity");
            }

            return validAmount;
        }
    }
}

