using UnityEngine;
using System.Collections;

class CirclesSpawner : Spawner
{
    [SerializeField] private float spawnDuration = 3f;

    WaitForSeconds t;


    private void Start()
    {
        t = new WaitForSeconds(spawnDuration);

        StartCoroutine(RandomSpawning());
    }

    private IEnumerator RandomSpawning()
    {
        while (true)
        {
            yield return t;

            Spawn(new Vector3(Random.Range(-3f, 3f), Random.Range(-4f, 4f), 0f),
           Quaternion.identity);

            Despawn(1f);
        }
    }
}
