using UnityEngine;

public class HumanCattleFarmBuilding : PlanetEffect
{

    private const float RateIncrease = 1.5f; 

    public override void apply(Planet p)
    {
        p.PopulationGrowthRatePercentage += RateIncrease;
    }

    public override void reverse(Planet p)
    {
        p.PopulationGrowthRatePercentage -= RateIncrease;
    }

    public override long GetPrice()
    {
        return BasePrice();
    }
    
    public new static long BasePrice()
    {
        return 2_000_000;
    }
}
