using UnityEngine;

public class KowloonCity : PlanetEffect
{

    private const long MaxPopulationIncrease = 2_000_000; 

    public override void apply(Planet p)
    {
        p.MaxPopulation += MaxPopulationIncrease;
    }

    public override void reverse(Planet p)
    {
        p.MaxPopulation -= MaxPopulationIncrease;
    }

    public override long GetPrice()
    {
        return BasePrice();
    }
    
    public new static long BasePrice()
    {
        return 1_000_000;
    }
}
