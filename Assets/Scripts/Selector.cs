using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

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
        // only execute UI input, swallow input afterwards
        if (TrySelectUIByLayer("UI", out var _))
            return;

        // we need to handle hovering independent of any mouse clicks
        Planet Target = GetSelectedPlanet();
        if (Selected != Target && Selected != null)
        {
            Selected.DeHover();
        }
        if (Target != null)
        {
            Target.Hover();
        }

        bool bIsLeftClick = Mouse.current.leftButton.wasPressedThisFrame;
        bool bIsRightClick = Mouse.current.rightButton.wasPressedThisFrame;
        if (!bIsLeftClick && !bIsRightClick)
            return;

        if (bIsLeftClick)
            HandleLeftButtonPress(Target);

        if (bIsRightClick)
            HandleRightButtonPress(Target);
    }

    #nullable enable
    T? SelectByTag<T>(string tag) where T: MonoBehaviour
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


    bool TrySelectUIByLayer(string Layer, out GameObject? Target) 
    {
        List<RaycastResult> Hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(GetPointerData(), Hits);

        Target = default;
        if (Hits.Count == 0)
            return false;

        for (int i = 0; i < Hits.Count; i++)
        {
            if (Hits[i].gameObject.layer != LayerMask.NameToLayer(Layer))
                continue;

            Target = Hits[i].gameObject;
            return true;
        }
        return false;
    }

    static PointerEventData GetPointerData()
    {
        return new PointerEventData(EventSystem.current)
        {
            position = Mouse.current.position.value
        };
    }

    Planet? GetSelectedPlanet()
    {
        return SelectByTag<Planet>("Planet");
    }

    void HandleLeftButtonPress(Planet Target)
    {
        if (Selected)
        {
            Selected.DeSelect();
            Selected = null;
        }

        if (!Target)
            return;

        Selected = Target;
        Selected.Select();
    }

    void HandleRightButtonPress(Planet Target)
    {
        if (!Target)
            return;

        if (!Target.IsCorrupted())
        {
            Target.CorruptionProgress = Target.CorruptionMaximum;
        }
    }
    
    public void CorruptSelectedPlanet()
    {
        if (!Selected)
            return;

        Selected.AttemptCorruptionManually();
    }
}
