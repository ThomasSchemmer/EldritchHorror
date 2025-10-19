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

    public Button HumanCattleBtn;
    public Button KowloonCityBtn;
    public Button TentacleMouthBtn;

    public static PlanetInfoPanel Instance;

    private Planet Target;

    public void Show(Planet P)
    {
        Target = P;
        Container.gameObject.SetActive(true);
        ShowCorruptionPanel();
    }

    private void ShowCorruptionPanel()
    {
        if (!Target)
            return;

        bool bIsCorrupted = Target.IsCorrupted();
        CorruptingContainer.gameObject.SetActive(!bIsCorrupted);
        CorruptedContainer.gameObject.SetActive(bIsCorrupted);
        SacrificeRateSlider.gameObject.SetActive(bIsCorrupted);

        var canAffordCattleFarm = PlayerInfo.Instance.BrainMatterKG > HumanCattleFarmBuilding.BasePrice();
        var canAffordKowloon = PlayerInfo.Instance.BrainMatterKG > KowloonCity.BasePrice();
        var canAffordTentacle = PlayerInfo.Instance.BrainMatterKG > TentacleMouth.BasePrice();

        HumanCattleBtn.gameObject.SetActive(canAffordCattleFarm);
        KowloonCityBtn.gameObject.SetActive(canAffordKowloon);
        TentacleMouthBtn.gameObject.SetActive(canAffordTentacle);

        var canAffordAtLeastOne = canAffordCattleFarm || canAffordKowloon || canAffordTentacle;

        BuildingContainer.gameObject.SetActive(bIsCorrupted && canAffordAtLeastOne);
    }

    public void Hide()
    {
        Target = null;
        Container.gameObject.SetActive(false);
        BuildingContainer.gameObject.SetActive(false);
        CorruptingContainer.gameObject.SetActive(false);
        CorruptedContainer.gameObject.SetActive(false);
        SacrificeRateSlider.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!Target)
            return;

        Name.text = Target.Name;
        Population.text = $"{PopulationText} (x{Target.PopulationGrowthRatePercentage}): \n {Target.Population}";
        Sacrifice.text = $"{SacrificeText} (x{Target.SacrificeGainKG}): \n {Target.GetSacrificedPercent()}";
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

    private const string PopulationText = "Population";
    private const string SacrificeText = "Sacrifice Rate";
    private const string ProductionText = "Brainmatter Production:\n";
}
