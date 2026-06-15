using UnityEngine;

[CreateAssetMenu(fileName = "CoinSO", menuName = "Items/CoinSO")]
public class CoinSO : ItemSO
{
    // --- Members ---
    [Header("--- Coin Settings ---")]
    public int value = 1;

    // --- Methods ---
    public override Item CreateNewItem()
    {
        return new Coin(this);
    }
}
