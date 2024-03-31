using System;

namespace Ex03
{
    public class ValueOutOfRangeException : Exception
    {
        private float m_MinValue;
        private float m_MaxValue;

        public ValueOutOfRangeException(float i_MinValue, float i_MaxValue)
            : base(string.Format("An error occurd. Value should be between {0} to {1}",
                i_MinValue, i_MaxValue))
        {
            m_MinValue = i_MinValue;
            m_MaxValue = i_MaxValue;
        }

        public float MinValue
        {
            get{ return m_MinValue; }
        }

        public float MaxValue
        {
            get{ return m_MaxValue; }
        }
    }
}
