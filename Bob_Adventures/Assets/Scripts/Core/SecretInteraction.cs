using UnityEngine;
using UnityEngine.Tilemaps;

public class SecretInteraction : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponentInParent<TilemapRenderer>().sortingLayerName = "Background";
            GetComponentInParent<Tilemap>().color = new Color(1,1,1,0.75f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponentInParent<TilemapRenderer>().sortingLayerName = "HiddenRoom";
            GetComponentInParent<Tilemap>().color = Color.white;
        }
    }
}
