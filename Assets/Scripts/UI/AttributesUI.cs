using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AttributesUI : UIBase
{
    // --- Members ---
    public GameObject attrSlotList;

    [SerializeField] protected StatsController m_stats;
    [SerializeField] protected GameObject m_attrSlotPrefab;

    // NOTE: Create an array of the number of values of AttributeType.
    // We will map each position to the corresponding AttributeType.
    // i.e. m_attrIcons[0] will be the icon for AttributeType.Strength...
    //[SerializeField] protected Sprite[] m_attrIcons = new Sprite[System.Enum.GetValues(typeof(AttributeType)).Length];


    // --- Methods ---
    private void Awake()
    {
        if (attrSlotList == null) {
            Debug.LogError("AttributesUI: attrSlotList is not assigned in the inspector.");
        }
    }

    protected override void Start()
    {
        base.Start();

        // Create an AttributeSlotUI for each attribute in the AttributesController.
        foreach (Attribute attr in m_stats) {
            GameObject attrSlotGO = Instantiate(m_attrSlotPrefab, attrSlotList.transform);
            if (attrSlotGO != null) {

                // Initialize the AttributeSlotUI.
                AttributeSlotUI attrSlot = attrSlotGO.GetComponent<AttributeSlotUI>();
                if (attrSlot != null) {
                    attrSlot.attr = attr;
                    attrSlot.sprite = attr.icon;
                }
            }
        }
    }


    public override void UpdateUI()
    {
        // Nothing to do, AttributeSlotUI will update themselves.
    }
}
