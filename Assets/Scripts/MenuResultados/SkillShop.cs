using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillShop : MonoBehaviour
{
    [Header("Todas las habilidades")]
    [SerializeField] private List<SkillData> availableSkills = new();

    [Header("Botones UI")]
    [SerializeField] private Button[] skillButtons;
    [SerializeField] private Image[] skillIcons;
    [SerializeField] private TextMeshProUGUI[] skillNames;
    [SerializeField] private Button rerollButton;
    [SerializeField] private int rerollCost = 1;

    [Header("Info Panel")]
    [SerializeField] private SkillInfoPanel infoPanel;

    [Header("Equip Panel")]
    [SerializeField] private EquipSkillPanel equipPanel;

    private readonly List<SkillData> selectedSkills = new();

    private void Start()
    {
        GenerateRandomSkills();
        SetupButtons();

        infoPanel.Hide();

        rerollButton.onClick.AddListener(RerollShop);
    }

    // =========================
    // GENERAR TIENDA
    // =========================

    private void GenerateRandomSkills()
    {
        selectedSkills.Clear();

        List<SkillData> pool = BuildSkillPool();

        int amountToShow = Mathf.Min(2, pool.Count);

        for (int i = 0; i < amountToShow; i++)
        {
            int index = Random.Range(0, pool.Count);

            selectedSkills.Add(pool[index]);
            pool.RemoveAt(index);
        }
    }

    // =========================
    // POOL DE HABILIDADES
    // =========================

    private List<SkillData> BuildSkillPool()
    {
        List<SkillData> pool = new();

        foreach (SkillData skill in availableSkills)
        {
            if (skill == null)
                continue;

            // Más adelante:
            // - Mejoras
            // - Habilidades ya equipadas
            // - Etc.

            pool.Add(skill);
        }

        return pool;
    }

    private void RerollShop()
    {
        GameManager gameManager = GameManager.Instance;

        if (gameManager == null)
            return;

        // Después de la primera compra el reroll cuesta dinero
        if (!gameManager.firstSkillFree)
        {
            if (gameManager.money < rerollCost)
            {
                Debug.Log("No hay suficiente dinero para hacer reroll.");
                return;
            }

            gameManager.money -= rerollCost;
        }

        GenerateRandomSkills();
        SetupButtons();

        infoPanel.Hide();

        Debug.Log("Tienda actualizada.");
    }

    // =========================
    // UI
    // =========================

    private void SetupButtons()
    {
        for (int i = 0; i < skillButtons.Length; i++)
        {
            if (i >= selectedSkills.Count)
            {
                skillButtons[i].gameObject.SetActive(false);
                continue;
            }

            SkillData skill = selectedSkills[i];

            skillButtons[i].gameObject.SetActive(true);

            if (i < skillIcons.Length)
                skillIcons[i].sprite = skill.icon;

            if (i < skillNames.Length)
                skillNames[i].text = skill.skillName;

            int index = i;

            skillButtons[i].onClick.RemoveAllListeners();
            skillButtons[i].onClick.AddListener(() => SelectSkill(index));
        }
    }

    // =========================
    // CLICK
    // =========================

    private void SelectSkill(int index)
    {
        SkillData skill = selectedSkills[index];

        infoPanel.Show(skill);

        equipPanel.Show(slot =>
        {
            TryBuySkill(skill, slot);
        });
    }

    // =========================
    // COMPRA
    // =========================

    private void TryBuySkill(SkillData skill, int slot)
    {
        GameManager gameManager = GameManager.Instance;

        if (gameManager == null)
            return;

        // Primera compra gratis
        if (gameManager.firstSkillFree)
        {
            gameManager.firstSkillFree = false;
        }
        else
        {
            // Comprobar dinero
            if (gameManager.money < skill.cost)
            {
                Debug.Log("No hay suficiente dinero.");
                return;
            }

            // Descontar dinero
            gameManager.money -= skill.cost;
        }

        // Guardar la habilidad en el slot elegido
        if (slot == 1)
        {
            gameManager.slot1Skill = skill;
        }
        else if (slot == 2)
        {
            gameManager.slot2Skill = skill;
        }

        Debug.Log($"{skill.skillName} equipada en el Slot {slot}");
    }
}
