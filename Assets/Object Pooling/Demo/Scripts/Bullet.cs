using UnityEngine;
using GDTools.ObjectPooling;

public class Bullet : PoolObject
{
    public Rigidbody2D rb2D;

    public override void Despawn()
    {
        base.Despawn();

        Debug.Log("Cleaned up!");

        // Other cleanups
        rb2D.velocity = Vector2.zero;
    }
}
