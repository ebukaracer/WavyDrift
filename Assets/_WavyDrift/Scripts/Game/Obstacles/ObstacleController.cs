using UnityEngine;

class ObstacleController : MonoBehaviour
{
    int playerDistance;

    int playerDistanceIndex = -1;

    Transform player;

    [SerializeField]
    ObstacleBunchSpawner obstacleBunchSpawner;

    private void Start()
    {
        player = PlayerController.Instance.PlayerMovement.transform;
    }

    private void Update()
    {
        playerDistance = (int)player.transform.position.z / obstacleBunchSpawner.NextSpawnPos;

        //Debug.Log($"Player Distance: {playerDistance}, Player Distance Index: {playerDistanceIndex}");

        if (playerDistanceIndex != playerDistance)
        {
            obstacleBunchSpawner.SpawnObstacleBunch();

            playerDistanceIndex = playerDistance;
        }
    }
}
