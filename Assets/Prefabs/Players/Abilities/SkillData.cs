using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Skill")]
public class SkillData : ScriptableObject
{
    [Header("General")]
    public string skillName;

    [TextArea(3, 5)]
    public string description;

    [TextArea(3, 5)]
    public string mejora;

    public Sprite icon;

    [Header("Shop")]
    public int cost = 1;

    [Header("Skill Values")]
    public int cooldown;
    public int upgradedCooldown;
    public int uses;
    public int upgradedUses;

    [Header("Type")]
    public SkillType skillType;
}

public enum SkillType
{
    None,

    Punch,
    TrankGun,
    EMPGun,
    Hammer
}