using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    [Header("Objetivos")]
    [SerializeField] private TextMeshProUGUI objectiveText;
    [SerializeField] private TextMeshProUGUI extraText;

    [Header("Detección")]
    [SerializeField] private Slider detectionSlider;

    private void Awake()
    {
        Instance = this;

        if (detectionSlider != null)
            detectionSlider.gameObject.SetActive(false);
    }

    // =========================
    // OBJETIVOS
    // =========================
    public void UpdateObjective(int current, int total)
    {
        if (objectiveText != null)
            objectiveText.text = $"Objetivo {current}/{total}";
    }

    public void UpdateExtra(int current, int total)
    {
        if (extraText != null)
            extraText.text = $"Extras {current}/{total}";
    }

    // =========================
    // DETECCIÓN
    // =========================
    public void ShowDetection(float value, float max)
    {
        if (detectionSlider == null)
            return;

        detectionSlider.gameObject.SetActive(true);
        detectionSlider.maxValue = max;
        detectionSlider.value = value;
    }

    public void HideDetection()
    {
        if (detectionSlider == null)
            return;

        detectionSlider.value = 0f;
        detectionSlider.gameObject.SetActive(false);
    }
}