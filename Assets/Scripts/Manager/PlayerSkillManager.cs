using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    [Header("Abilities")]
    //[SerializeField] private PunchSkill punchSkill;
    [SerializeField] private TrankGunSkill trankGunSkill;
    [SerializeField] private EMPGunSkill empGunSkill;
    [SerializeField] private HammerSkill hammerSkill;
    [SerializeField] private DistractionSkill distractionSkill;

    private IAbility ability1;
    private IAbility ability2;

    private IResettableAbility reset1;
    private IResettableAbility reset2;

    private void Start()
    {
        EquipSkillsFromGameManager();
    }

    // =========================
    // EQUIP SKILLS
    // =========================

    private void EquipSkillsFromGameManager()
    {
        if (GameManager.Instance == null)
            return;

        SetAbility1(GetAbility(GameManager.Instance.slot1Skill));
        SetAbility2(GetAbility(GameManager.Instance.slot2Skill));
    }

    private IAbility GetAbility(SkillData skillData)
    {
        if (skillData == null)
            return null;

        switch (skillData.skillType)
        {
            //case SkillType.Punch:
            //    return punchSkill;

            case SkillType.TrankGun:
                return trankGunSkill;

            case SkillType.EMPGun:
                return empGunSkill;

            case SkillType.Hammer:
                return hammerSkill;

            case SkillType.Distraction:
                return distractionSkill;

            default:
                return null;
        }
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
    // RESETS
    // =========================

    public void ResetAllUses()
    {
        reset1?.ResetUses();
        reset2?.ResetUses();
    }

    // =========================
    // ABILITY MANAGEMENT
    // =========================

    public void SetAbility1(IAbility ability)
    {
        ability1 = ability;
        reset1 = ability as IResettableAbility;
    }

    public void SetAbility2(IAbility ability)
    {
        ability2 = ability;
        reset2 = ability as IResettableAbility;
    }
}