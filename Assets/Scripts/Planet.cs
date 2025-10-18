using System.Collections.Concurrent;
using UnityEngine;

public class Planet : MonoBehaviour
{

    public float Speed = 1;
    public float Scale = 5;
    public Transform target;

    public float Angle = 0;

    public bool bIsCorrupted = false;
    public string Name = string.Empty;
    public long Population = 1000000;
    public long MaxPopulation = 2000000;
    public float PopulationGrowthRatePercentage = 1.17f; // % per year (according to wikipedia)
    public float SacrificePercent = 1;
    public float SacrificeGainKG = 1;
    public float MinimalCorruptionAttemptsBeforeCorruption = 10;
    public float ManualCorruptionBrainmatterCost = 200000;

    public float CurrentManualCorruptionAttempts = 0;

    public uint planetsNearby = 0;

    public float CorruptionRadius = 10;

    private Planet[] AllPlanets = null;


    void Start()
    {
        AllPlanets = FindObjectsByType<Planet>(FindObjectsSortMode.None);
    }

    void Update()
    {
        Rotate();

        planetsNearby = GetCorruptedNeighborhoodCount();
    }

    void FixedUpdate()
    {
        Procreate();
        Sacrifice();
    }

    void Procreate()
    {
        long populationGrowth = (long)(PopulationGrowthRatePercentage * Time.fixedDeltaTime * Population / 100.0f);

        Population = ClampPopulationValue(Population + populationGrowth);
    }

    void Sacrifice()
    {
        if (!bIsCorrupted)
            return;

        PlayerInfo.Instance.BrainMatterKG += GetSacrificeProduction();
        Population -= (long)Mathf.CeilToInt(GetSacrificedPopulation());

        Population = ClampPopulationValue(Population);
    }

    private long ClampPopulationValue(long val)
    {
        return Clamp(val, 0, MaxPopulation);
    }

    void Rotate()
    {
        Angle += Time.deltaTime * Speed % 360;
        var position = new Vector3(Mathf.Cos(Angle), 0, Mathf.Sin(Angle)) * Scale;
        this.transform.localPosition = position + target.position;
    }

    public void Select()
    {
        PlanetInfoPanel.Instance.Show(this);
    }

    public void DeSelect()
    {
        PlanetInfoPanel.Instance.Hide();
    }

    public float GetSacrificeProduction()
    {
        return Mathf.CeilToInt(GetSacrificedPopulation() * SacrificeGainKG);
    }

    public float GetSacrificedPopulation()
    {
        if (!bIsCorrupted)
            return 0;

        return Population * SacrificePercent * Time.fixedDeltaTime / 100.0f;
    }

    public bool CorruptionPossible()
    {
        return (GetCorruptedNeighborhoodCount() + CurrentManualCorruptionAttempts) > MinimalCorruptionAttemptsBeforeCorruption;
    }
    
    public bool ManualCorruptionAttemptPossible()
    {
        return PlayerInfo.Instance.BrainMatterKG > ManualCorruptionBrainmatterCost;
    }

    public uint GetCorruptedNeighborhoodCount()
    {
        uint count = 0;
        foreach (Planet p in AllPlanets)
        {
            if (!p.bIsCorrupted) continue;

            if (Vector3.Distance(transform.position, p.transform.position) < CorruptionRadius)
            {
                count += 1;
            }
        }

        return count;
    }

    public void AttemptCorruptionManually()
    {
        if (!ManualCorruptionAttemptPossible())
        {
            // TODO notify missing brainmatter for action
            return;
        }

        PlayerInfo.Instance.BrainMatterKG -= ManualCorruptionBrainmatterCost;
        CurrentManualCorruptionAttempts += 1;

        if (CorruptionPossible())
        {
            bIsCorrupted = true;
        }
    }

    private static long Clamp(long val, long min, long max)
    {
        if (val < min) return min;
        if (val > max) return max;

        return val;
    }

}
