namespace Ex03
{
    public class CustomerData
    {
        private OwnerInformation m_OwnerInfo;
        private Vehicle m_Vehicle;

        public OwnerInformation OwnerInfo
        {
            get { return m_OwnerInfo; }
            set { m_OwnerInfo = value; }
        }

        public Vehicle Vehicle
        {
            get { return m_Vehicle; }
            set { m_Vehicle = value; }
        }

        public override string ToString()
        {
            return string.Format(
@"{0}
{1}",m_OwnerInfo.ToString(), m_Vehicle.ToString());
        }
    }
}
