using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{
    public CollectibleType collectibleType;

    public void Interact()
    {
        Debug.Log("Interact ejecutado en: " + gameObject.name);

        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance es NULL");
            return;
        }

        GameManager.Instance.Collect(collectibleType);

        Destroy(gameObject);
    }
}