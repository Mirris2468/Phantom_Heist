using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int currentObjectives;
    private int currentExtras;

    [Header("HUD")]
    public HUDManager hud;

    [Header("Salida")]
    public GameObject exitDoor;

    private bool objectiveCompleted;
    private float levelTimer;

    public int money;

    // ALERTA GLOBAL DEL NIVEL
    private int suspicionLevel;

    private int totalObjectives;
    private int totalExtras;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        levelTimer += Time.deltaTime;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        hud = FindFirstObjectByType<HUDManager>();

        ExitZone exit = FindFirstObjectByType<ExitZone>();
        if (exit != null)
            exitDoor = exit.gameObject;

        LevelConfig config = FindFirstObjectByType<LevelConfig>();
        if (config != null)
        {
            totalObjectives = config.totalObjectives;
            totalExtras = config.totalExtras;
        }

        currentObjectives = 0;
        currentExtras = 0;
        objectiveCompleted = false;
        levelTimer = 0f;

        // RESET DE ALERTA POR NIVEL
        suspicionLevel = 0;

        if (exitDoor != null)
            exitDoor.SetActive(false);

        if (hud != null)
        {
            hud.UpdateObjective(currentObjectives, totalObjectives);
            hud.UpdateExtra(currentExtras, totalExtras);
        }

        if (scene.name == "MainMenu")
        {
            ResetRun();
        }
    }

    public void Collect(CollectibleType type)
    {
        switch (type)
        {
            case CollectibleType.Objective:
                currentObjectives++;
                hud?.UpdateObjective(currentObjectives, totalObjectives);
                CheckObjectiveCompletion();
                break;

            case CollectibleType.Extra:
                currentExtras++;
                hud?.UpdateExtra(currentExtras, totalExtras);
                break;
        }
    }

    public void AddSuspicion(int amount)
    {
        suspicionLevel += amount;
    }

    private void CheckObjectiveCompletion()
    {
        if (objectiveCompleted)
            return;

        if (currentObjectives >= totalObjectives)
        {
            objectiveCompleted = true;
            ActivateExit();
        }
    }

    private void ActivateExit()
    {
        if (exitDoor != null)
            exitDoor.SetActive(true);
    }

    public void SaveResults()
    {
        LevelResults.levelTime = levelTimer;

        LevelResults.objectivesCollected = currentObjectives;
        LevelResults.totalObjectives = totalObjectives;

        LevelResults.extrasCollected = currentExtras;
        LevelResults.totalExtras = totalExtras;

        LevelResults.suspicionLevel = suspicionLevel;

        int levelMoney = 0;

        if (currentObjectives >= totalObjectives)
            levelMoney++;

        if (currentExtras >= totalExtras)
            levelMoney++;

        if (suspicionLevel <= 0)
            levelMoney++;

        money += levelMoney;

        LevelResults.moneyEarned = levelMoney;
        LevelResults.totalMoney = money;
    }

    private void ResetRun()
    {
        money = 0;
        currentObjectives = 0;
        currentExtras = 0;
        levelTimer = 0f;
        objectiveCompleted = false;
        suspicionLevel = 0;
    }
}