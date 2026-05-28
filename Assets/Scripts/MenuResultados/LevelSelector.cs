using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelSelector : MonoBehaviour
{
    [Header("Niveles posibles")]
    public List<string> availableLevels = new List<string>();

    [Header("Botones UI")]
    public Button[] levelButtons;
    public TextMeshProUGUI[] buttonTexts;

    private List<string> selectedLevels = new List<string>();

    private void Start()
    {
        GenerateRandomLevels();
        SetupButtons();
    }

    private void GenerateRandomLevels()
    {
        selectedLevels.Clear();

        List<string> pool = new List<string>(availableLevels);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (pool.Count == 0)
                break;

            int index = Random.Range(0, pool.Count);

            selectedLevels.Add(pool[index]);
            pool.RemoveAt(index);
        }
    }

    private void SetupButtons()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i >= selectedLevels.Count)
            {
                levelButtons[i].gameObject.SetActive(false);
                continue;
            }

            string levelName = selectedLevels[i];

            buttonTexts[i].text = levelName;

            int index = i;

            levelButtons[i].onClick.RemoveAllListeners();
            levelButtons[i].onClick.AddListener(() => SelectLevel(index));
        }
    }

    private void SelectLevel(int index)
    {
        string sceneName = selectedLevels[index];
        SceneManager.LoadScene(sceneName);
    }
}