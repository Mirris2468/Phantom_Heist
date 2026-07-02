using UnityEngine;
using UnityEngine.UI;

public class HUDInputBridge : MonoBehaviour
{
    [Header("Run Button")]
    [SerializeField] private Image runButtonImage;
    [SerializeField] private Sprite walkSprite;
    [SerializeField] private Sprite runSprite;

    public void Ability1()
    {
        PlayerMovement.LocalPlayer?.UseAbility1();
    }

    public void Ability2()
    {
        PlayerMovement.LocalPlayer?.UseAbility2();
    }

    public void Interact()
    {
        PlayerMovement.LocalPlayer?.UseInteract();
    }

    public void ToggleRun()
    {
        if (PlayerMovement.LocalPlayer == null)
            return;

        PlayerMovement.LocalPlayer.ToggleRun();

        runButtonImage.sprite = PlayerMovement.LocalPlayer.IsRunning
            ? runSprite
            : walkSprite;
    }
}