using UnityEngine;

public class PlayerSpawnInScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Transform spawnPoint;

    private void Start()
    {
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
        }
    }
}
