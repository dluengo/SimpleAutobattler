using UnityEngine;
using System;

[Serializable]
public class CoinBag
{
    // --- Members ---
    [SerializeField] protected int m_coinAmount = 0;
    public int coinAmount
    {
        get => m_coinAmount;
        set {
            int clampedValue = Math.Clamp(value, 0, m_maxCoins);
            if (clampedValue != m_coinAmount) {
                m_coinAmount = clampedValue;
                OnCoinAmountChanged?.Invoke();
            }
        }
    }
    [SerializeField] protected int m_maxCoins = 100;
    public int maxCoins
    {
        get => m_maxCoins;
        set {
            if (value != m_maxCoins) {
                m_maxCoins = value;

                // Here OnCoinAmountChanged may be invoked.
                coinAmount = Math.Clamp(coinAmount, 0, m_maxCoins);
                OnMaxCoinsChanged?.Invoke();
            }
        }
    }


    // --- Events ---
    public event Action OnCoinAmountChanged;
    public event Action OnMaxCoinsChanged;


    // --- Methods ---
    public CoinBag(int initialCoins = 0, int maxCoins = 100)
    {
        m_maxCoins = maxCoins;
        m_coinAmount = Mathf.Clamp(initialCoins, 0, m_maxCoins);
    }
}
