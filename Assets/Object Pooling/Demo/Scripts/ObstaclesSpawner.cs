using System.Collections;
using UnityEngine;

class ObstaclesSpawner : MultipleSpawner
{
    [SerializeField] private float spawnDuration = 3f;

    WaitForSeconds t;


    private void Start()
    {
        t = new WaitForSeconds(spawnDuration);

        StartCoroutine(RandomSpawningBullet());
    }


    private IEnumerator RandomSpawningBullet()
    {
        while (true)
        {
            yield return t;

            var bullet = Spawn(0, new Vector3(Random.Range(-3f, 3f), -3f, 0f),
             Quaternion.identity) as Bullet;

            bullet.rb2D.velocity = Vector2.up * 3f;

          //  Despawn(0, 2f);
        }
    }
}



