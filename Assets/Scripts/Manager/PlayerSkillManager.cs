using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    [Header("Starting Skills")]
    [SerializeField] private MonoBehaviour startingAbility1;
    [SerializeField] private MonoBehaviour startingAbility2;

    private IAbility ability1;
    private IAbility ability2;

    private IResettableAbility reset1;
    private IResettableAbility reset2;

    private void Awake()
    {
        ability1 = startingAbility1 as IAbility;
        ability2 = startingAbility2 as IAbility;

        reset1 = startingAbility1 as IResettableAbility;
        reset2 = startingAbility2 as IResettableAbility;
    }

    // =========================
    // CALLED FROM PLAYER INPUT
    // =========================

    public void UseAbility1()
    {
        ability1?.Press();
    }

    public void UseAbility2()
    {
        if (ability1 != null && ability1.IsAiming)
        {
            ability1.Cancel();
            return;
        }

        ability2?.Press();
    }

    // =========================
    // RESETS (NEW RUN / SCENE)
    // =========================

    public void ResetAllUses()
    {
        reset1?.ResetUses();
        reset2?.ResetUses();
    }

    // =========================
    // ABILITY MANAGEMENT (ROGUELIKE SYSTEM)
    // =========================

    public void SetAbility1(IAbility ability)
    {
        ability1 = ability;

        if (ability is IResettableAbility resettable)
            reset1 = resettable;
    }

    public void SetAbility2(IAbility ability)
    {
        ability2 = ability;

        if (ability is IResettableAbility resettable)
            reset2 = resettable;
    }
}