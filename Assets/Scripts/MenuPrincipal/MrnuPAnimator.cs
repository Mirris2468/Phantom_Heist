using UnityEngine;

public class MenuPAnimator : MonoBehaviour
{
    public Animator mainMenuAnimator;
    public Animator characterMenuAnimator;
    public Animator optionsMenuAnimator;

    // ===== MAIN -> CHARACTER =====
    public void OpenCharacterMenu()
    {
        mainMenuAnimator.SetTrigger("Exit");
        characterMenuAnimator.SetTrigger("Enter");
    }

    // ===== CHARACTER -> MAIN =====
    public void BackToMainMenu()
    {
        characterMenuAnimator.SetTrigger("Exit");
        mainMenuAnimator.SetTrigger("Enter");
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
}