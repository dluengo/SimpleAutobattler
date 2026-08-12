using System;
using UnityEngine;

public class DropManager : MonoBehaviour
{
    // --- Singleton ---

    public static DropManager Instance { get; private set; }


    // --- Members ---
    [Header("--- DropManager Settings ---")]
    [SerializeField] float m_dropPositionRadius = 2.5f;
    [SerializeField] GameObject pickUpPrefab;
    [SerializeField] CoinSO goldSO;


    // --- Unity Methods ---

    private void Awake()
    {
        // Implement Singleton pattern
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(gameObject);
    }


    // --- Public Methods ---

    public void HandleDrop(ActorController actor)
    {
        if (actor == null) {
            Debug.LogError("DropManager: Enemy reference is null.");
            return;
        }

        // Shall we drop items?
        if (ShouldDropItems(actor)) {
            DropItems(actor);
        }

        // Shall we drop gold?
        if (ShouldDropGold(actor)) {
            DropGold(actor);
        }
    }

    public void HandleDrop(RoomController room)
    {
        Debug.LogWarning("DropByRoom not implemented yet.");
    }

    public void DropItems(ActorController actor)
    {
        int numDrops = DetermineNumDrops(actor);

        for (int i = 0; i < numDrops; i++) {

            // Determine what item to drop.
            ItemSO itemSO = DetermineItem(actor);

            // Create a new PickUp GameObject, and initialize it with the Item
            // previously determined.
            GameObject pickUpGO = Instantiate(
                pickUpPrefab,
                DetermineDropPosition(actor.transform.position),
                Quaternion.identity);

            PickUpController pickUp = pickUpGO.GetComponent<PickUpController>();
            if (pickUp != null) {

                // Binds the PickUpController to an Item Description (ItemSO),
                // triggering the instantiation of the Item object.
                // NOTE: Item is not a Unity GameObject or MonoBehaviour, but a pure C# class.
                pickUp.Init(itemSO);

                if (pickUp.item == null) {
                    Debug.LogError("DropManager: Failed to create item for drop.");
                    Destroy(pickUpGO);
                }
            }
            else {
                Debug.LogError("DropManager: PickUpController component is missing on the pickUpPrefab.");
                Destroy(pickUpGO);
            }
        }
    }

    public void DropGold(ActorController actor)
    {
        // How much gold should we drop?
        int goldAmount = DetermineGoldAmount(actor);

        if (goldAmount > 0) {

            // Create a new PickUp GameObject.
            GameObject pickUpGO = Instantiate(
                pickUpPrefab,
                DetermineDropPosition(actor.transform.position),
                Quaternion.identity);

            PickUpController pickUp = pickUpGO.GetComponent<PickUpController>();
            if (pickUp != null) {

                // Binds the PickUpController to the gold Item Description (goldSO),
                pickUp.Init(goldSO);

                // Set the amount of gold.
                (pickUp.item as Coin).value = goldAmount;
            }
            else {
                Debug.LogError("DropManager: PickUpController component is missing on the pickUpPrefab.");
                Destroy(pickUpGO);
            }
        }
    }


    // --- Helper Methods for Items ---

    private bool ShouldDropItems(ActorController actor)
    {
        // TODO: Implement logic to determine if items should be dropped
        return true;
    }

    private int DetermineNumDrops(ActorController actor)
    {
        // TODO: Implement logic to determine the number of items to drop
        return 0;
    }

    private Vector3 DetermineDropPosition(Vector3 center)
    {
        return Utils.GetRandomPositionCircle(center, m_dropPositionRadius);
    }

    private ItemSO DetermineItem(ActorController actor)
    {
        // TODO: Implement logic to determine which item to drop based on the actor
        return null;
    }


    // --- Helper Methods for Gold ---

    private bool ShouldDropGold(ActorController actor)
    {
        // TODO: Implement logic to determine if gold should be dropped
        return true;
    }

    private int DetermineGoldAmount(ActorController actor)
    {
        // TODO: Implement logic to determine the amount of gold to drop
        return 5;
    }
}
