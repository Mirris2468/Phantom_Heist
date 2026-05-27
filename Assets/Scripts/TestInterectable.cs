using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{
    public CollectibleType collectibleType;

    public void Interact()
    {
        Debug.Log("Objeto recogido: " + gameObject.name);

        GameManager.Instance.Collect(collectibleType);

        Destroy(gameObject);
    }
}