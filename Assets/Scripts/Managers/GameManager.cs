using System;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // --- Singleton Instance ---
    public static GameManager Instance { get; private set; }

    // To create an empty animation clip.
    //[SerializeField] private string animClipName;


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


    // Simple method to trigger an action. For now it is bound to the space key
    // in the input system.
    public void TriggerAction()
    {
        // Press space to kill the player for testing purposes
        //Player.actor.Die();

        // To create an empty animation clip.
        //AnimationClip clip = new AnimationClip();
        //clip.name = animClipName;
        //AnimationCurve curve = AnimationCurve.Linear(0.0F, 1.0F, .0001F, 1.0F); // Unity won’t let me use 0 length, so use a very small length instead
        //EditorCurveBinding binding = EditorCurveBinding.FloatCurve(string.Empty, typeof(UnityEngine.Animator), "ThisIsAnEmptyAnimationClip"); // Just dummy data
        //AnimationUtility.SetEditorCurve(clip, binding, curve);
        //AssetDatabase.CreateAsset(clip, "Assets/" + animClipName + ".anim");
    }
}
