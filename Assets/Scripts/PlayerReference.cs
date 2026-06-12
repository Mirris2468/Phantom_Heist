using UnityEngine;

public class PlayerReference : MonoBehaviour
{
    public static PlayerMovement Instance;

    private void Awake()
    {
        Instance = GetComponent<PlayerMovement>();
    }
}