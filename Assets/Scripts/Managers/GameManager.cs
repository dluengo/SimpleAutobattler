using System;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // --- Singleton Instance ---
    public static GameManager Instance { get; private set; }


    // --- Members ---
    public PlayerController Player;
    public float ElapsedTime { get; private set; } = 0f;

    [SerializeField] GameObject gameOverScreen;

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

    private void Start()
    {
        // Disable game over screen.
        if (gameOverScreen != null) {
            gameOverScreen.SetActive(false);
        }

        // Subscribe to the enemyPlayer's OnDeath event to trigger the game over screen.
        if (Player != null) {
            Player.actor.OnDeath += EndGame;
        }
        else {
            Debug.LogWarning("GameManager: Player reference is not set. Game over screen will not be triggered on enemyPlayer death.");
        }
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

    // --- Public Methods ---
    // An easy way for other modules to update their animation clips.
    public static bool UpdateAnimClip(
        AnimatorOverrideController animator,
        string animClipName,
        AnimationClip newAnimClip)
    {
        bool ret = false;

        if (animator != null) {

            foreach (var pair in animator.animationClips) {
                if (pair != null && pair.name == animClipName) {

                    animator[pair.name] = newAnimClip;
                    ret = true;
                    break;
                }
            }
        }

        return ret;
    }

    public static void PauseGame(bool pause) {
        Time.timeScale = pause ? 0f : 1f;
    }

    public static void EndGame() {
        Debug.Log("Game Over!");
        PauseGame(true);

        if (Instance.gameOverScreen != null) {
            Instance.gameOverScreen.SetActive(true);
        }
    }


    // --- Button Handlers ---
    public void OnClickRetryButton()
    {
        Debug.Log("Retry button clicked: reloading the current scene.");
        PauseGame(false);
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
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


    // Simple method to trigger an action. For now it is bound to the space key
    // in the input system.
    public void TriggerAction()
    {
        // Press space to kill the enemyPlayer for testing purposes
        Player.actor.Die();

        // To create an empty animation clip.
        //AnimationClip clip = new AnimationClip();
        //clip.name = animClipName;
        //AnimationCurve curve = AnimationCurve.Linear(0.0F, 1.0F, .0001F, 1.0F); // Unity won’t let me use 0 length, so use a very small length instead
        //EditorCurveBinding binding = EditorCurveBinding.FloatCurve(string.Empty, typeof(UnityEngine.Animator), "ThisIsAnEmptyAnimationClip"); // Just dummy data
        //AnimationUtility.SetEditorCurve(clip, binding, curve);
        //AssetDatabase.CreateAsset(clip, "Assets/" + animClipName + ".anim");

        // Set enemyPlayer sprite to 50% transparency for testing purposes
        //Debug.Log("TriggerAction called: setting enemyPlayer sprite to 50% transparency.");
        //SpriteRenderer spriteRenderer = Player.gameObject.GetComponent<SpriteRenderer>();
        //if (spriteRenderer != null) {
        //    Color currColor = spriteRenderer.color;
        //    Color newColor = new Color(currColor.r, currColor.g, currColor.b, 0.5f);
        //    spriteRenderer.color = newColor;
        //}
    }
}
