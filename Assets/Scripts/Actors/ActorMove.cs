using UnityEngine;
using System;

[RequireComponent(typeof(ActorController), typeof(Animator), typeof(Rigidbody2D))]
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
        get {
            // When someone asks for the moveSpeed, we check if we have a SpeedStat
            // and take it into account.
            //if (m_speedStat != null) {
            //    return m_moveSpeed + m_speedStat.currentValue;
            //}

            return m_moveSpeed;
        }
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
    //protected SpeedStat m_speedStat;
    protected Rigidbody2D m_rb;

    private string m_moveAnimClipName = "Actor-Move";


    // --- Events ---
    public event Action OnMoveStart;
    public event Action OnMove;
    public event Action OnMoveEnd;


    // --- Methods ---
    protected virtual void Awake()
    {
        //m_speedStat = GetComponent<SpeedStat>();

        m_rb = GetComponent<Rigidbody2D>();
        Debug.Assert(m_rb != null, "ActorMove requires a Rigidbody2D component.");

        m_actor = GetComponent<ActorController>();
        Debug.Assert(m_actor != null, "ActorMove requires an ActorController component.");
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

    protected virtual void FixedUpdate()
    {
        // NOTE: We control the final speed using both moveSpeed from this module and
        // m_speedStat.value if it exists throught the getter of moveSpeed.
        if (movementEnabled && moveDir != Vector2.zero) {
            m_rb.MovePosition(
                (Vector2)transform.position + moveDir.normalized * moveSpeed * Time.fixedDeltaTime);
        }
    }
}
