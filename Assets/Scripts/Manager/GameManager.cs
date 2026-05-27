using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Totales")]
    public int totalObjectives;
    public int totalExtras;

    private int currentObjectives;
    private int currentExtras;

    [Header("HUD")]
    public HUDManager hud;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        hud.UpdateObjective(currentObjectives, totalObjectives);
        hud.UpdateExtra(currentExtras, totalExtras);
    }

    public void Collect(CollectibleType type)
    {
        switch (type)
        {
            case CollectibleType.Objective:
                currentObjectives++;
                hud.UpdateObjective(currentObjectives, totalObjectives);
                break;

            case CollectibleType.Extra:
                currentExtras++;
                hud.UpdateExtra(currentExtras, totalExtras);
                break;
        }
    }
}