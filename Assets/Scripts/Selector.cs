using UnityEngine;
using UnityEngine.InputSystem;

public class Selector : MonoBehaviour
{
    private Camera Cam;
    private Planet Selected = null;

    void Start()
    {
        Cam = Camera.main;    
    }

    void Update()
    {
        if (!Mouse.current.leftButton.wasPressedThisFrame && !Mouse.current.rightButton.wasPressedThisFrame)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
            HandleLeftButtonPress();

        if (Mouse.current.rightButton.wasPressedThisFrame)
            HandleRightButtonPress();
    }

    #nullable enable
    T? FilterByTag<T>(string tag) where T: MonoBehaviour
    {
        var Hits = Physics.RaycastAll(Cam.ScreenPointToRay(Mouse.current.position.value));
        if (Hits.Length == 0)
            return null;

        T? Target = null;
        for (int i = 0; i < Hits.Length; i++)
        {
            if (!Hits[i].transform.tag.Equals(tag))
                continue;

            Target = Hits[i].transform.GetComponent<T>();
            break;
        }

        return Target;
    }

    Planet? FilterSelectedPlanet()
    {
        return FilterByTag<Planet>("Planet");
    }

    void HandleLeftButtonPress()
    {
        Planet? Target = FilterSelectedPlanet();
        
        // TODO if deselection is possible, we cannot click buttons /shrug
        // if (Selected)
        // {
        //     Selected.DeSelect();
        //     Selected = null;
        // }

        if (!Target)
            return;

        Selected = Target;
        Selected.Select();
    }

    void HandleRightButtonPress()
    {
        Planet? Target = FilterSelectedPlanet();

        if (!Target)
            return;

        if (!Target.bIsCorrupted)
        {
            Target.bIsCorrupted = true;
        }
    }

    public void ManualCorruptionAttemptOnSelected()
    {
        Selected.AttemptCorruptionManually();
    }
}
