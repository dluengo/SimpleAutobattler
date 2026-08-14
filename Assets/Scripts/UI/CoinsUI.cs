using TMPro;
using TMPro.Examples;
using UnityEngine;

public class CoinsUI : UIBase
{
    // --- Members ---
    [SerializeField] ActorController m_player;
    [SerializeField] TextMeshProUGUI m_coinsText;

    [SerializeField] CoinBag m_coinBag;


    // --- Methods ---
    protected virtual void Awake()
    {
        Debug.Assert(m_player != null, "ActorController reference is not assigned.");
        Debug.Assert(m_coinsText != null, "Coins TextMeshProUGUI reference is not assigned.");

        if (m_player != null) {
            m_coinBag = m_player.coinBag;
            Debug.Assert(m_coinBag != null, "CoinBag reference is not found in ActorController.");

            //SubscribeEvents();
        }
    }

    protected virtual void OnEnable()
    {
        // BUG: There's something odd here. This OnEnable() is being called
        // before the Awake() of the player (ActorController). In the project
        // settings -> script execution order, if ActorController is set before
        // UIBase to ensure that the Awake() run first, it doesn't. Weirder,
        // if we set ActorController BEFORE default time and then UIBase, it works.
        // That's the approach we take for now, but...
        SubscribeEvents();
    }

    protected virtual void OnDisable()
    {
        UnsubscribeEvents();
    }

    protected override void Start()
    {
        base.Start();

        //if (m_player != null) {
        //    m_coinBag = m_player.coinBag;
        //    Debug.Assert(m_coinBag != null, "CoinBag reference is not found in ActorController.");

        //    SubscribeEvents();
        //}

        UpdateUI();
    }

    public override void UpdateUI()
    {
        if (m_coinBag != null) {
            m_coinsText.text = m_coinBag.coinAmount.ToString();
        }
    }


    // --- Helpers ---
    private void SubscribeEvents()
    {
        if (m_coinBag != null) {
            m_coinBag.OnCoinAmountChanged += UpdateUI;
        }
    }

    private void UnsubscribeEvents()
    {
        if (m_coinBag != null) {
            m_coinBag.OnCoinAmountChanged -= UpdateUI;
        }
    }
}
