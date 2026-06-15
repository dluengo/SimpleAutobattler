using UnityEngine;

public class Coin : Item
{
    // --- Members ---
    public int value;


    // --- Methods ---
    public Coin(CoinSO coinSO) : base(coinSO)
    {
        value = coinSO.value;
    }
}
