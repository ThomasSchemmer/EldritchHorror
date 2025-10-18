using UnityEngine;

public class PlanetInfoPanel : MonoBehaviour
{
    public Transform Container;
    public TMPro.TextMeshProUGUI Name;
    public TMPro.TextMeshProUGUI Population;
    public TMPro.TextMeshProUGUI Sacrifice;
    public TMPro.TextMeshProUGUI Production;

    public static PlanetInfoPanel Instance;

    private Planet Target;

    public void Show(Planet P)
    {
        Target = P;
        Container.gameObject.SetActive(true);
    }

    public void Hide()
    {
        Target = null;
        Container.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!Target)
            return;

        Name.text = Target.Name;
        Population.text = PopulationText + Target.Population;
        Sacrifice.text = SacrificeText + Target.SacrificePercent;
        Production.text = ProductionText + Target.GetSacrificeProduction();
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
