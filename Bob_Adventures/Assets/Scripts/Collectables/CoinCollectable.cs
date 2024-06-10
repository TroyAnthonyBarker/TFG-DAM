using UnityEngine;

public class CoinCollectable : Collectable
{
    [SerializeField] private float coinValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        Do(() =>
        {

        });
    }
}
