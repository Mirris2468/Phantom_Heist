using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Skill")]
public class SkillData : ScriptableObject
{
    [Header("General")]
    public string skillName;

    [TextArea(3, 5)]
    public string description;

    public Sprite icon;

    [Header("Shop")]
    public int cost = 1;

    [Header("Skill Values")]
    public int cooldown;
    public int uses;

    [Header("Type")]
    public SkillType skillType;
}

public enum SkillType
{
    Punch,
    TrankGun,
    EMPGun,
    Hammer
}