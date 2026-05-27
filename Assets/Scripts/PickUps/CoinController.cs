using UnityEngine;

public class CoinController : PickUpController
{
    // --- Members ---
    [Header("--- Coin Settings ---")]
    [SerializeField] int m_coinValue = 1;


    // --- Methods ---
    protected override bool TryPickUp(GameObject picker)
    {
        Debug.Log($"Attempting to pick up coin with value {m_coinValue} by {picker.name}.");
        ActorController actorPicker = picker.GetComponent<ActorController>();
        if (actorPicker != null && actorPicker.coinBag != null) {
            actorPicker.coinBag.coinAmount += m_coinValue;
            return true;
        }

        return false;
    }
}
