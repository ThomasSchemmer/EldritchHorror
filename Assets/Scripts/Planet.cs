using System.Collections.Concurrent;
using UnityEngine;

public class Planet : MonoBehaviour
{

    public float Speed = 1;
    public float Scale = 5;
    public Transform target;

    public float Angle = 0;

    public string Name = string.Empty;
    public long Population = 1000000;
    public long MaxPopulation = 2000000;
    public float PopulationGrowthRatePercentage = 1.17f; // % per year (according to wikipedia)
    public float SacrificePercent = 1;
    public float SacrificeGainKG = 1;


    public float ManualCorruptionBrainmatterCost = 200000;
    public float CorruptionProgress = 0;
    public float CorruptionMaximum = 100;
    public float CorruptionOnButtonPress = 10;
    public float CorruptionOnSpread = 1;

    public float CorruptionRadius = 10;

    private Planet[] AllPlanets = null;
    private MeshRenderer HighlightRenderer;
    private bool bIsSelected, bIsHovered;

    void Start()
    {
        CreateHighlight();
        AllPlanets = FindObjectsByType<Planet>(FindObjectsSortMode.None);
    }

    void Update()
    {
        Rotate();

    }

    void FixedUpdate()
    {
        Procreate();
        Sacrifice();
        CorruptNeighbours();
        UpdateVisualization();
    }

    void Procreate()
    {
        long populationGrowth = (long)(PopulationGrowthRatePercentage * Time.fixedDeltaTime * Population / 100.0f);

        Population = ClampPopulationValue(Population + populationGrowth);
    }

    void CorruptNeighbours()
    {
        if (!IsCorrupted())
            return;

        foreach (var Planet in AllPlanets)
        {
            if (Planet.IsCorrupted())
                continue;

            var Distance = Vector3.Distance(transform.position, Planet.transform.position);
            if (Distance >= CorruptionRadius)
                continue;

            // todo: show with tentacles
            Planet.ReceiveCorruption();
        }
    }

    void Sacrifice()
    {
        if (!IsCorrupted())
            return;

        PlayerInfo.Instance.BrainMatterKG += GetSacrificeProduction();
        Population -= Mathf.CeilToInt(GetSacrificedPopulation());

        Population = ClampPopulationValue(Population);
    }

    public void ReceiveCorruption()
    {
        CorruptionProgress += CorruptionOnSpread * Time.fixedDeltaTime;
        CorruptionProgress = Mathf.Clamp(CorruptionProgress, 0, CorruptionMaximum);
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
        bIsSelected = true;
    }

    public void DeSelect()
    {
        PlanetInfoPanel.Instance.Hide();
        bIsSelected = false;
    }

    public void Hover()
    {
        bIsHovered = true;
    }

    public void DeHover()
    {
        bIsHovered = false;
    }

    public float GetSacrificeProduction()
    {
        return Mathf.CeilToInt(GetSacrificedPopulation() * SacrificeGainKG);
    }

    public float GetSacrificedPopulation()
    {
        return Population * GetSacrificedPercent() * Time.fixedDeltaTime / 100.0f;
    }

    public float GetSacrificedPercent()
    {
        if (!IsCorrupted())
            return 0;

        return SacrificePercent;
    }
        
    public bool ManualCorruptionAttemptPossible()
    {
        return PlayerInfo.Instance.BrainMatterKG > ManualCorruptionBrainmatterCost;
    }

    public void AttemptCorruptionManually()
    {
        if (!ManualCorruptionAttemptPossible())
        {
            // TODO notify missing brainmatter for action
            return;
        }

        PlayerInfo.Instance.BrainMatterKG -= ManualCorruptionBrainmatterCost;
        CorruptionProgress += CorruptionOnButtonPress;
        CorruptionProgress = Mathf.Clamp(CorruptionProgress, 0, CorruptionMaximum);
    }

    public bool IsCorrupted()
    {
        return CorruptionProgress >= CorruptionMaximum;
    }

    private static long Clamp(long val, long min, long max)
    {
        if (val < min) return min;
        if (val > max) return max;

        return val;
    }

    private void CreateHighlight()
    {
        if (!tag.Equals("Planet"))
            return;
        
        GameObject Circle = Instantiate(Resources.Load("HighlightCircle") as GameObject);
        Circle.transform.SetParent(transform, false);
        Circle.transform.localPosition = HighlightOffset;
        Circle.transform.localScale = Vector3.one;
        Circle.transform.eulerAngles = new Vector3(-135, 60, 0);
        Circle.name = "HighlighCircle";
        HighlightRenderer = Circle.GetComponent<MeshRenderer>();
        HighlightRenderer.material = new Material(HighlightRenderer.sharedMaterial);

        UpdateVisualization();
    }

    private void UpdateVisualization()
    {
        if (!HighlightRenderer)
            return;

        HighlightRenderer.material.SetInt("_IsCorrupted", IsCorrupted() ? 1 : 0);
        HighlightRenderer.material.SetInt("_IsSelected", bIsSelected ? 1 : 0);
        HighlightRenderer.material.SetInt("_IsHovered", bIsHovered ? 1 : 0);
    }

    private static Vector3 HighlightOffset = new Vector3(0.007f, -0.004f, -0.0081f);

}
