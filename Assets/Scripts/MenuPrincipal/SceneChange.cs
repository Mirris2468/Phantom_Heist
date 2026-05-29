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
        // El botón empieza desactivado
        startButton.interactable = false;
    }

    // Se llama al tocar un personaje
    public void SelectCharacter(int characterID)
    {
        selectedCharacter = characterID;

        // Activar botón de iniciar
        startButton.interactable = true;

        Debug.Log("Personaje seleccionado: " + characterID);
    }

    // Se llama al tocar "Iniciar partida"
    public void StartGame()
    {
        // Guardar personaje elegido
        PlayerPrefs.SetInt("SelectedCharacter", selectedCharacter);

        // Esperar y cambiar escena
        Invoke(nameof(LoadScene), 1.5f);
    }

    void LoadScene()
    {
        SceneManager.LoadScene("R-T-N-Menu");
    }
}