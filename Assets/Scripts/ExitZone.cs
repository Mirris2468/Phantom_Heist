using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitZone : MonoBehaviour
{
    [Header("Animación")]
    public Animator animator;

    [Header("Escena a cargar")]
    public string sceneToLoad;

    [Header("Tiempo antes de cargar")]
    public float delayBeforeLoad = 1f;

    private bool activated;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activated)
            return;

        if (other.CompareTag("Player"))
        {
            activated = true;

            if (animator != null)
            {
                animator.SetTrigger("Activate");
            }

            StartCoroutine(LoadScene());
        }
    }

    private IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(delayBeforeLoad);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SaveResults(true);
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}