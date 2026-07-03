using TMPro;
using UnityEngine;

public class ResultsUI : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI objectivesText;
    public TextMeshProUGUI extrasText;
    public TextMeshProUGUI suspicionText;
    public TextMeshProUGUI moneyText;

    public TextMeshProUGUI runText;

    private void Start()
    {
        DisplayResults();
    }

    private void Update()
    {
        UpdateMoney();
    }

    private void DisplayResults()
    {
        // =========================
        // NIVEL ACTUAL
        // =========================
        int minutes = Mathf.FloorToInt(LevelResults.levelTime / 60);
        int seconds = Mathf.FloorToInt(LevelResults.levelTime % 60);

        timeText.text = $"TIEMPO {minutes:00}:{seconds:00}";

        objectivesText.text =
            $"OBJETIVOS {LevelResults.objectivesCollected}/{LevelResults.totalObjectives}";

        extrasText.text =
            $"EXTRAS {LevelResults.extrasCollected}/{LevelResults.totalExtras}";

        suspicionText.text =
            $"ALERTAS {LevelResults.suspicionLevel}/2";

        // =========================
        // RUN COMPLETA
        // =========================
        int runMin = Mathf.FloorToInt(LevelResults.runTime / 60);
        int runSec = Mathf.FloorToInt(LevelResults.runTime % 60);
        UpdateMoney();
        runText.text =
            $"{LevelResults.runLevelsPlayed} niveles | {runMin:00}:{runSec:00}";
    }

    private void UpdateMoney()
    {
        if (GameManager.Instance == null)
            return;

        if (LevelResults.moneyEarned > 0)
        {
            moneyText.text =
                $"DINERO {GameManager.Instance.money} (+{LevelResults.moneyEarned})";
        }
        else
        {
            moneyText.text =
                $"DINERO {GameManager.Instance.money}";
        }
    }
}