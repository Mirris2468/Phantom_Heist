using System;
using UnityEngine;
using UnityEngine.UI;

public class EquipSkillPanel : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button slot1Button;
    [SerializeField] private Button slot2Button;

    private Action<int> onSlotSelected;

    private void Awake()
    {
        slot1Button.onClick.AddListener(() => SelectSlot(1));
        slot2Button.onClick.AddListener(() => SelectSlot(2));

        gameObject.SetActive(false);
    }

    // =========================
    // OPEN
    // =========================

    public void Show(Action<int> callback)
    {
        onSlotSelected = callback;
        gameObject.SetActive(true);
    }

    // =========================
    // CLOSE
    // =========================

    public void Hide()
    {
        gameObject.SetActive(false);
        onSlotSelected = null;
    }

    // =========================
    // SLOT SELECTED
    // =========================

    private void SelectSlot(int slot)
    {
        onSlotSelected?.Invoke(slot);
        Hide();
    }
}