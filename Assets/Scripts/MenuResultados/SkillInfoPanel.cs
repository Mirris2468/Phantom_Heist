using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfoPanel : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI skillNameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI improvementText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI usesText;
    [SerializeField] private TextMeshProUGUI cooldownText;

    [Header("Sprite")]
    [SerializeField] private Image iconImage;

    // =========================
    // SHOW
    // =========================

    public void Show(SkillData skill)
    {
        gameObject.SetActive(true);

        skillNameText.text = skill.skillName;
        descriptionText.text = skill.description;
        improvementText.text = skill.mejora;

        iconImage.sprite = skill.icon;

        costText.text = $"Coste: {skill.cost}";
        usesText.text = $"Usos: {skill.uses}";
        cooldownText.text = $"Cooldown: {skill.cooldown}";
    }

    // =========================
    // HIDE
    // =========================

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}