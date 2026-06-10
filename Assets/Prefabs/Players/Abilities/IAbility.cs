using UnityEngine;

public interface IAbility
{
    void Press();   // click
    void Cancel();  // para cosas como AIM
    bool IsAiming { get; }
}