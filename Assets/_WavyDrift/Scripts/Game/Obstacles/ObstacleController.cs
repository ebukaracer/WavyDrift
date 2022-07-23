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
        SpawnObstacle();
    }

    private void SpawnObstacle()
    {
        playerDistance = (int)player.transform.position.z / obstacleBunchSpawner.NextSpawnPos;

        if (playerDistanceIndex <= playerDistance)
        {
            obstacleBunchSpawner.PoolObstacleBunch();

            playerDistanceIndex++;
        }
    }
}
