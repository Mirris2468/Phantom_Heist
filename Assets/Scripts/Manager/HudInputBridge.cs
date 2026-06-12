using UnityEngine;

public class HUDInputBridge : MonoBehaviour
{
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
        PlayerMovement.LocalPlayer?.ToggleRun();
    }
}