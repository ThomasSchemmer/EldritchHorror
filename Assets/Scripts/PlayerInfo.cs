using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public float BrainMatterKG;

    public void Start()
    {
        Instance = this;
    }

    public static PlayerInfo Instance;
}
