using System.Collections;
using UnityEngine;

class BulletsSpawner : Spawner<Bullet>
{

    [SerializeField] 
    private float spawnDelay = .5f;

    WaitForSeconds delay;

    private void Start()
    {
        delay = new WaitForSeconds(spawnDelay);

        StartCoroutine(RandomSpawning());
    }

    private IEnumerator RandomSpawning()
    {
        while (true)
        {
            yield return delay;

            var bullet = Spawn(new Vector3(Random.Range(-3f, 3f), -3f, 0f),
             Quaternion.identity);

            bullet.rb2D.velocity = Vector2.up * 3f;

            Despawn(2f);
        }
    }
}

