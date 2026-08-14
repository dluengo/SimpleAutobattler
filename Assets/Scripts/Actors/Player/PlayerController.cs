using UnityEngine;

public class PlayerController : ActorController
{
    // --- Unity Methods ---
    protected override void Awake()
    {
        this.coinBag = new CoinBag();
        this.m_inventory = GetComponent<InventoryController>();

        base.Awake();
    }
}
