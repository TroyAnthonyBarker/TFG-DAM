using UnityEngine;

public class DiamondCollectable : Collectable
{
    [SerializeField] private int diamondValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        Do(() =>
        {
            collision.GetComponent<PlayerInventory>().AddDiamonds(diamondValue);
        });
    }
}
