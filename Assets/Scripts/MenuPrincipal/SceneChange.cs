using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public Button startButton;
    public string nextScene = "GameScene";

    private int selectedCharacter = -1;

    void Start()
    {
        startButton.interactable = false;
    }

    public void SelectCharacter(int characterID)
    {
        selectedCharacter = characterID;
        startButton.interactable = true;
        Debug.Log("Personaje seleccionado: " + characterID);
    }

    public void StartGame()
    {
        PlayerPrefs.SetInt("SelectedCharacter", selectedCharacter);
        Invoke(nameof(LoadScene), 1.5f);
    }

    void LoadScene()
    {
        SceneManager.LoadScene("R-T-N-Menu");
    }

    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}