using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour
{
    // --- Singleton Instance ---
    public static GameMenuManager Instance { get; private set; }

    public string gameSceneName = "GameScene";


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


    // --- Button Handlers ---
    public void OnClickNewGameButton()
    {
        // Change scene to the main game scene here.
        SceneManager.LoadScene(gameSceneName);
    }

    public void OnClickSettingsButton()
    {
        Debug.Log("Settings button clicked.");
    }

    public void OnClickQuitButton()
    {
#if UNITY_EDITOR
        // Stop play mode in the editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Quit the application in a build
        Application.Quit();
#endif
    }
}
