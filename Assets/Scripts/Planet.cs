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
    public long MaxPopulation = 1000000;
    public float PopulationGrowthRatePercentage = 1.17f; // % per year (according to wikipedia)
    public float SacrificePercent = 1;
    public float SacrificeGainKG = 1;

    void Start()
    {
    }

    void Update()
    {
        Rotate();
        Procreate();
        Sacrifice();
    }

    void Procreate()
    {
        long populationGrowth = (long)(PopulationGrowthRatePercentage / 100.0f * Time.deltaTime * Population);

        long populationCandidate = Population + populationGrowth;
        if (populationCandidate > MaxPopulation)
        {
            populationCandidate = MaxPopulation;
        }

        Population = populationCandidate;
    }

    void Sacrifice()
    {
        if (!bIsCorrupted)
            return;

        PlayerInfo.Instance.BrainMatterKG += GetSacrificeProduction();
        Population -= (long)GetSacrificedPopulation();
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
        return GetSacrificedPopulation() * SacrificeGainKG;
    }

    public float GetSacrificedPopulation()
    {
        if (!bIsCorrupted)
            return 0;

        return Population * SacrificePercent / 100.0f * Time.deltaTime;
    }
}
