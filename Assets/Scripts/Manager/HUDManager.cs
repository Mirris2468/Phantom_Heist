using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public TextMeshProUGUI objectiveText;
    public TextMeshProUGUI extraText;

    public void UpdateObjective(int current, int total)
    {
        objectiveText.text = $"OBJETIVO {current}/{total}";
    }

    public void UpdateExtra(int current, int total)
    {
        extraText.text = $"EXTRA {current}/{total}";
    }
}