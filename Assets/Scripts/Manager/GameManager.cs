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

    public int selectedCharacter = -1;
    public int money;

    public bool firstSkillFree = true;

    [Header("Equipped Skills")]
    public SkillData slot1Skill;
    public SkillData slot2Skill;

    // PAUSA DEL JUEGO
    public bool IsPaused { get; private set; }

    // ALERTA GLOBAL DEL NIVEL
    private int suspicionLevel;

    private int totalObjectives;
    private int totalExtras;

    private PlayerSkillManager skillManager;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        selectedCharacter = -1;

        slot1Skill = null;
        slot2Skill = null;
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

        suspicionLevel = 0;

        // Reiniciar pausa
        IsPaused = false;
        Time.timeScale = 1f;

        if (exitDoor != null)
            exitDoor.SetActive(false);

        hud?.UpdateObjective(currentObjectives, totalObjectives);
        hud?.UpdateExtra(currentExtras, totalExtras);
        hud?.HideDetection();

        skillManager = FindFirstObjectByType<PlayerSkillManager>();

        if (skillManager != null)
        {
            skillManager.ResetAllUses();
        }

        if (scene.name == "MainMenu")
        {
            ResetRun();
        }
    }

    // =========================
    // OBJETIVOS
    // =========================

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

    // =========================
    // SOSPECHA GLOBAL
    // =========================

    public void AddSuspicion(int amount)
    {
        suspicionLevel += amount;
    }

    public int GetSuspicionLevel()
    {
        return suspicionLevel;
    }

    // =========================
    // OBJETIVOS COMPLETADOS
    // =========================

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

    // =========================
    // RESULTADOS
    // =========================

    public void SaveResults(bool levelCompleted)
    {
        LevelResults.levelTime = levelTimer;

        LevelResults.objectivesCollected = currentObjectives;
        LevelResults.totalObjectives = totalObjectives;

        LevelResults.extrasCollected = currentExtras;
        LevelResults.totalExtras = totalExtras;

        LevelResults.suspicionLevel = suspicionLevel;

        int levelMoney = 0;

        if (levelCompleted)
        {
            if (currentObjectives >= totalObjectives)
                levelMoney++;

            if (currentExtras >= totalExtras)
                levelMoney++;

            if (suspicionLevel <= 0)
                levelMoney++;
        }

        LevelResults.moneyEarned = levelMoney;

        money += levelMoney;

        LevelResults.totalMoney = money;

        LevelResults.runLevelsPlayed++;
        LevelResults.runTime += levelTimer;
        LevelResults.runMoney = money;
    }

    // =========================
    // PAUSA
    // =========================

    public void SetPause(bool pause)
    {
        IsPaused = pause;
        Time.timeScale = pause ? 0f : 1f;
    }

    // =========================
    // RESET RUN
    // =========================

    private void ResetRun()
    {
        money = 0;

        currentObjectives = 0;
        currentExtras = 0;

        levelTimer = 0f;

        objectiveCompleted = false;

        suspicionLevel = 0;

        firstSkillFree = true;

        slot1Skill = null;
        slot2Skill = null;
    }
}