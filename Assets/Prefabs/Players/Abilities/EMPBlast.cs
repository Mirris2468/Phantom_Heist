using UnityEngine;

public class EMPBlast : MonoBehaviour
{
    [SerializeField] private float duration = 5f;

    private void Start()
    {
        Collider2D[] hits =
            Physics2D.OverlapCircleAll(
                transform.position,
                transform.localScale.x * 0.5f
            );

        foreach (Collider2D hit in hits)
        {
            if (!hit.CompareTag("Electronic"))
                continue;

            ElectronicDevice device =
                hit.GetComponent<ElectronicDevice>();

            if (device != null)
            {
                device.DisableTemporarily(duration);
            }
        }

        Destroy(gameObject, 0.2f);
    }
}