using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject[] characterPrefabs;
    public Transform spawnPoint;

    void Start()
    {
        int selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter", 0);

        GameObject player = Instantiate(
            characterPrefabs[selectedCharacter],
            spawnPoint.position,
            Quaternion.identity
        );

        Camera.main.GetComponent<CameraFollow>()
            .SetTarget(player.transform);
    }
}