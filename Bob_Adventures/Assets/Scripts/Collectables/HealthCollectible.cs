using UnityEngine;

public class HealthCollectible : Collectable
{
    [SerializeField] private float healthValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        Do(() => { 
            collision.GetComponent<Health>().AddHealth(healthValue); 
        });
    }
}
