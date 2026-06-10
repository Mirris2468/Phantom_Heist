using System.Collections;
using UnityEngine;

public class ElectronicDevice : MonoBehaviour
{
    private Behaviour[] behaviours;

    private void Awake()
    {
        behaviours = GetComponents<Behaviour>();
    }

    public void DisableTemporarily(float duration)
    {
        StartCoroutine(DisableRoutine(duration));
    }

    private IEnumerator DisableRoutine(float duration)
    {
        foreach (Behaviour b in behaviours)
        {
            if (b != this)
                b.enabled = false;
        }

        yield return new WaitForSeconds(duration);

        foreach (Behaviour b in behaviours)
        {
            if (b != this)
                b.enabled = true;
        }
    }
}