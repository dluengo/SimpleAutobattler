using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AttributesUI : UIBase
{
    // --- Members ---
    public GameObject attrSlotList;

    [SerializeField] protected ActorController m_actor;
    [SerializeField] protected GameObject m_attrSlotPrefab;

    private List<Attribute> m_attrList;


    // --- Methods ---
    private void Awake()
    {
        Debug.Assert(attrSlotList != null, "AttributesUI: attrSlotList is not assigned in the inspector.");
        Debug.Assert(m_actor != null, "AttributesUI: m_actor is not assigned in the inspector.");

        m_attrList = new List<Attribute>();
    }

    protected override void Start()
    {
        base.Start();

        // Create an AttributeSlotUI for each attribute in the AttributesController.
        foreach (Stat stat in m_actor.stats) {
            if (stat is Attribute attr) {
                GameObject attrSlotGO = Instantiate(m_attrSlotPrefab, attrSlotList.transform);
                if (attrSlotGO != null) {

                    // Initialize the AttributeSlotUI.
                    AttributeSlotUI attrSlot = attrSlotGO.GetComponent<AttributeSlotUI>();
                    if (attrSlot != null) {
                        attrSlot.Init(attr, OnPlusButtonClicked, OnMinusButtonClicked);
                    }

                    // Add the attribute to our list for future reference.
                    m_attrList.Add(attr);
                }
            }
        }
    }

    public override void UpdateUI()
    {
        // Nothing to do, AttributeSlotUI will update themselves.
    }


    // --- Buttons Handlers ---
    private void OnPlusButtonClicked()
    {
    }

    private void OnMinusButtonClicked()
    {
    }
}
