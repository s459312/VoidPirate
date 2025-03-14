using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestProjectile : MonoBehaviour, IProjectile
{

    //public float damage;
    protected float delay = 1;

    public float Damage { get; set; }

    protected virtual void WaitAndDestroy()
    {
        Destroy(gameObject, delay);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {

        CollisionBehavior(collision);
    }

    public virtual void CollisionBehavior(Collider2D collision)
    {
        if (collision.tag == "Wall" || collision.tag == "Door" || collision.tag == "CollisionTilemap")
        {
            Destroy(gameObject);
        }

        if (collision.tag == "Enemy" || collision.tag == "Boss")
        {
            if (collision.GetComponent<IEnemy>() != null)
            {
                collision.GetComponent<IEnemy>().TakeDamage(Damage);
                Destroy(gameObject);
            }
        }
        else
        {
            WaitAndDestroy();
        }
    }
}
