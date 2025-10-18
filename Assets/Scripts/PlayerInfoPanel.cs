using TMPro;
using UnityEngine;

public class PlayerInfoPanel : MonoBehaviour
{
    public TextMeshProUGUI Text;

    void Update()
    {
        if (PlayerInfo.Instance == null)
            return;

        Text.text = "" + PlayerInfo.Instance.BrainMatterKG;
    }
}
