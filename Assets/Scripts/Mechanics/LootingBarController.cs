using UnityEngine;
using UnityEngine.UI;

public class LootingBarController : MonoBehaviour
{
    [SerializeField]
    private Transform lootingBar;
    public void SetPercentage(float percentage)
    {
        lootingBar.localPosition = new Vector2(0 - percentage * lootingBar.localScale.x, 0);
    }

    public void ResetBar()
    {
        lootingBar.localPosition = Vector2.zero;
    }
}
