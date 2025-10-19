
using UnityEngine;

public abstract class PlanetEffect
{

    public abstract long GetPrice();
    public static long BasePrice()
    {
        return 0;
    }

    /**
        Applies the effect on the given planet
    */
    public abstract void apply(Planet p);

    /** 
        Reverses the effect on the given planet
    */
    public abstract void reverse(Planet p);
}