using TMPro;
using UnityEngine;

public class ResultsUI : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI objectivesText;
    public TextMeshProUGUI extrasText;
    public TextMeshProUGUI suspicionText;
    public TextMeshProUGUI moneyText;

    private void Start()
    {
        DisplayResults();
    }

    private void DisplayResults()
    {
        int minutes = Mathf.FloorToInt(LevelResults.levelTime / 60);
        int seconds = Mathf.FloorToInt(LevelResults.levelTime % 60);

        timeText.text = $"TIEMPO {minutes:00}:{seconds:00}";

        objectivesText.text =
            $"OBJETIVOS {LevelResults.objectivesCollected}/{LevelResults.totalObjectives}";

        extrasText.text =
            $"EXTRAS {LevelResults.extrasCollected}/{LevelResults.totalExtras}";

        suspicionText.text =
            $"ALERTAS {LevelResults.suspicionLevel}/2";

        //  dinero acumulado
        moneyText.text =
            $"DINERO {LevelResults.totalMoney} (+{LevelResults.moneyEarned})";
    }
}