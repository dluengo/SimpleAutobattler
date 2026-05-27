using TMPro;
using TMPro.Examples;
using UnityEngine;

public class CoinsUI : UIBase
{
    // --- Members ---
    [SerializeField] TextMeshProUGUI m_coinsText;

    private ActorController m_player;
    private CoinBag m_coinBag;


    // --- Methods ---
    protected virtual void Awake()
    {
        m_player = GameManager.Instance.Player;
        if (m_player != null) {
            m_coinBag = m_player.coinBag;
            Debug.Assert(m_coinBag != null, "CoinBag reference is not found in ActorController.");
        }

        Debug.Assert(m_player != null, "ActorController reference is not found in GameManager.");
        Debug.Assert(m_coinsText != null, "Coins TextMeshProUGUI reference is not assigned.");
    }

    protected virtual void OnEnable()
    {
        // BUG: There's something odd here. This OnEnable() is being called
        // before the Awake() of the player (ActorController). In the project
        // settings -> script execution order, if ActorController is set before
        // UIBase to ensure that the Awake() run first, it doesn't. Weirder,
        // if we set ActorController BEFORE default time and then UIBase, it works.
        // That's the approach we take for now, but...
        if (m_coinBag != null) {
            m_coinBag.OnCoinAmountChanged += UpdateUI;
        }
    }

    protected virtual void OnDisable()
    {
        if (m_coinBag != null) {
            m_coinBag.OnCoinAmountChanged -= UpdateUI;
        }
    }

    public override void UpdateUI()
    {
        if (m_coinBag != null) {
            m_coinsText.text = m_coinBag.coinAmount.ToString();
        }
    }
}
