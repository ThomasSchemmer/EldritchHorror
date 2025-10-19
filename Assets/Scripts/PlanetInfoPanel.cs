using UnityEngine;
using UnityEngine.UI;

public class PlanetInfoPanel : MonoBehaviour
{
    public Transform Container, BuildingContainer;
    public Transform CorruptingContainer, CorruptedContainer;
    public TMPro.TextMeshProUGUI Name;
    public TMPro.TextMeshProUGUI Population;
    public TMPro.TextMeshProUGUI Sacrifice;
    public TMPro.TextMeshProUGUI Production;
    public TMPro.TextMeshProUGUI CurrentCorruption;
    public TMPro.TextMeshProUGUI MaxCorruption;
    public Slider SacrificeRateSlider;
    public Slider CorruptionSlider;

    public static PlanetInfoPanel Instance;

    private Planet Target;

    public void Show(Planet P)
    {
        Target = P;
        Container.gameObject.SetActive(true);
        BuildingContainer.gameObject.SetActive(true);
        ShowCorruptionPanel();
    }

    private void ShowCorruptionPanel()
    {
        if (!Target)
            return;

        bool bIsCorrupted = Target.IsCorrupted();
        CorruptingContainer.gameObject.SetActive(!bIsCorrupted);
        CorruptedContainer.gameObject.SetActive(bIsCorrupted);
    }

    public void Hide()
    {
        Target = null;
        Container.gameObject.SetActive(false);
        BuildingContainer.gameObject.SetActive(false);
        CorruptingContainer.gameObject.SetActive(false);
        CorruptedContainer.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!Target)
            return;

        Name.text = Target.Name;
        Population.text = PopulationText + Target.Population;
        Sacrifice.text = SacrificeText + Target.GetSacrificedPercent();
        Production.text = ProductionText + Target.GetSacrificeProduction();

        int CorruptionProgress = (int)Target.CorruptionProgress;
        int CorruptionTarget = (int)Target.CorruptionMaximum;
        float Percentage = CorruptionProgress / (float)CorruptionTarget;
        CurrentCorruption.text = "" + CorruptionProgress;
        MaxCorruption.text = "" + CorruptionTarget;
        CorruptionSlider.value = Percentage;

        ShowCorruptionPanel();
    }

    private void Start()
    {
        Instance = this;
        Hide();
    }

    private const string PopulationText = "Population:\n";
    private const string SacrificeText = "Sacrifice Rate:\n";
    private const string ProductionText = "Brainmatter Production:\n";
}
