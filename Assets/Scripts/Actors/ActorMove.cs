using UnityEngine;
using System;

[RequireComponent(typeof(ActorController), typeof(Animator))]
public class ActorMove : MonoBehaviour
{
    // --- Members ---
    [Header("--- Movement Settings ---")]
    public bool enableMovement = true;
    public bool isMoving => moveDir != Vector2.zero;

    [SerializeField] protected float m_moveSpeed = 5f;
    public float moveSpeed     {
        get => m_moveSpeed;
        protected set => m_moveSpeed = value;
    }
    [SerializeField] protected AnimationClip m_moveAnimClip;
    public AnimationClip moveAnimClip { get; protected set; }

    public string animParamName { get; protected set; } = "isMoving";

    private Vector2 m_moveDir = Vector2.zero;
    public Vector2 moveDir {
        get => m_moveDir;
        set {
            // Check if we are stopping movement.
            if (m_moveDir != Vector2.zero && value == Vector2.zero) {
                OnMoveEnd?.Invoke();
            }
            // Check if we are starting movement.
            else if (m_moveDir == Vector2.zero && value != Vector2.zero) {
                OnMoveStart?.Invoke();
            }

            m_moveDir = value;

            // Deal with the animator.
            if (m_animator != null) {
                m_animator.SetBool(animParamName, m_moveDir != Vector2.zero);
            }
        }
    }

    protected ActorController m_actor;
    protected Animator m_animator;

    private string m_moveAnimClipName = "Actor-Move";


    // --- Events ---
    public event Action OnMoveStart;
    public event Action OnMove;
    public event Action OnMoveEnd;


    // --- Methods ---
    protected virtual void Awake()
    {
        m_actor = GetComponent<ActorController>();
        if (m_actor == null) {
            Debug.LogError("ActorMove requires an ActorController component.");
        }

        m_animator = GetComponent<Animator>();
        if (m_animator == null) {
            Debug.LogError("ActorMove requires an Animator component.");
        }
    }

    protected virtual void OnEnable()
    {
        moveSpeed = m_moveSpeed;
        moveAnimClip = m_moveAnimClip;
    }

    protected virtual void Start()
    {
        m_actor.UpdateAnimClip(m_moveAnimClipName, m_moveAnimClip);
    }

    protected virtual void Update()
    {
        // Check every frame if we are moving and if so , invoke the OnMove event.
        if (enableMovement && isMoving) {
            OnMove?.Invoke();
        }
    }
}
