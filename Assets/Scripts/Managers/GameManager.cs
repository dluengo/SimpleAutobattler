using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // --- Singleton Instance ---
    public static GameManager Instance { get; private set; }


    // --- Members ---
    public PlayerController Player;
    public float ElapsedTime { get; private set; } = 0f;

    private int m_lastElapsedSeconds = 0;


    // --- Events ---
    public event Action OnSecondElapsed;


    // --- Methods ---
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

    private void Update()
    {
        CountTime();
    }

    private void CountTime()
    {
        ElapsedTime += Time.deltaTime;
        int currentSeconds = Mathf.FloorToInt(ElapsedTime);

        if (currentSeconds > m_lastElapsedSeconds) {
            m_lastElapsedSeconds = currentSeconds;
            OnSecondElapsed?.Invoke();
        }
    }

    // Simple method to trigger an action. For now it is bound to the space key
    // in the input system.
    public void TriggerAction()
    {
        // Press space to kill the player for testing purposes
        Player.actor.Die();
    }
}
