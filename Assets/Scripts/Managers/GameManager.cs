using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // --- Singleton Instance ---
    public static GameManager Instance { get; private set; }


    // --- Members ---
    public PlayerController Player;


    // --- Methods ---
    private void Awake()
    {
        // Implement Singleton pattern
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Simple method to trigger an action. For now it is bound to the space key
    // in the input system.
    public void TriggerAction()
    {
        EnemyManager.Instance.GenerateEnemy();
    }
}
