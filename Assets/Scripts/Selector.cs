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
        if (!Mouse.current.leftButton.wasPressedThisFrame)
            return;

        if (Selected)
        {
            Selected.DeSelect();
            Selected = null;
        }

        var Hits = Physics.RaycastAll(Cam.ScreenPointToRay(Mouse.current.position.value));
        if (Hits.Length == 0)
            return;

        Planet Target = null;
        for (int i = 0; i < Hits.Length; i++)
        {
            if (!Hits[i].transform.tag.Equals("Planet"))
                continue;

            Target = Hits[i].transform.GetComponent<Planet>();
            break;
        }

        if (!Target)
            return;

        Selected = Target;
        Selected.Select();
    }
}
