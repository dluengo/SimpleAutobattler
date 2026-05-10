using UnityEngine;
using System;

[RequireComponent(typeof(ActorController), typeof(Animator))]
public class ActorMove : MonoBehaviour
{
    // --- Members ---
    [Header("--- Movement Settings ---")]
    [SerializeField] bool m_movementEnabled = true;
    public bool movementEnabled
    {
        get => m_movementEnabled;
        set {
            // BUG: Not called when setting movementEnabled to false in the inspector
            // NOTE: moveDir needs to be set to zero before disabling movement
            if (!value) {
                moveDir = Vector2.zero;
            }

            //Debug.Log($"Setting movementEnabled to {value} for {name}");
            m_movementEnabled = value;
        }
    }
    public bool isMoving => movementEnabled && moveDir != Vector2.zero;

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
            if (!movementEnabled) {
                m_moveDir = Vector2.zero;
                m_actor.animator.SetBool(animParamName, false);
                return;
            }

            // Check if we are stopping movement.
            if (m_moveDir != Vector2.zero && value == Vector2.zero) {
                OnMoveEnd?.Invoke();
            }
            // Check if we are starting movement.
            else if (m_moveDir == Vector2.zero && value != Vector2.zero) {
                OnMoveStart?.Invoke();
            }

            m_moveDir = value.normalized;

            // Deal with the animator.
            if (m_actor != null && m_actor.animator != null) {
                m_actor.animator.SetBool(animParamName, m_moveDir != Vector2.zero);
            }
        }
    }

    protected ActorController m_actor;

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
    }

    protected virtual void OnEnable()
    {
        moveSpeed = m_moveSpeed;
        moveAnimClip = m_moveAnimClip;
        movementEnabled = m_movementEnabled;
    }

    protected virtual void Start()
    {
        m_actor.UpdateAnimClip(m_moveAnimClipName, m_moveAnimClip);
    }

    // NOTE: When called from children, do it at the end of the overridden
    // Update(), so invoking OnMove actually matches with this frame and not
    // the previous one.
    protected virtual void Update()
    {
        // Check every frame if we are moving and if so , invoke the OnMove event.
        if (movementEnabled && isMoving) {
            OnMove?.Invoke();
        }
    }
}
