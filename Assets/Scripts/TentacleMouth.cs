using UnityEngine;

public class TentacleMouth : PlanetEffect
{

    private const float MindMatterFactorIncrease = 1; 

    public override void apply(Planet p)
    {
        p.SacrificeGainKG += MindMatterFactorIncrease;
    }

    public override void reverse(Planet p)
    {
        p.SacrificeGainKG -= MindMatterFactorIncrease;
    }

    public override long GetPrice()
    {
        return BasePrice();
    }
    
    public new static long BasePrice()
    {
        return 3_000_000;
    }
}
