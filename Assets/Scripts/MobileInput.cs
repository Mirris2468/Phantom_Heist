using UnityEngine;

public class MobileInput : MonoBehaviour
{
    public static MobileInput Instance;

    [SerializeField] private FixedJoystick joystick;

    public Vector2 Move => joystick != null
        ? new Vector2(joystick.Horizontal, joystick.Vertical)
        : Vector2.zero;

    private void Awake()
    {
        Instance = this;
    }
}