using UnityEngine;

public class MenuPAnimator : MonoBehaviour
{
    public Animator mainMenuAnimator;
    public Animator characterMenuAnimator;
    public Animator optionsMenuAnimator;

    [Header("Imágenes")]
    public GameObject[] characterImages;

    // ===== MAIN -> CHARACTER =====
    public void OpenCharacterMenu()
    {
        Debug.Log("Personaje seleccionado: " + GameManager.Instance.selectedCharacter);

        mainMenuAnimator.SetTrigger("Exit");
        characterMenuAnimator.SetTrigger("Enter");

        MostrarPersonajeSeleccionado();
    }

    // ===== CHARACTER -> MAIN =====
    public void BackToMainMenu()
    {
        characterMenuAnimator.SetTrigger("Exit");
        mainMenuAnimator.SetTrigger("Enter");

        OcultarTodos();
    }

    // ===== MAIN -> OPTIONS =====
    public void OpenOptionsMenu()
    {
        mainMenuAnimator.SetTrigger("Exit");
        optionsMenuAnimator.SetTrigger("Enter");
    }

    // ===== OPTIONS -> MAIN =====
    public void BackFromOptions()
    {
        optionsMenuAnimator.SetTrigger("Exit");
        mainMenuAnimator.SetTrigger("Enter");
    }


    public void SeleccionarPersonaje(int index)
    {
        GameManager.Instance.selectedCharacter = index;
        MostrarPersonajeSeleccionado();
    }

    private void MostrarPersonajeSeleccionado()
    {
        OcultarTodos();

        int index = GameManager.Instance.selectedCharacter;

        if (index >= 0 && index < characterImages.Length)
        {
            characterImages[index].SetActive(true);
        }
    }

    private void OcultarTodos()
    {
        foreach (GameObject img in characterImages)
            img.SetActive(false);
    }
}