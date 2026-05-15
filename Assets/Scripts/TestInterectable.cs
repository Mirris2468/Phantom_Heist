using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Objeto recogido: " + gameObject.name);

        Destroy(gameObject);
    }
}