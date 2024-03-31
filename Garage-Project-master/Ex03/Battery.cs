namespace Ex03
{
    public class Battery : EnergySource
    {
        public Battery(float i_MaxAmountOfEnergy) : base(i_MaxAmountOfEnergy) { }

        public override string ToString()
        {
            return string.Format(
@"{0}
{1}", eEnergySource.Battery, base.ToString());
        }
    }
}
