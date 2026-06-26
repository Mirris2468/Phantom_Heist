using UnityEngine;

public class BreakableWall : MonoBehaviour, IBreakable
{
    [Header("Visual")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite brokenSprite;

    private Collider2D col;
    private bool isBroken;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    public void Break()
    {
        if (isBroken) return;

        isBroken = true;

        // 1. quitar colisión
        if (col != null)
            col.enabled = false;

        // 2. cambiar visual (o podrías desactivar el GO)
        if (spriteRenderer != null && brokenSprite != null)
            spriteRenderer.sprite = brokenSprite;
        else
            gameObject.SetActive(false);
    }
}