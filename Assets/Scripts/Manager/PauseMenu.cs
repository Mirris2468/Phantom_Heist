using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void OpenPauseMenu()
    {
        if (GameManager.Instance.IsPaused)
            return;

        GameManager.Instance.SetPause(true);
        animator.SetTrigger("Enter");
    }

    public void ClosePauseMenu()
    {
        GameManager.Instance.SetPause(false);
        animator.SetTrigger("Exit");
    }

public void ExitToMainMenu()
{
    GameManager.Instance.SetPause(false);

    SceneManager.LoadScene("MenuPrincipal");
}

}