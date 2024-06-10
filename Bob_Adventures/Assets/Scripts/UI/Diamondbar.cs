using UnityEngine.UI;
using UnityEngine;

public class DiamondProgressBar : MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private Image totalDiamondbar;
    [SerializeField] private Image currentDiamondbar;

    private void Start()
    {
        totalDiamondbar.fillAmount = 1;
    }

    private void Update()
    {
        currentDiamondbar.fillAmount = playerInventory.diamonds / 3f;
    }
}
